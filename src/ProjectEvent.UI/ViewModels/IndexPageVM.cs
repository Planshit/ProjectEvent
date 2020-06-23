using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Controls.ItemSelect.Models;
using ProjectEvent.UI.Models;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.ViewModels
{
    public class IndexPageVM : IndexPageModel
    {
        private readonly MainViewModel mainVM;
        public Command RedirectCommand { get; set; }

        public IndexPageVM(MainViewModel mainVM)
        {
            this.mainVM = mainVM;
            RedirectCommand = new Command(new Action<object>(OnRedirectCommand));
            PropertyChanged += IndexPageVM_PropertyChanged;
            Projects = new System.Collections.ObjectModel.ObservableCollection<Controls.ItemSelect.Models.ItemModel>();
            Projects.CollectionChanged += Projects_CollectionChanged;
            ImportProjects();
            mainVM.Data = null;
            NavigationItems = new System.Collections.ObjectModel.ObservableCollection<Controls.Navigation.Models.NavigationItemModel>()
            {

                new Controls.Navigation.Models.NavigationItemModel()
                {
                    BadgeText = "1",
                Icon = Controls.Base.IconTypes.Timer,
                Title = "Test",

                }
            };
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

        private void ImportProjects()
        {
            DirectoryInfo folder = IOHelper.GetDirectoryInfo("Projects");
            int i = 0;
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                i++;
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                Projects.Add(new Controls.ItemSelect.Models.ItemModel
                {
                    ID = i,
                    Title = project.ProjectName,
                    Description=project.ProjectDescription
                });
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
