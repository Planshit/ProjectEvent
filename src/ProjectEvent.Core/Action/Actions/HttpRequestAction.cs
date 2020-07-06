using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class HttpRequestAction : IAction
    {
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
             {
                 var p = ObjectConvert.Get<HttpRequestActionParameterModel>(action.Parameter);
                 var result = new ActionResultModel();
                 result.ID = action.ID;
                 result.Result = new Dictionary<int, string>();
                 result.Result.Add((int)HttpResultType.IsSuccess, "false");
                 if (p != null)
                 {
                     p.Url = ActionParameterConverter.ConvertToString(taskID, p.Url);
                     Debug.WriteLine("http request:" + p.Url);
                     var http = new HttpRequest();
                     http.Url = p.Url;
                     http.Headers = p.Headers;
                     http.Data = p.QueryParams;
                     http.Files = p.Files;
                     http.ParamsType = p.ParamsType;
                     try
                     {
                         var content = p.Method == Net.Types.MethodType.GET ? http.GetAsync().Result : http.PostAsync().Result;
                         result.Result.Add((int)HttpResultType.StatusCode, content.StatusCode.ToString());
                         result.Result.Add((int)HttpResultType.Content, content.Content);
                         result.Result[(int)HttpResultType.IsSuccess] = content.IsSuccess.ToString().ToLower();
                     }
                     catch (Exception e)
                     {
                         Debug.WriteLine(e.ToString());
                     }
                 }

                 //返回数据
                 ActionTaskResulter.Add(taskID, result);
             };
        }
    }
}
