using ProjectEvent.UI.Base.Color;
using ProjectEvent.UI.Controls.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.ItemSelect.Models
{
    public class ItemModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUri { get; set; }
        public IconTypes Icon { get; set; } = IconTypes.AppIconDefault;
        public bool IsSelected { get; set; }
        public ColorTypes Color { get; set; } = ColorTypes.White;
    }
}
