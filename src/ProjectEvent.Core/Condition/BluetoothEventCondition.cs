using ProjectEvent.Core.Condition.Models;
using ProjectEvent.Core.Event.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class BluetoothEventCondition : ICondition
    {
        /// <summary>
        /// 过滤设备名称
        /// </summary>
        public string DeviceName { get; set; }
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
            //特殊事件直接通过
            var result = new ConditionCheckResultModel();
            result.IsValid = true;
            return result;
        }

        public bool IsPass(object data = null)
        {
            var eventData = (BluetoothEventDataStruct)data;
            if (string.IsNullOrEmpty(DeviceName))
            {
                return true;
            }

            if (Caseinsensitive)
            {
                DeviceName = DeviceName.ToLower();
                eventData.DeviceName = eventData.DeviceName.ToLower();
            }
            if (FuzzyMatch)
            {
                return eventData.DeviceName.Contains(DeviceName);
            }
            return DeviceName == eventData.DeviceName;
        }
    }
}
