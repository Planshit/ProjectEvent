using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    public class BaseActionItemModel
    {
        public int ID { get; set; }
        public string ActionName { get; set; }
        public string Icon { get; set; }
        public int Index { get; set; }
        public ActionType ActionType { get; set; }
    }
}
