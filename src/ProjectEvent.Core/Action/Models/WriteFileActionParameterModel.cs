﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class WriteFileActionParameterModel : BaseParameterModel
    {
        public string FilePath { get; set; }

        public string Content { get; set; }
    }
}
