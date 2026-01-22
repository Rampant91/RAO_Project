using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Reactive.Linq;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;
using Client_App.Properties;

namespace Client_App.ViewModels;

public class OnStartProgressBarVM : BaseVM, INotifyPropertyChanged
{
    private Task MainTask { get; set; }

    private MainWindowVM MainWindowVM { get; set; }

    #region Constructor

    public OnStartProgressBarVM() {}

    public OnStartProgressBarVM(IBackgroundLoader backgroundWorker)
    {
        ShowDialog = new Interaction<MainWindowVM, object>();
        backgroundWorker.BackgroundWorker(() =>
        {
            ServiceExtension.LoggerManager.CreateFile("Import.log");
            ServiceExtension.LoggerManager.CreateFile("Crash.log");
        }, () =>
        {
            MainTask = new Task(async () => await Start().ConfigureAwait(false));
            MainTask.GetAwaiter().OnCompleted(async () => await ShowDialog.Handle(MainWindowVM));
            MainTask.Start();
        });
    }

    #endregion

    #region Properties

    private double _onStartProgressBar;
    public double OnStartProgressBar
    {
        get => _onStartProgressBar;
        set
        {
            if (_onStartProgressBar.Equals(value)) return;
            _onStartProgressBar = value;
            OnPropertyChanged();
        }
    }

    private string _loadStatus;
    public string LoadStatus
    {
        get => _loadStatus;
        set
        {
            if (_loadStatus == value) return;
            _loadStatus = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private async Task Start()
    {
        MainWindowVM = new MainWindowVM();
        MainWindowVM.PropertyChanged += OnMainWindowVMPropertyChanged;
        await new InitializationAsyncCommand(MainWindowVM).AsyncExecute(this);

        if (Settings.Default.AppStartupParameters.Trim().Split(',').Any(x => x is "-p"))
        {
            await BackgroundWorkThenAppLaunchedWithOperParameter();
            Environment.Exit(0);
        }
        else if (Settings.Default.AppStartupParameters.Trim().Split(',').Any(x => x is "-y"))
        {
            await BackgroundWorkThenAppLaunchedWithYearParameter();
            Environment.Exit(0);
        }
        Settings.Default.AppLaunchedInNorao = Settings.Default.AppStartupParameters.Trim().Split(',').Any(x => x is "-n"); 
    }

    #region BackgroundWork
    
    #region BackgroundWorkThenAppLaunchedWithOperParameter

    /// <summary>
    /// Команды, выполняющиеся автоматически при запуске программы с ключом "-p" (оперативная отчётность).
    /// </summary>
    private async Task BackgroundWorkThenAppLaunchedWithOperParameter()
    {
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute("full");
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute(null);
        await new ExcelExportExecutorsAsyncCommand().AsyncExecute(null);
        await new ExcelExportIntersectionsAsyncCommand().AsyncExecute(null);
        await new ExcelExportListOfForms1AsyncCommand().AsyncExecute(null);
        await new ExcelExportAllAsyncCommand(MainWindowVM).AsyncExecute(null);
    }

    #endregion

    #region BackgroundWorkThenAppLaunchedWithYearParameter

    /// <summary>
    /// Команды, выполняющиеся автоматически при запуске программы с ключом "-y" (годовая отчётность).
    /// </summary>
    private async Task BackgroundWorkThenAppLaunchedWithYearParameter()
    {
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute("full");
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute(null);
        await new ExcelExportExecutorsAsyncCommand().AsyncExecute(null);
        await new ExcelExportListOfForms2AsyncCommand().AsyncExecute(null);
        await new ExcelExportAllAsyncCommand(MainWindowVM).AsyncExecute(null);
    }

    #endregion 
    
    #endregion

    private void OnMainWindowVMPropertyChanged(object sender,PropertyChangedEventArgs args)
    {
        if(args.PropertyName==nameof(OnStartProgressBar))
        {
            OnStartProgressBar = MainWindowVM.OnStartProgressBar;
        }
    }

    public Interaction<MainWindowVM, object> ShowDialog { get; private set; }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    
    #endregion
}