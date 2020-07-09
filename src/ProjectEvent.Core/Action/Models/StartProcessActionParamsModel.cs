using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class StartProcessActionParamsModel
    {
        /// <summary>
        /// 进程路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 参数（可选）
        /// </summary>
        public string Args{ get; set; }

    }
}
