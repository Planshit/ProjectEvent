using ProjectEvent.Core.Event.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Models
{
    /// <summary>
    /// 事件日志模型
    /// </summary>
    public class EventLogModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType { get; set; }
        /// <summary>
        /// 自定义标签
        /// </summary>
        public object Tag { get; set; }

    }
}
