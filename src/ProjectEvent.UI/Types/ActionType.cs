using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Types
{
    /// <summary>
    /// 用于UI的ActionType
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        WriteFile = 1,
        /// <summary>
        /// 判断开始
        /// </summary>
        IF = 2,
        /// <summary>
        /// 否则
        /// </summary>
        IFElse = 3,
        /// <summary>
        /// 判断结束
        /// </summary>
        IFEnd = 4,
        /// <summary>
        /// HTTP GET请求
        /// </summary>
        HttpGet = 5,
        /// <summary>
        /// 关机
        /// </summary>
        Shutdown=6
    }
}
