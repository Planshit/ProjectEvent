using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class KillProcessActionInputModel
    {
        public string ProcessName { get; set; }
        public bool IsFuzzy { get; set; }
    }
}
