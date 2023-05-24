using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;
using Client_App.Views;

namespace Client_App.ViewModels;

public class OnStartProgressBarVM : BaseVM
{
    public Task MainTask { get; set; }
    public OnStartProgressBarVM(Window onStartProgressBar, IBackgroundLoader backgroundWorker)
    {
        backgroundWorker.BackgroundWorker(() =>
        {
            ServiceExtension.LoggerManager.CreateFile("Import.log");
        }, () =>
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow2() { DataContext = new MainWindow2VM() };
                desktop.MainWindow.Show();
                onStartProgressBar.Close();
            }
        });
    }
}