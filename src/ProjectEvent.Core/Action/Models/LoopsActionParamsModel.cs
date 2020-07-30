using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class LoopsActionParamsModel : BaseGroupActionParamsModel
    {
        /// <summary>
        /// 循环次数（为0时永远循环）
        /// </summary>
        public int Count { get; set; }

    }
}
