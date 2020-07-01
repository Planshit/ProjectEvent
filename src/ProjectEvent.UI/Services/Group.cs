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
    public class Group : IGroup
    {
        private List<GroupModel> groups;
        private List<GroupModel> groupsBackup;
        private const string path = "Data\\Groups.json";

        public Group()
        {
            groups = new List<GroupModel>();
        }

        private void UpdateBackup()
        {
            groupsBackup = JsonConvert.DeserializeObject<List<GroupModel>>(JsonConvert.SerializeObject(groups));
        }
        private void Save()
        {
            if (groups != null)
            {
                IOHelper.WriteFile(path, JsonConvert.SerializeObject(groups));
            }
        }
        public GroupModel Add(GroupModel group)
        {
            if (group.ID == null || group.ID <= 0)
            {
                group.ID = GetCreateID();
            }
            groups.Add(group);
            UpdateBackup();
            Save();
            return group;
        }
        private int GetCreateID()
        {
            return groups.Count == 0 ? 100 : groups.Max(m => m.ID) + 1;
        }
        public void Delete(int gid)
        {
            var group = groups.Where(m => m.ID == gid).FirstOrDefault();
            if (group != null)
            {
                groups.Remove(group);
                UpdateBackup();
                Save();
            }
        }

        public List<GroupModel> GetGroups()
        {
            return groupsBackup;
        }

        public void Load()
        {
            //分组数据路径
            IOHelper.CreateDirectory("Data");

            if (IOHelper.FileExists(path))
            {
                var groups_ = JsonConvert.DeserializeObject<List<GroupModel>>(IOHelper.ReadFile(path));
                if (groups_ != null)
                {
                    groups = groups_;
                }
            }
            UpdateBackup();
        }

        public void Update(GroupModel group)
        {
            var g = groups.Where(m => m.ID == group.ID).FirstOrDefault();
            if (g != null)
            {
                groups[groups.IndexOf(g)] = group;
                UpdateBackup();
                Save();
            }
        }

        public GroupModel GetGroup(int id)
        {
            return groupsBackup.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
