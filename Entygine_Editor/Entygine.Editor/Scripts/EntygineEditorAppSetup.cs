using Avalonia;
using Avalonia.ReactiveUI;

namespace Entygine.Editor
{
    public static class EntygineEditorAppSetup
    {
        public static void StartEditor()
        {
            BuildApp().StartWithClassicDesktopLifetime(null);
        }

        public static AppBuilder BuildApp()
        {
            return AppBuilder.Configure<EntygineEditorApp>().UseReactiveUI().UsePlatformDetect().LogToDebug();
        }
    }
}
