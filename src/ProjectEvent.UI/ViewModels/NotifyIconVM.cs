using ProjectEvent.UI.Controls.Window;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ProjectEvent.UI.ViewModels
{
    public class NotifyIconVM
    {
        private readonly IServiceProvider serviceProvider;
        private bool isMainWindowShow = false;
        private DefaultWindow mainWindow;

        public Command OpenUriCommand { get; set; }
        public Command ExitCommand { get; set; }

        public NotifyIconVM(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            OpenUriCommand = new Command(new Action<object>(OnOpenUriCommand));
            ExitCommand = new Command(new Action<object>(OnExitCommand));
        }

        private void OnExitCommand(object obj)
        {
            Application.Current.Shutdown();
        }

        private void OnOpenUriCommand(object obj)
        {
            ShowPage(obj.ToString());
        }

        private void ShowPage(string url)
        {
            if (isMainWindowShow && mainWindow != null)
            {
                var vm = mainWindow.DataContext as MainViewModel;
                if (vm != null)
                {
                    vm.Uri = url;
                    mainWindow.Show();
                    mainWindow.Activate();
                }
            }
            else
            {
                CreateMainWindow(url);
            }
        }
        private MainViewModel CreateMainWindow(string uri)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            var dataContext = serviceProvider.GetService<MainViewModel>();
            mainWindow.DataContext = dataContext;
            dataContext.SelectGroup(-1);
            dataContext.Uri = nameof(IndexPage);
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            mainWindow.Activate();
            isMainWindowShow = true;
            this.mainWindow = mainWindow;

            if (uri != nameof(IndexPage))
            {
                dataContext.Uri = uri;
            }
            return dataContext;
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            isMainWindowShow = false;
            this.mainWindow = null;
        }
    }
}
