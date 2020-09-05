using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class SystemNotificationActionParamsModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ToastScenarioType ToastScenarioType { get; set; }
        public string Icon { get; set; }
        public ToastActionType ToastActionType { get; set; }
        public string Url { get; set; }
    }
}
