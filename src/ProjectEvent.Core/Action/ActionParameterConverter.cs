using ProjectEvent.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action
{
    /// <summary>
    /// action参数转换器，将参数中的变量转换为相应的数据字符串
    /// </summary>
    public static class ActionParameterConverter
    {
        public static string ConvertToString(int taskID, string parameter)
        {
            //事件变量
            parameter = EventTemporaryObject.ConvertToContent(taskID, parameter);
            //action result value
            parameter = ActionTaskResulter.GetActionResultsString(taskID, parameter);
            //全局变量
            parameter = GlobalVariable.ConvertToContent(parameter);
            return parameter;
        }

        public static Dictionary<string, string> ConvertToKeyValues(int taskID, Dictionary<string, string> data)
        {
            var resval = new Dictionary<string, string>();
            foreach (var item in data)
            {
                string key = ConvertToString(taskID, item.Key);
                string value = ConvertToString(taskID, item.Value);
                if (!resval.ContainsKey(key))
                {
                    resval.Add(key, value);
                }
            }
            return resval;
        }

    }
}
