using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Base.Color;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProjectEvent.UI.Controls.Base;
using System.Windows.Media;
using Colors = ProjectEvent.UI.Base.Color.Colors;

namespace ProjectEvent.UI.Controls.ItemSelect
{
    public class Item : Control
    {
        public int ID { get; set; }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Item), new PropertyMetadata("未知"));
        public string Tag
        {
            get { return (string)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }
        public static readonly DependencyProperty TagProperty =
            DependencyProperty.Register("Tag", typeof(string), typeof(Item));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(Item), new PropertyMetadata("未知"));

        public IconTypes Icon
        {
            get { return (IconTypes)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconTypes), typeof(Item), new PropertyMetadata(IconTypes.StatusCircleQuestionMark));
        public SolidColorBrush IconColor
        {
            get { return (SolidColorBrush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor", typeof(SolidColorBrush), typeof(Item), new PropertyMetadata(Colors.GetColor(ColorTypes.Blue)));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Item), new PropertyMetadata(false));
        public ColorTypes Color
        {
            get { return (ColorTypes)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(ColorTypes), typeof(Item), new PropertyMetadata(ColorTypes.White, new PropertyChangedCallback(OnColorChanged)));

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Item;
            if (e.NewValue != e.OldValue)
            {
                control.IconColor = Colors.GetColor(control.Color);
                //if (control.Color == ColorTypes.White)
                //{
                //    control.Foreground = Colors.GetFromString("#323130");
                //    control.BorderBrush = Colors.GetFromString("#8A8886");
                //    control.BorderThickness = new Thickness(1);
                //}
                //else
                //{
                //    control.Foreground = Colors.GetColor(ColorTypes.White);
                //    control.BorderBrush = control.Background;
                //    control.BorderThickness = new Thickness(0);
                //}
            }
        }

        public Item()
        {
            DefaultStyleKey = typeof(Item);
            Foreground = Colors.GetFromString("#323130");
            BorderThickness = new Thickness(0);
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "MouseEnter", true);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "MouseLeave", true);
        }
    }
}
