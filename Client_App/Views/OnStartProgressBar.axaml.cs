using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using Client_App.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Interfaces.BackgroundLoader;

namespace Client_App.Views;

public partial class OnStartProgressBar : ReactiveWindow<OnStartProgressBarVM>
{
    public OnStartProgressBar()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync));
            //SetWindowStartupLocationWorkaroundForLinux();
        });
    }

    private async Task DoShowDialogAsync(InteractionContext<MainWindowVM, object> interaction)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(interaction.Input);
            desktop.MainWindow.Show();

            //SetWindowStartupLocationWorkaroundForLinux();

            Close();
        }
        interaction.SetOutput(null);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new OnStartProgressBarVM(new BackgroundLoader());
        //SetWindowStartupLocationWorkaroundForLinux();
    }

    private void SetWindowStartupLocationWorkaroundForLinux()
    {
        if(OperatingSystem.IsWindows()) return;

        var scale = PlatformImpl?.DesktopScaling ?? 1.0;
        var windowBase = Owner?.PlatformImpl;
        if(windowBase != null) 
        {
            scale = windowBase.DesktopScaling;
        }
        var rect = new PixelRect(PixelPoint.Origin, PixelSize.FromSize(ClientSize, scale));
        if(WindowStartupLocation == WindowStartupLocation.CenterScreen) 
        {
            var screen = Screens.ScreenFromPoint(windowBase?.Position ?? Position);
            if(screen == null) return;
            Position = screen.WorkingArea.CenterRect(rect).Position;
        }
        else 
        {
            if(windowBase == null || WindowStartupLocation != WindowStartupLocation.CenterOwner) return;
            Position = new PixelRect(windowBase.Position, PixelSize.FromSize(windowBase.ClientSize, scale))
                .CenterRect(rect).Position;
        }
    }
}