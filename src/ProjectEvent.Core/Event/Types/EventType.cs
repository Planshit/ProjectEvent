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
        /// 时钟发生变化
        /// </summary>
        OnTimerChanged,
        
        /// <summary>
        /// 设备启动
        /// </summary>
        OnDeviceStartup,

        /// <summary>
        /// 设备关闭
        /// </summary>
        OnDeviceShutdown,

        /// <summary>
        /// 网络断开
        /// </summary>
        OnNetworkDisconnection,

        /// <summary>
        /// 进程启动
        /// </summary>
        OnProcessStartup,

        /// <summary>
        /// 进程关闭
        /// </summary>
        OnProcessShutdown,

        /// <summary>
        /// 鼠标单击
        /// </summary>
        OnMouseClick,

        /// <summary>
        /// 间隔重复计时
        /// </summary>
        OnIntervalTimer,
    }
}
