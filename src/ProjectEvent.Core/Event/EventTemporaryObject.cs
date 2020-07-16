using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Event.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using ProjectEvent.Core.Extensions;
using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Event.Structs;

namespace ProjectEvent.Core.Event
{
    /// <summary>
    /// 事件临时变量数据管理器
    /// </summary>
    public class EventTemporaryObject
    {
        private static Dictionary<int, Dictionary<string, string>> data_ = new Dictionary<int, Dictionary<string, string>>();

        public static void Add(int taskID, EventModel ev, object data)
        {
            Remove(taskID);

            var todata = GetEventTOData(ev, data);
            if (todata.Count > 0)
            {
                data_.Add(taskID, todata);

            }
        }

        /// <summary>
        /// 获取整合的事件临时变量数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetEventTOData(EventModel ev, object data)
        {
            var result = new Dictionary<string, string>();
            switch (ev.EventType)
            {
                case Types.EventType.OnProcessCreated:
                    var p = ((ManagementBaseObject)(data as ManagementBaseObject)["TargetInstance"]);

                    result.Add(nameof(ProcessCreatedEventVariableType.ProcessName), p.TryGetProperty("Name"));
                    result.Add(nameof(ProcessCreatedEventVariableType.ExecutablePath), p.TryGetProperty("ExecutablePath"));
                    result.Add(nameof(ProcessCreatedEventVariableType.Handle), p.TryGetProperty("Handle"));
                    result.Add(nameof(ProcessCreatedEventVariableType.CommandLine), p.TryGetProperty("CommandLine"));
                    break;
                case EventType.OnFileChanged:
                    var fcdata = data as FileChangedDataModel;
                    result.Add(nameof(FileChangedEventVariableType.Type), fcdata.FileSystemEventArgs.ChangeType.ToString());
                    result.Add(nameof(FileChangedEventVariableType.Path), fcdata.FileSystemEventArgs.FullPath.ToString());

                    break;
                case EventType.KeyboardEvent:
                    var kedata = (KeyboardEventDataStruct)data;
                    result.Add(nameof(KeyboardEventVariableType.Action), kedata.Action);
                    result.Add(nameof(KeyboardEventVariableType.KeyName), kedata.KeyName);
                    result.Add(nameof(KeyboardEventVariableType.KeyCode), kedata.KeyCode.ToString());
                    break;
                case EventType.NetworkStatusEvent:
                    var nsedata = (NetworkStatusDataStruct)data;
                    result.Add(nameof(NetworkStatusEventVariableType.IsConnected), nsedata.IsConnected.ToString());
                    break;
                case EventType.WIFIConnectedEvent:
                    result.Add(nameof(WIFIConnectedEventVariableType.SSID), data != null ? data.ToString() : string.Empty);
                    break;
            }

            return result;
        }
        public static void Remove(int taskID)
        {
            if (data_.ContainsKey(taskID))
            {
                data_.Remove(taskID);
            }
        }

        /// <summary>
        /// 将带有变量的内容转换为数据内容
        /// </summary>
        /// <param name="taskID">事件执行id</param>
        /// <param name="content">变量内容</param>
        /// <returns></returns>
        public static string ConvertToContent(int taskID, string content)
        {
            var variables = Regex.Matches(content, @"\{(?<key>[a-zA-Z]{1,25})\}");
            if (!data_.ContainsKey(taskID))
            {
                return content;
            }
            var taskData = data_[taskID];
            foreach (Match variable in variables)
            {
                var key = variable.Groups["key"].Value;
                if (taskData.ContainsKey(key))
                {
                    content = content.Replace(variable.Value, taskData[key]);
                }
            }
            return content;
        }
    }
}
