using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class ActionResultValueModel
    {
        public ActoinResultValueType Type { get; set; }
        public object Value { get; set; }
    }
}
