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

    private MainWindowVM VMDataContext { get; set; }

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
            MainTask.GetAwaiter().OnCompleted(async () => await ShowDialog.Handle(VMDataContext));
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
        VMDataContext = new MainWindowVM();
        VMDataContext.PropertyChanged += OnVMPropertyChanged;
        await new InitializationAsyncCommand(VMDataContext).AsyncExecute(this);

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
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute(this);
        await new ExcelExportExecutorsAsyncCommand().AsyncExecute(this);
        await new ExcelExportIntersectionsAsyncCommand().AsyncExecute(this);
        await new ExcelExportListOfForms1AsyncCommand().AsyncExecute(this);
        await new ExcelExportAllAsyncCommand().AsyncExecute(this);
    }

    #endregion

    #region BackgroundWorkThenAppLaunchedWithYearParameter

    /// <summary>
    /// Команды, выполняющиеся автоматически при запуске программы с ключом "-y" (годовая отчётность).
    /// </summary>
    private async Task BackgroundWorkThenAppLaunchedWithYearParameter()
    {
        await new ExcelExportListOfOrgsAsyncCommand().AsyncExecute(this);
        await new ExcelExportExecutorsAsyncCommand().AsyncExecute(this);
        await new ExcelExportListOfForms2AsyncCommand().AsyncExecute(this);
        await new ExcelExportAllAsyncCommand().AsyncExecute(this);
    }

    #endregion 
    
    #endregion

    private void OnVMPropertyChanged(object sender,PropertyChangedEventArgs args)
    {
        if(args.PropertyName==nameof(OnStartProgressBar))
        {
            OnStartProgressBar = VMDataContext.OnStartProgressBar;
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