using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Net.Types;
using ProjectEvent.Core.Types;
using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class GetIPAddressActionData
    {
        public static List<ComBoxModel> IPAddressTypes = new List<ComBoxModel>()
                {
                    new ComBoxModel()
                    {
                        ID=(int)IPAddressType.LocalIPV4,
                        DisplayName="内网IPV4"
                    },
                    new ComBoxModel()
                    {
                        ID=(int)IPAddressType.PublicIPV4,
                        DisplayName="公网IPV4"
                    },
                };
        public static ComBoxModel GetIPAddressType(int id)
        {
            return IPAddressTypes.Where(m => m.ID == id).FirstOrDefault();
        }

    }
}
