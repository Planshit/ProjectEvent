using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public interface IEventContainerService
    {
        /// <summary>
        /// 添加一个事件
        /// </summary>
        /// <param name="eventModel"></param>
        /// <returns>成功返回true，失败返回false</returns>
        bool Add(EventModel eventModel);
        /// <summary>
        /// 添加事件时发生
        /// </summary>
        event ContainerEventHandler OnAddEvent;
        /// <summary>
        /// 移除事件时发生
        /// </summary>
        event ContainerEventHandler OnRemoveEvent;
        /// <summary>
        /// 获取所有事件
        /// </summary>
        /// <returns></returns>
        IEnumerable<EventModel> GetEvents();

    }
}
