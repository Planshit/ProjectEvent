﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectEvent.UI.Controls.Toggle
{
    public class Toggle : Control
    {
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(Toggle), new PropertyMetadata(new PropertyChangedCallback(OnIsCheckedChanged)));

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Toggle;
            if (e.OldValue != e.NewValue)
            {
                control.Text = control.IsChecked ? control.OnText : control.OffText;
            }
        }

        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }
        public static readonly DependencyProperty OnTextProperty =
            DependencyProperty.Register("OnText", typeof(string), typeof(Toggle), new PropertyMetadata("开"));
        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }
        public static readonly DependencyProperty OffTextProperty =
            DependencyProperty.Register("OffText", typeof(string), typeof(Toggle), new PropertyMetadata("关"));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Toggle), new PropertyMetadata("关"));

        public Toggle()
        {
            DefaultStyleKey = typeof(Toggle);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, IsChecked ? "On" : "Off", true);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            IsChecked = !IsChecked;
            VisualStateManager.GoToState(this, IsChecked ? "On" : "Off", true);
        }
    }
}
