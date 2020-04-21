using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class AddEventPageModel : UINotifyPropertyChanged
    {
        //private List<BaseActionItemModel> Actions_;
        //public List<BaseActionItemModel> Actions
        //{
        //    get { return Actions_; }
        //    set { Actions_ = value; OnPropertyChanged(); }
        //}
        public ObservableCollection<BaseActionItemModel> Actions { get; set; }
    }
}
