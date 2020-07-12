using System;
using OpenTK;

namespace Entygine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MainDevWindow mainWindow = new MainDevWindow(800, 600, "Main Window"))
            {
                mainWindow.Run(60.0d);
            }
        }
    }
}
