using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
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
        public Task<ActionResultModel> GenerateAction(int taskID, int actionID, object parameter)
        {
            var task = new Task<ActionResultModel>(() =>
            {
                var result = new ActionResultModel();
                result.ID = actionID;
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
                return result;
            });
            return task;
        }
    }
}
