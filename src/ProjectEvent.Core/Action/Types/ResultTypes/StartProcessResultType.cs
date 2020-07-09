using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types.ResultTypes
{
    /// <summary>
    /// 启动进程操作返回类型
    /// </summary>
    public enum StartProcessResultType
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        IsSuccess = 1,
        /// <summary>
        /// 句柄
        /// </summary>
        Handle = 2,
        /// <summary>
        /// 进程id
        /// </summary>
        Id
    }
}
