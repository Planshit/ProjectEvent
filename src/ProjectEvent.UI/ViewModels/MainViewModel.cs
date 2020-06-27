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
        public Command GotoPageCommand { get; set; }

        public MainViewModel(
            IServiceProvider serviceProvider
            )
        {
            this.serviceProvider = serviceProvider;
            ServiceProvider = serviceProvider;
            Uri = "IndexPage";
            Title = "Project Event";
            OnSelectedCommand = new Command(new Action<object>(OnSelectedCommandHandle));
            GotoPageCommand = new Command(new Action<object>(OnGotoPageCommand));
            Items = new System.Collections.ObjectModel.ObservableCollection<Controls.Navigation.Models.NavigationItemModel>();
            InitNavigation();
        }

        private void OnGotoPageCommand(object obj)
        {
            Uri = obj.ToString();
        }

        private void OnSelectedCommandHandle(object obj)
        {
            var navigation = obj as Navigation;
            if (!string.IsNullOrEmpty(navigation.SelectedItem.Uri))
            {
                Uri = navigation.SelectedItem.Uri;
            }
            Debug.Write(navigation.SelectedItem.ID);
        }

        private void InitNavigation()
        {
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                Icon = Controls.Base.IconTypes.AppIconDefaultList,
                Title = "所有自动化方案",
                Uri = "IndexPage"

            });
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                Icon = Controls.Base.IconTypes.ClipboardList,
                Title = "触发日志",
            });
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
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
