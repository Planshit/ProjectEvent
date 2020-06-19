using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class HttpGetActionInputModel : UINotifyPropertyChanged
    {
        private string Url_ = "";
        public string Url { get { return Url_; } set { Url_ = value; } }
       
    }
}
