using ProjectEvent.Core.Action.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public interface IAction
    {
        System.Action GetAction(string[] args);
    }
}
