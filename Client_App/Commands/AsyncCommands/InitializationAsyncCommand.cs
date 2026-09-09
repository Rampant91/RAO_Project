using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Properties;
using Client_App.Resources.CustomComparers;
using Client_App.Services.DataAccess;
using Client_App.Services.Updates;
using Client_App.ViewModels;
using Client_App.Views.Messages;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using Spravochniki;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Client_App.ViewModels.BaseVM;
using Models.Forms.Form4;
using Models.Forms.Form5;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Инициализация программы при запуске.
/// </summary>
/// <param name="mainWindowViewModel">ViewModel главного окна.</param>
public partial class InitializationAsyncCommand(MainWindowVM mainWindowViewModel) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var onStartProgressBarVm = parameter as OnStartProgressBarVM;
        onStartProgressBarVm!.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Поиск системной директории";
        mainWindowViewModel.OnStartProgressBar = 1;
        await GetSystemDirectory();

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Создание временных файлов";
        mainWindowViewModel.OnStartProgressBar = 5;
        await ProcessRaoDirectory();

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Загрузка справочников";
        mainWindowViewModel.OnStartProgressBar = 10;
        await ProcessSpravochniks();

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Подключение к базе данных";
        mainWindowViewModel.OnStartProgressBar = 15;
        await ProcessDataBaseCreate(onStartProgressBarVm);

        onStartProgressBarVm.ThrowIfStartupCancelled();
        EnsureDatabaseBackupScheduleInitialized();

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Загрузка таблиц";
        mainWindowViewModel.OnStartProgressBar = 20;
        var dbm = StaticConfiguration.DBModel;

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Загрузка коллекций организаций";
        mainWindowViewModel.OnStartProgressBar = 25;
        await dbm.ReportsCollectionDbSet
            .Include(r => r.Master_DB)
            .LoadAsync(onStartProgressBarVm.StartupCts.Token);

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Загрузка коллекций базы";
        mainWindowViewModel.OnStartProgressBar = 60;
        if (!dbm.DBObservableDbSet.Any())
        {
            dbm.DBObservableDbSet.Add(new DBObservable());
            dbm.DBObservableDbSet.Local.First().Reports_Collection.AddRange(dbm.ReportsCollectionDbSet);
        }

        var removedReports = dbm.ReportsCollectionDbSet.Where(reps => reps.DBObservable == null);

        foreach (var reports in removedReports)
        {
            dbm.ReportsCollectionDbSet.Remove(reports);
        }

        await dbm.DBObservableDbSet.LoadAsync(onStartProgressBarVm.StartupCts.Token);

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Сортировка организаций";
        mainWindowViewModel.OnStartProgressBar = 70;
        await ProcessDataBaseFillEmpty(dbm);

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Сортировка примечаний";
        mainWindowViewModel.OnStartProgressBar = 80;
        ReportsStorage.LocalReports = dbm.DBObservableDbSet.Local.First();

        await ProcessDataBaseFillNullOrder();

        onStartProgressBarVm.ThrowIfStartupCancelled();
        onStartProgressBarVm.LoadStatus = "Сохранение";
        mainWindowViewModel.OnStartProgressBar = 90;
        if (dbm.ChangeTracker.HasChanges())
            await dbm.SaveChangesAsync(onStartProgressBarVm.StartupCts.Token);
        ReportsStorage.LocalReports.PropertyChanged += Local_ReportsChanged;

        onStartProgressBarVm.ThrowIfStartupCancelled();
        mainWindowViewModel.OnStartProgressBar = 100;

        var dbPath = StaticConfiguration.DBPath;
        Forms1WarmCache.Instance.PrefetchTabs(
            dbPath,
            MainWindowPagingDefaults.DefaultOrgsPerPage,
            MainWindowPagingDefaults.DefaultFormsPerPage,
            "1.0", "2.0", "4.0", "5.0");

        mainWindowViewModel.Forms1TabControlVM.ActivateTab();

        ScheduleBackgroundTitleCleanup();

        //new CountRowsInAllReportByRegionAndYearCommand().AsyncExecute(null);

    }

    #region Initialization

    #region GetSystemDirectory
    
    /// <summary>
    /// Определение системной директории
    /// </summary>
    private static Task GetSystemDirectory()
    {
        try
        {
            SystemDirectory = GetSystemDirectoryPath();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.System);
        }
        return Task.CompletedTask;
    }

    #endregion

    #region ProcessRaoDirectory
    
    /// <summary>
    /// Определение внутренних подпапок программы
    /// </summary>
    /// <returns></returns>
    private static Task ProcessRaoDirectory()
    {
        try
        {
            RaoDirectory = Path.Combine(SystemDirectory, "RAO");
            LogsDirectory = Path.Combine(RaoDirectory, "logs");
            ReserveDirectory = Path.Combine(RaoDirectory, "reserve");
            TmpDirectory = Path.Combine(RaoDirectory, "temp");
            ConfigDirectory = Path.Combine(RaoDirectory, "config");
            Directory.CreateDirectory(LogsDirectory);
            Directory.CreateDirectory(ReserveDirectory);
            Directory.CreateDirectory(TmpDirectory);
            Directory.CreateDirectory(ConfigDirectory);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.System);
        }

        foreach (var file in Directory.GetFiles(TmpDirectory, "*.*", SearchOption.AllDirectories))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                           $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Warning(msg, ErrorCodeLogger.System);
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region ProcessSpravochniks
    
    /// <summary>
    /// Инициализация справочников
    /// </summary>
    /// <returns></returns>
    private static Task ProcessSpravochniks()
    {
        _ = Spravochniks.SprRadionuclids;
        _ = Spravochniks.SprTypesToRadionuclids;
        _ = Spravochniks.SprRadionuclidRusNames;
        _ = ViewModels.Forms.Forms1.Providers.RadionuclidsProvider.AllRadionuclidNames;
        return Task.CompletedTask;
    }

    #endregion

    #region ProcessDataBaseBackup

    /// <summary>Отсрочка первого напоминания о резервной копии (дни).</summary>
    private const int FirstBackupRemindDays = 7;

    /// <summary>Интервал повторных напоминаний о резервной копии (дни).</summary>
    private const int RegularBackupRemindDays = 30;

    /// <summary>
    /// Первый запуск: инициализация даты бэкапа в настройках (без диалога).
    /// </summary>
    public static void EnsureDatabaseBackupScheduleInitialized()
    {
        if (Settings.Default.AppStartupParameters != string.Empty)
            return;

        var lastBackupDate = Settings.Default.LastDbBackupDate;
        var isUnsetBackupDate = lastBackupDate == default || lastBackupDate == DateTime.MinValue;
        if (!isUnsetBackupDate)
            return;

        Settings.Default.LastDbBackupDate = DateTime.Now;
        Settings.Default.IsFirstAppRun = true;
        Settings.Default.Save();
    }

    /// <summary>
    /// Диалог резервного копирования после открытия главного окна (не блокирует splash).
    /// </summary>
    public static async Task ShowDatabaseBackupPromptIfDueAsync()
    {
        if (!IsDatabaseBackupPromptDue())
            return;

        await ShowDatabaseBackupPromptAsync();
    }

    private static bool IsDatabaseBackupPromptDue()
    {
        if (Settings.Default.AppStartupParameters != string.Empty)
            return false;

        var lastBackupDate = Settings.Default.LastDbBackupDate;
        if (lastBackupDate == default || lastBackupDate == DateTime.MinValue)
            return false;

        var remindAfterDays = Settings.Default.IsFirstAppRun
            ? FirstBackupRemindDays
            : RegularBackupRemindDays;

        return (DateTime.Now - lastBackupDate).TotalDays >= remindAfterDays;
    }

    private static async Task ShowDatabaseBackupPromptAsync()
    {
        var isFirstPrompt = Settings.Default.IsFirstAppRun;
        var lastBackupDate = Settings.Default.LastDbBackupDate;
        var lastBackupTime = isFirstPrompt
            ? string.Empty
            : $" ({lastBackupDate})";
        var contentMessage = isFirstPrompt
            ? $"Рекомендуется создать резервную копию базы данных." +
              $"{Environment.NewLine}Хотите выполнить резервное копирование?"
            : $"Последняя резервная копия базы данных создавалась более месяца назад{lastBackupTime}." +
              $"{Environment.NewLine}Хотите выполнить резервное копирование?";

        var owner = Desktop.MainWindow ?? Desktop.Windows.FirstOrDefault();
        if (owner is null)
            return;

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить в папку по умолчанию", IsDefault = true },
                    new ButtonDefinition { Name = "Выбрать папку и сохранить" },
                    new ButtonDefinition { Name = "Не сохранять", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Резервное копирование",
                ContentMessage = contentMessage,
                MinWidth = 450,
                MinHeight = 150,
                SizeToContent = SizeToContent.Width,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowWindowDialogAsync(owner));

        switch (res)
        {
            case "Сохранить в папку по умолчанию":
            {
                var count = 0;
                string reserveDbPath;
                do
                {
                    reserveDbPath = Path.Combine(ReserveDirectory, DbFileName + $"_{++count}.RAODB");
                } while (File.Exists(reserveDbPath));

                try
                {
                    File.Copy(Path.Combine(RaoDirectory, DbFileName + ".RAODB"), reserveDbPath);
                }
                catch (Exception ex)
                {
                    var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                              $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                    ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
                }
                break;
            }
            case "Выбрать папку и сохранить":
            {
                OpenFolderDialog dial = new() { Directory = ReserveDirectory };
                var folderPath = await dial.ShowAsync(owner);
                if (folderPath is not null)
                {
                    var count = 0;
                    string reserveDbPath;
                    do
                    {
                        reserveDbPath = Path.Combine(folderPath, DbFileName + $"_{++count}.RAODB");
                    } while (File.Exists(reserveDbPath));

                    try
                    {
                        File.Copy(Path.Combine(RaoDirectory, DbFileName + ".RAODB"), reserveDbPath);
                    }
                    catch (Exception ex)
                    {
                        var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                                  $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                        ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
                    }
                }
                break;
            }
        }

        Settings.Default.LastDbBackupDate = DateTime.Now;
        Settings.Default.IsFirstAppRun = false;
        Settings.Default.Save();
    }

    #endregion

    #region CleanUpMasterRep

    private static void LogStartupKostylError(string stage, Exception ex)
    {
        var msg = $"Ошибка на этапе «{stage}» при инициализации (продолжаем запуск)." +
                  $"{Environment.NewLine}Message: {ex.Message}" +
                  $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
        ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
    }

    /// <summary>
    /// Фоновая санитизация титулов 1.0/2.0 после открытия главного окна.
    /// При изменениях в БД — сброс кэшей, пересортировка коллекции, обновление org-гридов.
    /// </summary>
    private void ScheduleBackgroundTitleCleanup()
    {
        var dbPath = StaticConfiguration.DBPath;
        _ = Task.Run(async () =>
        {
            var anyChanged = false;
            try
            {
                anyChanged = await TitleRowSanitizer.CleanUpAsync(dbPath);
            }
            catch (Exception ex)
            {
                LogStartupKostylError("Очистка (фон)", ex);
                return;
            }

            if (!anyChanged)
                return;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Forms1WarmCache.Instance.InvalidateAll();
                ResortLocalReportsCollection();
                mainWindowViewModel.RefreshAllTabsAfterTitleSanitizer();
            });
        });
    }

    /// <summary>
    /// Пересортировка org в <see cref="ReportsStorage.LocalReports"/> по актуальным ключам из БД.
    /// </summary>
    public static void ResortLocalReportsCollection()
    {
        if (ReportsStorage.LocalReports == null)
            return;

        MainWindowListQuery.InvalidateAllOrgKeysCaches();

        var comparator = new CustomReportsComparer();
        var db = StaticConfiguration.DBModel;
        var form10Keys = MainWindowListQuery.GetForm10DisplayKeys(db);
        var form20Keys = MainWindowListQuery.GetForm20DisplayKeys(db);

        var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
        ReportsStorage.LocalReports.Reports_Collection.Clear();
        ReportsStorage.LocalReports.Reports_Collection
            .AddRange(tmpReportsList
                .OrderBy(x => GetSortRegNo(x, form10Keys, form20Keys), comparator)
                .ThenBy(x => GetSortOkpo(x, form10Keys, form20Keys), comparator));
    }

    private static string GetSortRegNo(
        Reports x,
        IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> form10Keys,
        IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> form20Keys)
    {
        var formNum = x.Master_DB?.FormNum_DB;
        if (formNum == "1.0" && form10Keys.TryGetValue(x.Id, out var t10))
            return t10.RegNo;
        if (formNum == "2.0" && form20Keys.TryGetValue(x.Id, out var t20))
            return t20.RegNo;
        // 4.0/5.0: RegNoRep не трогает Rows*. 1.0/2.0 без ключей — не RegNoRep (stub без Rows10/20).
        if (formNum is "4.0" or "5.0")
            return x.Master_DB?.RegNoRep?.Value ?? "";
        return "";
    }

    private static string GetSortOkpo(
        Reports x,
        IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> form10Keys,
        IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> form20Keys)
    {
        var formNum = x.Master_DB?.FormNum_DB;
        if (formNum == "1.0" && form10Keys.TryGetValue(x.Id, out var t10))
            return t10.Okpo;
        if (formNum == "2.0" && form20Keys.TryGetValue(x.Id, out var t20))
            return t20.Okpo;
        if (formNum is "4.0" or "5.0")
            return x.Master_DB?.OkpoRep?.Value ?? "";
        return "";
    }

    #endregion

    /// <summary>
    /// Для отдела — текстовое имя выкладки (1.3.0.11_test5 / 1.3.0.11), иначе AssemblyVersion.
    /// </summary>
    private static string GetVersionLabelForWindowTitle()
    {
        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        if (!Settings.Default.AppLaunchedInNorao)
        {
            return assemblyVersion;
        }

        try
        {
            var state = new LocalUpdateStateStore().Load();
            var installed = NetworkUpdateLabels.FormatInstalled(state);
            if (NetworkUpdateLabels.IsTrackedInstallLabel(installed))
            {
                return installed;
            }

            if (NetworkUpdatePaths.TryParseReleaseFromAppDirectory(null, out var major, out var releaseId))
            {
                var fromPath = NetworkUpdateLabels.FormatVersion(major, releaseId);
                if (!string.IsNullOrWhiteSpace(fromPath))
                {
                    return fromPath;
                }
            }
        }
        catch
        {
            // ignore — в шапке останется номер из сборки
        }

        return assemblyVersion;
    }

    private static string BuildMainWindowTitle(string dbFileName) =>
        $"МПЗФ ver.{GetVersionLabelForWindowTitle()} Текущая база данных - {dbFileName}";

    #region ProcessDataBaseCreate

    /// <summary>
    /// Создание файла БД, либо чтение имеющегося.
    /// </summary>
    private async Task ProcessDataBaseCreate(OnStartProgressBarVM? progressBarVm = null)
    {
        var i = 0;
        var loadDbFileError = false;
        DBModel dbm;
        DirectoryInfo dirInfo = new(RaoDirectory);
        FileInfo dbFileInfo = null;
        var raodbFiles = GetRaodbFiles(dirInfo).OrderByDescending(x => x.LastWriteTime).ToList();

        IEnumerable<FileInfo> filesToTry;
        if (raodbFiles.Count > 1)
        {
            var selectedFile = await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var window = new MultipleRaodbFilesMessageWindow(raodbFiles);
                return window.ShowDialog<FileInfo?>(Desktop.MainWindow);
            });

            if (selectedFile is null)
            {
                Environment.Exit(0);
                throw new InvalidOperationException("Запуск программы отменён пользователем.");
            }

            filesToTry = [selectedFile];
        }
        else
        {
            filesToTry = raodbFiles;
        }

        foreach (var fileInfo in filesToTry)
        {
            try
            {
                if (progressBarVm != null)
                    progressBarVm.LoadStatus = "Открытие базы данных";

                dbFileInfo = fileInfo;
                DbFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                mainWindowViewModel.Current_Db = BuildMainWindowTitle(DbFileName);
                StaticConfiguration.DBPath = fileInfo.FullName;
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;

                #region Test Version

                //var t = await dbm.Database.GetPendingMigrationsAsync();
                //var a = dbm.Database.GetMigrations();
                //var b = await dbm.Database.GetAppliedMigrationsAsync();

                #endregion

                await dbm.MigrateDatabaseAsync();

                return;
            }
            catch (FirebirdSql.Data.FirebirdClient.FbException fbEx)
            {
                loadDbFileError = true;
                var msg = $"{Environment.NewLine}Message: {fbEx.Message}" +
                          $"{Environment.NewLine}StackTrace: {fbEx.StackTrace}" +
                          $"{Environment.NewLine}ErrorCode: {fbEx.ErrorCode}" +
                          $"{Environment.NewLine}SQLSTATE: {fbEx.SQLSTATE}";
                ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
            }
            catch (Exception ex)
            {
                loadDbFileError = true;
                var msg =  $"{Environment.NewLine}Message: {ex.Message}" + 
                           $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
            }
        }

        if (progressBarVm != null)
            progressBarVm.LoadStatus = "Создание базы данных";

        DbFileName = $"Local_{i}";
        mainWindowViewModel.Current_Db = BuildMainWindowTitle(DbFileName);
        StaticConfiguration.DBPath = Path.Combine(RaoDirectory, $"{DbFileName}.RAODB");
        StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
        dbm = StaticConfiguration.DBModel;
        if (loadDbFileError)
        {
            try
            {
                var lastModifiedFile = true;
                var actualReserveFileFullPath = string.Empty;
                foreach (var fileInfo in GetRaodbFiles(dirInfo).OrderByDescending(x => x.LastWriteTime))
                {
                    if (!File.Exists(fileInfo.FullName)) continue;
                    var reserveFileFullPath = Path.Combine(ReserveDirectory, Path.GetFileNameWithoutExtension(fileInfo.Name) + $"_{DateTime.Now.Ticks}.RAODB");
                    if (lastModifiedFile)
                    {
                        actualReserveFileFullPath = reserveFileFullPath;
                        lastModifiedFile = false;
                    }
                    File.Copy(fileInfo.FullName, reserveFileFullPath);
                    File.Delete(fileInfo.FullName);
                }
                
                await dbm.MigrateDatabaseAsync();

                #region MessageFailedToReadFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Ошибка",
                        ContentHeader = "Ошибка при чтении файла .RAODB",
                        ContentMessage = $"Возникла ошибка при чтении файла базы данных (БД)" +
                                         $"{Environment.NewLine}{dbFileInfo.FullName}." +
                                         $"{Environment.NewLine}Файл БД был перемещён по пути " +
                                         $"{Environment.NewLine}{actualReserveFileFullPath}." +
                                         $"{Environment.NewLine}Программа запущена с новым пустым файлом БД" +
                                         $"{Environment.NewLine}{StaticConfiguration.DBPath}." +
                                         $"{Environment.NewLine}Для восстановления данных воспользуйтесь функцией \"Импорт -> из RAODB\"," +
                                         $"{Environment.NewLine}указав путь к резервному файлу.",

                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(Desktop.MainWindow)).GetAwaiter().GetResult(); 

                #endregion
            }
            catch (FirebirdSql.Data.FirebirdClient.FbException fbEx)
            {
                #region MessageFailedToCreateFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Ошибка",
                        ContentHeader = "Ошибка при создании файла .RAODB",
                        ContentMessage = $"Не удалось создать файл базы данных." +
                                         $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(Desktop.MainWindow)).GetAwaiter().GetResult();

                #endregion

                var msg = $"{Environment.NewLine}Message: {fbEx.Message}" +
                          $"{Environment.NewLine}StackTrace: {fbEx.StackTrace}" +
                          $"{Environment.NewLine}ErrorCode: {fbEx.ErrorCode}" +
                          $"{Environment.NewLine}SQLSTATE: {fbEx.SQLSTATE}";
                ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
                Console.WriteLine(fbEx.Message);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                #region MessageFailedToCreateFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Ошибка",
                        ContentHeader = "Ошибка при создании файла .RAODB",
                        ContentMessage = $"Не удалось создать файл базы данных." +
                                         $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(Desktop.MainWindow)).GetAwaiter().GetResult();

                #endregion

                var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                          $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
                ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        try
        {
            await dbm.MigrateDatabaseAsync();
        }
        catch (FirebirdSql.Data.FirebirdClient.FbException fbEx)
        {
            #region MessageFailedToCreateFile

            Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Ошибка",
                    ContentHeader = "Ошибка при создании файла .RAODB",
                    ContentMessage = $"Не удалось создать файл базы данных." +
                                     $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow)).GetAwaiter().GetResult();

            #endregion

            var msg = $"{Environment.NewLine}Message: {fbEx.Message}" +
                      $"{Environment.NewLine}StackTrace: {fbEx.StackTrace}" +
                      $"{Environment.NewLine}ErrorCode: {fbEx.ErrorCode}" +
                      $"{Environment.NewLine}SQLSTATE: {fbEx.SQLSTATE}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
            Console.WriteLine(fbEx.Message);
            Environment.Exit(0);
        }
        catch (Exception ex)
        {

            #region MessageFailedToCreateFile

            Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Ошибка",
                    ContentHeader = "Ошибка при создании файла .RAODB",
                    ContentMessage = $"Не удалось создать файл базы данных." +
                                     $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow)).GetAwaiter().GetResult();

            #endregion
            
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase, filePath: dbFileInfo.FullName);
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }
    }

    #endregion

    #region ProcessDataBaseFillEmpty

    /// <summary>
    /// Создание головных отчётов организации и сортировка
    /// </summary>
    /// <param name="dbm">Контекст</param>
    /// <returns></returns>
    public static async Task ProcessDataBaseFillEmpty(DataContext dbm)
    {
        try
        {
            await ProcessDataBaseFillEmptyCore(dbm);
        }
        catch (Exception ex)
        {
            LogStartupKostylError("Сортировка организаций", ex);
        }
    }

    private static async Task ProcessDataBaseFillEmptyCore(DataContext dbm)
    {
        if (!dbm.DBObservableDbSet.Any()) dbm.DBObservableDbSet.Add(new DBObservable());

        var masterIds10 = new List<int>();
        var masterIds20 = new List<int>();
        var masterIds40 = new List<int>();
        var masterIds50 = new List<int>();

        foreach (var item in dbm.DBObservableDbSet)
        {
            foreach (var key in item.Reports_Collection)
            {
                var it = (Reports)key;
                if (it.Master_DB is null) continue;
                switch (it.Master_DB.FormNum_DB)
                {
                    case "1.0": masterIds10.Add(it.Master_DB.Id); break;
                    case "2.0": masterIds20.Add(it.Master_DB.Id); break;
                    case "4.0": masterIds40.Add(it.Master_DB.Id); break;
                    case "5.0": masterIds50.Add(it.Master_DB.Id); break;
                }
            }
        }

        // Только master-id org из коллекции — без полного DISTINCT по form_*.
        var masterIdsWithForm10 = LoadExistingTitleMasterIds(dbm, masterIds10, formNum: 10);
        var masterIdsWithForm20 = LoadExistingTitleMasterIds(dbm, masterIds20, formNum: 20);
        var masterIdsWithForm40 = LoadExistingTitleMasterIds(dbm, masterIds40, formNum: 40);
        var masterIdsWithForm50 = LoadExistingTitleMasterIds(dbm, masterIds50, formNum: 50);

        foreach (var item in dbm.DBObservableDbSet)
        {
            foreach (var key in item.Reports_Collection)
            {
                var it = (Reports)key;
                if (it.Master_DB is null) continue;
                if (it.Master_DB.FormNum_DB == "") continue;

                if (it.Master_DB.FormNum_DB == "1.0"
                    && !masterIdsWithForm10.Contains(it.Master_DB.Id)
                    && it.Master_DB.Rows10.Count == 0)
                {
                    var ty1 = (Form10)FormCreator.Create("1.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create("1.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows10.Add(ty1);
                    it.Master_DB.Rows10.Add(ty2);
                }

                if (it.Master_DB.FormNum_DB == "2.0"
                    && !masterIdsWithForm20.Contains(it.Master_DB.Id)
                    && it.Master_DB.Rows20.Count == 0)
                {
                    var ty1 = (Form20)FormCreator.Create("2.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create("2.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows20.Add(ty1);
                    it.Master_DB.Rows20.Add(ty2);
                }
                if (it.Master_DB.FormNum_DB == "4.0"
                    && !masterIdsWithForm40.Contains(it.Master_DB.Id)
                    && it.Master_DB.Rows40.Count == 0)
                {
                    var ty = (Form40)FormCreator.Create("4.0");
                    ty.NumberInOrder_DB = 1;
                    it.Master_DB.Rows40.Add(ty);
                }
                if (it.Master_DB.FormNum_DB == "5.0"
                    && !masterIdsWithForm50.Contains(it.Master_DB.Id)
                    && it.Master_DB.Rows50.Count == 0)
                {
                    var ty = (Form50)FormCreator.Create("5.0");
                    ty.NumberInOrder_DB = 1;
                    it.Master_DB.Rows50.Add(ty);
                }

                if (it.Master_DB.Rows10.Count > 0)
                {
                    it.Master_DB.Rows10.Sorted = false;
                    await it.Master_DB.Rows10.QuickSortAsync();
                }

                it.Master_DB.Rows20.Sorted = false;
                it.Master_DB.Rows40.Sorted = false;
                it.Master_DB.Rows50.Sorted = false;
                await it.Master_DB.Rows20.QuickSortAsync();
                await it.Master_DB.Rows40.QuickSortAsync();
                await it.Master_DB.Rows50.QuickSortAsync();
            }
        }
    }

    private static HashSet<int> LoadExistingTitleMasterIds(DataContext dbm, List<int> masterIds, int formNum)
    {
        if (masterIds.Count == 0)
            return [];

        var distinct = masterIds.Distinct().ToList();
        var db = (DBModel)dbm;
        var result = new HashSet<int>();

        foreach (var batch in FirebirdInClause.Chunk(distinct))
        {
            List<int> batchIds = formNum switch
            {
                10 => db.form_10.AsNoTracking()
                    .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                    .Select(f => f.ReportId!.Value)
                    .ToList(),
                20 => db.form_20.AsNoTracking()
                    .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                    .Select(f => f.ReportId!.Value)
                    .ToList(),
                40 => db.form_40.AsNoTracking()
                    .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                    .Select(f => f.ReportId!.Value)
                    .ToList(),
                50 => db.form_50.AsNoTracking()
                    .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                    .Select(f => f.ReportId!.Value)
                    .ToList(),
                _ => []
            };

            foreach (var id in batchIds)
                result.Add(id);
        }

        return result;
    }

    #endregion

    #region ProcessDataBaseFillNullOrder
    
    /// <summary>
    /// Выставление порядкового номера и сортировка
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessDataBaseFillNullOrder()
    {
        try
        {
            await ProcessDataBaseFillNullOrderCore();
        }
        catch (Exception ex)
        {
            LogStartupKostylError("Сортировка примечаний", ex);
        }
    }

    private static async Task ProcessDataBaseFillNullOrderCore()
    {
        var db = StaticConfiguration.DBModel;
        var zeroOrderNotes = await db.notes
            .Where(n => n.Order == 0 && n.ReportId != null)
            .ToListAsync();

        if (zeroOrderNotes.Count > 0)
        {
            var reportIds = zeroOrderNotes.Select(n => n.ReportId!.Value).Distinct().ToList();
            foreach (var batch in FirebirdInClause.Chunk(reportIds))
            {
                var maxOrders = await db.notes
                    .Where(n => n.ReportId != null && batch.Contains(n.ReportId.Value))
                    .GroupBy(n => n.ReportId!.Value)
                    .Select(g => new { ReportId = g.Key, MaxOrder = g.Max(x => x.Order) })
                    .ToListAsync();
                var maxMap = maxOrders.ToDictionary(x => x.ReportId, x => x.MaxOrder);
                foreach (var note in zeroOrderNotes.Where(n => batch.Contains(n.ReportId!.Value)))
                {
                    var next = maxMap.GetValueOrDefault(note.ReportId!.Value, 0) + 1;
                    note.Order = next;
                    maxMap[note.ReportId!.Value] = next;
                }
            }

            ResortLocalReportsCollection();
        }
    }

    #endregion

    #region GetNumberInOrder

    /// <summary>
    /// Получить порядковый номер
    /// </summary>
    /// <param name="lst">Список элементов</param>
    /// <returns></returns>
    public static int GetNumberInOrder(IEnumerable lst)
    {
        var maxNum = 0;
        foreach (var item in lst)
        {
            var frm = (INumberInOrder)item;
            if (frm.Order >= maxNum)
            {
                maxNum++;
            }
        }

        return maxNum + 1;
    }

    #endregion

    #region Local_ReportsChanged

    /// <summary>
    /// PropertyChanged локального списка организаций
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
    {
        mainWindowViewModel.OnPropertyChanged(nameof(ReportsStorage.LocalReports));
    }

    #endregion

    #endregion
}