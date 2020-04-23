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
        public Action(List<ActionInputModel> inputs)
        {
            DefaultStyleKey = typeof(Action);
            this.inputs = inputs;
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
                var input = new ActionInput();
                container.Children.Add(label);
                container.Children.Add(input);

            }
        }
    }
}
