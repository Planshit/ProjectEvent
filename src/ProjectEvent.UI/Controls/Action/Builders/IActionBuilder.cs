using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Builders
{
    public interface IActionBuilder
    {
        /// <summary>
        /// 获取支持的操作结果返回
        /// </summary>
        /// <returns></returns>
        List<ComBoxModel> GetResultKeys();
        /// <summary>
        /// 获取用于UI的action item model
        /// </summary>
        /// <returns></returns>
        ActionItemModel GetActionItemModel();
        /// <summary>
        /// 获取用于core action model
        /// </summary>
        /// <returns></returns>
        Core.Action.Models.ActionModel GetCoreActionModel();
        /// <summary>
        /// 获取用于UI的基本输入模板
        /// </summary>
        /// <returns></returns>
        List<ActionInputModel> GetBaseActionInputModels();
        /// <summary>
        /// 获取用于UI的更多选项输入模板
        /// </summary>
        /// <returns></returns>
        List<ActionInputModel> GetDetailActionInputModels();
        /// <summary>
        ///获取输入数据
        /// </summary>
        /// <returns></returns>
        object GetInputModelData();
        /// <summary>
        /// 导入一个core action
        /// </summary>
        /// <param name="action"></param>
        void ImportAction(Core.Action.Models.ActionModel action);
        /// <summary>
        /// 导入一个actoin item
        /// </summary>
        /// <param name="actionItem"></param>
        void ImportActionItem(ActionItemModel actionItem);
        
    }
}
