using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.ConditionModels
{
    public class ProcessCreatedConditionModel : UINotifyPropertyChanged
    {
        private string ProcessName_;
        public string ProcessName
        {
            get { return ProcessName_; }
            set { ProcessName_ = value; OnPropertyChanged(); }
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
