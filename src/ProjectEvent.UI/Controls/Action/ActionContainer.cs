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
        /// <summary>
        /// 等待添加入容器的控件列表
        /// </summary>
        private List<ActionItemModel> appendList;
        private bool isRendering;
        public Command RemoveCommand { get; set; }
        private double oldMoveItemY;
        private bool isClick = false;
        /// <summary>
        /// 当前移动的if action items暂存区
        /// </summary>
        private List<ActionItem> ifActionItems;
        public ActionContainer()
        {
            DefaultStyleKey = typeof(ActionContainer);
            oldPoint = new Point();
            actionItems = new List<ActionItem>();
            appendList = new List<ActionItemModel>();
            ifActionItems = new List<ActionItem>();
            RemoveCommand = new Command(new Action<object>(OnRemoveCommand));

            isRendering = false;
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

        #region 添加控件
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
            ttf.Changed += (e, c) =>
            {
                item.Y = ttf.Y;
            };
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
        #endregion

        #region 移除控件
        private void Remove(int id)
        {
            var action = actionItems.Where(m => m.Action.ID == id).FirstOrDefault();
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                var actions = actionItems.Where(m => m.Action.ID == id || m.Action.ParentID == id).ToList();
                foreach (var item in actions)
                {
                    Remove(item);
                }
            }
            else
            {
                Remove(action);
            }
        }
        private void Remove(ActionItem item)
        {
            ActionPanel.Children.Remove(item);
            actionItems.Remove(item);
            SortAction();
            UpdateActionsLocation();
        }
        #endregion

        #region 排序
        /// <summary>
        /// 根据action位置排序分配index
        /// </summary>
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
        #endregion

        #region 正在移动中
        private void HandleActionMove(ActionItem action, double moveY)
        {
            bool moveUp = false;
            //更新自身Y
            action.Y = moveY;

            //查找相邻的action
            var items = actionItems.Where(m => m.Y > moveY).OrderBy(m => m.Y);
            //相邻的action
            ActionItem item = null;
            //移动的距离
            double moveLength = action.ActualHeight;
            //if (action.Action.ActionType == UI.Types.ActionType.IF)
            //{
            //    moveLength += ifActionItems.Sum(m => m.ActualHeight);
            //}
            if (oldMoveItemY > moveY)
            {
                //向上拖动
                moveUp = true;
                //向上拖动，查找小于移动位置的action
                items = actionItems.Where(m => m.Y < moveY).OrderBy(m => m.Y);
            }
            if (items.Count() > 0)
            {
                item = moveUp ? items.Last() : items.First();
            }


            if (moveUp)
            {
                //向上拖动，查找小于移动位置的action
                if (item != null)
                {
                    //存在时判断位置是否需要调换
                    if (moveY < item.Y + item.ActualHeight / 2)
                    {
                        //需要
                        (item.RenderTransform as TranslateTransform).Y += moveLength;
                        //更新Y
                        item.Y = (item.RenderTransform as TranslateTransform).Y;
                    }
                }
            }
            else
            {
                //向下拖动
                if (item != null)
                {

                    //存在时判断位置是否需要调换
                    if (moveY + action.ActualHeight > item.Y + item.ActualHeight / 2)
                    {
                        //需要
                        (item.RenderTransform as TranslateTransform).Y -= moveLength;

                        //更新Y
                        item.Y = (item.RenderTransform as TranslateTransform).Y;
                        if (actionItems.Where(m => m.Y < item.Y && m.Visibility != Visibility.Hidden && m != item).ToList().Count() == 0)
                        {
                            (item.RenderTransform as TranslateTransform).Y = 0;
                            item.Y = 0;
                        }

                    }
                }
            }


            //更新
            oldMoveItemY = moveY;

        }
        #endregion

        #region 移动结束（鼠标释放）
        private void HandleMoveEnd(ActionItem action)
        {
            var actionPoint = action.RenderTransform as TranslateTransform;
            //调整控件自身的位置对齐

            //查找当前控件的上一个控件
            var topActionItems = actionItems.
                Where(
                m => m.Visibility != Visibility.Hidden &&
                m.Y < actionPoint.Y
                ).
                OrderBy(m => m.Y);
            ActionItem topActionItem = null;
            if (topActionItems.Count() > 0)
            {
                //存在上一个时
                topActionItem = topActionItems.Last();
                //调整自身对齐上一个
                actionPoint.Y = topActionItem.Y + topActionItem.ActualHeight;



                //判断上一个是否是判断action
                if (topActionItem.Action.ActionType == UI.Types.ActionType.IF ||
                    topActionItem.Action.ActionType == UI.Types.ActionType.IFElse ||
                    topActionItem.Action.ParentID > 0 &&
                    topActionItem.Action.ActionType != UI.Types.ActionType.IFEnd)
                {
                    //是的话需要将自身设置为if的下级
                    action.Action.ParentID = topActionItem.Action.ActionType == UI.Types.ActionType.IF ? topActionItem.Action.ID : topActionItem.Action.ParentID;

                    //设置间距
                    double margin = topActionItem.Action.ActionType == UI.Types.ActionType.IF || topActionItem.Action.ActionType == UI.Types.ActionType.IFElse ? topActionItem.Margin.Left + 10 : topActionItem.Margin.Left;
                    action.Margin = new Thickness(margin, 0, margin, 0);

                }
                else
                {
                    //重置父级id
                    action.Action.ParentID = 0;
                    action.Margin = new Thickness(0);
                }
            }
            else
            {
                //不存在说明已经是第一个了
                actionPoint.Y = 0;
                //重置父级id
                action.Action.ParentID = 0;
                action.Margin = new Thickness(0);
            }

            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                //查找if action下方的action
                var bottomActions = actionItems.Where(m => m.Y > actionPoint.Y && m.Visibility != Visibility.Hidden).OrderBy(m => m.Y).ToList();

                //调整子级的位置
                for (int i = 0; i < ifActionItems.Count; i++)
                {
                    var actionItem = ifActionItems[i];
                    var actionItemPoint = actionItem.RenderTransform as TranslateTransform;

                    double newY = 0;
                    if (i == 0)
                    {
                        //第一个跟随移动的action
                        newY = actionPoint.Y + action.ActualHeight;
                    }
                    else
                    {
                        //跟随前一个action
                        var lastItem = ifActionItems[i - 1];
                        var lastItemPoint = lastItem.RenderTransform as TranslateTransform;
                        newY = lastItemPoint.Y + lastItem.ActualHeight;
                    }

                    actionItemPoint.Y = newY;
                    //恢复显示
                    actionItem.Visibility = Visibility.Visible;
                }
                if (bottomActions.Count > 0)
                {
                    for (int i = 0; i < bottomActions.Count; i++)
                    {
                        var actionTTF = bottomActions[i].RenderTransform as TranslateTransform;
                        if (i == 0)
                        {
                            actionTTF.Y = ifActionItems[ifActionItems.Count - 1].Y + ifActionItems[ifActionItems.Count - 1].ActualHeight;
                        }
                        else
                        {
                            var lastactionTTF = bottomActions[i - 1].RenderTransform as TranslateTransform;
                            actionTTF.Y = lastactionTTF.Y + bottomActions[i - 1].ActualHeight;
                        }
                        bottomActions[i].Y = actionTTF.Y;
                    }
                }
                ifActionItems.Clear();

                //调整间距
                ResetIfActionMargin(action);
            }

            //排序
            SortAction();
        }

        #endregion

        #region 移动开始（鼠标点击）
        private void HandleMoveStart(ActionItem action)
        {
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                //移动的是if action时需要将它的子级全部隐藏
                int start = action.Action.Index + 1;
                int end = actionItems.Where(m => m.Action.ParentID == action.Action.ID).Last().Action.Index + 1;
                for (int i = start; i < end; i++)
                {
                    actionItems[i].Visibility = Visibility.Hidden;
                    actionItems[i].Margin = new Thickness(0);
                    ifActionItems.Add(actionItems[i]);
                }
                var bottomItems = actionItems.Where(m => m.Y > action.Y && m.Visibility != Visibility.Hidden).ToList();
                for (int i = 0; i < bottomItems.Count; i++)
                {
                    var item = bottomItems[i];
                    var itemPoint = item.RenderTransform as TranslateTransform;
                    if (i == 0)
                    {
                        itemPoint.Y = action.Y + action.ActualHeight;
                    }
                    else
                    {
                        itemPoint.Y = bottomItems[i - 1].Y + bottomItems[i - 1].ActualHeight;
                    }
                    item.Y = itemPoint.Y;
                }
            }
        }
        #endregion

        #region 调整ifaction的间距
        /// <summary>
        /// 调整ifaction的间距
        /// </summary>
        /// <param name="action"></param>
        private void ResetIfActionMargin(ActionItem action)
        {
            var actionPoint = action.RenderTransform as TranslateTransform;
            //查找当前控件的上一个控件
            var topActionItems = actionItems.
                Where(
                m => m.Visibility != Visibility.Hidden &&
                m.Y < actionPoint.Y
                ).
                OrderBy(m => m.Y);
            ActionItem topActionItem = null;
            double addMargin = 0;
            if (topActionItems.Count() > 0)
            {
                //存在上一个时
                topActionItem = topActionItems.Last();
                if (topActionItem.Action.ActionType == UI.Types.ActionType.IF ||
                    topActionItem.Action.ParentID > 0 &&
                    topActionItem.Action.ActionType != UI.Types.ActionType.IFEnd)
                {
                    //上一个action还是属于if子级
                    //addMargin = topActionItem.Margin.Left + 10;
                    addMargin = topActionItem.Action.ActionType == UI.Types.ActionType.IF || topActionItem.Action.ParentID != action.Action.ID ? topActionItem.Margin.Left + 10 : 10;
                }
            }


            //调整间距
            var ifactions = actionItems.Where(m => m.Action.ID == action.Action.ID || m.Action.ParentID == action.Action.ID).OrderBy(m => m.Y).ToList();
            foreach (var ifaction in ifactions)
            {
                var newmarigin = addMargin;
                if (ifaction.Action.ActionType != UI.Types.ActionType.IF &&
                    ifaction.Action.ActionType != UI.Types.ActionType.IFElse &&
                    ifaction.Action.ActionType != UI.Types.ActionType.IFEnd)
                {
                    newmarigin = addMargin + 10;
                }
                ifaction.Margin = new Thickness(newmarigin, 0, newmarigin, 0);
                if (ifaction.Action.ActionType == UI.Types.ActionType.IF && ifaction != action)
                {
                    ResetIfActionMargin(ifaction);
                }
            }
        }
        #endregion

        #region 调整所有action位置
        private void UpdateActionsLocation()
        {
            var actions = actionItems.OrderBy(m => m.Y).ToList();

            for (int i = 0; i < actions.Count; i++)
            {
                var actionPoint = actions[i].RenderTransform as TranslateTransform;
                if (i == 0)
                {
                    actionPoint.Y = 0;
                    actions[i].Y = 0;
                }
                else
                {
                    var topAction = actions[i - 1];
                    actionPoint.Y = topAction.Y + topAction.ActualHeight;
                    actions[i].Y = actionPoint.Y;
                }
            }

        }
        #endregion

        #region 鼠标事件
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
                //if (movetoY >= 0 && movetoY <= ActualHeight)
                //{
                controlPoint.Y = movetoY;
                oldPoint = e.GetPosition(null);
                HandleActionMove(control, movetoY);
                //}
            }
        }
        private void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isClick)
            {
                isClick = false;
                var control = sender as ActionItem;
                control.ReleaseMouseCapture();
                control.Cursor = Cursors.Arrow;
                control.SetValue(Panel.ZIndexProperty, 0);
                HandleMoveEnd(control);
            }
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
                HandleMoveStart(control);
            }
        }
        #endregion

        
    }
}
