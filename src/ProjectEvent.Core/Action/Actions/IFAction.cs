using ProjectEvent.Core.Action.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class IFAction : IAction
    {
        public Task<object> GenerateAction(int taskID, int actionID, object[] args)
        {
            var task = new Task<object>(() =>
            {
                var left = args[0];
                var condition = args[1];
                var right = args[2];

                var passAction = args[3];
                var noPassAction = args[4];

                bool isPass = false;

                switch (condition)
                {
                    case "=":

                        break;
                }

                if (isPass)
                {
                    if (passAction as List<ActionModel> != null)
                    {
                        ActionTask.InvokeAction(taskID, passAction as List<ActionModel>, actionID);
                    }
                }
                else
                {
                    if (noPassAction as List<ActionModel> != null)
                    {
                        ActionTask.InvokeAction(taskID, noPassAction as List<ActionModel>, actionID);
                    }
                }


                return true;
            });
            return task;

        }
    }
}
