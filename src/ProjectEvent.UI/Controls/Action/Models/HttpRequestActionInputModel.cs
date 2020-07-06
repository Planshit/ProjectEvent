using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class HttpRequestActionInputModel : UINotifyPropertyChanged
    {
        private string Url_ = "";
        public string Url { get { return Url_; } set { Url_ = value; } }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, string> QueryParams { get; set; }
    }
}
