using System.ComponentModel;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
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
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<MainWindowVM, object> interaction)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(interaction.Input);
            desktop.MainWindow.Show();
            Close();
        }
        interaction.SetOutput(null);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new OnStartProgressBarVM(new BackgroundLoader());
    }
}