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
    public class RegexActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        private List<ComBoxModel> actionResultKeys;
        public RegexActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.Regex);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.Regex;
            inputData = new RegexActionInputModel();
            inputModels = new List<ActionInputModel>();
            detailInputModels = new List<ActionInputModel>();

            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.Regex);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入",
                Title = "内容",
                BindingName = nameof(RegexActionInputModel.Content)
            });
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入",
                Title = "正则表达式",
                BindingName = nameof(RegexActionInputModel.Regex)
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
            var parameter = ObjectConvert.Get<RegexActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as RegexActionInputModel;
                data.Content = parameter.Content;
                data.Regex = parameter.Regex;
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as RegexActionInputModel;
            if (data != null)
            {
                action.Parameter = new RegexActionParamsModel()
                {
                    Content = data.Content,
                    Regex = data.Regex
                };
            }
        }
    }
}
