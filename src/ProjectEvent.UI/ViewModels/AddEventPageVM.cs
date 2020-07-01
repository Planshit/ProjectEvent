using Newtonsoft.Json;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls.Action;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Services;
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
        private readonly MainViewModel mainVM;
        private readonly IProjects projects;

        public Command AddActionCommand { get; set; }
        public Command AddCommand { get; set; }
        public Command ActionDialogStateCommand { get; set; }
        public Command ShowActionDialogCommand { get; set; }
        public Command PageLoadedCommand { get; set; }
        public Command RedirectCommand { get; set; }
        public Command LoadedCommand { get; set; }
        private ActionContainer actionContainer;
        private ProjectModel project;
        private Page page;
        public AddEventPageVM(
            MainViewModel mainVM,
            IProjects projects)
        {
            this.mainVM = mainVM;
            this.projects = projects;

            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            LoadedCommand = new Command(new Action<object>(OnLoadedCommand));
            Events = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            ComBoxActions = new System.Collections.ObjectModel.ObservableCollection<ComBoxActionModel>();
            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
            AddCommand = new Command(new Action<object>(OnAddCommand));
            ActionDialogStateCommand = new Command(new Action<object>(OnActionDialogStateCommand));
            ShowActionDialogCommand = new Command(new Action<object>(OnShowActionDialogCommand));
            PageLoadedCommand = new Command(new Action<object>(OnPageLoadedCommand));

            AddACtionDialogVisibility = System.Windows.Visibility.Hidden;
            PropertyChanged += AddEventPageVM_PropertyChanged;

            IsActionsTabItemSelected = true;
            mainVM.IsShowNavigation = false;
            mainVM.IsShowTitleBar = true;
            mainVM.Title = "创建自动化方案";
            InitEvents();
            InitConditions();
            InitAcions();
            HandleEdit();
        }

        private void OnPageLoadedCommand(object obj)
        {
            page = obj as Page;
        }

        private void OnLoadedCommand(object obj)
        {
            actionContainer = obj as ActionContainer;
            if (project != null && mainVM.Data != null)
            {
                actionContainer.ImportActions(project.Actions);
            }
        }

        private void OnRedirectCommand(object obj)
        {
            mainVM.Uri = obj.ToString();
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
            if (mainVM.Data == null)
            {
                if (projects.GetProjects().Where(m => m.ProjectName == ProjectName).Any())
                {
                    mainVM.Toast("添加失败，名称已存在", Types.ToastType.Failed);
                }
                else if (projects.Add(GenerateModel(container)))
                {
                    mainVM.Toast("添加成功", Types.ToastType.Success);
                    mainVM.Uri = "IndexPage";
                }
                else
                {
                    mainVM.Toast("添加失败", Types.ToastType.Failed);
                }
            }
            else
            {
                mainVM.Toast("已更新", Types.ToastType.Success);
                projects.Update(GenerateModel(container));
            }

        }

        private void AddEventPageVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedEventID):
                    HandleEventIDChanged();
                    break;
                case nameof(IsEventTabItemSelected):
                    if (IsEventTabItemSelected)
                    {
                        VisualStateManager.GoToElementState(page, "EventTabSelected", true);
                    }
                    else
                    {
                        VisualStateManager.GoToElementState(page, "EventTabUnSelected", true);

                    }
                    break;
            }
        }

        private void HandleEventIDChanged()
        {
            IsConditionTabItemSelected = true;
            InitConditions();
        }

        #region 初始化事件
        private void InitEvents()
        {
            Events.Add(new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnIntervalTimer,
                Title = "计时器",
                Icon = Controls.Base.IconTypes.Timer,
                Description = "每隔多少秒触发"
            });
            Events.Add(new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnDeviceStartup,
                Title = "设备启动",
                Description = "电脑首次开机或注销后重新登录时触发",
                Icon = Controls.Base.IconTypes.DeviceRun
            });
            Events.Add(new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnProcessCreated,
                Title = "进程创建",
                Description = "当有新的程序首次运行时触发",
                Icon = Controls.Base.IconTypes.ProcessingRun

            });
        }
        #endregion

        #region 初始化Actions
        private void InitAcions()
        {
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = (int)Types.ActionType.WriteFile,
                Name = ActionNameData.Names[Types.ActionType.WriteFile]
            });
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = (int)Types.ActionType.IF,
                Name = ActionNameData.Names[Types.ActionType.IF]
            });
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = (int)Types.ActionType.HttpGet,
                Name = ActionNameData.Names[Types.ActionType.HttpGet]
            });
            ComBoxActions.Add(new ComBoxActionModel()
            {
                ID = (int)Types.ActionType.Shutdown,
                Name = ActionNameData.Names[Types.ActionType.Shutdown]
            });
            ComBoxSelectedAction = ComBoxActions[0];
        }
        #endregion

        #region 初始化事件条件
        /// <summary>
        /// 初始化事件条件
        /// </summary>
        private void InitConditions()
        {
            var cds = new List<InputModel>();
            switch ((EventType)SelectedEventID)
            {

                case EventType.OnIntervalTimer:
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
                case EventType.OnProcessCreated:
                    //进程创建
                    ConditionData = new ProcessCreatedConditionModel();
                    cds.Add(new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = "ProcessName",
                        BindingProperty = TextBox.TextProperty,
                        Title = "进程名"
                    });
                    cds.Add(new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Bool,
                        BindingName = "Caseinsensitive",
                        BindingProperty = CheckBox.IsCheckedProperty,
                        Title = "不区分大小写"
                    });
                    cds.Add(new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Bool,
                        BindingName = "FuzzyMatch",
                        BindingProperty = CheckBox.IsCheckedProperty,
                        Title = "模糊匹配"
                    });
                    break;
            }

            Conditions = cds;
        }
        #endregion
        private void OnAddActionCommand(object obj)
        {
            var container = obj as ActionContainer;
            switch ((Types.ActionType)ComBoxSelectedAction.ID)
            {
                //特殊action 单独处理
                case Types.ActionType.IF:
                    int id = container.GetCreateActionID();
                    var ifmodel = ActionItemsData.Get((Types.ActionType)ComBoxSelectedAction.ID);
                    ifmodel.ID = id;
                    container.AddItem(ifmodel);

                    var elsemodel = ActionItemsData.Get(Types.ActionType.IFElse);
                    elsemodel.ID = container.GetCreateActionID();
                    elsemodel.ParentID = id;
                    container.AddItem(elsemodel);

                    var endmodel = ActionItemsData.Get(Types.ActionType.IFEnd);
                    endmodel.ID = container.GetCreateActionID();
                    endmodel.ParentID = id;
                    container.AddItem(endmodel);
                    break;
                default:
                    //非特殊action
                    var model = ActionItemsData.Get((Types.ActionType)ComBoxSelectedAction.ID);
                    model.ID = container.GetCreateActionID();
                    container.AddItem(model);
                    break;
            }

            OnActionDialogStateCommand(false);
        }

        private ProjectModel GenerateModel(ActionContainer container)
        {
            ProjectModel project = null;
            if (mainVM.Data != null)
            {
                project = projects.GetProject((int)mainVM.Data);
            }
            if (project == null)
            {
                project = new ProjectModel();
            }
            project.ProjectName = ProjectName;
            project.EventID = SelectedEventID;
            project.ConditionData = ConditionData;
            project.ProjectDescription = ProjectDescription;
            project.Actions = container.GenerateActions();
            project.Icon = ProjectIcon;
            if (mainVM.SelectedGroup != null)
            {
                project.GroupID = mainVM.SelectedGroup.ID;
            }
            return project;
        }

        private void HandleEdit()
        {
            if (mainVM.Data != null)
            {
                project = projects.GetProject((int)mainVM.Data);
                if (project != null)
                {
                    mainVM.Title = $"编辑方案 - {project.ProjectName}";
                    ButtonSaveName = "保 存";
                    object cdata = null;
                    switch ((EventType)project.EventID)
                    {
                        case EventType.OnProcessCreated:
                            cdata = ObjectConvert.Get<ProcessCreatedConditionModel>(project.ConditionData);
                            break;
                        case EventType.OnIntervalTimer:
                            cdata = ObjectConvert.Get<IntervalTimerConditionModel>(project.ConditionData);
                            break;
                        default:
                            cdata = project.ConditionData;
                            break;
                    }
                    if (project != null)
                    {
                        ProjectName = project.ProjectName;
                        SelectedEventID = project.EventID;
                        ConditionData = cdata;
                        ProjectDescription = project.ProjectDescription;
                    }

                }
            }
            else
            {
                IsInfoTabItemSelected = true;
                IsActionsTabItemSelected = false;
            }
        }
    }
}
