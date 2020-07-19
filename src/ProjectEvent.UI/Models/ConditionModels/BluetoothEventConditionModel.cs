using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.ConditionModels
{
    public class BluetoothEventConditionModel : UINotifyPropertyChanged
    {
        private string DeviceName_;
        public string DeviceName
        {
            get { return DeviceName_; }
            set { DeviceName_ = value; OnPropertyChanged(); }
        }

        private bool Caseinsensitive_;
        public bool Caseinsensitive
        {
            get { return Caseinsensitive_; }
            set { Caseinsensitive_ = value; OnPropertyChanged(); }
        }
        private bool FuzzyMatch_;
        public bool FuzzyMatch
        {
            get { return FuzzyMatch_; }
            set { FuzzyMatch_ = value; OnPropertyChanged(); }
        }
    }
}
