using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
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
                Parameter = new WriteFileActionParameterModel()
                {
                    FilePath = "d:\\hello_project_event.txt",
                    Content = ":)"
                },
                Num = 1
            });


            //test if action
            var ifPassActions = new List<Core.Action.Models.ActionModel>();
            ifPassActions.Add(new Core.Action.Models.ActionModel()
            {
                Action = Core.Action.Types.ActionType.WriteFile,
                Parameter = new WriteFileActionParameterModel()
                {
                    FilePath = "d:\\ifpass.txt",
                    Content = ":)"
                },
                Num = 1
            });
            var ifUnPassActions = new List<Core.Action.Models.ActionModel>();
            ifUnPassActions.Add(new Core.Action.Models.ActionModel()
            {
                Action = Core.Action.Types.ActionType.WriteFile,
                Parameter = new WriteFileActionParameterModel()
                {
                    FilePath = "d:\\unifpass.txt",
                    Content = ":)"
                },
                Num = 1
            });
            //actions.Add(new Core.Action.Models.ActionModel()
            //{
            //    Action = Core.Action.Types.ActionType.IF,
            //    Parameter = new IFActionParameterModel()
            //    {
            //        LeftInput = new IFActionResultInputModel()
            //        {
            //            InputType = Core.Action.Types.IFActionInputType.ActionResult,
            //            ActionID = 1,
            //            ResultKey = (int)CommonResultKeyType.Status
            //        },
            //        RightInput = new IFActionTextInputModel()
            //        {
            //            InputType = Core.Action.Types.IFActionInputType.Text,
            //            Value = "bool"
            //        },
            //        Condition = Core.Action.Types.IFActionConditionType.UnEqual,
            //        PassActions = ifPassActions,
            //        NoPassActions = ifUnPassActions
            //    },
            //    Num = 1
            //});
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
                    Num = 1
                },
                Actions = actions,
            });

            _timerTaskService.Run();
        }
    }
}
