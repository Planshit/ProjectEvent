using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Types
{
    public enum ProcessCreatedEventVariableType
    {
        /// <summary>
        /// 进程名称
        /// </summary>
        ProcessName,
        /// <summary>
        /// 执行路径
        /// </summary>
        ExecutablePath,
        /// <summary>
        /// 句柄
        /// </summary>
        Handle,
        /// <summary>
        /// 命令行参数
        /// </summary>
        CommandLine
    }
}
