using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class ActionItemsData
    {
        /// <summary>
        /// 通过UI action type获取一个新的基础AcionItemModel
        /// </summary>
        /// <param name="uiactionType"></param>
        /// <returns></returns>
        public static ActionItemModel Get(ActionType uiactionType)
        {
            var action = new ActionItemModel();
            action.ActionType = uiactionType;
            action.ActionName = ActionNameData.Names[uiactionType];
            switch (uiactionType)
            {
                case ActionType.HttpRequest:
                    action.Icon = Base.IconTypes.DownloadDocument;
                    break;
                case ActionType.IF:
                    action.Icon = Base.IconTypes.FlowChart;
                    break;
                case ActionType.WriteFile:
                    action.Icon = Base.IconTypes.FileTemplate;
                    break;
                case ActionType.Shutdown:
                    action.Icon = Base.IconTypes.DeviceOff;
                    break;
            }

            return action;
        }
    }
}
