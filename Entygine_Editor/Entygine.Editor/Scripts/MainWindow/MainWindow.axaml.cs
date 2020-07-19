using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;

namespace Entygine.Editor
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            this.WhenActivated(disposables => 
            {
            });

            AvaloniaXamlLoader.Load(this);
        }
    }

    public class MainWindowViewModel : BaseViewModel
    {
    }
}
