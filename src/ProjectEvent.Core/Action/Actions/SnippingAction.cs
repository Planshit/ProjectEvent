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
using System.Linq;
using System.Text;

namespace ProjectEvent.Core.Action.Actions
{
    public class SnippingAction : IAction
    {
        public System.Action GenerateAction(int taskID, ActionModel action)
        {
            return () =>
            {
                var p = ObjectConvert.Get<SnippingActionParamsModel>(action.Parameter);
                var result = new ActionResultModel();
                result.ID = action.ID;
                result.Result = new Dictionary<int, string>();
                result.Result.Add((int)SnippingResultType.IsSuccess, "false");
                result.Result.Add((int)SnippingResultType.SavePath, p.SavePath);
                p.SavePath = ActionParameterConverter.ConvertToString(taskID, p.SavePath);
                try
                {
                    var sr = CommonWin32API.GetScreenResolution();
                    Bitmap bitmap = new Bitmap(sr.Width, sr.Height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(sr.Width, sr.Height));
                    EncoderParameters encoderParams = new EncoderParameters();
                    EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, new long[] { 100 });
                    encoderParams.Param[0] = encoderParam;
                    var codecInfo = ImageCodecInfo.GetImageEncoders().FirstOrDefault(ici => ici.MimeType == "image/jpeg");
                    bitmap.Save(p.SavePath, codecInfo, encoderParams);
                    result.Result[(int)SnippingResultType.IsSuccess] = "true";
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
