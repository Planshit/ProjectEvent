using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    /// <summary>
    /// 组类型的action基础类
    /// </summary>
    public class BaseGroupActionParamsModel
    {
        /// <summary>
        /// 组内的action
        /// </summary>
        public List<ActionModel> Actions { get; set; }
    }
}
