using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Controls.Navigation.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class IndexPageModel : UINotifyPropertyChanged
    {
        public ObservableCollection<ItemModel> Projects { get; set; }

        private int SelectedProjectID_ = 0;
        public int SelectedProjectID
        {
            get { return SelectedProjectID_; }
            set { SelectedProjectID_ = value; OnPropertyChanged(); }
        }
        private string Title_;
        public string Title
        {
            get { return Title_; }
            set { Title_ = value; OnPropertyChanged(); }
        }

        private ItemModel SelectItem_;
        public ItemModel SelectItem
        {
            get { return SelectItem_; }
            set { SelectItem_ = value; OnPropertyChanged(); }
        }
    }
}
