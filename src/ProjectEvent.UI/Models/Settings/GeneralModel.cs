using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.Settings
{
    public class GeneralModel : UINotifyPropertyChanged
    {
        private bool IsDeviceStartupRun_;
        /// <summary>
        /// 是否启用开机启动
        /// </summary>
        public bool IsDeviceStartupRun
        {
            get { return IsDeviceStartupRun_; }
            set
            {
                IsDeviceStartupRun_ = value;
                OnPropertyChanged();
            }
        }
    }
}
