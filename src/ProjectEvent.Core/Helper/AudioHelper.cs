using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectEvent.Core.Helper
{
    public static class AudioHelper
    {
        static CoreAudioController controller;
        private static void Init()
        {
            if (controller == null)
            {
                controller = new CoreAudioController();
            }
        }
        /// <summary>
        /// 获取当前声音播放设备的音量
        /// </summary>
        /// <returns></returns>
        public static double GetMasterVolume()
        {
            try
            {
                Init();
                var device = controller.DefaultPlaybackDevice;
                return device.Volume;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }
            return -1;
        }
        /// <summary>
        /// 设置当前声音播放设备的音量
        /// </summary>
        /// <param name="value"></param>
        public static void SetMasterVolume(double value)
        {
            try
            {
                Init();
                var device = controller.DefaultPlaybackDevice;
                device.Volume = value;
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }

        }
    }
}
