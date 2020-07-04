using ProjectEvent.UI.Models;
using ProjectEvent.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class EventLogPageVM : EventLogPageModel
    {
        private readonly IEventLog eventLog;
        private readonly IProjects projects;
        public EventLogPageVM(
            IEventLog eventLog,
            IProjects projects)
        {
            this.eventLog = eventLog;
            this.projects = projects;
            ReadLogText();
        }

        private void ReadLogText()
        {
            var logs = eventLog.GetEventLogs().OrderByDescending(m => m.ID);
            foreach (var log in logs)
            {
                var project = projects.GetProject(log.ProjectID);
                string name = "[已被删除]";
                if (project != null)
                {
                    name = project.ProjectName;
                }
                Log += $"{log.ID}\r\n方案：{name}，时间：{log.Time.ToString()}\r\n-----\r\n";
            }
        }
    }
}
