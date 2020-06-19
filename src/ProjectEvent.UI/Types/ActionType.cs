using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Types
{
    /// <summary>
    /// 用于UI的ActionType
    /// </summary>
    public enum ActionType
    {
        WriteFile = 1,
        IF = 2,
        IFElse = 3,
        IFEnd = 4,
        HttpGet = 5
    }
}
