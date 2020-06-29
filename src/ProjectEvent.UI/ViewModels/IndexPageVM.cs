using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Controls.Navigation;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class IndexPageVM : IndexPageModel
    {
        private readonly MainViewModel mainVM;
        public Command RedirectCommand { get; set; }
        private GroupModel group;
        public IndexPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;

            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            PropertyChanged += IndexPageVM_PropertyChanged;
            Projects = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            Projects.CollectionChanged += Projects_CollectionChanged;
            mainVM.PropertyChanged += MainVM_PropertyChanged;
            mainVM.IsShowNavigation = true;
            mainVM.IsShowTitleBar = false;

            Init();
        }

        private void MainVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(mainVM.Data))
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

            group = mainVM.Data as GroupModel;
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
            DirectoryInfo folder = IOHelper.GetDirectoryInfo("Projects");
            int i = 0;
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                i++;
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                if (group == null || group.ID == project.GroupID)
                {
                    Projects.Add(new Controls.ItemSelect.Models.ItemModel
                    {
                        ID = i,
                        Title = project.ProjectName,
                        Description = project.ProjectDescription
                    });
                }
            }
        }
        private void IndexPageVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedProjectID":
                    HandleProjectIDChanged();
                    break;
            }
        }

        private void HandleProjectIDChanged()
        {
            if (SelectedProjectID > 0)
            {
                mainVM.Data = Projects.Where(m => m.ID == SelectedProjectID).FirstOrDefault().Title;
                mainVM.Uri = "AddEventPage";
            }
        }

        private void OnRedirectCommand(object obj)
        {
            mainVM.Uri = obj.ToString();
        }
    }
}
