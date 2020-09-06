using Newtonsoft.Json;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class JsonDeserializeAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
             {
                 OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                 var p = ObjectConvert.Get<JsonDeserializeActionParamsModel>(action.Parameter);
                 var result = new ActionResultModel();
                 result.ID = action.ID;
                 result.Result = new Dictionary<int, object>();
                 result.Result.Add((int)CommonResultKeyType.IsSuccess, false.ToString());
                 p.Content = ActionParameterConverter.ConvertToString(taskID, p.Content);

                 Debug.WriteLine("JsonDeserialize:" + p.Content);
                 try
                 {
                     //尝试用正则表达式取出有效范围
                     var regx = Regex.Match(p.Content, @"\{([\s\S]*)\}");
                     if (regx.Success)
                     {
                         p.Content = regx.Value;
                     }
                     result.Result[-1] = JsonConvert.DeserializeObject<object>(p.Content);
                     result.Result[(int)CommonResultKeyType.IsSuccess] = true;

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
