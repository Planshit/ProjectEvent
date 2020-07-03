using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action
{
    /// <summary>
    /// 全局变量管理器
    /// 全局变量仅用于常用且数据量小的数据
    /// </summary>
    public static class GlobalVariable
    {
        public static readonly Dictionary<string, string> Variables = new Dictionary<string, string>()
        {
            {nameof(GlobalVariableType.Timestamp),"当前本地时间戳（秒)" },
            {nameof(GlobalVariableType.CurrentUserName),"当前登录用户名" }

        };
        private static Dictionary<GlobalVariableType, string> cacheVariablesData = new Dictionary<GlobalVariableType, string>();

        public static string ConvertToContent(string parameter)
        {
            var variables = Regex.Matches(parameter, @"\{@(?<key>[a-zA-Z]{1,25})\}");
            if (variables.Count == 0)
            {
                return parameter;
            }
            foreach (Match variable in variables)
            {
                var key = variable.Groups["key"].Value;
                if (Enum.IsDefined(typeof(GlobalVariableType), key))
                {
                    Enum.TryParse(key, out GlobalVariableType t);
                    string content = GetVariableContent(t);
                    parameter = parameter.Replace(variable.Value, content);
                }
            }
            return parameter;
        }

        private static string GetVariableContent(GlobalVariableType globalVariable)
        {
            string resultValue = string.Empty;
            if (cacheVariablesData.ContainsKey(globalVariable))
            {
                resultValue = cacheVariablesData[globalVariable];
            }
            else
            {
                switch (globalVariable)
                {
                    case GlobalVariableType.Timestamp:
                        resultValue = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
                        break;
                    case GlobalVariableType.CurrentUserName:
                        resultValue = SystemHelper.GetCurrentUserName();
                        //缓存
                        cacheVariablesData.Add(globalVariable, resultValue);
                        break;
                }
            }

            return resultValue;
        }
    }
}
