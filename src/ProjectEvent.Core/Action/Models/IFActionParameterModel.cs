using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class IFActionParameterModel : BaseParameterModel
    {
        public List<ActionModel> PassActions { get; set; }

        public List<ActionModel> NoPassActions { get; set; }

    }
}
