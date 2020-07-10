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
    public class ShutdownActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private List<ComBoxModel> actionResultKeys;
        public ShutdownActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.Shutdown);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.Shutdown;
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.Shutdown);


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
            return null;
        }

        public List<ActionInputModel> GetDetailActionInputModels()
        {
            return null;
        }

        public object GetInputModelData()
        {
            return null;
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
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {
            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
        }
    }
}
