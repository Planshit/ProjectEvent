using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class ActionNameData
    {
        public static Dictionary<ActionType, string> Names = new Dictionary<ActionType, string>()
        {
            {
                ActionType.HttpRequest,"HTTP请求"
            },
            {
                ActionType.IF,"判断"
            },
            {
                ActionType.IFElse,"否则"
            },
            {
                ActionType.IFEnd,"判断结束"
            },
            {
                ActionType.WriteFile,"创建文件"
            },
            {
                ActionType.Shutdown,"关闭电脑"
            },
            {
                ActionType.StartProcess,"启动进程"
            },
        };
    }
}
