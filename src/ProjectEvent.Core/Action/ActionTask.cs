using ProjectEvent.Core.Action.Actions;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Event;
using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action
{
    public static class ActionTask
    {
        /// <summary>
        /// 单个action状态发生改变时发生
        /// </summary>
        public static event ActionInvokeHandler OnActionState;
        /// <summary>
        /// 一组action状态发生改变时发生
        /// </summary>
        public static event ActionsInvokeHandler OnActionGroupState;
        public static int TestTaskID;
        private static CancellationTokenSource testcts;
        public static void StopTestInvokeAction()
        {
            OnActionGroupState?.Invoke(TestTaskID, ActionInvokeStateType.Busy);
            if (testcts.IsCancellationRequested)
            {
                testcts?.Cancel();
            }
        }
        public static void RunTestInvokeAction(List<ActionModel> actions)
        {
            testcts = new CancellationTokenSource();
            TestTaskID = new Random().Next(10000, 999999999);
            Task.Factory.StartNew(() =>
            {
                //执行actions
                Invoke(TestTaskID, actions, true);
            });

        }
        public static void Invoke(int taskID, List<ActionModel> actions, bool isTest = false, bool isChildren = false)
        {
            if (actions == null)
            {
                return;
            }
            CancellationTokenSource cts;
            if (isTest)
            {
                cts = testcts;
            }
            else
            {
                cts = new CancellationTokenSource();
            }

            CancellationToken ct = cts.Token;


            if (isChildren)
            {
                InvokeActions(taskID, actions, ct);
            }
            else
            {
                var task = new Task(() =>
                {
                    try
                    {
                        OnActionGroupState?.Invoke(taskID, ActionInvokeStateType.Runing);
                        InvokeActions(taskID, actions, ct);
                    }
                    catch (OperationCanceledException)
                    {
                        OnActionGroupState?.Invoke(taskID, ActionInvokeStateType.Done);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.ToString());
                    }
                    //finally
                    //{
                    //    cts.Dispose();
                    //}
                }, ct);

                task.Start();
            }

        }

        //private static void InvokeActions(int taskID, List<ActionModel> actions, CancellationToken ct)
        //{
        //    if (actions == null)
        //    {
        //        return;
        //    }
        //    int count = actions.Count();
        //    int runingIndex = 0;
        //    if (count > 0)
        //    {

        //    }

        //    ct.ThrowIfCancellationRequested();
        //    foreach (var action in actions)
        //    {
        //        OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Runing);
        //        GetAction(taskID, action).Invoke();
        //        if (ct.IsCancellationRequested)
        //        {
        //            OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Done);
        //            ct.ThrowIfCancellationRequested();
        //        }
        //        OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Done);
        //    }
        //}
        private static void InvokeActions(int taskID, List<ActionModel> actions, CancellationToken ct, int index = 0)
        {
            if (actions == null || actions.Count == 0)
            {
                return;
            }
            ct.ThrowIfCancellationRequested();
            var actionModel = actions[index];
            var action = GetAction(actionModel);
            action.OnEventStateChanged += (taskID, actionID, state) =>
            {
                if (ct.IsCancellationRequested)
                {
                    OnActionState?.Invoke(taskID, actionID, Types.ActionInvokeStateType.Done);
                    ct.ThrowIfCancellationRequested();
                }
                if (state == ActionInvokeStateType.Done)
                {
                    OnActionState?.Invoke(taskID, actionID, Types.ActionInvokeStateType.Done);
                    if (index + 1 < actions.Count)
                    {
                        InvokeActions(taskID, actions, ct, index + 1);
                    }
                    else
                    {
                        //全部运行完成
                        Debug.WriteLine("全部运行完成");
                        OnActionGroupState?.Invoke(taskID, ActionInvokeStateType.Done);
                    }
                }
            };
            OnActionState?.Invoke(taskID, actionModel.ID, Types.ActionInvokeStateType.Runing);
            action.GenerateAction(taskID, actionModel)?.Invoke();

        }
        private static IAction GetAction(ActionModel action)
        {
            IAction actionInstance = null;

            switch (action.Action)
            {
                case ActionType.WriteFile:
                    actionInstance = new WriteFileAction();
                    break;
                case ActionType.IF:
                    actionInstance = new IFAction();
                    break;
                case ActionType.HttpRequest:
                    actionInstance = new HttpRequestAction();
                    break;
                case ActionType.Shutdown:
                    actionInstance = new ShutdownAction();
                    break;
                case ActionType.StartProcess:
                    actionInstance = new StartProcessAction();
                    break;
                case ActionType.OpenURL:
                    actionInstance = new OpenURLAction();
                    break;
                case ActionType.Snipping:
                    actionInstance = new SnippingAction();
                    break;
                case ActionType.DeleteFile:
                    actionInstance = new DeleteFileAction();
                    break;
                case ActionType.SoundPlay:
                    actionInstance = new SoundPlayAction();
                    break;
                case ActionType.GetIPAddress:
                    actionInstance = new GetIPAddressAction();
                    break;
                case ActionType.Keyboard:
                    actionInstance = new KeyboardAction();
                    break;
                case ActionType.SystemNotification:
                    actionInstance = new SystemNotificationAction();
                    break;
                case ActionType.DownloadFile:
                    actionInstance = new DownloadFileAction();
                    break;
                case ActionType.Dialog:
                    actionInstance = new DialogAction();
                    break;
                case ActionType.Delay:
                    actionInstance = new DelayAction();
                    break;
            }
            if (actionInstance != null)
            {
                return actionInstance;
            }
            return null;
        }
    }
}
