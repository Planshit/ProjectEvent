using ProjectEvent.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action
{
    /// <summary>
    /// action参数转换器，将参数中的变量转换为相应的数据字符串
    /// </summary>
    public static class ActionParameterConverter
    {
        public static string ConvertToString(int taskID, string parameter, bool isBreak = false)
        {
            if (parameter == null)
            {
                return string.Empty;
            }
            //事件变量
            parameter = EventTemporaryObject.ConvertToContent(taskID, parameter);
            //action result value
            parameter = ActionTaskResulter.GetActionResultsString(taskID, parameter);
            //全局变量
            parameter = GlobalVariable.ConvertToContent(parameter);
            if (IsHasVariable(parameter) && !isBreak)
            {
                parameter = ConvertToString(taskID, parameter, true);
            }
            return parameter;
        }
        private static bool IsHasVariable(string value)
        {
            return Regex.IsMatch(value, @"\{(?<id>[0-9]{1,5})\.(?<key>-?[0-9]{1,25})\}") || Regex.IsMatch(value, @"\{@(?<key>[a-zA-Z]{1,25})\}") || Regex.IsMatch(value, @"\{(?<key>[a-zA-Z]{1,25})\}");
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
