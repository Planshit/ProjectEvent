using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Net.Types;
using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class WriteFileActionData
    {
        public static List<ComBoxModel> ActionResults = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)CommonResultKeyType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
          

        };



    }
}
