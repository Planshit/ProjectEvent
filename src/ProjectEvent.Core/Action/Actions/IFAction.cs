using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public class IFAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                bool isPass = false;
                var p = ObjectConvert.Get<IFActionParameterModel>(action.Parameter);
                string left, right;
                //获取左输入
                left = ActionParameterConverter.ConvertToString(taskID, p.LeftInput);
                //获取右输入
                right = ActionParameterConverter.ConvertToString(taskID, p.RightInput);


                switch (p.Condition)
                {
                    case Types.IFActionConditionType.Equal:
                        isPass = left == right;
                        break;
                    case Types.IFActionConditionType.UnEqual:
                        isPass = left != right;
                        break;
                    case Types.IFActionConditionType.Has:
                        isPass = left.IndexOf(right) != -1;
                        break;
                    case Types.IFActionConditionType.Miss:
                        isPass = left.IndexOf(right) == -1;
                        break;
                }

                if (isPass)
                {
                    ActionTask.Invoke(taskID, p.PassActions, taskID == ActionTask.TestTaskID, true);
                }
                else
                {
                    ActionTask.Invoke(taskID, p.NoPassActions, taskID == ActionTask.TestTaskID, true);
                }

                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)CommonResultKeyType.IsSuccess, isPass.ToString());
                //result.Result = new Dictionary<int, ActionResultValueModel>();
                //result.Result.Add((int)CommonResultKeyType.Status, new ActionResultValueModel()
                //{
                //    Type = ActoinResultValueType.BOOL,
                //    Value = isPass
                //});
                //返回数据
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };

        }


    }
}
