using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.ExcelExport.Passports;
using Client_App.Commands.AsyncCommands.Import;
using Client_App.Commands.AsyncCommands.Import.ImportJson;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Commands.AsyncCommands.Save;

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

    private static DBObservable LocalReports
    {
        get => ReportsStorage.LocalReports;
        set
        {
            if (ReportsStorage.LocalReports != value)
            {
                ReportsStorage.LocalReports = value;
            }
        }
    }

    #endregion

    #region Initialization

    #region Init
    
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

        onStartProgressBarVm.LoadStatus = "Загрузка форм 1.0";
        OnStartProgressBar = 24;
        await dbm.form_10.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка форм 2.0";
        OnStartProgressBar = 45;
        await dbm.form_20.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций отчетов";
        OnStartProgressBar = 72;
        await dbm.ReportCollectionDbSet.LoadAsync();

        onStartProgressBarVm.LoadStatus = "Загрузка коллекций организаций";
        OnStartProgressBar = 74;
        await dbm.ReportsCollectionDbSet.LoadAsync();
        var A = ReportsStorage.LocalReports.Reports_Collection;
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
        ReportsStorage.LocalReports = dbm.DBObservableDbSet.Local.First();

        await ProcessDataBaseFillNullOrder();

        onStartProgressBarVm.LoadStatus = "Сохранение";
        OnStartProgressBar = 90;
        await dbm.SaveChangesAsync();
        ReportsStorage.LocalReports.PropertyChanged += Local_ReportsChanged;

        OnStartProgressBar = 100;
    }

    #endregion

    #region GetSystemDirectory
    
    private static async Task<string> GetSystemDirectory()
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
            return SystemDirectory;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error1);
            throw new Exception(ErrorMessages.Error1[0]);
        }
    }

    #endregion

    #region ProcessRaoDirectory
    
    private static async Task ProcessRaoDirectory()
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
            catch
            {
                // ignored
            }
        }
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
            catch
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
            try
            {
                await dbm.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
        OnPropertyChanged(nameof(ReportsStorage.LocalReports));
    }

    #endregion

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
    public ICommand ExcelExportAll { get; set; }                //  Excel -> Все формы и Excel -> Выбранная организация -> Все формы
    public ICommand ExcelExportExecutors { get; set; }          //  Excel -> Список исполнителей
    public ICommand ExcelExportFormAnalysis { get; set; }       //  Выбранная форма -> Выгрузка Excel -> Для анализа
    public ICommand ExcelExportFormPrint { get; set; }          //  Выбранная форма -> Выгрузка Excel -> Для печати
    public ICommand ExcelExportForms { get; set; }              //  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    public ICommand ExcelExportIntersections { get; set; }      //  Excel -> Разрывы и пересечения
    public ICommand ExcelExportListOfForms1 { get; set; }       //  Excel -> Список форм 1
    public ICommand ExcelExportListOfForms2 { get; set; }       //  Excel -> Список форм 2
    public ICommand ExcelExportListOfOrgs { get; set; }         //  Excel -> Список организаций
    public ICommand ExcelExportPasWithoutRep { get; set; }      //  Excel -> Паспорта -> Паспорта без отчетов
    public ICommand ExcelExportRepWithoutPas { get; set; }      //  Excel -> Паспорта -> Отчеты без паспортов
    public ICommand ExportAllReports { get; set; }              //  Экспорт всех организаций организации в отдельные файлы .raodb
    public ICommand ExportForm { get; set; }                    //  Экспорт формы в файл .raodb
    public ICommand ExportReports { get; set; }                 //  Экспорт организации в файл .raodb
    public ICommand ExportReportsWithDateRange { get; set; }    //  Экспорт организации в файл .raodb с указанием диапазона дат выгружаемых форм
    public ICommand ImportExcel { get; set; }                   //  Импорт -> Из Excel
    public ICommand ImportJson { get; set; }                    //  Импорт -> Из Json
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
        ExcelExportExecutors = new ExcelExportExecutorsAsyncCommand();
        ExcelExportFormAnalysis = new ExcelExportFormAnalysisAsyncCommand();
        ExcelExportFormPrint = new ExcelExportFormPrintAsyncCommand(); 
        ExcelExportForms = new ExcelExportFormsAsyncCommand();
        ExcelExportIntersections = new ExcelExportIntersectionsAsyncCommand();
        ExcelExportListOfForms1 = new ExcelExportListOfForms1AsyncCommand();
        ExcelExportListOfForms2 = new ExcelExportListOfForms2AsyncCommand();
        ExcelExportListOfOrgs = new ExcelExportListOfOrgsAsyncCommand();
        ExcelExportPasWithoutRep = new ExcelExportPasWithoutRepAsyncCommand();
        ExcelExportRepWithoutPas = new ExcelExportRepWithoutPasAsyncCommand();
        ExcelExportAll = new ExcelExportAllAsyncCommandAsyncCommand();
        ExportAllReports = new ExportAllReportsAsyncCommand();
        ExportForm = new ExportFormAsyncCommand();
        ExportReports = new ExportReportsAsyncCommand();
        ExportReportsWithDateRange = new ExportReportsWithDateRangeAsyncCommand(this);
        ImportExcel = new ImportExcelAsyncCommand();
        ImportJson = new ImportJsonAsyncCommand();
        ImportRaodb = new ImportRaodbAsyncCommand();
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