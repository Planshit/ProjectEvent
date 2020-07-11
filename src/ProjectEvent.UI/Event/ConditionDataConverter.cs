using ProjectEvent.Core.Event.Types;
using ProjectEvent.Core.Helper;
using ProjectEvent.UI.Models.ConditionModels;
using ProjectEvent.UI.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Event
{
    /// <summary>
    /// 条件数据转换器
    /// 用于将项目保存的条件数据json格式转换为可识别的条件模型
    /// </summary>
    public class ConditionDataConverter
    {
        public static object GetCreateConditionData(EventType type)
        {
            object res = null;
            switch (type)
            {
                case EventType.OnIntervalTimer:
                    //循环计时
                    res = new IntervalTimerConditionModel();
                    break;
                case EventType.OnProcessCreated:
                    //进程创建
                    res = new ProcessCreatedConditionModel();
                    break;
            }
            return res;
        }

        public static object GetObj(ProjectModel project)
        {
            var res = project.ConditionData;
            switch ((EventType)project.EventID)
            {
                case EventType.OnProcessCreated:
                    res = ObjectConvert.Get<ProcessCreatedConditionModel>(project.ConditionData);
                    break;
                case EventType.OnIntervalTimer:
                    res = ObjectConvert.Get<IntervalTimerConditionModel>(project.ConditionData);
                    break;
            }
            return res;
        }
    }
}
