using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Structs
{
    public struct BluetoothEventDataStruct
    {
        public string DeviceName { get; set; }
        public bool IsConnected { get; set; }
    }
}
