using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnIntervalTimerCondition : ICondition
    {
        /// <summary>
        /// 间隔秒数
        /// </summary>
        public double Seconds { get; set; }
        /// <summary>
        /// 循环次数
        /// </summary>
        public int Num { get; set; }

        public ConditionCheckResultModel Check()
        {
            var result = new ConditionCheckResultModel();
            result.IsValid = true;

            if (Seconds <= 0)
            {
                result.IsValid = false;
                result.Msg = "间隔秒数不能小于等于0";
            }
            if (Num < 0)
            {
                result.IsValid = false;
                result.Msg = "循环次数不能小于0";
            }

            return result;
        }

        public bool IsPass(object data = null)
        {
            var eventLogs = EventLoger.Get(Event.Types.EventType.OnIntervalTimer);
            if (Num > 0 && eventLogs.Count >= Num)
            {
                return false;
            }
            return true;
        }
    }
}
