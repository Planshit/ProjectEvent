using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class BaseIFActionInputModel
    {
        /// <summary>
        /// 输入类型
        /// </summary>
        public IFActionInputType InputType { get; set; }
    }
}
