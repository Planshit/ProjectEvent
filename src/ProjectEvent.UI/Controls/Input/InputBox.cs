using ProjectEvent.UI.Controls.Action.Types;
using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ProjectEvent.UI.Controls.Input
{
    public class InputBox : TextBox
    {
        /// <summary>
        /// 当前选中时间（仅输入类型为datetime时有效）
        /// </summary>
        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
        public static readonly DependencyProperty SelectedDateTimeProperty =
            DependencyProperty.Register("SelectedDateTime",
                typeof(DateTime),
                typeof(InputBox));

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
                typeof(InputBox), new PropertyMetadata(IconTypes.None));
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
                typeof(InputBox), new PropertyMetadata(InputTypes.Text));
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
                typeof(InputBox), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 最小更改值
        /// 仅在输入类型是数字时有效，滚动鼠标滑轮可以上下加减
        /// </summary>
        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }
        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register("SmallChange",
                typeof(double),
                typeof(InputBox), new PropertyMetadata((double)1));
        /// <summary>
        /// 最小值
        /// 仅在输入类型是数字时有效
        /// </summary>
        public double Mininum
        {
            get { return (double)GetValue(MininumProperty); }
            set { SetValue(MininumProperty, value); }
        }
        public static readonly DependencyProperty MininumProperty =
            DependencyProperty.Register("Mininum",
                typeof(double),
                typeof(InputBox), new PropertyMetadata((double)0));
        /// <summary>
        /// 最大值
        /// 仅在输入类型是数字时有效
        /// </summary>
        public double Maxnum
        {
            get { return (double)GetValue(MaxnumProperty); }
            set { SetValue(MaxnumProperty, value); }
        }
        public static readonly DependencyProperty MaxnumProperty =
            DependencyProperty.Register("Maxnum",
                typeof(double),
                typeof(InputBox), new PropertyMetadata(double.MaxValue));
        private Popup DateTimePopup;
        private DateTimePicker DateTimePicker;
        private Grid Grid;
        private TextBlock SelectedTimeText;
        public InputBox()
        {
            DefaultStyleKey = typeof(InputBox);
            LostFocus += InputBox_LostFocus;
            MouseWheel += InputBox_MouseWheel;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (InputType == InputTypes.DateTime)
            {
                DateTimePopup = new Popup();
                DateTimePopup.AllowsTransparency = true;
                DateTimePopup.Width = 570;
                DateTimePopup.Height = 270;
                DateTimePopup.StaysOpen = false;

                DateTimePicker = new DateTimePicker();

                var border = new Border();
                border.Margin = new Thickness(10);
                border.Background = System.Windows.Media.Brushes.White;
                border.CornerRadius = new CornerRadius(2);
                border.Effect = new DropShadowEffect()
                {
                    BlurRadius = 10,
                    Opacity = .2,
                    ShadowDepth = 0
                };
                border.Child = DateTimePicker;
                DateTimePopup.Child = border;
                Grid = GetTemplateChild("Grid") as Grid;
                SelectedTimeText = GetTemplateChild("SelectedTimeText") as TextBlock;
                Grid.Children.Add(DateTimePopup);

                BindingOperations.SetBinding(DateTimePicker, DateTimePicker.SelectedDateTimeProperty, new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(SelectedDateTime)),
                    Mode = BindingMode.TwoWay,
                });
                BindingOperations.SetBinding(SelectedTimeText, TextBlock.TextProperty, new Binding()
                {
                    Source = DateTimePicker,
                    Path = new PropertyPath(nameof(DateTimePicker.SelectedDateTimeStr)),
                    Mode = BindingMode.Default,
                });
            }

        }

        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InputType == InputTypes.DateTime)
            {
                DateTimePopup.IsOpen = true;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (InputType == InputTypes.DateTime)
            {
                DateTimePopup.IsOpen = true;
            }
        }

        private void InputBox_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            if (InputType == InputTypes.Number && IsFocused)
            {
                NumberValid();
                if (e.Delta > 0)
                {
                    Text = (double.Parse(Text) + SmallChange).ToString();
                }
                else
                {
                    Text = (double.Parse(Text) - SmallChange).ToString();
                }
            }
        }

        private void InputBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberValid();
        }


        private void NumberValid()
        {

            if (InputType == InputTypes.Number)
            {
                var floatNumber = Regex.Match(Text, @"[0-9]+\.[0-9]+|[0-9]+|-[0-9]+\.[0-9]+|-[0-9]+").Value;
                if (string.IsNullOrEmpty(floatNumber))
                {
                    Text = Mininum.ToString();
                }
                else
                {
                    var dv = double.Parse(floatNumber);
                    Text = dv > Maxnum ? Maxnum.ToString() : dv < Mininum ? Mininum.ToString() : floatNumber;
                }
            }


        }
    }
}
