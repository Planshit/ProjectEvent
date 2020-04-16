using ProjectEvent.Core.Action;
using ProjectEvent.Core.Condition;
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

            _eventContainerService.OnAddEvent += _eventContainerService_OnAddEvent;

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
            if (ev.EventType == Event.Types.EventType.OnTimerChanged)
            {
                if (_handledEventIDs.Contains(ev.ID))
                {
                    return;
                }
                _handledEventIDs.Add(ev.ID);

                var condition = ev.Condition as OnTimerChangedCondition;
                _timerService.StartNew(() =>
                {
                    foreach (var action in ev.Actions)
                    {
                        for (int i = 0; i < action.Num; i++)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                Debug.WriteLine(100);
                                new ActionBuilder(action.Action, action.Args).Builer().Invoke();
                            });
                        }
                    }

                    OnEventTrigger?.Invoke(ev, 0);
                }, condition.AtDateTime, condition.IsRepetition);

            }
        }


        private void _eventContainerService_OnAddEvent(EventModel ev)
        {
            Handle(ev);
        }
    }
}
