using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Structs
{
    public struct PipeCallFunctionFeedbackStruct
    {
        public int ID { get; set; }
        public PipeCallFunctionType CallFunctionType { get; set; }
        public object FeedbackData { get; set; }
    }
}
