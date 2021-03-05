using Entygine.DevTools;
using OpenTK.Windowing.Desktop;

namespace Entygine
{
    internal static class EntygineApp
    {
        internal static void StartEngine()
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            //Everything works but there are some blinking at the start
            //gameWindowSettings.IsMultiThreaded = true;
            gameWindowSettings.UpdateFrequency = 60.0d;
            nativeWindowSettings.Title = "Entygine";
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(1600, 900);

            using MainDevWindowGL mainWindow = new MainDevWindowGL(gameWindowSettings, nativeWindowSettings);

            DevConsole.Log("Engine started succesfully.");

            mainWindow.Run();
        }
    }
}
