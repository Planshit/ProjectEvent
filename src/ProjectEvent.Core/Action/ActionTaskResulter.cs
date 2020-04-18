using ProjectEvent.Core.Action.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
