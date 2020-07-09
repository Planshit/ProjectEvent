using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionItem : Control
    {
        public IconTypes Icon
        {
            get { return (IconTypes)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(IconTypes),
                typeof(ActionItem));
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y",
                typeof(double),
                typeof(ActionItem));
        public int ID
        {
            get { return (int)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }
        public static readonly DependencyProperty IDProperty =
            DependencyProperty.Register("ID",
                typeof(int),
                typeof(ActionItem));

        public string ActionName
        {
            get { return (string)GetValue(ActionNameProperty); }
            set { SetValue(ActionNameProperty, value); }
        }
        public static readonly DependencyProperty ActionNameProperty =
            DependencyProperty.Register("ActionName",
                typeof(string),
                typeof(ActionItem));

        public ActionItemModel Action
        {
            get { return (ActionItemModel)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action",
                typeof(ActionItemModel),
                typeof(ActionItem),
                new PropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged))
                );
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ActionItem);
            if (e.Property == ActionProperty)
            {

                var newValue = e.NewValue as ActionItemModel;

                if (newValue != null)
                {
                    control.Render();
                }
            }
        }

        public int RuningID
        {
            get { return (int)GetValue(RuningIDProperty); }
            set { SetValue(RuningIDProperty, value); }
        }
        public static readonly DependencyProperty RuningIDProperty =
            DependencyProperty.Register("RuningID",
                typeof(int),
                typeof(ActionItem), new PropertyMetadata(new PropertyChangedCallback(OnRuningStateChanged)));

        private static void OnRuningStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionItem;

            if (control.RuningID == control.ID)
            {
                control.State = control.RuningState;
            }
            else
            {
                control.State = Core.Action.Types.ActionInvokeStateType.Done;
            }
        }

        /// <summary>
        /// 运行中的action state
        /// </summary>
        public ProjectEvent.Core.Action.Types.ActionInvokeStateType RuningState
        {
            get { return (ProjectEvent.Core.Action.Types.ActionInvokeStateType)GetValue(RuningStateProperty); }
            set { SetValue(RuningStateProperty, value); }
        }
        public static readonly DependencyProperty RuningStateProperty =
            DependencyProperty.Register("RuningState",
                typeof(ProjectEvent.Core.Action.Types.ActionInvokeStateType),
                typeof(ActionItem), new PropertyMetadata(new PropertyChangedCallback(OnRuningStateChanged)));

        /// <summary>
        /// 当前状态
        /// </summary>
        public ProjectEvent.Core.Action.Types.ActionInvokeStateType State
        {
            get { return (ProjectEvent.Core.Action.Types.ActionInvokeStateType)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State",
                typeof(ProjectEvent.Core.Action.Types.ActionInvokeStateType),
                typeof(ActionItem), new PropertyMetadata(ProjectEvent.Core.Action.Types.ActionInvokeStateType.Done, new PropertyChangedCallback(OnStateChanged)));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionItem;
            if (e.NewValue != e.OldValue)
            {
                VisualStateManager.GoToState(control, control.State == Core.Action.Types.ActionInvokeStateType.Done ? "Done" : "Runing", true);
            }
        }

        public ProjectEvent.Core.Action.Types.ActionInvokeStateType ContainerState
        {
            get { return (ProjectEvent.Core.Action.Types.ActionInvokeStateType)GetValue(ContainerStateProperty); }
            set { SetValue(ContainerStateProperty, value); }
        }
        public static readonly DependencyProperty ContainerStateProperty =
            DependencyProperty.Register("ContainerState",
                typeof(ProjectEvent.Core.Action.Types.ActionInvokeStateType),
                typeof(ActionItem), new PropertyMetadata(ProjectEvent.Core.Action.Types.ActionInvokeStateType.Done));

        public event EventHandler OnRenderDone;

        public event EventHandler OnClick;
        public object InputDataModel { get; set; }

        private Grid Header;
        private Border Input;
        private Button ButtonRemove;
        private ActionForm ActionForm;
        public ActionContainer ActionContainer { get; set; }
        public object VMDataContext { get; set; }
        public ActionItem()
        {
            DefaultStyleKey = typeof(ActionItem);
            var translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            translateTransform.Changed += translateTransform_Changed;
            RenderTransform = translateTransform;
            //SetDefautlInputData();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Input = GetTemplateChild("Input") as Border;
            Header = GetTemplateChild("Header") as Grid;
            ButtonRemove = GetTemplateChild("ButtonRemove") as Button;
            ActionForm = GetTemplateChild("ActionForm") as ActionForm;
            Render();
        }
        private void translateTransform_Changed(object sender, EventArgs e)
        {
            var ttf = RenderTransform as TranslateTransform;
            Y = ttf.Y;
        }

        private void Render()
        {
            if (Input == null)
            {
                return;
            }
            if (Action.ActionType == ActionType.IFElse || Action.ActionType == ActionType.IFEnd)
            {
                ActionName = $"{Action.ActionName}";
                Loaded += (e, c) =>
                {
                    OnRenderDone?.Invoke(this, null);
                };
            }
            else
            {
                //Action item = new Action(Action.ID, GetInputs());
                //item.VMDataContext = VMDataContext;
                //item.ActionContainer = ActionContainer;
                ActionName = $"[{Action.ID}] {Action.ActionName}";
                //item.Data = InputDataModel;
                Icon = Action.Icon;
                ActionForm.DataContext = InputDataModel;
                ActionForm.LineInputGroups = GetCreateActionLineInputGroups();
                ActionForm.MultiLineInputGroups = GetCreateActionMultiLineInputGroups();
                ActionForm.Action = Action;
                ActionForm.ActionContainer = ActionContainer;
                //Input.Child = item;
                //item.OnClick += Item_OnClick;
                //item.Loaded += (e, c) =>
                //{
                //    OnRenderDone?.Invoke(this, null);
                //};
                ActionForm.OnRenderDone += (e, c) =>
                {
                    OnRenderDone?.Invoke(this, null);
                };
            }
            if (Action.ActionType == ActionType.IFElse || Action.ActionType == ActionType.IFEnd)
            {
                ButtonRemove.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 获取输入数据
        /// </summary>
        /// <returns></returns>
        public object GetInputData()
        {
            return InputDataModel;
        }
        private void Item_OnClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(sender, e);
        }

        #region 获取创建单行输入组模板
        private List<ActionInputModel> GetCreateActionLineInputGroups()
        {
            var groups = new List<ActionInputModel>();
            switch (Action.ActionType)
            {
                case ActionType.WriteFile:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Placeholder = "请输入文件路径",
                        Title = "路径",
                        BindingName = nameof(WriteFileActionInputModel.FilePath)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Placeholder = "请输入文件内容",
                        Title = "内容",
                        BindingName = nameof(WriteFileActionInputModel.Content)
                    });
                    break;
                case ActionType.IF:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Title = "如果",
                        Placeholder = "请输入",
                        BindingName = nameof(IFActionInputModel.Left)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Select,
                        SelectItems = IFActionConditionData.ComBoxData,
                        BindingName = nameof(IFActionInputModel.Condition)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Placeholder = "请输入",
                        BindingName = nameof(IFActionInputModel.Right)
                    });
                    break;
                case ActionType.HttpRequest:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Title = "请求地址",
                        Placeholder = "请输入完整地址",
                        IsStretch = true,
                        BindingName = nameof(HttpRequestActionInputModel.Url)
                    });
                    break;
                case ActionType.StartProcess:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Title = "进程路径",
                        Placeholder = "请输入进程路径",
                        IsStretch = true,
                        BindingName = nameof(StartProcessActionInputModel.Path)
                    });
                    break;
            }
            return groups;
        }
        #endregion

        #region 获取创建多行输入组模板
        private List<ActionInputModel> GetCreateActionMultiLineInputGroups()
        {
            var groups = new List<ActionInputModel>();
            switch (Action.ActionType)
            {
                case ActionType.HttpRequest:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Select,
                        Title = "方法",
                        SelectItems = HttpRequestActionData.MethodTypes,
                        BindingName = nameof(HttpRequestActionInputModel.Method)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Select,
                        Title = "参数类型",
                        SelectItems = HttpRequestActionData.PamramsTypes,
                        BindingName = nameof(HttpRequestActionInputModel.PamramsType)
                    });

                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.CustomKeyValue,
                        Title = "请求参数",
                        BindingName = nameof(HttpRequestActionInputModel.QueryParams)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.CustomKeyValue,
                        Title = "请求头",
                        BindingName = nameof(HttpRequestActionInputModel.Headers)
                    });
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.CustomKeyValue,
                        Title = "文件（仅Form参数类型时有效）",
                        BindingName = nameof(HttpRequestActionInputModel.Files)
                    });
                    break;
                case ActionType.StartProcess:
                    groups.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                        Title = "启动参数",
                        BindingName = nameof(StartProcessActionInputModel.Args)
                    });
                    break;

            }
            return groups;
        }
        #endregion
    }
}
