using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Controls.ItemSelect;
using ProjectEvent.UI.Controls.Navigation;
using ProjectEvent.UI.Controls.Navigation.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class MainViewModel : MainWindowModel
    {
        private readonly IServiceProvider serviceProvider;
        public Command OnSelectedCommand { get; set; }
        public Command GotoPageCommand { get; set; }
        public Command SaveGroupCommand { get; set; }
        public Command ShowGroupModalCommand { get; set; }
        public Command HideGroupModalCommand { get; set; }
        public Command OnMouseRightButtonUPCommand { get; set; }
        public Command DeleteGroupCommand { get; set; }
        private ContextMenu groupManagerContextMenu;
        private NavigationItemModel selectedNavigationItem;
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
            SaveGroupCommand = new Command(new Action<object>(OnSaveGroupCommand));
            ShowGroupModalCommand = new Command(new Action<object>(OnShowGroupModalCommand));
            HideGroupModalCommand = new Command(new Action<object>(OnHideGroupModalCommand));
            OnMouseRightButtonUPCommand = new Command(new Action<object>(OnMouseRightButtonUPCommandHandle));
            DeleteGroupCommand = new Command(new Action<object>(OnDeleteGroupCommand));

            Items = new System.Collections.ObjectModel.ObservableCollection<Controls.Navigation.Models.NavigationItemModel>();
            Groups = new List<GroupModel>();

            PropertyChanged += MainViewModel_PropertyChanged;

            InitGroupManagerContextMenu();
            InitNavigation();
            LoadGroups();
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Uri))
            {
                if (Uri == nameof(IndexPage))
                {
                    Data = null;
                }
            }
        }

        private void OnDeleteGroupCommand(object obj)
        {
            Toast("分组已被移除", ToastType.Success);
            var group = Groups.Where(m => m.ID == selectedNavigationItem.ID).FirstOrDefault();
            int removeIndex = Groups.IndexOf(group);
            Groups.Remove(group);
            Items.Remove(selectedNavigationItem);
            if (NavSelectedItem.ID != -1)
            {
                if (Groups.Count > 0)
                {
                    SelectGroup(Groups[removeIndex - 1].ID);
                }
                else
                {
                    SelectGroup(-1);
                }
            }
            SaveGroup();
        }

        private void OnMouseRightButtonUPCommandHandle(object obj)
        {
            var args = obj as RoutedEventArgs;
            selectedNavigationItem = args.Source as NavigationItemModel;
            if (selectedNavigationItem.ID >= 100)
            {
                groupManagerContextMenu.IsOpen = true;
            }
        }

        private void OnHideGroupModalCommand(object obj)
        {
            GroupModalVisibility = System.Windows.Visibility.Collapsed;
            ResetGroupInput();
        }

        private void OnShowGroupModalCommand(object obj)
        {
            GroupModalVisibility = System.Windows.Visibility.Visible;
            if (obj != null)
            {
                GroupName = selectedNavigationItem.Title;
                GroupID = selectedNavigationItem.ID;
                GroupIcon = selectedNavigationItem.Icon;
            }
        }

        private void OnSaveGroupCommand(object obj)
        {
            if (GroupID != -1)
            {
                EditGroup();
            }
            else
            {
                CreateGroup();
            }
        }

        private void OnGotoPageCommand(object obj)
        {
            Uri = obj.ToString();
        }

        private void OnSelectedCommandHandle(object obj)
        {
            if (!string.IsNullOrEmpty(NavSelectedItem.Uri))
            {
                Uri = NavSelectedItem.Uri;
            }
            else
            {
                //分组功能
                SelectedGroup = Groups.Where(m => m.ID == NavSelectedItem.ID).FirstOrDefault();
            }
            Debug.Write(NavSelectedItem.ID);
        }

        private void InitNavigation()
        {
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                Icon = Controls.Base.IconTypes.AppIconDefaultList,
                Title = "所有自动化方案",
                ID = -1

            });
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                Icon = Controls.Base.IconTypes.ClipboardList,
                Title = "触发日志",
                ID = 2
            });
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
            });
            NavSelectedItem = Items[0];
        }

        private void LoadGroups()
        {
            if (Items.Count > 3)
            {
                for (int i = 3; i < Items.Count; i++)
                {
                    Items.RemoveAt(i);
                }
            }
            //分组数据路径
            string path = IOHelper.GetFullPath("Data\\Groups.json");
            if (File.Exists(path))
            {
                Groups = JsonConvert.DeserializeObject<List<GroupModel>>(File.ReadAllText(path));
                foreach (var group in Groups)
                {
                    Items.Add(new NavigationItemModel()
                    {
                        ID = group.ID,
                        Icon = group.Icon,
                        Title = group.Name,
                    });
                }
            }

        }


        public void Toast(string content, ToastType toastType = ToastType.Normal)
        {
            if (IsShowToast)
            {
                IsShowToast = false;
            }
            ToastContent = content;
            IsShowToast = true;
            ToastType = toastType;
        }
        private void CreateGroup()
        {
            if (ValidGroupInput())
            {
                int GID = Groups.Count > 0 ? Groups.Max(m => m.ID) + 1 : 100;
                Groups.Add(new GroupModel()
                {
                    ID = GID,
                    Icon = GroupIcon,
                    Name = GroupName
                });

                Items.Add(new NavigationItemModel()
                {
                    ID = GID,
                    Icon = GroupIcon,
                    Title = GroupName
                });
                SaveGroup();
                Toast("分组已添加", ToastType.Success);
                SelectGroup(GID);
                HideGroupModalCommand.Execute(null);
            }
        }
        private void EditGroup()
        {
            if (ValidGroupInput(true))
            {
                var group = Groups.Where(m => m.ID == GroupID).FirstOrDefault();
                var navitem = Items.Where(m => m.ID == GroupID).FirstOrDefault();
                int navitemIndex = Items.IndexOf(navitem);
                if (group != null)
                {
                    group.Name = GroupName;
                    group.Icon = GroupIcon;

                    navitem.Title = GroupName;
                    navitem.Icon = GroupIcon;
                    Items[navitemIndex] = navitem;
                    SaveGroup();
                    Toast("分组已更新", ToastType.Success);
                    SelectGroup(group.ID);
                }
            }
        }
        private bool ValidGroupInput(bool edit = false)
        {
            if (GroupName == string.Empty)
            {
                Toast("请输入分组名称", ToastType.Failed);
            }
            else if (GroupName.Length > 8)
            {
                Toast("分组名称限制最多8个字符", ToastType.Failed);
            }
            else if (!edit && Groups.Where(m => m.Name == GroupName).Count() > 0)
            {
                Toast("分组名称已存在", ToastType.Failed);
            }
            else
            {
                return true;
            }
            return false;
        }
        private void ResetGroupInput()
        {
            GroupID = -1;
            GroupName = "";
            GroupIcon = Controls.Base.IconTypes.BulletedList;
        }
        private void SaveGroup()
        {
            IOHelper.WriteFile("Data\\Groups.json", JsonConvert.SerializeObject(Groups));
        }
        private void InitGroupManagerContextMenu()
        {
            groupManagerContextMenu = new ContextMenu();
            MenuItem delItem = new MenuItem()
            {
                Header = "删除分组"
            };
            delItem.Command = DeleteGroupCommand;
            MenuItem editItem = new MenuItem()
            {
                Header = "编辑分组"
            };
            editItem.Command = ShowGroupModalCommand;
            editItem.CommandParameter = true;
            groupManagerContextMenu.Items.Add(delItem);
            groupManagerContextMenu.Items.Add(editItem);

        }
        private void SelectGroup(int ID)
        {
            NavSelectedItem = Items.Where(m => m.ID == ID).FirstOrDefault();
            SelectedGroup = Groups.Where(m => m.ID == ID).FirstOrDefault();
        }
    }
}
