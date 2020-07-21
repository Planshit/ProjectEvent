using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ProjectEvent.UI.Services
{
    public class SettingsService : ISettingsService
    {
        private SettingsModel Settings { get; set; }
        private const string SettingsDir = "Data\\";
        private const string SettingsPath = SettingsDir + "Settings.json";
        private readonly object saveLock = new object();
        private readonly object hdsLock = new object();

        public void Load()
        {
            IOHelper.CreateDirectory(SettingsDir);
            if (IOHelper.FileExists(SettingsPath))
            {
                Settings = JsonConvert.DeserializeObject<SettingsModel>(IOHelper.ReadFile(SettingsPath));
            }
            if (Settings == null)
            {
                Settings = GetCreateDefaultSettings();
                Save();
            }
            HandleSettingsChanged();
            GetVersions();
        }

        /// <summary>
        /// 获取并设置程序版本号到配置数据中
        /// </summary>
        private void GetVersions()
        {
            Settings.UIVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Settings.CoreVersion = typeof(IOHelper).Assembly.GetName().Version.ToString();
        }
        private SettingsModel GetCreateDefaultSettings()
        {
            var settings = new SettingsModel();
            settings.General = new GeneralModel();
            settings.General.IsDeviceStartupRun = false;
            return settings;
        }
        public void Update<T>(T model)
        {
            if (typeof(T) == typeof(GeneralModel))
            {
                Settings.General = model as GeneralModel;
            }
            HandleSettingsChanged();
            Save();
        }
        /// <summary>
        /// 持久化数据
        /// </summary>
        private void Save()
        {
            Task.Run(() =>
            {
                lock (saveLock)
                {
                    if (Settings == null)
                    {
                        Load();
                    }
                    IOHelper.CreateDirectory(SettingsDir);
                    IOHelper.WriteFile(SettingsPath, JsonConvert.SerializeObject(Settings));
                }

            });

        }

        public SettingsModel GetSettings()
        {
            if (Settings == null)
            {
                Settings = GetCreateDefaultSettings();
                Save();
            }
            return Settings;
        }

        /// <summary>
        /// 根据设置项作出响应
        /// </summary>
        private void HandleSettingsChanged()
        {
            Task.Run(async () =>
            {
                //开机启动
                StartupTask startupTask = await StartupTask.GetAsync("ProjectEvent");
                var s = startupTask.State;
                if (s == StartupTaskState.Disabled && Settings.General.IsDeviceStartupRun)
                {
                    StartupTaskState newState = await startupTask.RequestEnableAsync();
                    Settings.General.IsDeviceStartupRun = newState == StartupTaskState.EnabledByPolicy || newState == StartupTaskState.Enabled;
                }

                if (s == StartupTaskState.Enabled && !Settings.General.IsDeviceStartupRun)
                {
                    startupTask.Disable();
                }
                //lock (hdsLock)
                //{


                //    //SystemHelper.SetStartup(Settings.General.IsDeviceStartupRun);
                //}
            });

        }
    }
}
