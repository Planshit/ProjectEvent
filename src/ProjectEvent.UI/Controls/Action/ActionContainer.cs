using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionContainer : Control
    {
        public ICommand AddActionCommand
        {
            get { return (ICommand)GetValue(AddActionCommandProperty); }
            set { SetValue(AddActionCommandProperty, value); }
        }
        public static readonly DependencyProperty AddActionCommandProperty =
            DependencyProperty.Register("AddActionCommand",
                typeof(ICommand),
                typeof(ActionContainer));

        protected static PropertyChangedCallback ItemsPropertyChangedCallback = new PropertyChangedCallback(ItemsPropertyChanged);

        public static DependencyProperty ItemsProperty = DependencyProperty.RegisterAttached("Items", typeof(ObservableCollection<ActionItemModel>), typeof(ActionContainer), new PropertyMetadata(null, ItemsPropertyChangedCallback));

        private static void ItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (ActionContainer)sender;
            if (control == null)
            {
                return;
            }
            control.UnregisterItems(e.OldValue as ObservableCollection<ActionItemModel>);
            control.RegisterItems(e.NewValue as ObservableCollection<ActionItemModel>);
        }

        public ObservableCollection<ActionItemModel> Items
        {
            get
            {
                return (ObservableCollection<ActionItemModel>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        protected void UnregisterItems(ObservableCollection<ActionItemModel> items)
        {
            if (items == null)
            {
                return;
            }
            items.CollectionChanged -= ItemsChanged;
        }

        protected void RegisterItems(ObservableCollection<ActionItemModel> items)
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
                    var itemAction = item as ActionItemModel;
                    AddItem(itemAction);
                }
            }
        }
        public event EventHandler ItemIndexChanged;
        private Grid ActionPanel;
        private Point oldPoint;
        private Button AddActionBtn;

        private double oldMoveItemY;
        private List<ActionItem> actionItems;
        private bool isCanMove;
        private Timer moveTimer;
        private List<ActionItemModel> appendList;
        private bool isRendering;
        public Command RemoveCommand { get; set; }

        public ActionContainer()
        {
            DefaultStyleKey = typeof(ActionContainer);
            oldPoint = new Point();
            actionItems = new List<ActionItem>();
            appendList = new List<ActionItemModel>();
            RemoveCommand = new Command(new Action<object>(OnRemoveCommand));
            moveTimer = new Timer();
            moveTimer.Interval = 500;
            moveTimer.Elapsed += MoveTimer_Elapsed;
            isCanMove = false;
            isRendering = false;
        }

        private void MoveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            isCanMove = true;
            moveTimer.Stop();
        }

        private void OnRemoveCommand(object obj)
        {
            Remove(int.Parse(obj.ToString()));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActionPanel = GetTemplateChild("ActionPanel") as Grid;
            AddActionBtn = GetTemplateChild("AddActionBtn") as Button;

            //ActionPanel.Height = 0;
            ActionPanel.VerticalAlignment = VerticalAlignment.Top;

            AddActionBtn.Command = AddActionCommand;
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

        private void AddItem(ActionItemModel action)
        {
            if (!appendList.Contains(action))
            {
                appendList.Add(action);
            }

            if (isRendering)
            {
                return;
            }

            if (ActionPanel.Children.Count == 0)
            {
                AddItem(action, 0);
                return;
            }
            var lastItem = ActionPanel.Children[ActionPanel.Children.Count - 1] as ActionItem;
            var lastItemTTF = lastItem.RenderTransform as TranslateTransform;
            AddItem(action, lastItemTTF.Y + lastItem.ActualHeight);
        }

        private void AddItem(ActionItemModel action, double y)
        {
            isRendering = true;
            var item = new ActionItem();
            item.DataContext = this;
            item.ID = action.ID;
            item.Action = action;
            item.VerticalAlignment = VerticalAlignment.Top;
            var ttf = item.RenderTransform as TranslateTransform;
            ttf.Y = y;

            item.MouseLeftButtonDown += Item_MouseLeftButtonDown;
            item.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            item.MouseMove += Item_MouseMove;
            item.Loaded += (e, c) =>
            {
                if (item.Tag != null && (bool)item.Tag == true)
                {
                    //如果tag为true表示已加载完成了
                    return;
                }
                item.Tag = true;
                ActionPanel.Height += item.ActualHeight;
                if (double.IsNaN(ActionPanel.Height))
                {
                    ActionPanel.Height = item.ActualHeight;
                }
                //判断需要下移的项目
                foreach (var actionItem in ActionPanel.Children)
                {
                    var itemControl = actionItem as ActionItem;
                    if (itemControl != item)
                    {
                        var itemControlTTF = (itemControl.RenderTransform as TranslateTransform);
                        if (itemControlTTF.Y >= y)
                        {
                            MoveY(itemControl, itemControlTTF.Y + item.ActualHeight);
                        }
                    }
                }
                //继续队列
                appendList.Remove(action);
                isRendering = false;
                if (appendList.Count > 0)
                {
                    AddItem(appendList[0]);
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
            ActionPanel.Height -= control.ActualHeight;
            if (actionItems.Count == 1)
            {
                ActionPanel.Height = double.NaN;
            }
            ActionPanel.Children.Remove(control);
            actionItems.Remove(control);
        }
        private void Sort()
        {
            actionItems = actionItems.OrderBy(m => m.Y).ToList();
            for (int i = 0; i < actionItems.Count; i++)
            {
                var item = Items.Where(m => m.ID == actionItems[i].ID).FirstOrDefault();
                if (item != null)
                {
                    item.Index = i;
                }
            }
            ItemIndexChanged?.Invoke(this, null);
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

        private void HandleIFActionMoveState(ActionItem control, bool isshow)
        {
            //if action
            if (control.Action.ActionType == UI.Types.ActionType.IF)
            {
                //if action需要移动包含的全部action
                int startIndex = control.Action.Index;
                int endIndex = Items.Where(m => m.ParentID == control.Action.ID).LastOrDefault().Index;
                var allActions = Items.Where(m => m.Index > startIndex && m.Index <= endIndex);
                foreach (var action in allActions)
                {
                    actionItems[action.Index].Visibility = isshow ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }
        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as ActionItem;
            if (control.Action.ActionType == UI.Types.ActionType.IFElse ||
                control.Action.ActionType == UI.Types.ActionType.IFEnd)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed && isCanMove)
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

                HandleIFActionMoveState(control, false);
                //是否上移
                bool isUp = false;
                if (movetoY >= 0 && movetoY <= ActualHeight)
                {
                    controlPoint.Y = movetoY;
                    oldPoint = e.GetPosition(null);
                    if (oldMoveItemY > movetoY)
                    {
                        isUp = true;
                    }

                    //上一个action索引
                    int upItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) - 1;

                    if (isUp)
                    {
                        //判断上移
                        if (upItemIndex >= 0)
                        {
                            var upItem = actionItems[upItemIndex] as ActionItem;
                            var upItemPoint = upItem.RenderTransform as TranslateTransform;
                            if (movetoY <= upItemPoint.Y + upItem.ActualHeight / 2)
                            {
                                MoveY(upItem, upItemPoint.Y + control.ActualHeight);
                                //MoveY(upItem, upItemPoint.Y + control.ActualHeight);
                            }
                        }
                    }
                    else
                    {
                        //判断下移
                        //int downItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) + 1;
                        int downItemIndex = actionItems.Where(m => m.ID == control.ID).FirstOrDefault().Action.Index + 1;
                        if (control.Action.ActionType == UI.Types.ActionType.IF)
                        {
                            downItemIndex = actionItems.Where(m => m.Action.ParentID == control.ID).LastOrDefault().Action.Index + 1;
                        }
                        if (downItemIndex < actionItems.Count)
                        {
                            //Debug.WriteLine("has down item");
                            var downItem = actionItems[downItemIndex] as ActionItem;
                            var downItemPoint = downItem.RenderTransform as TranslateTransform;

                            if (movetoY + control.ActualHeight >= downItemPoint.Y + downItem.ActualHeight / 2)
                            {
                                //Debug.WriteLine("set down item");
                                if (upItemIndex >= 0)
                                {
                                    //存在上一个，则移动到上一个的末尾
                                    MoveY(downItem, actionItems[upItemIndex].Y + actionItems[upItemIndex].ActualHeight);
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


            }

        }

        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var control = sender as ActionItem;

            if (control.Action.ActionType == UI.Types.ActionType.IFElse ||
                control.Action.ActionType == UI.Types.ActionType.IFEnd)
            {
                return;
            }

            isCanMove = false;
            moveTimer.Stop();


            control.ReleaseMouseCapture();
            control.Cursor = Cursors.Arrow;
            control.SetValue(Panel.ZIndexProperty, 0);

            var newMargin = new Thickness(0, 0, 0, 0);
            //拖动结束后调整自身位置对齐
            //上一个action索引
            int upItemIndex = actionItems.IndexOf(actionItems.Where(m => m.ID == control.ID).FirstOrDefault()) - 1;
            if (upItemIndex >= 0)
            {
                var upItem = actionItems[upItemIndex] as ActionItem;
                var upItemPoint = upItem.RenderTransform as TranslateTransform;
                MoveY(control, upItemPoint.Y + upItem.ActualHeight);
                if (upItem.Action.ActionType == UI.Types.ActionType.IF ||
                    upItem.Action.ActionType == UI.Types.ActionType.IFElse)
                {
                    newMargin = new Thickness(10, 0, 10, 0);

                }
            }
            else
            {
                //没有上一个了
                MoveY(control, 0);
            }
            control.Margin = newMargin;
            HandleIFActionMoveState(control, true);
        }

        private void Item_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var control = sender as ActionItem;
            if (control.Action.ActionType == UI.Types.ActionType.IFElse ||
                control.Action.ActionType == UI.Types.ActionType.IFEnd)
            {
                return;
            }

            if (control != null)
            {
                oldPoint = e.GetPosition(null);

                control.CaptureMouse();
                control.Cursor = Cursors.SizeAll;
                control.SetValue(Panel.ZIndexProperty, 1);
                oldMoveItemY = (control.RenderTransform as TranslateTransform).Y;
                moveTimer.Start();
            }
        }
    }
}
