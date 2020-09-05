using Microsoft.Win32;
using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Services;
using ProjectEvent.UI.Base.Color;
using ProjectEvent.UI.Controls.Base;
using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Controls.Navigation;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Services;
using ProjectEvent.UI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class IndexPageVM : IndexPageModel
    {
        private readonly MainViewModel mainVM;
        private readonly IProjects projects;
        private readonly IGroup groupService;
        private readonly IApp app;
        public Command RedirectCommand { get; set; }
        public Command LoadedCommand { get; set; }

        public ContextMenu ItemContextMenu { get; set; }
        public ContextMenu ContainerContextMenu { get; set; }

        private GroupModel group;
        private MenuItem moveGroupMenutItem;
        public IndexPageVM(
            MainViewModel mainVM,
            IProjects projects,
            IGroup groupService,
            IApp app)
        {
            this.mainVM = mainVM;
            this.projects = projects;
            this.groupService = groupService;
            this.app = app;

            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            LoadedCommand = new Command(new Action<object>(OnLoadedCommand));
            PropertyChanged += IndexPageVM_PropertyChanged;
            Projects = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            mainVM.PropertyChanged += MainVM_PropertyChanged;
            mainVM.IsShowNavigation = true;
            mainVM.IsShowTitleBar = false;

            ItemContextMenu = new ContextMenu();
            ContainerContextMenu = new ContextMenu();
            Init();
            CreateItemContextMenu();
            CreateContainerContextMenu();
        }

        private void OnLoadedCommand(object obj)
        {
            Grid container = obj as Grid;
            if (container != null)
            {
                container.Drop += (e, c) =>
                {
                    try
                    {
                        string fileName = ((System.Array)c.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                        if (ImportFromFile(fileName))
                        {
                            mainVM.Toast("导入成功", Types.ToastType.Success);
                            ImportProjects();
                            return;
                        }
                    }
                    catch (Exception ec)
                    {
                        LogHelper.Error(ec.ToString());
                        mainVM.Toast("导入失败", Types.ToastType.Failed);
                    }

                };
            }
        }

        private void MainVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mainVM.SelectedGroup))
            {
                Init();
            }
        }


        private void Init()
        {
            HandleGroupInfo();
            ImportProjects();
        }

        private void HandleGroupInfo()
        {
            group = mainVM.SelectedGroup;
            if (group != null)
            {
                Title = group.Name;
            }
            else
            {
                Title = "所有自动化方案";
            }
        }
        private void ImportProjects()
        {
            Projects.Clear();
            var projectsData = group == null ? projects.GetProjects() : projects.GetProjects(group.ID);
            foreach (var project in projectsData)
            {
                string gpName = string.Empty;
                if (group == null)
                {
                    var gp = project.GroupID > 0 ? groupService.GetGroup(project.GroupID) : null;
                    if (gp != null)
                    {
                        gpName = gp.Name;
                    }
                }
                Projects.Add(new Controls.ItemSelect.Models.ItemModel
                {
                    ID = project.ID,
                    Title = project.ProjectName,
                    Description = project.ProjectDescription,
                    Icon = project.Icon,
                    Tag = gpName
                });
            }
        }

        private void CreateItemContextMenu()
        {
            MenuItem delItem = new MenuItem();
            delItem.Header = "删除";
            delItem.Foreground = Colors.GetColor(ColorTypes.Red);
            delItem.Icon = new Icon()
            {
                IconType = IconTypes.Delete,
                Foreground = Colors.GetColor(ColorTypes.Red)
            };
            delItem.Click += (e, c) =>
            {
                DeleteProject(SelectItem);
            };
            moveGroupMenutItem = new MenuItem();
            moveGroupMenutItem.Header = "移动到";
            moveGroupMenutItem.Icon = new Icon()
            {
                IconType = IconTypes.OpenWithMirrored,
            };


            MenuItem exportItem = new MenuItem();
            exportItem.Header = "导出";
            exportItem.Icon = new Icon()
            {
                IconType = IconTypes.Share,
            };
            exportItem.Click += (e, c) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Json (*.project.json)|*.project.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string projectJson = JsonConvert.SerializeObject(projects.GetProject(SelectItem.ID));
                    IOHelper.WriteFile(saveFileDialog.FileName, projectJson);
                    mainVM.Toast("导出完成", Types.ToastType.Success);
                }

            };
            ItemContextMenu.Items.Add(moveGroupMenutItem);
            ItemContextMenu.Items.Add(exportItem);

            ItemContextMenu.Items.Add(new Separator());
            ItemContextMenu.Items.Add(delItem);


        }
        private void CreateContainerContextMenu()
        {
            MenuItem importItem = new MenuItem();
            importItem.Header = "导入";
            importItem.Icon = new Icon()
            {
                IconType = IconTypes.PageCheckedin,
            };
            importItem.Click += (e, c) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Json (*.project.json)|*.project.json";
                if (openFileDialog.ShowDialog() == true)
                {
                    if (ImportFromFile(openFileDialog.FileName))
                    {
                        mainVM.Toast("导入成功", Types.ToastType.Success);
                        ImportProjects();
                        return;
                    }

                }
                mainVM.Toast("导入失败", Types.ToastType.Failed);


            };
            ContainerContextMenu.Items.Add(importItem);
        }
        private void IndexPageVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SelectedProjectID):
                    HandleProjectIDChanged();
                    break;
                case nameof(SelectItem):
                    UpdateContextMenu();
                    break;
            }
        }

        #region 更新右键菜单（移动分组数据更新）
        /// <summary>
        /// 更新右键菜单（移动分组数据更新）
        /// </summary>
        private void UpdateContextMenu()
        {
            moveGroupMenutItem.Items.Clear();
            var project = projects.GetProject(SelectItem.ID);
            foreach (var g in groupService.GetGroups())
            {
                if (g.ID != project.GroupID)
                {
                    var gm = new MenuItem();
                    gm.Click += (e, c) =>
                    {
                        project.GroupID = g.ID;
                        projects.Update(project);
                        if (group != null)
                        {
                            Projects.Remove(SelectItem);
                        }
                        else
                        {
                            int index = Projects.IndexOf(SelectItem);
                            SelectItem.Tag = g.Name;
                            Projects[index] = SelectItem;
                        }
                        mainVM.Toast("移动完成", Types.ToastType.Success);
                    };
                    gm.Icon = new Icon()
                    {
                        IconType = g.Icon
                    };
                    gm.Header = g.Name;
                    moveGroupMenutItem.Items.Add(gm);
                }
            }
        }
        #endregion
        private void HandleProjectIDChanged()
        {
            if (SelectedProjectID > 0)
            {
                mainVM.Data = SelectedProjectID;
                mainVM.Uri = nameof(AddEventPage);
            }
        }

        private void OnRedirectCommand(object obj)
        {
            mainVM.Uri = obj.ToString();
        }

        #region 删除项目
        private void DeleteProject(ItemModel item)
        {
            projects.Delete(item.ID);
            Projects.Remove(item);
            app.Remove(item.ID);
            mainVM.Toast("方案已被删除", Types.ToastType.Success);
        }
        #endregion

        #region 从文件导入方案
        private bool ImportFromFile(string file)
        {
            ProjectModel model = JsonConvert.DeserializeObject<ProjectModel>(IOHelper.ReadFile(file, false));
            if (model != null)
            {
                //创建新的Id
                model.ID = projects.GetCreateID();
                model.ProjectName += model.ID;
                if (group != null)
                {
                    model.GroupID = group.ID;
                }
                else
                {
                    model.GroupID = 0;
                }
                return projects.Add(model);
            }
            return false;
        }
        #endregion
    }
}
