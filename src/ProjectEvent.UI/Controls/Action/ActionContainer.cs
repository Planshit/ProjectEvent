using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionContainer : Control
    {
        protected static PropertyChangedCallback ItemsPropertyChangedCallback = new PropertyChangedCallback(ItemsPropertyChanged);

        public static DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ObservableCollection<BaseActionItemModel>), typeof(ActionContainer), new PropertyMetadata(null, ItemsPropertyChangedCallback));

        private static void ItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (ActionContainer)sender;
            if (control == null)
            {
                return;
            }
            control.UnregisterItems(e.OldValue as ObservableCollection<BaseActionItemModel>);
            control.RegisterItems(e.NewValue as ObservableCollection<BaseActionItemModel>);
        }

        public ObservableCollection<BaseActionItemModel> Items
        {
            get
            {
                return (ObservableCollection<BaseActionItemModel>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        protected void UnregisterItems(ObservableCollection<BaseActionItemModel> items)
        {
            if (items == null)
            {
                return;
            }
            items.CollectionChanged -= ItemsChanged;
        }

        protected void RegisterItems(ObservableCollection<BaseActionItemModel> items)
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
                    var itemAction = item as BaseActionItemModel;
                    AddItem(itemAction);
                }
            }
        }

        private Grid ActionPanel;
        private Point oldPoint;
        private List<ActionItem> actionItems;
        public Command RemoveCommand { get; set; }
        public ActionContainer()
        {
            DefaultStyleKey = typeof(ActionContainer);
            oldPoint = new Point();
            actionItems = new List<ActionItem>();
            RemoveCommand = new Command(new Action<object>(OnRemoveCommand));
        }

        private void OnRemoveCommand(object obj)
        {
            Remove(int.Parse(obj.ToString()));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActionPanel = GetTemplateChild("ActionPanel") as Grid;
            ActionPanel.Height = 100;
            ActionPanel.VerticalAlignment = VerticalAlignment.Top;
        }

        private void Render()
        {
            //if (ActionPanel == null || Actions == null)
            //{
            //    return;
            //}
            //ActionPanel.Children.Clear();
            //ActionPanel.Height = 100;
            //foreach (var action in Actions)
            //{
            //    var item = new ActionItem();
            //    item.Action = action;
            //    item.VerticalAlignment = VerticalAlignment.Top;
            //    item.RenderTransform = new TranslateTransform()
            //    {
            //        X = 0,
            //        Y = 0
            //    };
            //    item.MouseLeftButtonDown += Item_MouseLeftButtonDown;
            //    item.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            //    item.MouseMove += Item_MouseMove;
            //    item.Loaded += (e, c) =>
            //    {
            //        ActionPanel.Height += item.ActualHeight;
            //    };
            //    ActionPanel.Children.Add(item);
            //}


        }

        private void AddItem(BaseActionItemModel action)
        {
            if (ActionPanel.Children.Count == 0)
            {
                AddItem(action, 0);
                return;
            }
            var lastItem = ActionPanel.Children[ActionPanel.Children.Count - 1] as ActionItem;
            var lastItemTTF = lastItem.RenderTransform as TranslateTransform;
            AddItem(action, lastItemTTF.Y + lastItem.ActualHeight + 10);
        }

        private void AddItem(BaseActionItemModel action, double y)
        {
            var item = new ActionItem();
            item.DataContext = this;
            item.ID = ActionPanel.Children.Count + 1;
            item.Action = action;
            item.VerticalAlignment = VerticalAlignment.Top;
            var ttf = item.RenderTransform as TranslateTransform;
            ttf.Y = y;

            item.MouseLeftButtonDown += Item_MouseLeftButtonDown;
            item.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            item.MouseMove += Item_MouseMove;
            item.Loaded += (e, c) =>
            {
                ActionPanel.Height += item.ActualHeight;
                //判断需要下移的项目
                foreach (var actionItem in ActionPanel.Children)
                {
                    var itemControl = actionItem as ActionItem;
                    if (itemControl != item)
                    {
                        var itemControlTTF = (itemControl.RenderTransform as TranslateTransform);
                        if (itemControlTTF.Y >= y)
                        {
                            MoveY(itemControl, itemControlTTF.Y + item.ActualHeight + 10);
                        }
                    }
                }
            };
            ActionPanel.Children.Add(item);
            actionItems.Add(item);
            Sort();
        }

        private void Remove(int id)
        {
            var control = actionItems.Where(m => m.ID == id).FirstOrDefault();
            Remove(control);
        }
        private void Remove(ActionItem actionItem)
        {
            var control = actionItems.Where(m => m.ID == actionItem.ID).FirstOrDefault();
            int index = actionItems.IndexOf(control) + 1;
            if (index < actionItems.Count)
            {
                //上移项目
                for (int i = index; i < actionItems.Count; i++)
                {
                    var itemControl = actionItems[i] as ActionItem;

                    var itemControlTTF = (itemControl.RenderTransform as TranslateTransform);

                    MoveY(itemControl, itemControlTTF.Y - control.ActualHeight);
                }
            }
            ActionPanel.Children.Remove(control);
            actionItems.Remove(control);
        }
        private void Sort()
        {
            actionItems = actionItems.OrderBy(m => m.Y).ToList();
        }

        private void MoveY(ActionItem item, double y, bool soft = true)
        {
            var ttf = item.RenderTransform as TranslateTransform;
            ttf.Y = y;
            if (soft)
            {
                Sort();
            }
        }


        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as ActionItem;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //控件的坐标信息
                var controlPoint = control.RenderTransform as TranslateTransform;
                //当前鼠标坐标信息
                var mousePoint = e.GetPosition(this);
                //鼠标在控件的坐标信息
                var mouseinControlPoint = e.GetPosition(control);


                //最终移动坐标
                double movetoX = e.GetPosition(null).X - oldPoint.X + controlPoint.X;
                double movetoY = e.GetPosition(null).Y - oldPoint.Y + controlPoint.Y;


                if (movetoY >= 0 && movetoY <= ActualHeight)
                {
                    controlPoint.Y = movetoY;
                    oldPoint = e.GetPosition(null);
                }

                //判断上移
                //上一个action索引
                int upItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) - 1;
                if (upItemIndex >= 0)
                {
                    var upItem = actionItems[upItemIndex] as ActionItem;
                    var upItemPoint = upItem.RenderTransform as TranslateTransform;
                    if (movetoY <= upItemPoint.Y + upItem.ActualHeight / 2)
                    {
                        MoveY(upItem, upItemPoint.Y + control.ActualHeight + 10);
                    }
                }
                //判断下移
                int downItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) + 1;
                if (downItemIndex < actionItems.Count)
                {
                    Debug.WriteLine("has down item");
                    var downItem = actionItems[downItemIndex] as ActionItem;
                    var downItemPoint = downItem.RenderTransform as TranslateTransform;

                    if (movetoY + control.ActualHeight >= downItemPoint.Y + downItem.ActualHeight / 2)
                    {
                        Debug.WriteLine("set down item");
                        if (upItemIndex >= 0)
                        {
                            //存在上一个，则移动到上一个的末尾
                            MoveY(downItem, actionItems[upItemIndex].Y + actionItems[upItemIndex].ActualHeight + 10);
                        }
                        else
                        {
                            //不存在则移动到第一位
                            MoveY(downItem, 0);
                        }

                    }
                }
            }

        }

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var control = sender as ActionItem;
            control.ReleaseMouseCapture();
            control.Cursor = Cursors.Arrow;
            control.SetValue(Panel.ZIndexProperty, 0);
            //拖动结束后调整自身位置对齐
            //上一个action索引
            int upItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) - 1;
            if (upItemIndex >= 0)
            {
                var upItem = actionItems[upItemIndex] as ActionItem;
                var upItemPoint = upItem.RenderTransform as TranslateTransform;
                MoveY(control, upItemPoint.Y + upItem.ActualHeight + 10);
            }
            else
            {
                //没有上一个了
                MoveY(control, 0);
            }
        }

        private void Item_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var control = sender as ActionItem;

            if (control != null)
            {
                oldPoint = e.GetPosition(null);

                control.CaptureMouse();
                control.Cursor = Cursors.SizeAll;
                control.SetValue(Panel.ZIndexProperty, 1);
            }
        }
    }
}
