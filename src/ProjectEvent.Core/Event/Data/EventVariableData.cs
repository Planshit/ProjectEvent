using ProjectEvent.Core.Event.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectEvent.Core.Event.Data
{
    /// <summary>
    /// 事件变量可读名称字典
    /// </summary>
    public static class EventVariableData
    {
        public static readonly Dictionary<EventType, Dictionary<string, string>> Variables = new Dictionary<EventType, Dictionary<string, string>>()
        {
            { EventType.OnProcessCreated,new Dictionary<string, string>()
            {
                { nameof(ProcessCreatedEventVariableType.ProcessName),"进程名称" },
                { nameof(ProcessCreatedEventVariableType.ExecutablePath),"可执行文件路径" },
                { nameof(ProcessCreatedEventVariableType.Handle),"句柄" },
                { nameof(ProcessCreatedEventVariableType.CommandLine),"命令行参数" },

            } },
            { EventType.OnFileChanged,new Dictionary<string, string>()
            {
                { nameof(FileChangedEventVariableType.Type),$"变化类型（{nameof(WatcherChangeTypes.Created)}:创建，{nameof(WatcherChangeTypes.Deleted)}:删除，{nameof(WatcherChangeTypes.Renamed)}:重命名）" },
                { nameof(FileChangedEventVariableType.Path),"文件路径" },

            } }

        };
    }
}
