using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// 写文件action输入UI模型
    /// </summary>
    public class WriteFileActionInputModel
    {
        public string FilePath { get; set; }
        public string Content { get; set; }
    }
}
