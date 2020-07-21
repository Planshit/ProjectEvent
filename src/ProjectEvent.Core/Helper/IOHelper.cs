using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace ProjectEvent.Core.Helper
{
    public static class IOHelper
    {
        /// <summary>
        /// 获取文件完整路径
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isinbasedir">是否在程序运行目录中</param>
        /// <returns></returns>
        public static string GetFullPath(string path, bool isinbasedir = true)
        {
            return isinbasedir ?
                Path.Combine(
                    Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                    //"Project Event",
                    path) :
                path;

        }
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dir">文件夹或完整路径</param>
        /// <param name="isinbasedir">是否在程序运行目录中创建</param>
        public static void CreateDirectory(string dir, bool isinbasedir = true)
        {
            try
            {
                string path = GetFullPath(dir, isinbasedir);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e.ToString());
            }

        }

        /// <summary>
        /// 获取文件夹信息
        /// </summary>
        /// <param name="dir">文件夹或完整路径</param>
        /// <param name="isinbasedir">是否在程序运行目录中</param>
        /// <returns></returns>
        public static DirectoryInfo GetDirectoryInfo(string dir, bool isinbasedir = true)
        {
            CreateDirectory(dir, isinbasedir);
            return new DirectoryInfo(GetFullPath(dir, isinbasedir));
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="isinbasedir">是否在程序运行目录中</param>
        /// <returns></returns>
        public static bool FileExists(string path, bool isinbasedir = true)
        {
            return File.Exists(GetFullPath(path, isinbasedir));
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isinbasedir">是否在程序运行目录中</param>
        /// <returns></returns>
        public static bool FileDelete(string path, bool isinbasedir = true)
        {
            if (FileExists(path, isinbasedir))
            {
                File.Delete(GetFullPath(path, isinbasedir));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="contents">内容</param>
        /// <param name="isinbasedir">是否在程序运行目录中</param>
        public static void WriteFile(string path, string contents, bool isinbasedir = true)
        {
            path = GetFullPath(path, isinbasedir);
            File.WriteAllText(path, contents);
        }

        public static string ReadFile(string path, bool isinbasedir = true)
        {
            path = GetFullPath(path, isinbasedir);
            return File.ReadAllText(path);
        }

        public static void AppendFile(string path, string content, bool isinbasedir = true)
        {
            path = GetFullPath(path, isinbasedir);
            File.AppendAllText(path, content);
        }
    }
}
