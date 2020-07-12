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
        private readonly IEventService eventService;
        private readonly ITimerService _timerService;
        public event EventHandler OnEventTrigger;
        public TimerTaskService(IEventService eventService,
            ITimerService timerService)
        {
            this.eventService = eventService;
            _timerService = timerService;

            eventService.OnAddEvent += EventService_OnAddEvent;
            eventService.OnRemoveEvent += EventService_OnRemoveEvent;
            eventService.OnUpdateEvent += EventService_OnUpdateEvent;
        }

        private void EventService_OnUpdateEvent(EventModel oldValue, EventModel newValue)
        {
            //旧值是否是时间事件
            bool oldIsTimeEvent = oldValue.EventType == Event.Types.EventType.OnIntervalTimer || oldValue.EventType == Event.Types.EventType.OnTimeChanged;
            //新值是否是时间事件
            bool newIsTimeEvent = newValue.EventType == Event.Types.EventType.OnIntervalTimer || newValue.EventType == Event.Types.EventType.OnTimeChanged;
            if (oldIsTimeEvent && !newIsTimeEvent)
            {
                //旧值是,新值不是时卸载timer
                _timerService.Close(oldValue.ID);
            }
            else if (oldIsTimeEvent && newIsTimeEvent)
            {
                //旧值新值都是timer事件时判断是否更改条件或事件类型
                if ((oldValue.EventType != newValue.EventType) || (oldValue.Condition != newValue.Condition))
                {
                    //条件或类型被修改

                    //关闭旧的timer
                    _timerService.Close(oldValue.ID);
                    //启动新的
                    Handle(newValue);
                }
            }
            else if (!oldIsTimeEvent && newIsTimeEvent)
            {
                //旧值不是新值是
                Handle(newValue);
            }
        }

        private void EventService_OnRemoveEvent(EventModel ev)
        {
            if (ev.EventType == Event.Types.EventType.OnTimeChanged || ev.EventType == Event.Types.EventType.OnIntervalTimer)
            {
                _timerService.Close(ev.ID);
            }
        }

        private void EventService_OnAddEvent(EventModel ev)
        {
            Handle(ev);
        }

        public void Run()
        {
            foreach (var ev in eventService.GetEvents())
            {
                Handle(ev);
            }
        }

        private void Handle(EventModel ev)
        {
            //指定日期的事件
            if (ev.EventType == Event.Types.EventType.OnTimeChanged)
            {
                var condition = ev.Condition as OnTimeChangedCondition;
                _timerService.StartNew(ev.ID, () =>
                 {
                     eventService.Invoke(ev, null);
                 }, condition.Time, condition.RepetitionType);

            }

            //计时循环事件
            if (ev.EventType == Event.Types.EventType.OnIntervalTimer)
            {
                var condition = ev.Condition as OnIntervalTimerCondition;
                _timerService.StartNew(ev.ID,
                    () =>
                    {
                        eventService.Invoke(ev, null);
                    },
                condition.Seconds,
                condition.Num);

            }
        }
    }
}
