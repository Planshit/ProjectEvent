using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Net.Types;
using ProjectEvent.Core.Types;
using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class SystemNotificationActionData
    {
        public static List<ComBoxModel> ToastScenarioTypes = new List<ComBoxModel>()
                {
                    new ComBoxModel()
                    {
                        ID=(int)ToastScenarioType.Default,
                        DisplayName="默认"
                    },
                    new ComBoxModel()
                    {
                        ID=(int)ToastScenarioType.Reminder,
                        DisplayName="提醒通知"
                    },
                    new ComBoxModel()
                    {
                        ID=(int)ToastScenarioType.Alarm,
                        DisplayName="警报通知"
                    },
                    new ComBoxModel()
                    {
                        ID=(int)ToastScenarioType.IncomingCall,
                        DisplayName="来电通知"
                    },
                };
        public static ComBoxModel GetToastScenarioType(int id)
        {
            return ToastScenarioTypes.Where(m => m.ID == id).FirstOrDefault();
        }
        public static List<ComBoxModel> ToastActionTypes = new List<ComBoxModel>()
                {
                    new ComBoxModel()
                    {
                        ID=(int)ToastActionType.Default,
                        DisplayName="默认（启动主程序或显示主窗口）"
                    },
                    new ComBoxModel()
                    {
                        ID=(int)ToastActionType.Url,
                        DisplayName="打开一个链接"
                    },
                   
                };
        public static ComBoxModel GetToastActionType(int id)
        {
            return ToastActionTypes.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
