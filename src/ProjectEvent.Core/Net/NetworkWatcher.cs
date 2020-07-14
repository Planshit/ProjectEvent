using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ProjectEvent.Core.Net
{
    public class NetworkWatcher
    {
        /// <summary>
        /// 网络连接时发生
        /// </summary>
        public event EventHandler NetworkConnected;
        /// <summary>
        /// 网络断开时发生
        /// </summary>
        public event EventHandler NetworkDisconnect;

        /// <summary>
        /// 当前网络状态
        /// </summary>
        private bool isconnected = false;

        private bool iswatching = false;
        private long lfag;
        public NetworkWatcher()
        {
            isconnected = CommonWin32API.InternetGetConnectedState(out lfag, 0);
        }

        public void Watch()
        {
            if (!iswatching)
            {
                iswatching = true;
                NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            }
        }

        public void StopWatch()
        {
            if (iswatching)
            {
                iswatching = false;
                NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
            }
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            bool isc = CommonWin32API.InternetGetConnectedState(out lfag, 0);
            if (isc != isconnected)
            {
                isconnected = isc;
                if (isc)
                {
                    NetworkConnected?.Invoke(this, e);
                }
                else
                {
                    NetworkDisconnect?.Invoke(this, e);
                }
            }
        }
    }
}
