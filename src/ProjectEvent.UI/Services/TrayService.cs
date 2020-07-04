using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Controls.Window;
using ProjectEvent.UI.ViewModels;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;

namespace ProjectEvent.UI.Services
{
    public class TrayService : ITrayService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventLog eventLog;

        //隐藏窗口，用于处理托盘菜单关闭功能
        private Window stateWindow;
        //托盘图标
        private System.Windows.Forms.NotifyIcon notifyIcon;


        //托盘菜单项
        private ContextMenu contextMenu;

        private MenuItem menuItemSettings;
        private MenuItem menuItemQuit;
        private MenuItem menuItemIndexPage;
        private bool IsMainWindowShow = false;
        private DefaultWindow mainWindow;
        public TrayService(
            MainWindow mainWindow,
            IServiceProvider _serviceProvider,
            IEventLog eventLog)
        {
            this._serviceProvider = _serviceProvider;
            this.eventLog = eventLog;

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            stateWindow = new Window();
            stateWindow.Width = 0;
            stateWindow.Height = 0;
            stateWindow.Visibility = Visibility.Hidden;
            stateWindow.AllowsTransparency = true;
            stateWindow.ShowInTaskbar = false;
            stateWindow.Opacity = 0;
            stateWindow.WindowStyle = WindowStyle.None;
            stateWindow.WindowState = WindowState.Minimized;
        }

        public void Init()
        {
            //托盘菜单
            CreateTrayMenu();
            UpdateIcon("tray");
            notifyIcon.Text = "Project Event";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;
            stateWindow.Show();
        }

        private void NotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                OpenShowPage(nameof(IndexPage));
            }
        }

        private void OpenShowPage(string page)
        {
            if (IsMainWindowShow && mainWindow != null)
            {
                var vm = mainWindow.DataContext as MainViewModel;
                if (vm != null)
                {
                    vm.Uri = page;
                    mainWindow.Show();
                    mainWindow.Activate();
                }
            }
            else
            {
                CreateMainWindow(page);
            }
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            IsMainWindowShow = false;
            this.mainWindow = null;
        }

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //右键单击弹出托盘菜单
                contextMenu.IsOpen = true;
                //激活隐藏窗口，用于处理关闭托盘菜单
                stateWindow.Activate();
            }
        }
        private void CreateTrayMenu()
        {
            contextMenu = new ContextMenu();
            stateWindow.Deactivated += (e, c) =>
            {
                contextMenu.IsOpen = false;
            };

            menuItemIndexPage = new MenuItem();
            menuItemIndexPage = new MenuItem();
            menuItemIndexPage.Header = "主界面";
            //menuItemIndexPage.Icon = new Controls.Base.Icon()
            //{
            //    IconType = IconTypes.Settings
            //};

            menuItemIndexPage.Click += MenuItemIndexPage_Click; ;
            menuItemSettings = new MenuItem();
            menuItemSettings.Header = "设置";
            menuItemSettings.Icon = new Controls.Base.Icon()
            {
                IconType = IconTypes.Settings
            };
            menuItemSettings.Click += MenuItem_Settings_Click; ;

            menuItemQuit = new MenuItem();
            menuItemQuit.Header = "退出";
            menuItemQuit.Click += menuItem_Exit_Click;

            contextMenu.Items.Add(menuItemIndexPage);
            contextMenu.Items.Add(menuItemSettings);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(menuItemQuit);
        }

        private void MenuItemIndexPage_Click(object sender, RoutedEventArgs e)
        {
            OpenShowPage(nameof(IndexPage));
        }

        private MainViewModel CreateMainWindow(string uri)
        {
            DefaultWindow mainWindow = _serviceProvider.GetService<MainWindow>();
            var dataContext = _serviceProvider.GetService<MainViewModel>();
            mainWindow.DataContext = dataContext;
            dataContext.SelectGroup(-1);
            dataContext.Uri = nameof(IndexPage);
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Show();
            mainWindow.Activate();
            IsMainWindowShow = true;
            this.mainWindow = mainWindow;

            if (uri != nameof(IndexPage))
            {
                dataContext.Uri = uri;
            }
            return dataContext;
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            OpenShowPage(nameof(SettingsPage));
        }

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            eventLog.Save();
            Application.Current.Shutdown();
        }
        private void UpdateIcon(string name = "")
        {
            if (notifyIcon != null && name != "")
            {
                Uri iconUri = new Uri("/ProjectEvent.UI;component/Assets/Icons/" + name + ".ico", UriKind.RelativeOrAbsolute);
                StreamResourceInfo info = Application.GetResourceStream(iconUri);
                notifyIcon.Icon = new System.Drawing.Icon(info.Stream);

            }
        }
    }
}
