using ProjectEvent.Core.Net.Models;
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
        /// 设置Request header authorization
        /// </summary>
        public AuthenticationHeaderValue Authorization { get; set; }
        /// <summary>
        /// 设置表单数据
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 设置上传文件路径
        /// </summary>
        public string FilePath { get; set; }
        public HttpRequest()
        {
            httpClient = HttpClient.client;
            httpClient.Timeout = new TimeSpan(0, 0, Timeout);
            httpClient.DefaultRequestHeaders.Authorization = Authorization;
        }
        /// <summary>
        /// 发送一个GET请求并获取返回内容
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResultModel> GetAsync()
        {
            var response = await httpClient.GetAsync(Url);
            var content = await response.Content.ReadAsStringAsync();
            var result = new HttpResultModel()
            {
                IsSuccess=true,
                Content = content,
                StatusCode = (int)response.StatusCode
            };
            return result;
        }
        /// <summary>
        /// 发送一个POST请求并返回内容
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResultModel> PostAsync()
        {
            var response = await httpClient.PostAsync(Url, new FormUrlEncodedContent(Data));

            var content = await response.Content.ReadAsStringAsync();
            var result = new HttpResultModel()
            {
                IsSuccess = true,
                Content = content,
                StatusCode = (int)response.StatusCode
            };
            return result;
        }
        /// <summary>
        /// 发送一个上传文件请求
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResultModel> UploadFileAsync()
        {
            var result = new HttpResultModel();
            if (FilePath == null || !File.Exists(FilePath))
            {
                return result;
            }
            var fileInfo = new FileInfo(FilePath);
            var multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(new ByteArrayContent(File.ReadAllBytes(FilePath)), fileInfo.Name, fileInfo.Name);
            var response = await httpClient.PostAsync(Url, multipartFormData);
            var content = await response.Content.ReadAsStringAsync();
            result.Content = content;
            result.IsSuccess = true;
            result.StatusCode = (int)response.StatusCode;
            return result;

        }
    }
}
