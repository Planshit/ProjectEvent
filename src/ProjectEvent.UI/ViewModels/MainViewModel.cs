using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls;
using ProjectEvent.UI.Controls.ItemSelect;
using ProjectEvent.UI.Controls.Navigation;
using ProjectEvent.UI.Controls.Navigation.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            //创建数据文件夹
            IOHelper.CreateDirectory("Data");
            InitGroupManagerContextMenu();
            InitNavigation();
            LoadGroups();
        }

        private void OnDeleteGroupCommand(object obj)
        {
            Toast("分组已被移除");
            Groups.Remove(Groups.Where(m => m.ID == selectedNavigationItem.ID).FirstOrDefault());
            Items.Remove(selectedNavigationItem);
            SaveGroup();
        }

        private void OnMouseRightButtonUPCommandHandle(object obj)
        {
            var navigation = obj as Navigation;
            selectedNavigationItem = navigation.SelectedItem;
            groupManagerContextMenu.IsOpen = true;
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
            var navigation = obj as Navigation;
            if (!string.IsNullOrEmpty(navigation.SelectedItem.Uri))
            {
                Uri = navigation.SelectedItem.Uri;
            }
            else
            {
                //分组功能
                Data = Groups.Where(m => m.ID == navigation.SelectedItem.ID).FirstOrDefault();
            }
            Debug.Write(navigation.SelectedItem.ID);
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
            Items.Add(new Controls.Navigation.Models.NavigationItemModel()
            {
                BadgeText = "1",
                Icon = Controls.Base.IconTypes.Timer,
                Title = "Test3",
                ID = 100
            });
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


        private void Toast(string content)
        {
            if (IsShowToast)
            {
                IsShowToast = false;
            }
            ToastContent = content;
            IsShowToast = true;
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
                Toast("分组已添加");
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
                    Toast("分组已更新");
                }
            }
        }
        private bool ValidGroupInput(bool edit = false)
        {
            if (GroupName == string.Empty)
            {
                Toast("请输入分组名称");
            }
            else if (GroupName.Length > 8)
            {
                Toast("分组名称限制最多8个字符");
            }
            else if (!edit && Groups.Where(m => m.Name == GroupName).Count() > 0)
            {
                Toast("分组名称已存在");
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
    }
}
