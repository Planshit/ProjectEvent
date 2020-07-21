﻿using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class SystemNotificationAction : IAction
    {
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                var p = ObjectConvert.Get<SystemNotificationActionParamsModel>(action.Parameter);
                p.Title = ActionParameterConverter.ConvertToString(taskID, p.Title);
                p.Content = ActionParameterConverter.ConvertToString(taskID, p.Content);
                p.Icon = ActionParameterConverter.ConvertToString(taskID, p.Icon);
                try
                {
                    NotificationService notificationService = new NotificationService();
                    notificationService.ShowNotification(p.Title, p.Content, p.ToastScenarioType, p.Icon);
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }
                //返回数据
                //ActionTaskResulter.Add(taskID, result);
            };
        }
    }
}