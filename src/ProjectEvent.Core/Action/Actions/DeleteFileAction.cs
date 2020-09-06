using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class DeleteFileAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<DeleteFileActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, object>();
                result.Result.Add((int)DeleteFileResultType.IsSuccess, "false");
                result.Result.Add((int)DeleteFileResultType.Path, p.Path);
                p.Path = ActionParameterConverter.ConvertToString(taskID, p.Path);
                try
                {
                    File.Delete(p.Path);
                    result.Result[(int)DeleteFileResultType.IsSuccess] = "true";
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
