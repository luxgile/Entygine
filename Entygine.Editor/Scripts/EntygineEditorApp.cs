using Entygine.DevTools;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

namespace Entygine.Editor
{
    internal static class EntygineEditorApp
    {
        internal static void StartEditor()
        {

            GameWindowSettings gameWindowSettings = new GameWindowSettings();
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();

            gameWindowSettings.UpdateFrequency = 60.0d;
            nativeWindowSettings.Title = "Entygine Editor";
            nativeWindowSettings.Size = new OpenTK.Mathematics.Vector2i(1600, 900);

            using MainEditorWindow mainWindow = new MainEditorWindow(gameWindowSettings, nativeWindowSettings);

            EntygineApp.LoadEngine();

            System.Console.ReadKey();

            DevConsole.Log("Editor started succesfully.");

            //mainWindow.Run();
        }
    }
}