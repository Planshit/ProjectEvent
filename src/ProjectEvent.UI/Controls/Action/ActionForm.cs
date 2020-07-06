using ProjectEvent.UI.Base.Color;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Controls.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
                control.MultiLineGrid.Visibility = Visibility.Visible;
            }
            else
            {
                control.MultiLineGrid.Visibility = Visibility.Collapsed;
            }
        }
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
        private Grid MultiLineGrid;
        #endregion

        public event EventHandler OnRenderDone;
        public ActionForm()
        {
            DefaultStyleKey = typeof(ActionForm);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LineContainer = GetTemplateChild("LineContainer") as WrapPanel;
            ExpandBtn = GetTemplateChild("ExpandBtn") as Button;
            MultiLineContainer = GetTemplateChild("MultiLineContainer") as StackPanel;
            MultiLineGrid = GetTemplateChild("MultiLineGrid") as Grid;
            ExpandBtn.Click += (e, c) =>
            {
                IsExpandMultiForm = !IsExpandMultiForm;
            };
            Render();
           
        }

        #region 渲染输入控件
        private void Render()
        {
            RenderLineInputGroups();
            RenderMultiLineInputGroups();
            MultiLineContainer.Loaded += (e, c) =>
            {
                Debug.WriteLine("action form height:" + this.ActualHeight);
                MultiLineGrid.Visibility = Visibility.Collapsed;
                OnRenderDone?.Invoke(this, null);
            };
        }
        #endregion
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
        #endregion

        #region 渲染一个Kv表单控件
        private void RenderKVForm(ActionInputModel item)
        {
            var form = new ActionCustomKVForm();
            var label = GetLabel(item);
            MultiLineContainer.Children.Add(label);
            MultiLineContainer.Children.Add(form);

        }
        #endregion

        #region 获得一个文本标签
        /// <summary>
        /// 获得一个文本标签
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TextBlock GetLabel(ActionInputModel item)
        {
            var lable = new TextBlock();
            lable.Text = item.Title;
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
    }
}
