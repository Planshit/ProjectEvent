using ProjectEvent.Core.Action.Types;
using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class IFActionConditionData
    {
        public static List<ComBoxModel> ComBoxData = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.Equal,
                DisplayName = "等于"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.UnEqual,
                DisplayName = "不等于"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.Has,
                DisplayName = "包含"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.Miss,
                DisplayName = "不包含"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.Greater,
                DisplayName = "大于"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.GreaterOrEqual,
                DisplayName = "大于或等于"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.Less,
                DisplayName = "小于"
            },
            new ComBoxModel()
            {
                ID = (int)IFActionConditionType.LessOrEqual,
                DisplayName = "小于或等于"
            }
        };
        public static ComBoxModel GetCombox(int id)
        {
            return ComBoxData.Where(m => m.ID == id).FirstOrDefault();
        }
    }
}
