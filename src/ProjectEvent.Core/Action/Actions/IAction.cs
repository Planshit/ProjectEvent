using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public interface IAction
    {
        System.Action GenerateAction(int taskID, ActionModel action);
        /// <summary>
        /// 当action状态发生变化时
        /// </summary>
        event ActionInvokeHandler OnEventStateChanged;
    }
}
