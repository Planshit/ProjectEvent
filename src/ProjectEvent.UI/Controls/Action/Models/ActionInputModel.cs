using ProjectEvent.UI.Controls.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// action输入模型
    /// </summary>
    public class ActionInputModel
    {
        /// <summary>
        /// 输入类型
        /// </summary>
        public InputType InputType { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 绑定到数据键名
        /// </summary>
        public string BindingName { get; set; }
        /// <summary>
        /// 占位符
        /// </summary>
        public string Placeholder { get; set; }
        /// <summary>
        /// 可选择的项目（仅在输入类型是下拉选择时有效）
        /// </summary>

        public List<ComBoxModel> SelectItems { get; set; }
        /// <summary>
        /// 是否拉伸（仅在单行输入模板中且仅有一个参数时有效）
        /// </summary>
        public bool IsStretch { get; set; } = false;

    }
}
