using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Controls.Navigation;
using ProjectEvent.UI.Controls.Navigation.Models;
using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ProjectEvent.UI.ViewModels
{
    public class MainViewModel : MainWindowModel
    {
        private readonly IServiceProvider serviceProvider;
        public Command OnSelectedCommand { get; set; }
        public MainViewModel(
            IServiceProvider serviceProvider
            )
        {
            this.serviceProvider = serviceProvider;
            ServiceProvider = serviceProvider;
            Uri = "IndexPage";
            OnSelectedCommand = new Command(new Action<object>(OnSelectedCommandHandle));
            Items = new System.Collections.ObjectModel.ObservableCollection<Controls.Navigation.Models.NavigationItemModel>();
            r();
        }

        private void OnSelectedCommandHandle(object obj)
        {
            var navigation = obj as Navigation;
            Debug.Write(navigation.SelectedItem.ID);
        }

        private void r()
        {
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                BadgeText = "1",
                Icon = Controls.Base.IconTypes.Timer,
                Title = "全部",

            });

            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                BadgeText = "1",
                Icon = Controls.Base.IconTypes.Timer,
                Title = "Test3",

            });
        }

    }
}
