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
    public class DelayActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ComBoxModel> actionResultKeys;
        public DelayActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.Delay);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.Delay;
            inputData = new DelayActionInputModel()
            {
                Seconds = "3"
            };
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.Delay);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Number,
                Placeholder = "秒",
                Title = "延迟（秒）",
                IsStretch = true,
                BindingName = nameof(DelayActionInputModel.Seconds)
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
            var parameter = ObjectConvert.Get<DelayActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as DelayActionInputModel;
                data.Seconds = parameter.Seconds.ToString();
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as DelayActionInputModel;
            if (data != null)
            {
                action.Parameter = new DelayActionParamsModel()
                {
                    Seconds = int.Parse(data.Seconds)
                };
            }
        }
    }
}
