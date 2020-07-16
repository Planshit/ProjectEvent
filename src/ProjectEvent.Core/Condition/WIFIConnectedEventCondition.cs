using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class WIFIConnectedEventCondition : ICondition
    {
        /// <summary>
        /// 连接指定WIFI名称
        /// </summary>
        public string SSID { get; set; }
        public ConditionCheckResultModel Check()
        {
            //特殊事件直接通过
            var result = new ConditionCheckResultModel();
            result.IsValid = true;
            return result;
        }

        public bool IsPass(object data = null)
        {
            var filterSSID = data != null ? data.ToString() : string.Empty;
            return string.IsNullOrEmpty(SSID) || SSID == filterSSID;
        }
    }
}
