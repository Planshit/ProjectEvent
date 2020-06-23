﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ProjectEvent.UI.Base.Color
{
    public class Colors
    {
        public struct IColor
        {
            /// <summary>
            /// 颜色名称
            /// </summary>
            public string Name;
            /// <summary>
            /// 颜色
            /// </summary>
            public string Color;
        }
        public static Dictionary<ColorTypes, IColor> ColorList = new Dictionary<ColorTypes, IColor>() {
            {ColorTypes.Aquamarine,new IColor{Name="碧绿", Color="#00AA90"}  },
            {ColorTypes.Black,new IColor{Name="暗黑", Color="#080808"} },
            {ColorTypes.Blue,new IColor{Name="蓝色", Color="#0078d4"} },
            {ColorTypes.Cyan,new IColor{Name="青色", Color="#51A8DD"} },
            {ColorTypes.Gold,new IColor{Name="金色", Color="#EFBB24"} },
            {ColorTypes.Gray,new IColor{Name="灰色", Color="#828282"} },
            {ColorTypes.Green,new IColor{Name="绿色", Color="#227D51"} },
            {ColorTypes.Orange,new IColor{Name="橙色", Color="#E98B2A"} },
            {ColorTypes.Pink,new IColor{Name="粉红", Color="#B5495B"} },
            {ColorTypes.Red,new IColor{Name="赤红", Color="#CB4042"} },
            {ColorTypes.Violet,new IColor{Name="紫色", Color="#77428D"} },
            {ColorTypes.Yellow,new IColor{Name="黄色", Color="#FFC408"} },
            {ColorTypes.White,new IColor{Name="白色", Color="#FFFFFF"} },

        };

        public static IColor Get(ColorTypes color)
        {
            return ColorList[color];
        }
        public static SolidColorBrush GetColor(ColorTypes color, double opacity = 1)
        {
            return GetFromString(ColorList[color].Color, opacity);
        }
        public static SolidColorBrush GetFromString(string color, double opacity = 1)
        {
            return new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(color))
            {
                Opacity = opacity
            };
        }
    }
}
