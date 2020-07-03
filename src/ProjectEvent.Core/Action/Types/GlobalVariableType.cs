using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types
{
    public enum GlobalVariableType
    {
        /// <summary>
        /// 当前时间戳（秒)
        /// </summary>
        Timestamp,
        /// <summary>
        /// 当前用户名
        /// </summary>
        CurrentUserName,
    }
}
