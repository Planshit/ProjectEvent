using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.ConditionModels
{
    public class FileChangedConditionModel : UINotifyPropertyChanged
    {
        private string WatchPath_;
        public string WatchPath
        {
            get { return WatchPath_; }
            set { WatchPath_ = value; OnPropertyChanged(); }
        }
        private string Extname_ = "*.*";
        public string Extname
        {
            get { return Extname_; }
            set { Extname_ = value; OnPropertyChanged(); }
        }

    }
}
