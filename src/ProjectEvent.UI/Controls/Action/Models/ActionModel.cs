using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class ActionModel : UINotifyPropertyChanged
    {
        public ObservableCollection<ActionItemModel> Actions { get; set; }
    }
}
