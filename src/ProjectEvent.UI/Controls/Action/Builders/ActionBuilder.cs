using ProjectEvent.Core.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Builders
{
    public class ActionBuilder
    {
        public static IActionBuilder BuilderByActionItem(ActionItemModel action)
        {
            IActionBuilder builder = null;
            switch (action.ActionType)
            {
                case ActionType.WriteFile:
                    builder = new WriteFileActionBuilder();
                    break;
                case ActionType.IF:
                    builder = new IFActionActionBuilder();
                    break;
                case ActionType.HttpRequest:
                    builder = new HttpRequestActionBuilder();
                    break;
                case ActionType.StartProcess:
                    builder = new StartProcessActionBuilder();
                    break;
                case ActionType.Shutdown:
                    builder = new ShutdownActionBuilder();
                    break;
                case ActionType.OpenURL:
                    builder = new OpenURLActionBuilder();
                    break;
                case ActionType.Snipping:
                    builder = new SnippingActionBuilder();
                    break;
                case ActionType.DeleteFile:
                    builder = new DeleteFileActionBuilder();
                    break;
                case ActionType.SoundPlay:
                    builder = new SoundPlayActionBuilder();
                    break;
                case ActionType.GetIPAddress:
                    builder = new GetIPAddressActionBuilder();
                    break;
                case ActionType.Keyboard:
                    builder = new KeyboardActionBuilder();
                    break;
                case ActionType.SystemNotification:
                    builder = new SystemNotificationActionBuilder();
                    break;
                case ActionType.DownloadFile:
                    builder = new DownloadFileActionBuilder();
                    break;
            }
            if (builder != null)
            {
                builder.ImportActionItem(action);
            }
            return builder;
        }
        public static IActionBuilder BuilderByAction(ActionModel action)
        {
            IActionBuilder builder = null;
            switch (action.Action)
            {
                case Core.Action.Types.ActionType.WriteFile:
                    builder = new WriteFileActionBuilder();
                    break;
                case Core.Action.Types.ActionType.IF:
                    builder = new IFActionActionBuilder();
                    break;
                case Core.Action.Types.ActionType.HttpRequest:
                    builder = new HttpRequestActionBuilder();
                    break;
                case Core.Action.Types.ActionType.StartProcess:
                    builder = new StartProcessActionBuilder();
                    break;
                case Core.Action.Types.ActionType.Shutdown:
                    builder = new ShutdownActionBuilder();
                    break;
                case Core.Action.Types.ActionType.OpenURL:
                    builder = new OpenURLActionBuilder();
                    break;
                case Core.Action.Types.ActionType.Snipping:
                    builder = new SnippingActionBuilder();
                    break;
                case Core.Action.Types.ActionType.DeleteFile:
                    builder = new DeleteFileActionBuilder();
                    break;
                case Core.Action.Types.ActionType.SoundPlay:
                    builder = new SoundPlayActionBuilder();
                    break;
                case Core.Action.Types.ActionType.GetIPAddress:
                    builder = new GetIPAddressActionBuilder();
                    break;
                case Core.Action.Types.ActionType.Keyboard:
                    builder = new KeyboardActionBuilder();
                    break;
                case Core.Action.Types.ActionType.SystemNotification:
                    builder = new SystemNotificationActionBuilder();
                    break;
                case Core.Action.Types.ActionType.DownloadFile:
                    builder = new DownloadFileActionBuilder();
                    break;
            }
            if (builder != null)
            {
                builder.ImportAction(action);
            }
            return builder;
        }
    }
}
