using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public interface ICondition
    {
        /// <summary>
        /// 判断当前是否满足条件
        /// </summary>
        /// <returns>true是，false否</returns>
        bool IsPass();

        /// <summary>
        /// 提供给外部检查条件是否输入正确
        /// </summary>
        /// <returns></returns>
        ConditionCheckResultModel Check();
    }
}
