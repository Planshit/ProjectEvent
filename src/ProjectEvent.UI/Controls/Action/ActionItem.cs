using ProjectEvent.UI.Controls.Action.Models;
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
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon",
                typeof(string),
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
        public event EventHandler OnClick;
        private Grid Header;
        private Border Input;
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
            if (Action.ParentID > 0)
            {
                ActionName = $"{Action.ActionName}";
            }
            else
            {
                Action item = new Action(Action.ID, GetInputs());
                ActionName = $"[{Action.ID}] {Action.ActionName}";
                Icon = Action.Icon;
                Input.Child = item;
                item.OnClick += Item_OnClick;
            }

        }

        private void Item_OnClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(sender, e);
        }

        private List<ActionInputModel> GetInputs()
        {
            var inputs = new List<ActionInputModel>();
            switch (Action.ActionType)
            {
                case ActionType.WriteFile:
                    inputs.Add(new ActionInputModel()
                    {
                        Title = "路径",
                        InputType = Types.InputType.Text,
                    });
                    inputs.Add(new ActionInputModel()
                    {
                        Title = "内容",
                        InputType = Types.InputType.Text
                    });
                    break;
                case ActionType.IF:
                    inputs.Add(new ActionInputModel()
                    {
                        Title = "如果",
                        InputType = Types.InputType.Text,
                    });
                    inputs.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Select,
                        SelectItems = new List<string>()
                        {
                            "等于",
                            "不等于",
                        }
                    });
                    inputs.Add(new ActionInputModel()
                    {
                        InputType = Types.InputType.Text,
                    });
                    break;
            }
            return inputs;
        }

    }
}
