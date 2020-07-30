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
    public class KillProcessActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        private List<ComBoxModel> actionResultKeys;
        public KillProcessActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.KillProcess);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.KillProcess;
            inputData = new KillProcessActionInputModel();
            inputModels = new List<ActionInputModel>();
            detailInputModels = new List<ActionInputModel>();

            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.KillProcess);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Placeholder = "区分大小写，不需要填写.exe",
                Title = "进程名称",
                IsStretch = true,
                BindingName = nameof(KillProcessActionInputModel.ProcessName)
            });

            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Bool,
                Title = "模糊匹配",
                BindingName = nameof(KillProcessActionInputModel.IsFuzzy)
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
            var parameter = ObjectConvert.Get<KillProcessActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as KillProcessActionInputModel;
                data.ProcessName = parameter.ProcessName;
                data.IsFuzzy = parameter.IsFuzzy;
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as KillProcessActionInputModel;
            if (data != null)
            {
                action.Parameter = new KillProcessActionParamsModel()
                {
                    ProcessName = data.ProcessName,
                    IsFuzzy = data.IsFuzzy
                };
            }
        }
    }
}
