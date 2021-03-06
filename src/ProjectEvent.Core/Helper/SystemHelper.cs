﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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

        /// <summary>
        /// 获取当前登录用户名
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserName()
        {
            string name = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (name.IndexOf('\\') != -1)
            {
                return name.Split('\\')[1];
            }
            return string.Empty;
        }
        //public static void GetStartupTime()
        //{
        //    var process = Process.GetProcesses().Where(m => m.ProcessName == "explorer").FirstOrDefault();

        //    try
        //    {
        //        Debug.WriteLine(process.ProcessName);
        //        Debug.WriteLine(process.StartTime);

        //    }
        //    catch
        //    {

        //    }


        //}
        /// <summary>
        /// 获取当前连接的无线网络SSID
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentWIFISSID()
        {
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "netsh.exe",
                    Arguments = "wlan show interfaces",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var line = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(l => l.Contains("SSID") && !l.Contains("BSSID"));
            string ssid = string.Empty;
            if (line != null)
            {
                ssid = line.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart();
            }
            return ssid;
        }
    }
}
