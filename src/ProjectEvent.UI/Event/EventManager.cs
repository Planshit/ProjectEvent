using ProjectEvent.Core.Condition;
using ProjectEvent.Core.Condition.Types;
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
    public class EventManager
    {
        public static Core.Event.Models.EventModel CreateEventModel(ProjectModel project)
        {
            ICondition condition = null;
            switch ((EventType)project.EventID)
            {
                case EventType.OnDeviceStartup:
                    condition = new OnDeviceStartupCondition();
                    break;
                case EventType.OnIntervalTimer:
                    var ttimerconditionData = ObjectConvert.Get<IntervalTimerConditionModel>(project.ConditionData);
                    if (ttimerconditionData != null)
                    {
                        condition = new OnIntervalTimerCondition()
                        {
                            Num = int.Parse(ttimerconditionData.Num),
                            Seconds = int.Parse(ttimerconditionData.Second)
                        };
                    }
                    break;
                case EventType.OnProcessCreated:
                    var pcconditionData = ObjectConvert.Get<ProcessCreatedConditionModel>(project.ConditionData);
                    condition = new OnProcessCreatedCondition()
                    {
                        ProcessName = pcconditionData.ProcessName,
                        Caseinsensitive = pcconditionData.Caseinsensitive,
                        FuzzyMatch = pcconditionData.FuzzyMatch
                    };
                    break;
                case EventType.OnFileChanged:
                    var fcconditionData = ObjectConvert.Get<FileChangedConditionModel>(project.ConditionData);
                    condition = new OnFileChangedCondition()
                    {
                        WatchPath = fcconditionData.WatchPath,
                        Extname = fcconditionData.Extname
                    };
                    break;
                case EventType.OnTimeChanged:
                    var tcconditionData = ObjectConvert.Get<TimeChangedConditionModel>(project.ConditionData);
                    condition = new OnTimeChangedCondition()
                    {
                        Time = tcconditionData.Time,
                        RepetitionType = (TimeChangedRepetitionType)tcconditionData.RepetitionType.ID
                    };
                    break;
                case EventType.KeyboardEvent:
                    condition = new KeyboardEventCondition();
                    break;
            }
            return new Core.Event.Models.EventModel()
            {
                ID = project.ID,
                EventType = (EventType)project.EventID,
                Condition = condition,
                Actions = project.Actions
            };
        }

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
                case EventType.OnFileChanged:
                    //文件变化
                    res = new FileChangedConditionModel();
                    break;
                case EventType.OnTimeChanged:
                    //进程创建
                    res = new TimeChangedConditionModel();
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
                case EventType.OnFileChanged:
                    res = ObjectConvert.Get<FileChangedConditionModel>(project.ConditionData);
                    break;
                case EventType.OnTimeChanged:
                    res = ObjectConvert.Get<TimeChangedConditionModel>(project.ConditionData);
                    break;
            }
            return res;
        }
    }
}
