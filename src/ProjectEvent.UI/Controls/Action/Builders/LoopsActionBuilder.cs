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
    public class LoopsActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ComBoxModel> actionResultKeys;
        public LoopsActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.Loops);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.Loops;
            inputData = new LoopsActionInputModel()
            {
                Count = 1
            };
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.Loops);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Number,
                Title = "次数",
                Placeholder = "请输入循环次数",
                BindingName = nameof(LoopsActionInputModel.Count)
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
            var parameter = ObjectConvert.Get<LoopsActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as LoopsActionInputModel;
                data.Count = parameter.Count;
            }
        }

        /// <summary>
        /// [特殊action，无效方法]
        /// </summary>
        /// <param name="actionItem"></param>
        public void ImportActionItem(ActionItemModel actionItem)
        {
        }
    }
}
