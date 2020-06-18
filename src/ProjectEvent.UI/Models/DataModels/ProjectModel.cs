using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    public class ProjectModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
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
    }
}
