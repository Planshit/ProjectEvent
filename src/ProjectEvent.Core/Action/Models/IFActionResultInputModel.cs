using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class IFActionResultInputModel : BaseIFActionInputModel
    {
        /// <summary>
        /// 获取的目标action id
        /// </summary>
        public int ActionID { get; set; }

        /// <summary>
        /// 返回数据集合的key
        /// </summary>
        public int ResultKey { get; set; }
    }
}
