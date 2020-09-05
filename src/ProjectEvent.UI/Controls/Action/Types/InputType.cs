using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Types
{
    public enum InputType
    {
        /// <summary>
        /// 文本输入
        /// </summary>
        Text,
        /// <summary>
        /// 下拉选择
        /// </summary>
        Select,
        /// <summary>
        /// 自定义键值
        /// </summary>
        CustomKeyValue,
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 单选
        /// </summary>
        Bool,
        /// <summary>
        /// 多行文本输入
        /// </summary>
        MultiLineText,
    }
}
