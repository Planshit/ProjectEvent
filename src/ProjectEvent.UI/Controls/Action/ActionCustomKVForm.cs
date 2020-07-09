using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Controls.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionCustomKVForm : Control
    {
        #region 依赖属性
        #region 键值数据
        /// <summary>
        /// 键值数据
        /// </summary>
        public Dictionary<string, string> KeyValues
        {
            get { return (Dictionary<string, string>)GetValue(KeyValuesProperty); }
            set { SetValue(KeyValuesProperty, value); }
        }
        public static readonly DependencyProperty KeyValuesProperty =
            DependencyProperty.Register("KeyValues",
                typeof(Dictionary<string, string>),
                typeof(ActionCustomKVForm), new PropertyMetadata(new PropertyChangedCallback(OnKeyValuesChanged)));

        private static void OnKeyValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionCustomKVForm;
            var oldValue = e.OldValue as Dictionary<string, string>;
            var newValue = e.NewValue as Dictionary<string, string>;

            string oldstr = string.Empty, newstr = string.Empty;
            if (oldValue != null)
            {
                oldstr += string.Join('.', oldValue.Keys);
                oldstr += string.Join('.', oldValue.Values);
            }
            if (newValue != null)
            {
                newstr += string.Join('.', newValue.Keys);
                newstr += string.Join('.', newValue.Values);
            }

            if (oldstr != newstr)
            {
                control.RenderData();
            }
        }
        #endregion

        #endregion
        #region 私有属性
        private StackPanel Container;
        private Button AddBtn;
        private Dictionary<InputBox, InputBox> inputBoxs;
        #endregion
        #region 公共属性
        public event EventHandler OnAddInputBoxEvent;
        public event EventHandler OnRemoveInputBoxEvent;
        #endregion
        public ActionCustomKVForm()
        {
            DefaultStyleKey = typeof(ActionCustomKVForm);
            inputBoxs = new Dictionary<InputBox, InputBox>();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Container = GetTemplateChild("Container") as StackPanel;
            AddBtn = GetTemplateChild("AddBtn") as Button;
            AddBtn.Click += AddBtn_Click;
            RenderData();
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateInputGroup();
        }

        #region 渲染数据
        private void RenderData()
        {
            if (KeyValues != null && Container != null)
            {
                Container.Children.Clear();
                inputBoxs.Clear();
                foreach (var item in KeyValues)
                {
                    CreateInputGroup(item.Key, item.Value);
                }
            }
        }
        #endregion

        #region 渲染一组输入框
        private void CreateInputGroup(string key = null, string value = null)
        {
            if (Container != null)
            {
                var grid = GetLineGrid();
                var keyBox = GetInputBox("键名");
                var valueBox = GetInputBox("值");
                var delBtn = new Button();

                //数据监听
                keyBox.LostKeyboardFocus += (e, c) =>
                {
                    UpdateData();
                };
                valueBox.LostKeyboardFocus += (e, c) =>
                {
                    UpdateData();
                };
                delBtn.VerticalAlignment = VerticalAlignment.Center;
                delBtn.HorizontalAlignment = HorizontalAlignment.Center;
                delBtn.Style = FindResource("Icon") as Style;
                delBtn.Content = new Icon()
                {
                    IconType = IconTypes.Delete
                };
                delBtn.Click += (e, c) =>
                {
                    Container.Children.Remove(grid);
                    inputBoxs.Remove(keyBox);
                    UpdateData();
                    //响应事件
                    OnRemoveInputBoxEvent?.Invoke(keyBox, null);
                    OnRemoveInputBoxEvent?.Invoke(valueBox, null);
                };
                //填充数据
                if (key != null)
                {
                    keyBox.Text = key;
                }
                if (value != null)
                {
                    valueBox.Text = value;
                }
                keyBox.Margin = new Thickness(0, 0, 10, 0);
                grid.Margin = new Thickness(0, 10, 0, 0);
                Grid.SetColumn(valueBox, 1);
                Grid.SetColumn(delBtn, 2);
                grid.Children.Add(keyBox);
                grid.Children.Add(valueBox);
                grid.Children.Add(delBtn);
                Container.Children.Add(grid);

                inputBoxs.Add(keyBox, valueBox);
                //响应事件
                OnAddInputBoxEvent?.Invoke(keyBox, null);
                OnAddInputBoxEvent?.Invoke(valueBox, null);
            }


        }
        #endregion

        #region 渲染一个文本输入框
        private InputBox GetInputBox(string placeholder)
        {
            var inputBox = new InputBox();
            inputBox.Placeholder = placeholder;
            inputBox.Width = double.NaN;
            inputBox.VerticalAlignment = VerticalAlignment.Center;
            return inputBox;
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
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(50, GridUnitType.Pixel)
            });
            grid.Margin = new Thickness(0, 0, 0, 10);
            return grid;
        }
        #endregion

        #region 刷新数据
        private void UpdateData()
        {
            var newData = new Dictionary<string, string>();
            foreach (var item in inputBoxs)
            {
                if (!string.IsNullOrEmpty(item.Key.Text) && !newData.ContainsKey(item.Key.Text))
                {
                    newData.Add(item.Key.Text, item.Value.Text);
                }
            }
            KeyValues = newData;
        }
        #endregion
    }
}
