using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class SettingsPageModel : UINotifyPropertyChanged
    {
        private List<InputModel> GeneralInputModels_;
        public List<InputModel> GeneralInputModels
        {
            get { return GeneralInputModels_; }
            set { GeneralInputModels_ = value; OnPropertyChanged(); }
        }

        private SettingsModel Settings_;
        public SettingsModel Settings
        {
            get { return Settings_; }
            set
            {
                Settings_ = value;
                OnPropertyChanged();
            }
        }
        //private GeneralModel GeneralData_;
        //public GeneralModel GeneralData
        //{
        //    get
        //    {
        //        return GeneralData_;
        //    }
        //    set
        //    {
        //        GeneralData_ = value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}
