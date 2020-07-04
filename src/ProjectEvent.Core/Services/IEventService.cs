using ProjectEvent.Core.Event.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Services
{
    public interface IEventService
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
        /// 更新事件时发生
        /// </summary>
        event EventChangedHandler OnUpdateEvent;

        /// <summary>
        /// 获取所有事件
        /// </summary>
        /// <returns></returns>
        IEnumerable<EventModel> GetEvents();
        /// <summary>
        /// 事件被触发时发生
        /// </summary>

        event EventHandler OnEventTrigger;
        /// <summary>
        /// 尝试触发事件
        /// </summary>
        /// <param name="ev">事件模型</param>
        /// <param name="data">事件数据</param>
        void Invoke(EventModel ev, object data);
        /// <summary>
        /// 移除一个event
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="ev"></param>
        void Update(EventModel ev);
    }
}
