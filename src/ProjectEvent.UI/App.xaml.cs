﻿using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.Core.Services;
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
            services.AddSingleton<IMainService, MainService>();
            services.AddSingleton<IEventContainerService, EventContainerService>();
            services.AddSingleton<ITimerService, TimerService>();
            services.AddSingleton<ITimerTaskService, TimerTaskService>();
            services.AddSingleton<ITrayService, TrayService>();
            

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
            var tray = _serviceProvider.GetService<ITrayService>();
            tray.Init();
        }

    }
}
