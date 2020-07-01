using ProjectEvent.UI.Controls.ItemSelect.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
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
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var data = item as ItemModel;
                    Remove(data.ID);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                itemControls.Clear();
                container.Children.Clear();
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
            DependencyProperty.Register("SelectID", typeof(int), typeof(ItemSelect), new PropertyMetadata(1, new PropertyChangedCallback(OnSelectIDChanged)));

        private static void OnSelectIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ItemSelect)d;
            control.Select((int)e.NewValue);
        }
        #endregion
        public ItemModel SelectItem
        {
            get { return (ItemModel)GetValue(SelectItemProperty); }
            set { SetValue(SelectItemProperty, value); }
        }
        public static readonly DependencyProperty SelectItemProperty =
            DependencyProperty.Register("SelectItem", typeof(ItemModel), typeof(ItemSelect));
        public ContextMenu ItemContextMenu
        {
            get { return (ContextMenu)GetValue(ItemContextMenuProperty); }
            set { SetValue(ItemContextMenuProperty, value); }
        }
        public static readonly DependencyProperty ItemContextMenuProperty =
            DependencyProperty.Register("ItemContextMenu", typeof(ContextMenu), typeof(ItemSelect));

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
            item.ID = data.ID;
            item.Description = data.Description;
            item.Icon = data.Icon;
            item.Color = data.Color;
            item.Tag = data.Tag;
            item.IsSelected = data.IsSelected;
            item.MouseLeftButtonUp += (e, c) =>
            {
                item.IsSelected = !item.IsSelected;
                Select(data.ID);
            };
            item.MouseRightButtonUp += Item_MouseRightButtonUp;
            container.Children.Add(item);
            itemControls.Add(data.ID, item);
        }

        private void Item_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as Item;
            SelectItem = Items.Where(m => m.ID == item.ID).FirstOrDefault();
            if (ItemContextMenu != null)
            {
                ItemContextMenu.IsOpen = true;
            }
        }
        private void Remove(int id)
        {
            container.Children.Remove(itemControls[id]);
            itemControls.Remove(id);
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
