using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class SystemNotificationActionInputModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ComBoxModel ToastScenarioType { get; set; }
        public string Icon { get; set; }
        public ComBoxModel ToastActionType { get; set; }
        public string Url { get; set; }

    }
}
