using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class GetIPAddressActionParamsModel
    {
        /// <summary>
        /// 获取类型
        /// </summary>
        public IPAddressType Type { get; set; }
    }
}
