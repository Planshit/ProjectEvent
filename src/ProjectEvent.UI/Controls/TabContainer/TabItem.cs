using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProjectEvent.UI.Controls.TabContainer
{
    public class TabItem : ContentControl
    {
        public string Title { get; set; }
        public TabItem()
        {
            DefaultStyleKey = typeof(TabItem);
        }
    }
}
