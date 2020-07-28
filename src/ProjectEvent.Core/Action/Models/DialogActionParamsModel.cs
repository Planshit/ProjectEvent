using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class DialogActionParamsModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public Dictionary<string, string> Buttons { get; set; }
    }
}
