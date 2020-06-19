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
            switch (uiactionType)
            {
                case ActionType.HttpGet:
                    action.ActionName = "HTTP GET请求";
                    action.Icon = "\xEC27";
                    break;
                case ActionType.IF:
                    action.ActionName = "判断";
                    action.Icon = "\xE9D4";
                    break;
                case ActionType.IFElse:
                    action.ActionName = "否则";
                    break;
                case ActionType.IFEnd:
                    action.ActionName = "判断结束";
                    break;
                case ActionType.WriteFile:
                    action.ActionName = "创建文件";
                    action.Icon = "\xF2E6";
                    break;
            }

            return action;
        }
    }
}
