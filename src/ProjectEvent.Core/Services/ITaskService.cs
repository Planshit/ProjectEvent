using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public interface ITaskService
    {
        void Run();
        /// <summary>
        /// 事件触发
        /// </summary>

        event EventHandler OnEventTrigger;
    }
}
