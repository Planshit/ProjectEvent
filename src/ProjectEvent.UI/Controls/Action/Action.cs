using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.Action
{
    public class Action : Control
    {
        private List<ActionInputModel> inputs;
        private StackPanel container;
        private int ID;
        public Action(int ID, List<ActionInputModel> inputs)
        {
            DefaultStyleKey = typeof(Action);
            this.inputs = inputs;
            this.ID = ID;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = GetTemplateChild("Container") as StackPanel;
            Render();
        }

        private void Render()
        {
            container.Children.Clear();
            foreach (var item in inputs)
            {
                var label = new Label();
                label.Content = item.Title;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                var input = new ActionInput();
                input.ActionID = ID;
                input.InputType = item.InputType;
                input.SelectItems = item.SelectItems;
                input.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                container.Children.Add(label);
                container.Children.Add(input);

            }
        }
    }
}
