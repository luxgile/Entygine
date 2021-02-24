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

            //gameWindowSettings.IsMultiThreaded = true;
            gameWindowSettings.UpdateFrequency = 60.0d;
            nativeWindowSettings.Title = "Entygine";
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(800, 600);

            using MainDevWindowGL mainWindow = new MainDevWindowGL(gameWindowSettings, nativeWindowSettings);

            DevConsole.Log("Engine started succesfully.");

            mainWindow.Run();
        }
    }
}
