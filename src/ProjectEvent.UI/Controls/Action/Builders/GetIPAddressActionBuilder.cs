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
    public class GetIPAddressActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ComBoxModel> actionResultKeys;
        public GetIPAddressActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.GetIPAddress);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.GetIPAddress;
            inputData = new GetIPAddressActionInputModel()
            {
                //选择下拉框的需要指定默认选择
                Type = GetIPAddressActionData.IPAddressTypes[0]
            };
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.GetIPAddress);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Select,
                Title = "获取类型",
                IsStretch = true,
                SelectItems = GetIPAddressActionData.IPAddressTypes,
                BindingName = nameof(GetIPAddressActionInputModel.Type)
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
            var parameter = ObjectConvert.Get<GetIPAddressActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as GetIPAddressActionInputModel;
                data.Type = GetIPAddressActionData.GetIPAddressType((int)parameter.Type);
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as GetIPAddressActionInputModel;
            if (data != null)
            {
                action.Parameter = new GetIPAddressActionParamsModel()
                {
                    Type = (IPAddressType)data.Type.ID
                };
            }
        }
    }
}
