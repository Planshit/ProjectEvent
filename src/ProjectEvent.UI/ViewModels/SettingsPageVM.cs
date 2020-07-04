using ProjectEvent.UI.Controls.Toggle;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.Settings;
using ProjectEvent.UI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class SettingsPageVM : SettingsPageModel
    {
        private readonly MainViewModel mainVM;
        private readonly ISettingsService settingsService;

        public Command OpenUrlCommand { get; set; }
        public SettingsPageVM(
            MainViewModel mainVM,
            ISettingsService settingsService)
        {
            this.mainVM = mainVM;
            this.settingsService = settingsService;

            OpenUrlCommand = new Command(new Action<object>(OnOpenUrl));

            mainVM.IsShowNavigation = false;
            mainVM.IsShowTitleBar = true;
            mainVM.Title = "设置";
            InitGeneral();
        }

        private void OnOpenUrl(object obj)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = obj.ToString(),
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        #region 初始化常规设置
        private void InitGeneral()
        {
            //创建常规设置模板
            var inputModels = new List<Controls.InputGroup.Models.InputModel>();
            inputModels.Add(new Controls.InputGroup.Models.InputModel()
            {
                Title = "开机启动",
                BindingName = nameof(GeneralModel.IsDeviceStartupRun),
                BindingProperty = Toggle.IsCheckedProperty,
                Type = Controls.InputGroup.InputType.Bool
            });

            GeneralInputModels = inputModels;

            //载入常规数据
            Settings = settingsService.GetSettings();
            Settings.General.PropertyChanged += GeneralData_PropertyChanged;
        }

        private void GeneralData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            settingsService.Update(Settings.General);
        }
        #endregion
    }
}
