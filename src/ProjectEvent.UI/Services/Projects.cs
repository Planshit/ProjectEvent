using Newtonsoft.Json;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public class Projects : IProjects
    {
        private List<ProjectModel> projects;
        public Projects()
        {
            projects = new List<ProjectModel>();
        }
        public void LoadProjects()
        {
            DirectoryInfo folder = IOHelper.GetDirectoryInfo("Projects");
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                Add(project, false);
            }
        }
        public void Add(ProjectModel project, bool isSave = true)
        {
            if (project != null && project.ID != null && project.ID > 0 && !projects.Where(m => m.ID == project.ID).Any() && !projects.Where(m => m.ProjectName == project.ProjectName).Any())
            {
                projects.Add(project);
                if (isSave)
                {
                    IOHelper.WriteFile($"Projects\\{project.ProjectName}.project.json", JsonConvert.SerializeObject(project));
                }
            }
        }

        public void Delete(int ID)
        {
            var project = projects.Where(m => m.ID == ID).FirstOrDefault();
            projects.Remove(project);
            string oldpath = $"Projects\\{project.ProjectName}.project.json";
            if (IOHelper.FileExists(oldpath))
            {
                IOHelper.FileDelete(oldpath);
            }
        }

        public ProjectModel GetProject(int ID)
        {
            return projects.Where(m => m.ID == ID).FirstOrDefault();
        }

        public List<ProjectModel> GetProjects()
        {
            return projects;
        }

        public void Update(ProjectModel project)
        {
            var oldproject = projects.Where(m => m.ID == project.ID).FirstOrDefault();
            int index = projects.IndexOf(oldproject);
            projects[index] = project;
            //更新本地数据
            string oldpath = $"Projects\\{oldproject.ProjectName}.project.json";
            if (IOHelper.FileExists(oldpath))
            {
                IOHelper.FileDelete(oldpath);
            }
            IOHelper.WriteFile($"Projects\\{project.ProjectName}.project.json", JsonConvert.SerializeObject(project));
        }

        public List<ProjectModel> GetProjects(int GID)
        {
            return projects.Where(m => m.GroupID == GID).ToList();
        }

    }
}
