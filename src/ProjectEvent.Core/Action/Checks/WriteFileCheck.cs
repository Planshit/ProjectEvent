using Newtonsoft.Json.Linq;
using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Checks
{
    public class WriteFileCheck : ICheck
    {
        private readonly object parameter;
        public WriteFileCheck(object parameter)
        {
            this.parameter = parameter;
        }
        public bool IsCheck()
        {
            var p = ObjectConvert.Get<WriteFileActionParameterModel>(parameter);
            if (p == null)
            {
                return false;
            }
            else if (p.FilePath == string.Empty)
            {
                return false;
            }
            return true;
        }
    }
}
