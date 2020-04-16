using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnTimerChangedCondition : ICondition
    {
        /// <summary>
        /// 在某个时间
        /// </summary>
        public DateTime AtDateTime { get; set; }

        /// <summary>
        /// 是否重复
        /// </summary>
        public bool IsRepetition { get; set; }


        public ConditionCheckResultModel Check()
        {
            var result = new ConditionCheckResultModel();
            result.IsValid = true;

            if (!IsRepetition && AtDateTime < DateTime.Now)
            {
                result.IsValid = false;
                result.Msg = "在不选择重复时指定的时间应该大于当前时间";
            }

            return result;
        }

        public bool IsPass()
        {
            if (!Check().IsValid)
            {
                return false;
            }

            if (IsRepetition)
            {
                //重复表示每天

                return DateTime.Now.Hour == AtDateTime.Hour &&
                    DateTime.Now.Minute == AtDateTime.Minute;
            }
            else
            {
                //否则是指定唯一时间

                var dateA = new DateTime(AtDateTime.Year, AtDateTime.Month, AtDateTime.Day, AtDateTime.Hour, AtDateTime.Minute, 0);
                var dateB = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

                return dateA == dateB;
            }
        }
    }
}
