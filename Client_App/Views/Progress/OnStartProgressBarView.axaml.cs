using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Client_App.ViewModels;
using Client_App.Interfaces.BackgroundLoader;

namespace Client_App.Views.Progress;

public partial class OnStartProgressBarView : ReactiveWindow<OnStartProgressBarVM>
{
    public OnStartProgressBarView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new OnStartProgressBarVM(this, new BackgroundLoader());
    }
}