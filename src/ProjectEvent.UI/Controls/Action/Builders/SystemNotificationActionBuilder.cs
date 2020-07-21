using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Types;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Builders
{
    public class SystemNotificationActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        public SystemNotificationActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.SystemNotification);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.SystemNotification;
            inputData = new SystemNotificationActionInputModel()
            {
                ToastScenarioType = SystemNotificationActionData.ToastScenarioTypes[0]
            };
            inputModels = new List<ActionInputModel>();
            detailInputModels = new List<ActionInputModel>();

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入",
                Title = "标题",
                //IsStretch = true,
                BindingName = nameof(SystemNotificationActionInputModel.Title)
            });
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入",
                Title = "内容",
                //IsStretch = true,
                BindingName = nameof(SystemNotificationActionInputModel.Content)
            });

            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Select,
                Title = "通知类型",
                SelectItems = SystemNotificationActionData.ToastScenarioTypes,
                BindingName = nameof(SystemNotificationActionInputModel.ToastScenarioType)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "图标",
                Placeholder = "支持网络图片和本地图片",
                BindingName = nameof(SystemNotificationActionInputModel.Icon)
            });
        }
        public ActionItemModel GetActionItemModel()
        {
            ImportAction(action);
            return actionItem;
        }
        public Core.Action.Models.ActionModel GetCoreActionModel()
        {
            ImportActionItem(actionItem);
            return action;
        }
        public List<ActionInputModel> GetBaseActionInputModels()
        {
            return inputModels;
        }

        public List<ActionInputModel> GetDetailActionInputModels()
        {
            return detailInputModels;
        }

        public object GetInputModelData()
        {
            return inputData;
        }

        public List<ComBoxModel> GetResultKeys()
        {
            return null;
        }

        public void ImportAction(Core.Action.Models.ActionModel action)
        {

            this.action = action;
            //构建ui action
            actionItem.ID = action.ID;
            var parameter = ObjectConvert.Get<SystemNotificationActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as SystemNotificationActionInputModel;
                data.Title = parameter.Title;
                data.Content = parameter.Content;
                data.Icon = parameter.Icon;
                data.ToastScenarioType = SystemNotificationActionData.GetToastScenarioType((int)parameter.ToastScenarioType);
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as SystemNotificationActionInputModel;
            if (data != null)
            {
                action.Parameter = new SystemNotificationActionParamsModel()
                {
                    Title = data.Title,
                    Content = data.Content,
                    Icon = data.Icon,
                    ToastScenarioType = (Core.Types.ToastScenarioType)data.ToastScenarioType.ID
                };
            }
        }
    }
}
