using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition.Types
{
    /// <summary>
    /// 重复方式
    /// </summary>
    public enum TimeChangedRepetitionType
    {
        /// <summary>
        /// 不重复
        /// </summary>
        None = 1,
        /// <summary>
        /// 每天
        /// </summary>
        Day = 2,
        /// <summary>
        /// 每周
        /// </summary>
        Week = 3
    }
}
