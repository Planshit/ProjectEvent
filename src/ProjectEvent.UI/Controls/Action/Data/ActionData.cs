using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;
using ActionType = ProjectEvent.UI.Types.ActionType;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class ActionData
    {
        #region 通过UI action type获取一个新的基础AcionItemModel
        /// <summary>
        /// 通过UI action type获取一个新的基础AcionItemModel
        /// </summary>
        /// <param name="uiactionType"></param>
        /// <returns></returns>
        public static ActionItemModel GetCreateActionItemModel(ActionType uiactionType)
        {
            var action = new ActionItemModel();
            action.ActionType = uiactionType;
            action.ActionName = ActionData.Names[uiactionType];
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
                case ActionType.StartProcess:
                    action.Icon = Base.IconTypes.ProcessingRun;
                    break;
            }

            return action;
        }
        #endregion

        #region action names
        public static Dictionary<ActionType, string> Names = new Dictionary<ActionType, string>()
        {
            {
                ActionType.HttpRequest,"HTTP请求"
            },
            {
                ActionType.IF,"判断"
            },
            {
                ActionType.IFElse,"否则"
            },
            {
                ActionType.IFEnd,"判断结束"
            },
            {
                ActionType.WriteFile,"创建文件"
            },
            {
                ActionType.Shutdown,"关闭电脑"
            },
            {
                ActionType.StartProcess,"启动进程"
            },
        };
        #endregion

        #region 通过action type获取支持的返回操作结果
        /// <summary>
        /// 通过action type获取支持的返回操作结果
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static List<ComBoxModel> GetActionResults(ActionType actionType)
        {
            List<ComBoxModel> data = null;
            switch (actionType)
            {
                case UI.Types.ActionType.HttpRequest:
                    //http请求
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)HttpResultType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
            new ComBoxModel()
            {
                ID = (int)HttpResultType.StatusCode,
                DisplayName = "状态码"
            },
            new ComBoxModel()
            {
                ID = (int)HttpResultType.Content,
                DisplayName = "响应内容"
            },

        };
                    break;
                case UI.Types.ActionType.WriteFile:
                    //写文件
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)CommonResultKeyType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
        };
                    break;
                case UI.Types.ActionType.StartProcess:
                    //启动进程
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)StartProcessResultType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Handle,
                DisplayName = "句柄（仅成功时有效）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Id,
                DisplayName = "进程ID（仅成功时有效）"
            },

        };
                    break;
            }
            return data;
        }
        #endregion
    }
}
