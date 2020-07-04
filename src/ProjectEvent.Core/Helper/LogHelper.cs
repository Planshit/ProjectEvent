using ProjectEvent.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectEvent.Core.Helper
{
    public class LogHelper
    {
        //最大日志阈值
        private static int MaxLogsCount = 100;
        //缓存
        private static Dictionary<string, List<string>> storage = new Dictionary<string, List<string>>();
        //日志存放目录
        private const string LogDir = "Log\\";
        private static readonly object writeLock = new object();
        private static readonly object addLock = new object();

        public static void Debug(string text, bool write = false, bool console = true)
        {
            Log(LogLevelType.DEBUG, text, write, console);
        }
        public static void Error(string text, bool write = true, bool console = true)
        {
            Log(LogLevelType.ERROR, text, write, console);
        }
        public static void Warning(string text, bool write = true, bool console = true)
        {
            Log(LogLevelType.WARNING, text, write, console);
        }
        private static void Log(LogLevelType level, string text, bool write = false, bool console = true)
        {
            Task.Run(() =>
            {
                lock (addLock)
                {
                    string log = LogFormat(level, text);
                    if (write)
                    {
                        string key = DateTime.Now.ToString($"{level}_yyyyMMdd");
                        if (storage.ContainsKey(key))
                        {
                            if (storage[key].Count >= MaxLogsCount)
                            {
                                var logs = storage[key].ToArray();
                                SaveToFileTask(key, logs);
                                storage[key].Clear();
                            }
                            storage[key].Add(log);
                        }
                        else
                        {
                            storage.Add(key, new List<string>()
                        {
                            {log }
                        });
                        }
                    }
                }
            });

            if (console)
            {
                //在debug模式下打印到输出
                System.Diagnostics.Debug.WriteLine(LogFormat(level, text));
            }
        }

        private static void SaveToFileTask(string key, string[] logs)
        {
            Task.Run(() =>
            {
                SaveToFile(key, logs);
            });
        }
        private static void SaveToFile(string key, string[] logs)
        {
            lock (writeLock)
            {
                string date = key.Substring(key.Length - 8, 8);
                string dir = $"{LogDir}{date}\\";
                IOHelper.CreateDirectory(dir);
                string logstr = string.Join("\r\n", logs);
                IOHelper.AppendFile($"{dir}{key}.log", logstr);
            }
        }

        public static void Save()
        {
            Task.Run(() =>
            {
                foreach (var log in storage)
                {
                    SaveToFile(log.Key, log.Value.ToArray());
                }
            });
        }
        private static string LogFormat(LogLevelType level, string text)
        {
            string logText = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}]\r\n{text}\r\n------------------------\r\n\r\n";
            return logText;
        }
    }
}
