using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;

namespace ProjectEvent.Core.Services
{
    public class TimerService : ITimerService, IDisposable
    {
        private Dictionary<int, Timer> _timers;

        public TimerService()
        {
            _timers = new Dictionary<int, Timer>();
        }

        public void StartNew(System.Action action, DateTime dateTime, bool autoReset = false)
        {
            //转换时间
            var trueTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

            if (autoReset)
            {
                //每天
                trueTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, dateTime.Hour, dateTime.Minute, 0);
                StartNew(action, DateTime.Now.Subtract(trueTime).Seconds, false, () =>
                  {
                      //进行下一个周期
                      StartNew(action, dateTime, autoReset);
                  });
            }
            else
            {
                //指定日期
                if (DateTime.Now < trueTime)
                {
                    var s = trueTime.Subtract(DateTime.Now).TotalSeconds;
                    //可以执行
                    StartNew(action, s);
                }
                else
                {
                    //已经超过指定日期不再执行
                    Debug.WriteLine("指定的日期已超过，无法再执行。");
                }
            }

        }



        public void StartNew(System.Action action, double seconds, bool autoReset = false, System.Action timerClosedAction = null)
        {
            int id = _timers.Count + 1;

            var timer = new Timer();
            timer.Interval = seconds * 1000;
            Debug.WriteLine("任务在：" + timer.Interval + "秒后执行");
            timer.Elapsed += (e, c) =>
            {
                if (!autoReset)
                {
                    timer.Stop();
                    timer.Close();
                    _timers.Remove(id);
                }
                action.Invoke();
                timerClosedAction?.Invoke();
            };

            _timers.Add(id, timer);
            timer.Start();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    foreach (var timer in _timers.Values)
                    {
                        timer.Stop();
                        timer.Close();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                _timers = null;

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~TimerService()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
