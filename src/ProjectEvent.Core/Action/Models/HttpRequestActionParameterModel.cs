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
        public Dictionary<string, string> QueryParams { get; set; }
        public Dictionary<string, string> Files { get; set; }
        public ParamsType ParamsType { get; set; }
        public MethodType Method { get; set; }
        public string JsonStr { get; set; }
    }
}
