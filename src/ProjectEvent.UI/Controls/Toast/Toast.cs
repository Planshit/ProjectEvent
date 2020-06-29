using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProjectEvent.UI.Controls.Toast
{
    public class Toast : Control
    {
        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }
        public static readonly DependencyProperty IsShowProperty =
            DependencyProperty.Register("IsShow",
                typeof(bool),
                typeof(Toast), new PropertyMetadata(false, new PropertyChangedCallback(OnIsShowChanged)));

        private static void OnIsShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Toast;
            if (e.OldValue != e.NewValue)
            {
                VisualStateManager.GoToState(control, control.IsShow ? "Show" : "Hide", true);
                if (control.IsShow)
                {
                    control.autoHideTimer.Start();
                }
            }
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content",
                typeof(string),
                typeof(Toast), new PropertyMetadata(string.Empty));

        public ToastType Type
        {
            get { return (ToastType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type",
                typeof(ToastType),
                typeof(Toast), new PropertyMetadata(ToastType.Normal));
        private DispatcherTimer autoHideTimer;
        public Toast()
        {
            DefaultStyleKey = typeof(Toast);
            autoHideTimer = new DispatcherTimer();
            autoHideTimer.Tick += AutoHideTimer_Tick;
            autoHideTimer.Interval = new TimeSpan(0, 0, 4);
        }

        private void AutoHideTimer_Tick(object sender, EventArgs e)
        {
            autoHideTimer.Stop();
            IsShow = false;
        }

    }
}
