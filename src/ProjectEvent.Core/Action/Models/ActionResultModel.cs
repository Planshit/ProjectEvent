using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class ActionResultModel
    {
        public int ID { get; set; }

        //public Dictionary<int, string> Result { get; set; }
        public Dictionary<int, object> Result { get; set; }

        //public Dictionary<int, ActionResultValueModel> Result { get; set; }
    }
}
