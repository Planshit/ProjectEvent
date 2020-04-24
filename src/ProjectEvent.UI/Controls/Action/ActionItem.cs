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

        public BaseActionItemModel Action
        {
            get { return (BaseActionItemModel)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action",
                typeof(BaseActionItemModel),
                typeof(ActionItem),
                new PropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged))
                );
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (d as ActionItem);
            if (e.Property == ActionProperty)
            {

                var newValue = e.NewValue as BaseActionItemModel;

                if (newValue != null)
                {
                    control.Render();
                }
            }
        }
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
            var inputs = new List<ActionInputModel>();
            inputs.Add(new ActionInputModel()
            {
                Title = "路径",
                InputType = Types.InputType.Text
            });
            inputs.Add(new ActionInputModel()
            {
                Title = "内容",
                InputType = Types.InputType.Text
            });
            UIElement item = new Action(Action.ID, inputs, GetActionResults());

            //填充基本信息
            ActionName = $"[{Action.ID}] {Action.ActionName}";
            Icon = Action.Icon == string.Empty || Action.Icon == null ? "\xE3AE" : Action.Icon;

            Input.Child = item;


        }

        private Dictionary<int, List<string>> GetActionResults()
        {
            var result = new Dictionary<int, List<string>>();
            switch (Action.ActionType)
            {
                case ActionType.WriteFile:
                    //填充action results
                    result.Add(Action.ID, new List<string>()
                    {
                        "Status",
                    });
                    break;
            }
            return result;
        }
    }
}
