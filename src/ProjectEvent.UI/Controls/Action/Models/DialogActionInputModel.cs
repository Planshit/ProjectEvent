using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class DialogActionInputModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public Dictionary<string, string> Buttons { get; set; }
    }
}
