using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
namespace ProjectEvent.Core.Services
{
    public class NotificationService
    {
        /// <summary>
        /// 显示一条通知
        /// </summary>
        /// <param name="title">通知标题</param>
        /// <param name="content">通知内容</param>
        /// <param name="toastScenarioType">通知类型</param>
        /// <param name="img">图标（支持网络图片和本地图片）</param>
        public void ShowNotification(string title, string content, ToastScenarioType toastScenarioType = ToastScenarioType.Default, string img = null, ToastActionType toastActionType = ToastActionType.Default, string actionUrl = null)
        {
            string launchStr = " launch=\"" + (int)toastActionType + "`\" ";
            if (toastActionType == ToastActionType.Url && actionUrl != null)
            {
                launchStr = " launch=\"" + (int)toastActionType + "`" + actionUrl + "\" ";
            }
            string imgChild = !string.IsNullOrEmpty(img) ? $@"<image placement=""appLogoOverride"" hint-crop=""circle"" src=""{img}""/>" : "";
            string xml = $@"<toast{launchStr}scenario=""{toastScenarioType}"">
                      <visual>
                        <binding template='ToastGeneric'>
                          <text>{title}</text>
                          <text>{content}</text>
                          {imgChild}
                        </binding>
                      </visual>
                    </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            ToastNotification toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
