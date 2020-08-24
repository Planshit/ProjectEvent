using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types
{
    public enum IFActionConditionType
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 1,
        /// <summary>
        /// 不等于
        /// </summary>
        UnEqual = 2,
        /// <summary>
        /// 包含
        /// </summary>
        Has = 3,
        /// <summary>
        /// 不包含
        /// </summary>
        Miss = 4,
        /// <summary>
        /// 大于
        /// </summary>
        Greater = 5,
        /// <summary>
        /// 小于
        /// </summary>
        Less = 6,
        /// <summary>
        /// 大于或等于
        /// </summary>
        GreaterOrEqual = 7,
        /// <summary>
        /// 小于或等于
        /// </summary>
        LessOrEqual = 8,
    }
}
