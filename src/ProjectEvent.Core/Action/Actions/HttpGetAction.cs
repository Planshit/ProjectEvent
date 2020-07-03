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
    public class HttpGetAction : IAction
    {
        public Task<ActionResultModel> GenerateAction(int taskID, int actionID, object parameter)
        {
            var task = new Task<ActionResultModel>(() =>
            {
                var p = ObjectConvert.Get<HttpGetActionParameterModel>(parameter);
                var result = new ActionResultModel();
                result.ID = actionID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)HttpResultType.IsSuccess, "false");
                if (p != null)
                {
                    p.Url = ActionParameterConverter.ConvertToString(taskID, p.Url);
                    Debug.WriteLine("http get:" + p.Url);
                    var http = new HttpRequest();
                    http.Url = p.Url;
                    try
                    {
                        var content = http.GetAsync().Result;
                        result.Result.Add((int)HttpResultType.StatusCode, content.StatusCode.ToString());
                        result.Result.Add((int)HttpResultType.Content, content.Content);
                        result.Result[(int)HttpResultType.IsSuccess] = content.IsSuccess.ToString().ToLower();
                    }
                    catch
                    {
                    }
                }

                return result;
            });
            return task;
        }
    }
}
