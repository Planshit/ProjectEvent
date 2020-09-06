using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
                    case IFActionConditionType.Greater:
                    case IFActionConditionType.GreaterOrEqual:
                    case IFActionConditionType.Less:
                    case IFActionConditionType.LessOrEqual:
                        if (IsTime(left))
                        {
                            isPass = TimeConditionCheck(left, right, p.Condition);
                        }
                        else
                        {
                            isPass = NumberConditionCheck(left, right, p.Condition);
                        }
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
                result.Result = new Dictionary<int, object>();
                result.Result.Add((int)CommonResultKeyType.IsSuccess, isPass);
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
        /// <summary>
        /// 是否是时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTime(string value)
        {
            if (IsDateTime(value))
            {
                return true;
            }
            if (IsTimestamp(value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 时间判断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsDateTime(string value)
        {
            //正则判断
            //y-m-d h:m:s格式
            return Regex.IsMatch(value, @"([0-9]*)-([0-9]*)-([0-9]*) ([0-9]*):([0-9]*):([0-9]*)");
        }
        /// <summary>
        /// 时间戳判断
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsTimestamp(string value)
        {
            //时间戳
            bool isp = int.TryParse(value, out int n);
            return isp ? n.ToString().Length == 10 : false;
        }
        /// <summary>
        /// 字符串转时间类型（支持unix时间戳和mysql时间格式）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private DateTime StringToDateTime(string value)
        {
            if (IsTimestamp(value))
            {
                int unixTimeStamp = int.Parse(value);
                DateTime startTime = new DateTime(1970, 1, 1).ToLocalTime();
                DateTime dt = startTime.AddSeconds(unixTimeStamp);
                return dt;
            }
            if (IsDateTime(value))
            {
                return DateTime.Parse(value);
            }
            return DateTime.Now;
        }
        /// <summary>
        /// 时间类型判断
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        private bool TimeConditionCheck(string left, string right, IFActionConditionType con)
        {
            DateTime ln = StringToDateTime(left), rn = StringToDateTime(right);

            if (con == IFActionConditionType.Greater)
            {
                return ln > rn;
            }
            if (con == IFActionConditionType.GreaterOrEqual)
            {
                return ln >= rn;
            }
            if (con == IFActionConditionType.Less)
            {
                return ln < rn;
            }
            if (con == IFActionConditionType.LessOrEqual)
            {
                return ln <= rn;
            }
            return false;
        }
        /// <summary>
        /// 数字类型判断
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="con"></param>
        /// <returns></returns>
        private bool NumberConditionCheck(string left, string right, IFActionConditionType con)
        {
            bool lpr = double.TryParse(left, out double ln), rpr = double.TryParse(right, out double rn);
            if (!lpr || !rpr)
            {
                return false;
            }
            if (con == IFActionConditionType.Greater)
            {
                return ln > rn;
            }
            if (con == IFActionConditionType.GreaterOrEqual)
            {
                return ln >= rn;
            }
            if (con == IFActionConditionType.Less)
            {
                return ln < rn;
            }
            if (con == IFActionConditionType.LessOrEqual)
            {
                return ln <= rn;
            }
            return false;
        }
    }
}
