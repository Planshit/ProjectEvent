using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types.ResultTypes
{
    public enum HttpResultType
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        IsSuccess = 1,
        /// <summary>
        /// 响应内容
        /// </summary>
        Content = 2,
        /// <summary>
        /// 状态码
        /// </summary>
        StatusCode = 3
    }
}
