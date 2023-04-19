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
using System.CodeDom;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Excel;
using DynamicData;

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
        AddReport = ReactiveCommand.CreateFromTask<object>(_AddReport);
        AddForm = ReactiveCommand.CreateFromTask<object>(_AddForm);
        ImportForm = ReactiveCommand.CreateFromTask(_ImportForm);
        ImportFrom = ReactiveCommand.CreateFromTask(_ImportFrom);
        ExportForm = ReactiveCommand.CreateFromTask<object>(_ExportForm);
        ExportOrg = ReactiveCommand.CreateFromTask<object>(_ExportOrg);
        ExportAllOrg = ReactiveCommand.CreateFromTask<object>(_ExportAllOrg);
        ExportOrgWithDateRange = ReactiveCommand.CreateFromTask<object>(_ExportOrgWithDateRange);
        ChangeForm = ReactiveCommand.CreateFromTask<object>(_ChangeForm);
        ChangeReport = ReactiveCommand.CreateFromTask<object>(_ChangeReport);
        DeleteForm = ReactiveCommand.CreateFromTask<object>(_DeleteForm);
        DeleteReport = ReactiveCommand.CreateFromTask<object>(_DeleteReport);
        SaveReport = ReactiveCommand.CreateFromTask<object>(_SaveReport);
        //Print_Excel_Export = ReactiveCommand.CreateFromTask<object>(_Print_Excel_Export);
        //Excel_Export = ReactiveCommand.CreateFromTask<object>(_Excel_Export);
        //All_Excel_Export = ReactiveCommand.CreateFromTask<object>(_All_Excel_Export);
        //SelectOrgExcelExport = ReactiveCommand.CreateFromTask(_SelectOrgExcelExport);
        //AllOrganization_Excel_Export = ReactiveCommand.CreateFromTask(_AllOrganization_Excel_Export);
        //ExcelMissingPas = ReactiveCommand.CreateFromTask<object>(_ExcelMissingPas);
        //ExcelPasWithoutRep = ReactiveCommand.CreateFromTask<object>(_ExcelPasWithoutRep);
        //ChangePasDir = ReactiveCommand.CreateFromTask<object>(_ChangePasDir);
        ShowDialog = new Interaction<ChangeOrCreateVM, object>();
        ShowMessage = new Interaction<List<string>, string>();

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

    public ICommand ChangePasFolder { get; set; }           //  Excel -> Паспорта -> Изменить расположение паспортов по умолчанию
    public ICommand ExcelExportFormAnalysis { get; set; }   //  Выбранная форма -> Выгрузка Excel -> Для анализа
    public ICommand ExcelExportFormPrint { get; set; }      //  Выбранная форма -> Выгрузка Excel -> Для печати
    public ICommand ExcelExportForms { get; set; }          //  Excel -> Формы 1.x, 2.x и Excel -> Выбранная организация -> Формы 1.x, 2.x
    public ICommand ExcelExportIntersections { get; set; }  //  Excel -> Разрывы и пересечения
    public ICommand ExcelExportListOfForms1 { get; set; }   //  Excel -> Список форм 1
    public ICommand ExcelExportListOfForms2 { get; set; }   //  Excel -> Список форм 2
    public ICommand ExcelExportListOfOrgs { get; set; }     //  Excel -> Список организаций
    public ICommand ExcelExportPasWithoutRep { get; set; }  //  Excel -> Список организаций
    public ICommand ExcelExportRepWithoutPas { get; set; }  //  Excel -> Паспорта -> Отчеты без паспортов
    public ICommand ExcelExportSelectedOrgAll { get; set; } //  Excel -> Выбранная организация -> Все формы

    #endregion

    #region Constructor

    public MainWindowVM()
    {
        ChangePasFolder = new ChangePasFolderAsyncCommand();
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
    }

    #endregion

    #region Interactions

    public Interaction<ChangeOrCreateVM, object> ShowDialog { get; private set; }
    public Interaction<List<string>, string> ShowMessage { get; private set; }

    #endregion

    #region AddReport

    public ReactiveCommand<object, Unit> AddReport { get; private set; }

    private async Task _AddReport(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            par is string param)
        {
            var t = desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            ChangeOrCreateVM frm = new(param, LocalReports);
            await ShowDialog.Handle(frm);
            t.SelectedReports = tmp;
            await LocalReports.Reports_Collection.QuickSortAsync();
        }
    }

    #endregion

    #region AddForm

    public ReactiveCommand<object, Unit> AddForm { get; private set; }

    private async Task _AddForm(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            par is string param)
        {
            var t = desktop.MainWindow as MainWindow;
            if (t?.SelectedReports is null
                || !t.SelectedReports.Any()
                || ((Reports)t.SelectedReports.First()).Master.FormNum_DB[0] != param[0])
            {
                #region MessageFailedToOpenForm

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = $"Создание формы {param}",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"Не удалось создать форму {param}, поскольку не выбрана организация. Перед созданием формы убедитесь,"
                            + $"{Environment.NewLine}что в списке организаций имеется выбранная организация (подсвечивается голубым цветом).",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(desktop.MainWindow);

                #endregion

                return;
            }

            var y = t.SelectedReports.First() as Reports;
            if (y?.Master.FormNum_DB.Split(".")[0] == param.Split(".")[0])
            {
                var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                ChangeOrCreateVM frm = new(param, y);
                Form2_Visual.tmpVM = param switch
                {
                    "2.1" or "2.2" => frm,
                    _ => Form2_Visual.tmpVM
                };
                await ShowDialog.Handle(frm);
                t.SelectedReports = tmp;
                await y.Report_Collection.QuickSortAsync();
            }
        }
    }

    #endregion

    #region ImportFromEx

    public ReactiveCommand<Unit, Unit> ImportFrom { get; private set; }

    private async Task GetDataFromRow(string param1, ExcelWorksheet worksheet1, int start, Report repFromEx)
    {
        switch (param1)
        {
            case "1.1":
            {
                Form11 form11 = new();
                form11.ExcelGetRow(worksheet1, start);
                repFromEx.Rows11.Add(form11);
                break;
            }
            case "1.2":
            {
                Form12 form12 = new();
                form12.ExcelGetRow(worksheet1, start);
                repFromEx.Rows12.Add(form12);
                break;
            }
            case "1.3":
            {
                Form13 form13 = new();
                form13.ExcelGetRow(worksheet1, start);
                repFromEx.Rows13.Add(form13);
                break;
            }
            case "1.4":
            {
                Form14 form14 = new();
                form14.ExcelGetRow(worksheet1, start);
                repFromEx.Rows14.Add(form14);
                break;
            }
            case "1.5":
            {
                Form15 form15 = new();
                form15.ExcelGetRow(worksheet1, start);
                repFromEx.Rows15.Add(form15);
                break;
            }
            case "1.6":
            {
                Form16 form16 = new();
                form16.ExcelGetRow(worksheet1, start);
                repFromEx.Rows16.Add(form16);
                break;
            }
            case "1.7":
            {
                Form17 form17 = new();
                form17.ExcelGetRow(worksheet1, start);
                repFromEx.Rows17.Add(form17);
                break;
            }
            case "1.8":
            {
                Form18 form18 = new();
                form18.ExcelGetRow(worksheet1, start);
                repFromEx.Rows18.Add(form18);
                break;
            }
            case "1.9":
            {
                Form19 form19 = new();
                form19.ExcelGetRow(worksheet1, start);
                repFromEx.Rows19.Add(form19);
                break;
            }
            case "2.1":
            {
                Form21 form21 = new();
                form21.ExcelGetRow(worksheet1, start);
                repFromEx.Rows21.Add(form21);
                break;
            }
            case "2.2":
            {
                Form22 form22 = new();
                form22.ExcelGetRow(worksheet1, start);
                repFromEx.Rows22.Add(form22);
                break;
            }
            case "2.3":
            {
                Form23 form23 = new();
                form23.ExcelGetRow(worksheet1, start);
                repFromEx.Rows23.Add(form23);
                break;
            }
            case "2.4":
            {
                Form24 form24 = new();
                form24.ExcelGetRow(worksheet1, start);
                repFromEx.Rows24.Add(form24);
                break;
            }
            case "2.5":
            {
                Form25 form25 = new();
                form25.ExcelGetRow(worksheet1, start);
                repFromEx.Rows25.Add(form25);
                break;
            }
            case "2.6":
            {
                Form26 form26 = new();
                form26.ExcelGetRow(worksheet1, start);
                repFromEx.Rows26.Add(form26);
                break;
            }
            case "2.7":
            {
                Form27 form27 = new();
                form27.ExcelGetRow(worksheet1, start);
                repFromEx.Rows27.Add(form27);
                break;
            }
            case "2.8":
            {
                Form28 form28 = new();
                form28.ExcelGetRow(worksheet1, start);
                repFromEx.Rows28.Add(form28);
                break;
            }
            case "2.9":
            {
                Form29 form29 = new();
                form29.ExcelGetRow(worksheet1, start);
                repFromEx.Rows29.Add(form29);
                break;
            }
            case "2.10":
            {
                Form210 form210 = new();
                form210.ExcelGetRow(worksheet1, start);
                repFromEx.Rows210.Add(form210);
                break;
            }
            case "2.11":
            {
                Form211 form211 = new();
                form211.ExcelGetRow(worksheet1, start);
                repFromEx.Rows211.Add(form211);
                break;
            }
            case "2.12":
            {
                Form212 form212 = new();
                form212.ExcelGetRow(worksheet1, start);
                repFromEx.Rows212.Add(form212);
                break;
            }
        }
    }

    private async Task GetDataTitleReps(Reports newRepsFromExcel, ExcelWorksheet worksheet0)
    {
        switch (worksheet0.Name)
        {
            case "1.0":
                newRepsFromExcel.Master_DB.Rows10[0].RegNo_DB = Convert.ToString(worksheet0.Cells["F6"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].OrganUprav_DB = Convert.ToString(worksheet0.Cells["F15"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F16"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLico_DB = Convert.ToString(worksheet0.Cells["F17"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].ShortJurLico_DB = worksheet0.Cells["F18"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["F18"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F19"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F20"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F21"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Telephone_DB = Convert.ToString(worksheet0.Cells["F22"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Fax_DB = Convert.ToString(worksheet0.Cells["F23"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Email_DB = Convert.ToString(worksheet0.Cells["F24"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F25"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLico_DB = Convert.ToString(worksheet0.Cells["F26"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].ShortJurLico_DB = worksheet0.Cells["F27"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["F27"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F29"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Telephone_DB = Convert.ToString(worksheet0.Cells["F30"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Fax_DB = Convert.ToString(worksheet0.Cells["F31"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Email_DB = Convert.ToString(worksheet0.Cells["F32"].Value);

                newRepsFromExcel.Master_DB.Rows10[0].Okpo_DB = worksheet0.Cells["B36"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["B36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okved_DB = Convert.ToString(worksheet0.Cells["C36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okogu_DB = Convert.ToString(worksheet0.Cells["D36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Oktmo_DB = Convert.ToString(worksheet0.Cells["E36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Inn_DB = Convert.ToString(worksheet0.Cells["F36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Kpp_DB = Convert.ToString(worksheet0.Cells["G36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okopf_DB = Convert.ToString(worksheet0.Cells["H36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okfs_DB = Convert.ToString(worksheet0.Cells["I36"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].Okpo_DB = worksheet0.Cells["B37"].Value == null
                    ? ""
                    : Convert.ToString(worksheet0.Cells["B37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okved_DB = Convert.ToString(worksheet0.Cells["C37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okogu_DB = Convert.ToString(worksheet0.Cells["D37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Oktmo_DB = Convert.ToString(worksheet0.Cells["E37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Inn_DB = Convert.ToString(worksheet0.Cells["F37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Kpp_DB = Convert.ToString(worksheet0.Cells["G37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okopf_DB = Convert.ToString(worksheet0.Cells["H37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okfs_DB = Convert.ToString(worksheet0.Cells["I37"].Value);
                break;
            case "2.0":
                newRepsFromExcel.Master_DB.Rows20[0].RegNo.Value = Convert.ToString(worksheet0.Cells["F6"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].OrganUprav_DB = Convert.ToString(worksheet0.Cells["F15"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F16"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLico_DB = Convert.ToString(worksheet0.Cells["F17"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].ShortJurLico_DB = Convert.ToString(worksheet0.Cells["F18"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F19"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F20"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F21"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Telephone_DB = Convert.ToString(worksheet0.Cells["F22"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Fax_DB = Convert.ToString(worksheet0.Cells["F23"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Email_DB = Convert.ToString(worksheet0.Cells["F24"].Value);

                newRepsFromExcel.Master_DB.Rows20[1].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F25"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].JurLico_DB = Convert.ToString(worksheet0.Cells["F26"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].ShortJurLico_DB = Convert.ToString(worksheet0.Cells["F27"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F29"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Telephone_DB = Convert.ToString(worksheet0.Cells["F30"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Fax_DB = Convert.ToString(worksheet0.Cells["F31"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Email_DB = Convert.ToString(worksheet0.Cells["F32"].Value);

                newRepsFromExcel.Master_DB.Rows20[0].Okpo_DB = Convert.ToString(worksheet0.Cells["B36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okved_DB = Convert.ToString(worksheet0.Cells["C36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okogu_DB = Convert.ToString(worksheet0.Cells["D36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Oktmo_DB = Convert.ToString(worksheet0.Cells["E36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Inn_DB = Convert.ToString(worksheet0.Cells["F36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Kpp_DB = Convert.ToString(worksheet0.Cells["G36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okopf_DB = Convert.ToString(worksheet0.Cells["H36"].Value);
                newRepsFromExcel.Master_DB.Rows20[0].Okfs_DB = Convert.ToString(worksheet0.Cells["I36"].Value);

                newRepsFromExcel.Master_DB.Rows20[1].Okpo_DB = Convert.ToString(worksheet0.Cells["B37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okved_DB = Convert.ToString(worksheet0.Cells["C37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okogu_DB = Convert.ToString(worksheet0.Cells["D37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Oktmo_DB = Convert.ToString(worksheet0.Cells["E37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Inn_DB = Convert.ToString(worksheet0.Cells["F37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Kpp_DB = Convert.ToString(worksheet0.Cells["G37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okopf_DB = Convert.ToString(worksheet0.Cells["H37"].Value);
                newRepsFromExcel.Master_DB.Rows20[1].Okfs_DB = Convert.ToString(worksheet0.Cells["I37"].Value);
                break;
        }
    }

    private async Task<Reports> CheckReps(ExcelWorksheet worksheet0)
    {
        IEnumerable<Reports>? reps = worksheet0.Name switch
        {
            "1.0" => LocalReports.Reports_Collection10
                .Where(t => Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[0].Okpo_DB &&
                            Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[0].RegNo_DB),
            "2.0" => LocalReports.Reports_Collection20
                .Where(t => Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows20[0].Okpo_DB &&
                            Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows20[0].RegNo_DB),
            _ => null
        };
        if (reps.Any())
        {
            return reps.FirstOrDefault();
        }

        var newRepsFromExcel = new Reports();
        var param0 = worksheet0.Name;
        newRepsFromExcel.Master_DB = new Report
        {
            FormNum_DB = param0
        };
        switch (param0)
        {
            case "1.0":
            {
                var ty1 = (Form10)FormCreator.Create(param0);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param0);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows10.Add(ty1);
                newRepsFromExcel.Master_DB.Rows10.Add(ty2);
                break;
            }
            case "2.0":
            {
                var ty1 = (Form20)FormCreator.Create(param0);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param0);
                ty2.NumberInOrder_DB = 2;
                newRepsFromExcel.Master_DB.Rows20.Add(ty1);
                newRepsFromExcel.Master_DB.Rows20.Add(ty2);
                break;
            }
        }

        await GetDataTitleReps(newRepsFromExcel, worksheet0);
        LocalReports.Reports_Collection.Add(newRepsFromExcel);
        return newRepsFromExcel;
    }

    private async Task _ImportFrom()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        string[] extensions = { "xlsx", "XLSX" };
        var answer = await GetSelectedFilesFromDialog("Excel", extensions);
        if (answer is null) return;
        foreach (var res in answer)
        {
            if (res is "") continue;
            var fileInfo = new FileInfo(res);
            using ExcelPackage excelPackage = new(fileInfo);
            var worksheet0 = excelPackage.Workbook.Worksheets[0];
            var val = worksheet0.Name == "1.0"
                      && Convert.ToString(worksheet0.Cells["A3"].Value)
                          is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                          or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                      || worksheet0.Name == "2.0"
                      && Convert.ToString(worksheet0.Cells["A4"].Value)
                          is "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"
                          or "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ";
            if (!val)
            {
                #region InvalidDataFormatMessage

                await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                                },
                                ContentTitle = "Импорт из Excel",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    $"Не удалось импортировать данные из {fileInfo.FullName}." +
                                    $"{Environment.NewLine}Не соответствует формат данных!",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow); 

                #endregion

                continue;
            }

            var timeCreate = new List<string>()
            {
                excelPackage.File.CreationTime.Day.ToString(),
                excelPackage.File.CreationTime.Month.ToString(),
                excelPackage.File.CreationTime.Year.ToString()
            };
            if (timeCreate[0].Length == 1)
            {
                timeCreate[0] = $"0{timeCreate[0]}";
            }

            if (timeCreate[1].Length == 1)
            {
                timeCreate[1] = $"0{timeCreate[1]}";
            }

            var baseReps = await CheckReps(worksheet0);
            var worksheet1 = excelPackage.Workbook.Worksheets[1];
            var repNumber = worksheet0.Name;
            var formNumber = worksheet1.Name;
            var impRep = new Report
            {
                FormNum_DB = formNumber,
                ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}"
            };
            if (formNumber.Split('.')[0] == "1")
            {
                impRep.StartPeriod_DB = Convert.ToString(worksheet1.Cells["G3"].Text).Replace("/", ".");
                impRep.EndPeriod_DB = Convert.ToString(worksheet1.Cells["G4"].Text).Replace("/", ".");
                impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G5"].Value);
            }
            else
            {
                switch (formNumber)
                {
                    case "2.6":
                    {
                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                        impRep.SourcesQuantity26_DB = Convert.ToInt32(worksheet1.Cells["G5"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                        break;
                    }
                    case "2.7":
                    {
                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                        impRep.PermissionNumber27_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                        impRep.ValidBegin27_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                        impRep.ValidThru27_DB = Convert.ToString(worksheet1.Cells["J5"].Value);
                        impRep.PermissionDocumentName27_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                        break;
                    }
                    case "2.8":
                    {
                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                        impRep.PermissionNumber_28_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                        impRep.ValidBegin_28_DB = Convert.ToString(worksheet1.Cells["K4"].Value);
                        impRep.ValidThru_28_DB = Convert.ToString(worksheet1.Cells["N4"].Value);
                        impRep.PermissionDocumentName_28_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                        impRep.PermissionNumber1_28_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                        impRep.ValidBegin1_28_DB = Convert.ToString(worksheet1.Cells["K6"].Value);
                        impRep.ValidThru1_28_DB = Convert.ToString(worksheet1.Cells["N6"].Value);
                        impRep.PermissionDocumentName1_28_DB = Convert.ToString(worksheet1.Cells["G7"].Value);
                        impRep.ContractNumber_28_DB = Convert.ToString(worksheet1.Cells["G8"].Value);
                        impRep.ValidBegin2_28_DB = Convert.ToString(worksheet1.Cells["K8"].Value);
                        impRep.ValidThru2_28_DB = Convert.ToString(worksheet1.Cells["N8"].Value);
                        impRep.OrganisationReciever_28_DB = Convert.ToString(worksheet1.Cells["G9"].Value);
                        impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells["D21"].Value);
                        impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells["F21"].Value);
                        impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells["I21"].Value);
                        impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells["K21"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                        break;
                    }
                    default:
                    {
                        impRep.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                        impRep.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Text);
                        break;
                    }
                }
            }

            impRep.GradeExecutor_DB = Convert.ToString(worksheet1.Cells[$"D{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.FIOexecutor_DB = Convert.ToString(worksheet1.Cells[$"F{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecPhone_DB = Convert.ToString(worksheet1.Cells[$"I{worksheet1.Dimension.Rows - 1}"].Value);
            impRep.ExecEmail_DB = Convert.ToString(worksheet1.Cells[$"K{worksheet1.Dimension.Rows - 1}"].Value);
            var start = formNumber is "2.8"
                ? 14
                : 11;
            var end = $"A{start}";

            var value = worksheet1.Cells[end].Value;
            while (value != null && Convert.ToString(value)?.ToLower() is not ("примечание:" or "примечания:"))
            {
                await GetDataFromRow(formNumber, worksheet1, start, impRep);
                start++;
                end = $"A{start}";
                value = worksheet1.Cells[end].Value;
            }

            if (value is null)
                start += 3;
            else if (Convert.ToString(value)?.ToLower() is "примечание:" or "примечания:")
                start += 2;
            while (worksheet1.Cells[$"A{start}"].Value != null ||
                   worksheet1.Cells[$"B{start}"].Value != null ||
                   worksheet1.Cells[$"C{start}"].Value != null)
            {
                Note newNote = new();
                newNote.ExcelGetRow(worksheet1, start);
                impRep.Notes.Add(newNote);
                start++;
            }

            skipNewOrg = false;
            skipInter = false;
            skipLess = false;
            skipNew = false;
            skipReplace = false;
            hasMultipleReport = answer.Length > 1;
            if (baseReps.Report_Collection.Count != 0)
            {
                switch (worksheet0.Name)
                {
                    case "1.0":
                    {
                        await ProcessIfHasReports11(baseReps, null, impRep);
                        break;
                    }
                    case "2.0":
                    {
                        await ProcessIfHasReports21(baseReps, null, impRep);
                        break;
                    }
                }
            }
            else
            {
                #region AddNewOrg

                var an = "Добавить";
                if (!skipNewOrg)
                {
                    if (answer.Length > 1)
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
                                    $"Будет добавлена новая организация ({repNumber}), содержащая отчет по форме {impRep.FormNum_DB}." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
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
                                    $"Будет добавлена новая организация ({repNumber})." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion
                    }
                }

                if (an is "Добавить" or "Да для всех")
                {
                    baseReps.Report_Collection.Add(impRep);
                }

                #endregion
            }

            var dbm = StaticConfiguration.DBModel;
            await dbm.SaveChangesAsync();
        }
    }

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

    private async Task ProcessIfHasReports11(Reports baseReps, Reports? impReps = null, Report? impReport = null)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var listImpRep = new List<Report>();
        if (impReps != null)
        {
            listImpRep.AddRange(impReps.Report_Collection);
        }
        if (impReport != null)
        {
            listImpRep.Add(impReport);
        }
        foreach (var impRep in listImpRep) //Для каждой импортируемой формы
        {
            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе ищем совпадение
            {
                var baseRep = (Report)key1;

                #region Periods

                var stBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у отчета в базе
                var endBase = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у отчета в базе
                try
                {
                    stBase = DateTime.Parse(baseRep.StartPeriod_DB) > DateTime.Parse(baseRep.EndPeriod_DB)
                        ? DateTime.Parse(baseRep.EndPeriod_DB)
                        : DateTime.Parse(baseRep.StartPeriod_DB);
                    endBase = DateTime.Parse(baseRep.StartPeriod_DB) < DateTime.Parse(baseRep.EndPeriod_DB)
                        ? DateTime.Parse(baseRep.EndPeriod_DB)
                        : DateTime.Parse(baseRep.StartPeriod_DB);
                }
                catch (Exception)
                {
                    // ignored
                }

                var stImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Начало периода у импортируемого отчета
                var endImp = DateTime.Parse(DateTime.Now.ToShortDateString()); //Конец периода у импортируемого отчета
                try
                {
                    stImp = DateTime.Parse(impRep.StartPeriod_DB) > DateTime.Parse(impRep.EndPeriod_DB)
                        ? DateTime.Parse(impRep.EndPeriod_DB)
                        : DateTime.Parse(impRep.StartPeriod_DB);
                    endImp = DateTime.Parse(impRep.StartPeriod_DB) < DateTime.Parse(impRep.EndPeriod_DB)
                        ? DateTime.Parse(impRep.EndPeriod_DB)
                        : DateTime.Parse(impRep.StartPeriod_DB);
                }
                catch (Exception)
                {
                    // ignored
                }

                #endregion

                #region SamePeriod

                if (stBase == stImp && endBase == endImp && impRep.FormNum_DB == baseRep.FormNum_DB)
                {
                    impInBase = true;

                    #region LessCorrectionNumber

                    if (impRep.CorrectionNumber_DB < baseRep.CorrectionNumber_DB)
                    {
                        if (skipLess) break;

                        #region MessageImportReportHasLowerCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                    new ButtonDefinition { Name = "Пропустить для всех" }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Отчет не будет импортирован, поскольку вы пытаетесь загрузить форму" +
                                    $"{Environment.NewLine}с меньшим номером корректировки, чем у текущего отчета в базе." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                    $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion

                        if (res is "Пропустить для всех") skipLess = true;
                        break;
                    }

                    #endregion

                    #region SameCorrectionNumber

                    if (impRep.CorrectionNumber_DB == baseRep.CorrectionNumber_DB)
                    {
                        #region MessageImportReportHasSamePeriodCorrectionNumberAndExportDate

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Дополнить" },
                                    new ButtonDefinition { Name = "Сохранить оба" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет тот же период, номер корректировки, что и имеющийся в базе." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                    $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion

                        await ChechAanswer(res, baseReps, baseRep, impRep);
                        break;
                    }

                    #endregion

                    #region HigherCorrectionNumber

                    res = "Заменить";
                    if (!skipReplace)
                    {
                        if (hasMultipleReport)
                        {
                            #region MessageImportReportHasHigherCorrectionNumber

                            res = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Заменять все формы" },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                        $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                        $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Кнопка \"Заменять все формы\" заменит без уведомлений" +
                                        $"{Environment.NewLine}все формы с меньшим номером корректировки.",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(desktop.MainWindow);

                            #endregion

                            if (res is "Заменять все формы") skipReplace = true;
                        }
                        else
                        {
                            #region MessageImportReportHasHigherCorrectionNumber

                            res = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                    },
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        "Импортируемый отчет имеет больший номер корректировки чем имеющийся в базе." +
                                        $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                        $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                        $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                        $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                        $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                        $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                        $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                        $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(desktop.MainWindow);

                            #endregion
                        }
                    }

                    await ChechAanswer(res, baseReps, baseRep, impRep);
                    break;

                    #endregion
                }

                #endregion

                #region Intersect

                if (stBase < endImp && endBase > stImp && impRep.FormNum_DB == baseRep.FormNum_DB)
                {
                    impInBase = true;

                    if (skipInter) break;

                    #region MessagePeriodsIntersect

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Сохранить оба", IsDefault = true },
                                new ButtonDefinition { Name = "Отменить для всех пересечений" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Периоды импортируемого и имеющегося в базе отчетов пересекаются, но не совпадают." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Начало периода отчета в базе - {baseRep.StartPeriod_DB}" +
                                $"{Environment.NewLine}Конец периода отчета в базе - {baseRep.EndPeriod_DB}" +
                                $"{Environment.NewLine}Начало периода импортируемого отчета - {impRep.StartPeriod_DB}" +
                                $"{Environment.NewLine}Конец периода импортируемого отчета - {impRep.EndPeriod_DB}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета- {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion

                    if (res is "Отменить для всех пересечений")
                    {
                        skipInter = true;
                        break;
                    }

                    if (res is "Отменить импорт формы") break;

                    await ChechAanswer(res, baseReps, null, impRep);
                    break;
                }

                #endregion
            }

            #region TryAddEmptyOrg

            if (impReps?.Report_Collection.Count == 0)
            {
                impInBase = true;

                #region MessageNewReport

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        },
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Импортируемая организация не содержит отчетов и уже присутствует в базе." +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                            $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                            $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(desktop.MainWindow);

                #endregion
            }

            #endregion

            if (impInBase) continue;

            #region AddNewForm

            res = "Да";
            if (!skipNew)
            {
                if (hasMultipleReport)
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений импортировать" +
                                $"{Environment.NewLine}все новые формы для уже имеющихся в базе организаций.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion

                    if (res is "Да для всех") skipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Начало отчетного периода - {impRep.StartPeriod_DB}" +
                                $"{Environment.NewLine}Конец отчетного периода - {impRep.EndPeriod_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion
                }
            }

            await ChechAanswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    private async Task ProcessIfHasReports21(Reports baseReps, Reports? impReps = null, Report? impReport = null)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var listImpRep = new List<Report>();
        if (impReps != null)
        {
            listImpRep.AddRange(impReps.Report_Collection);
        }
        if (impReport != null)
        {
            listImpRep.Add(impReport);
        }
        foreach (var key in listImpRep) //Для каждой импортируемой формы
        {
            var impRep = (Report)key;
            var impInBase = false; //Импортируемая форма заменяет/пересекает имеющуюся в базе
            string? res;
            foreach (var key1 in baseReps.Report_Collection) //Для каждой формы соответствующей организации в базе
            {
                var baseRep = (Report)key1;
                if (baseRep.Year_DB != impRep.Year_DB || impRep.FormNum_DB != baseRep.FormNum_DB) continue;
                impInBase = true;

                #region LessCorrectionNumber

                if (impRep.CorrectionNumber_DB < baseRep.CorrectionNumber_DB)
                {
                    if (skipLess) break;

                    #region MessageImportReportHasLowerCorrectionNumber

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true },
                                new ButtonDefinition { Name = "Пропустить для всех" }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Отчет не будет импортирован, поскольку вы пытаетесь загрузить форму" +
                                $"{Environment.NewLine}с меньшим номером корректировки, чем у текущего отчета в базе." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Пропустить для всех\" позволяет не показывать данное уведомление для всех случаев," +
                                $"{Environment.NewLine}когда номер корректировки импортируемого отчета меньше, чем у имеющегося в базе.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion

                    if (res == "Пропустить для всех") skipLess = true;
                    break;
                }

                #endregion

                #region SameCorrectionNumber

                if (impRep.CorrectionNumber_DB == baseRep.CorrectionNumber_DB)
                {
                    #region MessageImportReportHasSameYearCorrectionNumberAndExportDate

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                new ButtonDefinition { Name = "Дополнить" },
                                new ButtonDefinition { Name = "Сохранить оба" },
                                new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортируемый отчет имеет тот же год и номер корректировки, что и имеющийся в базе." +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion

                    await ChechAanswer(res, baseReps, baseRep, impRep);
                    break;
                }

                #endregion

                #region HigherCorrectionNumber

                res = "Заменить";
                if (!skipReplace)
                {
                    if (hasMultipleReport)
                    {
                        #region MessageImportReportHasHigherCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Заменять все формы" },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                    $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Кнопка \"Заменять все формы\" заменит без уведомлений" +
                                    $"{Environment.NewLine}все формы с меньшим номером корректировки.",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion

                        if (res is "Заменять все формы") skipReplace = true;
                    }
                    else
                    {
                        #region MessageImportReportHasHigherCorrectionNumber

                        res = await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                ButtonDefinitions = new[]
                                {
                                    new ButtonDefinition { Name = "Заменить", IsDefault = true },
                                    new ButtonDefinition { Name = "Отменить импорт формы", IsCancel = true }
                                },
                                ContentTitle = "Импорт из .raodb",
                                ContentHeader = "Уведомление",
                                ContentMessage =
                                    "Импортируемый отчет имеет больший номер корректировки, чем имеющийся в базе." +
                                    $"{Environment.NewLine}Форма с предыдущим номером корректировки будет безвозвратно удалена." +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                    $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                    $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                    $"{Environment.NewLine}" +
                                    $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                    $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки отчета в базе - {baseRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Дата выгрузки импортируемого отчета - {impRep.ExportDate_DB}" +
                                    $"{Environment.NewLine}Номер корректировки отчета в базе - {baseRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Номер корректировки импортируемого отчета - {impRep.CorrectionNumber_DB}" +
                                    $"{Environment.NewLine}Количество строк отчета в базе - {baseRep.Rows.Count}{InventoryCheck(baseRep)}" +
                                    $"{Environment.NewLine}Количество строк импортируемого отчета - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                                MinWidth = 400,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion
                    }

                    if (res is "Отменить импорт формы") break;
                }

                await ChechAanswer(res, baseReps, baseRep, impRep);
                break;

                #endregion
            }

            #region TryAddEmptyOrg

            if (impReps?.Report_Collection.Count == 0)
            {
                impInBase = true;

                #region MessageNewReport

                await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition { Name = "Ок", IsDefault = true, IsCancel = true }
                        },
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Импортируемая организация не содержит отчетов и уже присутствует в базе." +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                            $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                            $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(desktop.MainWindow);

                #endregion
            }

            #endregion

            if (impInBase) continue;

            #region AddNewForm

            res = "Да";
            if (!skipNew)
            {
                if (hasMultipleReport)
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Да для всех" },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений импортировать" +
                                $"{Environment.NewLine}все новые формы для уже имеющихся в базе организаций.",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion

                    if (res == "Да для всех") skipNew = true;
                }
                else
                {
                    #region MessageNewReport

                    res = await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions = new[]
                            {
                                new ButtonDefinition { Name = "Да", IsDefault = true },
                                new ButtonDefinition { Name = "Нет", IsCancel = true }
                            },
                            ContentTitle = "Импорт из .raodb",
                            ContentHeader = "Уведомление",
                            ContentMessage =
                                "Импортировать новый отчет в уже имеющуюся в базе организацию?" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Регистрационный номер - {baseReps.Master.RegNoRep.Value}" +
                                $"{Environment.NewLine}ОКПО - {baseReps.Master.OkpoRep.Value}" +
                                $"{Environment.NewLine}Сокращенное наименование - {baseReps.Master.ShortJurLicoRep.Value}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Номер формы - {impRep.FormNum_DB}" +
                                $"{Environment.NewLine}Отчетный год - {impRep.Year_DB}" +
                                $"{Environment.NewLine}Дата выгрузки - {impRep.ExportDate_DB}" +
                                $"{Environment.NewLine}Номер корректировки - {impRep.CorrectionNumber_DB}" +
                                $"{Environment.NewLine}Количество строк - {impRep.Rows.Count}{InventoryCheck(impRep)}",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(desktop.MainWindow);

                    #endregion
                }
            }

            await ChechAanswer(res, baseReps, null, impRep);

            #endregion
        }

        await baseReps.SortAsync();
    }

    private async Task ChechAanswer(string an, Reports first, Report? elem = null, Report? it = null, bool doSomething = false)
    {
        switch (an)
        {
            case "Сохранить оба" or "Да" or "Да для всех":
            {
                if (!doSomething)
                    first.Report_Collection.Add(it);
                break;
            }
            case "Заменить" or "Заменять все формы" or "Загрузить новую" or "Загрузить новую форму":
                first.Report_Collection.Remove(elem);
                first.Report_Collection.Add(it);
                break;
            case "Дополнить" when it != null && elem != null:
                first.Report_Collection.Remove(elem);
                it.Rows.AddRange<IKey>(0, elem.Rows.GetEnumerable());
                it.Notes.AddRange<IKey>(0, elem.Notes);
                first.Report_Collection.Add(it);
                break;
        }
    }

    private bool skipNewOrg; //Пропустить уведомления о добавлении новой организации
    private bool skipInter; //Пропускать уведомления и отменять импорт при пересечении дат
    private bool skipLess; //Пропускать уведомления о том, что номер корректировки у импортируемого отчета меньше
    private bool skipNew; //Пропускать уведомления о добавлении новой формы для уже имеющейся в базе организации
    private bool skipReplace; //Пропускать уведомления о замене форм
    private bool hasMultipleReport; //Имеет множество форм

    private async Task _ImportForm()
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

    #region ChangeForm

    public ReactiveCommand<object, Unit> ChangeForm { get; private set; }

    private async Task _ChangeForm(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && par is ObservableCollectionWithItemPropertyChanged<IKey> param
            && param.First() is { } obj)
        {
            var t = desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            var rep = (Report)obj;
            var tre = LocalReports.Reports_Collection
                .FirstOrDefault(i => i.Report_Collection.Contains(rep));
            var numForm = rep.FormNum.Value;
            var frm = new ChangeOrCreateVM(numForm, rep, tre, LocalReports);
            switch (numForm)
            {
                case "2.1":
                {
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        //var sumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                        await frm.UnSum21();
                        await frm.Sum21();
                        //var newSumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                    }

                    break;
                }
                case "2.2":
                {
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        var sumRow = frm.Storage.Rows22.Where(x => x.Sum_DB).ToList();
                        Dictionary<long, List<string>> dic = new();
                        foreach (var oldR in sumRow)
                        {
                            dic[oldR.NumberInOrder_DB] = new List<string>
                                { oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB };
                        }

                        await frm.UnSum22();
                        await frm.Sum22();
                        var newSumRow = frm.Storage.Rows22.Where(x => x.Sum_DB);
                        foreach (var newR in newSumRow)
                        {
                            foreach (var oldR in dic.Where(oldR => newR.NumberInOrder_DB == oldR.Key))
                            {
                                newR.PackQuantity_DB = oldR.Value[0];
                                newR.VolumeInPack_DB = oldR.Value[1];
                                newR.MassInPack_DB = oldR.Value[2];
                            }
                        }
                    }

                    break;
                }
            }

            await ShowDialog.Handle(frm);
            t.SelectedReports = tmp;
        }
    }

    #endregion

    #region ChangeReport

    public ReactiveCommand<object, Unit> ChangeReport { get; private set; }

    private async Task _ChangeReport(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            && par is ObservableCollectionWithItemPropertyChanged<IKey> param
            && param.First() is { } obj)
        {
            var t = desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            var rep = (Reports)obj;
            var frm = new ChangeOrCreateVM(rep.Master.FormNum.Value, rep.Master, rep, LocalReports);
            await ShowDialog.Handle(frm);

            //Local_Reports.Reports_Collection.Sorted = false;
            //await Local_Reports.Reports_Collection.QuickSortAsync();
            t.SelectedReports = tmp;
        }
    }

    #endregion

    #region DeleteForm

    public ReactiveCommand<object, Unit> DeleteForm { get; private set; }

    private async Task _DeleteForm(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var answer = await ShowMessage.Handle(new List<string>
                { "Вы действительно хотите удалить отчет?", "Уведомление", "Да", "Нет" });
            if (answer == "Да")
            {
                var t = desktop.MainWindow as MainWindow;
                if (t.SelectedReports.Count() != 0)
                {
                    var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
                    var y = t.SelectedReports.First() as Reports;
                    if (par is IEnumerable param)
                    {
                        foreach (var item in param)
                        {
                            y.Report_Collection.Remove((Report)item);
                        }
                    }

                    t.SelectedReports = tmp;
                }

                await StaticConfiguration.DBModel.SaveChangesAsync();
            }
        }
    }

    #endregion

    #region DeleteReport

    public ReactiveCommand<object, Unit> DeleteReport { get; private set; }

    private async Task _DeleteReport(object par)
    {
        //if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var answer = await ShowMessage.Handle(new List<string>
                { "Вы действительно хотите удалить организацию?", "Уведомление", "Да", "Нет" });
            if (answer == "Да")
            {
                if (par is IEnumerable param)
                {
                    //var t = desktop.MainWindow as MainWindow;

                    foreach (var item in param)
                    {
                        LocalReports.Reports_Collection.Remove((Reports)item);
                    }
                }

                await StaticConfiguration.DBModel.SaveChangesAsync();
            }
            //await Local_Reports.Reports_Collection.QuickSortAsync();
        }
    }

    #endregion

    #region SaveReport

    public ReactiveCommand<object, Unit> SaveReport { get; private set; }

    private async Task _SaveReport(object par)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime)
        {
            await StaticConfiguration.DBModel.SaveChangesAsync();
            await LocalReports.Reports_Collection.QuickSortAsync();
        }
    }

    #endregion

    #region INotifyPropertyChanged

    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}