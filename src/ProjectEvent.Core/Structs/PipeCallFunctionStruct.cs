using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Structs
{
    public struct PipeCallFunctionStruct
    {
        public int ID { get; set; }
        public PipeCallFunctionType CallFunctionType { get; set; }
        public object Data { get; set; }
    }
}
