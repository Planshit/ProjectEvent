using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Action.Actions
{
    public interface IAction
    {
        Task<ActionResultModel> GenerateAction(int taskID, int actionID, BaseParameterModel parameter);
    }
}
