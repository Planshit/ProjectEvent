using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.ConditionModels
{
    public class WIFIConnectedEventConditionModel : UINotifyPropertyChanged
    {
        private string SSID_;
        public string SSID
        {
            get { return SSID_; }
            set { SSID_ = value; OnPropertyChanged(); }
        }


    }
}
