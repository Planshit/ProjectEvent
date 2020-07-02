using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public delegate void EventHandler(EventModel @event, bool isSuccess);
}
