using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Services.TimerTask;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public class MainService : IMainService
    {
        private readonly ITimerTaskService _timerTaskService;
        private readonly IEventContainerService _eventContainerService;

        public MainService(ITimerTaskService timerTaskService,
            IEventContainerService eventContainerService)
        {
            _timerTaskService = timerTaskService;
            _eventContainerService = eventContainerService;
        }

        public void Start()
        {
            var actions = new List<Core.Action.Models.ActionModel>();
            actions.Add(new Core.Action.Models.ActionModel()
            {
                Action = Core.Action.Types.ActionType.WriteFile,
                Args = new string[] { "d:\\hello_project_event.txt", ":)" },
                Num = 1
            });

            actions.Add(new Core.Action.Models.ActionModel()
            {
                Action = Core.Action.Types.ActionType.IF,
                Args = new string[] { "d:\\hello_project_event.txt", ":)" },
                Num = 1
            });
            //_eventContainerService.Add(new Event.Models.EventModel()
            //{
            //    EventType = Event.Types.EventType.OnTimerChanged,
            //    Condition = new OnTimerChangedCondition()
            //    {
            //        AtDateTime = new DateTime(2020, 4, 17, 15, 58, 0),
            //        IsRepetition = false
            //    },
            //    Actions = actions
            //});
            //计时循环
            _eventContainerService.Add(new Event.Models.EventModel()
            {
                EventType = Event.Types.EventType.OnIntervalTimer,
                Condition = new OnIntervalTimerCondition()
                {
                    Seconds = 3,
                    Num = 2
                },
                Actions = actions,
            });

            _timerTaskService.Run();
        }
    }
}
