using Microsoft.Extensions.DependencyInjection;
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

        private MenuItem menuItem_Options;
        private MenuItem menuItem_Quit;
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
                if (IsMainWindowShow && mainWindow!=null)
                {
                    mainWindow.Show();
                    mainWindow.Activate();
                }
                else
                {
                    DefaultWindow mainWindow = _serviceProvider.GetService<MainWindow>();
                    var dataContext = _serviceProvider.GetService<MainViewModel>();
                    mainWindow.DataContext = dataContext;
                    dataContext.SelectGroup(-1);
                    dataContext.Uri = nameof(IndexPage);
                    mainWindow.Closed += MainWindow_Closed;
                    mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    mainWindow.Focus();
                    mainWindow.Show();
                    IsMainWindowShow = true;
                    this.mainWindow = mainWindow;
                }
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

            menuItem_Options = new MenuItem();
            menuItem_Options.Header = "选项";
            //menuItem_Options.Click += menuItem_Options_Click;




            menuItem_Quit = new MenuItem();
            menuItem_Quit.Header = "退出";
            menuItem_Quit.Click += menuItem_Exit_Click;

            //添加托盘菜单项
            contextMenu.Items.Add(menuItem_Options);
            contextMenu.Items.Add(new Separator());
            contextMenu.Items.Add(menuItem_Quit);

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
                notifyIcon.Icon = new Icon(info.Stream);

            }
        }
    }
}
