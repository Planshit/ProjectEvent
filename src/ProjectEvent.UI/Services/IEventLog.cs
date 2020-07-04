using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public interface IEventLog
    {
        void Listen();
        void Save();
        /// <summary>
        /// 获取最近的一些日志
        /// </summary>
        /// <returns></returns>
        List<EventLogModel> GetEventLogs();
    }
}
