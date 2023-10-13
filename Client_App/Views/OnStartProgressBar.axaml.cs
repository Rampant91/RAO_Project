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
using Client_App.Resources;

namespace Client_App.Views;

public partial class OnStartProgressBar : BaseWindow<OnStartProgressBarVM>
{
    public OnStartProgressBar()
    {
        InitializeComponent();
        DataContext = new OnStartProgressBarVM(new BackgroundLoader());
        this.WhenActivated(d =>
        {
            var vm = (OnStartProgressBarVM)ViewModel;
            d(vm!.ShowDialog.RegisterHandler(DoShowDialogAsync));
        });
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
}