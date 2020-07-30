using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Structs;
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
    public class DialogAction : IAction
    {
        private int callID;
        private int taskID;
        private int actionID;
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            this.taskID = taskID;
            actionID = action.ID;
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<DialogActionParamsModel>(action.Parameter);
                p.Title = ActionParameterConverter.ConvertToString(taskID, p.Title);
                p.Content = ActionParameterConverter.ConvertToString(taskID, p.Content);
                p.Image = ActionParameterConverter.ConvertToString(taskID, p.Image);

                try
                {
                    callID = new Random().Next(9999) + DateTime.Now.Second;
                    PipeCallFunction.Call(new Structs.PipeCallFunctionStruct()
                    {
                        ID = callID,
                        CallFunctionType = UI.Types.PipeCallFunctionType.Dialog,
                        Data = p
                    });
                    PipeCallFunction.OnCallFeedback += PipeCallFunction_OnCallFeedback;
                }
                catch (Exception e)
                {
                    OnEventStateChanged?.Invoke(taskID, actionID, ActionInvokeStateType.Done);
                    LogHelper.Error(e.ToString());
                }

            };
        }

        private void PipeCallFunction_OnCallFeedback(PipeCallFunctionFeedbackStruct fb)
        {
            if (fb.ID == callID)
            {
                var result = new ActionResultModel();
                result.ID = actionID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)DialogResultType.ClickButtonValue, fb.FeedbackData.ToString());
                //返回数据
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, actionID, ActionInvokeStateType.Done);
            }
        }


    }
}
