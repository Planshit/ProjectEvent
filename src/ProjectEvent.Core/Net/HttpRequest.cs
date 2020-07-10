using Newtonsoft.Json;
using ProjectEvent.Core.Net.Models;
using ProjectEvent.Core.Net.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Net
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class HttpRequest
    {
        private readonly System.Net.Http.HttpClient httpClient;
        /// <summary>
        /// 获取或设置请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 获取或设置请求超时时间（单位秒，默认60）
        /// </summary>
        public int Timeout { get; set; } = 60;
        /// <summary>
        /// 设置表单数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 设置上传文件路径
        /// </summary>
        public Dictionary<string, string> Files { get; set; }
        /// <summary>
        /// 请求头信息
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// POST数据类型
        /// </summary>
        public ParamsType ParamsType { get; set; } = ParamsType.Json;
        public HttpRequest()
        {
            httpClient = new System.Net.Http.HttpClient();

        }
        /// <summary>
        /// 发送一个GET请求并获取返回内容
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResultModel> GetAsync()
        {
            HandleConfig();
            var response = await httpClient.GetAsync(Url);
            var content = await response.Content.ReadAsStringAsync();
            var result = new HttpResultModel()
            {
                IsSuccess = true,
                Content = content,
                StatusCode = (int)response.StatusCode
            };
            return result;
        }


        public async Task<HttpResultModel> PostAsync()
        {
            HandleConfig();
            HttpContent httpContent = null;
            if (ParamsType == ParamsType.Json)
            {
                httpContent = new StringContent(JsonConvert.SerializeObject(Data), Encoding.UTF8, "application/json");
            }
            else if (ParamsType == ParamsType.FormData)
            {
                var data = new MultipartFormDataContent();
                //表单数据
                if (Data != null && Data.Count > 0)
                {
                    foreach (var item in Data)
                    {
                        data.Add(new StringContent(item.Value), item.Key);
                    }
                }
                //文件数据

                if (Files != null && Files.Count > 0)
                {
                    foreach (var item in Files)
                    {
                        var fileInfo = new FileInfo(item.Value);
                        data.Add(new ByteArrayContent(File.ReadAllBytes(item.Value)), item.Key, fileInfo.Name);
                    }
                }
                httpContent = data;
            }
            var response = await httpClient.PostAsync(Url, httpContent);
            var content = await response.Content.ReadAsStringAsync();
            var result = new HttpResultModel()
            {
                IsSuccess = true,
                Content = content,
                StatusCode = (int)response.StatusCode
            };
            return result;
        }
        private void HandleConfig()
        {
            httpClient.Timeout = new TimeSpan(0, 0, Timeout);
            if (Headers != null && Headers.Count > 0)
            {
                foreach (var item in Headers)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
    }
}
