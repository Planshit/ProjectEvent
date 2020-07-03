using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public interface IApp
    {
        void Run();
        void Add(ProjectModel project);
        void Update(ProjectModel project);
        void Remove(int id);
    }
}
