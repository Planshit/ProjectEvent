using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class OpenURLAction : IAction
    {
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                try
                {
                    var p = ObjectConvert.Get<OpenURLActionParamsModel>(action.Parameter);
                    var result = new ActionResultModel();
                    result.ID = action.ID;
                    result.Result = new Dictionary<int, string>();
                    result.Result.Add((int)CommonResultKeyType.IsSuccess, "false");
                    p.URL = ActionParameterConverter.ConvertToString(taskID, p.URL);

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = p.URL,
                        UseShellExecute = true
                    };
                    Process.Start(psi);

                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

            };
        }
    }
}
