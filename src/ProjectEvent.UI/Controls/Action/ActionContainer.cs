using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net.Types;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
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
        public Core.Action.Types.ActionInvokeStateType State
        {
            get { return (Core.Action.Types.ActionInvokeStateType)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State",
                typeof(Core.Action.Types.ActionInvokeStateType),
                typeof(ActionContainer), new PropertyMetadata(Core.Action.Types.ActionInvokeStateType.Done, new PropertyChangedCallback(OnStateChanged)));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionContainer;
            if (e.NewValue != e.OldValue && control.AddActionBtn != null)
            {
                if (control.State == Core.Action.Types.ActionInvokeStateType.Done)
                {

                    control.AddActionBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    control.AddActionBtn.Visibility = Visibility.Hidden;
                }
            }
        }

        #region 事件类型
        public EventType EventType
        {
            get { return (EventType)GetValue(EventTypeProperty); }
            set { SetValue(EventTypeProperty, value); }
        }
        public static readonly DependencyProperty EventTypeProperty =
            DependencyProperty.Register("EventType",
                typeof(EventType),
                typeof(ActionContainer), new PropertyMetadata(new PropertyChangedCallback(OnEventTypeChanged)));

        private static void OnEventTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionContainer;
            if (e.OldValue != e.NewValue)
            {
                control.EventTypeChanged?.Invoke(control, null);
            }
        }

        #endregion
        #endregion

        public event RoutedEventHandler RenderDone;
        public event EventHandler ItemIndexChanged;
        public event EventHandler EventTypeChanged;
        private Grid ActionPanel;
        private StackPanel ActionTempPanel;
        private Point oldPoint;
        private Button AddActionBtn;
        private int seedID = 0;
        //控件列表
        public List<ActionItem> ActionItems { get; set; }
        /// <summary>
        /// 等待添加入容器的控件列表
        /// </summary>
        private List<ActionItemModel> appendList;
        private List<object> appendInputDataList;

        private bool isRendering;
        public Command RemoveCommand { get; set; }
        private double oldMoveItemY;
        private bool isClick = false;
        /// <summary>
        /// 当前移动的if action items暂存区
        /// </summary>
        private List<ActionItem> ifActionItems;
        /// <summary>
        /// action高度
        /// </summary>
        private Dictionary<int, double> actionsHeightTemp;
        public ActionContainer()
        {
            DefaultStyleKey = typeof(ActionContainer);
            oldPoint = new Point();
            ActionItems = new List<ActionItem>();
            appendList = new List<ActionItemModel>();
            ifActionItems = new List<ActionItem>();
            RemoveCommand = new Command(new Action<object>(OnRemoveCommand));
            appendInputDataList = new List<object>();
            isRendering = false;
            actionsHeightTemp = new Dictionary<int, double>();
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
            ActionTempPanel = GetTemplateChild("ActionTempPanel") as StackPanel;
            ActionPanel.VerticalAlignment = VerticalAlignment.Top;
            AddActionBtn.Command = AddActionCommand;
            RenderDone?.Invoke(this, null);
        }

        #region 添加控件
        public void AddItem(ActionItemModel action, object inputdata = null)
        {
            if (!appendList.Contains(action))
            {
                appendList.Add(action);
                appendInputDataList.Add(inputdata);
            }

            if (isRendering)
            {
                return;
            }
            AddItemControl(action, inputdata);
        }
        private void AddItemControl(ActionItemModel action, object inputdata = null)
        {
            isRendering = true;
            double Y = 0;

            if (ActionItems.Count > 0)
            {
                var lastItem = ActionItems.Last();
                var lastItemTTF = lastItem.RenderTransform as TranslateTransform;
                Y = lastItemTTF.Y + lastItem.ActualHeight;
            }

            var item = new ActionItem();
            item.ActionContainer = this;
            item.DataContext = this;
            item.VMDataContext = this.DataContext;
            BindingOperations.SetBinding(item, ActionItem.ContainerStateProperty, new Binding()
            {
                Source = this,
                Path = new PropertyPath(nameof(State)),
                Mode = BindingMode.Default,
            });
            BindingOperations.SetBinding(item, ActionItem.RuningIDProperty, new Binding()
            {
                Source = DataContext,
                Path = new PropertyPath("RuningActionID"),
                Mode = BindingMode.Default,
            });
            BindingOperations.SetBinding(item, ActionItem.RuningStateProperty, new Binding()
            {
                Source = DataContext,
                Path = new PropertyPath("RuningActionState"),
                Mode = BindingMode.Default,
            });
            item.ID = action.ID;
            item.InputDataModel = inputdata == null ? GetCreateDefaultInputData(action.ActionType) : inputdata;
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

            item.OnRenderDone += (e, c) =>
            {
                if (item.Tag != null)
                {
                    //如果tag不为null表示已加载完成了
                    return;
                }
                item.Tag = string.Empty;
                if (ActionItems.Count == 1)
                {
                    //仅存在一个时使用auto height
                    ActionPanel.Height = Double.NaN;
                }
                else
                {
                    ActionPanel.Height = ActionPanel.ActualHeight + item.ActualHeight;
                }
                //ActionPanel.Height += item.ActualHeight;
                //if (double.IsNaN(ActionPanel.Height))
                //{
                //    ActionPanel.Height = item.ActualHeight;
                //}
                UpdateActionHeight(item);
                ActionTempPanel.Children.Remove(item);
                ActionPanel.Children.Add(item);
                //继续队列
                appendList.RemoveAt(0);
                appendInputDataList.RemoveAt(0);

                isRendering = false;
                if (appendList.Count > 0)
                {
                    AddItem(appendList[0], appendInputDataList[0]);
                }
                else
                {
                    ResetAllActionsMarigin();
                }
                //继续监控item高度变化
                item.LayoutUpdated += (e, v) =>
                {
                    UpdateActionHeight(item);
                };
            };
            //ActionPanel.Children.Add(item);
            //添加到临时容器中
            ActionTempPanel.Children.Add(item);
            ActionItems.Add(item);
            SortAction();
        }

        #region 刷新action高度
        private void UpdateActionHeight(ActionItem item)
        {

            if (!actionsHeightTemp.ContainsKey(item.ID))
            {
                actionsHeightTemp.Add(item.ID, item.ActualHeight);
            }
            else
            {
                if (actionsHeightTemp[item.ID] != item.ActualHeight)
                {

                    //旧高度大于新高度时容器高度也随着变化
                    ActionPanel.Height += (double)(item.ActualHeight - actionsHeightTemp[item.ID]);
                    actionsHeightTemp[item.ID] = item.ActualHeight;
                    //刷新控件位置
                    UpdateActionsLocation();

                }
            }
        }
        #endregion

        private object GetCreateDefaultInputData(UI.Types.ActionType actionType)
        {
            object result = null;
            switch (actionType)
            {
                case UI.Types.ActionType.IF:
                    result = new IFActionInputModel()
                    {
                        Condition = IFActionConditionData.ComBoxData[0]
                    };

                    break;
                case UI.Types.ActionType.WriteFile:
                    result = new WriteFileActionInputModel()
                    {
                        FilePath = "",
                        Content = ""
                    };
                    break;
                case UI.Types.ActionType.HttpRequest:
                    result = new HttpRequestActionInputModel()
                    {
                        Url = "",
                        PamramsType = HttpRequestActionData.PamramsTypes[0],
                        Method = HttpRequestActionData.MethodTypes[0]
                    };
                    break;
            }
            return result;
        }
        #endregion

        #region 移除控件
        private void Remove(int id)
        {
            var action = ActionItems.Where(m => m.Action.ID == id).FirstOrDefault();
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                var endaction = ActionItems.Where(m => m.Action.ParentID == id && m.Action.ActionType == UI.Types.ActionType.IFEnd).FirstOrDefault();
                var actions = ActionItems.Where(m => m.Y >= action.Y && m.Y <= endaction.Y).ToList();
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
            double newHeight = ActionPanel.Height - item.ActualHeight;
            if (newHeight <= 0)
            {
                ActionPanel.Height = double.NaN;
            }
            else
            {
                ActionPanel.Height = newHeight;
            }
            ActionPanel.Children.Remove(item);
            ActionItems.Remove(item);
            if (ActionItems.Count == 1)
            {
                ActionPanel.Height = Double.NaN;
            }
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
            ActionItems = ActionItems.OrderBy(m => m.Y).ToList();

            for (int i = 0; i < ActionItems.Count; i++)
            {
                //var item = Items.Where(m => m.ID == actionItems[i].ID).FirstOrDefault();
                //if (item != null)
                //{
                //    item.Index = i;
                //}
                ActionItems[i].Action.Index = i;
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
            var items = ActionItems.Where(m => m.Y > moveY).OrderBy(m => m.Y);
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
                items = ActionItems.Where(m => m.Y < moveY).OrderBy(m => m.Y);
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
                        if (ActionItems.Where(m => m.Y < item.Y && m.Visibility != Visibility.Hidden && m != item).ToList().Count() == 0)
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
            AddActionBtn.Visibility = Visibility.Visible;

            var actionPoint = action.RenderTransform as TranslateTransform;
            //调整控件自身的位置对齐

            //查找当前控件的上一个控件
            var topActionItems = ActionItems.
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
                var bottomActions = ActionItems.Where(m => m.Y > actionPoint.Y && m.Visibility != Visibility.Hidden).OrderBy(m => m.Y).ToList();

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
            AddActionBtn.Visibility = Visibility.Hidden;
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                //移动的是if action时需要将它的子级全部隐藏
                int start = action.Action.Index + 1;
                int end = ActionItems.Where(m => m.Action.ParentID == action.Action.ID).Last().Action.Index + 1;
                for (int i = start; i < end; i++)
                {
                    ActionItems[i].Visibility = Visibility.Hidden;
                    ActionItems[i].Margin = new Thickness(0);
                    ifActionItems.Add(ActionItems[i]);
                }
                var bottomItems = ActionItems.Where(m => m.Y > action.Y && m.Visibility != Visibility.Hidden).ToList();
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
            var topActionItems = ActionItems.
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
            var ifactions = ActionItems.Where(m => m.Action.ID == action.Action.ID || m.Action.ParentID == action.Action.ID).OrderBy(m => m.Y).ToList();
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

        #region 调整所有action的间距
        private void ResetAllActionsMarigin()
        {
            foreach (var item in ActionItems)
            {
                if (item.Action.ActionType == UI.Types.ActionType.IF && item.Action.ParentID == 0)
                {
                    ResetIfActionMargin(item);
                }
            }
        }
        #endregion

        #region 调整所有action位置
        private void UpdateActionsLocation()
        {
            var actions = ActionItems.OrderBy(m => m.Y).ToList();

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

        #region 生成actions
        public string GenerateActionsJson()
        {
            return JsonConvert.SerializeObject(GenerateActions());
        }
        public List<Core.Action.Models.ActionModel> GenerateActions()
        {
            if (ActionItems.Count == 0)
            {
                return null;
            }
            SortAction();
            var actions = GenerateActions(ActionItems);
            return actions;
        }
        /// <summary>
        /// 将list中的action生成可执行的actionlist
        /// </summary>
        /// <param name="actionItems"></param>
        private List<Core.Action.Models.ActionModel> GenerateActions(List<ActionItem> actionItems)
        {
            var actions = new List<Core.Action.Models.ActionModel>();
            int ifID = 0;
            foreach (var action in actionItems)
            {
                if (ifID == 0)
                {
                    var actionData = GenerateAction(action);
                    if (actionData != null)
                    {
                        actions.Add(actionData);
                    }
                }
                else
                {
                    if (ifID != action.Action.ParentID)
                    {
                        var actionData = GenerateAction(action);
                        if (actionData != null)
                        {
                            actions.Add(actionData);
                        }
                    }
                }

                if (action.Action.ActionType == UI.Types.ActionType.IF)
                {
                    ifID = action.Action.ID;
                }
            }
            return actions;
        }
        /// <summary>
        /// 生成普通action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private Core.Action.Models.ActionModel GenerateAction(ActionItem action)
        {
            Core.Action.Models.ActionModel result = null;
            if (action.Action.ActionType == UI.Types.ActionType.IF)
            {
                result = GenerateIFAction(action);
            }
            else
            {
                //switch ActionType是UI中使用的Type，与Core使用的ActionType并不一致，这里需要转换
                switch (action.Action.ActionType)
                {
                    case UI.Types.ActionType.WriteFile:
                        result = new Core.Action.Models.ActionModel();
                        var inputdata = action.GetInputData() as WriteFileActionInputModel;
                        result.Action = Core.Action.Types.ActionType.WriteFile;
                        result.ID = action.Action.ID;
                        result.Parameter = new WriteFileActionParameterModel()
                        {
                            FilePath = inputdata.FilePath,
                            Content = inputdata.Content
                        };
                        result.Num = 1;
                        break;
                    case UI.Types.ActionType.HttpRequest:
                        result = new Core.Action.Models.ActionModel();
                        var httprequestInputdata = action.GetInputData() as HttpRequestActionInputModel;
                        result.Action = Core.Action.Types.ActionType.HttpRequest;
                        result.ID = action.Action.ID;
                        result.Parameter = new HttpRequestActionParameterModel()
                        {
                            Url = httprequestInputdata.Url,
                            QueryParams = httprequestInputdata.QueryParams,
                            Method = httprequestInputdata.Method == null ? Core.Net.Types.MethodType.GET : (MethodType)httprequestInputdata.Method.ID,

                            ParamsType = httprequestInputdata.PamramsType == null ? Core.Net.Types.ParamsType.Json : (ParamsType)httprequestInputdata.PamramsType.ID,
                            Files = httprequestInputdata.Files,
                            Headers = httprequestInputdata.Headers
                        };
                        result.Num = 1;
                        break;
                    case UI.Types.ActionType.Shutdown:
                        result = new Core.Action.Models.ActionModel();
                        result.Action = Core.Action.Types.ActionType.Shutdown;
                        result.ID = action.Action.ID;
                        result.Num = 1;
                        break;
                }
            }
            return result;
        }


        /// <summary>
        /// 生成if action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private Core.Action.Models.ActionModel GenerateIFAction(ActionItem action)
        {
            //action input
            var inputdata = action.GetInputData() as IFActionInputModel;

            //else item
            var elseActionItem = ActionItems.Where(m => m.Action.ParentID == action.Action.ID && m.Action.ActionType == UI.Types.ActionType.IFElse).FirstOrDefault();
            //end item
            var endActionItem = ActionItems.Where(m => m.Action.ParentID == action.Action.ID && m.Action.ActionType == UI.Types.ActionType.IFEnd).FirstOrDefault();
            //pass actions
            var passActionItems = ActionItems.Where(m => m.Action.ParentID == action.Action.ID && m.Y > action.Y && m.Y < elseActionItem.Y).ToList();
            var passActions = GenerateActions(passActionItems);

            //unpass actions
            var unpassActionItems = ActionItems.Where(m => m.Action.ParentID == action.Action.ID && m.Y > elseActionItem.Y && m.Y < endActionItem.Y).ToList();
            var unpassActions = GenerateActions(unpassActionItems);

            var result = new Core.Action.Models.ActionModel()
            {
                ID = action.Action.ID,
                Action = Core.Action.Types.ActionType.IF,
                Parameter = new Core.Action.Models.IFActionParameterModel()
                {
                    LeftInput = inputdata.Left,
                    RightInput = inputdata.Right,
                    Condition = inputdata.Condition == null ? Core.Action.Types.IFActionConditionType.Equal : (Core.Action.Types.IFActionConditionType)inputdata.Condition.ID,
                    PassActions = passActions,
                    NoPassActions = unpassActions
                },
                Num = 1
            };
            return result;
        }
        #endregion

        #region 导入actions
        public void ImportActionsJson(string json)
        {
            List<Core.Action.Models.ActionModel> actions = JsonConvert.DeserializeObject<List<Core.Action.Models.ActionModel>>(json);
            ImportActions(actions);
        }
        public void ImportActions(List<Core.Action.Models.ActionModel> actions)
        {
            if (actions == null)
            {
                return;
            }
            //先计算总数
            foreach (var action in actions)
            {
                GetMaxID(action);
            }
            //导入容器
            foreach (var action in actions)
            {
                ImportAction(action);
            }
        }
        private void GetIFChildrenMaxID(Core.Action.Models.ActionModel action)
        {
            //var parameterjobject = action.Parameter as JObject;
            var parameter = ObjectConvert.Get<IFActionParameterModel>(action.Parameter);
            //var parameter = parameterjobject.ToObject<IFActionParameterModel>();
            foreach (var paction in parameter.PassActions.Concat(parameter.NoPassActions))
            {
                GetMaxID(paction);
            }
        }
        private void GetMaxID(Core.Action.Models.ActionModel action)
        {
            if (action.Action == Core.Action.Types.ActionType.IF)
            {
                GetIFChildrenMaxID(action);
            }
            else
            {
                seedID = action.ID > seedID ? action.ID : seedID;
            }
        }

        private void ImportAction(Core.Action.Models.ActionModel action, int parentID = 0)
        {
            if (action.Action == Core.Action.Types.ActionType.IF)
            {
                ImportIFAction(action);
                return;
            }
            ActionItemModel actionModel = null;
            object inputdata = null;
            //switch Core Type，和UI Type并不一致，这里需要转换
            switch (action.Action)
            {
                case Core.Action.Types.ActionType.WriteFile:
                    actionModel = ActionItemsData.Get(UI.Types.ActionType.WriteFile);
                    var writefileParameter = ObjectConvert.Get<WriteFileActionParameterModel>(action.Parameter);
                    if (writefileParameter != null)
                    {
                        inputdata = new WriteFileActionInputModel()
                        {
                            FilePath = writefileParameter.FilePath,
                            Content = writefileParameter.Content
                        };
                    }
                    break;
                case Core.Action.Types.ActionType.HttpRequest:
                    actionModel = ActionItemsData.Get(UI.Types.ActionType.HttpRequest);
                    var httpRequestParameter = ObjectConvert.Get<HttpRequestActionParameterModel>(action.Parameter);
                    if (httpRequestParameter != null)
                    {
                        inputdata = new HttpRequestActionInputModel()
                        {
                            Url = httpRequestParameter.Url,
                            Method = HttpRequestActionData.GetMethodType((int)httpRequestParameter.Method),
                            PamramsType = HttpRequestActionData.GetPamramsType((int)httpRequestParameter.ParamsType),
                            QueryParams = httpRequestParameter.QueryParams,
                            Files = httpRequestParameter.Files,
                            Headers = httpRequestParameter.Headers
                        };
                    }
                    break;
                case Core.Action.Types.ActionType.Shutdown:
                    actionModel = ActionItemsData.Get(UI.Types.ActionType.Shutdown);
                    break;
            }
            if (actionModel == null)
            {
                return;
            }
            actionModel.ID = action.ID;
            actionModel.ParentID = parentID;
            AddItem(actionModel, inputdata);
        }
        private void ImportIFAction(Core.Action.Models.ActionModel action)
        {
            if (action.Action != Core.Action.Types.ActionType.IF)
            {
                return;
            }
            //创建if action
            var ifParameter = ObjectConvert.Get<IFActionParameterModel>(action.Parameter);

            var ifActionModel = ActionItemsData.Get(UI.Types.ActionType.IF);
            ifActionModel.ID = action.ID;
            var ifActionInputData = new IFActionInputModel();
            ifActionInputData.Left = ifParameter.LeftInput;
            ifActionInputData.Right = ifParameter.RightInput;
            ifActionInputData.Condition = IFActionConditionData.GetCombox((int)ifParameter.Condition);
            AddItem(ifActionModel, ifActionInputData);
            //创建pass子级
            if (ifParameter.PassActions.Count > 0)
            {
                foreach (var passaction in ifParameter.PassActions)
                {
                    ImportAction(passaction, ifActionModel.ID);
                }
            }
            //创建else action
            var elseActionModel = ActionItemsData.Get(UI.Types.ActionType.IFElse);
            elseActionModel.ID = GetCreateActionID();
            elseActionModel.ParentID = ifActionModel.ID;
            AddItem(elseActionModel);
            //创建unpass子级
            if (ifParameter.NoPassActions.Count > 0)
            {
                foreach (var passaction in ifParameter.NoPassActions)
                {
                    ImportAction(passaction, ifActionModel.ID);
                }
            }
            //创建end action
            var endActionModel = ActionItemsData.Get(UI.Types.ActionType.IFEnd);
            endActionModel.ID = GetCreateActionID();
            endActionModel.ParentID = ifActionModel.ID;
            AddItem(endActionModel);
        }
        #endregion

        #region 生成action id
        public int GetCreateActionID()
        {
            seedID++;
            return seedID;
        }
        #endregion
    }
}
