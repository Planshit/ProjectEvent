using Microsoft.Extensions.DependencyInjection;
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
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        private void ConfigureServices(IServiceCollection services)
        {
            //core services
            services.AddSingleton<IEventContainerService, EventContainerService>();
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ITimerTaskService, TimerTaskService>();
            services.AddSingleton<IMainService, MainService>();
            services.AddSingleton<IDeviceTaskService, DeviceTaskService>();
            services.AddSingleton<IProcessTaskService, ProcessTaskService>();

            //ui services
            services.AddSingleton<ITrayService, TrayService>();
            services.AddSingleton<IApp, ProjectEvent.UI.Services.App>();
            services.AddSingleton<IProjects, Projects>();
            services.AddSingleton<IGroup, Group>();

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
            services.AddSingleton<IServiceProvider>(services.BuildServiceProvider());
        }


        private void OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            var app = _serviceProvider.GetService<IApp>();
            app.Run();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Error(e.Exception.ToString());
            e.Handled = true;
        }
    }
}
