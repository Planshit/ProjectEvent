using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.UI.Controls.Action.Models;
using ProjectEvent.UI.Models.DataModels;
using ProjectEvent.UI.Types;
using System;
using System.Collections.Generic;
using System.Text;
using ActionType = ProjectEvent.UI.Types.ActionType;

namespace ProjectEvent.UI.Controls.Action.Data
{
    public class ActionData
    {
        #region 通过UI action type获取一个新的基础AcionItemModel
        /// <summary>
        /// 通过UI action type获取一个新的基础AcionItemModel
        /// </summary>
        /// <param name="uiactionType"></param>
        /// <returns></returns>
        public static ActionItemModel GetCreateActionItemModel(ActionType uiactionType)
        {
            var action = new ActionItemModel();
            action.ActionType = uiactionType;
            action.ActionName = ActionData.Names[uiactionType];
            switch (uiactionType)
            {
                case ActionType.HttpRequest:
                    action.Icon = Base.IconTypes.DownloadDocument;
                    break;
                case ActionType.IF:
                    action.Icon = Base.IconTypes.FlowChart;
                    break;
                case ActionType.WriteFile:
                    action.Icon = Base.IconTypes.FileTemplate;
                    break;
                case ActionType.Shutdown:
                    action.Icon = Base.IconTypes.DeviceOff;
                    break;
                case ActionType.StartProcess:
                    action.Icon = Base.IconTypes.ProcessingRun;
                    break;
                case ActionType.OpenURL:
                    action.Icon = Base.IconTypes.WebEnvironment;
                    break;
                case ActionType.Snipping:
                    action.Icon = Base.IconTypes.DesktopScreenshot;
                    break;
                case ActionType.DeleteFile:
                    action.Icon = Base.IconTypes.Delete;
                    break;
                case ActionType.SoundPlay:
                    action.Icon = Base.IconTypes.MusicInCollectionFill;
                    break;
                case ActionType.GetIPAddress:
                    action.Icon = Base.IconTypes.NetworkTower;
                    break;
                case ActionType.Keyboard:
                    action.Icon = Base.IconTypes.KeyboardClassic;
                    break;
                case ActionType.SystemNotification:
                    action.Icon = Base.IconTypes.Message;
                    break;
                case ActionType.DownloadFile:
                    action.Icon = Base.IconTypes.Download;
                    break;
                case ActionType.Dialog:
                    action.Icon = Base.IconTypes.Storyboard;
                    break;
                case ActionType.Delay:
                    action.Icon = Base.IconTypes.HandsFree;
                    break;
                case ActionType.Loops:
                    action.Icon = Base.IconTypes.PlaybackRate1x;
                    break;
                case ActionType.KillProcess:
                    action.Icon = Base.IconTypes.ProcessingCancel;
                    break;
            }

            return action;
        }
        #endregion

        #region action names
        public static Dictionary<ActionType, string> Names = new Dictionary<ActionType, string>()
        {
            {
                ActionType.HttpRequest,"HTTP请求"
            },
            {
                ActionType.IF,"判断"
            },
            {
                ActionType.IFElse,"否则"
            },
            {
                ActionType.IFEnd,"判断结束"
            },
            {
                ActionType.WriteFile,"创建文件"
            },
            {
                ActionType.Shutdown,"关闭电脑"
            },
            {
                ActionType.StartProcess,"启动进程"
            },
            {
                ActionType.OpenURL,"浏览器打开网页"
            },
            {
                ActionType.Snipping,"获取屏幕截图"
            },
            {
                ActionType.DeleteFile,"删除本地文件"
            },
            {
                ActionType.SoundPlay,"播放本地音频"
            },
            {
                ActionType.GetIPAddress,"获取IP"
            },
            {
                ActionType.Keyboard,"模拟键盘操作"
            },
            {
                ActionType.SystemNotification,"发送系统通知"
            },
            {
                ActionType.DownloadFile,"下载网络文件"
            },
            {
                ActionType.Dialog,"对话框"
            },
            {
                ActionType.Delay,"等待"
            },
            {
                ActionType.Loops,"循环"
            },
            {
                ActionType.LoopsEnd,"循环结束"
            },
            {
                ActionType.KillProcess,"关闭进程"
            },
        };
        #endregion

        #region 通过action type获取支持的返回操作结果
        /// <summary>
        /// 通过action type获取支持的返回操作结果
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static List<ComBoxModel> GetActionResults(ActionType actionType)
        {
            List<ComBoxModel> data = null;
            switch (actionType)
            {
                case UI.Types.ActionType.HttpRequest:
                    //http请求
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)HttpResultType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
            new ComBoxModel()
            {
                ID = (int)HttpResultType.StatusCode,
                DisplayName = "状态码"
            },
            new ComBoxModel()
            {
                ID = (int)HttpResultType.Content,
                DisplayName = "响应内容"
            },

        };
                    break;
                case UI.Types.ActionType.WriteFile:
                    //写文件
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)CommonResultKeyType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
        };
                    break;
                case UI.Types.ActionType.StartProcess:
                    //启动进程
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)StartProcessResultType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Handle,
                DisplayName = "句柄（仅成功时有效）"
            },
          new ComBoxModel()
            {
                ID = (int)StartProcessResultType.Id,
                DisplayName = "进程ID（仅成功时有效）"
            },

        };
                    break;
                case ActionType.OpenURL:
                    //http请求
                    data = new List<ComBoxModel>()
        {
            new ComBoxModel()
            {
                ID = (int)CommonResultKeyType.IsSuccess,
                DisplayName = "是否成功（true，false）"
            },
                    };
                    break;
                case ActionType.Snipping:
                    //截屏
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)SnippingResultType.IsSuccess,
                            DisplayName = "是否成功（true，false）"
                        },
                        new ComBoxModel()
                        {
                            ID=(int)SnippingResultType.SavePath,
                            DisplayName = "保存路径"
                        }
                    };
                    break;
                case ActionType.DeleteFile:
                    //删除文件
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)DeleteFileResultType.IsSuccess,
                            DisplayName = "是否成功（true，false）"
                        },
                        new ComBoxModel()
                        {
                            ID=(int)DeleteFileResultType.Path,
                            DisplayName = "被删除文件路径"
                        }
                    };
                    break;
                case ActionType.SoundPlay:
                    //播放音频
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)CommonResultKeyType.IsSuccess,
                            DisplayName = "是否成功（true，false）"
                        },
                    };
                    break;
                case ActionType.GetIPAddress:
                    //获取ip
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)GetIPAddressResultType.IsSuccess,
                            DisplayName = "是否成功（true，false）"
                        },
                        new ComBoxModel()
                        {
                            ID=(int)GetIPAddressResultType.IP,
                            DisplayName = "IP"
                        },
                    };
                    break;
                case ActionType.DownloadFile:
                    //删除文件
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)DownloadFileResultType.IsSuccess,
                            DisplayName = "是否成功（True，False）"
                        },
                        new ComBoxModel()
                        {
                            ID=(int)DownloadFileResultType.SavePath,
                            DisplayName = "文件保存路径"
                        }
                    };
                    break;
                case ActionType.Dialog:
                    //删除文件
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)DialogResultType.ClickButtonValue,
                            DisplayName = "点击的按钮值"
                        }
                    };
                    break;
                case ActionType.KillProcess:
                    data = new List<ComBoxModel>()
                    {
                        new ComBoxModel()
                        {
                            ID=(int)CommonResultKeyType.IsSuccess,
                            DisplayName = "是否成功（True，False）"
                        },
                    };
                    break;
            }
            return data;
        }
        #endregion
    }
}
