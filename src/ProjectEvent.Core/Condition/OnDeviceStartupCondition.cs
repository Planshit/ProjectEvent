using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnDeviceStartupCondition : ICondition
    {

        public ConditionCheckResultModel Check()
        {
            //开机启动不需要输入条件，直接通过
            var result = new ConditionCheckResultModel();
            result.IsValid = true;
            return result;
        }

        public bool IsPass(object data = null)
        {
            return Environment.GetCommandLineArgs().Contains("-autorun");
        }
    }
}
