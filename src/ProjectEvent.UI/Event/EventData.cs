using ProjectEvent.Core.Condition.Types;
using ProjectEvent.Core.Event.Types;
using ProjectEvent.UI.Controls.Input;
using ProjectEvent.UI.Controls.InputGroup.Models;
using ProjectEvent.UI.Controls.Toggle;
using ProjectEvent.UI.Models.ConditionModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProjectEvent.UI.Event
{
    public static class EventData
    {
        ///// <summary>
        ///// 事件条件输入模型
        ///// </summary>
        //public static Dictionary<EventType, List<InputModel>> InputModels = new Dictionary<EventType, List<InputModel>>()
        //{
        //    //计时器事件
        //    {EventType.OnIntervalTimer,},
        //    //进程创建事件
        //    {EventType.OnProcessCreated,new List<InputModel>()
        //    {
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Text,
        //                BindingName = "ProcessName",
        //                BindingProperty = TextBox.TextProperty,
        //                Title = "进程名"
        //            },
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Bool,
        //                BindingName = "Caseinsensitive",
        //                BindingProperty = Toggle.IsCheckedProperty,
        //                Title = "忽略大小写"
        //            },
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Bool,
        //                BindingName = "FuzzyMatch",
        //                BindingProperty = Toggle.IsCheckedProperty,
        //                Title = "模糊匹配"
        //            }
        //    }},
        //     //文件更改事件
        //    {EventType.OnFileChanged,new List<InputModel>()
        //    {
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Text,
        //                BindingName = nameof(FileChangedConditionModel.WatchPath),
        //                BindingProperty = TextBox.TextProperty,
        //                Title = "监听文件夹路径",
        //            },
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Text,
        //                BindingName = nameof(FileChangedConditionModel.Extname),
        //                BindingProperty = TextBox.TextProperty,
        //                Title = "过滤文件扩展名",
        //            },
        //    }},
        //     //周期事件
        //    {EventType.OnTimeChanged,new List<InputModel>()
        //    {
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.DateTime,
        //                BindingName = nameof(TimeChangedConditionModel.Time),
        //                BindingProperty = InputBox.SelectedDateTimeProperty,
        //                Title = "选择时间",
        //            },
        //        new InputModel()
        //            {
        //                Type = Controls.InputGroup.InputType.Select,
        //                BindingName = nameof(TimeChangedConditionModel.RepetitionType),
        //                BindingProperty = ComboBox.SelectedValueProperty,
        //                Title = "重复行为",
        //                SelectItems=TimeChangedData.RepetitionTypes
        //            },
        //    }}
        //};


        public static List<InputModel> GetInputModels(EventType type)
        {
            List<InputModel> res = null;
            switch (type)
            {
                case EventType.OnIntervalTimer:
                    res = new List<InputModel>()
            {
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Number,
                        BindingName = "Second",
                        BindingProperty = TextBox.TextProperty,
                        Title = "间隔秒数"
                    },
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Number,
                        BindingName = "Num",
                        BindingProperty = TextBox.TextProperty,
                        Title = "循环次数（0时永远）"
                    }
            };
                    break;
                case EventType.OnProcessCreated:
                    res = new List<InputModel>()
            {
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = "ProcessName",
                        BindingProperty = TextBox.TextProperty,
                        Title = "进程名"
                    },
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Bool,
                        BindingName = "Caseinsensitive",
                        BindingProperty = Toggle.IsCheckedProperty,
                        Title = "忽略大小写"
                    },
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Bool,
                        BindingName = "FuzzyMatch",
                        BindingProperty = Toggle.IsCheckedProperty,
                        Title = "模糊匹配"
                    }
            };
                    break;
                case EventType.OnFileChanged:
                    new List<InputModel>()
            {
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = nameof(FileChangedConditionModel.WatchPath),
                        BindingProperty = TextBox.TextProperty,
                        Title = "监听文件夹路径",
                    },
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Text,
                        BindingName = nameof(FileChangedConditionModel.Extname),
                        BindingProperty = TextBox.TextProperty,
                        Title = "过滤文件扩展名",
                    },
            };
                    break;
                case EventType.OnTimeChanged:
                    res = new List<InputModel>()
            {
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.DateTime,
                        BindingName = nameof(TimeChangedConditionModel.Time),
                        BindingProperty = InputBox.SelectedDateTimeProperty,
                        Title = "选择时间",
                    },
                new InputModel()
                    {
                        Type = Controls.InputGroup.InputType.Select,
                        BindingName = nameof(TimeChangedConditionModel.RepetitionType),
                        BindingProperty = ComboBox.SelectedValueProperty,
                        Title = "重复行为",
                        SelectItems=TimeChangedData.RepetitionTypes
                    },
            };
                    break;
            }


            return res;
        }
        public static List<Controls.ItemSelect.Models.ItemModel> Events = new List<Controls.ItemSelect.Models.ItemModel>()
        {
            new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnTimeChanged,
                Title = "周期事件",
                Icon = Controls.Base.IconTypes.DateTime,
                Description = "指定日期或每天/每周触发"
            },
            new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnIntervalTimer,
                Title = "计时器",
                Icon = Controls.Base.IconTypes.Timer,
                Description = "每隔多少秒触发"
            },
            new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnDeviceStartup,
                Title = "设备启动",
                Description = "电脑首次开机或注销后重新登录时触发",
                Icon = Controls.Base.IconTypes.DeviceRun
            },
            new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnProcessCreated,
                Title = "进程创建",
                Description = "当有新的程序首次运行时触发",
                Icon = Controls.Base.IconTypes.ProcessingRun

            },
            new Controls.ItemSelect.Models.ItemModel()
            {
                ID = (int)EventType.OnFileChanged,
                Title = "文件/目录变化",
                Description = "当目录中发生文件/目录创建、修改、删除时触发",
                Icon = Controls.Base.IconTypes.FabricSyncFolder

            }
        };
    }
}
