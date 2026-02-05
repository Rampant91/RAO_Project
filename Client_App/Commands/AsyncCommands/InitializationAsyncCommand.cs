using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Properties;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        onStartProgressBarVm!.LoadStatus = "Поиск системной директории";
        mainWindowViewModel.OnStartProgressBar = 1;
        await GetSystemDirectory();

        onStartProgressBarVm.LoadStatus = "Создание временных файлов";
        mainWindowViewModel.OnStartProgressBar = 5;
        await ProcessRaoDirectory();

        onStartProgressBarVm.LoadStatus = "Загрузка справочников";
        mainWindowViewModel.OnStartProgressBar = 10;
        await ProcessSpravochniks();

        onStartProgressBarVm.LoadStatus = "Создание базы данных";
        mainWindowViewModel.OnStartProgressBar = 15;
        await ProcessDataBaseCreate();

        onStartProgressBarVm.LoadStatus = "Очистка";
        mainWindowViewModel.OnStartProgressBar = 17;
        await CleanUpMasterRep();

        onStartProgressBarVm.LoadStatus = "Создание резервной копии БД";
        mainWindowViewModel.OnStartProgressBar = 18;
        await ProcessDataBaseBackup();

        onStartProgressBarVm.LoadStatus = "Загрузка таблиц";
        mainWindowViewModel.OnStartProgressBar = 20;
        var dbm = StaticConfiguration.DBModel;

        #region LoadTables

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.0";
        mainWindowViewModel.OnStartProgressBar = 25;
        await dbm.form_10.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.0";
        mainWindowViewModel.OnStartProgressBar = 35;
        await dbm.form_20.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 4.0";
        mainWindowViewModel.OnStartProgressBar = 45;
        await dbm.form_40.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 5.0";
        mainWindowViewModel.OnStartProgressBar = 55;
        await dbm.form_50.LoadAsync();

        try
        {
            onStartProgressBarVm.LoadStatus = "Загрузка коллекций отчетов";
            mainWindowViewModel.OnStartProgressBar = 72;
            await dbm.ReportCollectionDbSet.LoadAsync();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" + 
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций организаций";
        mainWindowViewModel.OnStartProgressBar = 74;
        await dbm.ReportsCollectionDbSet.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций базы";
        mainWindowViewModel.OnStartProgressBar = 76;
        if (!dbm.DBObservableDbSet.Any())
        {
            dbm.DBObservableDbSet.Add(new DBObservable());
            dbm.DBObservableDbSet.Local.First().Reports_Collection.AddRange(dbm.ReportsCollectionDbSet);
        }

        await dbm.DBObservableDbSet.LoadAsync();

        #endregion

        onStartProgressBarVm.LoadStatus = "Сортировка организаций";
        mainWindowViewModel.OnStartProgressBar = 80;
        await ProcessDataBaseFillEmpty(dbm);

        onStartProgressBarVm.LoadStatus = "Сортировка примечаний";
        mainWindowViewModel.OnStartProgressBar = 85;
        ReportsStorage.LocalReports = dbm.DBObservableDbSet.Local.First();

        await ProcessDataBaseFillNullOrder();

        onStartProgressBarVm.LoadStatus = "Сохранение";
        mainWindowViewModel.OnStartProgressBar = 90;
        await dbm.SaveChangesAsync();
        ReportsStorage.LocalReports.PropertyChanged += Local_ReportsChanged;

        mainWindowViewModel.OnStartProgressBar = 100;
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
            SystemDirectory = Settings.Default.SystemFolderDefaultPath is "default"
                ? OperatingSystem.IsWindows()
                    ? Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!
                    : SystemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                : Settings.Default.SystemFolderDefaultPath;
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

        var fl = Directory.GetFiles(TmpDirectory, ".");
        foreach (var file in fl)
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
        var a = Spravochniks.SprRadionuclids;
        var b = Spravochniks.SprTypesToRadionuclids;
        return Task.CompletedTask;
    }

    #endregion

    #region ProcessDataBaseBackup

    /// <summary>
    /// Создание резервной копии БД раз в месяц.
    /// </summary>
    private static async Task ProcessDataBaseBackup()
    {
        //Settings.Default.LastDbBackupDate = DateTime.MinValue;    //Сброс даты для тестирования
        //Settings.Default.Save();

        if (Settings.Default.IsFirstAppRun)
        {
            Settings.Default.LastDbBackupDate = DateTime.Now;
            Settings.Default.IsFirstAppRun = false;
            Settings.Default.Save();
            return;
        }

        if ((DateTime.Now - Settings.Default.LastDbBackupDate).TotalDays < 30
            || Settings.Default.AppStartupParameters != string.Empty)
        {
            return;
        }

        #region MessageInputCategoryNums

        var lastBackupTime = Settings.Default.LastDbBackupDate == DateTime.MinValue
            ? string.Empty
            : $" ({Settings.Default.LastDbBackupDate})";
        var res = Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxInputParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить в папку по умолчанию", IsDefault = true },
                    new ButtonDefinition { Name = "Выбрать папку и сохранить" },
                    new ButtonDefinition { Name = "Не сохранять", IsCancel = true }
                ],
                CanResize = true,
                ContentTitle = "Резервное копирование",
                ContentMessage = $"Последняя резервная копия базы данных создавалась более месяца назад{lastBackupTime}." +
                                 $"{Environment.NewLine}Хотите выполнить резервное копирование?",
                MinWidth = 450,
                MinHeight = 150,
                SizeToContent = SizeToContent.Width,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.Windows[0])).GetAwaiter().GetResult();

        #endregion

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
                Settings.Default.LastDbBackupDate = DateTime.Now;
                Settings.Default.Save();
                break;
            }
            case "Выбрать папку и сохранить":
            {
                OpenFolderDialog dial = new() { Directory = ReserveDirectory };
                var folderPath = dial.ShowAsync(Desktop.Windows[0]).GetAwaiter().GetResult();
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
                Settings.Default.LastDbBackupDate = DateTime.Now;
                Settings.Default.Save();
                break;
            }
        }
    }

    #endregion

    #region CleanUpMasterRep

    private static async Task CleanUpMasterRep()
    {
        await using var db = new DBModel(StaticConfiguration.DBPath);

        var masterRepRows10List = db.ReportsCollectionDbSet
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Where(x => x.DBObservable != null)
            .SelectMany(x => x.Master_DB.Rows10)
            .ToList();

        var masterRepRows20List = db.ReportsCollectionDbSet
            .Include(x => x.DBObservable)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Where(x => x.DBObservable != null)
            .SelectMany(x => x.Master_DB.Rows20)
            .ToList();

        foreach (var form10 in masterRepRows10List)
        {
            form10.RegNo_DB = CustomTrim(form10.RegNo_DB);
            form10.OrganUprav_DB = CustomTrim(form10.OrganUprav_DB);
            form10.SubjectRF_DB = CustomTrim(form10.SubjectRF_DB);
            form10.JurLico_DB = CustomTrim(form10.JurLico_DB);
            form10.ShortJurLico_DB = CustomTrim(form10.ShortJurLico_DB);
            form10.JurLicoAddress_DB = CustomTrim(form10.JurLicoAddress_DB);
            form10.JurLicoFactAddress_DB = CustomTrim(form10.JurLicoFactAddress_DB);
            form10.GradeFIO_DB = CustomTrim(form10.GradeFIO_DB);
            form10.Telephone_DB = CustomTrim(form10.Telephone_DB);
            form10.Fax_DB = CustomTrim(form10.Fax_DB);
            form10.Email_DB = CustomTrim(form10.Email_DB);
            form10.Okpo_DB = CustomTrim(form10.Okpo_DB);
            form10.Okved_DB = CustomTrim(form10.Okved_DB);
            form10.Okogu_DB = CustomTrim(form10.Okogu_DB);
            form10.Oktmo_DB = CustomTrim(form10.Oktmo_DB);
            form10.Inn_DB = CustomTrim(form10.Inn_DB);
            form10.Kpp_DB = CustomTrim(form10.Kpp_DB);
            form10.Okopf_DB = CustomTrim(form10.Okopf_DB);
            form10.Okfs_DB = CustomTrim(form10.Okfs_DB);
        }
        foreach (var form20 in masterRepRows20List)
        {
            form20.RegNo_DB = CustomTrim(form20.RegNo_DB);
            form20.OrganUprav_DB = CustomTrim(form20.OrganUprav_DB);
            form20.SubjectRF_DB = CustomTrim(form20.SubjectRF_DB);
            form20.JurLico_DB = CustomTrim(form20.JurLico_DB);
            form20.ShortJurLico_DB = CustomTrim(form20.ShortJurLico_DB);
            form20.JurLicoAddress_DB = CustomTrim(form20.JurLicoAddress_DB);
            form20.JurLicoFactAddress_DB = CustomTrim(form20.JurLicoFactAddress_DB);
            form20.GradeFIO_DB = CustomTrim(form20.GradeFIO_DB);
            form20.Telephone_DB = CustomTrim(form20.Telephone_DB);
            form20.Fax_DB = CustomTrim(form20.Fax_DB);
            form20.Email_DB = CustomTrim(form20.Email_DB);
            form20.Okpo_DB = CustomTrim(form20.Okpo_DB);
            form20.Okved_DB = CustomTrim(form20.Okved_DB);
            form20.Okogu_DB = CustomTrim(form20.Okogu_DB);
            form20.Oktmo_DB = CustomTrim(form20.Oktmo_DB);
            form20.Inn_DB = CustomTrim(form20.Inn_DB);
            form20.Kpp_DB = CustomTrim(form20.Kpp_DB);
            form20.Okopf_DB = CustomTrim(form20.Okopf_DB);
            form20.Okfs_DB = CustomTrim(form20.Okfs_DB);
        }
        await db.SaveChangesAsync();
    }

    private static string CustomTrim(string? str)
    {
        if (string.IsNullOrEmpty(str)) return string.Empty;

        // Use Span to avoid allocations
        var span = str.AsSpan();

        // Trim leading and trailing whitespace
        span = span.Trim();

        // Allocate a buffer to build the result
        var buffer = new char[span.Length];
        var bufferIndex = 0;

        // Iterate through the span and skip newline characters
        foreach (var currentChar in span)
        {
            if (currentChar != '\r' && currentChar != '\n')
            {
                buffer[bufferIndex++] = currentChar;
            }
        }

        // Return the new string with the correct length
        return new string(buffer, 0, bufferIndex);
    }

    #endregion

    #region ProcessDataBaseCreate

    /// <summary>
    /// Создание файла БД, либо чтение имеющегося
    /// </summary>
    /// <returns></returns>
    private async Task ProcessDataBaseCreate()
    {
        var i = 0;
        var loadDbFileError = false;
        DBModel dbm;
        DirectoryInfo dirInfo = new(RaoDirectory);
        FileInfo dbFileInfo = null;
        foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                     .Where(x => x.Name.ToLower().EndsWith(".raodb"))
                     .OrderByDescending(x => x.LastWriteTime))
        {
            try
            {
                dbFileInfo = fileInfo;
                DbFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                mainWindowViewModel.Current_Db =
                    $"МПЗФ ver.{Assembly.GetExecutingAssembly().GetName().Version} Текущая база данных - {DbFileName}";
                StaticConfiguration.DBPath = fileInfo.FullName;
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;

                #region Test Version

                //var t = await dbm.Database.GetPendingMigrationsAsync();
                //var a = dbm.Database.GetMigrations();
                //var b = await dbm.Database.GetAppliedMigrationsAsync();

                #endregion

                await dbm.Database.MigrateAsync();

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
        DbFileName = $"Local_{i}";
        mainWindowViewModel.Current_Db = $"МПЗФ ver.{Assembly.GetExecutingAssembly().GetName().Version} " +
                                         $"Текущая база данных - {DbFileName}";
        StaticConfiguration.DBPath = Path.Combine(RaoDirectory, $"{DbFileName}.RAODB");
        StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
        dbm = StaticConfiguration.DBModel;
        if (loadDbFileError)
        {
            try
            {
                var lastModifiedFile = true;
                var actualReserveFileFullPath = string.Empty;
                foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                             .Where(x => x.Name.ToLower().EndsWith(".raodb"))
                             .OrderByDescending(x => x.LastWriteTime))
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
                
                await dbm.Database.MigrateAsync();

                #region MessageFailedToReadFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Ошибка при чтении файла .RAODB",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Возникла ошибка при чтении файла базы данных (БД)" +
                                         $"{Environment.NewLine}{dbFileInfo.FullName}." +
                                         $"{Environment.NewLine}Файл БД был перемещён по пути " +
                                         $"{Environment.NewLine}{actualReserveFileFullPath}." +
                                         $"{Environment.NewLine}Программа запущена с новым пустым файлом БД" +
                                         $"{Environment.NewLine}{StaticConfiguration.DBPath}." +
                                         $"{Environment.NewLine}Для восстановления данных воспользуйтель функцией \"Импорт -> из RAODB\"," +
                                         $"{Environment.NewLine}указав путь к резервному файлу.",

                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult(); 

                #endregion
            }
            catch (FirebirdSql.Data.FirebirdClient.FbException fbEx)
            {
                #region MessageFailedToCreateFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Не удалось создать файл базы данных." +
                                         $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult();

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

                Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Не удалось создать файл базы данных." +
                                         $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult();

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
            await dbm.Database.MigrateAsync();
        }
        catch (FirebirdSql.Data.FirebirdClient.FbException fbEx)
        {
            #region MessageFailedToCreateFile

            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Не удалось создать файл базы данных." +
                                     $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult();

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

            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Не удалось создать файл базы данных." +
                                     $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult();

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
        if (!dbm.DBObservableDbSet.Any()) dbm.DBObservableDbSet.Add(new DBObservable());
        foreach (var item in dbm.DBObservableDbSet)
        {
            foreach (var key in item.Reports_Collection)
            {
                var it = (Reports)key;
                if (it.Master_DB.FormNum_DB == "") continue;
                if (it.Master_DB.Rows10.Count == 0)
                {
                    var ty1 = (Form10)FormCreator.Create("1.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create("1.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows10.Add(ty1);
                    it.Master_DB.Rows10.Add(ty2);
                }

                if (it.Master_DB.Rows20.Count == 0)
                {
                    var ty1 = (Form20)FormCreator.Create("2.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create("2.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows20.Add(ty1);
                    it.Master_DB.Rows20.Add(ty2);
                }
                if (it.Master_DB.Rows40.Count == 0)
                {
                    var ty = (Form40)FormCreator.Create("4.0");
                    ty.NumberInOrder_DB = 1;
                    it.Master_DB.Rows40.Add(ty);
                }
                if (it.Master_DB.Rows50.Count == 0)
                {
                    var ty = (Form50)FormCreator.Create("5.0");
                    ty.NumberInOrder_DB = 1;
                    it.Master_DB.Rows50.Add(ty);
                }

                //if (it.Master_DB.Rows40.Count == 0)
                //{
                //    var ty1 = (Form40)FormCreator.Create("4.0");
                //    ty1.NumberInOrder_DB = 1;
                //    var ty2 = (Form40)FormCreator.Create("4.0");
                //    ty2.NumberInOrder_DB = 2;
                //    it.Master_DB.Rows40.Add(ty1);
                //    it.Master_DB.Rows40.Add(ty2);
                //}

                it.Master_DB.Rows10.Sorted = false;
                it.Master_DB.Rows20.Sorted = false;
                it.Master_DB.Rows40.Sorted = false;
                it.Master_DB.Rows50.Sorted = false;
                //it.Master_DB.Rows40.Sorted = false;
                await it.Master_DB.Rows10.QuickSortAsync();
                await it.Master_DB.Rows20.QuickSortAsync();
                await it.Master_DB.Rows40.QuickSortAsync();
                await it.Master_DB.Rows50.QuickSortAsync();
                //await it.Master_DB.Rows40.QuickSortAsync();
            }
        }
    }

    #endregion

    #region ProcessDataBaseFillNullOrder
    
    /// <summary>
    /// Выставление порядкового номера и сортировка
    /// </summary>
    /// <returns></returns>
    private static async Task ProcessDataBaseFillNullOrder()
    {
        foreach (var key in ReportsStorage.LocalReports.Reports_Collection)
        {
            var item = (Reports)key;
            foreach (var key1 in item.Report_Collection)
            {
                var it = (Report)key1;
                foreach (var key2 in it.Notes)
                {
                    var i = (Note)key2;
                    if (i.Order == 0)
                    {
                        i.Order = GetNumberInOrder(it.Notes);
                    }
                }
            }

            await item.SortAsync();
        }

        
        var comparator = new CustomReportsComparer();
        var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
        ReportsStorage.LocalReports.Reports_Collection.Clear();
        ReportsStorage.LocalReports.Reports_Collection
            .AddRange(tmpReportsList
                .OrderBy(x => x.Master_DB?.RegNoRep?.Value, comparator)
                .ThenBy(x => x.Master_DB?.OkpoRep?.Value, comparator));

        //await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
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