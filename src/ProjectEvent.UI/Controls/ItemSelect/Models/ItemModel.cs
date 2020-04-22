using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.ItemSelect.Models
{
    public class ItemModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUri { get; set; }
        public string Icon { get; set; }
        public bool IsSelected { get; set; }
    }
}
