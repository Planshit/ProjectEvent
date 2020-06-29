using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    /// <summary>
    /// 分组数据模型
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// 分组ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组图标
        /// </summary>
        public IconTypes Icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Index { get; set; }
    }
}
