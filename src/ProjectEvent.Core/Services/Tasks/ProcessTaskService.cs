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
        private readonly IEventContainerService _eventContainerService;
        private ManagementEventWatcher watcher;
        public ProcessTaskService(IEventContainerService eventContainerService)
        {
            _eventContainerService = eventContainerService;
            EventQuery query = new EventQuery();
            query.QueryString = "SELECT * FROM" +
                " __InstanceCreationEvent WITHIN 1 " +
                "WHERE TargetInstance isa 'Win32_Process'";
            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += NewProcess_Created;
            _eventContainerService.OnAddEvent += _eventContainerService_OnAddEvent;
        }

        private void _eventContainerService_OnAddEvent(EventModel @event)
        {
            Run();
        }

        public void Run()
        {
            var hasEv = _eventContainerService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.OnProcessCreated ||
               m.EventType == Event.Types.EventType.OnProcessShutdown
               ).
               Any();

            if (hasEv)
            {
                watcher.Start();
            }
        }
        private void Handle(ManagementBaseObject baseObject)
        {
            var evs = _eventContainerService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.OnProcessCreated ||
                m.EventType == Event.Types.EventType.OnProcessShutdown).
                ToList();

            foreach (var ev in evs)
            {
                switch (ev.EventType)
                {
                    case Event.Types.EventType.OnProcessCreated:
                        if (ActionTask.Invoke(ev, baseObject))
                        {
                            OnEventTrigger?.Invoke(ev, 0);
                        }
                        break;
                }
            }

        }
        private void NewProcess_Created(object sender, EventArrivedEventArgs se)
        {
            Handle(se.NewEvent);
            //ManagementBaseObject e = se.NewEvent;
            //var Processname = ((ManagementBaseObject)e["TargetInstance"])["Name"].ToString();
            //var ExecutablePath = ((ManagementBaseObject)e["TargetInstance"])["ExecutablePath"];
            //Debug.WriteLine("进程创建：" + Processname + ",进程文件路径：" + ExecutablePath);
        }
    }
}
