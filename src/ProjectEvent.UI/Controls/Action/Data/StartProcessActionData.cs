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
    public class StartProcessActionData
    {
        public static List<ComBoxModel> ActionResults = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)StartProcessResultType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Handle,
                DisplayName = "句柄（仅成功时有效）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Id,
                DisplayName = "进程ID（仅成功时有效）"
            },

        };



    }
}
