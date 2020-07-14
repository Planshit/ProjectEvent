using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Event.Structs
{
    public struct KeyboardEventDataStruct
    {
        /// <summary>
        /// 操作up抬起，down按下
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 按键名字符串
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 按键虚拟键码
        /// </summary>
        public int KeyCode { get; set; }
    }
}
