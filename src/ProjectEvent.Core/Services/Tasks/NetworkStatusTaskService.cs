using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Event.Structs;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class NetworkStatusTaskService : INetworkStatusTaskService
    {

        private readonly IEventService eventService;
        private bool iswatching = false;
        private NetworkWatcher networkWatcher;
        public NetworkStatusTaskService(IEventService eventContainerService)
        {
            eventService = eventContainerService;
            eventService.OnAddEvent += EventService_OnAddEvent;
            eventService.OnRemoveEvent += EventService_OnRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
            networkWatcher = new NetworkWatcher();
            networkWatcher.NetworkConnected += NetworkWatcher_NetworkConnected;
            networkWatcher.NetworkDisconnect += NetworkWatcher_NetworkDisconnect;
        }

        private void NetworkWatcher_NetworkDisconnect(object sender, EventArgs e)
        {
            var data = new NetworkStatusDataStruct()
            {
                IsConnected = false
            };
            HandleNetworkStatusEvent(data);
        }

        private void NetworkWatcher_NetworkConnected(object sender, EventArgs e)
        {
            var data = new NetworkStatusDataStruct()
            {
                IsConnected = true
            };
            HandleNetworkStatusEvent(data);
            //处理wifi连接事件
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface networkInterface = null;
            foreach (var item in networkInterfaces)
            {
                if (item.OperationalStatus == OperationalStatus.Up &&
                    item.Speed > 0 &&
                    item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    networkInterface = item;
                }
            }
            if (networkInterface != null)
            {
                HandleWIFIConnectedEvent(SystemHelper.GetCurrentWIFISSID());
            }
        }

        private void HandleNetworkStatusEvent(NetworkStatusDataStruct data)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.NetworkStatusEvent).
                ToList();

            foreach (var ev in evs)
            {
                eventService.Invoke(ev, data);
            }
        }

        private void HandleWIFIConnectedEvent(string data)
        {
            var evs = eventService.
                GetEvents().
                Where(m => m.EventType == Event.Types.EventType.WIFIConnectedEvent).
                ToList();

            foreach (var ev in evs)
            {
                eventService.Invoke(ev, data);
            }

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

        private void Watch()
        {
            if (eventService.
               GetEvents().
               Where(
                m => m.EventType == Event.Types.EventType.NetworkStatusEvent ||
                m.EventType == Event.Types.EventType.WIFIConnectedEvent
               ).Any())
            {
                if (!iswatching)
                {
                    networkWatcher.Watch();
                    iswatching = true;
                }
            }
            else
            {
                if (iswatching)
                {
                    //取消
                    networkWatcher.StopWatch();
                    iswatching = false;
                }
            }

        }
        public void Run()
        {
            Watch();
        }
    }
}
