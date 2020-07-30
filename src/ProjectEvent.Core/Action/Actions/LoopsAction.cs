using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class LoopsAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<LoopsActionParamsModel>(action.Parameter);
                int i = 0;
                if (p.Count <= 0)
                {
                    p.Count = 1;
                }
                while (true)
                {
                    ActionTask.Invoke(taskID, p.Actions, taskID == ActionTask.TestTaskID, true);
                    i++;
                    if (i == p.Count)
                    {
                        break;
                    }
                }
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };

        }


    }
}
