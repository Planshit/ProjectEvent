using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action.Actions
{
    public class DownloadFileAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<DownloadFileParamsModel>(action.Parameter);
                p.Url = ActionParameterConverter.ConvertToString(taskID, p.Url);
                p.SavePath = ActionParameterConverter.ConvertToString(taskID, p.SavePath);

                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)DownloadFileResultType.IsSuccess, false.ToString());
                result.Result.Add((int)DownloadFileResultType.SavePath, p.SavePath);
                try
                {
                    var wc = new WebClient();
                    wc.DownloadFile(p.Url, p.SavePath);
                    result.Result[(int)DownloadFileResultType.IsSuccess] = true.ToString();
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
