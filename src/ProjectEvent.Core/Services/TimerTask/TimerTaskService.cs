using ProjectEvent.Core.Action;
using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Event;
using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Services.TimerTask
{
    public class TimerTaskService : ITimerTaskService
    {
        private readonly IEventContainerService _eventContainerService;
        private readonly ITimerService _timerService;

        public event EventHandler OnEventTrigger;

        private List<int> _handledEventIDs;

        public TimerTaskService(IEventContainerService eventContainerService,
            ITimerService timerService)
        {
            _eventContainerService = eventContainerService;
            _timerService = timerService;

            //_eventContainerService.OnAddEvent += _eventContainerService_OnAddEvent;

            _handledEventIDs = new List<int>();
        }



        public void Run()
        {
            foreach (var ev in _eventContainerService.GetEvents())
            {
                Handle(ev);
            }
        }

        private void Handle(EventModel ev)
        {
            if (_handledEventIDs.Contains(ev.ID))
            {
                Debug.WriteLine("已存在 event " + ev.ID);
                return;
            }

            //指定日期的事件
            if (ev.EventType == Event.Types.EventType.OnTimerChanged)
            {
                _handledEventIDs.Add(ev.ID);

                var condition = ev.Condition as OnTimerChangedCondition;
                _timerService.StartNew(() =>
                {
                    if (ActionTask.Invoke(ev))
                    {
                        OnEventTrigger?.Invoke(ev, 0);
                    }
                }, condition.AtDateTime, condition.IsRepetition);

            }

            //计时循环事件
            if (ev.EventType == Event.Types.EventType.OnIntervalTimer)
            {
                _handledEventIDs.Add(ev.ID);

                var condition = ev.Condition as OnIntervalTimerCondition;
                _timerService.StartNew(
                    () =>
                    {
                        if (ActionTask.Invoke(ev))
                        {
                            OnEventTrigger?.Invoke(ev, 0);
                        }
                    },
                condition.Seconds,
                condition.Num);

            }
        }


        private void _eventContainerService_OnAddEvent(EventModel ev)
        {
            Handle(ev);
        }
    }
}
