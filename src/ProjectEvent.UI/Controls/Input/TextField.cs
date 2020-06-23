using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.Input
{
    public class TextField : Control
    {
        /// <summary>
        /// 图标
        /// </summary>
        public IconTypes IconType
        {
            get { return (IconTypes)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }
        public static readonly DependencyProperty IconTypeProperty =
            DependencyProperty.Register("IconType",
                typeof(IconTypes),
                typeof(TextField), new PropertyMetadata(IconTypes.None));
        /// <summary>
        /// 输入框类型
        /// </summary>
        public InputTypes InputType
        {
            get { return (InputTypes)GetValue(InputTypeProperty); }
            set { SetValue(InputTypeProperty, value); }
        }
        public static readonly DependencyProperty InputTypeProperty =
            DependencyProperty.Register("InputType",
                typeof(InputTypes),
                typeof(TextField), new PropertyMetadata(InputTypes.Text));
        /// <summary>
        /// 占位符
        /// </summary>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder",
                typeof(string),
                typeof(TextField), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 内容
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                typeof(string),
                typeof(TextField), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 名称
        /// </summary>
        public string LabelName
        {
            get { return (string)GetValue(LabelNameProperty); }
            set { SetValue(LabelNameProperty, value); }
        }
        public static readonly DependencyProperty LabelNameProperty =
            DependencyProperty.Register("LabelName",
                typeof(string),
                typeof(TextField), new PropertyMetadata(string.Empty));
        public TextField()
        {
            DefaultStyleKey = typeof(TextField);
        }
    }
}
