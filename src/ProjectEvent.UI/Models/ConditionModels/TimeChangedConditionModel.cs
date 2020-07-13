using ProjectEvent.Core.Condition.Types;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.ConditionModels
{
    public class TimeChangedConditionModel : UINotifyPropertyChanged
    {
        private DateTime Time_ = DateTime.Now;
        public DateTime Time
        {
            get { return Time_; }
            set { Time_ = value; OnPropertyChanged(); }
        }

        private ComBoxModel RepetitionType_ = new ComBoxModel()
        {
            ID = (int)TimeChangedRepetitionType.None,
            DisplayName = "不重复"
        };
        public ComBoxModel RepetitionType
        {
            get { return RepetitionType_; }
            set { RepetitionType_ = value; OnPropertyChanged(); }
        }

    }
}
