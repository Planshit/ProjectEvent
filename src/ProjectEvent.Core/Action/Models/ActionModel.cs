using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class ActionModel
    {
        /// <summary>
        /// 执行次数
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 执行操作
        /// </summary>
        public ActionType Action { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>
        public object Parameter { get; set; }
        //public BaseParameterModel Parameter { get; set; }
    }
}
