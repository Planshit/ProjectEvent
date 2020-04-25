using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class AddEventPageVM : AddEventPageModel
    {

        public Command AddActionCommand { get; set; }
        public Command AddCommand { get; set; }
        public Command ActionDialogStateCommand { get; set; }
        public Command ShowActionDialogCommand { get; set; }

        public AddEventPageVM()
        {
            Actions = new System.Collections.ObjectModel.ObservableCollection<ActionItemModel>();
            Events = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            ComBoxActions = new System.Collections.ObjectModel.ObservableCollection<ComBoxActionModel>();

            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
            AddCommand = new Command(new Action<object>(OnAddCommand));
            ActionDialogStateCommand = new Command(new Action<object>(OnActionDialogStateCommand));
            ShowActionDialogCommand = new Command(new Action<object>(OnShowActionDialogCommand));

            StepIndex = 0;
            AddACtionDialogVisibility = System.Windows.Visibility.Hidden;
            PropertyChanged += AddEventPageVM_PropertyChanged;

            InitEvents();
            InitConditions();
            InitComboxAcions();
        }

        private void OnShowActionDialogCommand(object obj)
        {
            OnActionDialogStateCommand(true);
        }

        private void OnActionDialogStateCommand(object obj)
        {
            if (bool.Parse(obj.ToString()))
            {
                AddACtionDialogVisibility = System.Windows.Visibility.Visible;
            }
            else
            {
                AddACtionDialogVisibility = System.Windows.Visibility.Hidden;
            }
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

        private void InitComboxAcions()
        {
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = 1,
                Name = "创建文件"
            });
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = 2,
                Name = "判断"
            });
            ComBoxSelectedAction = ComBoxActions[0];
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
        private int CreateActionID()
        {
            //int count = Actions.Where(m => m.ParentID == 0).Count();

            return Actions.Count + 1;
        }
        private void OnAddActionCommand(object obj)
        {
            switch (ComBoxSelectedAction.ID)
            {
                case 1:
                    Actions.Add(new ActionItemModel()
                    {
                        ID = CreateActionID(),
                        ActionName = "写文件",
                        ActionType = Types.ActionType.WriteFile,
                        Icon = "\xF2E6",
                        //Index = new Random().Next(10)
                    });
                    break;
                case 2:
                    int id = CreateActionID();
                    Actions.Add(new ActionItemModel()
                    {
                        ID = id,
                        ActionName = "判断",
                        ActionType = Types.ActionType.IF,
                        Icon = "\xE9D4",
                    });
                    Actions.Add(new ActionItemModel()
                    {
                        ID = CreateActionID(),
                        ActionName = "否则",
                        ActionType = Types.ActionType.IFElse,
                        ParentID = id
                    });
                    Actions.Add(new ActionItemModel()
                    {
                        ID = CreateActionID(),
                        ActionName = "判断结束",
                        ActionType = Types.ActionType.IFEnd,
                        ParentID = id
                    });
                    break;
            }

            OnActionDialogStateCommand(false);
        }
    }
}
