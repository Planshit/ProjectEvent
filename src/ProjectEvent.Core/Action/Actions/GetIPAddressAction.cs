using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Net;
using ProjectEvent.Core.Services;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectEvent.Core.Action.Actions
{
    public class GetIPAddressAction : IAction
    {
        public event ActionInvokeHandler OnEventStateChanged;

        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Runing);
                var p = ObjectConvert.Get<GetIPAddressActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)GetIPAddressResultType.IsSuccess, "false");
                result.Result.Add((int)GetIPAddressResultType.IP, string.Empty);
                try
                {
                    if (p.Type == Core.Types.IPAddressType.LocalIPV4)
                    {
                        //获取内网ipv4
                        var host = Dns.GetHostEntry(Dns.GetHostName());
                        string ipaddress = string.Empty;
                        foreach (var ip in host.AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipaddress = ip.ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(ipaddress))
                        {
                            result.Result[(int)GetIPAddressResultType.IsSuccess] = "true";
                            result.Result[(int)GetIPAddressResultType.IP] = ipaddress;
                        }

                    }
                    else if (p.Type == Core.Types.IPAddressType.PublicIPV4 && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        //获取公网ipv4
                        var httprequest = new HttpRequest();
                        httprequest.Url = "https://202020.ip138.com/";
                        httprequest.Headers = new Dictionary<string, string>();
                        httprequest.Headers.Add("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 10_3_1 like Mac OS X) AppleWebKit/603.1.30 (KHTML, like Gecko) Version/10.0 Mobile/14E304 Safari/602.1");

                        string html = httprequest.GetAsync().Result.Content;
                        var reg = Regex.Match(html, @"(?<ip>(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d)\.(25[0-5]|2[0-4]\d|[0-1]\d{2}|[1-9]?\d))");
                        if (reg.Success && !string.IsNullOrEmpty(reg.Groups["ip"].Value))
                        {
                            result.Result[(int)GetIPAddressResultType.IsSuccess] = "true";
                            result.Result[(int)GetIPAddressResultType.IP] = reg.Groups["ip"].Value;
                        }
                    }

                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }
                //返回数据
                ActionTaskResulter.Add(taskID, result);
                OnEventStateChanged?.Invoke(taskID, action.ID, ActionInvokeStateType.Done);
            };
        }
    }
}
