using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Action.Types
{
    public enum ActionType
    {
        /// <summary>
        /// 写文件
        /// </summary>
        WriteFile = 1,
        /// <summary>
        /// 删除文件
        /// </summary>
        DeleteFile = 2,
        /// <summary>
        /// 判断
        /// </summary>
        IF = 3,
        /// <summary>
        /// HTTP请求
        /// </summary>
        HttpRequest = 4,
        /// <summary>
        /// 关机
        /// </summary>
        Shutdown = 5,
        /// <summary>
        /// 启动一个进程
        /// </summary>
        StartProcess = 6,
        /// <summary>
        /// 打开一个网页链接
        /// </summary>
        OpenURL = 7,
        /// <summary>
        /// 截取当前屏幕
        /// </summary>
        Snipping = 8,
        /// <summary>
        /// 播放音频
        /// </summary>
        SoundPlay = 9,
        /// <summary>
        /// 获取IP地址
        /// </summary>
        GetIPAddress = 10,
        /// <summary>
        /// 键盘操作
        /// </summary>
        Keyboard = 11,
        /// <summary>
        /// 发送系统通知
        /// </summary>
        SystemNotification = 12,
        /// <summary>
        /// 下载网络文件
        /// </summary>
        DownloadFile = 13,
        /// <summary>
        /// 对话框
        /// </summary>
        Dialog = 14,
        /// <summary>
        /// 等待/延迟
        /// </summary>
        Delay = 15,
        /// <summary>
        /// 循环
        /// </summary>
        Loops = 16,
        /// <summary>
        /// 关闭进程
        /// </summary>
        KillProcess = 17,
        /// <summary>
        /// 设置设备音量
        /// </summary>
        SetDeviceVolume = 18,
        /// <summary>
        /// 正则表达式匹配
        /// </summary>
        Regex = 19,
        /// <summary>
        /// 读取文件
        /// </summary>
        ReadFile = 20
    }
}
