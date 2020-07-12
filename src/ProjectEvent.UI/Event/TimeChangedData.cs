using ProjectEvent.Core.Condition.Types;
using ProjectEvent.UI.Controls.Action.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Event
{
    public static class TimeChangedData
    {
        public static List<ComBoxModel> RepetitionTypes = new List<Controls.Action.Models.ComBoxModel>()
                        {
                         new Controls.Action.Models.ComBoxModel()
                         {
                             ID=(int)TimeChangedRepetitionType.None,
                             DisplayName="不重复"
                         },
                          new Controls.Action.Models.ComBoxModel()
                         {
                             ID=(int)TimeChangedRepetitionType.Day,
                             DisplayName="每天"
                         },
                           new Controls.Action.Models.ComBoxModel()
                         {
                             ID=(int)TimeChangedRepetitionType.Week,
                             DisplayName="每周"
                         }
                        };
    }
}
