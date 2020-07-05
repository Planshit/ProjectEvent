using ProjectEvent.Core.Net.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Models
{
    public class HttpRequestActionParameterModel
    {
        public string Url { get; set; }
        public int Timeout { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Data { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public PostType PostType { get; set; }
    }
}
