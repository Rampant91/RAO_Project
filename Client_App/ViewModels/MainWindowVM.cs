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

    private static async Task<string> GetSystemDirectory()
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
            await process.StandardInput.WriteLineAsync("logname");
            var userName = await process.StandardOutput.ReadLineAsync();
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

    private static async Task ProcessRaoDirectory()
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

    private static async Task ProcessDataBaseFillNullOrder()
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

    public ICommand AddForm { get; set; }                       //  Создать и открыть новое окно формы для выбранной организации
    public ICommand AddReports { get; set; }                    //  Создать и открыть новое окно формы организации (1.0 и 2.0)
    public ICommand ChangeForm { get; set; }                    //  Открыть окно редактирования выбранной формы
    public ICommand ChangePasFolder { get; set; }               //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ChangeReports { get; set; }                 //  Изменить Формы организации (1.0 и 2.0)
    public ICommand DeleteForm { get; set; }                    //  Удалить выбранную форму у выбранной организации
    public ICommand DeleteReports { get; set; }                 //  Удалить выбранную организацию
    public ICommand ExcelExportFormAnalysis { get; set; }       //  Выбранная форма -> Выгрузка Excel -> Для анализа
    public ICommand ExcelExportFormPrint { get; set; }          //  Выбранная форма -> Выгрузка Excel -> Для печати
    public ICommand ExcelExportForms { get; set; }              //  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    public ICommand ExcelExportIntersections { get; set; }      //  Excel -> Разрывы и пересечения
    public ICommand ExcelExportListOfForms1 { get; set; }       //  Excel -> Список форм 1
    public ICommand ExcelExportListOfForms2 { get; set; }       //  Excel -> Список форм 2
    public ICommand ExcelExportListOfOrgs { get; set; }         //  Excel -> Список организаций
    public ICommand ExcelExportPasWithoutRep { get; set; }      //  Excel -> Паспорта -> Паспорта без отчетов
    public ICommand ExcelExportRepWithoutPas { get; set; }      //  Excel -> Паспорта -> Отчеты без паспортов
    public ICommand ExcelExportSelectedOrgAll { get; set; }     //  Excel -> Выбранная организация -> Все формы
    public ICommand ExportAllReports { get; set; }              //  Экспорт всех организаций организации в отдельные файлы .raodb
    public ICommand ExportForm { get; set; }                    //  Экспорт формы в файл .raodb
    public ICommand ExportReports { get; set; }                 //  Экспорт организации в файл .raodb
    public ICommand ExportReportsWithDateRange { get; set; }    //  Экспорт организации в файл .raodb с указанием диапазона дат выгружаемых форм
    public ICommand ImportExcel { get; set; }                   //  Импорт -> Из Excel
    public ICommand ImportRaodb { get; set; }                   //  Импорт -> Из RAODB
    public ICommand SaveReports { get; set; }                   //  Сохраняет текущую базу, используется только для сохранения комментария формы

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
        ExportAllReports = new ExportAllReportsAsyncCommand();
        ExportForm = new ExportFormAsyncCommand();
        ExportReports = new ExportReportsAsyncCommand();
        ExportReportsWithDateRange = new ExportReportsWithDateRangeAsyncCommand(this);
        ImportExcel = new ExcelAsyncCommand();
        ImportRaodb = new RaodbAsyncCommand();
        SaveReports = new SaveReportsAsyncCommand();
    }

    #endregion

    #region Interactions

    public static Interaction<ChangeOrCreateVM, object> ShowDialog  { get; } = new();
    public static Interaction<List<string>, string> ShowMessage { get; } = new();

    #endregion

    #region INotifyPropertyChanged

    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}