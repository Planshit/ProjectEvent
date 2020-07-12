using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectEvent.Core.Condition.Models
{
    public class FileChangedDataModel
    {
        public string WatchPath { get; set; }
        public FileSystemEventArgs FileSystemEventArgs { get; set; }
    }
}
