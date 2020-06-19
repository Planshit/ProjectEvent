using Microsoft.Win32;
using System;
using System.IO;

namespace ProjectEvent.Core.Helper
{
    public class SystemHelper
    {
        /// <summary>
        /// 设置开机启动
        /// </summary>
        /// <param name="startup"></param>
        /// <returns></returns>
        public static bool SetStartup(bool startup = true)
        {
            string name = "ProjectEvent";
            string path = Path.Combine(
                   AppDomain.CurrentDomain.BaseDirectory,
                    "ProjectEvent.UI.exe");
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue(name) == null && startup)
            {
                rkApp.SetValue(name, $"\"{path}\" -autorun");
                return true;
            }
            if (rkApp.GetValue(name) != null && !startup)
            {
                rkApp.DeleteValue(name);
            }
            return false;
        }

    }
}
