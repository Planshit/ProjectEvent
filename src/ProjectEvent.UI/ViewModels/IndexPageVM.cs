using Newtonsoft.Json;
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
            ImportProjects();
            mainVM.Data = null;
        }

        private void ImportProjects()
        {
            if (!Directory.Exists("Projects"))
            {
                Directory.CreateDirectory("Projects");
            }
            DirectoryInfo folder = new DirectoryInfo("Projects");
            int i = 0;
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                i++;
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                Projects.Add(new Controls.ItemSelect.Models.ItemModel
                {
                    ID = i,
                    Title = project.ProjectName
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
