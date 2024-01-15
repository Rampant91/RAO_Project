using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.ViewModels.ProgressBar;

namespace Client_App.Views.ProgressBar;

public partial class ExcelExportProgressBar : BaseWindow<ExcelExportProgressBarVM>
{
    public ExcelExportProgressBarVM ExcelExportProgressBarVM { get; }

    public ExcelExportProgressBar()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        //if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        //{
        //    ShowDialog(desktop.MainWindow);
        //}
        
        //this.WhenActivated(d =>
        //{
        //    var vm = (ExcelExportProgressBarVM)ViewModel;
        //    d(vm!.ShowDialog.RegisterHandler(DoShowDialogAsync));
        //});

        DataContext = new ExcelExportProgressBarVM(this, new BackgroundLoader());
        ExcelExportProgressBarVM = (DataContext as ExcelExportProgressBarVM)!;
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ShowDialog(desktop.MainWindow);
        }
    }

    //private async Task DoShowDialogAsync(InteractionContext<MainWindowVM, object> interaction)
    //{
    //    Show();
    //    interaction.SetOutput(null);
    //}

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}