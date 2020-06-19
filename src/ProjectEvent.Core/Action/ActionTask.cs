using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Event;
using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action
{
    public static class ActionTask
    {
        private static int _index = 0;
        public static bool Invoke(EventModel ev)
        {
            bool isSuccess = false;
            if (ev.Condition.IsPass())
            {
                Task.Factory.StartNew(() =>
                {
                    int taskID = _index++;
                    InvokeAction(taskID, ev.Actions);

                });
                isSuccess = true;
            }

            //记录event触发
            EventLoger.Add(new EventLogModel()
            {
                EventType = ev.EventType,
                IsSuccess = isSuccess,
            });

            return isSuccess;
        }

        public static void InvokeAction(int taskID, IEnumerable<ActionModel> actions)
        {
            foreach (var action in actions)
            {
                var actionBuilder = new ActionBuilder(action.Action, action.Parameter);
                var actionTask = actionBuilder.Builer(taskID, action.ID);
                if (actionTask != null)
                {
                    for (int i = 0; i < action.Num; i++)
                    {
                        actionTask.Start();
                        if (actionBuilder.IsHasResult())
                        {
                            var actionResult = actionTask.Result;
                            ActionTaskResulter.Add(taskID, actionResult);
                        }
                    }
                }
                else
                {
                    LogHelper.Warning($"找不到Action Type：{action.Action}");
                }
            }
        }
    }
}
