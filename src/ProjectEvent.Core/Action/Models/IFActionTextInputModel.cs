using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class IFActionTextInputModel : BaseIFActionInputModel
    {
        /// <summary>
        /// 文本内容，支持变量
        /// </summary>
        public string Value { get; set; }
    }
}
