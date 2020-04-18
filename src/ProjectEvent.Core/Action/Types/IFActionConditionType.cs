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
        Equal,
        /// <summary>
        /// 不等于
        /// </summary>
        UnEqual,
        /// <summary>
        /// 包含
        /// </summary>
        Has,
        /// <summary>
        /// 不包含
        /// </summary>
        Miss
    }
}
