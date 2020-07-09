using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// 启动进程action输入UI模型
    /// </summary>
    public class StartProcessActionInputModel
    {
        public string Path { get; set; }
        public string Args { get; set; }
    }
}
