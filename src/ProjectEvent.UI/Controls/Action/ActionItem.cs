using ProjectEvent.UI.Controls.Action.Builders;
using ProjectEvent.UI.Controls.Action.Data;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Border IDBorder;

        public ActionContainer ActionContainer { get; set; }
        public object VMDataContext { get; set; }
        public IActionBuilder Builder { get; set; }
        private ActionType[] specialTypes = { ActionType.IFElse, ActionType.IFEnd, ActionType.LoopsEnd };
        public ActionItem()
        {
            DefaultStyleKey = typeof(ActionItem);
            var translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            translateTransform.Changed += translateTransform_Changed;
            RenderTransform = translateTransform;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Input = GetTemplateChild("Input") as Border;
            Header = GetTemplateChild("Header") as Grid;
            ButtonRemove = GetTemplateChild("ButtonRemove") as Button;
            ActionForm = GetTemplateChild("ActionForm") as ActionForm;
            IDBorder = GetTemplateChild("IDBorder") as Border;
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
            if (specialTypes.Contains(Action.ActionType))
            {
                ActionName = $"{Action.ActionName}";
                Loaded += (e, c) =>
                {
                    OnRenderDone?.Invoke(this, null);
                };
                IDBorder.Visibility = Visibility.Collapsed;
                //查找父级
                var parent = ActionContainer.ActionItems.Where(m => m.Action.ID == Action.ParentID).FirstOrDefault();
                ToolTip = $"属于 [{parent.Action.ID}] {parent.Action.ActionName}";
                Cursor = Cursors.No;
            }
            else
            {
                ActionName = Action.ActionName;
                Icon = Action.Icon;
                ActionForm.DataContext = Builder.GetInputModelData();
                ActionForm.LineInputGroups = Builder.GetBaseActionInputModels();
                ActionForm.MultiLineInputGroups = Builder.GetDetailActionInputModels();
                ActionForm.Action = Action;

                ActionForm.ActionContainer = ActionContainer;
                ActionForm.OnRenderDone += (e, c) =>
                {
                    OnRenderDone?.Invoke(this, null);
                };
            }
            if (specialTypes.Contains(Action.ActionType))
            {
                ButtonRemove.Visibility = Visibility.Collapsed;
            }

        }

        private void Item_OnClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(sender, e);
        }


    }
}
