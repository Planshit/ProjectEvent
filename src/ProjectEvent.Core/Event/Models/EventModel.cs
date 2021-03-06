﻿using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Event.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Models
{
    public class EventModel
    {
        public int ID { get; set; }
        public bool IsEnabled { get; set; }
        public string Name { get; set; }
        public EventType EventType { get; set; }
        public ICondition Condition { get; set; }

        public List<ActionModel> Actions { get; set; }

        public EventModel Copy()
        {
            return new EventModel()
            {
                ID = ID,
                Actions = Actions,
                Condition = Condition,
                EventType = EventType,
                IsEnabled = IsEnabled,
                Name = Name
            };
        }
    }
}
