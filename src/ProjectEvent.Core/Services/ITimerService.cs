using ProjectEvent.Core.Condition.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public interface ITimerService
    {
        /// <summary>
        /// 启动一个周期任务
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <param name="dateTime">指定时间</param>
        /// <param name="repetitionType">重复方式</param>
        void StartNew(int id, System.Action action, DateTime dateTime, TimeChangedRepetitionType repetitionType);
        /// <summary>
        /// 启动一个定时器任务
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <param name="seconds">多少秒后执行</param>
        /// <param name="num">循环次数（为0时无限）</param>
        /// <param name="timerClosedAction">计时器关闭后执行方法</param>
        void StartNew(int id, System.Action action, double seconds, int num = 0, System.Action timerClosedAction = null);

        /// <summary>
        /// 关闭一个event任务
        /// </summary>
        /// <param name="id"></param>
        void Close(int id);
    }
}
