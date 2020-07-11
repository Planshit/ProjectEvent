using ProjectEvent.Core.Action.Models;
using ProjectEvent.Core.Action.Types;
using ProjectEvent.Core.Action.Types.ResultTypes;
using ProjectEvent.Core.Helper;
using ProjectEvent.Core.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ProjectEvent.Core.Action.Actions
{
    public class SoundPlayAction : IAction
    {
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern long mciSendString(string strCommand, string strReturn, int iReturnLength, IntPtr hwndCallback);
        private string alias;
        private Thread thread;
        private void Close()
        {
            mciSendString($"close {alias}", null, 0, IntPtr.Zero);
        }

        private void Play(string file)
        {
            mciSendString($"open \"{file}\" type mpegvideo alias {alias}", null, 0, IntPtr.Zero);
            mciSendString($"play {alias}", null, 0, IntPtr.Zero);
        }
        public SoundPlayAction()
        {
            alias = "spa_" + new Random().Next(0, 9999);
        }

        private void PlaySound(string file)
        {
            thread = new Thread((ThreadStart)delegate
            {
                Play(file);
                while (true)
                {
                    string status = "";
                    status = status.PadLeft(10, Convert.ToChar(" "));
                    mciSendString($"status {alias} mode", status, status.Length, IntPtr.Zero);
                    if (status.Contains("stopped"))
                    {
                        Close();
                        break;
                    }
                    Thread.Sleep(30000);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                var p = ObjectConvert.Get<SoundPlayActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)CommonResultKeyType.IsSuccess, "false");
                p.Path = ActionParameterConverter.ConvertToString(taskID, p.Path);
                try
                {
                    PlaySound(p.Path);
                    result.Result[(int)CommonResultKeyType.IsSuccess] = "true";
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }
                //返回数据
                ActionTaskResulter.Add(taskID, result);
            };
        }
    }
}
