using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ProjectEvent.UI.Controls.IconSelection
{
    public class IconSelection : Control
    {
        public IconTypes SelectedIcon
        {
            get { return (IconTypes)GetValue(SelectedIconProperty); }
            set { SetValue(SelectedIconProperty, value); }
        }
        public static readonly DependencyProperty SelectedIconProperty =
            DependencyProperty.Register("SelectedIcon", typeof(IconTypes), typeof(IconSelection));

        private Popup popup;
        private WrapPanel iconsPanel;
        private Button button;
        public IconSelection()
        {
            DefaultStyleKey = typeof(IconSelection);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            popup = GetTemplateChild("Popup") as Popup;
            button = GetTemplateChild("Button") as Button;
            iconsPanel = GetTemplateChild("IconsPanel") as WrapPanel;
            Init();
        }

        private void Init()
        {
            button.Click += (e, c) =>
            {
                popup.IsOpen = true;
            };
            //导入图标
            foreach (IconTypes icon in Enum.GetValues(typeof(IconTypes)))
            {
                if(icon!= IconTypes.None)
                {
                    var iconBtn = new Button();
                    iconBtn.Width = 50;
                    iconBtn.Height = 50;
                    iconBtn.FontSize = 12;
                    iconBtn.Style = FindResource("Icon") as Style;
                    iconBtn.Content = new Icon()
                    {
                        IconType = icon
                    };
                    iconBtn.Click += (e, c) =>
                    {
                        SelectedIcon = icon;
                        popup.IsOpen = false;
                    };
                    iconsPanel.Children.Add(iconBtn);
                }
                
            }
        }
    }
}
