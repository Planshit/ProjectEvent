using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// if action输入UI模型
    /// </summary>
    public class IFActionInputModel : UINotifyPropertyChanged
    {
        private string Left_ = "";
        public string Left { get { return Left_; } set { Left_ = value; } }
        private string Right_ = "";
        public string Right { get { return Right_; } set { Right_ = value; } }
        private ComBoxModel Condition_;
        public ComBoxModel Condition { get { return Condition_; } set { Condition_ = value; } }
    }
}
