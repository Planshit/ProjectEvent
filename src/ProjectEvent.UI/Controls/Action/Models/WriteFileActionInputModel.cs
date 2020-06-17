using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// 写文件action输入UI模型
    /// </summary>
    public class WriteFileActionInputModel : UINotifyPropertyChanged
    {
        private string FilePath_ = "";
        public string FilePath { get { return FilePath_; } set { FilePath_ = value; } }
        private string Content_ = "";
        public string Content { get { return Content_; } set { Content_ = value; } }
    }
}
