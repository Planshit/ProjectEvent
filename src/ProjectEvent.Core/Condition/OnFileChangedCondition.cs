using ProjectEvent.Core.Condition.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Condition
{
    public class OnFileChangedCondition : ICondition
    {
        /// <summary>
        /// 监听的文件夹路径
        /// </summary>
        public string WatchPath { get; set; }
        /// <summary>
        /// 监听的后缀
        /// </summary>
        public string Extname { get; set; }
        public ConditionCheckResultModel Check()
        {
            var result = new ConditionCheckResultModel();
            result.IsValid = Directory.Exists(WatchPath);
            return result;
        }

        public bool IsPass(object data = null)
        {
            var evargs = data as FileChangedDataModel;
            return evargs.WatchPath.ToLower() == WatchPath.ToLower();
        }
    }
}
