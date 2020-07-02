﻿using ProjectEvent.Core.Action;
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
        public ProcessTaskService(IEventService eventService)
        {
            this.eventService = eventService;
            EventQuery query = new EventQuery();
            query.QueryString = "SELECT * FROM" +
                " __InstanceCreationEvent WITHIN 1 " +
                "WHERE TargetInstance isa 'Win32_Process'";
            watcher = new ManagementEventWatcher(query);
            watcher.EventArrived += NewProcess_Created;
            eventService.OnAddEvent += _eventContainerService_OnAddEvent;
        }

        private void _eventContainerService_OnAddEvent(EventModel @event)
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

            if (hasEv)
            {
                watcher.Start();
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
                if(ev.EventType== Event.Types.EventType.OnProcessCreated)
                {
                    eventService.Invoke(ev, baseObject);
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
