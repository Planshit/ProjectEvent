using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class ProcessTaskService : IProcessTaskService
    {
        public event EventHandler OnEventTrigger;
        private readonly IEventService eventService;
        private ManagementEventWatcher watcher;
        private bool isRun = false;
        public ProcessTaskService(IEventService eventService)
        {
            this.eventService = eventService;
            EventQuery query = new EventQuery();
            query.QueryString = "SELECT * FROM" +
                " __InstanceCreationEvent WITHIN 1 " +
                "WHERE TargetInstance isa 'Win32_Process'";
            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += NewProcess_Created;
            eventService.OnAddEvent += _eventService_OnAddAndRemoveEvent;
            eventService.OnRemoveEvent += _eventService_OnAddAndRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            Run();
        }

        private void _eventService_OnAddAndRemoveEvent(EventModel @event)
        {
            Run();
        }

        public void Run()
        {
            var hasEv = eventService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.OnProcessCreated ||
               m.EventType == Event.Types.EventType.OnProcessShutdown
               ).
               Any();

            if (hasEv && !isRun)
            {
                //存在进程创建事件且未启动时
                watcher.Start();
                isRun = true;
            }
            else if (!hasEv && isRun)
            {
                //不存在进程创建事件且在运行中时
                watcher.Stop();
                isRun = false;
            }
        }
        private void Handle(ManagementBaseObject baseObject)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.OnProcessCreated ||
                m.EventType == Event.Types.EventType.OnProcessShutdown).
                ToList();

            foreach (var ev in evs)
            {
                if (ev.EventType == Event.Types.EventType.OnProcessCreated)
                {
                    eventService.Invoke(ev, baseObject);
                }
            }

        }
        private void NewProcess_Created(object sender, EventArrivedEventArgs se)
        {
            Handle(se.NewEvent);
        }
    }
}
