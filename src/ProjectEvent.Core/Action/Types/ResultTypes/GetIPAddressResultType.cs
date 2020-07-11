using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types.ResultTypes
{
    public enum GetIPAddressResultType
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        IsSuccess = 1,
        /// <summary>
        /// ip地址
        /// </summary>
        IP = 2
    }
}
