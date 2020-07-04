using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.UI.Services
{
    public class EventLog : IEventLog
    {
        private readonly IEventService eventService;

        private List<EventLogModel> logs;
        private const string LogDir = "Data\\EventLog\\";
        private const int MaxLogsCount = 100;
        private int Index = 0;
        /// <summary>
        /// 等待写入文件的log list
        /// </summary>
        private List<EventLogModel> storageLogs;

        //写入文件锁
        private readonly object writeLock = new object();
        //添加log锁
        private readonly object addLock = new object();

        public EventLog(IEventService eventService)
        {
            this.eventService = eventService;
            logs = new List<EventLogModel>();
            storageLogs = new List<EventLogModel>();
        }

        public void Save()
        {
            Task.Run(() =>
            {
                lock (writeLock)
                {
                    if (storageLogs.Count == 0 && logs.Count > 0)
                    {
                        storageLogs.AddRange(logs);
                        logs.Clear();
                    }

                    if (storageLogs.Count > 0)
                    {
                        IOHelper.CreateDirectory(LogDir);
                        string dataPath = $"{LogDir}{DateTime.Now.ToString("yyyy-MM-dd")}.json";
                        if (IOHelper.FileExists(dataPath))
                        {
                            var data = JsonConvert.DeserializeObject<List<EventLogModel>>(IOHelper.ReadFile(dataPath));
                            if (data != null)
                            {
                                data.AddRange(storageLogs);
                                IOHelper.WriteFile(dataPath, JsonConvert.SerializeObject(data));
                            }
                        }
                        else
                        {
                            IOHelper.WriteFile(dataPath, JsonConvert.SerializeObject(storageLogs));
                        }
                        storageLogs.Clear();
                    }

                }
            });

        }

        public void Listen()
        {
            eventService.OnEventTrigger -= EventService_OnEventTrigger;
            eventService.OnEventTrigger += EventService_OnEventTrigger;
        }

        private void EventService_OnEventTrigger(Core.Event.Models.EventModel ev, bool isSuccess)
        {
            Add(ev);
        }

        private void Add(Core.Event.Models.EventModel ev)
        {
            Task.Run(() =>
            {
                lock (addLock)
                {
                    Clear();
                    Index++;
                    long id = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + Index;
                    logs.Add(new EventLogModel()
                    {
                        ID = id,
                        ProjectID = ev.ID,
                        Time = DateTime.Now
                    });
                }
            });

        }

        private void Clear()
        {
            if (logs.Count >= MaxLogsCount)
            {
                var clearData = logs.GetRange(0, MaxLogsCount);
                storageLogs.AddRange(clearData);
                logs.RemoveRange(0, MaxLogsCount);
                Save();
            }
        }

        public List<EventLogModel> GetEventLogs()
        {
            string dataPath = $"{LogDir}{DateTime.Now.ToString("yyyy-MM-dd")}.json";
            var data = new List<EventLogModel>();
            if (IOHelper.FileExists(dataPath))
            {
                data = JsonConvert.DeserializeObject<List<EventLogModel>>(IOHelper.ReadFile(dataPath));
            }
            data.AddRange(logs);
            return data;
        }
    }
}
