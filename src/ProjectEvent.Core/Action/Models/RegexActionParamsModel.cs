using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class RegexActionParamsModel
    {
        public string Content { get; set; }
        /// <summary>
        /// 正则表达式（不支持任何变量
        /// </summary>
        public string Regex { get; set; }
    }
}
