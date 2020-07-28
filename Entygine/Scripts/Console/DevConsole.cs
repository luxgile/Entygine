using System;
using System.Collections.Generic;

namespace Entygine.DevTools
{
    public static class DevConsole
    {
        private static List<string> logs = new List<string>();

        public static event Action<string> LogAdded;
        public static event Action LogCleared;

        public static void Log(object log)
        {
            string msg = log.ToString();
            logs.Add(msg);
            LogAdded?.Invoke(msg);
        }

        public static void ClearAll()
        {
            logs.Clear();
            LogCleared?.Invoke();
        }

        public static string[] GetLogs() => logs.ToArray();
    }
}
