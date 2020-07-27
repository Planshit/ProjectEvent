using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Services.Tasks;
using ProjectEvent.Core.Services.TimerTask;
using ProjectEvent.UI;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Services;
using ProjectEvent.UI.ViewModels;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProjectEvent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private TaskbarIcon notifyIcon;
        private Window lifeWindow;
        private System.Threading.Mutex mutex;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

        }
        private void ConfigureServices(IServiceCollection services)
        {
            //core services
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ITimerTaskService, TimerTaskService>();
            services.AddSingleton<IMainService, MainService>();
            services.AddSingleton<IDeviceTaskService, DeviceTaskService>();
            services.AddSingleton<IProcessTaskService, ProcessTaskService>();
            services.AddSingleton<IFileTaskService, FileTaskService>();
            services.AddSingleton<IKeyboardTaskService, KeyboardTaskService>();
            services.AddSingleton<INetworkStatusTaskService, NetworkStatusTaskService>();
            services.AddSingleton<IBluetoothTaskService, BluetoothTaskService>();

            //ui services
            services.AddSingleton<IApp, ProjectEvent.UI.Services.App>();
            services.AddSingleton<IProjects, Projects>();
            services.AddSingleton<IGroup, Group>();
            services.AddSingleton<IEventLog, UI.Services.EventLog>();
            services.AddSingleton<ISettingsService, UI.Services.SettingsService>();

            //services.AddTransient<PageContainer>();
            services.AddSingleton<MainViewModel>();
            services.AddTransient<MainWindow>();
            //主页
            services.AddTransient<IndexPage>();
            services.AddTransient<IndexPageVM>();
            //设置页
            services.AddTransient<SettingsPage>();
            services.AddTransient<SettingsPageVM>();
            //添加页
            services.AddTransient<AddEventPage>();
            services.AddTransient<AddEventPageVM>();
            //事件日志页
            services.AddTransient<EventLogPage>();
            services.AddTransient<EventLogPageVM>();
            //托盘图标
            services.AddSingleton<NotifyIconVM>();

            services.AddSingleton<IServiceProvider>(services.BuildServiceProvider());
        }


        private void OnStartup(object sender, StartupEventArgs e)
        {
            if (IsRuning())
            {
                //发送标记
                var client = new NamedPipeClientStream(nameof(ProjectEvent));
                client.Connect();
                StreamWriter writer = new StreamWriter(client);
                string input = "0";
                writer.WriteLine(input);
                writer.Flush();
                Current.Shutdown();
            }
            else
            {
                lifeWindow = new Window();
                lifeWindow = new Window();
                lifeWindow.Width = 0;
                lifeWindow.Height = 0;
                lifeWindow.Visibility = Visibility.Hidden;
                lifeWindow.AllowsTransparency = true;
                lifeWindow.ShowInTaskbar = false;
                lifeWindow.Opacity = 0;
                lifeWindow.WindowStyle = WindowStyle.None;
                lifeWindow.WindowState = WindowState.Minimized;
                lifeWindow.Show();
                DispatcherUnhandledException += App_DispatcherUnhandledException;
                var app = _serviceProvider.GetService<IApp>();
                app.Run();
                var notifyIconVM = _serviceProvider.GetService<NotifyIconVM>();

                notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
                if (notifyIcon != null)
                {
#if DEBUG
                    notifyIconVM.ToolTipText = "[Debug] Project Event";
#endif
                    notifyIcon.DataContext = notifyIconVM;
                }
                StartPipeServer();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose();
            base.OnExit(e);
            //保存事件日志
            var evlog = _serviceProvider.GetService<IEventLog>();
            evlog?.Save();
            //保存程序日志
            LogHelper.Save();
        }
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.Exception.ToString());
            e.Handled = true;
            var evlog = _serviceProvider.GetService<IEventLog>();
            evlog.Save();
        }
        private bool IsRuning()
        {
            //debug模式允许多开
#if DEBUG
            return false;

#endif
            bool ret;
            mutex = new System.Threading.Mutex(true, System.AppDomain.CurrentDomain.FriendlyName, out ret);
            return !ret;
        }

        #region pipes server
        private void StartPipeServer()
        {
            //启动通讯服务，接收多次启动标记
            var notifyIconVM = _serviceProvider.GetService<NotifyIconVM>();
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream(nameof(ProjectEvent));
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == "0")
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            notifyIconVM?.ShowPage(nameof(IndexPage));
                        });
                    }
                    if (!server.IsConnected)
                    {
                        server.Close();
                        reader.Close();
                        StartPipeServer();
                        break;
                    }
                }
            });
        }
        #endregion
    }
}
