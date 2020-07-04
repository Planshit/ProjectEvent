using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Helper
{
    public static class ObjectConvert
    {
        /// <summary>
        /// 尝试使用指定对象类型转换匿名对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Get<T>(object obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }
            var result = obj as T;
            if (result == null)
            {
                var jobject = obj as JObject;
                result = jobject.ToObject<T>();
            }
            if (result != null)
            {
                string jstr = JsonConvert.SerializeObject(result);
                result = JsonConvert.DeserializeObject<T>(jstr);
            }
            return result;
        }
    }
}
