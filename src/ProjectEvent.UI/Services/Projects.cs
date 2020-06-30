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
        private List<ProjectModel> projectsBackup;
        public Projects()
        {
            projects = new List<ProjectModel>();
        }
        private void UpdateBackup()
        {
            projectsBackup = JsonConvert.DeserializeObject<List<ProjectModel>>(JsonConvert.SerializeObject(projects));
        }
        public void LoadProjects()
        {
            DirectoryInfo folder = IOHelper.GetDirectoryInfo("Projects");
            foreach (FileInfo file in folder.GetFiles("*.project.json"))
            {
                var project = JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file.FullName));
                Add(project, false);
            }
            UpdateBackup();
        }
        public bool Add(ProjectModel project, bool isSave = true)
        {
            if (project.ID == null || project.ID <= 0)
            {
                project.ID = GetCreateID();
            }
            if (project != null && project.ID != null && !projects.Where(m => m.ID == project.ID).Any() && !projects.Where(m => m.ProjectName == project.ProjectName).Any())
            {
                if (isSave)
                {
                    IOHelper.WriteFile($"Projects\\{project.ProjectName}.project.json", JsonConvert.SerializeObject(project));
                }
                projects.Add(project);
                UpdateBackup();
                return true;
            }
            return false;
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
            UpdateBackup();
        }

        public ProjectModel GetProject(int ID)
        {
            return projects.Where(m => m.ID == ID).ToList().FirstOrDefault();
        }

        public List<ProjectModel> GetProjects()
        {
            return projects;
        }

        public void Update(ProjectModel project)
        {
            var oldproject = projectsBackup.Where(m => m.ID == project.ID).FirstOrDefault();
            //更新本地数据
            if (oldproject.ProjectName != project.ProjectName)
            {
                string oldpath = $"Projects\\{oldproject.ProjectName}.project.json";
                if (IOHelper.FileExists(oldpath))
                {
                    IOHelper.FileDelete(oldpath);
                }
            }
            IOHelper.WriteFile($"Projects\\{project.ProjectName}.project.json", JsonConvert.SerializeObject(project));
            UpdateBackup();
        }

        public List<ProjectModel> GetProjects(int GID)
        {
            return projects.Where(m => m.GroupID == GID).ToList();
        }

        public int GetCreateID()
        {
            return projects.Count > 0 ? projects.Max(m => m.ID) + 1 : 1;
        }
    }
}
