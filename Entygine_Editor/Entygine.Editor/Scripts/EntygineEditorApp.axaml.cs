using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using OpenToolkit.Graphics.ES20;
using System.Security.Cryptography;
using System.Threading;

namespace Entygine.Editor
{
    public class EntygineEditorApp : Application
    {
        private Thread mainEngineThread;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();

            mainEngineThread = new Thread(StartEngine);
            mainEngineThread.Start();
        }

        private void StartEngine()
        {
            EntygineApp.StartEngine();
        }
    }
}
