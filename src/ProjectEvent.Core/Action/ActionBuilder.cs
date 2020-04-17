using ProjectEvent.Core.Action.Actions;
using ProjectEvent.Core.Action.Checks;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action
{
    public class ActionBuilder
    {
        private readonly ActionType actionType;
        private readonly string[] args;
        public ActionBuilder(ActionType actionType, string[] args)
        {
            this.actionType = actionType;
            this.args = args;
        }

        /// <summary>
        /// 检查action是否可执行
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            switch (actionType)
            {
                case ActionType.WriteFile:
                    return new WriteFileCheck(args).IsCheck();
            }
            return false;
        }

        /// <summary>
        /// 获取action是否有返回值
        /// </summary>
        /// <returns></returns>
        public bool IsHasResult()
        {
            var result = false;
            switch (actionType)
            {
                case ActionType.WriteFile:
                    result = true;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 构建一个action，如果检查不通过时返回null
        /// </summary>
        /// <returns></returns>
        public Task<object> Builer(int taskID, int actionID)
        {
            if (!Check())
            {
                return null;
            }
            switch (actionType)
            {
                case ActionType.WriteFile:
                    return new WriteFileAction().GenerateAction(taskID, actionID, args);

                default:
                    return null;
            }

        }
    }
}
