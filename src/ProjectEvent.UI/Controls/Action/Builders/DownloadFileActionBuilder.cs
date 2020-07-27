using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Builders
{
    public class DownloadFileActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ComBoxModel> actionResultKeys;
        public DownloadFileActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.DownloadFile);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.DownloadFile;
            inputData = new DownloadFileActionInputModel();
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.DownloadFile);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入网络文件链接",
                Title = "网络文件链接",
                BindingName = nameof(DownloadFileActionInputModel.Url)
            });
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入保存路径，包括文件名",
                Title = "保存路径",
                BindingName = nameof(DownloadFileActionInputModel.SavePath)
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
            return null;
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
            var parameter = ObjectConvert.Get<DownloadFileParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as DownloadFileActionInputModel;
                data.Url = parameter.Url;
                data.SavePath = parameter.SavePath;

            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as DownloadFileActionInputModel;
            if (data != null)
            {
                action.Parameter = new DownloadFileParamsModel()
                {
                    Url = data.Url,
                    SavePath = data.SavePath
                };
            }
        }
    }
}
