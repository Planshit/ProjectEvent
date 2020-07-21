using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Types
{
    public enum ToastScenarioType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 提醒通知。 这类通知将以预先展开的状态显示在用户屏幕上并一直显示，直至消失。
        /// </summary>
        Reminder,
        /// <summary>
        /// 警报通知。 这类通知将以预先展开的状态显示在用户屏幕上并一直显示，直至消失。 音频在默认情况下将循环播放，并将使用警报音频。
        /// </summary>
        Alarm,
        /// <summary>
        /// 来电通知。 这将以特殊通话格式以预先展开的形式显示在用户屏幕上并一直显示，直至消失。 音频在默认情况下将循环播放，并将使用铃声音频。
        /// </summary>
        IncomingCall
    }
}
