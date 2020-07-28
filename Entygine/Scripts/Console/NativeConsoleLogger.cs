using System;

namespace Entygine.DevTools
{
    public class NativeConsoleLogger : IConsoleLogger
    {
        public void Log(object log)
        {
            Console.WriteLine(log);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
