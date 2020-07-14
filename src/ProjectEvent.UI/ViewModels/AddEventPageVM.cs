using Newtonsoft.Json;
using ProjectEvent.Core.Action;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.UI.Controls.Action;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Controls.Toggle;
using ProjectEvent.UI.Event;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly IApp app;
        public Command AddActionCommand { get; set; }
        public Command AddCommand { get; set; }
        public Command ActionDialogStateCommand { get; set; }
        public Command ShowActionDialogCommand { get; set; }
        public Command PageLoadedCommand { get; set; }
        public Command RedirectCommand { get; set; }
        public Command LoadedCommand { get; set; }
        public Command ActionInvokeCommand { get; set; }

        private ActionContainer actionContainer;
        private ProjectModel project;
        private Page page;
        public AddEventPageVM(
            MainViewModel mainVM,
            IProjects projects,
            IApp app)
        {
            this.mainVM = mainVM;
            this.projects = projects;
            this.app = app;

            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            LoadedCommand = new Command(new Action<object>(OnLoadedCommand));
            Events = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            ComBoxActions = new System.Collections.ObjectModel.ObservableCollection<ComBoxActionModel>();
            AddActionCommand = new Command(new Action<object>(OnAddActionCommand));
            AddCommand = new Command(new Action<object>(OnAddCommand));
            ActionDialogStateCommand = new Command(new Action<object>(OnActionDialogStateCommand));
            ShowActionDialogCommand = new Command(new Action<object>(OnShowActionDialogCommand));
            PageLoadedCommand = new Command(new Action<object>(OnPageLoadedCommand));
            ActionInvokeCommand = new Command(new Action<object>(OnActionInvokeCommand));
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
        #region 运行或停止actions
        private void OnActionInvokeCommand(object obj)
        {
            if (obj != null)
            {
                if (obj.ToString() == "run")
                {
                    RunActions();
                }
                else
                {
                    StopActions();
                }
            }
        }
        private void RunActions()
        {
            var actions = actionContainer.GenerateActions();
            if (actions == null || actions.Count == 0)
            {
                mainVM.Toast("请至少添加一个操作", Types.ToastType.Failed);
            }
            else
            {
                mainVM.Toast("正在执行操作", Types.ToastType.Warning);
                VisualStateManager.GoToElementState(page, "ActionsRuning", true);
                ActionTask.OnActionState += ActionTask_OnActionInvoke;
                ActionTask.OnActionsState += ActionTask_OnActionsState;
                ActionTask.RunTestInvokeAction(actions);
            }
        }

        private void ActionTask_OnActionsState(int taskID, Core.Action.Types.ActionInvokeStateType state)
        {
            if (taskID == ActionTask.TestTaskID)
            {
                ContainerState = state;
                if (state == Core.Action.Types.ActionInvokeStateType.Done)
                {
                    ActionTask.OnActionsState -= ActionTask_OnActionsState;
                    ActionTask.OnActionState -= ActionTask_OnActionInvoke;
                    RunActionsButtonVisibility = Visibility.Visible;
                    StopActionsButtonVisibility = Visibility.Collapsed;
                    ActionsStateVisibility = Visibility.Collapsed;
                    page.Dispatcher.Invoke(() =>
                    {
                        UpdateVisualState();
                    });
                    mainVM.Toast("操作已执行完成", Types.ToastType.Success);
                }
                else if (state == Core.Action.Types.ActionInvokeStateType.Busy)
                {
                    RunActionsButtonVisibility = Visibility.Collapsed;
                    StopActionsButtonVisibility = Visibility.Collapsed;
                    ActionsStateVisibility = Visibility.Visible;
                }
                else
                {
                    RunActionsButtonVisibility = Visibility.Collapsed;
                    StopActionsButtonVisibility = Visibility.Visible;
                    ActionsStateVisibility = Visibility.Collapsed;
                }
            }
        }

        private void StopActions()
        {
            ActionTask.StopTestInvokeAction();
        }

        private void ActionTask_OnActionInvoke(int taskID, int actionID, Core.Action.Types.ActionInvokeStateType state)
        {
            RuningActionID = actionID;
            RuningActionState = state;
        }
        #endregion

        private void OnPageLoadedCommand(object obj)
        {
            page = obj as Page;
            UpdateVisualState();
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
                    mainVM.Toast("方案名称已存在，请更换", Types.ToastType.Failed);
                }
                else
                {
                    AddProject(GenerateModel(container));
                }

            }
            else
            {
                UpdateProject(GenerateModel(container));
            }

        }

        private void AddEventPageVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedEventID):
                    HandleEventIDChanged();
                    break;
                case nameof(IsActionsTabItemSelected):
                case nameof(IsConditionTabItemSelected):
                case nameof(IsEventTabItemSelected):
                case nameof(IsInfoTabItemSelected):
                    //case nameof(ContainerState):
                    UpdateVisualState();
                    break;
            }
        }

        #region 切换tab时更新visual state
        private void UpdateVisualState()
        {
            if (page == null)
            {
                return;
            }
            RunActionsButtonVisibility = Visibility.Collapsed;

            if (ContainerState == Core.Action.Types.ActionInvokeStateType.Runing || ContainerState == Core.Action.Types.ActionInvokeStateType.Busy)
            {
                VisualStateManager.GoToElementState(page, "ActionsRuning", true);
            }
            else if (IsActionsTabItemSelected)
            {
                VisualStateManager.GoToElementState(page, "ActionsTabSelected", true);
                RunActionsButtonVisibility = Visibility.Visible;
            }
            else if (IsInfoTabItemSelected)
            {
                VisualStateManager.GoToElementState(page, "InfoTabSelected", true);
            }
            else if (IsEventTabItemSelected)
            {
                VisualStateManager.GoToElementState(page, "EventTabSelected", true);
            }
            else if (IsConditionTabItemSelected)
            {
                VisualStateManager.GoToElementState(page, "ConditionTabSelected", true);
            }
            else
            {
                VisualStateManager.GoToElementState(page, "NoTabSelected", true);
            }
        }
        #endregion
        private void HandleEventIDChanged()
        {
            IsConditionTabItemSelected = true;
            InitConditions();
        }

        #region 初始化事件
        private void InitEvents()
        {
            foreach (var e in EventData.Events)
            {
                Events.Add(e);
            }
        }
        #endregion

        #region 初始化Actions
        private void InitAcions()
        {
            foreach (var item in Enum.GetValues(typeof(Types.ActionType)))
            {
                var type = (Types.ActionType)item;
                if (type != Types.ActionType.IFElse && type != Types.ActionType.IFEnd)
                {
                    ComBoxActions.Add(new ComBoxActionModel()
                    {
                        ID = (int)type,
                        Name = ActionData.Names[type]
                    });
                }
            }

            ComBoxSelectedAction = ComBoxActions[0];
        }
        #endregion

        #region 初始化事件条件
        /// <summary>
        /// 初始化事件条件
        /// </summary>
        private void InitConditions()
        {
            EventType type = (EventType)SelectedEventID;
            ConditionData = Event.EventManager.GetCreateConditionData(type);
            Conditions = EventData.GetInputModels(type);
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
                    var ifmodel = ActionData.GetCreateActionItemModel((Types.ActionType)ComBoxSelectedAction.ID);
                    ifmodel.ID = id;
                    container.AddItem(ifmodel);

                    var elsemodel = ActionData.GetCreateActionItemModel(Types.ActionType.IFElse);
                    elsemodel.ID = container.GetCreateActionID();
                    elsemodel.ParentID = id;
                    container.AddItem(elsemodel);

                    var endmodel = ActionData.GetCreateActionItemModel(Types.ActionType.IFEnd);
                    endmodel.ID = container.GetCreateActionID();
                    endmodel.ParentID = id;
                    container.AddItem(endmodel);
                    break;
                default:
                    //非特殊action
                    var model = ActionData.GetCreateActionItemModel((Types.ActionType)ComBoxSelectedAction.ID);
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

                    if (project != null)
                    {
                        ProjectName = project.ProjectName;
                        SelectedEventID = project.EventID;
                        ConditionData = Event.EventManager.GetObj(project);
                        ProjectDescription = project.ProjectDescription;
                        ProjectIcon = project.Icon;
                    }

                }
            }
            else
            {
                IsInfoTabItemSelected = true;
                IsActionsTabItemSelected = false;
            }
        }

        private void AddProject(ProjectModel project)
        {
            bool res = projects.Add(project);
            if (res)
            {
                mainVM.Toast("添加成功", Types.ToastType.Success);
                mainVM.Uri = "IndexPage";
                app.Add(project);
            }
            else
            {
                mainVM.Toast("添加失败", Types.ToastType.Failed);
            }
        }

        private void UpdateProject(ProjectModel project)
        {
            mainVM.Toast("已更新", Types.ToastType.Success);
            projects.Update(project);
            app.Update(project);
        }
    }
}
