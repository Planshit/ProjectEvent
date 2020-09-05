using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Types
{
    public enum ToastActionType
    {
        /// <summary>
        /// 默认启动程序或显示主窗口
        /// </summary>
        Default = 0,
        /// <summary>
        /// 打开URL
        /// </summary>
        Url = 1,
    }
}
