using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class DownloadFileActionInputModel
    {
        public string Url { get; set; }

        public string SavePath { get; set; }
        public Dictionary<string, string> Headers { get; set; }

    }
}
