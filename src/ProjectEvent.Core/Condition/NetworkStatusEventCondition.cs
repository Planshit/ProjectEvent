using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class NetworkStatusEventCondition : ICondition
    {
        public ConditionCheckResultModel Check()
        {
            //特殊事件直接通过
            var result = new ConditionCheckResultModel();
            result.IsValid = true;
            return result;
        }

        public bool IsPass(object data = null)
        {
            //特殊事件直接通过
            return true;
        }
    }
}
