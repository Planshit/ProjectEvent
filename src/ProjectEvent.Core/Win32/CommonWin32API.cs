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
            res.Height= GetSystemMetrics(SM_CYSCREEN);
            return res;
        }
    }
}
