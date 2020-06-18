using Newtonsoft.Json;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Services.TimerTask;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public class App : IApp
    {
        private readonly ITrayService trayService;
        private readonly ITimerTaskService timerTaskService;
        private readonly IEventContainerService eventContainerService;

        public App(
            ITrayService trayService,
            ITimerTaskService timerTaskService,
            IEventContainerService eventContainerService
            )
        {
            this.trayService = trayService;
            this.timerTaskService = timerTaskService;
            this.eventContainerService = eventContainerService;
        }
        public void Run()
        {
            trayService.Init();
            LoadProject();
        }

        private void LoadProject()
        {
            if (!Directory.Exists("Projects"))
            {
                Directory.CreateDirectory("Projects");
            }
            DirectoryInfo folder = new DirectoryInfo("Projects");
            int i = 0;
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                i++;
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                
            }
        }
    }
}
