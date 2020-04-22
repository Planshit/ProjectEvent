using ProjectEvent.UI.Controls.InputGroup.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProjectEvent.UI.Controls.InputGroup
{
    public class InputGroup : Control
    {
        public object DataSource
        {
            get { return (object)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(object), typeof(InputGroup));


        public List<InputModel> Groups
        {
            get { return (List<InputModel>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }
        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(List<InputModel>), typeof(InputGroup), new PropertyMetadata(null, new PropertyChangedCallback(OnGroupsChanged)));

        private static void OnGroupsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as InputGroup;
            if (e.OldValue != e.NewValue)
            {
                control.Render();
            }
        }
        private StackPanel container;
        public InputGroup()
        {
            DefaultStyleKey = typeof(InputGroup);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            container = GetTemplateChild("Container") as StackPanel;
            Render();
        }

        private void Render()
        {
            if (Groups == null || container == null)
            {
                return;
            }
            container.Children.Clear();

            foreach (var input in Groups)
            {
                Control control = null;
                switch (input.Type)
                {
                    case InputType.Text:
                        control = new TextBox();
                        break;
                }
                //binding input
                BindingOperations.SetBinding(control, input.BindingProperty, new Binding()
                {
                    Source = DataSource,
                    Path = new PropertyPath(input.BindingName),
                    Mode = BindingMode.TwoWay,
                });
                var label = new Label();
                label.Content = input.Title;
                container.Children.Add(label);
                container.Children.Add(control);
            }
        }
    }
}
