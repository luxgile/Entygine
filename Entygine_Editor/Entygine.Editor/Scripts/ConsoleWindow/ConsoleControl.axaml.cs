using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DynamicData;
using Entygine.DevTools;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;

namespace Entygine.Editor
{
    public class ConsoleControl : ReactiveUserControl<ConsoleWindowModelView>
    {
        private ItemsControl LogsControl => this.FindControl<ItemsControl>("Logs");

        public ConsoleControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated((disposables) =>
            {
                this.OneWayBind(ViewModel, mv => mv.Logs, v => v.LogsControl.Items).
                DisposeWith(disposables);
            });

            AvaloniaXamlLoader.Load(this);
        }
    }

    public class ConsoleWindowModelView : BaseViewModel
    {
        public ConsoleWindowModelView()
        {
            Logs = new ObservableCollection<string>(DevConsole.GetLogs());
            DevConsole.ConsoleUpdated += OnConsoleUpdate;
        }

        private void OnConsoleUpdate()
        {
            Logs.Clear();
            Logs.AddRange(DevConsole.GetLogs());
        }

        public ObservableCollection<string> Logs { get; }
    }
}
