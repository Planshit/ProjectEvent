using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Data;
using ProjectEvent.UI.Base.Color;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Controls.Input;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Colors = ProjectEvent.UI.Base.Color.Colors;

namespace ProjectEvent.UI.Controls.Action
{
    /// <summary>
    /// Action输入表单控件
    /// </summary>
    public class ActionForm : Control
    {
        #region 依赖属性

        #region 单行输入可视状态
        /// <summary>
        /// 单行输入可视状态
        /// </summary>
        public Visibility LineVisibility
        {
            get { return (Visibility)GetValue(LineVisibilityProperty); }
            set { SetValue(LineVisibilityProperty, value); }
        }
        public static readonly DependencyProperty LineVisibilityProperty =
            DependencyProperty.Register("LineVisibility",
                typeof(Visibility),
                typeof(ActionForm), new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 多行输入可视状态
        /// <summary>
        /// 多行输入可视状态
        /// </summary>
        public Visibility MultiLineVisibility
        {
            get { return (Visibility)GetValue(MultiLineVisibilityProperty); }
            set { SetValue(MultiLineVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MultiLineVisibilityProperty =
            DependencyProperty.Register("MultiLineVisibility",
                typeof(Visibility),
                typeof(ActionForm), new PropertyMetadata(Visibility.Visible));
        #endregion

        #region 单行输入模板组
        /// <summary>
        /// 单行输入模板组
        /// </summary>
        public List<ActionInputModel> LineInputGroups
        {
            get { return (List<ActionInputModel>)GetValue(LineInputGroupsProperty); }
            set { SetValue(LineInputGroupsProperty, value); }
        }
        public static readonly DependencyProperty LineInputGroupsProperty =
            DependencyProperty.Register("LineInputGroups",
                typeof(List<ActionInputModel>),
                typeof(ActionForm), new PropertyMetadata(new PropertyChangedCallback(OnLineInputGroupsChanged)));

        private static void OnLineInputGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionForm;
            if (e.NewValue != e.OldValue)
            {
                control.RenderLineInputGroups();
            }
        }
        #endregion

        #region 多行输入模板组
        /// <summary>
        /// 多行输入模板组
        /// </summary>
        public List<ActionInputModel> MultiLineInputGroups
        {
            get { return (List<ActionInputModel>)GetValue(MultiLineInputGroupsProperty); }
            set { SetValue(MultiLineInputGroupsProperty, value); }
        }
        public static readonly DependencyProperty MultiLineInputGroupsProperty =
            DependencyProperty.Register("MultiLineInputGroups",
                typeof(List<ActionInputModel>),
                typeof(ActionForm), new PropertyMetadata(new PropertyChangedCallback(OnMultiLineInputGroupsChanged)));

        private static void OnMultiLineInputGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionForm;
            if (e.NewValue != e.OldValue)
            {
                control.RenderMultiLineInputGroups();
            }
        }
        #endregion

        #region 多行输入表单展开状态
        /// <summary>
        /// 多行输入表单展开状态
        /// </summary>
        public bool IsExpandMultiForm
        {
            get { return (bool)GetValue(IsExpandMultiFormProperty); }
            set { SetValue(IsExpandMultiFormProperty, value); }
        }
        public static readonly DependencyProperty IsExpandMultiFormProperty =
            DependencyProperty.Register("IsExpandMultiForm",
                typeof(bool),
                typeof(ActionForm), new PropertyMetadata(false, new PropertyChangedCallback(OnIsExpandMultiFormChanged)));

        private static void OnIsExpandMultiFormChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionForm;
            if (control.IsExpandMultiForm)
            {
                control.MultiLineBorder.Visibility = Visibility.Visible;
            }
            else
            {
                control.MultiLineBorder.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region action
        /// <summary>
        /// action
        /// </summary>
        public ActionItemModel Action
        {
            get { return (ActionItemModel)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action",
                typeof(ActionItemModel),
                typeof(ActionForm));
        #endregion

        #region ActionContainer
        /// <summary>
        /// ActionContainer
        /// </summary>
        public ActionContainer ActionContainer
        {
            get { return (ActionContainer)GetValue(ActionContainerProperty); }
            set { SetValue(ActionContainerProperty, value); }
        }
        public static readonly DependencyProperty ActionContainerProperty =
            DependencyProperty.Register("ActionContainer",
                typeof(ActionContainer),
                typeof(ActionForm));
        #endregion
        #endregion

        #region 私有属性
        /// <summary>
        /// 单行输入容器
        /// </summary>
        private WrapPanel LineContainer;
        /// <summary>
        /// 展开选项输入表单按钮
        /// </summary>
        private Button ExpandBtn;
        /// <summary>
        /// 多行输入容器
        /// </summary>
        private StackPanel MultiLineContainer;
        /// <summary>
        /// 多行容器外容器
        /// </summary>
        private Border MultiLineBorder;
        /// <summary>
        /// 变量输入弹出层
        /// </summary>
        private Popup VariablePopup;
        /// <summary>
        /// 操作结果Action选择框
        /// </summary>
        private ComboBox VariableActionComboBox;
        /// <summary>
        /// 选中的操作可选择的返回结果选择框
        /// </summary>
        private ComboBox VariableActionResultsComboBox;
        /// <summary>
        /// 操作结果可选的Action选择框数据
        /// </summary>
        public ObservableCollection<ComBoxModel> VariableActionItems { get; set; }
        /// <summary>
        /// 选中的操作可选择的返回结果选择框数据
        /// </summary>
        public ObservableCollection<ComBoxModel> VariableActionResultsItems { get; set; }
        /// <summary>
        /// 全局变量
        /// </summary>
        private WrapPanel GlobalVariablePanel;
        /// <summary>
        /// 事件变量
        /// </summary>
        private WrapPanel EventVariablePanel;
        private bool IsInVariablePopup = false;

        /// <summary>
        /// 当前键盘焦点输入框
        /// </summary>
        private InputBox KeyboradFocusInputBox;
        /// <summary>
        /// 添加操作结果变量按钮
        /// </summary>
        private Button AddActionResultBtn;
        private List<InputBox> inputBoxes;
        #endregion

        public event EventHandler OnRenderDone;
        public ActionForm()
        {
            DefaultStyleKey = typeof(ActionForm);
            VariableActionItems = new ObservableCollection<ComBoxModel>();
            VariableActionResultsItems = new ObservableCollection<ComBoxModel>();
            inputBoxes = new List<InputBox>();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LineContainer = GetTemplateChild("LineContainer") as WrapPanel;
            ExpandBtn = GetTemplateChild("ExpandBtn") as Button;
            MultiLineContainer = GetTemplateChild("MultiLineContainer") as StackPanel;
            MultiLineBorder = GetTemplateChild("MultiLineBorder") as Border;
            VariablePopup = GetTemplateChild("VariablePopup") as Popup;
            VariableActionComboBox = GetTemplateChild("VariableActionComboBox") as ComboBox;
            VariableActionResultsComboBox = GetTemplateChild("VariableActionResultsComboBox") as ComboBox;
            GlobalVariablePanel = GetTemplateChild("GlobalVariablePanel") as WrapPanel;
            EventVariablePanel = GetTemplateChild("EventVariablePanel") as WrapPanel;
            AddActionResultBtn = GetTemplateChild("AddActionResultBtn") as Button;

            VariablePopup.Opened += (e, c) =>
            {
                IsInVariablePopup = true;
            };
            VariablePopup.MouseLeave += (e, c) =>
            {
                IsInVariablePopup = false;
                VariablePopup.IsOpen = false;
            };

            ExpandBtn.Click += (e, c) =>
            {
                IsExpandMultiForm = !IsExpandMultiForm;
            };
            Render();
            //监听事件更改
            if (ActionContainer != null)
            {
                ActionContainer.EventTypeChanged += ActionContainer_EventTypeChanged;
            }
        }



        #region 渲染输入控件
        private void Render()
        {
            inputBoxes.Clear();
            RenderLineInputGroups();
            RenderMultiLineInputGroups();
            MultiLineContainer.Loaded += (e, c) =>
            {
                MultiLineBorder.Visibility = Visibility.Collapsed;
                OnRenderDone?.Invoke(this, null);
            };
            BindingActionResults();
            UpdateVariableAction();
            if (ActionContainer != null)
            {
                ActionContainer.ItemIndexChanged += ActionContainer_ItemIndexChanged;
            }
            RenderGlobalVariable();
            //添加操作结果变量
            AddActionResultBtn.Click += (e, c) =>
            {
                if (VariableActionComboBox.SelectedItem != null && VariableActionResultsComboBox.SelectedItem != null)
                {
                    var action = VariableActionComboBox.SelectedItem as ComBoxModel;
                    var variable = VariableActionResultsComboBox.SelectedItem as ComBoxModel;
                    KeyboradFocusInputBox.AppendText($"{{{action.ID}.{variable.ID}}}");
                }
            };
            //渲染事件变量
            RenderEventVariable();
            //验证输入
            Valid();
        }


        #endregion

        //事件
        #region action顺序发生变化的时候
        private void ActionContainer_ItemIndexChanged(object sender, EventArgs e)
        {
            UpdateVariableAction();
            Valid();
        }
        #endregion

        #region 事件更改
        private void ActionContainer_EventTypeChanged(object sender, EventArgs e)
        {
            RenderEventVariable();
            Valid();
        }
        #endregion

        //方法
        #region 渲染单行输入组
        private void RenderLineInputGroups()
        {
            if (LineContainer != null)
            {
                LineContainer.Children.Clear();
                if (LineInputGroups != null && LineInputGroups.Count > 0)
                {
                    //有数据
                    foreach (var item in LineInputGroups)
                    {
                        switch (item.InputType)
                        {
                            case Types.InputType.Text:
                                RenderInputBox(item);
                                break;
                            case Types.InputType.Select:
                                RenderSelectBox(item);
                                break;
                        }
                    }
                    LineVisibility = Visibility.Visible;
                }
                else
                {
                    //没有数据
                    LineVisibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region 渲染多行输入组
        private void RenderMultiLineInputGroups()
        {
            if (MultiLineContainer != null)
            {
                MultiLineContainer.Children.Clear();
                if (MultiLineInputGroups != null && MultiLineInputGroups.Count > 0)
                {
                    foreach (var item in MultiLineInputGroups)
                    {
                        switch (item.InputType)
                        {
                            case Types.InputType.Text:
                                RenderInputBox(item, true);
                                break;
                            case Types.InputType.Select:
                                RenderSelectBox(item, true);
                                break;
                            case Types.InputType.CustomKeyValue:
                                RenderKVForm(item);
                                break;
                        }
                    }

                    MultiLineVisibility = Visibility.Visible;
                }
                else
                {
                    //没有数据
                    MultiLineVisibility = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region 渲染一个文本输入框
        private void RenderInputBox(ActionInputModel item, bool isMultiLine = false)
        {
            var inputBox = new InputBox();
            inputBox.Placeholder = item.Placeholder;
            inputBoxes.Add(inputBox);
            //绑定数据
            BindingOperations.SetBinding(inputBox, TextBox.TextProperty, new Binding()
            {
                Source = DataContext,
                Path = new PropertyPath(item.BindingName),
                Mode = BindingMode.TwoWay,
            });
            HandleInputBoxEvent(inputBox);
            if (!isMultiLine)
            {
                //单行输入
                if (!item.IsStretch && LineInputGroups.Count > 1)
                {
                    inputBox.Width = double.NaN;
                    inputBox.VerticalAlignment = VerticalAlignment.Center;
                    inputBox.Margin = new Thickness(0, 0, 10, 0);
                    if (!string.IsNullOrEmpty(item.Title))
                    {
                        LineContainer.Children.Add(GetLabel(item));
                    }
                    LineContainer.Children.Add(inputBox);
                }
                else
                {
                    var grid = GetLineGrid();
                    var label = GetLabel(item);
                    Grid.SetColumn(inputBox, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(inputBox);
                    label.Loaded += (e, c) =>
                    {
                        grid.Width = LineContainer.ActualWidth;
                    };
                    LineContainer.Children.Add(grid);
                }
            }
            else
            {
                //多行输入
                var grid = GetLineGrid();
                var label = GetLabel(item);
                Grid.SetColumn(inputBox, 1);
                grid.Children.Add(label);
                grid.Children.Add(inputBox);
                MultiLineContainer.Children.Add(grid);
            }

        }

        //绑定事件处理
        private void HandleInputBoxEvent(InputBox inputBox)
        {
            //变量弹出层
            inputBox.GotKeyboardFocus += (e, c) =>
            {
                VariablePopup.PlacementTarget = inputBox;
                VariablePopup.IsOpen = true;
                KeyboradFocusInputBox = inputBox;
            };
            inputBox.LostKeyboardFocus += (e, c) =>
            {
                if (!IsInVariablePopup)
                {
                    VariablePopup.IsOpen = false;
                    //KeyboradFocusInputBox = null;
                    
                }
                //验证输入
                Valid(inputBox);
            };
        }
        #endregion

        #region 渲染一个Kv表单控件
        private void RenderKVForm(ActionInputModel item)
        {
            var form = new ActionCustomKVForm();
            var label = GetLabel(item, true);
            //绑定数据
            BindingOperations.SetBinding(form, ActionCustomKVForm.KeyValuesProperty, new Binding()
            {
                Source = DataContext,
                Path = new PropertyPath(item.BindingName),
                Mode = BindingMode.TwoWay,
            });
            MultiLineContainer.Children.Add(label);
            MultiLineContainer.Children.Add(form);
            //事件处理
            form.OnAddInputBoxEvent += (e, c) =>
            {
                inputBoxes.Add(e as InputBox);
                HandleInputBoxEvent(e as InputBox);
            };
            form.OnRemoveInputBoxEvent += (e, c) =>
            {
                inputBoxes.Remove(e as InputBox);
            };
        }
        #endregion

        #region 获得一个文本标签
        /// <summary>
        /// 获得一个文本标签
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TextBlock GetLabel(ActionInputModel item, bool isLabelName = false)
        {
            var lable = new TextBlock();
            lable.Text = item.Title;
            if (isLabelName)
            {
                lable.Style = FindResource("LabelName") as Style;
            }
            lable.Margin = new Thickness(0, 0, 10, 0);
            lable.VerticalAlignment = VerticalAlignment.Center;
            return lable;
        }
        #endregion

        #region 获得一个行表格
        private Grid GetLineGrid()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(120, GridUnitType.Pixel)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            grid.Margin = new Thickness(0, 0, 0, 10);
            return grid;
        }
        #endregion

        #region 渲染一个选择框
        private void RenderSelectBox(ActionInputModel item, bool isMultiLine = false)
        {
            if (item.SelectItems != null && item.SelectItems.Count > 0)
            {
                var control = new ComboBox();
                control.ItemsSource = item.SelectItems;
                control.SelectedValuePath = "ID";
                control.DisplayMemberPath = "DisplayName";
                //绑定数据
                BindingOperations.SetBinding(control, ComboBox.SelectedValueProperty, new Binding()
                {
                    Source = DataContext,
                    Path = new PropertyPath(item.BindingName + ".ID"),
                    Mode = BindingMode.TwoWay,

                });
                BindingOperations.SetBinding(control, ComboBox.SelectedItemProperty, new Binding()
                {
                    Source = DataContext,
                    Path = new PropertyPath(item.BindingName),
                    Mode = BindingMode.TwoWay,

                });


                if (!isMultiLine)
                {
                    //单行输入
                    control.Width = double.NaN;
                    control.VerticalAlignment = VerticalAlignment.Center;
                    control.Margin = new Thickness(0, 0, 10, 0);
                    if (!string.IsNullOrEmpty(item.Title))
                    {
                        LineContainer.Children.Add(GetLabel(item));
                    }
                    LineContainer.Children.Add(control);
                }
                else
                {
                    //多行输入
                    var grid = GetLineGrid();
                    var label = GetLabel(item);
                    Grid.SetColumn(control, 1);
                    grid.Children.Add(label);
                    grid.Children.Add(control);
                    MultiLineContainer.Children.Add(grid);
                }
            }
        }
        #endregion


        #region 绑定操作结果选择框
        private void BindingActionResults()
        {
            VariableActionComboBox.ItemsSource = VariableActionItems;
            VariableActionComboBox.DisplayMemberPath = "DisplayName";
            VariableActionResultsComboBox.ItemsSource = VariableActionResultsItems;
            VariableActionResultsComboBox.DisplayMemberPath = "DisplayName";

            //切换操作结果时
            VariableActionComboBox.SelectionChanged += (e, c) =>
            {
                UpdateSelectionActionResults();
            };
        }
        #endregion

        #region 刷新支持获取结果的操作
        private void UpdateVariableAction()
        {
            if (ActionContainer != null)
            {
                VariableActionItems.Clear();
                //查找符合条件的action item
                var actionItems = ActionContainer.
                    ActionItems.
                    Where(m =>
                    m.Action.ActionType != UI.Types.ActionType.IF &&
                        m.Action.ActionType != UI.Types.ActionType.IFElse &&
                        m.Action.ActionType != UI.Types.ActionType.IFEnd &&
                        m.Action.ID != Action.ID &&
                        m.Action.Index < Action.Index).
                        OrderBy(m => m.Action.Index).
                        ToList();
                foreach (var item in actionItems)
                {
                    var action = item.Action;

                    VariableActionItems.Add(new ComBoxModel()
                    {
                        ID = action.ID,
                        DisplayName = $"[{action.ID}] {ActionNameData.Names[action.ActionType]}"
                    });
                }
            }

        }
        #endregion

        #region 刷新选中的操作支持的返回结果
        private void UpdateSelectionActionResults()
        {
            if (VariableActionComboBox.SelectedItem != null && VariableActionResultsComboBox != null)
            {
                var selectionItem = VariableActionComboBox.SelectedItem as ComBoxModel;
                var action = ActionContainer.ActionItems.Where(m => m.Action.ID == selectionItem.ID).FirstOrDefault();
                if (action != null)
                {
                    List<ComBoxModel> data = null;
                    switch (action.Action.ActionType)
                    {
                        case UI.Types.ActionType.HttpRequest:
                            data = HttpRequestActionData.ActionResults;
                            break;
                        case UI.Types.ActionType.WriteFile:
                            data = WriteFileActionData.ActionResults;
                            break;
                    }
                    SetActionResults(data);
                }
            }
        }
        private void SetActionResults(List<ComBoxModel> data)
        {
            if (data != null)
            {
                VariableActionResultsItems.Clear();
                foreach (var item in data)
                {
                    VariableActionResultsItems.Add(item);
                }
            }
        }
        #endregion

        #region 渲染全局变量
        /// <summary>
        /// 渲染全局变量
        /// </summary>
        private void RenderGlobalVariable()
        {
            if (GlobalVariablePanel != null)
            {
                GlobalVariablePanel.Children.Clear();

                foreach (var v in GlobalVariable.Variables)
                {
                    GlobalVariablePanel.Children.Add(CreateVariableButton(v.Value, $"@{v.Key}"));
                }
            }
        }
        #endregion

        #region 创建变量按钮
        private Button CreateVariableButton(string name, string variable)
        {
            var btn = new Button();
            btn.Style = FindResource("Icon") as Style;
            btn.Content = name;
            btn.Padding = new Thickness(5, 0, 5, 0);
            btn.Click += (e, c) =>
            {
                if (KeyboradFocusInputBox != null)
                {
                    KeyboradFocusInputBox.AppendText($"{{{variable}}}");
                    //KeyboradFocusInputBox.Text = $"{{{variable}}}";
                    KeyboradFocusInputBox.Focus();
                }
            };
            return btn;
        }
        #endregion

        #region 渲染事件变量
        private void RenderEventVariable()
        {
            if (EventVariablePanel != null && ActionContainer != null)
            {
                EventVariablePanel.Children.Clear();
                if (EventVariableData.Variables.ContainsKey(ActionContainer.EventType))
                {
                    var variables = EventVariableData.Variables[ActionContainer.EventType];
                    foreach (var v in variables)
                    {
                        EventVariablePanel.Children.Add(CreateVariableButton(v.Value, v.Key));
                    }
                }
            }
        }
        #endregion
        #region 验证输入变量
        private void Valid()
        {
            foreach (var inputBox in inputBoxes)
            {
                Valid(inputBox);
            }
        }
        /// <summary>
        /// 验证输入是否有效
        /// </summary>
        private void Valid(InputBox inputTextBox)
        {
            inputTextBox.BorderBrush = FindResource("InputBorderBrush") as SolidColorBrush;
            if (inputTextBox == null)
            {
                return;
            }
            if (inputTextBox.Text == string.Empty)
            {
                return;
            }
            //验证操作结果变量
            var matchs = Regex.Matches(inputTextBox.Text, @"\{(?<id>[0-9]{1,5})\.(?<key>[0-9]{1,25})\}");
            foreach (Match match in matchs)
            {
                var id = int.Parse(match.Groups["id"].Value);
                var key = int.Parse(match.Groups["key"].Value);
                //判断action是否存在
                if (!VariableActionItems.Where(m => m.ID == id).Any())
                {
                    inputTextBox.BorderBrush = Colors.GetColor(ColorTypes.Red);
                    break;
                }

                //判断action是否支持该变量
                if (!HttpRequestActionData.ActionResults.Where(m => m.ID == key).Any())
                {
                    inputTextBox.BorderBrush = Colors.GetColor(ColorTypes.Red);
                    break;
                }
            }
            //验证事件变量
            var eventVariableMatchs = Regex.Matches(inputTextBox.Text, @"\{(?<key>[a-zA-Z]{1,25})\}");
            if (eventVariableMatchs.Count > 0)
            {
                if (!EventVariableData.Variables.ContainsKey(ActionContainer.EventType))
                {
                    inputTextBox.BorderBrush = Colors.GetColor(ColorTypes.Red);
                    return;
                }
                var eventVariables = EventVariableData.Variables[ActionContainer.EventType];
                foreach (Match match in eventVariableMatchs)
                {
                    var key = match.Groups["key"].Value;
                    if (!eventVariables.ContainsKey(key))
                    {
                        inputTextBox.BorderBrush = Colors.GetColor(ColorTypes.Red);
                        break;
                    }
                }
            }
            //验证全局变量
            var variables = Regex.Matches(inputTextBox.Text, @"\{@(?<key>[a-zA-Z]{1,25})\}");
            if (variables.Count > 0)
            {
                foreach (Match variable in variables)
                {
                    var key = variable.Groups["key"].Value;
                    if (!GlobalVariable.Variables.ContainsKey(key))
                    {
                        inputTextBox.BorderBrush = Colors.GetColor(ColorTypes.Red);
                        break;
                    }
                }
            }

        }
        #endregion
    }
}
