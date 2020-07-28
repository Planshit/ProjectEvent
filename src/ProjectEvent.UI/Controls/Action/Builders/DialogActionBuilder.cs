using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net.Types;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Builders
{
    public class DialogActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        private List<ComBoxModel> actionResultKeys;
        public DialogActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.Dialog);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.Dialog;
            inputData = new DialogActionInputModel();
            inputModels = new List<ActionInputModel>();
            detailInputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.Dialog);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "标题",
                Placeholder = "请输入",
                BindingName = nameof(DialogActionInputModel.Title)
            });

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "内容",
                Placeholder = "请输入",
                BindingName = nameof(DialogActionInputModel.Content)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "图标",
                Placeholder = "请输入图标文件路径，仅支持本地图片",
                BindingName = nameof(DialogActionInputModel.Image)
            });

            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.CustomKeyValue,
                Title = "按钮",
                BindingName = nameof(DialogActionInputModel.Buttons)
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
            return actionResultKeys;
        }

        public void ImportAction(Core.Action.Models.ActionModel action)
        {
            this.action = action;
            //构建ui action
            actionItem.ID = action.ID;
            var parameter = ObjectConvert.Get<DialogActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as DialogActionInputModel;
                data.Title = parameter.Title;
                data.Content = parameter.Content;
                data.Image = parameter.Image;
                data.Buttons = parameter.Buttons;
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as DialogActionInputModel;
            if (data != null)
            {
                action.Parameter = new DialogActionParamsModel()
                {
                    Title = data.Title,
                    Content = data.Content,
                    Image = data.Image,
                    Buttons = data.Buttons,
                };
            }
        }
    }
}
