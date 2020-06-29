using ProjectEvent.UI.Controls.Navigation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.Navigation
{
    public class Navigation : Control
    {
        public bool IsShowNavigation
        {
            get { return (bool)GetValue(IsShowNavigationProperty); }
            set { SetValue(IsShowNavigationProperty, value); }
        }
        public static readonly DependencyProperty IsShowNavigationProperty =
            DependencyProperty.Register("IsShowNavigation", typeof(bool), typeof(Navigation), new PropertyMetadata(true, new PropertyChangedCallback(OnIsShowNavigationChanged)));

        private static void OnIsShowNavigationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigation;
            if (e.NewValue != e.OldValue)
            {
                if (control.IsShowNavigation && control.Visibility == Visibility.Collapsed && control.WindowWidth > 700)
                {
                    control.Visibility = Visibility.Visible;
                }
                if (!control.IsShowNavigation && control.Visibility == Visibility.Visible)
                {
                    control.Visibility = Visibility.Collapsed;
                }
            }
        }

        public object TopExtContent
        {
            get { return (object)GetValue(TopExtContentProperty); }
            set { SetValue(TopExtContentProperty, value); }
        }
        public static readonly DependencyProperty TopExtContentProperty =
            DependencyProperty.Register("TopExtContent", typeof(object), typeof(Navigation));
        public object BottomExtContent
        {
            get { return (object)GetValue(BottomExtContentProperty); }
            set { SetValue(BottomExtContentProperty, value); }
        }
        public static readonly DependencyProperty BottomExtContentProperty =
            DependencyProperty.Register("BottomExtContent", typeof(object), typeof(Navigation));
        public ObservableCollection<NavigationItemModel> Data
        {
            get
            {
                return (ObservableCollection<NavigationItemModel>)GetValue(DataProperty);
            }
            set
            {
                SetValue(DataProperty, value);
            }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ObservableCollection<NavigationItemModel>), typeof(Navigation), new PropertyMetadata(new PropertyChangedCallback(OnDataChanged)));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigation;
            if (e.NewValue != e.OldValue)
            {
                if (control.Data != null)
                {
                    control.Data.CollectionChanged += control.Data_CollectionChanged;
                }
                foreach (var item in control.Data)
                {
                    control.AddItem(item);
                }
            }
        }

        public double WindowWidth
        {
            get
            {
                return (double)GetValue(WindowWidthProperty);
            }
            set
            {
                SetValue(WindowWidthProperty, value);
            }
        }
        public static readonly DependencyProperty WindowWidthProperty =
            DependencyProperty.Register("WindowWidth", typeof(double), typeof(Navigation), new PropertyMetadata(new PropertyChangedCallback(OnWindowWidthChanged)));

        private static void OnWindowWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Navigation;
            if (control.IsShowNavigation)
            {
                if ((double)e.NewValue <= 700)
                {
                    control.Visibility = Visibility.Collapsed;
                }
                else
                {

                    control.Visibility = Visibility.Visible;
                }
            }
        }


        #region items property


        //protected static PropertyChangedCallback ItemsPropertyChangedCallback = new PropertyChangedCallback(ItemsPropertyChanged);

        //public static DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ObservableCollection<NavigationItemModel>), typeof(Navigation), new PropertyMetadata(null, ItemsPropertyChangedCallback));

        //private static void ItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var control = (Navigation)sender;
        //    if (control == null)
        //    {
        //        return;
        //    }
        //    control.UnregisterItems(e.OldValue as ObservableCollection<NavigationItemModel>);
        //    control.RegisterItems(e.NewValue as ObservableCollection<NavigationItemModel>);
        //}

        //public ObservableCollection<NavigationItemModel> Items
        //{
        //    get
        //    {
        //        return (ObservableCollection<NavigationItemModel>)GetValue(ItemsProperty);
        //    }
        //    set
        //    {
        //        SetValue(ItemsProperty, value);
        //    }
        //}

        //protected void UnregisterItems(ObservableCollection<NavigationItemModel> items)
        //{
        //    if (items == null)
        //    {
        //        return;
        //    }
        //    items.CollectionChanged -= ItemsChanged;
        //}

        //protected void RegisterItems(ObservableCollection<NavigationItemModel> items)
        //{
        //    if (items == null)
        //    {
        //        return;
        //    }
        //    items.CollectionChanged += ItemsChanged;
        //}

        //protected virtual void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (var item in e.NewItems)
        //        {
        //            var data = item as NavigationItemModel;
        //            AddItem(data);
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (var item in e.OldItems)
        //        {
        //            var data = item as NavigationItemModel;
        //            RemoveItem(data);
        //        }
        //    }
        //}
        #endregion
        public event RoutedEventHandler OnSelected;
        public event RoutedEventHandler OnMouseRightButtonUP;

        private StackPanel ItemsPanel;
        private Dictionary<int, NavigationItem> ItemsDictionary;
        private int SelectedID;
        public NavigationItemModel SelectedItem { get; set; }
        public Navigation()
        {
            DefaultStyleKey = typeof(Navigation);
            ItemsDictionary = new Dictionary<int, NavigationItem>();
        }

        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    AddItem(item as NavigationItemModel);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    RemoveItem(item as NavigationItemModel);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (var ritem in e.NewItems)
                {
                    var item = ritem as NavigationItemModel;
                    var id = item.ID;
                    ItemsDictionary[id].Icon = item.Icon;
                    ItemsDictionary[id].Title = item.Title;

                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ItemsPanel = GetTemplateChild("ItemsPanel") as StackPanel;
            Render();
        }
        private int CreateID()
        {
            if (ItemsDictionary.Count == 0)
            {
                return 1;
            }
            return ItemsDictionary.Max(m => m.Key) + 1;
        }
        private void AddItem(NavigationItemModel item)
        {
            if (ItemsPanel != null)
            {
                var navItem = new NavigationItem();
                int id = item.ID <= 0 ? CreateID() : item.ID;
                item.ID = id;
                navItem.ID = id;
                navItem.Title = item.Title;
                navItem.Icon = item.Icon;
                navItem.IconColor = item.IconColor;
                navItem.BadgeText = item.BadgeText;
                navItem.Uri = item.Uri;
                navItem.IsSelected = item.IsSelected;
                if (navItem.IsSelected)
                {
                    SelectedID = id;
                }
                if (!string.IsNullOrEmpty(item.Title))
                {
                    navItem.MouseUp += NavItem_MouseUp;
                }
                ItemsPanel.Children.Add(navItem);
                ItemsDictionary.Add(id, navItem);
            }
        }

        private void NavItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                //左键选中
                SetSelectedItem((sender as NavigationItem).ID);
                OnSelected?.Invoke(this, null);
            }
            if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            {
                //右键
                SetSelectedItem((sender as NavigationItem).ID, false);
                OnMouseRightButtonUP?.Invoke(this, null);
            }
        }
        private void SetSelectedItem(int id, bool isSelected = true)
        {
            if (isSelected)
            {
                if (id == SelectedID)
                {
                    return;
                }
                if (ItemsDictionary.ContainsKey(SelectedID))
                {
                    ItemsDictionary[SelectedID].IsSelected = false;
                }
                ItemsDictionary[id].IsSelected = true;
                SelectedID = id;
            }
            var item = Data.Where(m => m.ID == id).FirstOrDefault();
            SelectedItem = item;

        }
        private void RemoveItem(NavigationItemModel item)
        {
            var navItem = ItemsDictionary[item.ID];
            ItemsPanel.Children.Remove(navItem);
            ItemsDictionary.Remove(item.ID);
        }
        private void Render()
        {
            if (Data != null)
            {
                foreach (var item in Data)
                {
                    AddItem(item);
                }
            }
        }
    }
}
