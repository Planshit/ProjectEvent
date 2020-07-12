using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Condition.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnTimeChangedCondition : ICondition
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 重复方式
        /// </summary>
        public TimeChangedRepetitionType RepetitionType { get; set; }


        public ConditionCheckResultModel Check()
        {
            var result = new ConditionCheckResultModel();
            result.IsValid = true;

            //var nowTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            if (RepetitionType == TimeChangedRepetitionType.None && Time <= DateTime.Now)
            {
                result.IsValid = false;
                result.Msg = "在不选择重复时指定的时间应该大于当前时间";
            }

            return result;
        }

        public bool IsPass(object data = null)
        {
            if (!Check().IsValid)
            {
                return false;
            }

            if (RepetitionType == TimeChangedRepetitionType.Day)
            {
                //重复每天
                return DateTime.Now.Hour == Time.Hour &&
                    DateTime.Now.Minute == Time.Minute;
            }
            else if (RepetitionType == TimeChangedRepetitionType.Week)
            {
                //重复每周
                var isTime = DateTime.Now.Hour == Time.Hour &&
                   DateTime.Now.Minute == Time.Minute;
                var isWeek = DateTime.Now.DayOfWeek == Time.DayOfWeek;
                return isTime && isWeek;
            }
            else
            {
                //指定唯一时间
                var dateA = new DateTime(Time.Year, Time.Month, Time.Day, Time.Hour, Time.Minute, 0);
                var dateB = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                return dateA == dateB;
            }
        }
    }
}
