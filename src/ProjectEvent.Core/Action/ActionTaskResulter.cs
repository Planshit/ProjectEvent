using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action
{
    /// <summary>
    /// action 执行返回管理器
    /// </summary>
    public static class ActionTaskResulter
    {
        private static Dictionary<int, List<ActionResultModel>> _actionResults = new Dictionary<int, List<ActionResultModel>>();

        /// <summary>
        /// 添加一条 action task result
        /// </summary>
        /// <param name="id">task id</param>
        /// <param name="value">action result</param>
        public static void Add(int id, ActionResultModel value)
        {
            if (id <= 0)
            {
                return;
            }
            var taskActionRes = new List<ActionResultModel>();
            if (_actionResults.ContainsKey(id))
            {
                taskActionRes = _actionResults[id];
                taskActionRes.Add(value);
            }
            else
            {
                taskActionRes.Add(value);
                _actionResults.Add(id, taskActionRes);
            }
        }

        /// <summary>
        /// 获取一条action task result
        /// </summary>
        /// <param name="id">task id</param>
        /// <returns></returns>
        public static List<ActionResultModel> Get(int id)
        {
            if (_actionResults.ContainsKey(id))
            {
                return _actionResults[id];
            }
            return null;
        }

        /// <summary>
        /// 获得一条action result
        /// </summary>
        /// <param name="taskid">task id</param>
        /// <param name="actionid">action id</param>
        /// <returns></returns>
        public static ActionResultModel GetActionResult(int taskid, int actionid)
        {
            var taskResult = Get(taskid);
            if (taskResult != null)
            {
                var actionResult = taskResult.Where(m => m.ID == actionid).FirstOrDefault();
                if (actionResult != null)
                {
                    return actionResult;
                }
            }
            return null;
        }
        /// <summary>
        /// 通过输入字符串转换为最终执行返回结果
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetActionResultsString(int taskid, string content)
        {
            string result = content;
            var taskResult = Get(taskid);
            if (taskResult != null)
            {

                //var variables = Regex.Matches(content, @"\{(?<id>[0-9]{1,5})\.(?<key>[a-zA-Z]{1,25})\}");
                var variables = Regex.Matches(content, @"\{(?<id>[0-9]{1,5})\.(?<key>-?[0-9]{1,25})\}");

                foreach (Match variable in variables)
                {
                    var id = variable.Groups["id"].Value;
                    var key = int.Parse(variable.Groups["key"].Value);
                    var actionResult = taskResult.Where(m => m.ID == int.Parse(id)).FirstOrDefault();
                    if (actionResult != null)
                    {
                        if (actionResult.Result.ContainsKey(key))
                        {
                            result = result.Replace(variable.Value, actionResult.Result[key]);
                        }
                    }
                }

            }
            return result;
        }
    }
}
