using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ProjectEvent.UI.Controls.InputGroup.Models
{
    public class InputModel
    {
        public InputType Type { get; set; }
        public string BindingName { get; set; }
        public string Placeholder { get; set; }

        public DependencyProperty BindingProperty { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 可选择项（仅输入类型是select时有效）
        /// </summary>
        public List<ComBoxModel> SelectItems { get; set; }
    }
}
