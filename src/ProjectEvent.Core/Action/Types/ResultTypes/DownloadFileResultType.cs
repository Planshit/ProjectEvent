using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types.ResultTypes
{
    public enum DownloadFileResultType
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        IsSuccess = 1,
        /// <summary>
        /// 文件保存地址
        /// </summary>
        SavePath = 2
    }
}
