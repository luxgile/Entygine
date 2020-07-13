using OpenToolkit.Windowing.Desktop;
using System;

namespace Entygine
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindowSettings settings = new GameWindowSettings();
            NativeWindowSettings settings1 = new NativeWindowSettings();
            settings.UpdateFrequency = 60.0d;
            settings1.Title = "Main Window";
            settings1.Size = new OpenToolkit.Mathematics.Vector2i(800, 600);
            using (MainDevWindow mainWindow = new MainDevWindow(settings, settings1))
            {
                mainWindow.Run();
            }
        }
    }
}
