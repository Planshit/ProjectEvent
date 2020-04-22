using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class AddEventPageVM : AddEventPageModel
    {

        public Command AddActionCommand { get; set; }
        public Command AddCommand { get; set; }

        public AddEventPageVM()
        {
            Actions = new System.Collections.ObjectModel.ObservableCollection<BaseActionItemModel>();
            Events = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();

            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
            AddCommand = new Command(new Action<object>(OnAddCommand));

            StepIndex = 0;

            PropertyChanged += AddEventPageVM_PropertyChanged;

            InitEvents();
            InitConditions();
        }

        private void OnAddCommand(object obj)
        {
            var a = ConditionData;

        }

        private void AddEventPageVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedEventID":
                    HandleEventIDChanged();
                    break;
            }
        }

        private void HandleEventIDChanged()
        {
            StepIndex = 1;
            InitConditions();
            Actions.Clear();
        }
        private void InitEvents()
        {
            Events.Add(new Controls.ItemSelect.Models.ItemModel()
            {
                ID = 1,
                Title = "日期更改",
            });
            Events.Add(new Controls.ItemSelect.Models.ItemModel()
            {
                ID = 2,
                Title = "循环计时",
            });
        }

        private void InitConditions()
        {
            var cds = new List<InputModel>();
            switch (SelectedEventID)
            {
                case 1:
                    break;
                case 2:
                    //循环计时
                    ConditionData = new IntervalTimerConditionModel();
                    cds.Add(new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = "Second",
                        BindingProperty = TextBox.TextProperty,
                        Title = "间隔秒数"
                    });
                    cds.Add(new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = "Num",
                        BindingProperty = TextBox.TextProperty,
                        Title = "循环次数（0时永远）"
                    });
                    break;
            }

            Conditions = cds;
        }
        private void OnAddActionCommand(object obj)
        {
            Actions.Add(new WriteFileActionModel()
            {
                ActionName = "写文件",
                ActionType = Types.ActionType.WriteFile,
                Icon = "\xF2E6",
                Index = new Random().Next(10)
            });
        }
    }
}
