using Microsoft.Win32;
using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class DeviceTaskService : IDeviceTaskService
    {
        private readonly IEventService eventService;


        public DeviceTaskService(IEventService eventContainerService)
        {
            eventService = eventContainerService;
        }


        public void Run()
        {
            var events = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.OnDeviceStartup
                ).
                ToList();

            foreach (var ev in events)
            {
                Handle(ev);
            }
        }


        private void Handle(EventModel ev)
        {
            eventService.Invoke(ev, null);
        }
    }
}
