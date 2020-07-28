using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class WriteFileAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
             {
                 OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                 var p = ObjectConvert.Get<WriteFileActionParameterModel>(action.Parameter);
                 var result = new ActionResultModel();
                 result.ID = action.ID;
                 result.Result = new Dictionary<int, string>();
                 result.Result.Add((int)CommonResultKeyType.IsSuccess, "false");
                 p.FilePath = ActionParameterConverter.ConvertToString(taskID, p.FilePath);
                 p.Content = ActionParameterConverter.ConvertToString(taskID, p.Content);

                 Debug.WriteLine("write file:" + p.FilePath);
                 try
                 {
                     File.WriteAllText(p.FilePath, p.Content);
                     result.Result[(int)CommonResultKeyType.IsSuccess] = "true";
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
