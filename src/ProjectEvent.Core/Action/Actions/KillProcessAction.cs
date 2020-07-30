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
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class KillProcessAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<KillProcessActionParamsModel>(action.Parameter);
                p.ProcessName = ActionParameterConverter.ConvertToString(taskID, p.ProcessName);

                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)CommonResultKeyType.IsSuccess, true.ToString());
                if (!string.IsNullOrEmpty(p.ProcessName))
                {
                    var processes = Process.GetProcesses();
                    List<Process> closeProcesses;
                    if (p.IsFuzzy)
                    {
                        closeProcesses = processes.Where(m => m.ProcessName.Contains(p.ProcessName)).ToList();
                    }
                    else
                    {
                        closeProcesses = processes.Where(m => m.ProcessName == p.ProcessName).ToList();
                    }
                    try
                    {
                        foreach (var item in closeProcesses)
                        {

                            item.Kill();

                        }
                        if (closeProcesses.Count == 0)
                        {
                            result.Result[(int)CommonResultKeyType.IsSuccess] = false.ToString();
                        }
                    }
                    catch
                    {
                        result.Result[(int)CommonResultKeyType.IsSuccess] = false.ToString();
                    }
                }
                else
                {
                    result.Result[(int)CommonResultKeyType.IsSuccess] = false.ToString();
                }
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };
        }
    }
}
