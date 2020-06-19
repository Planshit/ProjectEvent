using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.TabContainer
{
    public class TabContainer : Control
    {
        public int SelectIndex
        {
            get { return (int)GetValue(SelectIndexProperty); }
            set { SetValue(SelectIndexProperty, value); }
        }
        public static readonly DependencyProperty SelectIndexProperty =
            DependencyProperty.Register("SelectIndex",
                typeof(int),
                typeof(TabContainer), new PropertyMetadata(-1, new PropertyChangedCallback(OnPropertyChangedCallback)));

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TabContainer;
            if (e.NewValue != e.OldValue)
            {
                control.UpdateVisibility((int)e.OldValue, (int)e.NewValue);
            }
        }

        public ObservableCollection<TabItem> Items { get; set; }
        public ObservableCollection<TabHeaderButton> HeaderButtons { get; set; }

        private StackPanel Header;
        private Grid ContentContainer;
        public TabContainer()
        {
            DefaultStyleKey = typeof(TabContainer);
            Items = new ObservableCollection<TabItem>();
            HeaderButtons = new ObservableCollection<TabHeaderButton>();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Header = GetTemplateChild("Header") as StackPanel;
            ContentContainer = GetTemplateChild("Content") as Grid;
            Render();
        }
        private void Render()
        {
            foreach (var item in Items)
            {
                item.Visibility = Visibility.Hidden;
                item.DataContextChanged += Item_DataContextChanged;
                ContentContainer.Children.Add(item);
                var button = new TabHeaderButton();
                button.Title = item.Title;
                button.MouseLeftButtonUp += Button_MouseLeftButtonUp;
                HeaderButtons.Add(button);
                Header.Children.Add(button);
            }
            if (Items.Count > 0)
            {
                SelectIndex = 0;
            }
        }

        private void Item_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e.NewValue);
        }

        private void Button_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectIndex = HeaderButtons.IndexOf(sender as TabHeaderButton);
        }

        private void UpdateVisibility(int hide, int show)
        {
            if (hide >= 0)
            {
                Items[hide].Visibility = Visibility.Hidden;
                HeaderButtons[hide].IsSelected = false;
            }
            Items[show].Visibility = Visibility.Visible;
            HeaderButtons[show].IsSelected = true;
        }
    }
}
