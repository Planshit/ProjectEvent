using ProjectEvent.Core.Action.Models;
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
        private static Thread testThread;
        public static void StopTestInvokeAction()
        {
        }
        public static void RunTestInvokeAction(IEnumerable<ActionModel> actions)
        {
            Debug.WriteLine("start");

            Thread.Sleep(5000);

            //执行actions
            InvokeAction(17290302, actions);

            Debug.WriteLine("end");

        }
        public static void InvokeAction(int taskID, IEnumerable<ActionModel> actions)
        {
            if (actions == null)
            {
                return;
            }
            foreach (var action in actions)
            {
                var actionBuilder = new ActionBuilder(action.Action, action.Parameter);
                var actionTask = actionBuilder.Builer(taskID, action.ID);
                OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Runing);
                if (actionTask != null)
                {
                    for (int i = 0; i < action.Num; i++)
                    {
                        actionTask.Start();
                        var actionResult = actionTask.Result;
                        if (actionBuilder.IsHasResult())
                        {
                            ActionTaskResulter.Add(taskID, actionResult);
                        }
                        OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Success);
                    }
                }
                else
                {
                    OnActionInvoke?.Invoke(taskID, action.ID, Types.ActionInvokeStateType.Failed);
                    LogHelper.Warning($"找不到Action Type：{action.Action}");
                }
            }
        }
    }
}
