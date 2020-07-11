using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types.ResultTypes
{
    public enum DeleteFileResultType
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        IsSuccess = 1,
        /// <summary>
        /// 被删除文件路径
        /// </summary>
        Path = 2
    }
}
