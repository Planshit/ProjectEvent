using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    /// <summary>
    /// if action输入UI模型
    /// </summary>
    public class IFActionInputModel
    {
        public string Left { get; set; }
        public string Right { get; set; }
        public ComBoxModel Condition { get; set; }
    }
}
