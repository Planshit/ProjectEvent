using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public class EventContainerService : IEventContainerService
    {
        public event ContainerEventHandler OnAddEvent;
        public event ContainerEventHandler OnRemoveEvent;

        private Dictionary<int, EventModel> _events;

        public EventContainerService()
        {
            _events = new Dictionary<int, EventModel>();
        }

        public bool Add(EventModel ev)
        {
            if (ev.ID == 0)
            {
                ev.ID = _events.Count + 1;
            }
            //检查重复ID
            if (_events.ContainsKey(ev.ID))
            {
                return false;
            }

            //检查条件是否输入正确

            if (!ev.Condition.Check().IsValid)
            {
                return false;
            }

            //检查action是否输入正确
            if (ev.Actions != null)
            {
                foreach (var action in ev.Actions)
                {
                    var actionCheck = new ActionBuilder(action.Action, action.Parameter).Check();
                    if (!actionCheck)
                    {
                        return false;
                    }
                }
            }
            _events.Add(ev.ID, ev);

            OnAddEvent?.Invoke(ev);
            return true;
        }

        public IEnumerable<EventModel> GetEvents()
        {
            return _events.Values;
        }
    }
}
