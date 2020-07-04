using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Services.TimerTask;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ProjectEvent.UI.Services
{
    public class App : IApp
    {
        private readonly ITrayService trayService;
        private readonly IEventService eventService;
        private readonly IMainService mainService;
        private readonly IProjects projects;
        private readonly IGroup group;
        private readonly IEventLog eventLog;
        private readonly ISettingsService settingsService;
        public App(
            ITrayService trayService,
            IEventService eventContainerService,
            IMainService mainService,
            IProjects projects,
            IGroup group,
            IEventLog eventLog,
            ISettingsService settingsService
            )
        {
            this.trayService = trayService;
            this.eventService = eventContainerService;
            this.mainService = mainService;
            this.projects = projects;
            this.group = group;
            this.eventLog = eventLog;
            this.settingsService = settingsService;
        }
        public void Run()
        {
            //加载配置项
            settingsService.Load();
            //初始化事件日志
            eventLog.Listen();
            //加载自动化方案
            LoadProject();
            //启动主服务
            mainService.Run();
            //加载分组数据
            group.Load();


            //初始化托盘功能（必须在主要服务初始化后进行）
            trayService.Init();
        }
        private void LoadProject()
        {
            //查找并加载方案数据
            projects.LoadProjects();

            foreach (var project in projects.GetProjects())
            {
                if (project != null && project.EventID > 0)
                {
                    Add(project);
                }
            }
        }

        private Core.Event.Models.EventModel CreateEventModel(ProjectModel project)
        {
            ICondition condition = null;
            switch ((EventType)project.EventID)
            {
                case EventType.OnDeviceStartup:
                    condition = new OnDeviceStartupCondition();
                    break;
                case EventType.OnIntervalTimer:
                    var ttimerconditionData = ObjectConvert.Get<IntervalTimerConditionModel>(project.ConditionData);
                    if (ttimerconditionData != null)
                    {
                        condition = new OnIntervalTimerCondition()
                        {
                            Num = int.Parse(ttimerconditionData.Num),
                            Seconds = int.Parse(ttimerconditionData.Second)
                        };
                    }
                    break;
                case EventType.OnProcessCreated:
                    var pcconditionData = ObjectConvert.Get<ProcessCreatedConditionModel>(project.ConditionData);
                    condition = new OnProcessCreatedCondition()
                    {
                        ProcessName = pcconditionData.ProcessName,
                        Caseinsensitive = pcconditionData.Caseinsensitive,
                        FuzzyMatch = pcconditionData.FuzzyMatch
                    };
                    break;
            }
            return new Core.Event.Models.EventModel()
            {
                ID = project.ID,
                EventType = (EventType)project.EventID,
                Condition = condition,
                Actions = project.Actions
            };
        }

        public void Add(ProjectModel project)
        {
            eventService.Add(CreateEventModel(project));
        }

        public void Update(ProjectModel project)
        {
            eventService.Update(CreateEventModel(project));
        }

        public void Remove(int id)
        {
            eventService.Remove(id);
        }
    }
}
