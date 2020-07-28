using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Services.TimerTask;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Event;
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

namespace ProjectEvent.UI.Services
{
    public class App : IApp
    {
        private readonly IEventService eventService;
        private readonly IMainService mainService;
        private readonly IProjects projects;
        private readonly IGroup group;
        private readonly IEventLog eventLog;
        private readonly ISettingsService settingsService;
        private readonly ICallFunctionPipes callFunctionPipes;

        public App(
            IEventService eventContainerService,
            IMainService mainService,
            IProjects projects,
            IGroup group,
            IEventLog eventLog,
            ISettingsService settingsService,
            ICallFunctionPipes callFunctionPipes
            )
        {
            this.eventService = eventContainerService;
            this.mainService = mainService;
            this.projects = projects;
            this.group = group;
            this.eventLog = eventLog;
            this.settingsService = settingsService;
            this.callFunctionPipes = callFunctionPipes;
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
            //启动管道调用服务
            callFunctionPipes.StartServer();
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
        public void Add(ProjectModel project)
        {
            eventService.Add(EventManager.CreateEventModel(project));
        }

        public void Update(ProjectModel project)
        {
            eventService.Update(EventManager.CreateEventModel(project));
        }
        public void Remove(int id)
        {
            eventService.Remove(id);
        }
    }
}
