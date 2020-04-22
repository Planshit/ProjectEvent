using ProjectEvent.UI.Controls.ItemSelect.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.ItemSelect
{
    public class ItemSelect : Control
    {

        #region items property
        protected static PropertyChangedCallback ItemsPropertyChangedCallback = new PropertyChangedCallback(ItemsPropertyChanged);

        public static DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ObservableCollection<ItemModel>), typeof(ItemSelect), new PropertyMetadata(null, ItemsPropertyChangedCallback));

        private static void ItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (ItemSelect)sender;
            if (control == null)
            {
                return;
            }
            control.UnregisterItems(e.OldValue as ObservableCollection<ItemModel>);
            control.RegisterItems(e.NewValue as ObservableCollection<ItemModel>);
        }

        public ObservableCollection<ItemModel> Items
        {
            get
            {
                return (ObservableCollection<ItemModel>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        protected void UnregisterItems(ObservableCollection<ItemModel> items)
        {
            if (items == null)
            {
                return;
            }
            items.CollectionChanged -= ItemsChanged;
        }

        protected void RegisterItems(ObservableCollection<ItemModel> items)
        {
            if (items == null)
            {
                return;
            }
            items.CollectionChanged += ItemsChanged;
        }

        protected virtual void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var data = item as ItemModel;
                    AddItem(data);
                }
            }
        }
        #endregion

        #region selectindex
        public int SelectID
        {
            get { return (int)GetValue(SelectIDProperty); }
            set { SetValue(SelectIDProperty, value); }
        }
        public static readonly DependencyProperty SelectIDProperty =
            DependencyProperty.Register("SelectID", typeof(int), typeof(ItemSelect), new PropertyMetadata(1));
        #endregion
        private WrapPanel container;
        private Dictionary<int, Item> itemControls;
        public ItemSelect()
        {
            DefaultStyleKey = typeof(ItemSelect);
            itemControls = new Dictionary<int, Item>();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = GetTemplateChild("Container") as WrapPanel;
            Render();
        }

        private void Render()
        {
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    AddItem(item);
                }
            }
        }
        private void AddItem(ItemModel data)
        {
            var item = new Item();
            item.Title = data.Title;
            item.Description = data.Description;
            item.Icon = data.Icon;
            item.IsSelected = data.IsSelected;
            item.MouseLeftButtonUp += (e, c) =>
            {
                item.IsSelected = !item.IsSelected;
                Select(data.ID);
            };
            container.Children.Add(item);
            itemControls.Add(data.ID, item);
        }

        private void Select(int id)
        {
            SelectID = id;

            foreach (var item in itemControls)
            {
                item.Value.IsSelected = id == item.Key ? true : false;
            }

            foreach (var item in Items)
            {
                item.IsSelected = id == item.ID ? true : false;
            }

        }


    }
}
