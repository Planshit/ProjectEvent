using InTheHand.Net.Bluetooth;
using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Event.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class BluetoothTaskService : IBluetoothTaskService
    {
        private BluetoothWin32Events bluetoothWin32Events;
        private bool isWatching = false;
        private readonly IEventService eventService;
        public BluetoothTaskService(IEventService eventService)
        {
            this.eventService = eventService;
            eventService.OnAddEvent += EventService_OnAddEvent;
            eventService.OnRemoveEvent += EventService_OnRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            Watch();
        }

        private void EventService_OnRemoveEvent(EventModel ev)
        {
            Watch();
        }

        private void EventService_OnAddEvent(EventModel ev)
        {
            Watch();
        }


        private void HandleEvent(BluetoothEventDataStruct data)
        {
            var evs = eventService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.BluetoothEvent).
               ToList();

            foreach (var ev in evs)
            {
                eventService.Invoke(ev, data);
            }

        }

        private void Watch()
        {
            if (eventService.
               GetEvents().
               Where(m => m.EventType == Event.Types.EventType.BluetoothEvent).Any())
            {
                if (!isWatching && BluetoothRadio.IsSupported)
                {
                    isWatching = true;
                    bluetoothWin32Events = new BluetoothWin32Events();
                    bluetoothWin32Events.InRange += BluetoothWin32Events_InRange;
                }

            }
            else
            {
                if (isWatching)
                {
                    isWatching = false;
                    bluetoothWin32Events.InRange -= BluetoothWin32Events_InRange;
                    bluetoothWin32Events = null;
                }
            }

        }
        public void Run()
        {
            Watch();
        }

        private void BluetoothWin32Events_InRange(object sender, BluetoothWin32RadioInRangeEventArgs e)
        {
            var data = new BluetoothEventDataStruct();
            data.DeviceName = e.Device.DeviceName;
            data.IsConnected = e.Device.Connected;
            HandleEvent(data);
        }
    }
}
