using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace Entygine.Editor
{
    public class BaseViewModel : ReactiveObject
    {
        public ViewModelActivator Activator { get; }
        public BaseViewModel()
        {
            Activator = new ViewModelActivator();
        }
    }
}
