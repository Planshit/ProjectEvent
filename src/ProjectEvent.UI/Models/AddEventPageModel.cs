using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

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
        //public ObservableCollection<ActionItemModel> Actions { get; set; }
        public ObservableCollection<ItemModel> Events { get; set; }
        public ObservableCollection<ComBoxActionModel> ComBoxActions { get; set; }


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

        private string ProjectName_ = "新的Project";
        public string ProjectName
        {
            get { return ProjectName_; }
            set { ProjectName_ = value; OnPropertyChanged(); }
        }
        private string ProjectDescription_ = "";
        public string ProjectDescription
        {
            get { return ProjectDescription_; }
            set { ProjectDescription_ = value; OnPropertyChanged(); }
        }

        private string Title_ = "创建Project";
        public string Title
        {
            get { return Title_; }
            set { Title_ = value; OnPropertyChanged(); }
        }
        private string ButtonSaveName_ = "创建";
        public string ButtonSaveName
        {
            get { return ButtonSaveName_; }
            set { ButtonSaveName_ = value; OnPropertyChanged(); }
        }
        private Visibility AddACtionDialogVisibility_;
        public Visibility AddACtionDialogVisibility
        {
            get { return AddACtionDialogVisibility_; }
            set { AddACtionDialogVisibility_ = value; OnPropertyChanged(); }
        }

        private ComBoxActionModel ComBoxSelectedAction_;
        public ComBoxActionModel ComBoxSelectedAction
        {
            get { return ComBoxSelectedAction_; }
            set { ComBoxSelectedAction_ = value; OnPropertyChanged(); }
        }

        private bool IsInfoTabItemSelected_ = false;
        public bool IsInfoTabItemSelected
        {
            get { return IsInfoTabItemSelected_; }
            set { IsInfoTabItemSelected_ = value; OnPropertyChanged(); }
        }
        private bool IsEventTabItemSelected_ = false;
        public bool IsEventTabItemSelected
        {
            get { return IsEventTabItemSelected_; }
            set { IsEventTabItemSelected_ = value; OnPropertyChanged(); }
        }
        private bool IsConditionTabItemSelected_ = false;
        public bool IsConditionTabItemSelected
        {
            get { return IsConditionTabItemSelected_; }
            set { IsConditionTabItemSelected_ = value; OnPropertyChanged(); }
        }
        private bool IsActionsTabItemSelected_ = false;
        public bool IsActionsTabItemSelected
        {
            get { return IsActionsTabItemSelected_; }
            set { IsActionsTabItemSelected_ = value; OnPropertyChanged(); }
        }
    }
}
