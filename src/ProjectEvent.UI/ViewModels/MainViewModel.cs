using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class MainViewModel : MainWindowModel
    {
        private readonly IServiceProvider serviceProvider;
        
        public Command Test { get; set; }
       
        public string wb { get; set; }
        public MainViewModel(
            IServiceProvider serviceProvider
            )
        {
            this.serviceProvider = serviceProvider;

            ServiceProvider = serviceProvider;

            Uri = "IndexPage";

            Test = new Command(new Action<object>(test));


            wb = "123";

        }

        private void test(object obj)
        {
            Uri = "SettingsPage";
        }


    }
}
