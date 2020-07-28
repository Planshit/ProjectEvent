using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectEvent.UI.Controls.Window
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : DefaultWindow
    {
        private string clickValue = string.Empty;
        public delegate void WindowClosedEventHandler(object sender, string clickValue);
        public event WindowClosedEventHandler OnWindowClosedEvent;
        public DialogWindow(string title, string content, string icon, Dictionary<string, string> buttons)
        {
            InitializeComponent();
            this.Title = title;
            ContentBlock.Text = content;
            SetIcon(icon);
            CreateButtons(buttons);
        }

        private void SetIcon(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return;
            }
            var bitmap = new BitmapImage();
            byte[] imageData;
            bitmap.BeginInit();
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))

            using (var binaryReader = new BinaryReader(fileStream))
            {
                imageData = binaryReader.ReadBytes((int)fileStream.Length);
                bitmap.StreamSource = new MemoryStream(imageData);
                bitmap.EndInit();
                bitmap.Freeze();
            }
            Icon.Source = bitmap;
            Icon.Visibility = Visibility.Visible;
        }
        private void CreateButtons(Dictionary<string, string> buttons)
        {
            if (buttons != null)
            {
                int i = 0;
                foreach (var item in buttons)
                {
                    var btn = new Button();
                    btn.Style = FindResource(i == 0 ? "Primary" : "Standard") as Style;
                    btn.Content = item.Value;
                    if (i != 0)
                    {
                        btn.Margin = new Thickness(10, 0, 0, 0);
                    }
                    btn.Click += (e, c) =>
                    {
                        clickValue = item.Key;
                        Close();
                    };
                    ButtonsPanel.Children.Add(btn);
                    i++;
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            OnWindowClosedEvent?.Invoke(this, clickValue);
        }
    }
}
