using System;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Reactive.Linq;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Properties;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Avalonia;

namespace Client_App.ViewModels;

public class OnStartProgressBarVM : BaseVM, INotifyPropertyChanged
{
    private Task MainTask { get; set; }

    private MainWindowVM MainWindowVM { get; set; }

    /// <summary>Отмена случайного/прерванного запуска (крестик, Alt+F4).</summary>
    public CancellationTokenSource StartupCts { get; } = new();

    public string WindowTitle { get; } = "\u0417\u0430\u043f\u0443\u0441\u043a \u043f\u0440\u043e\u0433\u0440\u0430\u043c\u043c\u044b";

    public string CloseButtonTooltip { get; } =
        "\u041e\u0442\u043c\u0435\u043d\u0438\u0442\u044c \u0437\u0430\u043f\u0443\u0441\u043a \u0438 \u0437\u0430\u043a\u0440\u044b\u0442\u044c";

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
            // Task.Run дожидается полного завершения Start(); иначе главное окно открывалось бы до инициализации БД.
            MainTask = Task.Run(async () =>
            {
                try
                {
                    await Start().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Пользователь отменил запуск.
                }
            });
            _ = MainTask.ContinueWith(t =>
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (StartupCts.IsCancellationRequested || t.IsCanceled)
                    {
                        ShutdownApplication();
                        return;
                    }

                    if (t.IsFaulted)
                    {
                        var ex = t.Exception?.GetBaseException() ?? t.Exception;
                        var msg = $"\u041a\u0440\u0438\u0442\u0438\u0447\u0435\u0441\u043a\u0430\u044f \u043e\u0448\u0438\u0431\u043a\u0430 \u043f\u0440\u0438 \u0437\u0430\u043f\u0443\u0441\u043a\u0435 \u043f\u0440\u043e\u0433\u0440\u0430\u043c\u043c\u044b." +
                                  $"{Environment.NewLine}Message: {ex?.Message}" +
                                  $"{Environment.NewLine}StackTrace: {ex?.StackTrace}";
                        ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
                        Environment.Exit(1);
                        return;
                    }

                    await ShowDialog.Handle(MainWindowVM);

                    _ = InitializationAsyncCommand.ShowDatabaseBackupPromptIfDueAsync();
                });
            }, TaskContinuationOptions.None);
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
            OnPropertyChanged(nameof(ProgressPercentText));
        }
    }

    public string ProgressPercentText => $"{(int)Math.Round(OnStartProgressBar)}%";

    private string _loadStatus = string.Empty;
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

    public void RequestCancelStartup()
    {
        try
        {
            if (!StartupCts.IsCancellationRequested)
            {
                StartupCts.Cancel();
            }
        }
        catch (ObjectDisposedException)
        {
            // already disposed
        }
    }

    public void ThrowIfStartupCancelled() => StartupCts.Token.ThrowIfCancellationRequested();

    private static void ShutdownApplication()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(0);
        }
        else
        {
            Environment.Exit(0);
        }
    }

    private async Task Start()
    {
        Settings.Default.AppLaunchedInNorao = AppIsLaunchedInNorao();
        Settings.Default.Save(); // Сохраняем настройки

        ThrowIfStartupCancelled();

        MainWindowVM = new MainWindowVM();
        MainWindowVM.PropertyChanged += OnMainWindowVMPropertyChanged;
        await new InitializationAsyncCommand(MainWindowVM).AsyncExecute(this);

        ThrowIfStartupCancelled();

        if (Settings.Default.AppStartupParameters.Trim().Split(',').Any(x => x is "-p" or "-y"))
        {
            await BackgroundWorkThenAppLaunchedWithOperParameter();
            Environment.Exit(0);
        }
    }

    private static bool AppIsLaunchedInNorao()
    {
        var appIsLaunchedInNorao = Settings.Default.AppStartupParameters
            .Trim()
            .Split(',')
            .Any(x => x is "-n");

        return appIsLaunchedInNorao || File.Exists(@"Y:\АЧ 2021\Программа\developer.mode");
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
        await new ExcelExportListOfForms2AsyncCommand().AsyncExecute(null);
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
