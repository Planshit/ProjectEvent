using ProjectEvent.UI.Controls.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class ActionInputModel
    {
        public InputType InputType { get; set; }
        public string Title { get; set; }
        public string BindingName { get; set; }
        public List<ComBoxModel> SelectItems { get; set; }

    }
}
