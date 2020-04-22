using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class AddEventPageModel : UINotifyPropertyChanged
    {
        private List<InputModel> Conditions_;
        public List<InputModel> Conditions
        {
            get { return Conditions_; }
            set { Conditions_ = value; OnPropertyChanged(); }
        }
        public ObservableCollection<BaseActionItemModel> Actions { get; set; }
        public ObservableCollection<ItemModel> Events { get; set; }

        private int StepIndex_;
        public int StepIndex
        {
            get { return StepIndex_; }
            set { StepIndex_ = value; OnPropertyChanged(); }
        }

        private int SelectedEventID_;
        public int SelectedEventID
        {
            get { return SelectedEventID_; }
            set { SelectedEventID_ = value; OnPropertyChanged(); }
        }

        private object ConditionData_;
        public object ConditionData
        {
            get { return ConditionData_; }
            set { ConditionData_ = value; OnPropertyChanged(); }
        }
    }
}
