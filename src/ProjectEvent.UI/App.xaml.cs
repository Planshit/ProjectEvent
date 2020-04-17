using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Services.TimerTask;
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
        }


        private void OnStartup(object sender, StartupEventArgs e)
        {
            var main = _serviceProvider.GetService<IMainService>();
            main.Start();

            //var a = new Task<bool>(() =>
            //  {

            //      Thread.Sleep(3000);
            //      return true;
            //  });
            //a.Start();
            
            ////Debug.WriteLine(a.Result);
            //Debug.WriteLine(2);

        }

    }
}
