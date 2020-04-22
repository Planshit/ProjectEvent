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
        public DependencyProperty BindingProperty { get; set; }
        public string Title { get; set; }
    }
}
