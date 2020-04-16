using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public interface ITimerService
    {
        /// <summary>
        /// 启动一个定时器任务
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <param name="dateTime">指定时间</param>
        /// <param name="autoReset">是否重复执行（当true时表示每天的dateTime时分执行）</param>
        void StartNew(System.Action action, DateTime dateTime, bool autoReset = false);
        /// <summary>
        /// 启动一个定时器任务
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <param name="seconds">多少秒后执行</param>
        /// <param name="autoReset">是否重复执行</param>
        /// <param name="timerClosedAction">计时器关闭后执行方法</param>
        void StartNew(System.Action action, double seconds, bool autoReset = false, System.Action timerClosedAction = null);
    }
}
