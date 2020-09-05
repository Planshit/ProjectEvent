using ProjectEvent.UI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.UI.Controls.Action.Models
{
    public class HttpRequestActionInputModel
    {
        public string Url { get; set; }
        /// <summary>
        /// 方法
        /// </summary>
        public ComBoxModel Method{ get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public Dictionary<string, string> QueryParams { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public ComBoxModel PamramsType { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public Dictionary<string, string> Files { get; set; }
        /// <summary>
        /// 请求头信息
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
        public string JsonStr { get; set; }

    }
}
