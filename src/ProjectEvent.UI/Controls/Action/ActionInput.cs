using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ProjectEvent.UI.Controls.Action
{
    public class ActionInput : Control
    {
        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register("PlaceholderVisibility",
                typeof(Visibility),
                typeof(ActionInput));

        public bool PopupIsOpen
        {
            get { return (bool)GetValue(PopupIsOpenProperty); }
            set { SetValue(PopupIsOpenProperty, value); }
        }
        public static readonly DependencyProperty PopupIsOpenProperty =
            DependencyProperty.Register("PopupIsOpen",
                typeof(bool),
                typeof(ActionInput),
                new PropertyMetadata(false));
        public int ActionID { get; set; }
        private Dictionary<int, List<string>> actionResults;
        private TextBox inputTextBox;
        private ComboBox ActionIDComboBox;
        private ComboBox ActionResultsComboBox;
        private Popup Popup;
        private bool isEnterPopup;

        public ActionInput(Dictionary<int, List<string>> actionResults)
        {
            DefaultStyleKey = typeof(ActionInput);
            this.actionResults = actionResults;
            isEnterPopup = false;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            inputTextBox = GetTemplateChild("InputTextBox") as TextBox;
            ActionIDComboBox = GetTemplateChild("ActionIDComboBox") as ComboBox;
            ActionResultsComboBox = GetTemplateChild("ActionResultsComboBox") as ComboBox;
            Popup = GetTemplateChild("Popup") as Popup;
            ActionIDComboBox.SelectionChanged += ActionIDComboBox_SelectionChanged;
            inputTextBox.TextChanged += InputTextBox_TextChanged;
            inputTextBox.GotKeyboardFocus += InputTextBox_GotKeyboardFocus;
            inputTextBox.LostKeyboardFocus += InputTextBox_LostKeyboardFocus;
            Popup.MouseEnter += Popup_MouseEnter;
            Popup.MouseLeave += Popup_MouseLeave;
            Render();
        }

        private void InputTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (!isEnterPopup)
            {
                PopupIsOpen = false;
            }
        }

        private void Popup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isEnterPopup)
            {
                PopupIsOpen = false;
            }
            isEnterPopup = false;
        }

        private void Popup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isEnterPopup = true;
        }

        private void InputTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            PopupIsOpen = true;
        }

        private void ActionIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActionResultsComboBox.Items.Clear();
            var results = actionResults[(int)ActionIDComboBox.SelectedItem];
            foreach (var name in results)
            {
                ActionResultsComboBox.Items.Add(name);
            }
            if (ActionResultsComboBox.Items.Count > 0)
            {
                ActionResultsComboBox.SelectedIndex = 0;
            }
        }

        private void Render()
        {
            foreach (var result in actionResults)
            {
                ActionIDComboBox.Items.Add(result.Key);
            }
            if (ActionIDComboBox.Items.Count > 0)
            {
                ActionIDComboBox.SelectedIndex = 0;
            }
        }
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderVisibility = inputTextBox.Text == string.Empty ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
