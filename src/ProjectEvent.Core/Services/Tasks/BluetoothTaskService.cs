using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services.Tasks
{
    public class BluetoothTaskService : IBluetoothTaskService
    {

        public void Run()
        {
            var bluetoothWin32Events = new BluetoothWin32Events();
            bluetoothWin32Events.InRange += BluetoothWin32Events_InRange; ;
        }

        private void BluetoothWin32Events_InRange(object sender, BluetoothWin32RadioInRangeEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
