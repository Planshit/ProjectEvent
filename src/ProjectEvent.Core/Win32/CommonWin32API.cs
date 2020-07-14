using ProjectEvent.Core.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ProjectEvent.Core.Win32
{
    /// <summary>
    /// 通用win32 api
    /// </summary>
    public class CommonWin32API
    {
        #region 获取屏幕分辨率
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int SM);

        /// <summary>
        /// 获取屏幕分辨率
        /// </summary>
        /// <returns></returns>
        public static Rectangle GetScreenResolution()
        {
            var res = new Rectangle();
            res.Width = GetSystemMetrics(SM_CXSCREEN);
            res.Height = GetSystemMetrics(SM_CYSCREEN);
            return res;
        }
        #endregion

        #region 获取网络状态
        [DllImport("wininet.dll")]
        public static extern bool InternetGetConnectedState(out long lpdwFlags, long dwReserved);


        #endregion
    }
}
