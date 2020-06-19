using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Net.Models
{
    public class HttpResultModel
    {
        public bool IsSuccess { get; set; } = false;
        public string Content { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 0;
    }
}
