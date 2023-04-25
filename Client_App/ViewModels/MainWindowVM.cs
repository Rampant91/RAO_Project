using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Client_App.VisualRealization.Long_Visual;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using OfficeOpenXml;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Excel;

namespace Client_App.ViewModels;

public class MainWindowVM : BaseVM, INotifyPropertyChanged
{
    #region Current_Db

    private string _current_Db = "";

    public string Current_Db
    {
        get => _current_Db;
        set
        {
            if (_current_Db != value)
            {
                _current_Db = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region LocalReports

    private static DBObservable _localReports = new();
    public static DBObservable LocalReports
    {
        get => _localReports;
        set
        {
            if (_localReports != value)
            {
                _localReports = value;
            }
        }
    }

    #endregion

    #region Init

    private async Task<string> GetSystemDirectory()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                SystemDirectory = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!;
                return SystemDirectory;
            }

            //var userName = await RunCommandInBush("logname");
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "bash",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            process.StandardInput.WriteLine("logname");
            var userName = process.StandardOutput.ReadLine();
            SystemDirectory = Path.Combine("/home", userName!);
            return SystemDirectory;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error1);
            throw new Exception(ErrorMessages.Error1[0]);
        }
    }

    private async Task ProcessRaoDirectory()
    {
        try
        {
            RaoDirectory = Path.Combine(SystemDirectory, "RAO");
            TmpDirectory = Path.Combine(RaoDirectory, "temp");
            Directory.CreateDirectory(TmpDirectory);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error2);
            throw new Exception(ErrorMessages.Error2[0]);
        }
        var fl = Directory.GetFiles(TmpDirectory, ".");
        foreach (var file in fl)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                //await ShowMessage.Handle(ErrorMessages.Error3);
                //throw new Exception(ErrorMessages.Error3[0]);
            }
        }
    }

    private static Task ProcessSpravochniks()
    {
        var a = Spravochniks.SprRadionuclids;
        var b = Spravochniks.SprTypesToRadionuclids;
        return Task.CompletedTask;
    }

    private async Task ProcessDataBaseCreate()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var i = 0;
        var loadDbFile = false;
        DBModel dbm;
        DirectoryInfo dirInfo = new(RaoDirectory);
        foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                     .Where(x => x.Name.ToLower().EndsWith(".raodb"))
                     .OrderByDescending((x => x.LastWriteTime)))
        {
            try
            {
                DbFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                Current_Db = $"Интерактивное пособие по вводу данных ver.{Version} Текущая база данных - {DbFileName}";
                StaticConfiguration.DBPath = fileInfo.FullName;
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;
                await dbm.Database.MigrateAsync();
                loadDbFile = true;
                break;
            }
            catch (Exception)
            {
                //ignored
            }
        }

        if (!loadDbFile) //Если не прочитали файл базы, то создаем пустой.
        {
            DbFileName = $"Local_{i}";
            Current_Db = $"Интерактивное пособие по вводу данных ver.{Version} Текущая база данных - {DbFileName}";
            StaticConfiguration.DBPath = Path.Combine(RaoDirectory, $"{DbFileName}.RAODB");
            StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
            dbm = StaticConfiguration.DBModel;
            await dbm.Database.MigrateAsync();
        }
    }

    private static async Task ProcessDataBaseFillEmpty(DataContext dbm)
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

    private async Task ProcessDataBaseFillNullOrder()
    {
        foreach (var key in LocalReports.Reports_Collection)
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

        await LocalReports.Reports_Collection.QuickSortAsync();
    }

    private Task PropertiesInit()
    {
        ImportForm = ReactiveCommand.CreateFromTask(_ImportForm);
        ImportFrom = ReactiveCommand.CreateFromTask(_ImportFrom);
        ExportForm = ReactiveCommand.CreateFromTask<object>(_ExportForm);
        ExportOrg = ReactiveCommand.CreateFromTask<object>(_ExportOrg);
        ExportAllOrg = ReactiveCommand.CreateFromTask<object>(_ExportAllOrg);
        ExportOrgWithDateRange = ReactiveCommand.CreateFromTask<object>(_ExportOrgWithDateRange);

        return Task.CompletedTask;
    }

    private static int GetNumberInOrder(IEnumerable lst)
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

    public async Task Init(OnStartProgressBarVM onStartProgressBarVm)
    {
        onStartProgressBarVm.LoadStatus = "Поиск системной директории";
        OnStartProgressBar = 1;
        await GetSystemDirectory();

        onStartProgressBarVm.LoadStatus = "Создание временных файлов";
        OnStartProgressBar = 5;
        await ProcessRaoDirectory();

        onStartProgressBarVm.LoadStatus = "Загрузка справочников";
        OnStartProgressBar = 10;
        await ProcessSpravochniks();

        onStartProgressBarVm.LoadStatus = "Создание базы данных";
        OnStartProgressBar = 15;
        await ProcessDataBaseCreate();
        
        onStartProgressBarVm.LoadStatus = "Загрузка таблиц";
        OnStartProgressBar = 20;
        var dbm = StaticConfiguration.DBModel;

        #region LoadTables
        onStartProgressBarVm.LoadStatus = "Загрузка примечаний";
        OnStartProgressBar = 22;
        await dbm.notes.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.0";
        OnStartProgressBar = 24;
        await dbm.form_10.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.1";
        OnStartProgressBar = 26;
        await dbm.form_11.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.2";
        OnStartProgressBar = 28;
        await dbm.form_12.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.3";
        OnStartProgressBar = 30;
        await dbm.form_13.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.4";
        OnStartProgressBar = 32;
        await dbm.form_14.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.5";
        OnStartProgressBar = 34;
        await dbm.form_15.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.6";
        OnStartProgressBar = 36;
        await dbm.form_16.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.7";
        OnStartProgressBar = 38;
        await dbm.form_17.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.8";
        OnStartProgressBar = 40;
        await dbm.form_18.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.9";
        OnStartProgressBar = 42;
        await dbm.form_19.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.0";
        OnStartProgressBar = 45;
        await dbm.form_20.LoadAsync();
        try
        {

            onStartProgressBarVm.LoadStatus = "Загрузка форм 2.1";
            OnStartProgressBar = 48;
            await dbm.form_21.LoadAsync();

            onStartProgressBarVm.LoadStatus = "Загрузка форм 2.2";
            OnStartProgressBar = 50;
            await dbm.form_22.LoadAsync();
        }
        catch
        {
            dbm.form_21.Local.Clear();
            dbm.form_22.Local.Clear();
        }

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.3";
        OnStartProgressBar = 52;
        await dbm.form_23.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.4";
        OnStartProgressBar = 54;
        await dbm.form_24.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.5";
        OnStartProgressBar = 56;
        await dbm.form_25.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.6";
        OnStartProgressBar = 58;
        await dbm.form_26.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.7";
        OnStartProgressBar = 60;
        await dbm.form_27.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.8";
        OnStartProgressBar = 62;
        await dbm.form_28.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.9";
        OnStartProgressBar = 64;
        await dbm.form_29.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.10";
        OnStartProgressBar = 66;
        await dbm.form_210.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.11";
        OnStartProgressBar = 68;
        await dbm.form_211.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.12";
        OnStartProgressBar = 70;
        await dbm.form_212.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций отчетов";
        OnStartProgressBar = 72;
        await dbm.ReportCollectionDbSet.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций организаций";
        OnStartProgressBar = 74;
        await dbm.ReportsCollectionDbSet.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций базы";
        OnStartProgressBar = 76;
        if (!dbm.DBObservableDbSet.Any())
        {
            dbm.DBObservableDbSet.Add(new DBObservable());
            dbm.DBObservableDbSet.Local.First().Reports_Collection.AddRange(dbm.ReportsCollectionDbSet);
        }
        await dbm.DBObservableDbSet.LoadAsync(); 
        #endregion

        onStartProgressBarVm.LoadStatus = "Сортировка организаций";
        OnStartProgressBar = 80;
        await ProcessDataBaseFillEmpty(dbm);

        onStartProgressBarVm.LoadStatus = "Сортировка примечаний";
        OnStartProgressBar = 85;
        LocalReports = dbm.DBObservableDbSet.Local.First();
        await ProcessDataBaseFillNullOrder();

        onStartProgressBarVm.LoadStatus = "Сохранение";
        OnStartProgressBar = 90;
        await dbm.SaveChangesAsync();
        LocalReports.PropertyChanged += Local_ReportsChanged;

        onStartProgressBarVm.LoadStatus = "Инициализация";
        OnStartProgressBar = 95;
        await PropertiesInit();

        OnStartProgressBar = 100;
    }

    private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(LocalReports));
    }

    #endregion

    #region OnStartProgressBar

    private double _OnStartProgressBar;

    public double OnStartProgressBar
    {
        get => _OnStartProgressBar;
        private set
        {
            if (_OnStartProgressBar.Equals(value)) return;
            _OnStartProgressBar = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Commands

    public ICommand AddForm { get; set; }                   //  Создать и открыть новое окно формы для выбранной организации
    public ICommand AddReports { get; set; }                //  Создать и открыть новое окно формы организации (1.0 и 2.0)
    public ICommand ChangeForm { get; set; }                //  Открыть окно редактирования выбранной формы
    public ICommand ChangePasFolder { get; set; }           //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ChangeReports { get; set; }             //  Изменить Формы организации (1.0 и 2.0)
    public ICommand DeleteForm { get; set; }                //  Удалить выбранную форму у выбранной организации
    public ICommand DeleteReports { get; set; }             //  Удалить выбранную организацию
    public ICommand ExcelExportFormAnalysis { get; set; }   //  Выбранная форма -> Выгрузка Excel -> Для анализа
    public ICommand ExcelExportFormPrint { get; set; }      //  Выбранная форма -> Выгрузка Excel -> Для печати
    public ICommand ExcelExportForms { get; set; }          //  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    public ICommand ExcelExportIntersections { get; set; }  //  Excel -> Разрывы и пересечения
    public ICommand ExcelExportListOfForms1 { get; set; }   //  Excel -> Список форм 1
    public ICommand ExcelExportListOfForms2 { get; set; }   //  Excel -> Список форм 2
    public ICommand ExcelExportListOfOrgs { get; set; }     //  Excel -> Список организаций
    public ICommand ExcelExportPasWithoutRep { get; set; }  //  Excel -> Паспорта -> Паспорта без отчетов
    public ICommand ExcelExportRepWithoutPas { get; set; }  //  Excel -> Паспорта -> Отчеты без паспортов
    public ICommand ExcelExportSelectedOrgAll { get; set; } //  Excel -> Выбранная организация -> Все формы
    public ICommand ImportExcel { get; set; }               //  Импорт -> Из Excel
    public ICommand ImportRaodb { get; set; }               //  Импорт -> Из RAODB
    public ICommand SaveReports { get; set; }               //  Сохраняет текущую базу, используется только для сохранения комментария формы

    #endregion

    #region Constructor

    public MainWindowVM()
    {
        AddForm = new AddFormAsyncCommand();
        AddReports = new AddReportsAsyncCommand();
        ChangeForm = new ChangeFormAsyncCommand();
        ChangePasFolder = new ChangePasFolderAsyncCommand();
        ChangeReports = new ChangeReportsAsyncCommand();
        DeleteForm = new DeleteFormAsyncCommand();
        DeleteReports = new DeleteReportsAsyncCommand();
        ExcelExportFormAnalysis = new ExcelExportFormAnalysisAsyncCommand();
        ExcelExportFormPrint = new ExcelExportFormPrintAsyncCommand(); 
        ExcelExportForms = new ExcelExportFormsAsyncCommand();
        ExcelExportIntersections = new ExcelExportIntersectionsAsyncCommand();
        ExcelExportListOfForms1 = new ExcelExportListOfForms1AsyncCommand();
        ExcelExportListOfForms2 = new ExcelExportListOfForms2AsyncCommand();
        ExcelExportListOfOrgs = new ExcelExportListOfOrgsAsyncCommand();
        ExcelExportPasWithoutRep = new ExcelExportPasWithoutRepAsyncCommand();
        ExcelExportRepWithoutPas = new ExcelExportRepWithoutPasAsyncCommand();
        ExcelExportSelectedOrgAll = new ExcelExportSelectedOrgAllAsyncCommand();
        ImportExcel = new ImportExcelAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
    }

    #endregion

    #region Interactions

    public static Interaction<ChangeOrCreateVM, object> ShowDialog  { get; } = new();
    public static Interaction<List<string>, string> ShowMessage { get; } = new();

    #endregion

    #region ImportForm

    public ReactiveCommand<Unit, Unit> ImportForm { get; private set; }

    private async Task<string[]?> GetSelectedFilesFromDialog(string name, params string[] extensions)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
        {
            return null;
        }

        OpenFileDialog dial = new() { AllowMultiple = true };
        var filter = new FileDialogFilter
        {
            Name = name,
            Extensions = new List<string>(extensions)
        };
        dial.Filters = new List<FileDialogFilter> { filter };
        return await dial.ShowAsync(desktop.MainWindow);
    }

    private async Task<string> GetTempDirectory(string systemDirectory)
    {
        try
        {
            var path = Path.GetPathRoot(systemDirectory);
            var tmp = Path.Combine(path, "RAO");
            tmp = Path.Combine(tmp, "temp");
            Directory.CreateDirectory(tmp);
            return tmp;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error2);
            throw new Exception(ErrorMessages.Error2[0]);
        }
    }

    private static async Task<string> GetRaoFileName()
    {
        var count = 0;
        string? file;
        do
        {
            file = Path.Combine(TmpDirectory, $"file_imp_{count++}.raodb");
        } while (File.Exists(file));

        return file;
    }

    private async Task<List<Reports>> GetReportsFromDataBase(string file)
    {
        await using DBModel db = new(file);

        #region Test Version

        var t = await db.Database.GetPendingMigrationsAsync();
        var a = db.Database.GetMigrations();
        var b = await db.Database.GetAppliedMigrationsAsync();

        #endregion

        await db.Database.MigrateAsync();
        await db.LoadTablesAsync();
        await ProcessDataBaseFillEmpty(db);
        return db.DBObservableDbSet.Local.First().Reports_Collection.ToList().Count != 0
            ? db.DBObservableDbSet.Local.First().Reports_Collection.ToList()
            : await db.ReportsCollectionDbSet.ToListAsync();
    }

    private async Task<Reports?> GetReports11FromLocalEqual(Reports item)
    {
        try
        {
            if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('1')) || item.Master_DB.FormNum_DB is not "1.0")
            {
                return null;
            }

            return LocalReports.Reports_Collection10
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].RegNo_DB == "")

                   ?? LocalReports
                       .Reports_Collection10 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].Okpo_DB == ""
                           && item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB != ""
                           && item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    private async Task<Reports?> GetReports21FromLocalEqual(Reports item)
    {
        try
        {
            if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('2')) || item.Master_DB.FormNum_DB is not "2.0")
            {
                return null;
            }

            return LocalReports.Reports_Collection20
                       .FirstOrDefault(t => 

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].RegNo_DB == "")

                   ?? LocalReports.Reports_Collection20 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t => 
                               
                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].Okpo_DB == ""
                           && item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB != ""
                           && item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    private async Task RestoreReportsOrders(Reports item)
    {
        if (item.Master_DB.FormNum_DB == "1.0")
        {
            if (item.Master_DB.Rows10[0].Id > item.Master_DB.Rows10[1].Id)
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
        }

        if (item.Master_DB.FormNum_DB == "2.0")
        {
            if (item.Master_DB.Rows20[0].Id > item.Master_DB.Rows20[1].Id)
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }

    private async Task ProcessIfNoteOrder0(Reports item)
    {
        foreach (Report form in item.Report_Collection)
        {
            foreach (Note note in form.Notes)
            {
                if (note.Order == 0)
                {
                    note.Order = GetNumberInOrder(form.Notes);
                }
            }
        }
    }

    

    



    4private async Task _ImportForm()
    {
        try
        {
            if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
            string[] extensions = { "raodb", "RAODB" };
            var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
            if (answer is null) return;
            skipNewOrg = false;
            skipInter = false;
            skipLess = false;
            skipNew = false;
            skipReplace = false;
            hasMultipleReport = false;

            foreach (var res in answer) //Для каждого импортируемого файла
            {
                if (res == "") continue;
                var file = await GetRaoFileName();
                var sourceFile = new FileInfo(res);
                sourceFile.CopyTo(file, true);
                var reportsCollection = await GetReportsFromDataBase(file);

                if (!hasMultipleReport)
                {
                    hasMultipleReport = (reportsCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1);
                }
                foreach (var item in reportsCollection) //Для каждой импортируемой организации
                {
                    await item.SortAsync();
                    await RestoreReportsOrders(item);
                    if (item.Master.Rows10.Count != 0)
                    {
                        item.Master.Rows10[1].RegNo_DB = item.Master.Rows10[0].RegNo_DB;
                    }
                    if (item.Master.Rows20.Count != 0)
                    {
                        item.Master.Rows20[1].RegNo_DB = item.Master.Rows20[0].RegNo_DB;
                    }

                    var first11 = await GetReports11FromLocalEqual(item);
                    var first21 = await GetReports21FromLocalEqual(item);
                    FillEmptyRegNo(ref first11);
                    FillEmptyRegNo(ref first21);
                    item.CleanIds();
                    await ProcessIfNoteOrder0(item);

                    if (first11 != null)
                    {
                        await ProcessIfHasReports11(first11, item);
                    }
                    else if (first21 != null)
                    {
                        await ProcessIfHasReports21(first21, item);
                    }
                    else if (first11 == null && first21 == null)
                    {
                        #region AddNewOrg

                        var an = "Добавить";
                        if (!skipNewOrg)
                        {
                            if (answer.Length > 1 || reportsCollection.Count > 1)
                            {
                                #region MessageNewOrg

                                an = await MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                    {
                                        ButtonDefinitions = new[]
                                        {
                                            new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                            new ButtonDefinition { Name = "Да для всех" },
                                            new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                        },
                                        ContentTitle = "Импорт из .raodb",
                                        ContentHeader = "Уведомление",
                                        ContentMessage =
                                            $"Будет добавлена новая организация ({item.Master.FormNum_DB}) содержащая {item.Report_Collection.Count} форм отчетности." +
                                            $"{Environment.NewLine}" +
                                            $"{Environment.NewLine}Регистрационный номер - {item.Master.RegNoRep.Value}" +
                                            $"{Environment.NewLine}ОКПО - {item.Master.OkpoRep.Value}" +
                                            $"{Environment.NewLine}Сокращенное наименование - {item.Master.ShortJurLicoRep.Value}" +
                                            $"{Environment.NewLine}" +
                                            $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений " +
                                            $"{Environment.NewLine}импортировать все новые организации.",
                                        MinWidth = 400,
                                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                                    })
                                    .ShowDialog(desktop.MainWindow);

                                #endregion

                                if (an is "Да для всех") skipNewOrg = true;
                            }
                            else
                            {
                                #region MessageNewOrg

                                an = await MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                    {
                                        ButtonDefinitions = new[]
                                        {
                                            new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                            new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                        },
                                        ContentTitle = "Импорт из .raodb",
                                        ContentHeader = "Уведомление",
                                        ContentMessage =
                                            $"Будет добавлена новая организация ({item.Master.FormNum_DB}) содержащая {item.Report_Collection.Count} форм отчетности." +
                                            $"{Environment.NewLine}" +
                                            $"{Environment.NewLine}Регистрационный номер - {item.Master.RegNoRep.Value}" +
                                            $"{Environment.NewLine}ОКПО - {item.Master.OkpoRep.Value}" +
                                            $"{Environment.NewLine}Сокращенное наименование - {item.Master.ShortJurLicoRep.Value}",
                                        MinWidth = 400,
                                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                                    })
                                    .ShowDialog(desktop.MainWindow);

                                #endregion
                            }
                        }

                        if (an is "Добавить" or "Да для всех")
                        {
                            LocalReports.Reports_Collection.Add(item);
                        }

                        #endregion
                    }

                    switch (item.Master_DB.FormNum_DB)
                    {
                        case "1.0":
                            await item.Master_DB.Rows10.QuickSortAsync();
                            break;
                        case "2.0":
                            await item.Master_DB.Rows20.QuickSortAsync();
                            break;
                    }
                }
            }
            await LocalReports.Reports_Collection.QuickSortAsync();
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }
    }

    private static void FillEmptyRegNo(ref Reports? reps)
    {
        if (reps is null) return;
        if (reps.Master.Rows10.Count >= 2)
        {
            if (reps.Master.Rows10[0].RegNo_DB is "" && reps.Master.Rows10[1].RegNo_DB is not "" && reps.Master.Rows10[0].Okpo_DB is not "")
            {
                reps.Master.Rows10[0].RegNo_DB = reps.Master.Rows10[1].RegNo_DB;
            }
            if (reps.Master.Rows10[1].RegNo_DB is "" && reps.Master.Rows10[0].RegNo_DB is not "" && reps.Master.Rows10[1].Okpo_DB is not "")
            {
                reps.Master.Rows10[1].RegNo_DB = reps.Master.Rows10[0].RegNo_DB;
            }
        }
        if (reps.Master.Rows20.Count >= 2)
        {
            if (reps.Master.Rows20[0].RegNo_DB is "" && reps.Master.Rows20[1].RegNo_DB is not "" && reps.Master.Rows20[0].Okpo_DB is not "")
            {
                reps.Master.Rows20[0].RegNo_DB = reps.Master.Rows20[1].RegNo_DB;
            }
            if (reps.Master.Rows20[1].RegNo_DB is "" && reps.Master.Rows20[0].RegNo_DB is not "" && reps.Master.Rows20[1].Okpo_DB is not "")
            {
                reps.Master.Rows20[1].RegNo_DB = reps.Master.Rows20[0].RegNo_DB;
            }
        }
    }

    

    #endregion

    #region Export

    #region ExportForm

    public ReactiveCommand<object, Unit> ExportForm { get; private set; }

    private async Task _ExportForm(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || par is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;
        var folderPath = await new OpenFolderDialog().ShowAsync(desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;
        foreach (var item in param)
        {
            var a = DateTime.Now.Date;
            var aDay = a.Day.ToString();
            var aMonth = a.Month.ToString();
            if (aDay.Length < 2) aDay = $"0{aDay}";
            if (aMonth.Length < 2) aMonth = $"0{aMonth}";
            ((Report)item).ExportDate.Value = $"{aDay}.{aMonth}.{a.Year}";
        }

        var dt = DateTime.Now;
        var fileNameTmp = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
        var exportForm = (Report) param.First();

        var dtDay = dt.Day.ToString();
        var dtMonth = dt.Month.ToString();
        if (dtDay.Length < 2) dtDay = $"0{dtDay}";
        if (dtMonth.Length < 2) dtMonth = $"0{dtMonth}";
        exportForm.ExportDate.Value = $"{dtDay}.{dtMonth}.{dt.Year}";

        await StaticConfiguration.DBModel.SaveChangesAsync();

        var reps = LocalReports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection.Contains(exportForm));
        if (reps is null) return;

        var fullPathTmp = Path.Combine(await GetTempDirectory(await GetSystemDirectory()), $"{fileNameTmp}_exp.RAODB");

        Reports orgWithExpForm = new()
        {
            Master = reps.Master
        };
        orgWithExpForm.Report_Collection.Add(exportForm);

        var filename = reps.Master_DB.FormNum_DB switch
        {
            "1.0" =>
                RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportForm.FormNum_DB}" +
                $"_{RemoveForbiddenChars(exportForm.StartPeriod_DB)}" +
                $"_{RemoveForbiddenChars(exportForm.EndPeriod_DB)}" +
                $"_{exportForm.CorrectionNumber_DB}" +
                $"_{Version}",

            "2.0" when orgWithExpForm.Master.Rows20.Count > 0 =>
                RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportForm.FormNum_DB}" +
                $"_{RemoveForbiddenChars(exportForm.Year_DB)}" +
                $"_{exportForm.CorrectionNumber_DB}" +
                $"_{Version}",
            _ => throw new ArgumentOutOfRangeException()
        };

        var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception)
            {
                #region FailedToSaveFileMessage

                await Dispatcher.UIThread.InvokeAsync(() =>
                    MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Выгрузка в Excel",
                            ContentHeader = "Ошибка",
                            ContentMessage =
                                "Не удалось сохранить файл по пути:" +
                                $"{Environment.NewLine}{fullPath}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                $"{Environment.NewLine}и используется другим процессом.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }).ShowDialog(desktop.MainWindow));

                #endregion
                
                return;
            }
        }

        await Task.Run(async () =>
        {
            DBModel db = new(fullPathTmp);
            try
            {
                await db.Database.MigrateAsync();
                await db.ReportsCollectionDbSet.AddAsync(orgWithExpForm);
                if (!db.DBObservableDbSet.Any())
                {
                    db.DBObservableDbSet.Add(new DBObservable());
                    db.DBObservableDbSet.Local.First().Reports_Collection.AddRange(db.ReportsCollectionDbSet.Local);
                }
                await db.SaveChangesAsync();

                var t = db.Database.GetDbConnection() as FbConnection;
                await t.CloseAsync();
                await t.DisposeAsync();

                await db.Database.CloseConnectionAsync();
                await db.DisposeAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            try
            {
                File.Copy(fullPathTmp, fullPath);
                File.Delete(fullPathTmp);
            }
            catch (Exception e)
            {
                #region FailedCopyFromTempMessage

                await Dispatcher.UIThread.InvokeAsync(() =>
                    MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Выгрузка в Excel",
                            ContentHeader = "Ошибка",
                            ContentMessage = "При копировании файла базы данных из временной папки возникла ошибка." +
                                             $"{Environment.NewLine}Экспорт не выполнен.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        }).ShowDialog(desktop.MainWindow));
        
                #endregion

                return;
            }
        });

        #region ExportCompliteMessage

        var answer = await Dispatcher.UIThread.InvokeAsync(() =>                
            MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                    },
                    ContentTitle = "Выгрузка в .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Файл экспорта формы сохранен по пути:" +
                        $"{Environment.NewLine}{fullPath}" +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}Регистрационный номер - {orgWithExpForm.Master.RegNoRep.Value}" +
                        $"{Environment.NewLine}ОКПО - {orgWithExpForm.Master.OkpoRep.Value}" +
                        $"{Environment.NewLine}Сокращенное наименование - {orgWithExpForm.Master.ShortJurLicoRep.Value}" +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}Номер формы - {exportForm.FormNum_DB}" +
                        $"{Environment.NewLine}Начало отчетного периода - {exportForm.StartPeriod_DB}" +
                        $"{Environment.NewLine}Конец отчетного периода - {exportForm.EndPeriod_DB}" +
                        $"{Environment.NewLine}Дата выгрузки - {exportForm.ExportDate_DB}" +
                        $"{Environment.NewLine}Номер корректировки - {exportForm.CorrectionNumber_DB}" +
                        $"{Environment.NewLine}Количество строк - {exportForm.Rows.Count}{InventoryCheck(exportForm)}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(desktop.MainWindow));

        #endregion

        if (answer is "Открыть расположение файла")
        {
            Process.Start("explorer", folderPath);
        }
    }

    #endregion

    #region ExportOrg

    public ReactiveCommand<object, Unit> ExportOrg { get; private set; }

    private async Task _ExportOrg(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var folderPath = await new OpenFolderDialog().ShowAsync(desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;
        var dt = DateTime.Now;
        Reports exportOrg;
        string fileNameTmp;
        if (par is ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            foreach (var item in param)
            {
                ((Reports)item).Master.ExportDate.Value = dt.Date.ToShortDateString();
            }
            fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
            exportOrg = (Reports) param.First();
            await StaticConfiguration.DBModel.SaveChangesAsync();
        } else if (par is Reports)
        {
            fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
            exportOrg = (Reports)par;
            exportOrg.Master.ExportDate.Value = dt.Date.ToShortDateString();
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        else return;
        
        var fullPathTmp = Path.Combine(await GetTempDirectory(await GetSystemDirectory()), $"{fileNameTmp}_exp.RAODB");
        var filename = $"{RemoveForbiddenChars(exportOrg.Master.RegNoRep.Value)}" +
                       $"_{RemoveForbiddenChars(exportOrg.Master.OkpoRep.Value)}" +
                       $"_{exportOrg.Master.FormNum_DB}" +
                       $"_{Version}";

        var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception)
            {
                #region FailedToSaveFileMessage

                await Dispatcher.UIThread.InvokeAsync(() =>
                    MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Выгрузка",
                            ContentHeader = "Ошибка",
                            ContentMessage =
                                "Не удалось сохранить файл по пути:" +
                                $"{Environment.NewLine}{fullPath}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                $"{Environment.NewLine}и используется другим процессом.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }).ShowDialog(desktop.MainWindow));

                #endregion
                
                return;
            }
        }
        
        await Task.Run(async () =>
        {
            DBModel db = new(fullPathTmp);
            try
            {
                await db.Database.MigrateAsync();
                await db.ReportsCollectionDbSet.AddAsync(exportOrg);
                await db.SaveChangesAsync();

                var t = db.Database.GetDbConnection() as FbConnection;
                await t.CloseAsync();
                await t.DisposeAsync();

                await db.Database.CloseConnectionAsync();
                await db.DisposeAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            try
            {
                File.Copy(fullPathTmp, fullPath);
                File.Delete(fullPathTmp);
            }
            catch (Exception e)
            {
                #region FailedCopyFromTempMessage

                await Dispatcher.UIThread.InvokeAsync(() =>
                    MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Выгрузка",
                            ContentHeader = "Ошибка",
                            ContentMessage = "При копировании файла базы данных из временной папки возникла ошибка." +
                                             $"{Environment.NewLine}Экспорт не выполнен.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        }).ShowDialog(desktop.MainWindow));

                #endregion

                return;
            }
        });

        #region ExportDoneMessage

        var answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Открыть расположение файла" }
                },
                ContentTitle = "Выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage =
                    $"Экспорт завершен. Файл экспорта организации ({exportOrg.Master.FormNum_DB}) сохранен по пути:" +
                    $"{Environment.NewLine}{fullPath}" +
                    $"{Environment.NewLine}" +
                    $"{Environment.NewLine}Регистрационный номер - {exportOrg.Master.RegNoRep.Value}" +
                    $"{Environment.NewLine}ОКПО - {exportOrg.Master.OkpoRep.Value}" +
                    $"{Environment.NewLine}Сокращенное наименование - {exportOrg.Master.ShortJurLicoRep.Value}",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            }).ShowDialog(desktop.MainWindow); 

        #endregion

        if (answer is "Открыть расположение файла")
        {
            Process.Start("explorer", folderPath);
        }
    }

    #endregion

    #region ExportOrgWithDateRange

    public ReactiveCommand<object, Unit> ExportOrgWithDateRange { get; private set; }

    private async Task _ExportOrgWithDateRange(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || par is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;

        #region MessageAskStartDate
        var startDate = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                },
                ContentTitle = "Выгрузка",
                ShowInCenter = true,
                ContentMessage =
                    "Введите дату начала периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате начала периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow); 
        #endregion

        if (startDate.Button is null or "Отменить экспорт") return;

        #region MessageAskEndDate
        var endDate = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                },
                ContentTitle = "Выгрузка",
                ContentMessage =
                    "Введите дату конца периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате конца периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow); 
        #endregion

        if (startDate.Button is null or "Отменить экспорт") return;

        var canParseDateRange = 
            (DateTime.TryParse(startDate.Message, out var startDateTime) | string.IsNullOrEmpty(startDate.Message))
            & (DateTime.TryParse(endDate.Message, out var endDateTime) | string.IsNullOrEmpty(endDate.Message));
        if (endDateTime == DateTime.MinValue) endDateTime = DateTime.MaxValue;

        if (!canParseDateRange || startDateTime > endDateTime)
        {
            #region MessageErrorAtParseDate

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Экспорт не будет выполнен, поскольку период дат введён некорректно.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowDialog(desktop.MainWindow);

            #endregion

            return;
        }

        var org = (Reports) param.First();
        var repInRange = org.Report_Collection.Where(rep =>
        {
            if (!DateTime.TryParse(rep.StartPeriod_DB, out var repStartDateTime)
                || !DateTime.TryParse(rep.EndPeriod_DB, out var repEndDateTime)) return false;
            return startDateTime <= repEndDateTime && endDateTime >= repStartDateTime;
        });
        Reports exportOrg = new() { Master = org.Master };
        exportOrg.Report_Collection.AddRange(repInRange);
        await _ExportOrg(exportOrg);
    }

    #endregion

    #region ExportAllOrg

    public ReactiveCommand<object, Unit> ExportAllOrg { get; private set; }

    private async Task _ExportAllOrg(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        string? answer;
        if (LocalReports.Reports_Collection.Count > 10)
        {
            #region ExportDoneMessage

            answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Отменить выгрузку", IsCancel = true }
                    },
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Текущая база содержит {LocalReports.Reports_Collection.Count} форм организаций," +
                        $"{Environment.NewLine}выгрузка займет примерно {LocalReports.Reports_Collection.Count / 20} минут",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(desktop.MainWindow); 

            #endregion

            if (answer is "Отменить выгрузку") return;
        }
        var folderPath = await new OpenFolderDialog().ShowAsync(desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;

        ParallelOptions po = new ParallelOptions();
        Parallel.ForEach(LocalReports.Reports_Collection, async exportOrg =>
        {
            await Task.Run(async () =>
            {
                var dt = DateTime.Now;
                var fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}_{dt.Millisecond}";
                await StaticConfiguration.DBModel.SaveChangesAsync();
                var fullPathTmp = Path.Combine(await GetTempDirectory(await GetSystemDirectory()), $"{fileNameTmp}.RAODB");
                var filename = $"{RemoveForbiddenChars(exportOrg.Master.RegNoRep.Value)}" +
                               $"_{RemoveForbiddenChars(exportOrg.Master.OkpoRep.Value)}" +
                               $"_{exportOrg.Master.FormNum_DB}" +
                               $"_{Version}";

                var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");
                DBModel db = new(fullPathTmp);
                try
                {
                    await db.Database.MigrateAsync();
                    await db.ReportsCollectionDbSet.AddAsync(exportOrg);
                    await db.SaveChangesAsync();

                    var t = db.Database.GetDbConnection() as FbConnection;
                    await t.CloseAsync();
                    await t.DisposeAsync();

                    await db.Database.CloseConnectionAsync();
                    await db.DisposeAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                try
                {
                    while (File.Exists(fullPath)) // insert index if file already exist
                    {
                        MatchCollection matches = Regex.Matches(fullPath, @"(.+)#(\d+)(?=\.RAODB)");
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (!int.TryParse(match.Groups[2].Value, out var index)) return;
                                fullPath = match.Groups[1].Value + $"#{index + 1}.RAODB";
                            }
                        }
                        else
                        {
                            fullPath = fullPath.TrimEnd(".RAODB".ToCharArray()) + "#1.RAODB";
                        }
                    }
                    File.Copy(fullPathTmp, fullPath);
                    File.Delete(fullPathTmp);
                }
                catch (Exception e)
                {
                    #region FailedCopyFromTempMessage

                    await Dispatcher.UIThread.InvokeAsync(() =>
                        MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = "Выгрузка",
                                ContentHeader = "Ошибка",
                                ContentMessage = "При копировании файла базы данных из временной папки возникла ошибка." +
                                                 $"{Environment.NewLine}Экспорт не выполнен." +
                                                 $"{Environment.NewLine}{e.Message}",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            })
                            .ShowDialog(desktop.MainWindow));

                    #endregion

                    return;
                }
            });
        });
        
        #region ExportDoneMessage

        answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Открыть расположение файлов" }
                },
                ContentTitle = "Выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = "Выгрузка всех организаций в отдельные" +
                                 $"{Environment.NewLine}файлы .raodb завершена.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            })
            .ShowDialog(desktop.MainWindow);

        #endregion

        if (answer is "Открыть расположение файлов")
        {
            Process.Start("explorer", folderPath);
        }

        
    }

    #endregion

    #endregion

    #region INotifyPropertyChanged

    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}