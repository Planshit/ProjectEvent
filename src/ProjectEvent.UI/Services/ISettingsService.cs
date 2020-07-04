using ProjectEvent.UI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public interface ISettingsService
    {
        void Load();
        /// <summary>
        /// 更新程序设置
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="model">模块数据</param>
        void Update<T>(T model);
        /// <summary>
        /// 获取程序设置
        /// </summary>
        /// <returns></returns>
        SettingsModel GetSettings();
    }
}
