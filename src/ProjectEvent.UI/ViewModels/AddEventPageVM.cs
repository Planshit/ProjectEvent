using Newtonsoft.Json;
using ProjectEvent.UI.Controls.Action;
using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class AddEventPageVM : AddEventPageModel
    {
        public Command AddActionCommand { get; set; }
        public Command AddCommand { get; set; }
        public Command ActionDialogStateCommand { get; set; }
        public Command ShowActionDialogCommand { get; set; }
        public Command ImportCommand { get; set; }
        private readonly MainViewModel mainVM;
        public Command RedirectCommand { get; set; }
        public Command LoadedCommand { get; set; }
        private ActionContainer actionContainer;

        public AddEventPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            LoadedCommand = new Command(new Action<object>(OnLoadedCommand));
            Events = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            ComBoxActions = new System.Collections.ObjectModel.ObservableCollection<ComBoxActionModel>();
            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
            AddCommand = new Command(new Action<object>(OnAddCommand));
            ActionDialogStateCommand = new Command(new Action<object>(OnActionDialogStateCommand));
            ShowActionDialogCommand = new Command(new Action<object>(OnShowActionDialogCommand));
            ImportCommand = new Command(new Action<object>(OnImportCommandCommand));
            StepIndex = 0;
            AddACtionDialogVisibility = System.Windows.Visibility.Hidden;
            PropertyChanged += AddEventPageVM_PropertyChanged;
            InitEvents();
            InitConditions();
            InitComboxAcions();

        }

        private void OnLoadedCommand(object obj)
        {
            actionContainer = obj as ActionContainer;
            StepIndex = 3;
            actionContainer.RenderDone += (e, c) =>
            {
                HandleEdit();
            };
            //Thread.Sleep(1000);
            //HandleEdit();
        }

        private void OnRedirectCommand(object obj)
        {
            mainVM.Uri = obj.ToString();
        }

        private void OnImportCommandCommand(object obj)
        {
            var a = ConditionData;
            var container = obj as ActionContainer;
            container.ImportActionsJson(File.ReadAllText("d:\\action.json"));
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
            var container = obj as ActionContainer;
            string json = GenerateJson(container);
            File.WriteAllText($"Projects\\{ProjectName}.project.json", json);
            MessageBox.Show("项目已保存！");
            if (mainVM.Data == null)
            {
                mainVM.Uri = "IndexPage";
            }
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
            StepIndex = 2;
            InitConditions();
            //Actions.Clear();
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

        private void OnAddActionCommand(object obj)
        {
            var container = obj as ActionContainer;
            switch (ComBoxSelectedAction.ID)
            {
                case 1:
                    container.AddItem(new ActionItemModel()
                    {
                        ID = container.GetCreateActionID(),
                        ActionName = "创建文件",
                        ActionType = Types.ActionType.WriteFile,
                        Icon = "\xF2E6",
                        //Index = new Random().Next(10)
                    });
                    break;
                case 2:
                    int id = container.GetCreateActionID();
                    container.AddItem(new ActionItemModel()
                    {
                        ID = id,
                        ActionName = "判断",
                        ActionType = Types.ActionType.IF,
                        Icon = "\xE9D4",
                    });
                    container.AddItem(new ActionItemModel()
                    {
                        ID = container.GetCreateActionID(),
                        ActionName = "否则",
                        ActionType = Types.ActionType.IFElse,
                        ParentID = id
                    });
                    container.AddItem(new ActionItemModel()
                    {
                        ID = container.GetCreateActionID(),
                        ActionName = "判断结束",
                        ActionType = Types.ActionType.IFEnd,
                        ParentID = id
                    });
                    break;
            }

            OnActionDialogStateCommand(false);
        }


        private string GenerateJson(ActionContainer container)
        {
            var project = new ProjectModel();
            project.ProjectName = ProjectName;
            project.EventID = SelectedEventID;
            project.ConditionData = ConditionData;
            project.Actions = container.GenerateActions();
            var json = JsonConvert.SerializeObject(project);
            return json;
        }

        private void HandleEdit()
        {
            if (mainVM.Data != null)
            {
                string projectName = mainVM.Data.ToString();
                if (File.Exists($"Projects\\{projectName}.project.json"))
                {
                    Title = $"编辑Project - {projectName}";
                    ButtonSaveName = "保存";

                    //导入
                    var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText($"Projects\\{projectName}.project.json"));
                    if (project != null)
                    {
                        ProjectName = project.ProjectName;
                        SelectedEventID = project.EventID;
                        ConditionData = project.ConditionData;
                        actionContainer.ImportActions(project.Actions);
                        StepIndex = 0;
                    }

                }
            }
        }
    }
}
