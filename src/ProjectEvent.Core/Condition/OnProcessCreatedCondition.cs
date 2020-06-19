using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Event;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnProcessCreatedCondition : ICondition
    {
        /// <summary>
        /// 进程名
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /// 不区分大小写
        /// </summary>
        public bool Caseinsensitive { get; set; }
        /// <summary>
        /// 模糊匹配
        /// </summary>
        public bool FuzzyMatch { get; set; }


        public ConditionCheckResultModel Check()
        {
            var result = new ConditionCheckResultModel();
            result.IsValid = true;

            if (string.IsNullOrEmpty(ProcessName))
            {
                result.IsValid = false;
                result.Msg = "进程名不能为空";
            }
            return result;
        }

        public bool IsPass(object data = null)
        {
            if (data == null)
            {
                return false;
            }
            var e = data as ManagementBaseObject;
            string Name = ((ManagementBaseObject)e["TargetInstance"])["Name"].ToString();
            //string ExecutablePath = ((ManagementBaseObject)e["TargetInstance"])["ExecutablePath"].ToString();
            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }
            if (Caseinsensitive)
            {
                Name = Name.ToLower();
                ProcessName = ProcessName.ToLower();
            }
            if (FuzzyMatch)
            {
                return Name.IndexOf(ProcessName) != -1;
            }
            else
            {
                return Name == ProcessName;
            }
        }
    }
}
