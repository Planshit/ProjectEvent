using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class IFActionParameterModel : BaseParameterModel
    {
        public BaseIFActionInputModel LeftInput { get; set; }
        public BaseIFActionInputModel RightInput { get; set; }

        public IFActionConditionType Condition { get; set; }

        public List<ActionModel> PassActions { get; set; }

        public List<ActionModel> NoPassActions { get; set; }
    }
}
