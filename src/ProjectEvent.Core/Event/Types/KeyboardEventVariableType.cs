using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Types
{
    public enum KeyboardEventVariableType
    {
        /// <summary>
        /// 动作：up释放按键，down按下按键
        /// </summary>
        Action,
        /// <summary>
        /// 键名
        /// </summary>
        KeyName,
        /// <summary>
        /// 键码
        /// </summary>
        KeyCode
    }
}
