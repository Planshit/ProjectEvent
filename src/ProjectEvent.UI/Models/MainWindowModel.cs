using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Controls.Navigation.Models;
using ProjectEvent.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectEvent.UI.Models
{
    public class MainWindowModel : UINotifyPropertyChanged
    {
        private IServiceProvider ServiceProvider_;
        public IServiceProvider ServiceProvider
        {
            get { return ServiceProvider_; }
            set { ServiceProvider_ = value; OnPropertyChanged(); }
        }

        private string Uri_;
        public string Uri
        {
            get { return Uri_; }
            set { Uri_ = value; OnPropertyChanged(); }
        }

        private object Data_;
        public object Data
        {
            get { return Data_; }
            set { Data_ = value; OnPropertyChanged(); }
        }
        public ObservableCollection<NavigationItemModel> Items { get; set; }

        private double NavigationWidth_ = 220;
        public double NavigationWidth
        {
            get { return NavigationWidth_; }
            set { NavigationWidth_ = value; OnPropertyChanged(); }
        }
    }
}
