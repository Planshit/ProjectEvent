using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Models.DataModels
{
    public class WriteFileActionModel : BaseActionItemModel
    {
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
