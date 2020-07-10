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
    public class IFActionActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ComBoxModel> actionResultKeys;
        public IFActionActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.IF);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.IF;
            inputData = new IFActionInputModel()
            {
                Condition = IFActionConditionData.ComBoxData[0]
            };
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.IF);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "如果",
                Placeholder = "请输入",
                BindingName = nameof(IFActionInputModel.Left)
            });
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Select,
                SelectItems = IFActionConditionData.ComBoxData,
                BindingName = nameof(IFActionInputModel.Condition)
            });
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "请输入",
                BindingName = nameof(IFActionInputModel.Right)
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
            var parameter = ObjectConvert.Get<IFActionParameterModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as IFActionInputModel;
                data.Left = parameter.LeftInput;
                data.Right = parameter.RightInput;
                data.Condition = IFActionConditionData.GetCombox((int)parameter.Condition);
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
