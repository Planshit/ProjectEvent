using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action.Actions
{
    public class RegexAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<RegexActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, object>();
                result.Result.Add((int)RegexResultType.Count, "0");
                p.Content = ActionParameterConverter.ConvertToString(taskID, p.Content);

                try
                {
                    var matchs = Regex.Matches(p.Content, p.Regex);
                    int i = 0;
                    foreach (Match match in matchs)
                    {
                        result.Result.Add(i, match.Value);
                        i++;
                    }
                    result.Result[(int)RegexResultType.Count] = matchs.Count.ToString();
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
