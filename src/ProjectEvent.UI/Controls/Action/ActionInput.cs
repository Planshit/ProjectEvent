using ProjectEvent.Core.Action;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Event.Data;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Controls.Action.Types;
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

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionInput : Control
    {
        public EventType EventType
        {
            get { return (EventType)GetValue(EventTypeProperty); }
            set { SetValue(EventTypeProperty, value); }
        }
        public static readonly DependencyProperty EventTypeProperty =
            DependencyProperty.Register("EventType",
                typeof(EventType),
                typeof(ActionInput), new PropertyMetadata(new PropertyChangedCallback(OnEventTypeChanged)));

        private static void OnEventTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
            if (e.NewValue != e.OldValue)
            {
                (d as ActionInput).RenderEventVariable();
            }
        }

        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register("PlaceholderVisibility",
                typeof(Visibility),
                typeof(ActionInput));

        public bool PopupIsOpen
        {
            get { return (bool)GetValue(PopupIsOpenProperty); }
            set { SetValue(PopupIsOpenProperty, value); }
        }
        public static readonly DependencyProperty PopupIsOpenProperty =
            DependencyProperty.Register("PopupIsOpen",
                typeof(bool),
                typeof(ActionInput),
                new PropertyMetadata(false));

        public bool IsValidInput
        {
            get { return (bool)GetValue(IsValidInputProperty); }
            set { SetValue(IsValidInputProperty, value); }
        }
        public static readonly DependencyProperty IsValidInputProperty =
            DependencyProperty.Register("IsValidInput",
                typeof(bool),
                typeof(ActionInput),
                new PropertyMetadata(true));
        public InputType InputType
        {
            get { return (InputType)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType",
                typeof(InputType),
                typeof(ActionInput),
                new PropertyMetadata(InputType.Text));

        public object Data { get; set; }
        public string BindingName { get; set; }
        public List<ComBoxModel> ComboBoxItemsSource { get; set; }
        /// <summary>
        /// 当前可选Action Result
        /// </summary>
        public ObservableCollection<ComBoxModel> ActionResultItemsSource { get; set; }
        /// <summary>
        /// 当前可选的Action ID
        /// </summary>
        public ObservableCollection<ComBoxModel> ActionItemsSource { get; set; }

        public int ActionID { get; set; }
        /// <summary>
        /// 容器中所有Action的Result
        /// </summary>
        private Dictionary<int, List<ComBoxModel>> actionResults;
        private TextBox inputTextBox;
        private ComboBox ActionIDComboBox;
        private ComboBox ActionResultsComboBox;
        private ComboBox SelectComboBox;
        private Popup Popup;
        private WrapPanel eventVariablePanel, globalVariablePanel;
        private TabItem ActionResultTab;
        private bool isEnterPopup;
        public ActionContainer ActionContainer { get; set; }
        private Button addActionResultBtn;
        //public InputType InputType { get; set; }
        public ActionInput()
        {
            DefaultStyleKey = typeof(ActionInput);
            isEnterPopup = false;
            actionResults = new Dictionary<int, List<ComBoxModel>>();
            ComboBoxItemsSource = new List<ComBoxModel>();
            ActionResultItemsSource = new ObservableCollection<ComBoxModel>();
            ActionItemsSource = new ObservableCollection<ComBoxModel>();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //actionContainer = DataContext as ActionContainer;
            //ActionContainer.Items.CollectionChanged += Items_CollectionChanged;
            ActionContainer.ItemIndexChanged += ActionContainer_ItemIndexChanged;
            inputTextBox = GetTemplateChild("InputTextBox") as TextBox;
            inputTextBox.TextChanged += InputTextBox_TextChanged;
            inputTextBox.GotKeyboardFocus += InputTextBox_GotKeyboardFocus;
            inputTextBox.LostKeyboardFocus += InputTextBox_LostKeyboardFocus;

            ActionIDComboBox = GetTemplateChild("ActionIDComboBox") as ComboBox;
            ActionResultsComboBox = GetTemplateChild("ActionResultsComboBox") as ComboBox;
            SelectComboBox = GetTemplateChild("SelectComboBox") as ComboBox;
            Popup = GetTemplateChild("Popup") as Popup;
            addActionResultBtn = GetTemplateChild("AddActionResultBtn") as Button;
            eventVariablePanel = GetTemplateChild("EventVariablePanel") as WrapPanel;
            globalVariablePanel = GetTemplateChild("GlobalVariablePanel") as WrapPanel;
            ActionResultTab = GetTemplateChild("ActionResultTab") as TabItem;

            addActionResultBtn.Click += AddActionResultBtn_Click;
            ActionIDComboBox.SelectionChanged += ActionIDComboBox_SelectionChanged;

            Popup.MouseEnter += Popup_MouseEnter;
            Popup.MouseLeave += Popup_MouseLeave;
            Render();
            BindingData();
            RenderEventVariable();
            RenderGlobalVariable();
        }
        private void BindingData()
        {
            switch (InputType)
            {
                case InputType.Text:
                    //绑定数据
                    BindingOperations.SetBinding(inputTextBox, TextBox.TextProperty, new Binding()
                    {
                        Source = Data,
                        Path = new PropertyPath(BindingName),
                        Mode = BindingMode.TwoWay,
                    });
                    break;
                case InputType.Select:
                    //绑定数据
                    SelectComboBox.ItemsSource = ComboBoxItemsSource;
                    SelectComboBox.SelectedValuePath = "ID";
                    SelectComboBox.DisplayMemberPath = "DisplayName";
                    BindingOperations.SetBinding(SelectComboBox, ComboBox.SelectedValueProperty, new Binding()
                    {
                        Source = Data,
                        Path = new PropertyPath(BindingName + ".ID"),
                        Mode = BindingMode.TwoWay,

                    });
                    BindingOperations.SetBinding(SelectComboBox, ComboBox.SelectedItemProperty, new Binding()
                    {
                        Source = Data,
                        Path = new PropertyPath(BindingName),
                        Mode = BindingMode.TwoWay,

                    });

                    if (Data == null)
                    {
                        SelectComboBox.SelectedIndex = 0;
                    }
                    break;
            }

            //绑定action result combobox
            ActionResultsComboBox.ItemsSource = ActionResultItemsSource;
            ActionResultsComboBox.DisplayMemberPath = "DisplayName";
            //绑定action combobox
            ActionIDComboBox.ItemsSource = ActionItemsSource;
            ActionIDComboBox.DisplayMemberPath = "DisplayName";

        }
        private void AddActionResultBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ActionResultsComboBox.SelectedItem == null)
            {
                return;
            }
            inputTextBox.AppendText($"{{{(ActionIDComboBox.SelectedItem as ComBoxModel).ID}.{(ActionResultsComboBox.SelectedItem as ComBoxModel).ID}}}");
            inputTextBox.Focus();
            Valid();
        }

        private void ActionContainer_ItemIndexChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Render();
        }

        private void InputTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            Valid();
            if (!isEnterPopup)
            {
                PopupIsOpen = false;
            }
        }

        private void Popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isEnterPopup)
            {
                PopupIsOpen = false;
            }
            isEnterPopup = false;
        }

        private void Popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isEnterPopup = true;
        }

        private void InputTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            PopupIsOpen = true;
        }

        //选择的Action ID改变时重新更新Result box
        private void ActionIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActionResultItemsSource.Clear();
            if (ActionIDComboBox.SelectedItem != null)
            {
                var results = actionResults[(ActionIDComboBox.SelectedItem as ComBoxModel).ID];
                foreach (var item in results)
                {
                    ActionResultItemsSource.Add(item);
                }
                if (ActionResultsComboBox.Items.Count > 0)
                {
                    ActionResultsComboBox.SelectedIndex = 0;
                }
            }
        }

        private void Render()
        {
            if (InputType == InputType.Text)
            {
                UpdateActionResults();

                //更新ActionIDComboBox

                if (ActionIDComboBox.Items.Count > 0)
                {
                    ActionIDComboBox.SelectedIndex = 0;
                }
            }
            //else if (InputType == InputType.Select)
            //{
            //    //SelectComboBox.Items.Clear();
            //    //SelectComboBox.Items.Add("请选择条件");

            //    SelectComboBox.SelectedIndex = 0;
            //}
        }
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderVisibility = inputTextBox.Text == string.Empty ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateActionResults()
        {
            ActionItemsSource.Clear();
            actionResults.Clear();
            var selfItem = ActionContainer.ActionItems.Where(m => m.Action.ID == ActionID).FirstOrDefault();
            if (selfItem == null)
            {
                return;
            }

            //查找符合条件的action item
            var actionItems = ActionContainer.
                ActionItems.
                Where(m =>
                m.Action.ActionType != UI.Types.ActionType.IF &&
                    m.Action.ActionType != UI.Types.ActionType.IFElse &&
                    m.Action.ActionType != UI.Types.ActionType.IFEnd).
                ToList();
            foreach (var item in actionItems)
            {
                var action = item.Action;
                actionResults.Add(action.ID, new List<ComBoxModel>());
                var results = actionResults[action.ID];
                switch (action.ActionType)
                {
                    case UI.Types.ActionType.WriteFile:
                        results.Add(new ComBoxModel()
                        {
                            ID = (int)CommonResultKeyType.IsSuccess,
                            DisplayName = "是否成功"
                        });
                        break;
                    case UI.Types.ActionType.HttpGet:
                        results.Add(new ComBoxModel()
                        {
                            ID = (int)HttpResultType.IsSuccess,
                            DisplayName = "是否成功"
                        });
                        results.Add(new ComBoxModel()
                        {
                            ID = (int)HttpResultType.StatusCode,
                            DisplayName = "状态码"
                        });
                        results.Add(new ComBoxModel()
                        {
                            ID = (int)HttpResultType.Content,
                            DisplayName = "响应内容"
                        });
                        break;

                }
                if (item != selfItem &&
                    action.Index < selfItem.Action.Index)
                {
                    ActionItemsSource.Add(new ComBoxModel()
                    {
                        ID = action.ID,
                        DisplayName = $"[{action.ID}] {ActionNameData.Names[action.ActionType]}"
                    });
                }
            }
            Valid();
            //面板显示与隐藏
            if (ActionResultTab != null)
            {
                if (ActionItemsSource.Count == 0)
                {
                    ActionResultTab.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ActionResultTab.Visibility = Visibility.Visible;
                }
            }

        }


        /// <summary>
        /// 验证输入是否有效
        /// </summary>
        private void Valid()
        {
            IsValidInput = true;
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
                if (!actionResults.ContainsKey(id))
                {
                    IsValidInput = false;
                    break;
                }
                var ars = actionResults[id];
                if (!ActionItemsSource.Where(m => m.ID == id).Any() || !ars.Where(m => m.ID == key).Any())
                {
                    IsValidInput = false;
                    break;
                }
            }
            //验证事件变量
            var eventVariableMatchs = Regex.Matches(inputTextBox.Text, @"\{(?<key>[a-zA-Z]{1,25})\}");
            if (eventVariableMatchs.Count > 0)
            {
                if (!EventVariableData.Variables.ContainsKey(this.EventType))
                {
                    IsValidInput = false;
                    return;
                }
                var eventVariables = EventVariableData.Variables[this.EventType];
                foreach (Match match in eventVariableMatchs)
                {
                    var key = match.Groups["key"].Value;
                    if (!eventVariables.ContainsKey(key))
                    {
                        IsValidInput = false;
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
                        IsValidInput = false;
                        break;
                    }
                }
            }

        }

        private void RenderEventVariable()
        {
            if (eventVariablePanel != null)
            {
                eventVariablePanel.Children.Clear();
                if (EventVariableData.Variables.ContainsKey(this.EventType))
                {
                    var variables = EventVariableData.Variables[this.EventType];
                    foreach (var v in variables)
                    {
                        eventVariablePanel.Children.Add(CreateVariableButton(v.Value, v.Key));
                    }
                }
            }
            Valid();

        }
        private Button CreateVariableButton(string name, string variable)
        {
            var btn = new Button();
            btn.Style = FindResource("Icon") as Style;
            btn.Content = name;
            btn.Padding = new Thickness(5, 0, 5, 0);
            btn.Click += (e, c) =>
            {
                if (inputTextBox != null)
                {
                    inputTextBox.AppendText($"{{{variable}}}");
                    inputTextBox.Focus();
                    Valid();
                }
            };
            return btn;
        }

        /// <summary>
        /// 渲染全局变量
        /// </summary>
        private void RenderGlobalVariable()
        {
            if (globalVariablePanel != null)
            {
                globalVariablePanel.Children.Clear();

                foreach (var v in GlobalVariable.Variables)
                {
                    globalVariablePanel.Children.Add(CreateVariableButton(v.Value, $"@{v.Key}"));
                }
            }
            Valid();

        }
    }
}
