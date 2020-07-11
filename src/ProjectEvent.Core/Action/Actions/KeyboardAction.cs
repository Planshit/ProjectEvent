using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class KeyboardAction : IAction
    {
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                var p = ObjectConvert.Get<KeyboardActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                //result.Result.Add((int)SnippingResultType.IsSuccess, "false");
                p.Keys = ActionParameterConverter.ConvertToString(taskID, p.Keys);
                try
                {
                    if (p.Keys.Contains("+"))
                    {
                        //组合键
                        var keysgroup = p.Keys.Split('+');
                        foreach (var item in keysgroup)
                        {
                            string key = item.Substring(0, 1).ToUpper() + item.Substring(1);
                            KeyboardWin32API.Press(key);
                        }
                        foreach (var item in keysgroup)
                        {
                            string key = item.Substring(0, 1).ToUpper() + item.Substring(1);
                            KeyboardWin32API.Up(key);
                        }
                    }
                    else
                    {
                        //单键
                        p.Keys = p.Keys.Substring(0, 1).ToUpper() + p.Keys.Substring(1);
                        KeyboardWin32API.Press(p.Keys);
                        KeyboardWin32API.Up(p.Keys);
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }
                //返回数据
                //ActionTaskResulter.Add(taskID, result);
            };
        }
    }
}
