using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.Settings
{
    public class SettingsModel
    {
        /// <summary>
        /// 常规设置
        /// </summary>
        public GeneralModel General { get; set; }
        /// <summary>
        /// UI程序版本号
        /// </summary>
        public string UIVersion { get; set; }
        /// <summary>
        /// 核心程序版本号
        /// </summary>
        public string CoreVersion { get; set; }
    }
}
