using System;

namespace Entygine.DevTools
{
    public class NativeConsoleLogger : IConsoleLogger
    {
        public void Log(LogData log)
        {
            Console.WriteLine(log.ToString());
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
