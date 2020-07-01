using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    public class ProjectModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 方案描述
        /// </summary>
        public string ProjectDescription { get; set; }

        /// <summary>
        /// 事件ID
        /// </summary>
        public int EventID { get; set; }
        /// <summary>
        /// 条件数据
        /// </summary>
        public object ConditionData { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public List<Core.Action.Models.ActionModel> Actions { get; set; }
        /// <summary>
        /// 分组ID
        /// </summary>
        public int GroupID { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public IconTypes Icon { get; set; }
    }
}
