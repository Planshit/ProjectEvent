using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types
{
    /// <summary>
    /// action执行状态
    /// </summary>
    public enum ActionInvokeStateType
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Runing,
        /// <summary>
        /// 完成
        /// </summary>
        Done,
        /// <summary>
        /// 繁忙
        /// </summary>
        Busy
    }
}
