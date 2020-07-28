using System;
using System.Collections.Generic;
using System.Text;

namespace Entygine.DevTools
{
    public static class DevConsole
    {
        private static List<IConsoleLogger> loggers = new List<IConsoleLogger>();
        private static StringBuilder sb = new StringBuilder();

        public static void AddLogger(IConsoleLogger logger)
        {
            if (!loggers.Contains(logger))
                loggers.Add(logger);
        }

        public static void RemoveLogger(IConsoleLogger logger)
        {
            loggers.Remove(logger);
        }

        public static void Log(object log)
        {
            sb.Clear();
            sb.Append($"[{System.DateTime.Now}] ");
            sb.Append($"{log} \n");

            for (int i = 0; i < loggers.Count; i++)
            {
                try
                {
                    loggers[i].Log(sb);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void Clear()
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                try
                {
                    loggers[i].Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
