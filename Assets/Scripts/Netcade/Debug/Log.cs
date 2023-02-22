using System;
using System.Collections.Generic;

namespace Netcade.Debug
{
    public static class Logging
    {
        public enum LogType
        {
            Info = 0,
            Warning = 1,
            Important = 2,
            Error = 3,
            Success = 4,
            Networking = 5,
        }
        public class LogItem
        {
            public LogType Type;
            public string Text;
            public int Frame;
            public DateTime Time;

            public LogItem(string text, LogType type)
            {
                Text = text;
                Type = type;
                try
                {
                    Frame = UnityEngine.Time.frameCount;
                }
                catch
                {
                    Frame = 0;
                }

                Time = DateTime.Now;
            }
        }

        public static List<LogItem> Logs;
        public static List<GetLog> Subscribed;

        static Logging()
        {
            Subscribed = new List<GetLog>();
        }
        
        public static void Log(string text, LogType type = LogType.Info, string source = "unknown")
        {
            if (Logs == null)
            {
                Logs = new List<LogItem>();
            }

            LogItem Item = new LogItem("["+source+"] " + text, type);
            Logs.Add(Item);
            
            if (Item.Type == LogType.Info)
            {
                UnityEngine.Debug.Log(text);
            }
            if (Item.Type == LogType.Important || type == LogType.Warning)
            {
                UnityEngine.Debug.LogWarning(text);
            }
            if (Item.Type == LogType.Error)
            {
                UnityEngine.Debug.LogError(text);
            }
            if (Item.Type == LogType.Success)
            {
                UnityEngine.Debug.Log(text);
            }

            foreach (GetLog x in Subscribed)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() => { x.GetLog(Item); });
            }
        }

        public interface GetLog
        {
            void GetLog(LogItem Item);
        }
    }
}