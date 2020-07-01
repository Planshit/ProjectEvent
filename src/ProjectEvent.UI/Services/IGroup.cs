using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Services
{
    public interface IGroup
    {
        void Load();
        GroupModel Add(GroupModel group);
        void Delete(int gid);
        void Update(GroupModel group);
        List<GroupModel> GetGroups();
        GroupModel GetGroup(int id);
    }
}
