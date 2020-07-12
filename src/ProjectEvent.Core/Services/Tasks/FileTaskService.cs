using Microsoft.Win32;
using ProjectEvent.Core.Action;
using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class FileTaskService : IFileTaskService
    {
        private readonly IEventService eventService;

        private Dictionary<int, FileSystemWatcher> watchers;
        private List<EventModel> events;
        public FileTaskService(IEventService eventContainerService)
        {
            eventService = eventContainerService;
            watchers = new Dictionary<int, FileSystemWatcher>();
            events = new List<EventModel>();
            eventService.OnAddEvent += EventService_OnAddEvent;
            eventService.OnRemoveEvent += EventService_OnRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            if (newValue.EventType == Event.Types.EventType.OnFileChanged)
            {
                CreateWatch(newValue);
            }
            else if (oldValue.EventType == Event.Types.EventType.OnFileChanged)
            {
                Remove(oldValue);
            }
        }

        private void EventService_OnRemoveEvent(EventModel ev)
        {
            Remove(ev);
        }

        private void EventService_OnAddEvent(EventModel ev)
        {
            CreateWatch(ev);
        }

        public void Run()
        {
            var events = eventService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.OnFileChanged
               ).
               ToList();
            foreach (var ev in events)
            {
                CreateWatch(ev);
            }
        }
        private void Remove(EventModel ev)
        {
            if (watchers.ContainsKey(ev.ID))
            {
                watchers[ev.ID].EnableRaisingEvents = false;
                watchers.Remove(ev.ID);
            }
            var uev = events.Where(m => m.ID == ev.ID);
            if (uev != null)
            {
                events.Remove(uev.FirstOrDefault());
            }
        }
        private void CreateWatch(EventModel ev)
        {
            //先尝试卸载监听
            Remove(ev);
            //创建监听
            var watcher = new FileSystemWatcher();
            var ct = ev.Condition as OnFileChangedCondition;
            watcher.Path = ct.WatchPath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
            | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = ct.Extname;
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
            watchers.Add(ev.ID, watcher);
            events.Add(ev);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            var watcher = sender as FileSystemWatcher;
            Handle(new FileChangedDataModel()
            {
                WatchPath = watcher.Path,
                FileSystemEventArgs = e
            });
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            var watcher = sender as FileSystemWatcher;
            Handle(new FileChangedDataModel()
            {
                WatchPath = watcher.Path,
                FileSystemEventArgs = e
            });
        }

        private void Handle(object data)
        {
            foreach (var ev in events)
            {
                eventService.Invoke(ev, data);
            }
        }
    }
}
