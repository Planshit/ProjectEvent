using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
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
using System.Windows.Controls;

namespace ProjectEvent.UI.ViewModels
{
    public class IndexPageVM : IndexPageModel
    {
        private readonly MainViewModel mainVM;
        private readonly IProjects projects;
        private readonly IGroup groupService;
        public Command RedirectCommand { get; set; }
        public ContextMenu ItemContextMenu { get; set; }
        private GroupModel group;
        private MenuItem moveGroupMenutItem;
        public IndexPageVM(
            MainViewModel mainVM,
            IProjects projects,
            IGroup groupService)
        {
            this.mainVM = mainVM;
            this.projects = projects;
            this.groupService = groupService;

            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            PropertyChanged += IndexPageVM_PropertyChanged;
            Projects = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            Projects.CollectionChanged += Projects_CollectionChanged;
            mainVM.PropertyChanged += MainVM_PropertyChanged;
            mainVM.IsShowNavigation = true;
            mainVM.IsShowTitleBar = false;

            ItemContextMenu = new ContextMenu();
            Init();
            CreateItemContextMenu();
        }

        private void MainVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mainVM.SelectedGroup))
            {
                Init();
            }
        }

        private void Projects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    var data = item as ItemModel;
                    string project = $"Projects\\{data.Title}.project.json";
                    IOHelper.FileDelete(project);
                }

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
                Projects.Add(new Controls.ItemSelect.Models.ItemModel
                {
                    ID = project.ID,
                    Title = project.ProjectName,
                    Description = project.ProjectDescription
                });
            }
        }

        private void CreateItemContextMenu()
        {
            MenuItem del = new MenuItem();
            del.Header = "删除";
            del.Click += (e, c) =>
            {
                projects.Delete(SelectItem.ID);
                Projects.Remove(SelectItem);
                mainVM.Toast("方案已被删除", Types.ToastType.Success);
            };
            moveGroupMenutItem = new MenuItem();
            moveGroupMenutItem.Header = "移动到";

            ItemContextMenu.Items.Add(del);
            ItemContextMenu.Items.Add(moveGroupMenutItem);

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
                    };
                    gm.Header = g.Name;
                    moveGroupMenutItem.Items.Add(gm);
                }
            }
        }

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
    }
}
