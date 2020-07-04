using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    public class EventLogModel
    {
        /// <summary>
        /// 日志ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectID { get; set; }
        /// <summary>
        /// 触发时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
