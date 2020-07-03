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
        public static event ActionInvokeHandler OnActionInvoke;
        private static CancellationTokenSource testcts;
        public static void StopTestInvokeAction()
        {
            testcts.Cancel();
        }
        public static void RunTestInvokeAction(IEnumerable<ActionModel> actions)
        {
            testcts = new CancellationTokenSource();
            //执行actions
            InvokeAction(17290302, actions, true);
        }
        public static void InvokeAction(int taskID, IEnumerable<ActionModel> actions, bool isTest = false)
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
            var task = new Task(() =>
            {
                try
                {
                    Debug.WriteLine("进入");
                    ct.ThrowIfCancellationRequested();
                    foreach (var action in actions)
                    {

                        GetAction(taskID, action).Invoke();
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }

                        //var actionBuilder = new ActionBuilder(action.Action, action.Parameter);
                        //var actionTask = actionBuilder.Builer(taskID, action.ID);
                        //OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Runing);

                        //if (actionTask != null)
                        //{
                        //    Debug.WriteLine(actionTask.Status);
                        //    actionTask.Start();
                        //    var actionResult = actionTask.Result;
                        //    if (actionBuilder.IsHasResult())
                        //    {
                        //        ActionTaskResulter.Add(taskID, actionResult);
                        //    }
                        //    OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Success);

                        //}
                        //else
                        //{
                        //    OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Failed);
                        //    LogHelper.Warning($"找不到Action Type：{action.Action}");
                        //}

                        //if (ct.IsCancellationRequested)
                        //{
                        //    cts.Dispose();
                        //}

                    }
                }
                catch (OperationCanceledException e)
                {
                    Debug.WriteLine("OperationCanceledException:" + e.Message);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception:" + e.Message);
                }
            }, ct);

            task.Start();
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
