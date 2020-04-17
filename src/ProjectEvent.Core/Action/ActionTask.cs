using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Event;
using ProjectEvent.Core.Event.Models;
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

                    int actionID = 0;

                    InvokeAction(taskID, ev.Actions, actionID);

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

        public static void InvokeAction(int taskID, IEnumerable<ActionModel> actions, int actionID = 0)
        {
            foreach (var action in actions)
            {
                actionID++;

                var actionBuilder = new ActionBuilder(action.Action, action.Args);
                var actionTask = actionBuilder.Builer(taskID, actionID);

                for (int i = 0; i < action.Num; i++)
                {
                    actionTask.Start();
                    if (actionBuilder.IsHasResult())
                    {
                        var actionResult = actionTask.Result;
                        ActionTaskResulter.Add(taskID, new Models.ActionResultModel()
                        {
                            ID = actionID,
                            Result = actionResult
                        });
                    }
                }
            }
        }
    }
}
