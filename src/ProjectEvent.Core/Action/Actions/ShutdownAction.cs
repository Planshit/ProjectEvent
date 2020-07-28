using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class ShutdownAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)CommonResultKeyType.IsSuccess, "false");
                try
                {
                    ShutDownSys.ShutDown();
                    Debug.WriteLine("执行关机");
                    result.Result[(int)CommonResultKeyType.IsSuccess] = "true";
                }
                catch
                {
                }
                //返回数据
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };
        }
    }
}
