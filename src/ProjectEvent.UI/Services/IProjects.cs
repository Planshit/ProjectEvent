using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public interface IProjects
    {
        void LoadProjects();
        bool Add(ProjectModel project, bool isSave = true);
        void Update(ProjectModel project);
        void Delete(int ID);
        List<ProjectModel> GetProjects();
        ProjectModel GetProject(int ID);
        List<ProjectModel> GetProjects(int GID);
        int GetCreateID();
    }
}
