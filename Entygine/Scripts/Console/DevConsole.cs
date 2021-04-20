using System;
using System.Collections.Generic;
using System.Text;

namespace Entygine.DevTools
{
    public static class DevConsole
    {
        private static List<IConsoleLogger> loggers = new List<IConsoleLogger>();

        public static void AddLogger(IConsoleLogger logger)
        {
            if (!loggers.Contains(logger))
                loggers.Add(logger);
        }

        public static void RemoveLogger(IConsoleLogger logger)
        {
            loggers.Remove(logger);
        }

        public static void Log(LogType type, object log) => Log(new LogData(type, log));
        public static void Log(LogData logData)
        {
            for (int i = 0; i < loggers.Count; i++)
            {
                try
                {
                    loggers[i].Log(logData);
                }
                catch (Exception e)
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

    public enum LogType
    {
        VeryVerbose,
        Verbose,
        Info,
        Warning,
        Error,
    }

    public struct LogData
    {
        public LogType type;
        public object log;

        public LogData(LogType type, object log) : this()
        {
            this.type = type;
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            Date = DateTime.Now;
        }

        public DateTime Date { get; private set; }

        public override string ToString()
        {
            return $"[{Date}] - {type}: {log}";
        }

    }
}
