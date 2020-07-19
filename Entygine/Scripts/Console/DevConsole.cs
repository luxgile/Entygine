using System;
using System.Collections.Generic;

namespace Entygine.DevTools
{
    public static class DevConsole
    {
        private static List<string> logs = new List<string>();

        public static event Action ConsoleUpdated;

        public static void Log(object log)
        {
            logs.Add(log.ToString());
            ConsoleUpdated?.Invoke();
        }

        public static void ClearAll()
        {
            logs.Clear();
            ConsoleUpdated?.Invoke();
        }

        public static string[] GetLogs() => logs.ToArray();
    }
}
