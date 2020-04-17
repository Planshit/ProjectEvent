using ProjectEvent.Core.Event.Models;
using ProjectEvent.Core.Event.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Event
{
    public static class EventLoger
    {
        private static List<EventLogModel> _logs = new List<EventLogModel>();

        public static void Add(EventLogModel eventLog)
        {
            eventLog.ID = _logs.Count + 1;
            eventLog.Time = DateTime.Now;
            _logs.Add(eventLog);
        }

        public static List<EventLogModel> Get(EventType eventType)
        {
            return _logs.Where(m => m.EventType == eventType).ToList();
        }
    }
}
