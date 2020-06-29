using Microsoft.Extensions.DependencyInjection;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Controls.Navigation.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using ProjectEvent.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

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
        /// <summary>
        /// 页面传递数据
        /// </summary>
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

        private bool IsShowNavigation_ = true;
        public bool IsShowNavigation
        {
            get { return IsShowNavigation_; }
            set { IsShowNavigation_ = value; OnPropertyChanged(); }
        }

        private string Title_;
        public string Title
        {
            get { return Title_; }
            set { Title_ = value; OnPropertyChanged(); }
        }
        private bool IsShowTitleBar_ = false;
        public bool IsShowTitleBar
        {
            get { return IsShowTitleBar_; }
            set { IsShowTitleBar_ = value; OnPropertyChanged(); }
        }
        public List<GroupModel> Groups { get; set; }

        private ToastType ToastType_ = ToastType.Normal;
        public ToastType ToastType
        {
            get { return ToastType_; }
            set { ToastType_ = value; OnPropertyChanged(); }
        }
        public string ToastContent_;
        public string ToastContent
        {
            get { return ToastContent_; }
            set { ToastContent_ = value; OnPropertyChanged(); }
        }

        private bool IsShowToast_ = false;
        public bool IsShowToast
        {
            get { return IsShowToast_; }
            set { IsShowToast_ = value; OnPropertyChanged(); }
        }

        private int GroupID_ = -1;
        public int GroupID
        {
            get { return GroupID_; }
            set { GroupID_ = value; OnPropertyChanged(); }
        }
        private string GroupName_ = "";
        public string GroupName
        {
            get { return GroupName_; }
            set { GroupName_ = value; OnPropertyChanged(); }
        }
        private IconTypes GroupIcon_ = IconTypes.BulletedList;
        public IconTypes GroupIcon
        {
            get { return GroupIcon_; }
            set { GroupIcon_ = value; OnPropertyChanged(); }
        }
        private Visibility GroupModalVisibility_ = Visibility.Collapsed;
        public Visibility GroupModalVisibility
        {
            get { return GroupModalVisibility_; }
            set { GroupModalVisibility_ = value; OnPropertyChanged(); }
        }
    }
}
