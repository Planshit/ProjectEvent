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
using System.Text;
using System.Windows;

namespace ProjectEvent.UI.Services
{
    public class App : IApp
    {
        private readonly ITrayService trayService;
        private readonly IEventContainerService eventContainerService;
        private readonly IMainService mainService;
        public App(
            ITrayService trayService,
            IEventContainerService eventContainerService,
            IMainService mainService
            )
        {
            this.trayService = trayService;
            this.eventContainerService = eventContainerService;
            this.mainService = mainService;
        }
        public void Run()
        {
            //初始化托盘功能
            trayService.Init();
            //加载项目
            LoadProject();
            //初始化应用
            InitApp();
            //启动主服务
            mainService.Run();
        }

        private void LoadProject()
        {
            DirectoryInfo folder = IOHelper.GetDirectoryInfo("Projects");
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                if (project != null && project.EventID > 0)
                {
                    CreateEvent(project);
                }
            }
        }

        private void CreateEvent(ProjectModel project)
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

            eventContainerService.Add(new Core.Event.Models.EventModel()
            {
                EventType = (EventType)project.EventID,
                Condition = condition,
                Actions = project.Actions
            });
        }

        private void InitApp()
        {
            SystemHelper.SetStartup();
        }
    }
}
