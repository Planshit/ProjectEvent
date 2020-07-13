using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Types
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 周期事件
        /// </summary>
        OnTimeChanged = 11,

        /// <summary>
        /// 设备启动
        /// </summary>
        OnDeviceStartup = 2,

        ///// <summary>
        ///// 设备关闭
        ///// </summary>
        //OnDeviceShutdown = 3,

        /// <summary>
        /// 网络断开
        /// </summary>
        OnNetworkDisconnection = 4,

        /// <summary>
        /// 进程创建
        /// </summary>
        OnProcessCreated = 5,

        /// <summary>
        /// 进程关闭
        /// </summary>
        OnProcessShutdown = 6,

        /// <summary>
        /// 鼠标单击
        /// </summary>
        OnMouseClick = 7,

        /// <summary>
        /// 计时器
        /// </summary>
        OnIntervalTimer = 8,
        /// <summary>
        /// 文件发生更改（添加，删除，重命名）
        /// </summary>
        OnFileChanged = 9
    }
}
