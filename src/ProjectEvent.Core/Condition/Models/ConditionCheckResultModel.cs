using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition.Models
{
    /// <summary>
    /// 条件检查返回模型，在执行条件检查时返回。校验条件是否正确
    /// </summary>
    public class ConditionCheckResultModel
    {
        /// <summary>
        /// 是否正确
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
