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
        private Dictionary<int, int> _timerRunCount;
        public TimerService()
        {
            _timers = new Dictionary<int, Timer>();
            _timerRunCount = new Dictionary<int, int>();
        }

        public void StartNew(int id, System.Action action, DateTime dateTime, bool autoReset = false)
        {
            //转换时间
            var trueTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

            if (autoReset)
            {
                //每天
                trueTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, dateTime.Hour, dateTime.Minute, 0);
                StartNew(id, action, trueTime.Subtract(DateTime.Now).TotalSeconds, 1, () =>
                   {
                       //进行下一个周期
                       StartNew(id, action, dateTime, autoReset);
                   });
            }
            else
            {
                //指定日期
                if (DateTime.Now < trueTime)
                {
                    var s = trueTime.Subtract(DateTime.Now).TotalSeconds;
                    //可以执行
                    StartNew(id, action, s, 1);
                }
                else
                {
                    //已经超过指定日期不再执行
                    Debug.WriteLine("指定的日期已超过，无法再执行。");
                }
            }

        }



        public void StartNew(int id, System.Action action, double seconds, int num = 0, System.Action timerClosedAction = null)
        {

            if (_timers.ContainsKey(id))
            {
                return;
            }
            var timer = new Timer();
            timer.Interval = seconds * 1000;

            Debug.WriteLine("任务在：" + seconds + "秒后执行");
            timer.Elapsed += (e, c) =>
            {
                action?.Invoke();
                timerClosedAction?.Invoke();
                SetRunCount(id);

                int nextCount = GetRunCount(id) + 1;
                if (num > 0 && nextCount > num)
                {
                    Close(id);
                }
            };

            _timerRunCount.Add(id, 0);
            _timers.Add(id, timer);

            timer.Start();
        }

        public void Close(int id)
        {
            if (_timers.ContainsKey(id))
            {
                var timer = _timers[id];
                timer.Stop();
                timer.Dispose();
                _timers.Remove(id);
            }
            if (_timerRunCount.ContainsKey(id))
            {
                _timerRunCount.Remove(id);
            }
        }

        /// <summary>
        /// 获取timer运行次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetRunCount(int id)
        {
            int count = 0;

            if (_timerRunCount.ContainsKey(id))
            {
                return _timerRunCount[id];
            }
            return count;
        }

        /// <summary>
        /// 设置timer运行次数
        /// </summary>
        /// <param name="id">timer id</param>
        /// <param name="count">次数，默认为0，叠加一次</param>
        private void SetRunCount(int id, int count = 0)
        {
            if (_timerRunCount.ContainsKey(id))
            {
                if (count == 0)
                {
                    count = _timerRunCount[id] + 1;
                }
                _timerRunCount[id] = count;
            }
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
