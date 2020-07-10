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
    public class StartProcessActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        private List<ComBoxModel> actionResultKeys;
        public StartProcessActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.StartProcess);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.StartProcess;
            inputData = new StartProcessActionInputModel();
            inputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.StartProcess);
            detailInputModels = new List<ActionInputModel>();
            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "进程路径",
                Placeholder = "请输入进程路径",
                IsStretch = true,
                BindingName = nameof(StartProcessActionInputModel.Path)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "启动参数",
                BindingName = nameof(StartProcessActionInputModel.Args)
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
            var parameter = ObjectConvert.Get<StartProcessActionParamsModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as StartProcessActionInputModel;
                data.Path = parameter.Path;
                data.Args = parameter.Args;
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {
            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as StartProcessActionInputModel;
            if (data != null)
            {
                action.Parameter = new StartProcessActionParamsModel()
                {
                    Path = data.Path,
                    Args = data.Args
                };
            }
        }
    }
}
