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
        public static event ActionsInvokeHandler OnActionsState;
        public const int TestTaskID = 17290302;
        private static CancellationTokenSource testcts;
        public static void StopTestInvokeAction()
        {
            OnActionsState?.Invoke(TestTaskID, ActionInvokeStateType.Busy);
            testcts.Cancel();
        }
        public static void RunTestInvokeAction(IEnumerable<ActionModel> actions)
        {
            testcts = new CancellationTokenSource();
            //执行actions
            Invoke(TestTaskID, actions, true);
        }
        public static void Invoke(int taskID, IEnumerable<ActionModel> actions, bool isTest = false, bool isChildren = false)
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
                        OnActionsState?.Invoke(taskID, ActionInvokeStateType.Runing);
                        InvokeActions(taskID, actions, ct);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception e)
                    {
                        LogHelper.Error(e.ToString());
                    }
                    finally
                    {
                        OnActionsState?.Invoke(taskID, ActionInvokeStateType.Done);
                        cts.Dispose();
                    }
                }, ct);

                task.Start();
            }

        }

        private static void InvokeActions(int taskID, IEnumerable<ActionModel> actions, CancellationToken ct)
        {
            //OnActionsState?.Invoke(taskID, ActionInvokeStateType.Runing);
            ct.ThrowIfCancellationRequested();
            foreach (var action in actions)
            {
                OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Runing);
                GetAction(taskID, action).Invoke();
                if (ct.IsCancellationRequested)
                {
                    OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Done);
                    ct.ThrowIfCancellationRequested();
                }
                OnActionState?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Done);
            }
            //OnActionsState?.Invoke(taskID, ActionInvokeStateType.Done);
        }
        private static System.Action GetAction(int taskID, ActionModel action)
        {
            switch (action.Action)
            {
                case ActionType.WriteFile:
                    return new WriteFileAction().GenerateAction(taskID, action);

                case ActionType.IF:
                    return new IFAction().GenerateAction(taskID, action);
                case ActionType.HttpGet:
                    return new HttpGetAction().GenerateAction(taskID, action);
                case ActionType.Shutdown:
                    return new ShutdownAction().GenerateAction(taskID, action);

                default:
                    return null;
            }
        }
    }
}
