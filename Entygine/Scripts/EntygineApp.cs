using Entygine.DevTools;
using OpenToolkit.Windowing.Desktop;

namespace Entygine
{
    public static class EntygineApp
    {
        public static void StartEngine()
        {
            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            gameWindowSettings.UpdateFrequency = 60.0d;
            nativeWindowSettings.Title = "Entygine";
            nativeWindowSettings.Size = new OpenToolkit.Mathematics.Vector2i(800, 600);

            using MainDevWindowGL mainWindow = new MainDevWindowGL(gameWindowSettings, nativeWindowSettings);

            DevConsole.Log("Engine started succesfully.");

            mainWindow.Run();
        }
    }
}
