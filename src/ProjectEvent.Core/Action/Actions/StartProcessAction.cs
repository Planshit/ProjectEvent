using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class StartProcessAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<StartProcessActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, object>();
                result.Result.Add((int)StartProcessResultType.IsSuccess, "false");
                result.Result.Add((int)StartProcessResultType.Handle, "");
                result.Result.Add((int)StartProcessResultType.Id, "");
                p.Path = ActionParameterConverter.ConvertToString(taskID, p.Path);
                p.Args = ActionParameterConverter.ConvertToString(taskID, p.Args);

                Debug.WriteLine("启动进程:" + p.Path);
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo(p.Path, p.Args);
                    var res = Process.Start(psi);
                    if (res != null)
                    {
                        result.Result[(int)StartProcessResultType.Handle] = res.Handle.ToString();
                        result.Result[(int)StartProcessResultType.Id] = res.Id.ToString();
                    }
                    result.Result[(int)StartProcessResultType.IsSuccess] = "true";
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }
                //返回数据
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };
        }
    }
}
