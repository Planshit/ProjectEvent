using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class EventLogModel : UINotifyPropertyChanged
    {
        private string Log_;
        public string Log
        {
            get { return Log_; }
            set
            {
                Log_ = value;
                OnPropertyChanged();
            }
        }
    }
}
