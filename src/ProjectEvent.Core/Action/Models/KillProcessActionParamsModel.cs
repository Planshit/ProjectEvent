using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class KillProcessActionParamsModel
    {
        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 模糊匹配
        /// </summary>
        public bool IsFuzzy { get; set; }
    }
}
