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
    public class HttpRequestActionBuilder : IActionBuilder
    {
        private ActionItemModel actionItem;
        private Core.Action.Models.ActionModel action;
        private object inputData;
        private List<ActionInputModel> inputModels;
        private List<ActionInputModel> detailInputModels;

        private List<ComBoxModel> actionResultKeys;
        public HttpRequestActionBuilder()
        {
            actionItem = ActionData.GetCreateActionItemModel(UI.Types.ActionType.HttpRequest);
            action = new Core.Action.Models.ActionModel();
            action.Action = Core.Action.Types.ActionType.HttpRequest;
            inputData = new HttpRequestActionInputModel()
            {
                PamramsType = HttpRequestActionData.PamramsTypes[0],
                Method = HttpRequestActionData.MethodTypes[0]
            };
            inputModels = new List<ActionInputModel>();
            detailInputModels = new List<ActionInputModel>();
            actionResultKeys = ActionData.GetActionResults(UI.Types.ActionType.HttpRequest);

            inputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Text,
                Title = "请求地址",
                Placeholder = "请输入完整地址",
                IsStretch = true,
                BindingName = nameof(HttpRequestActionInputModel.Url)
            });

            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Select,
                Title = "方法",
                SelectItems = HttpRequestActionData.MethodTypes,
                BindingName = nameof(HttpRequestActionInputModel.Method)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.Select,
                Title = "参数类型",
                SelectItems = HttpRequestActionData.PamramsTypes,
                BindingName = nameof(HttpRequestActionInputModel.PamramsType)
            });

            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.CustomKeyValue,
                Title = "请求参数",
                BindingName = nameof(HttpRequestActionInputModel.QueryParams)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.CustomKeyValue,
                Title = "请求头",
                BindingName = nameof(HttpRequestActionInputModel.Headers)
            });
            detailInputModels.Add(new ActionInputModel()
            {
                InputType = Types.InputType.CustomKeyValue,
                Title = "文件（仅Form参数类型时有效）",
                BindingName = nameof(HttpRequestActionInputModel.Files)
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
            var parameter = ObjectConvert.Get<HttpRequestActionParameterModel>(action.Parameter);
            if (parameter != null)
            {
                var data = inputData as HttpRequestActionInputModel;
                data.Url = parameter.Url;
                data.Files = parameter.Files;
                data.Headers = parameter.Headers;
                data.Method = HttpRequestActionData.GetMethodType((int)parameter.Method);
                data.PamramsType = HttpRequestActionData.GetPamramsType((int)parameter.ParamsType);
                data.QueryParams = parameter.QueryParams;
            }
        }

        public void ImportActionItem(ActionItemModel actionItem)
        {

            this.actionItem = actionItem;
            //构建core action
            action.ID = actionItem.ID;
            var data = inputData as HttpRequestActionInputModel;
            if (data != null)
            {
                action.Parameter = new HttpRequestActionParameterModel()
                {
                    Files = data.Files,
                    Headers = data.Headers,
                    Method = (MethodType)data.Method.ID,
                    ParamsType = (ParamsType)data.PamramsType.ID,
                    QueryParams = data.QueryParams,
                    Url = data.Url
                };
            }
        }
    }
}
