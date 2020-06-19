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
        public event EventHandler OnEventTrigger;
        private readonly IEventContainerService _eventContainerService;


        public DeviceTaskService(IEventContainerService eventContainerService)
        {
            _eventContainerService = eventContainerService;

            //_eventContainerService.OnAddEvent += _eventContainerService_OnAddEvent;
        }
        public void Run()
        {
            var events = _eventContainerService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.OnDeviceStartup ||
                m.EventType == Event.Types.EventType.OnDeviceShutdown
                ).
                ToList();

            foreach (var ev in events)
            {
                Handle(ev);
            }
        }

        private void Handle(EventModel ev)
        {
            switch(ev.EventType)
            {
                case Event.Types.EventType.OnDeviceStartup:
                    if (ActionTask.Invoke(ev))
                    {
                        OnEventTrigger?.Invoke(ev, 0);
                    }
                    break;
            }
        }
    }
}
