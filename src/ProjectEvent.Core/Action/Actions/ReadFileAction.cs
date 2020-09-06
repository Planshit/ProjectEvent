using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
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
    public class ReadFileAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
             {
                 OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                 var p = ObjectConvert.Get<ReadFileActionParamsModel>(action.Parameter);
                 var result = new ActionResultModel();
                 result.ID = action.ID;
                 result.Result = new Dictionary<int, object>();
                 result.Result.Add((int)ReadFileResultType.IsSuccess, false.ToString());
                 result.Result.Add((int)ReadFileResultType.Content, string.Empty);

                 p.FilePath = ActionParameterConverter.ConvertToString(taskID, p.FilePath);
                 Debug.WriteLine("read file:" + p.FilePath);
                 try
                 {
                     result.Result[(int)ReadFileResultType.Content] = File.ReadAllText(p.FilePath);
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
