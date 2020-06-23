﻿using ProjectEvent.UI.Base.Color;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Controls.Navigation.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectEvent.UI.Controls.Navigation
{
    public class NavigationItem : Control
    {
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID",
                typeof(int),
                typeof(NavigationItem));
        public IconTypes Icon
        {
            get { return (IconTypes)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(IconTypes),
                typeof(NavigationItem), new PropertyMetadata(IconTypes.None));
        public ColorTypes IconColor
        {
            get { return (ColorTypes)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }
        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register("IconColor",
                typeof(ColorTypes),
                typeof(NavigationItem), new PropertyMetadata(ColorTypes.Blue));
        public SolidColorBrush IconColorBrush
        {
            get { return (SolidColorBrush)GetValue(IconColorBrushProperty); }
            set { SetValue(IconColorBrushProperty, value); }
        }
        public static readonly DependencyProperty IconColorBrushProperty =
            DependencyProperty.Register("IconColorBrush",
                typeof(SolidColorBrush),
                typeof(NavigationItem), new PropertyMetadata(UI.Base.Color.Colors.GetColor(ColorTypes.Blue)));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title",
                typeof(string),
                typeof(NavigationItem), new PropertyMetadata(string.Empty));

        public string BadgeText
        {
            get { return (string)GetValue(BadgeTextProperty); }
            set { SetValue(BadgeTextProperty, value); }
        }
        public static readonly DependencyProperty BadgeTextProperty =
            DependencyProperty.Register("BadgeText",
                typeof(string),
                typeof(NavigationItem), new PropertyMetadata(string.Empty));
        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri",
                typeof(string),
                typeof(NavigationItem), new PropertyMetadata(string.Empty));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
                typeof(bool),
                typeof(NavigationItem), new PropertyMetadata(false));

        public delegate void NavigationEventHandler(object sender, MouseButtonEventArgs e);
        public event NavigationEventHandler MouseUp;
        public NavigationItem()
        {
            DefaultStyleKey = typeof(NavigationItem);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            MouseUp?.Invoke(this, e);
        }
    }
}
