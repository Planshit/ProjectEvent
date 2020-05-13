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
        #region 依赖属性
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

        #endregion

        public event EventHandler ItemIndexChanged;
        private Grid ActionPanel;
        private Point oldPoint;
        private Button AddActionBtn;
        //控件列表
        private List<ActionItem> actionItems;
        private Timer moveTimer;
        private List<ActionItemModel> appendList;
        private bool isRendering;
        public Command RemoveCommand { get; set; }
        private double oldMoveItemY;
        private bool isClick = false;
        //当前移动的if action items
        private List<ActionItem> ifActionItems;
        public ActionContainer()
        {
            DefaultStyleKey = typeof(ActionContainer);
            oldPoint = new Point();
            actionItems = new List<ActionItem>();
            appendList = new List<ActionItemModel>();
            ifActionItems = new List<ActionItem>();
            RemoveCommand = new Command(new Action<object>(OnRemoveCommand));
            moveTimer = new Timer();
            moveTimer.Interval = 500;
            moveTimer.Elapsed += MoveTimer_Elapsed;
            isRendering = false;
        }

        private void MoveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            moveTimer.Stop();
        }

        private void OnRemoveCommand(object obj)
        {
            //Remove(int.Parse(obj.ToString()));
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
            AddItemControl(action);
        }

        private void AddItemControl(ActionItemModel action)
        {
            isRendering = true;
            double Y = 0;

            if (actionItems.Count > 0)
            {
                var lastItem = actionItems.Last();
                var lastItemTTF = lastItem.RenderTransform as TranslateTransform;
                Y = lastItemTTF.Y + lastItem.ActualHeight;
            }

            var item = new ActionItem();
            item.DataContext = this;
            item.ID = action.ID;
            item.Action = action;
            item.VerticalAlignment = VerticalAlignment.Top;
            var ttf = item.RenderTransform as TranslateTransform;
            ttf.Y = Y;
            item.Y = Y;
            item.MouseLeftButtonDown += Item_MouseLeftButtonDown;
            item.MouseLeftButtonUp += Item_MouseLeftButtonUp;
            item.MouseMove += Item_MouseMove;
            item.Loaded += (e, c) =>
            {
                if (item.Tag != null)
                {
                    //如果tag不为null表示已加载完成了
                    return;
                }
                item.Tag = string.Empty;
                ActionPanel.Height += item.ActualHeight;
                if (double.IsNaN(ActionPanel.Height))
                {
                    ActionPanel.Height = item.ActualHeight;
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
            SortAction();
        }

        private void SortAction()
        {
            actionItems = actionItems.OrderBy(m => m.Y).ToList();
            for (int i = 0; i < actionItems.Count; i++)
            {
                var item = Items.Where(m => m.ID == actionItems[i].ID).FirstOrDefault();
                if (item != null)
                {
                    item.Index = i;
                }
                actionItems[i].Action.Index = i;
            }
            ItemIndexChanged?.Invoke(this, null);
        }


        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            var control = sender as ActionItem;
            if (isClick && e.LeftButton == MouseButtonState.Pressed)
            {
                ////Debug.WriteLine(1);
                //Debug.WriteLine("oldY:" + oldPoint.Y + ",nowY:" + e.GetPosition(null).Y);
                ////return;
                if (oldPoint.Y == e.GetPosition(null).Y)
                {
                    return;
                }
                //控件的坐标信息
                var controlPoint = control.RenderTransform as TranslateTransform;
                //最终移动坐标
                double movetoY = e.GetPosition(null).Y - oldPoint.Y + controlPoint.Y;
                //判断是否在可移动区域
                if (movetoY >= 0 && movetoY <= ActualHeight)
                {
                    controlPoint.Y = movetoY;
                    oldPoint = e.GetPosition(null);
                    HandleActionMove(control, movetoY);
                }
            }
        }

        private void HandleActionMove(ActionItem action, double moveY)
        {
            bool moveUp = false;
            if (oldMoveItemY > moveY)
            {
                //向上拖动
                moveUp = true;
            }

            int itemIndex = moveUp ? action.Action.Index - 1 : action.Action.Index + 1;
            var item = actionItems.Where(a => a.Action.Index == itemIndex).SingleOrDefault();
            if (item != null)
            {
                var itemPoint = item.RenderTransform as TranslateTransform;
                var actionPoint = action.RenderTransform as TranslateTransform;
                if (moveUp)
                {
                    if (actionPoint.Y <= itemPoint.Y + item.ActualHeight / 2)
                    {
                        //移动超出了一半的范围则可以调整顺序
                        itemPoint.Y = itemPoint.Y + action.ActualHeight;
                        SortAction();
                    }
                }
                else
                {
                    //往下
                    if (actionPoint.Y + action.ActualHeight >= itemPoint.Y + item.ActualHeight / 2)
                    {
                        //移动超出了一半的范围则可以调整顺序
                        itemPoint.Y = itemPoint.Y - action.ActualHeight;
                        SortAction();
                    }
                }

            }
        }
        private void HandleMoveEnd(ActionItem action)
        {
            var item = actionItems.Where(a => a.Action.Index == action.Action.Index - 1).SingleOrDefault();
            var actionPoint = action.RenderTransform as TranslateTransform;
            if (item != null)
            {
                var itemPoint = item.RenderTransform as TranslateTransform;
                actionPoint.Y = itemPoint.Y + item.ActualHeight;
            }
            else
            {
                actionPoint.Y = 0;
            }

            
        }
        private void HandleMoveStart(ActionItem action)
        {
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                int endIndex = actionItems.Where(m => m.Action.ParentID == action.Action.ID).LastOrDefault().Action.Index;
                for (int i = action.Action.Index + 1; i < endIndex + 1; i++)
                {
                    actionItems[i].Visibility = Visibility.Hidden;
                    ifActionItems.Add(actionItems[i]);
                }
            }
        }
        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isClick = false;
            var control = sender as ActionItem;
            control.ReleaseMouseCapture();
            control.Cursor = Cursors.Arrow;
            control.SetValue(Panel.ZIndexProperty, 0);
            HandleMoveEnd(control);
        }

        private void Item_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var control = sender as ActionItem;
            if (control.Action.ActionType != UI.Types.ActionType.IFElse &&
                control.Action.ActionType != UI.Types.ActionType.IFEnd)
            {
                isClick = true;
                oldPoint = e.GetPosition(null);
                //Debug.WriteLine(oldPoint.Y);

                control.CaptureMouse();
                control.Cursor = Cursors.SizeAll;
                control.SetValue(Panel.ZIndexProperty, 1);

                oldMoveItemY = (control.RenderTransform as TranslateTransform).Y;
            }
        }
    }
}
