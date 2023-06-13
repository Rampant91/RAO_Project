using System;
using System.Reactive.Linq;
using Avalonia.Controls.Mixins;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Client_App.ViewModels;
using ReactiveUI;

namespace Client_App.Views;

public abstract class BaseWindow<T> : ReactiveWindow<T> where T : BaseVM
{
    protected BaseWindow()
    {
        this.WhenActivated(disposable =>
        {
            ViewModel
                .WhenAnyValue(x => x.IsBusy)
                .Do(UpdateCursor)
                .Subscribe()
                .DisposeWith(disposable);
        });
    }

    private void UpdateCursor (bool show)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Cursor = show
                ? new Avalonia.Input.Cursor(Avalonia.Input.StandardCursorType.Wait)
                : Avalonia.Input.Cursor.Default;
        });
    }
}