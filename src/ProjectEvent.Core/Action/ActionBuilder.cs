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
        //private readonly ActionType actionType;
        //private readonly object parameter;
        //public ActionBuilder(ActionType actionType, object parameter)
        //{
        //    this.actionType = actionType;
        //    this.parameter = parameter;
        //}

        ///// <summary>
        ///// 检查action是否可执行
        ///// </summary>
        ///// <returns></returns>
        //public bool Check()
        //{
        //    switch (actionType)
        //    {
        //        case ActionType.WriteFile:
        //            return new WriteFileCheck(parameter).IsCheck();
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 获取action是否有返回值
        ///// </summary>
        ///// <returns></returns>
        //public bool IsHasResult()
        //{
        //    var result = false;
        //    switch (actionType)
        //    {
        //        case ActionType.WriteFile:
        //            result = true;
        //            break;
        //        case ActionType.HttpGet:
        //            result = true;
        //            break;
        //        case ActionType.Shutdown:
        //            result = true;
        //            break;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 构建一个action，如果检查不通过时返回null
        ///// </summary>
        ///// <returns></returns>
        //public Task<ActionResultModel> Builer(int taskID, int actionID)
        //{
        //    if (!Check())
        //    {
        //        return null;
        //    }
        //    switch (actionType)
        //    {
        //        case ActionType.WriteFile:
        //            return new WriteFileAction().GenerateAction(taskID, actionID, parameter);

        //        case ActionType.IF:
        //            return new IFAction().GenerateAction(taskID, actionID, parameter);
        //        case ActionType.HttpGet:
        //            return new HttpGetAction().GenerateAction(taskID, actionID, parameter);
        //        case ActionType.Shutdown:
        //            return new ShutdownAction().GenerateAction(taskID, actionID, parameter);

        //        default:
        //            return null;
        //    }

        //}
    }
}
