using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Forms;
using Models.Interfaces;
using Spravochniki;
using Microsoft.EntityFrameworkCore;
using static Client_App.ViewModels.BaseVM;
using System.Reflection;

namespace Client_App.Commands.AsyncCommands;

public class InitializationAsyncCommand(MainWindowVM mainWindowViewModel) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var onStartProgressBarVm = parameter as OnStartProgressBarVM;
        onStartProgressBarVm!.LoadStatus = "Поиск системной директории";
        mainWindowViewModel.OnStartProgressBar = 1;
        GetSystemDirectory();

        onStartProgressBarVm.LoadStatus = "Создание временных файлов";
        mainWindowViewModel.OnStartProgressBar = 5;
        await ProcessRaoDirectory();

        onStartProgressBarVm.LoadStatus = "Загрузка справочников";
        mainWindowViewModel.OnStartProgressBar = 10;
        await ProcessSpravochniks();

        onStartProgressBarVm.LoadStatus = "Создание базы данных";
        mainWindowViewModel.OnStartProgressBar = 15;
        await ProcessDataBaseCreate();

        onStartProgressBarVm.LoadStatus = "Загрузка таблиц";
        mainWindowViewModel.OnStartProgressBar = 20;
        var dbm = StaticConfiguration.DBModel;

        #region LoadTables

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.0";
        mainWindowViewModel.OnStartProgressBar = 24;
        await dbm.form_10.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.0";
        mainWindowViewModel.OnStartProgressBar = 45;
        await dbm.form_20.LoadAsync();

        try
        {
            onStartProgressBarVm.LoadStatus = "Загрузка коллекций отчетов";
            mainWindowViewModel.OnStartProgressBar = 72;
            await dbm.ReportCollectionDbSet.LoadAsync();
        }
        catch (Exception ex)
        {
            //ignore
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
    
    private static void GetSystemDirectory()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                SystemDirectory = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!;
            }
            if (OperatingSystem.IsLinux())
            {
                SystemDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
        }
        catch (Exception e)
        {
            //ignore
        }
    }

    #endregion

    #region ProcessRaoDirectory
    
    private static Task ProcessRaoDirectory()
    {
        try
        {
            RaoDirectory = Path.Combine(SystemDirectory, "RAO");
            LogsDirectory = Path.Combine(RaoDirectory, "logs");
            TmpDirectory = Path.Combine(RaoDirectory, "temp");
            Directory.CreateDirectory(LogsDirectory);
            Directory.CreateDirectory(TmpDirectory);
        }
        catch (Exception e)
        {
            //ignore
        }

        var fl = Directory.GetFiles(TmpDirectory, ".");
        foreach (var file in fl)
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                // ignored
            }
        }

        return Task.CompletedTask;
    }

    #endregion

    #region ProcessSpravochniks
    
    private static Task ProcessSpravochniks()
    {
        var a = Spravochniks.SprRadionuclids;
        var b = Spravochniks.SprTypesToRadionuclids;
        return Task.CompletedTask;
    }

    #endregion

    #region ProcessDataBaseCreate
    
    private async Task ProcessDataBaseCreate()
    {
        var i = 0;
        var loadDbFileError = false;
        DBModel dbm;
        DirectoryInfo dirInfo = new(RaoDirectory);
        FileInfo dbFileInfo = null;
        foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                     .Where(x => x.Name.ToLower().EndsWith(".raodb"))
                     .OrderByDescending((x => x.LastWriteTime)))
        {
            try
            {
                dbFileInfo = fileInfo;
                DbFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                mainWindowViewModel.Current_Db = $"Интерактивное пособие по вводу данных ver.{Assembly.GetExecutingAssembly().GetName().Version} Текущая база данных - {DbFileName}";
                StaticConfiguration.DBPath = fileInfo.FullName;
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;
                await dbm.Database.MigrateAsync();
                return;
            }
            catch
            {
                loadDbFileError = true;
            }
        }
        DbFileName = $"Local_{i}";
        mainWindowViewModel.Current_Db = $"Интерактивное пособие по вводу данных ver.{Assembly.GetExecutingAssembly().GetName().Version} Текущая база данных - {DbFileName}";
        StaticConfiguration.DBPath = Path.Combine(RaoDirectory, $"{DbFileName}.RAODB");
        StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
        dbm = StaticConfiguration.DBModel;
        if (loadDbFileError)
        {
            try
            {
                var reservePath = Path.Combine(RaoDirectory, "reserve");
                Directory.CreateDirectory(reservePath);
                foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                             .Where(x => x.Name.ToLower().EndsWith(".raodb")))
                {
                    if (!File.Exists(fileInfo.FullName)) continue;
                    File.Copy(fileInfo.FullName, Path.Combine(reservePath, Path.GetFileNameWithoutExtension(fileInfo.Name)) + $@"{DateTime.Now.Ticks}.RAODB");
                    File.Delete(fileInfo.FullName);
                }
                
                await dbm.Database.MigrateAsync();

                #region MessageFailedToReadFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Ошибка при чтении файла .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Не удалось прочесть файл базы данных " +
                                         $"{Environment.NewLine}{dbFileInfo.FullName}. Файл поврежден." +
                                         $"{Environment.NewLine}Программа запущена с новым пустым файлом базы данных" +
                                         $"{Environment.NewLine}{StaticConfiguration.DBPath}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult(); 

                #endregion
            }
            catch (Exception e)
            {
                #region MessageFailedToCreateFile

                Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Создание файла .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Не удалось создать файл базы данных." +
                                         $"{Environment.NewLine}При установке(настройке) программы возникла ошибка.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)).GetAwaiter().GetResult(); 

                #endregion

                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        try
        {
            await dbm.Database.MigrateAsync();
        }
        catch (Exception e)
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

            Console.WriteLine(e.Message);
            Environment.Exit(0);
        }
    }

    #endregion

    #region ProcessDataBaseFillEmpty

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

                it.Master_DB.Rows10.Sorted = false;
                it.Master_DB.Rows20.Sorted = false;
                await it.Master_DB.Rows10.QuickSortAsync();
                await it.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }

    #endregion

    #region ProcessDataBaseFillNullOrder
    
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

        await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync().ConfigureAwait(false);
    }

    #endregion

    #region GetNumberInOrder

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
    
    private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
    {
        mainWindowViewModel.OnPropertyChanged(nameof(ReportsStorage.LocalReports));
    }

    #endregion

    #endregion
}
