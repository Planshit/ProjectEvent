using ProjectEvent.UI.Controls.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionCustomKVForm : Control
    {

        #region 私有属性
        private StackPanel Container;
        private Button AddBtn;
        #endregion
        public ActionCustomKVForm()
        {
            DefaultStyleKey = typeof(ActionCustomKVForm);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Container = GetTemplateChild("Container") as StackPanel;
            AddBtn = GetTemplateChild("AddBtn") as Button;
            AddBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateInputGroup();
        }


        #region 渲染一组输入框
        private void CreateInputGroup()
        {
            if (Container != null)
            {
                var grid = GetLineGrid();
                var keyBox = GetInputBox("键名");
                var valueBox = GetInputBox("值");
                grid.Margin = new Thickness(0, 10, 0, 0);
                Grid.SetColumn(valueBox, 1);
                grid.Children.Add(keyBox);
                grid.Children.Add(valueBox);
                Container.Children.Add(grid);
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
            grid.Margin = new Thickness(0, 0, 0, 10);
            return grid;
        }
        #endregion
    }
}
