using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Services
{
    public class EventService : IEventService
    {
        public event ContainerEventHandler OnAddEvent;
        public event ContainerEventHandler OnRemoveEvent;
        public event EventHandler OnEventTrigger;
        public event EventChangedHandler OnUpdateEvent;

        private Dictionary<int, EventModel> _events;
        private int taskID = 0;
        public EventService()
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
            //检查条件输入是否无误
            if (ev.Condition != null && !ev.Condition.Check().IsValid)
            {
                Debug.WriteLine("条件输入验证不通过");
                return false;
            }
            _events.Add(ev.ID, ev);

            OnAddEvent?.Invoke(ev);
            return true;
        }

        public IEnumerable<EventModel> GetEvents()
        {
            return _events.Values;
        }

        public void Invoke(EventModel ev, object data)
        {
            bool isSuccess = ev.Condition.IsPass(data);

            if (isSuccess)
            {
                Task.Factory.StartNew(() =>
                {
                    taskID++;

                    //储存事件临时对象数据，事件变量
                    EventTemporaryObject.Add(taskID, ev, data);

                    //执行actions
                    ActionTask.Invoke(taskID, ev.Actions);


                });
                //响应触发事件
                OnEventTrigger?.Invoke(ev, true);
                //记录event触发
                EventLoger.Add(new EventLogModel()
                {
                    EventType = ev.EventType,
                    IsSuccess = isSuccess,
                });
            }

        }

        public void Remove(int id)
        {
            if (_events.ContainsKey(id))
            {
                var ev = _events[id].Copy();
                _events.Remove(id);
                OnRemoveEvent?.Invoke(ev);
            }
        }

        public void Update(EventModel ev)
        {
            if (_events.ContainsKey(ev.ID))
            {
                var oldEV = _events[ev.ID].Copy();

                _events[ev.ID] = ev;

                OnUpdateEvent?.Invoke(oldEV, ev);

            }
            else
            {
                Add(ev);
            }
        }


    }
}
