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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Client_App.ViewModels;

public class MainWindowVM : BaseVM, INotifyPropertyChanged
{
    #region Local_Reports

    private DBObservable _local_Reports = new();

    public DBObservable Local_Reports
    {
        get => _local_Reports;
        set
        {
            if (_local_Reports != value)
            {
                _local_Reports = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Current_Db

    private string dbFileName;

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

    #region Init

    private async Task<string> ProcessRaoDirectory(string systemDirectory)
    {
        var tmp = "";
        var pty = "";
        try
        {
            tmp = Path.Combine(systemDirectory, "RAO");
            pty = tmp;
            tmp = Path.Combine(tmp, "temp");
            Directory.CreateDirectory(tmp);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error2);
            throw new Exception(ErrorMessages.Error2[0]);
        }
        var fl = Directory.GetFiles(tmp, ".");
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
        return pty;
    }

    private async Task<string> GetSystemDirectory()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                return Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))!;
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
            return Path.Combine("/home", userName!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await ShowMessage.Handle(ErrorMessages.Error1);
            throw new Exception(ErrorMessages.Error1[0]);
        }
    }

    private static Task ProcessSpravochniks()
    {
        var a = Spravochniks.SprRadionuclids;
        var b = Spravochniks.SprTypesToRadionuclids;
        return Task.CompletedTask;
    }

    private async Task ProcessDataBaseCreate(string tempDirectory)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var i = 0;
        var loadDbFile = false;
        DBModel dbm;
        DirectoryInfo dirInfo = new(tempDirectory);
        foreach (var fileInfo in dirInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                     .Where(x => x.Name.ToLower().EndsWith(".raodb"))
                     .OrderByDescending((x => x.LastWriteTime)))
        {
            try
            {
                dbFileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                Current_Db = $"Интерактивное пособие по вводу данных ver.{Version} Текущая база данных - {dbFileName}";
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
            dbFileName = $"Local_{i}";
            Current_Db = $"Интерактивное пособие по вводу данных ver.{Version} Текущая база данных - {dbFileName}";
            StaticConfiguration.DBPath = Path.Combine(tempDirectory, $"{dbFileName}.RAODB");
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
        foreach (var key in Local_Reports.Reports_Collection)
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

        await Local_Reports.Reports_Collection.QuickSortAsync();
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
        Print_Excel_Export = ReactiveCommand.CreateFromTask<object>(_Print_Excel_Export);
        Excel_Export = ReactiveCommand.CreateFromTask<object>(_Excel_Export);
        All_Excel_Export = ReactiveCommand.CreateFromTask<object>(_All_Excel_Export);
        AllForms1_Excel_Export = ReactiveCommand.CreateFromTask(_AllForms1_Excel_Export);
        AllForms2_Excel_Export = ReactiveCommand.CreateFromTask(_AllForms2_Excel_Export);
        Statistic_Excel_Export = ReactiveCommand.CreateFromTask(_Statistic_Excel_Export);
        SelectOrgExcelExport = ReactiveCommand.CreateFromTask(_SelectOrgExcelExport);
        AllOrganization_Excel_Export = ReactiveCommand.CreateFromTask(_AllOrganization_Excel_Export);
        ExcelMissingPas = ReactiveCommand.CreateFromTask<object>(_ExcelMissingPas);
        ExcelPasWithoutRep = ReactiveCommand.CreateFromTask<object>(_ExcelPasWithoutRep);
        ChangePasDir = ReactiveCommand.CreateFromTask<object>(_ChangePasDir);
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
        var systemDirectory = await GetSystemDirectory();

        onStartProgressBarVm.LoadStatus = "Создание временных файлов";
        OnStartProgressBar = 5;
        var raoDirectory = await ProcessRaoDirectory(systemDirectory);

        onStartProgressBarVm.LoadStatus = "Загрузка справочников";
        OnStartProgressBar = 10;
        await ProcessSpravochniks();

        onStartProgressBarVm.LoadStatus = "Создание базы данных";
        OnStartProgressBar = 15;
        await ProcessDataBaseCreate(raoDirectory);
        
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
        Local_Reports = dbm.DBObservableDbSet.Local.First();
        await ProcessDataBaseFillNullOrder();

        onStartProgressBarVm.LoadStatus = "Сохранение";
        OnStartProgressBar = 90;
        await dbm.SaveChangesAsync();
        Local_Reports.PropertyChanged += Local_ReportsChanged;

        onStartProgressBarVm.LoadStatus = "Инициализация";
        OnStartProgressBar = 95;
        await PropertiesInit();

        OnStartProgressBar = 100;
    }

    private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Local_Reports));
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
            ChangeOrCreateVM frm = new(param, Local_Reports);
            await ShowDialog.Handle(frm);
            t.SelectedReports = tmp;
            await Local_Reports.Reports_Collection.QuickSortAsync();
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
            "1.0" => Local_Reports.Reports_Collection10
                .Where(t => Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[0].Okpo_DB &&
                            Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[0].RegNo_DB),
            "2.0" => Local_Reports.Reports_Collection20
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
        Local_Reports.Reports_Collection.Add(newRepsFromExcel);
        return newRepsFromExcel;
    }

    private async Task _ImportFrom()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var answer = await GetSelectedFilesFromDialog("Excel", "xlsx");
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

    private async Task<string> GetRaoFileName()
    {
        var tmp = await GetTempDirectory(await GetSystemDirectory());
        var count = 0;
        string? file;
        do
        {
            file = Path.Combine(tmp, $"file_imp_{count++}.raodb");
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

            return Local_Reports.Reports_Collection10
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

                   ?? Local_Reports
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

            return Local_Reports.Reports_Collection20
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

                   ?? Local_Reports.Reports_Collection20 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
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
            var answer = await GetSelectedFilesFromDialog("RAODB", "raodb");
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
                            Local_Reports.Reports_Collection.Add(item);
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
            await Local_Reports.Reports_Collection.QuickSortAsync();
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

    private static string InventoryCheck(Report? rep)
    {
        if (rep is null)
        {
            return "";
        }

        var countCode10 = 0;
        foreach (var key in rep.Rows)
        {
            if (key is Form1 { OperationCode_DB: "10" })
            {
                countCode10++;
            }
        }

        return countCode10 == rep.Rows.Count
            ? " (ИНВ)"
            : countCode10 > 0
                ? " (инв)"
                : "";
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

        var reps = Local_Reports.Reports_Collection
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
        if (Local_Reports.Reports_Collection.Count > 10)
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
                        $"Текущая база содержит {Local_Reports.Reports_Collection.Count} форм организаций," +
                        $"{Environment.NewLine}выгрузка займет примерно {Local_Reports.Reports_Collection.Count / 20} минут",
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
        Parallel.ForEach(Local_Reports.Reports_Collection, async exportOrg =>
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
            var tre = Local_Reports.Reports_Collection
                .FirstOrDefault(i => i.Report_Collection.Contains(rep));
            var numForm = rep.FormNum.Value;
            var frm = new ChangeOrCreateVM(numForm, rep, tre, Local_Reports);
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
            var frm = new ChangeOrCreateVM(rep.Master.FormNum.Value, rep.Master, rep, Local_Reports);
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
                        Local_Reports.Reports_Collection.Remove((Reports)item);
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
            await Local_Reports.Reports_Collection.QuickSortAsync();
        }
    }

    #endregion

    #region Excel

    private ExcelWorksheet worksheet { get; set; }
    private ExcelWorksheet worksheetComment { get; set; }

    private static string StringReverse(string str)
    {
        var charArray = str.Replace("_", "0").Replace("/", ".").Split(".");
        Array.Reverse(charArray);
        return string.Join("", charArray);
    }

    #region StatisticExcelExport //Excel-Разрывы и пересечения

    public ReactiveCommand<Unit, Unit> Statistic_Excel_Export { get; private set; }

    private async Task _Statistic_Excel_Export()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        #region ReportsCountCheck

        var findRep = 0;
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "1")
                {
                    findRep++;
                }
            }
        }
        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка разрывов и пересечений дат," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 1",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        }

        #endregion

        var cts = new CancellationTokenSource();
        var exportType = "Разрывы и пересечения";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;
        

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (Local_Reports.Reports_Collection.Count == 0) return;

        worksheet = excelPackage.Workbook.Worksheets.Add("Разрывы и пересечения");

        #region Headers

        worksheet.Cells[1, 1].Value = "Рег.№";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Начало прошлого периода";
        worksheet.Cells[1, 6].Value = "Конец прошлого периода";
        worksheet.Cells[1, 7].Value = "Начало периода";
        worksheet.Cells[1, 8].Value = "Конец периода";
        worksheet.Cells[1, 9].Value = "Зона разрыва";
        worksheet.Cells[1, 10].Value = "Вид несоответствия"; 

        #endregion

        if (OperatingSystem.IsWindows()) worksheet.Column(3).AutoFit();   // Под Astra Linux эта команда крашит программу без GDI дров

        var listSortRep = new List<ReportForSort>();
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] != "1") continue;
            foreach (var key1 in item.Report_Collection)
            {
                var rep = (Report)key1;
                var start = StringReverse(rep.StartPeriod_DB);
                var end = StringReverse(rep.EndPeriod_DB);
                if (!long.TryParse(start, out var startL) || !long.TryParse(end, out var endL)) continue;
                listSortRep.Add(new ReportForSort
                {
                    RegNoRep = item.Master_DB.RegNoRep.Value ?? "",
                    OkpoRep = item.Master_DB.OkpoRep.Value ?? "",
                    FormNum = rep.FormNum_DB,
                    StartPeriod = startL,
                    EndPeriod = endL,
                    ShortYr = item.Master_DB.ShortJurLicoRep.Value
                });
            }
        }

        var newGen = listSortRep
            .GroupBy(x => x.RegNoRep)
            .ToDictionary(gr => gr.Key, gr => gr
                .ToList()
                .GroupBy(x => x.FormNum)
                .ToDictionary(gr => gr.Key, gr => gr
                    .ToList()
                    .OrderBy(elem => elem.EndPeriod)));
        var row = 2;
        foreach (var grp in newGen)
        {
            foreach (var gr in grp.Value)
            {
                var prevEnd = gr.Value.FirstOrDefault()?.EndPeriod;
                var prevStart = gr.Value.FirstOrDefault()?.StartPeriod;
                var newGr = gr.Value.Skip(1).ToList();
                foreach (var g in newGr)
                {
                    if (g.StartPeriod != prevEnd && g.StartPeriod != prevStart && g.EndPeriod != prevEnd)
                    {
                        if (g.StartPeriod < prevEnd)
                        {
                            var prevEndN = prevEnd.ToString()?.Length == 8
                                ? prevEnd.ToString()
                                : prevEnd == 0
                                    ? "нет даты конца периода"
                                    : prevEnd.ToString()?.Insert(6, "0");
                            var prevStartN = prevStart.ToString()?.Length == 8
                                ? prevStart.ToString()
                                : prevStart == 0
                                    ? "нет даты начала периода"
                                    : prevStart.ToString()?.Insert(6, "0");
                            var stPer = g.StartPeriod.ToString().Length == 8
                                ? g.StartPeriod.ToString()
                                : g.StartPeriod.ToString().Insert(6, "0");
                            var endPer = g.EndPeriod.ToString().Length == 8
                                ? g.EndPeriod.ToString()
                                : g.EndPeriod.ToString().Insert(6, "0");
                            worksheet.Cells[row, 1].Value = g.RegNoRep;
                            worksheet.Cells[row, 2].Value = g.OkpoRep;
                            worksheet.Cells[row, 3].Value = g.ShortYr;
                            worksheet.Cells[row, 4].Value = g.FormNum;
                            worksheet.Cells[row, 5].Value = prevStartN.Equals("нет даты начала периода")
                                ? prevStartN
                                : $"{prevStartN[6..8]}.{prevStartN[4..6]}.{prevStartN[..4]}";
                            worksheet.Cells[row, 6].Value = prevEndN.Equals("нет даты конца периода")
                                ? prevEndN
                                : $"{prevEndN[6..8]}.{prevEndN[4..6]}.{prevEndN[..4]}";
                            worksheet.Cells[row, 7].Value = $"{stPer[6..8]}.{stPer[4..6]}.{stPer[..4]}";
                            worksheet.Cells[row, 8].Value = $"{endPer[6..8]}.{endPer[4..6]}.{endPer[..4]}";
                            worksheet.Cells[row, 9].Value =
                                $"{worksheet.Cells[row, 7].Value}-{worksheet.Cells[row, 6].Value}";
                            worksheet.Cells[row, 10].Value = "пересечение";
                            row++;
                        }
                        else
                        {
                            var prevEndN = prevEnd?.ToString().Length == 8
                                ? prevEnd.ToString()
                                : prevEnd == 0
                                    ? "нет даты конца периода"
                                    : prevEnd?.ToString().Insert(6, "0");
                            var prevStartN = prevStart?.ToString().Length == 8
                                ? prevStart.ToString()
                                : prevStart == 0
                                    ? "нет даты начала периода"
                                    : prevStart?.ToString().Insert(6, "0");
                            var stPer = g.StartPeriod.ToString().Length == 8
                                ? g.StartPeriod.ToString()
                                : g.StartPeriod.ToString().Insert(6, "0");
                            var endPer = g.EndPeriod.ToString().Length == 8
                                ? g.EndPeriod.ToString()
                                : g.EndPeriod.ToString().Insert(6, "0");
                            worksheet.Cells[row, 1].Value = g.RegNoRep;
                            worksheet.Cells[row, 2].Value = g.OkpoRep;
                            worksheet.Cells[row, 3].Value = g.ShortYr;
                            worksheet.Cells[row, 4].Value = g.FormNum;
                            worksheet.Cells[row, 5].Value = prevStartN.Equals("нет даты начала периода")
                                ? prevStartN
                                : $"{prevStartN[6..8]}.{prevStartN[4..6]}.{prevStartN[..4]}";
                            worksheet.Cells[row, 6].Value = prevEndN.Equals("нет даты конца периода")
                                ? prevEndN
                                : $"{prevEndN[6..8]}.{prevEndN[4..6]}.{prevEndN[..4]}";
                            worksheet.Cells[row, 7].Value = $"{stPer[6..8]}.{stPer[4..6]}.{stPer[..4]}";
                            worksheet.Cells[row, 8].Value =
                                $"{endPer[6..8]}.{endPer[4..6]}.{endPer[..4]}";
                            worksheet.Cells[row, 9].Value =
                                $"{worksheet.Cells[row, 6].Value}-{worksheet.Cells[row, 7].Value}";
                            worksheet.Cells[row, 10].Value = "разрыв";
                            row++;
                        }
                    }

                    prevEnd = g.EndPeriod;
                    prevStart = g.StartPeriod;
                }
            }
        }

        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (worksheet.Cells[1, col].Value is "Сокращенное наименование") continue;

            if (OperatingSystem.IsWindows()) worksheet.Column(col).AutoFit();
        }

        worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region Excel_Export //SelectedForm-Выгрузка Excel-Для анализа

    public ReactiveCommand<object, Unit> Excel_Export { get; private set; }

    private async Task _Excel_Export(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
            par is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;

        var exportForm = (Report)forms.First();
        var orgWithExportForm = Local_Reports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection.Contains(exportForm));
        var formNum = exportForm.FormNum_DB;
        if (formNum is "" || forms.Count == 0 || orgWithExportForm is null) return;
        var exportType = "Для анализа";
        var regNum = RemoveForbiddenChars(orgWithExportForm.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(orgWithExportForm.Master.OkpoRep.Value);
        var corNum = Convert.ToString(exportForm.CorrectionNumber_DB);
        var cts = new CancellationTokenSource();
        string fileName;
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(exportForm.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(exportForm.EndPeriod_DB);
                fileName = $"{exportType}_{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Version}";
                break;
            case '2':
                var year = RemoveForbiddenChars(exportForm.Year_DB);
                fileName = $"{exportType}_{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Version}";
                break;
            default:
                return;
        }

        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (forms?.Count == 0) return;
        worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {formNum}");
        var worksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {formNum}");
        int masterHeaderLength;
        if (formNum.Split('.')[0] == "1")
        {
            Form10.ExcelHeader(worksheet, 1, 1);
            masterHeaderLength = Form10.ExcelHeader(worksheetPrim, 1, 1);
        }
        else
        {
            Form20.ExcelHeader(worksheet, 1, 1);
            masterHeaderLength = Form20.ExcelHeader(worksheetPrim, 1, 1);
        }

        var tmpLength = Report.ExcelHeader(worksheet, formNum, 1, masterHeaderLength + 1);
        Report.ExcelHeader(worksheetPrim, formNum, 1, masterHeaderLength + 1);
        masterHeaderLength += tmpLength;

        #region ExcelHeaders

        switch (formNum)
        {
            case "1.1":
                Form11.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.2":
                Form12.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.3":
                Form13.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.4":
                Form14.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.5":
                Form15.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.6":
                Form16.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.7":
                Form17.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.8":
                Form18.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.9":
                Form19.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.1":
                Form21.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.2":
                Form22.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.3":
                Form23.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.4":
                Form24.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.5":
                Form25.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.6":
                Form26.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.7":
                Form27.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.8":
                Form28.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.9":
                Form29.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.10":
                Form210.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.11":
                Form211.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.12":
                Form212.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
        }
        Note.ExcelHeader(worksheetPrim, 1, masterHeaderLength + 1); 

        #endregion

        if (OperatingSystem.IsWindows())
        {
            worksheet.Cells.AutoFitColumns();
            worksheetPrim.Cells.AutoFitColumns();
        }

        var exportFormList = new List<Report> { exportForm };
        _Excel_Export_Rows(formNum, 2, masterHeaderLength, worksheet, exportFormList);
        if (formNum is "2.2")
        {
            for (var col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
            {
                if (worksheet.Cells[1, col].Text != "№ п/п") continue;
                using var excelRange =
                    worksheet.Cells[2, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
                excelRange.Sort(col - 1);
                break;
            }
        }
        _Excel_Export_Notes(formNum, 2, masterHeaderLength, worksheetPrim, exportFormList);
        worksheet.View.FreezePanes(2, 1);
        worksheetPrim.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region Print_Excel_export //SelectedForm-Выгрузка Excel-Для печати

    public ReactiveCommand<object, Unit> Print_Excel_Export { get; private set; }

    private async Task _Print_Excel_Export(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || par is not ObservableCollectionWithItemPropertyChanged<IKey> forms) return;
        
        var exportForm = (Report)forms.First();
        var orgWithExportForm = Local_Reports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection.Contains(exportForm));
        var formNum = exportForm.FormNum_DB;
        if (formNum is "" || forms.Count == 0 || orgWithExportForm is null) return;

        var cts = new CancellationTokenSource();
        var exportType = "Для печати";
        var regNum = RemoveForbiddenChars(orgWithExportForm.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(orgWithExportForm.Master.OkpoRep.Value);
        var corNum = Convert.ToString(exportForm.CorrectionNumber_DB);
        string fileName;
        switch (formNum[0])
        {
            case '1':
                var startPeriod = RemoveForbiddenChars(exportForm.StartPeriod_DB);
                var endPeriod = RemoveForbiddenChars(exportForm.EndPeriod_DB);
                fileName = $"{exportType}_{regNum}_{okpo}_{formNum}_{startPeriod}_{endPeriod}_{corNum}_{Version}";
                break;
            case '2':
                var year = RemoveForbiddenChars(exportForm.Year_DB);
                fileName = $"{exportType}_{regNum}_{okpo}_{formNum}_{year}_{corNum}_{Version}";
                break;
            default:
                return;
        }
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

#if DEBUG
        var pth = Path.Combine(
            Path.Combine(
                Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")),
                    "data"), "Excel"), $"{formNum}.xlsx");
#else
                                string pth =
 Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),"data"), "Excel"), formNum+".xlsx");
#endif

        using ExcelPackage excelPackage = new(new FileInfo(fullPath), new FileInfo(pth));
        await exportForm.SortAsync();
        var worksheetTitle = excelPackage.Workbook.Worksheets[$"{formNum.Split('.')[0]}.0"];
        var worksheetMain = excelPackage.Workbook.Worksheets[formNum];
        worksheetTitle.Cells.Style.ShrinkToFit = true;
        worksheetMain.Cells.Style.ShrinkToFit = true;

        _Excel_Print_Titul_Export(formNum, worksheetTitle, exportForm);
        _Excel_Print_SubMain_Export(formNum, worksheetMain, exportForm);
        _Excel_Print_Notes_Export(formNum, worksheetMain, exportForm);
        _Excel_Print_Rows_Export(formNum, worksheetMain, exportForm);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region All_Excel_Export //Excel-Формы 1.x,2.x и Excel-Выбранная организация-Формы 1.x,2.x

    public ReactiveCommand<object, Unit> All_Excel_Export { get; private set; }

    private async Task _All_Excel_Export(object par)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var forSelectedOrg = par.ToString()!.Contains("Org");
        var param = Regex.Replace(par.ToString()!, "[^\\d.]", "");

        #region CheckReportsCount

        var findRep = 0;
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.StartsWith(param))
                {
                    findRep++;
                }
            }
        }
        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку форм {param}," +
                        $"{Environment.NewLine}поскольку эти формы отсутствуют в текущей базе.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        } 

        #endregion

        var mainWindow = desktop.MainWindow as MainWindow;
        var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
        if (forSelectedOrg && selectedReports is null)
        {
            #region MessageExcelExportFail

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentMessage = "Выгрузка не выполнена, поскольку не выбрана организация",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(mainWindow);

            #endregion

            return;
        }

        var cts = new CancellationTokenSource();
        var exportType = forSelectedOrg
            ? $"Выбранная организация_Формы {param}"
            : $"Формы {param}";
        var regNum = RemoveForbiddenChars(selectedReports!.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports!.Master.OkpoRep.Value);
        var fileName = forSelectedOrg
            ? $"{exportType}_{regNum}_{okpo}_{Version}"
            : $"{exportType}_{dbFileName}_{Version}";

        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (Local_Reports.Reports_Collection.Count == 0) return;
        worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {param}");
        var worksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {param}");
        int masterHeaderLength;
        if (param.Split('.')[0] == "1")
        {
            masterHeaderLength = Form10.ExcelHeader(worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form10.ExcelHeader(worksheetPrim, 1, 1, id: "ID") + 1;
        }
        else
        {
            masterHeaderLength = Form20.ExcelHeader(worksheet, 1, 1, id: "ID") + 1;
            masterHeaderLength = Form20.ExcelHeader(worksheetPrim, 1, 1, id: "ID") + 1;
        }

        var t = Report.ExcelHeader(worksheet, param, 1, masterHeaderLength);
        Report.ExcelHeader(worksheetPrim, param, 1, masterHeaderLength);
        masterHeaderLength += t;
        masterHeaderLength--;

        #region BindingsExcelHeaders

        switch (param)
        {
            case "1.1":
                Form11.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.2":
                Form12.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.3":
                Form13.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.4":
                Form14.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.5":
                Form15.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.6":
                Form16.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.7":
                Form17.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.8":
                Form18.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "1.9":
                Form19.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.1":
                Form21.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.2":
                Form22.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.3":
                Form23.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.4":
                Form24.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.5":
                Form25.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.6":
                Form26.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.7":
                Form27.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.8":
                Form28.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.9":
                Form29.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.10":
                Form210.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.11":
                Form211.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
            case "2.12":
                Form212.ExcelHeader(worksheet, 1, masterHeaderLength + 1);
                break;
        } 
        Note.ExcelHeader(worksheetPrim, 1, masterHeaderLength + 1);

        #endregion

        if (OperatingSystem.IsWindows())
        {
            worksheet.Cells.AutoFitColumns();
            worksheetPrim.Cells.AutoFitColumns();
        }

        var lst = new List<Report>();
        if (forSelectedOrg)
        {
            var newItem = selectedReports!.Report_Collection
                .Where(x => x.FormNum_DB.Equals(param))
                .OrderBy(x => param[0] is '1' ? StringReverse(x.StartPeriod_DB) : x.Year_DB);
            lst.AddRange(newItem);
        }
        else
        {
            foreach (var key in Local_Reports.Reports_Collection)
            {
                var item = (Reports)key;
                var newItem = item.Report_Collection
                    .Where(x => x.FormNum_DB.Equals(param))
                    .OrderBy(x => param[0] is '1' ? StringReverse(x.StartPeriod_DB) : x.Year_DB);
                lst.AddRange(newItem);
            }
        }

        //foreach (Reports item in Local_Reports.Reports_Collection)
        //{
        //    lst.AddRange(item.Report_Collection);
        //}

        _Excel_Export_Rows(param, 2, masterHeaderLength, worksheet, lst, true);
        _Excel_Export_Notes(param, 2, masterHeaderLength, worksheetPrim, lst, true);
        worksheet.View.FreezePanes(2, 1);

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region SelectOrgExcelExport //Excel-Выбранная организация-Все формы

    public ReactiveCommand<Unit, Unit> SelectOrgExcelExport { get; private set; }

    private async Task _SelectOrgExcelExport()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        var mainWindow = desktop.MainWindow as MainWindow;
        var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
        if (selectedReports is null || !selectedReports.Report_Collection.Any())
        {
            #region MessageExcelExportFail

            var msg = "Выгрузка не выполнена, поскольку ";
            msg += selectedReports is null
                ? "не выбрана организация."
                : "у выбранной организации отсутствуют формы отчетности.";
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = msg,
                    MinHeight = 125,
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(mainWindow);

            #endregion

            return;
        }

        var cts = new CancellationTokenSource();
        var exportType = $"Выбранная организация_Все формы";
        var regNum = RemoveForbiddenChars(selectedReports.Master.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports.Master.OkpoRep.Value);
        var fileName = $"{exportType}_{regNum}_{okpo}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        HashSet<string> formNums = new();
        foreach (var key in selectedReports.Report_Collection)
        {
            var rep = (Report)key;
            formNums.Add(rep.FormNum_DB);
        }

        #region BindingExportForms

        if (formNums.Contains("1.1"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.1");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.1");
            ExportForm11Data(selectedReports);
        }

        if (formNums.Contains("1.2"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.2");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.2");
            ExportForm12Data(selectedReports);
        }

        if (formNums.Contains("1.3"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.3");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.3");
            ExportForm13Data(selectedReports);
        }

        if (formNums.Contains("1.4"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.4");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.4");
            ExportForm14Data(selectedReports);
        }

        if (formNums.Contains("1.5"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.5");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.5");
            ExportForm15Data(selectedReports);
        }

        if (formNums.Contains("1.6"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.6");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.6");
            ExportForm16Data(selectedReports);
        }

        if (formNums.Contains("1.7"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.7");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.7");
            ExportForm17Data(selectedReports);
        }

        if (formNums.Contains("1.8"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.8");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.8");
            ExportForm18Data(selectedReports);
        }

        if (formNums.Contains("1.9"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 1.9");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 1.9");
            ExportForm19Data(selectedReports);
        }

        if (formNums.Contains("2.1"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.1");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.1");
            ExportForm21Data(selectedReports);
        }

        if (formNums.Contains("2.2"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.2");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.2");
            ExportForm22Data(selectedReports);
        }

        if (formNums.Contains("2.3"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.3");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.3");
            ExportForm23Data(selectedReports);
        }

        if (formNums.Contains("2.4"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.4");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.4");
            ExportForm24Data(selectedReports);
        }

        if (formNums.Contains("2.5"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.5");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.5");
            ExportForm25Data(selectedReports);
        }

        if (formNums.Contains("2.6"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.6");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.6");
            ExportForm26Data(selectedReports);
        }

        if (formNums.Contains("2.7"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.7");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.7");
            ExportForm27Data(selectedReports);
        }

        if (formNums.Contains("2.8"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.8");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.8");
            ExportForm28Data(selectedReports);
        }

        if (formNums.Contains("2.9"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.9");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.9");
            ExportForm29Data(selectedReports);
        }

        if (formNums.Contains("2.10"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.10");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.10");
            ExportForm210Data(selectedReports);
        }

        if (formNums.Contains("2.11"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.11");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.11");
            ExportForm211Data(selectedReports);
        }

        if (formNums.Contains("2.12"))
        {
            worksheet = excelPackage.Workbook.Worksheets.Add("Форма 2.12");
            worksheetComment = excelPackage.Workbook.Worksheets.Add("Примечания 2.12");
            ExportForm212Data(selectedReports);
        } 

        #endregion

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #region ExportForms

    #region ExportForm_11

    private void ExportForm11Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "количество, шт";
        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
        worksheet.Cells[1, 19].Value = "дата выпуска";
        worksheet.Cells[1, 20].Value = "категория";
        worksheet.Cells[1, 21].Value = "НСС, мес";
        worksheet.Cells[1, 22].Value = "код формы собственности";
        worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 24].Value = "вид";
        worksheet.Cells[1, 25].Value = "номер";
        worksheet.Cells[1, 26].Value = "дата";
        worksheet.Cells[1, 27].Value = "поставщика или получателя";
        worksheet.Cells[1, 28].Value = "перевозчика";
        worksheet.Cells[1, 29].Value = "наименование";
        worksheet.Cells[1, 30].Value = "тип";
        worksheet.Cells[1, 31].Value = "номер";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
            foreach (var rep in form.OrderBy(x => StringReverse(x.StartPeriod_DB)))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows11.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form11)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.CreatorOKPO_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.CreationDate_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.Category_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.SignedServicePeriod_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.PropertyCode_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.Owner_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 31].Value = repForm.PackNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_12

    private void ExportForm12Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта";
        worksheet.Cells[1, 13].Value = "наименование";
        worksheet.Cells[1, 14].Value = "номер";
        worksheet.Cells[1, 15].Value = "масса объединенного урана, кг";
        worksheet.Cells[1, 16].Value = "код ОКПО изготовителя";
        worksheet.Cells[1, 17].Value = "дата выпуска";
        worksheet.Cells[1, 18].Value = "НСС, мес";
        worksheet.Cells[1, 19].Value = "код формы собственности";
        worksheet.Cells[1, 20].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 21].Value = "вид";
        worksheet.Cells[1, 22].Value = "номер";
        worksheet.Cells[1, 23].Value = "дата";
        worksheet.Cells[1, 24].Value = "поставщика или получателя";
        worksheet.Cells[1, 25].Value = "перевозчика";
        worksheet.Cells[1, 26].Value = "наименование";
        worksheet.Cells[1, 27].Value = "тип";
        worksheet.Cells[1, 28].Value = "номер";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.2") && x.Rows12 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows12.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form12)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.NameIOU_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.FactoryNumber_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.Mass_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.CreatorOKPO_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.CreationDate_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.SignedServicePeriod_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.PropertyCode_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.Owner_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.PackNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_13

    private void ExportForm13Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "активность, Бк";
        worksheet.Cells[1, 17].Value = "код ОКПО изготовителя";
        worksheet.Cells[1, 18].Value = "дата выпуска";
        worksheet.Cells[1, 19].Value = "агрегатное состояние";
        worksheet.Cells[1, 20].Value = "код формы собственности";
        worksheet.Cells[1, 21].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 22].Value = "вид";
        worksheet.Cells[1, 23].Value = "номер";
        worksheet.Cells[1, 24].Value = "дата";
        worksheet.Cells[1, 25].Value = "поставщика или получателя";
        worksheet.Cells[1, 26].Value = "перевозчика";
        worksheet.Cells[1, 27].Value = "наименование";
        worksheet.Cells[1, 28].Value = "тип";
        worksheet.Cells[1, 29].Value = "номер";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.3") && x.Rows13 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows13.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form13)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.Activity_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.CreatorOKPO_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.CreationDate_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.AggregateState_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.PropertyCode_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.Owner_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.PackNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_14

    private void ExportForm14Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта";
        worksheet.Cells[1, 13].Value = "наименование";
        worksheet.Cells[1, 14].Value = "вид";
        worksheet.Cells[1, 15].Value = "радионуклиды";
        worksheet.Cells[1, 16].Value = "активность, Бк";
        worksheet.Cells[1, 17].Value = "дата измерения активности";
        worksheet.Cells[1, 18].Value = "объем, куб.м";
        worksheet.Cells[1, 19].Value = "масса, кг";
        worksheet.Cells[1, 20].Value = "агрегатное состояние";
        worksheet.Cells[1, 21].Value = "код формы собственности";
        worksheet.Cells[1, 22].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 23].Value = "вид";
        worksheet.Cells[1, 24].Value = "номер";
        worksheet.Cells[1, 25].Value = "дата";
        worksheet.Cells[1, 26].Value = "поставщика или получателя";
        worksheet.Cells[1, 27].Value = "перевозчика";
        worksheet.Cells[1, 28].Value = "наименование";
        worksheet.Cells[1, 29].Value = "тип";
        worksheet.Cells[1, 30].Value = "номер";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.4") && x.Rows14 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows14.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form14)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.Name_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Sort_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.Activity_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.ActivityMeasurementDate_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.Volume_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.Mass_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.AggregateState_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.PropertyCode_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.Owner_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.PackNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_15

    private void ExportForm15Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта (сертификата) Эри, акта определения характеристик ОЗИИ";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "количество, шт";
        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 18].Value = "дата выпуска";
        worksheet.Cells[1, 19].Value = "статус РАО";
        worksheet.Cells[1, 20].Value = "вид";
        worksheet.Cells[1, 21].Value = "номер";
        worksheet.Cells[1, 22].Value = "дата";
        worksheet.Cells[1, 23].Value = "поставщика или получателя";
        worksheet.Cells[1, 24].Value = "перевозчика";
        worksheet.Cells[1, 25].Value = "наименование";
        worksheet.Cells[1, 26].Value = "тип";
        worksheet.Cells[1, 27].Value = "заводской номер";
        worksheet.Cells[1, 28].Value = "наименование";
        worksheet.Cells[1, 29].Value = "код";
        worksheet.Cells[1, 30].Value = "Код переработки / сортировки РАО";
        worksheet.Cells[1, 31].Value = "Субсидия, %";
        worksheet.Cells[1, 32].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.5") && x.Rows15 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows15.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form15)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.CreationDate_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.StatusRAO_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.PackNumber_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.RefineOrSortRAOCode_DB;
                    worksheet.Cells[currentRow, 31].Value = repForm.Subsidy_DB;
                    worksheet.Cells[currentRow, 32].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_16

    private void ExportForm16Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "Код РАО";
        worksheet.Cells[1, 13].Value = "Статус РАО";
        worksheet.Cells[1, 14].Value = "объем без упаковки, куб.";
        worksheet.Cells[1, 15].Value = "масса без упаковки";
        worksheet.Cells[1, 16].Value = "количество ОЗИИИ";
        worksheet.Cells[1, 17].Value = "Основные радионуклиды";
        worksheet.Cells[1, 18].Value = "тритий";
        worksheet.Cells[1, 19].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 20].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 21].Value = "трансурановые радионуклиды";
        worksheet.Cells[1, 22].Value = "Дата измерения активности";
        worksheet.Cells[1, 23].Value = "вид";
        worksheet.Cells[1, 24].Value = "номер";
        worksheet.Cells[1, 25].Value = "дата";
        worksheet.Cells[1, 26].Value = "поставщика или получателя";
        worksheet.Cells[1, 27].Value = "перевозчика";
        worksheet.Cells[1, 28].Value = "наименование";
        worksheet.Cells[1, 29].Value = "код";
        worksheet.Cells[1, 30].Value = "Код переработки /";
        worksheet.Cells[1, 31].Value = "наименование";
        worksheet.Cells[1, 32].Value = "тип";
        worksheet.Cells[1, 33].Value = "номер упаковки";
        worksheet.Cells[1, 34].Value = "Субсидия, %";
        worksheet.Cells[1, 35].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.6") && x.Rows16 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows16.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form16)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.CodeRAO_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.StatusRAO_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Volume_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.Mass_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.QuantityOZIII_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.MainRadionuclids_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.TritiumActivity_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.BetaGammaActivity_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.AlphaActivity_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.TransuraniumActivity_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.ActivityMeasurementDate_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.RefineOrSortRAOCode_DB;
                    worksheet.Cells[currentRow, 31].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 32].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 33].Value = repForm.PackNumber_DB;
                    worksheet.Cells[currentRow, 34].Value = repForm.Subsidy_DB;
                    worksheet.Cells[currentRow, 35].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_17

    private void ExportForm17Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "наименование";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "заводской номер";
        worksheet.Cells[1, 15].Value = "номер упаковки (идентификационный код)";
        worksheet.Cells[1, 16].Value = "дата формирования";
        worksheet.Cells[1, 17].Value = "номер паспорта";
        worksheet.Cells[1, 18].Value = "объем, куб.м";
        worksheet.Cells[1, 19].Value = "масса брутто, т";
        worksheet.Cells[1, 20].Value = "наименования радионуклида";
        worksheet.Cells[1, 21].Value = "удельная активность, Бк/г";
        worksheet.Cells[1, 22].Value = "вид";
        worksheet.Cells[1, 23].Value = "номер";
        worksheet.Cells[1, 24].Value = "дата";
        worksheet.Cells[1, 25].Value = "поставщика или получателя";
        worksheet.Cells[1, 26].Value = "перевозчика";
        worksheet.Cells[1, 27].Value = "наименование";
        worksheet.Cells[1, 28].Value = "код";
        worksheet.Cells[1, 29].Value = "код";
        worksheet.Cells[1, 30].Value = "статус";
        worksheet.Cells[1, 31].Value = "объем без упаковки, куб.м";
        worksheet.Cells[1, 32].Value = "масса без упаковки (нетто), т";
        worksheet.Cells[1, 33].Value = "количество ОЗИИИ, шт";
        worksheet.Cells[1, 34].Value = "тритий";
        worksheet.Cells[1, 35].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 36].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 37].Value = "трансурановые радионуклиды";
        worksheet.Cells[1, 38].Value = "Код переработки/сортировки РАО";
        worksheet.Cells[1, 39].Value = "Субсидия, %";
        worksheet.Cells[1, 40].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.7") && x.Rows17 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows17.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form17)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.PackFactoryNumber_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.PackFactoryNumber_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.FormingDate_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.Volume_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.Mass_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.SpecificActivity_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.CodeRAO_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.StatusRAO_DB;
                    worksheet.Cells[currentRow, 31].Value = repForm.VolumeOutOfPack_DB;
                    worksheet.Cells[currentRow, 32].Value = repForm.MassOutOfPack_DB;
                    worksheet.Cells[currentRow, 33].Value = repForm.Quantity_DB;
                    worksheet.Cells[currentRow, 34].Value = repForm.TritiumActivity_DB;
                    worksheet.Cells[currentRow, 35].Value = repForm.BetaGammaActivity_DB;
                    worksheet.Cells[currentRow, 36].Value = repForm.AlphaActivity_DB;
                    worksheet.Cells[currentRow, 37].Value = repForm.TransuraniumActivity_DB;
                    worksheet.Cells[currentRow, 38].Value = repForm.RefineOrSortRAOCode_DB;
                    worksheet.Cells[currentRow, 39].Value = repForm.Subsidy_DB;
                    worksheet.Cells[currentRow, 40].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_18

    private void ExportForm18Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "индивидуальный номер (идентификационный код) партии ЖРО";
        worksheet.Cells[1, 13].Value = "номер паспорта";
        worksheet.Cells[1, 14].Value = "объем, куб.м";
        worksheet.Cells[1, 15].Value = "масса, т";
        worksheet.Cells[1, 16].Value = "солесодержание, г/л";
        worksheet.Cells[1, 17].Value = "наименование радионуклида";
        worksheet.Cells[1, 18].Value = "удельная активность, Бк/г";
        worksheet.Cells[1, 19].Value = "вид";
        worksheet.Cells[1, 20].Value = "номер";
        worksheet.Cells[1, 21].Value = "дата";
        worksheet.Cells[1, 22].Value = "поставщика или получателя";
        worksheet.Cells[1, 23].Value = "перевозчика";
        worksheet.Cells[1, 24].Value = "наименование";
        worksheet.Cells[1, 25].Value = "код";
        worksheet.Cells[1, 26].Value = "код";
        worksheet.Cells[1, 27].Value = "статус";
        worksheet.Cells[1, 28].Value = "объем, куб.м";
        worksheet.Cells[1, 29].Value = "масса, т";
        worksheet.Cells[1, 30].Value = "тритий";
        worksheet.Cells[1, 31].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 32].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 33].Value = "трансурановые радионуклиды";
        worksheet.Cells[1, 34].Value = "Код переработки/сортировки РАО";
        worksheet.Cells[1, 35].Value = "Субсидия, %";
        worksheet.Cells[1, 36].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.8") && x.Rows18 != null);
            foreach (var rep in form.OrderBy(x => x.StartPeriod_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows18.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form18)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.IndividualNumberZHRO_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.PassportNumber_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.Volume6_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.Mass7_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.SaltConcentration_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.SpecificActivity_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.ProviderOrRecieverOKPO_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.TransporterOKPO_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.CodeRAO_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.StatusRAO_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.Volume20_DB;
                    worksheet.Cells[currentRow, 29].Value = repForm.Mass21_DB;
                    worksheet.Cells[currentRow, 30].Value = repForm.TritiumActivity_DB;
                    worksheet.Cells[currentRow, 31].Value = repForm.BetaGammaActivity_DB;
                    worksheet.Cells[currentRow, 32].Value = repForm.AlphaActivity_DB;
                    worksheet.Cells[currentRow, 33].Value = repForm.TransuraniumActivity_DB;
                    worksheet.Cells[currentRow, 34].Value = repForm.RefineOrSortRAOCode_DB;
                    worksheet.Cells[currentRow, 35].Value = repForm.Subsidy_DB;
                    worksheet.Cells[currentRow, 36].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_19

    private void ExportForm19Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "вид";
        worksheet.Cells[1, 13].Value = "номер";
        worksheet.Cells[1, 14].Value = "дата";
        worksheet.Cells[1, 15].Value = "Код типа объектов учета";
        worksheet.Cells[1, 16].Value = "радионуклиды";
        worksheet.Cells[1, 17].Value = "активность, Бк";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.9") && x.Rows19 != null);
            foreach (var rep in form.OrderBy(x => StringReverse(x.StartPeriod_DB)))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows19.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form19)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                    worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.DocumentVid_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.CodeTypeAccObject_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                    worksheetComment.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 9].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_21

    private void ExportForm21Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "наименование";
        worksheet.Cells[1, 8].Value = "код";
        worksheet.Cells[1, 9].Value = "мощность куб.м/год";
        worksheet.Cells[1, 10].Value = "количество часов работы за год";
        worksheet.Cells[1, 11].Value = "код РАО";
        worksheet.Cells[1, 12].Value = "статус РАО";
        worksheet.Cells[1, 13].Value = "куб.м";
        worksheet.Cells[1, 14].Value = "т";
        worksheet.Cells[1, 15].Value = "ОЗИИИ, шт";
        worksheet.Cells[1, 16].Value = "тритий";
        worksheet.Cells[1, 17].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 18].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 19].Value = "трансурановые радионуклиды";
        worksheet.Cells[1, 20].Value = "код РАО";
        worksheet.Cells[1, 21].Value = "статус РАО";
        worksheet.Cells[1, 22].Value = "куб.м";
        worksheet.Cells[1, 23].Value = "т";
        worksheet.Cells[1, 24].Value = "ОЗИИИ, шт";
        worksheet.Cells[1, 25].Value = "тритий";
        worksheet.Cells[1, 26].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 27].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 28].Value = "трансурановые радионуклиды";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.1") && x.Rows21 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows21.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form21)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.RefineMachineName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.MachineCode_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.MachinePower_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.NumberOfHoursPerYear_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.CodeRAOIn_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.StatusRAOIn_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.VolumeIn_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.MassIn_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.QuantityIn_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.TritiumActivityIn_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.BetaGammaActivityIn_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.AlphaActivityIn_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.TransuraniumActivityIn_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.CodeRAOout_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.StatusRAOout_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.VolumeOut_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.MassOut_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.QuantityOZIIIout_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.TritiumActivityOut_DB;
                    worksheet.Cells[currentRow, 26].Value = repForm.BetaGammaActivityOut_DB;
                    worksheet.Cells[currentRow, 27].Value = repForm.AlphaActivityOut_DB;
                    worksheet.Cells[currentRow, 28].Value = repForm.TransuraniumActivityOut_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_22

    private void ExportForm22Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "наименование";
        worksheet.Cells[1, 8].Value = "код";
        worksheet.Cells[1, 9].Value = "наименование";
        worksheet.Cells[1, 10].Value = "тип";
        worksheet.Cells[1, 11].Value = "количество, шт";
        worksheet.Cells[1, 12].Value = "код РАО";
        worksheet.Cells[1, 13].Value = "статус РАО";
        worksheet.Cells[1, 14].Value = "РАО без упаковки";
        worksheet.Cells[1, 15].Value = "РАО с упаковкой";
        worksheet.Cells[1, 16].Value = "РАО без упаковки (нетто)";
        worksheet.Cells[1, 17].Value = "РАО с упаковкой (брутто)";
        worksheet.Cells[1, 18].Value = "Количество ОЗИИИ, шт";
        worksheet.Cells[1, 19].Value = "тритий";
        worksheet.Cells[1, 20].Value = "бета-, гамма-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 21].Value = "альфа-излучающие радионуклиды (исключая";
        worksheet.Cells[1, 22].Value = "трансурановые радионуклиды";
        worksheet.Cells[1, 23].Value = "Основные радионуклиды";
        worksheet.Cells[1, 24].Value = "Субсидия, %";
        worksheet.Cells[1, 25].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.2") && x.Rows22 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows22.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form22)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.PackName_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.PackType_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.PackQuantity_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.CodeRAO_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.StatusRAO_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.VolumeOutOfPack_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.VolumeInPack_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.MassOutOfPack_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.MassInPack_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.QuantityOZIII_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.TritiumActivity_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.BetaGammaActivity_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.AlphaActivity_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.TransuraniumActivity_DB;
                    worksheet.Cells[currentRow, 23].Value = repForm.MainRadionuclids_DB;
                    worksheet.Cells[currentRow, 24].Value = repForm.Subsidy_DB;
                    worksheet.Cells[currentRow, 25].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_23

    private void ExportForm23Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "наименование";
        worksheet.Cells[1, 8].Value = "код";
        worksheet.Cells[1, 9].Value = "проектный объем, куб.м";
        worksheet.Cells[1, 10].Value = "код РАО";
        worksheet.Cells[1, 11].Value = "объем, куб.м";
        worksheet.Cells[1, 12].Value = "масса, т";
        worksheet.Cells[1, 13].Value = "количество ОЗИИИ, шт";
        worksheet.Cells[1, 14].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "дата";
        worksheet.Cells[1, 17].Value = "срок действия";
        worksheet.Cells[1, 18].Value = "наименование документа";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.3") && x.Rows23 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows23.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form23)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.ProjectVolume_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.CodeRAO_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.Volume_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.Mass_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.QuantityOZIII_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.SummaryActivity_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.DocumentNumber_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.DocumentDate_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.ExpirationDate_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.DocumentName_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_24

    private void ExportForm24Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Код ОЯТ";
        worksheet.Cells[1, 8].Value = "Номер мероприятия ФЦП";
        worksheet.Cells[1, 9].Value = "масса образованного, т";
        worksheet.Cells[1, 10].Value = "количество образованного, шт";
        worksheet.Cells[1, 11].Value = "масса поступивших от сторонних, т";
        worksheet.Cells[1, 12].Value = "количество поступивших от сторонних, шт";
        worksheet.Cells[1, 13].Value = "масса импортированных от сторонних, т";
        worksheet.Cells[1, 14].Value = "количество импортированных от сторонних, шт";
        worksheet.Cells[1, 15].Value = "масса учтенных по другим причинам, т";
        worksheet.Cells[1, 16].Value = "количество учтенных по другим причинам, шт";
        worksheet.Cells[1, 17].Value = "масса переданных сторонним, т";
        worksheet.Cells[1, 18].Value = "количество переданных сторонним, шт";
        worksheet.Cells[1, 19].Value = "масса переработанных, т";
        worksheet.Cells[1, 20].Value = "количество переработанных, шт";
        worksheet.Cells[1, 21].Value = "масса снятия с учета, т";
        worksheet.Cells[1, 22].Value = "количество снятых с учета, шт";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.4") && x.Rows24 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows24.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form24)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.CodeOYAT_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.FcpNumber_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.MassCreated_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.QuantityCreated_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.MassFromAnothers_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.QuantityFromAnothers_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.MassFromAnothersImported_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.QuantityFromAnothersImported_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.MassAnotherReasons_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.QuantityAnotherReasons_DB;
                    worksheet.Cells[currentRow, 17].Value = repForm.MassTransferredToAnother_DB;
                    worksheet.Cells[currentRow, 18].Value = repForm.QuantityTransferredToAnother_DB;
                    worksheet.Cells[currentRow, 19].Value = repForm.MassRefined_DB;
                    worksheet.Cells[currentRow, 20].Value = repForm.QuantityRefined_DB;
                    worksheet.Cells[currentRow, 21].Value = repForm.MassRemovedFromAccount_DB;
                    worksheet.Cells[currentRow, 22].Value = repForm.QuantityRemovedFromAccount_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_25

    private void ExportForm25Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "наименование, номер";
        worksheet.Cells[1, 8].Value = "код";
        worksheet.Cells[1, 9].Value = "код ОЯТ";
        worksheet.Cells[1, 10].Value = "номер мероприятия ФЦП";
        worksheet.Cells[1, 11].Value = "топливо (нетто)";
        worksheet.Cells[1, 12].Value = "ОТВС(ТВЭЛ, выемной части реактора) брутто";
        worksheet.Cells[1, 13].Value = "количество, шт";
        worksheet.Cells[1, 14].Value = "альфа-излучающих нуклидов";
        worksheet.Cells[1, 15].Value = "бета-, гамма-излучающих нуклидов";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.5") && x.Rows25 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows25.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form25)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.StoragePlaceName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.StoragePlaceCode_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.CodeOYAT_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.FcpNumber_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.FuelMass_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.CellMass_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.Quantity_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.AlphaActivity_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.BetaGammaActivity_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_26

    private void ExportForm26Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Номер наблюдательной скважины";
        worksheet.Cells[1, 8].Value = "Наименование зоны контроля";
        worksheet.Cells[1, 9].Value = "Предполагаемый источник поступления радиоактивных веществ";
        worksheet.Cells[1, 10].Value =
            "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м";
        worksheet.Cells[1, 11].Value = "Глубина отбора проб, м";
        worksheet.Cells[1, 12].Value = "Наименование радионуклида";
        worksheet.Cells[1, 13].Value = "Среднегодовое содержание радионуклида, Бк/кг";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.6") && x.Rows26 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows26.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form26)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.ObservedSourceNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.ControlledAreaName_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.SupposedWasteSource_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.DistanceToWasteSource_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.TestDepth_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.RadionuclidName_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.AverageYearConcentration_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_27

    private void ExportForm27Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Наименование, номер источника выбросов";
        worksheet.Cells[1, 8].Value = "Наименование радионуклида";
        worksheet.Cells[1, 9].Value = "разрешенный выброс за отчетный год";
        worksheet.Cells[1, 10].Value = "фактический выброс за отчетный год";
        worksheet.Cells[1, 11].Value = "фактический выброс за предыдущий год";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.7") && x.Rows27 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows27.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form27)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.ObservedSourceNumber_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.RadionuclidName_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.AllowedWasteValue_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.FactedWasteValue_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.WasteOutbreakPreviousYear_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_28

    private void ExportForm28Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Наименование, номер выпуска сточных вод";
        worksheet.Cells[1, 8].Value = "наименование";
        worksheet.Cells[1, 9].Value = "код типа документа";
        worksheet.Cells[1, 10].Value = "Наименование бассейнового округа";
        worksheet.Cells[1, 11].Value = "Допустимый объем водоотведения за год, тыс.куб.м";
        worksheet.Cells[1, 12].Value = "Отведено за отчетный период, тыс.куб.м";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.8") && x.Rows28 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows28.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form28)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.WasteSourceName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.WasteRecieverName_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.RecieverTypeCode_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.PoolDistrictName_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.AllowedWasteRemovalVolume_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.RemovedWasteVolume_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_29

    private void ExportForm29Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Наименование, номер выпуска сточных вод";
        worksheet.Cells[1, 8].Value = "Наименование радионуклида";
        worksheet.Cells[1, 9].Value = "допустимая";
        worksheet.Cells[1, 10].Value = "фактическая";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.9") && x.Rows29 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows29.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form29)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.WasteSourceName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.RadionuclidName_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.AllowedActivity_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.FactedActivity_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_210

    private void ExportForm210Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Наименование показателя";
        worksheet.Cells[1, 8].Value = "Наименование участка";
        worksheet.Cells[1, 9].Value = "Кадастровый номер участка";
        worksheet.Cells[1, 10].Value = "Код участка";
        worksheet.Cells[1, 11].Value = "Площадь загрязненной территории, кв.м";
        worksheet.Cells[1, 12].Value = "средняя";
        worksheet.Cells[1, 13].Value = "максимальная";
        worksheet.Cells[1, 14].Value = "альфа-излучающие радионуклиды";
        worksheet.Cells[1, 15].Value = "бета-излучающие радионуклиды";
        worksheet.Cells[1, 16].Value = "Номер мероприятия ФЦП";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.10") && x.Rows210 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows210.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form210)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.IndicatorName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.PlotName_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.PlotKadastrNumber_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.PlotCode_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.InfectedArea_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.AvgGammaRaysDosePower_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.MaxGammaRaysDosePower_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.WasteDensityAlpha_DB;
                    worksheet.Cells[currentRow, 15].Value = repForm.WasteDensityBeta_DB;
                    worksheet.Cells[currentRow, 16].Value = repForm.FcpNumber_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_211

    private void ExportForm211Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Наименование участка";
        worksheet.Cells[1, 8].Value = "Кадастровый номер участка";
        worksheet.Cells[1, 9].Value = "Код участка";
        worksheet.Cells[1, 10].Value = "Площадь загрязненной территории, кв.м";
        worksheet.Cells[1, 11].Value = "Наименование радионуклидов";
        worksheet.Cells[1, 12].Value = "земельный участок";
        worksheet.Cells[1, 13].Value = "жидкая фаза";
        worksheet.Cells[1, 14].Value = "донные отложения";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.11") && x.Rows211 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows211.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form211)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.PlotName_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.PlotKadastrNumber_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.PlotCode_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.InfectedArea_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 12].Value = repForm.SpecificActivityOfPlot_DB;
                    worksheet.Cells[currentRow, 13].Value = repForm.SpecificActivityOfLiquidPart_DB;
                    worksheet.Cells[currentRow, 14].Value = repForm.SpecificActivityOfDensePart_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region ExportForm_212

    private void ExportForm212Data(Reports selectedReports)
    {
        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Номер корректировки";
        worksheet.Cells[1, 5].Value = "отчетный год";
        worksheet.Cells[1, 6].Value = "№ п/п";
        worksheet.Cells[1, 7].Value = "Код операции";
        worksheet.Cells[1, 8].Value = "Код типа объектов учета";
        worksheet.Cells[1, 9].Value = "радионуклиды";
        worksheet.Cells[1, 10].Value = "активность, Бк";
        worksheet.Cells[1, 11].Value = "ОКПО поставщика/получателя";
        NotesHeaders();

        var tmp = 2;
        List<Reports> repList = new() { selectedReports };
        foreach (var reps in repList)
        {
            var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.12") && x.Rows212 != null);
            foreach (var rep in form.OrderBy(x => x.Year_DB))
            {
                var currentRow = tmp;
                foreach (var key in rep.Rows212.OrderBy(x => x.NumberInOrder_DB))
                {
                    var repForm = (Form212)key;
                    worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                    worksheet.Cells[currentRow, 2].Value = reps.Master.Rows20[0].ShortJurLico_DB;
                    worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                    worksheet.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheet.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheet.Cells[currentRow, 6].Value = repForm.NumberInOrder_DB;
                    worksheet.Cells[currentRow, 7].Value = repForm.OperationCode_DB;
                    worksheet.Cells[currentRow, 8].Value = repForm.ObjectTypeCode_DB;
                    worksheet.Cells[currentRow, 9].Value = repForm.Radionuclids_DB;
                    worksheet.Cells[currentRow, 10].Value = repForm.Activity_DB;
                    worksheet.Cells[currentRow, 11].Value = repForm.ProviderOrRecieverOKPO_DB;
                    currentRow++;
                }

                tmp = currentRow;
                currentRow = 2;
                foreach (var key in rep.Notes.OrderBy(x => x.Order))
                {
                    var comment = (Note)key;
                    worksheetComment.Cells[currentRow, 1].Value = reps.Master.OkpoRep.Value;
                    worksheetComment.Cells[currentRow, 2].Value = reps.Master.ShortJurLicoRep.Value;
                    worksheetComment.Cells[currentRow, 3].Value = reps.Master.RegNoRep.Value;
                    worksheetComment.Cells[currentRow, 4].Value = rep.CorrectionNumber_DB;
                    worksheetComment.Cells[currentRow, 5].Value = rep.Year_DB;
                    worksheetComment.Cells[currentRow, 6].Value = comment.RowNumber_DB;
                    worksheetComment.Cells[currentRow, 7].Value = comment.GraphNumber_DB;
                    worksheetComment.Cells[currentRow, 8].Value = comment.Comment_DB;
                    currentRow++;
                }
            }
        }

        worksheet.View.FreezePanes(2, 1);
    }

    #endregion

    #region NotesHeader

    private void NotesHeaders()
    {
        worksheetComment.Cells[1, 1].Value = "ОКПО";
        worksheetComment.Cells[1, 2].Value = "Сокращенное наименование";
        worksheetComment.Cells[1, 3].Value = "Рег. №";
        worksheetComment.Cells[1, 4].Value = "Номер корректировки";
        worksheetComment.Cells[1, 5].Value = "Дата начала периода";
        worksheetComment.Cells[1, 6].Value = "Дата конца периода";
        worksheetComment.Cells[1, 7].Value = "№ строки";
        worksheetComment.Cells[1, 8].Value = "№ графы";
        worksheetComment.Cells[1, 9].Value = "Пояснение";
        worksheetComment.View.FreezePanes(2, 1);
    }

    #endregion

    #endregion

    #endregion
    
    #region AllForms1_Excel_Export //Excel-Список форм 1

    public ReactiveCommand<Unit, Unit> AllForms1_Excel_Export { get; private set; }

    private async Task _AllForms1_Excel_Export()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        #region ReportsCountCheck

        var findRep = 0;
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "1")
                {
                    findRep += 1;
                }
            }
        }
        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 1 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам 1",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        } 

        #endregion

        var cts = new CancellationTokenSource();
        var exportType = "Список форм 1";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (Local_Reports.Reports_Collection.Count == 0) return;
        worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 1");

        #region Headers

        worksheet.Cells[1, 1].Value = "Рег.№";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Форма";
        worksheet.Cells[1, 4].Value = "Дата начала";
        worksheet.Cells[1, 5].Value = "Дата конца";
        worksheet.Cells[1, 6].Value = "Номер кор.";
        worksheet.Cells[1, 7].Value = "Количество строк";
        worksheet.Cells[1, 8].Value = "Инвентаризация"; 

        #endregion

        var lst = new List<Reports>();
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "1")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        foreach (var reps in lst.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            foreach (var rep in reps.Report_Collection
                         .OrderBy(x => x.FormNum_DB)
                         .ThenBy(x => StringReverse(x.StartPeriod_DB)))
            {
                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                worksheet.Cells[row, 4].Value = rep.StartPeriod_DB;
                worksheet.Cells[row, 5].Value = rep.EndPeriod_DB;
                worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 7].Value = rep.Rows.Count;
                worksheet.Cells[row, 8].Value = InventoryCheck(rep).TrimStart();
                row++;
            }
        }

        if (OperatingSystem.IsWindows()) worksheet.Cells.AutoFitColumns();   // Под Astra Linux эта команда крашит программу без GDI дров
        worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region AllForms2_Excel_Export //Excel-Список форм 2

    public ReactiveCommand<Unit, Unit> AllForms2_Excel_Export { get; private set; }

    private async Task _AllForms2_Excel_Export()
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        #region ReportsCountCheck

        var findRep = 0;
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var reps = (Reports)key;
            foreach (var key1 in reps.Report_Collection)
            {
                var rep = (Report)key1;
                if (rep.FormNum_DB.Split('.')[0] == "2")
                {
                    findRep += 1;
                }
            }
        }
        if (findRep == 0)
        {
            #region MessageRepsNotFound

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Не удалось совершить выгрузку списка всех отчетов по форме 2 с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствуют отчеты по форме 2",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        } 

        #endregion

        var cts = new CancellationTokenSource();
        var exportType = "Список форм 2";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        if (Local_Reports.Reports_Collection.Count == 0) return;

        worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм 2");

        #region Headers

        worksheet.Cells[1, 1].Value = "Рег.№";
        worksheet.Cells[1, 2].Value = "ОКПО";
        worksheet.Cells[1, 3].Value = "Форма";
        worksheet.Cells[1, 4].Value = "Отчетный год";
        worksheet.Cells[1, 5].Value = "Номер кор.";
        worksheet.Cells[1, 6].Value = "Количество строк"; 

        #endregion

        var lst = new List<Reports>();
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var item = (Reports)key;
            if (item.Master_DB.FormNum_DB.Split('.')[0] == "2")
            {
                lst.Add(item);
            }
        }

        var row = 2;
        foreach (var reps in lst.OrderBy(x => x.Master_DB.RegNoRep.Value))
        {
            foreach (var rep in reps.Report_Collection
                         .OrderBy(x => x.FormNum_DB)
                         .ThenBy(x => x.Year_DB))
            {
                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                worksheet.Cells[row, 4].Value = rep.Year_DB;
                worksheet.Cells[row, 5].Value = rep.CorrectionNumber_DB;
                worksheet.Cells[row, 6].Value = rep.Rows.Count;
                row++;
            }
        }

        if (OperatingSystem.IsWindows()) worksheet.Cells.AutoFitColumns();   // Под Astra Linux эта команда крашит программу без GDI дров
        worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region AllOrganization_Excel_Export //Excel-Список организаций

    public ReactiveCommand<Unit, Unit> AllOrganization_Excel_Export { get; private set; }

    private async Task _AllOrganization_Excel_Export()
    {
        var findReps = Local_Reports.Reports_Collection.Count;
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || findReps == 0) return;

        var cts = new CancellationTokenSource();
        var exportType = "Список организаций";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;

        if (Local_Reports.Reports_Collection.Count == 0) return;
        worksheet = excelPackage.Workbook.Worksheets.Add("Список всех организаций");

        #region ColumnHeaders

        worksheet.Cells[1, 1].Value = "Рег.№";
        worksheet.Cells[1, 2].Value = "Регион";
        worksheet.Cells[1, 3].Value = "Орган управления";
        worksheet.Cells[1, 4].Value = "ОКПО";
        worksheet.Cells[1, 5].Value = "Сокращенное наименование";
        worksheet.Cells[1, 6].Value = "Адрес";
        worksheet.Cells[1, 7].Value = "ИНН";
        worksheet.Cells[1, 8].Value = "Форма 1.1";
        worksheet.Cells[1, 9].Value = "Форма 1.2";
        worksheet.Cells[1, 10].Value = "Форма 1.3";
        worksheet.Cells[1, 11].Value = "Форма 1.4";
        worksheet.Cells[1, 12].Value = "Форма 1.5";
        worksheet.Cells[1, 13].Value = "Форма 1.6";
        worksheet.Cells[1, 14].Value = "Форма 1.7";
        worksheet.Cells[1, 15].Value = "Форма 1.8";
        worksheet.Cells[1, 16].Value = "Форма 1.9";
        worksheet.Cells[1, 17].Value = "Форма 2.1";
        worksheet.Cells[1, 18].Value = "Форма 2.2";
        worksheet.Cells[1, 19].Value = "Форма 2.3";
        worksheet.Cells[1, 20].Value = "Форма 2.4";
        worksheet.Cells[1, 21].Value = "Форма 2.5";
        worksheet.Cells[1, 22].Value = "Форма 2.6";
        worksheet.Cells[1, 23].Value = "Форма 2.7";
        worksheet.Cells[1, 24].Value = "Форма 2.8";
        worksheet.Cells[1, 25].Value = "Форма 2.9";
        worksheet.Cells[1, 26].Value = "Форма 2.10";
        worksheet.Cells[1, 27].Value = "Форма 2.11";
        worksheet.Cells[1, 28].Value = "Форма 2.12";

        #endregion

        if (OperatingSystem.IsWindows())    // Под Astra Linux эта команда крашит программу без GDI дров
        {
            worksheet.Column(3).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
        }

        var lst = new List<Reports>();
        var checkedLst = new List<Reports>();
        foreach (var key in Local_Reports.Reports_Collection)
        {
            var item = (Reports)key;
            lst.Add(item);
        }

        var row = 2;
        foreach (var reps in lst)
        {
            if (checkedLst.FirstOrDefault(x => x.Master_DB.RegNoRep == reps.Master_DB.RegNoRep) != null)
            {
                row--;

                #region BindingCells

                worksheet.Cells[row, 8].Value =
                    (int)worksheet.Cells[row, 8].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.1"));
                worksheet.Cells[row, 9].Value =
                    (int)worksheet.Cells[row, 9].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.2"));
                worksheet.Cells[row, 10].Value =
                    (int)worksheet.Cells[row, 10].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.3"));
                worksheet.Cells[row, 11].Value =
                    (int)worksheet.Cells[row, 11].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.4"));
                worksheet.Cells[row, 12].Value =
                    (int)worksheet.Cells[row, 12].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.5"));
                worksheet.Cells[row, 13].Value =
                    (int)worksheet.Cells[row, 13].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.6"));
                worksheet.Cells[row, 14].Value =
                    (int)worksheet.Cells[row, 14].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.7"));
                worksheet.Cells[row, 15].Value =
                    (int)worksheet.Cells[row, 15].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.8"));
                worksheet.Cells[row, 16].Value =
                    (int)worksheet.Cells[row, 16].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("1.9"));
                worksheet.Cells[row, 17].Value =
                    (int)worksheet.Cells[row, 17].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.1"));
                worksheet.Cells[row, 18].Value =
                    (int)worksheet.Cells[row, 18].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.2"));
                worksheet.Cells[row, 19].Value =
                    (int)worksheet.Cells[row, 19].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.3"));
                worksheet.Cells[row, 20].Value =
                    (int)worksheet.Cells[row, 20].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.4"));
                worksheet.Cells[row, 21].Value =
                    (int)worksheet.Cells[row, 21].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.5"));
                worksheet.Cells[row, 22].Value =
                    (int)worksheet.Cells[row, 22].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.6"));
                worksheet.Cells[row, 23].Value =
                    (int)worksheet.Cells[row, 23].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.7"));
                worksheet.Cells[row, 24].Value =
                    (int)worksheet.Cells[row, 24].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.8"));
                worksheet.Cells[row, 25].Value =
                    (int)worksheet.Cells[row, 25].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.9"));
                worksheet.Cells[row, 26].Value =
                    (int)worksheet.Cells[row, 26].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.10"));
                worksheet.Cells[row, 27].Value =
                    (int)worksheet.Cells[row, 27].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.11"));
                worksheet.Cells[row, 28].Value =
                    (int)worksheet.Cells[row, 28].Value
                    + reps.Report_Collection.Count(x => x.FormNum_DB.Equals("2.12"));

                #endregion

                row++;
            }
            else
            {
                #region BindingCells

                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 2].Value = reps.Master.RegNoRep.Value.Length >= 2
                    ? reps.Master.RegNoRep.Value[..2]
                    : reps.Master.RegNoRep.Value;
                worksheet.Cells[row, 3].Value = !string.IsNullOrEmpty(reps.Master.Rows10[0].OrganUprav_DB)
                    ? reps.Master.Rows10[0].OrganUprav_DB
                    : !string.IsNullOrEmpty(reps.Master.Rows10[1].OrganUprav_DB)
                        ? reps.Master.Rows10[1].OrganUprav_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].OrganUprav_DB)
                            ? reps.Master.Rows20[0].OrganUprav_DB
                            : !string.IsNullOrEmpty(reps.Master.Rows20[1].OrganUprav_DB)
                                ? reps.Master.Rows20[1].OrganUprav_DB
                                : "";
                worksheet.Cells[row, 4].Value = reps.Master.OkpoRep.Value;
                worksheet.Cells[row, 5].Value = reps.Master.ShortJurLicoRep.Value;
                worksheet.Cells[row, 6].Value = 
                    !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoFactAddress_DB) &&
                    !reps.Master.Rows10[1].JurLicoFactAddress_DB.Equals("-")
                        ? reps.Master.Rows10[1].JurLicoFactAddress_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoFactAddress_DB) &&
                          !reps.Master.Rows20[1].JurLicoFactAddress_DB.Equals("-")
                            ? reps.Master.Rows20[1].JurLicoFactAddress_DB
                            : !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoAddress_DB) &&
                              !reps.Master.Rows10[1].JurLicoAddress_DB.Equals("-")
                                ? reps.Master.Rows10[1].JurLicoAddress_DB
                                : !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoAddress_DB) &&
                                  !reps.Master.Rows20[1].JurLicoAddress_DB.Equals("-")
                                    ? reps.Master.Rows20[1].JurLicoAddress_DB
                                    : !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoFactAddress_DB) &&
                                      !reps.Master.Rows10[0].JurLicoFactAddress_DB.Equals("-")
                                        ? reps.Master.Rows10[0].JurLicoFactAddress_DB
                                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].JurLicoFactAddress_DB) &&
                                          !reps.Master.Rows20[0].JurLicoFactAddress_DB.Equals("-")
                                            ? reps.Master.Rows20[0].JurLicoFactAddress_DB
                                            : !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoAddress_DB) &&
                                              !reps.Master.Rows10[0].JurLicoAddress_DB.Equals("-")
                                                ? reps.Master.Rows10[0].JurLicoAddress_DB
                                                : reps.Master.Rows20[0].JurLicoAddress_DB;
                worksheet.Cells[row, 7].Value = !string.IsNullOrEmpty(reps.Master.Rows10[0].Inn_DB)
                    ? reps.Master.Rows10[0].Inn_DB
                    : !string.IsNullOrEmpty(reps.Master.Rows10[1].Inn_DB)
                        ? reps.Master.Rows10[1].Inn_DB
                        : !string.IsNullOrEmpty(reps.Master.Rows20[0].Inn_DB)
                            ? reps.Master.Rows20[0].Inn_DB
                            : reps.Master.Rows20[1].Inn_DB;
                worksheet.Cells[row, 8].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.1"));
                worksheet.Cells[row, 9].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.2"));
                worksheet.Cells[row, 10].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.3"));
                worksheet.Cells[row, 11].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.4"));
                worksheet.Cells[row, 12].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.5"));
                worksheet.Cells[row, 13].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.6"));
                worksheet.Cells[row, 14].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.7"));
                worksheet.Cells[row, 15].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.8"));
                worksheet.Cells[row, 16].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("1.9"));
                worksheet.Cells[row, 17].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.1"));
                worksheet.Cells[row, 18].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.2"));
                worksheet.Cells[row, 19].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.3"));
                worksheet.Cells[row, 20].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.4"));
                worksheet.Cells[row, 21].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.5"));
                worksheet.Cells[row, 22].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.6"));
                worksheet.Cells[row, 23].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.7"));
                worksheet.Cells[row, 24].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.8"));
                worksheet.Cells[row, 25].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.9"));
                worksheet.Cells[row, 26].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.10"));
                worksheet.Cells[row, 27].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.11"));
                worksheet.Cells[row, 28].Value = reps.Report_Collection
                    .Count(x => x.FormNum_DB.Equals("2.12"));

                #endregion

                row++;
                checkedLst.Add(reps);
            }
        }

        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            if (worksheet.Cells[1, col].Value is "Сокращенное наименование" or "Адрес" or "Орган управления") continue;
            if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
            {
                worksheet.Column(col).AutoFit();
            }
        }
        worksheet.View.FreezePanes(2, 1);
        
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region Passports //Excel-Паспорта   

    #region ExcelMissingPas //Excel-Паспорта-Отчеты без паспортов

    public ReactiveCommand<object, Unit> ExcelMissingPas { get; protected set; }

    private async Task _ExcelMissingPas(object param)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        var cts = new CancellationTokenSource();
        var exportType = "Отчеты без паспортов";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        worksheet = excelPackage.Workbook.Worksheets.Add("Список отчётов без файла паспорта");

        #region ColumnHeaders

        worksheet.Cells[1, 1].Value = "Рег. №";
        worksheet.Cells[1, 2].Value = "Сокращенное наименование";
        worksheet.Cells[1, 3].Value = "ОКПО";
        worksheet.Cells[1, 4].Value = "Форма";
        worksheet.Cells[1, 5].Value = "Дата начала периода";
        worksheet.Cells[1, 6].Value = "Дата конца периода";
        worksheet.Cells[1, 7].Value = "Номер корректировки";
        worksheet.Cells[1, 8].Value = "Количество строк";
        worksheet.Cells[1, 9].Value = "№ п/п";
        worksheet.Cells[1, 10].Value = "код";
        worksheet.Cells[1, 11].Value = "дата";
        worksheet.Cells[1, 12].Value = "номер паспорта (сертификата)";
        worksheet.Cells[1, 13].Value = "тип";
        worksheet.Cells[1, 14].Value = "радионуклиды";
        worksheet.Cells[1, 15].Value = "номер";
        worksheet.Cells[1, 16].Value = "количество, шт";
        worksheet.Cells[1, 17].Value = "суммарная активность, Бк";
        worksheet.Cells[1, 18].Value = "код ОКПО изготовителя";
        worksheet.Cells[1, 19].Value = "дата выпуска";
        worksheet.Cells[1, 20].Value = "категория";
        worksheet.Cells[1, 21].Value = "НСС, мес";
        worksheet.Cells[1, 22].Value = "код формы собственности";
        worksheet.Cells[1, 23].Value = "код ОКПО правообладателя";
        worksheet.Cells[1, 24].Value = "вид";
        worksheet.Cells[1, 25].Value = "номер";
        worksheet.Cells[1, 26].Value = "дата";
        worksheet.Cells[1, 27].Value = "поставщика или получателя";
        worksheet.Cells[1, 28].Value = "перевозчика";
        worksheet.Cells[1, 29].Value = "наименование";
        worksheet.Cells[1, 30].Value = "тип";
        worksheet.Cells[1, 31].Value = "номер";

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            worksheet.Cells.AutoFitColumns();
        }
        worksheet.View.FreezePanes(2, 1);

        #endregion

        List<string> pasNames = new();
        List<string[]> pasUniqParam = new();
        DirectoryInfo directory = new(PasFolderPath);
        FileInfo[] files;
        try
        {
            files = directory.GetFiles("*#*#*#*#*.pdf");
        }
        catch (Exception)
        {
            #region MessageFailedToOpenPassportDirectory

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage =
                        "Не удалось открыть сетевое хранилище паспортов:" +
                        $"{Environment.NewLine}{directory.FullName}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        }

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));
        var currentRow = 2;
        foreach (var key in Local_Reports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null)
                .OrderBy(x => StringReverse(x.StartPeriod_DB))
                .ThenBy(x => x.NumberInOrder_DB);
            foreach (var rep in form11)
            {
                List<Form11> repPas = rep.Rows11
                    .Where(x => x.OperationCode_DB is "11" or "85" && x.Category_DB is 1 or 2 or 3)
                    .ToList();
                foreach (var repForm in repPas)
                {
                    var findPasFile = false;
                    foreach (var pasParam in pasUniqParam)
                    {
                        if (ComparePasParam(ConvertPrimToDash(repForm.CreatorOKPO_DB), pasParam[0])
                            && ComparePasParam(ConvertPrimToDash(repForm.Type_DB), pasParam[1])
                            && ComparePasParam(ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                            && ComparePasParam(ConvertPrimToDash(repForm.PassportNumber_DB), pasParam[3])
                            && ComparePasParam(ConvertPrimToDash(repForm.FactoryNumber_DB), pasParam[4]))
                        {
                            findPasFile = true;
                            break;
                        }
                    }

                    if (!findPasFile)
                    {
                        #region BindingCells

                        worksheet.Cells[currentRow, 1].Value = reps.Master.RegNoRep.Value;
                        worksheet.Cells[currentRow, 2].Value = reps.Master.Rows10[0].ShortJurLico_DB;
                        worksheet.Cells[currentRow, 3].Value = reps.Master.OkpoRep.Value;
                        worksheet.Cells[currentRow, 4].Value = rep.FormNum_DB;
                        worksheet.Cells[currentRow, 5].Value = rep.StartPeriod_DB;
                        worksheet.Cells[currentRow, 6].Value = rep.EndPeriod_DB;
                        worksheet.Cells[currentRow, 7].Value = rep.CorrectionNumber_DB;
                        worksheet.Cells[currentRow, 8].Value = rep.Rows.Count;
                        worksheet.Cells[currentRow, 9].Value = repForm.NumberInOrder_DB;
                        worksheet.Cells[currentRow, 10].Value = repForm.OperationCode_DB;
                        worksheet.Cells[currentRow, 11].Value = repForm.OperationDate_DB;
                        worksheet.Cells[currentRow, 12].Value = repForm.PassportNumber_DB;
                        worksheet.Cells[currentRow, 13].Value = repForm.Type_DB;
                        worksheet.Cells[currentRow, 14].Value = repForm.Radionuclids_DB;
                        worksheet.Cells[currentRow, 15].Value = repForm.FactoryNumber_DB;
                        worksheet.Cells[currentRow, 16].Value = repForm.Quantity_DB;
                        worksheet.Cells[currentRow, 17].Value = repForm.Activity_DB is null or "" or "-"
                            ? "-"
                            : double.TryParse(repForm.Activity_DB.Replace("е", "E")
                                .Replace("Е", "E").Replace("e", "E")
                                .Replace("(", "").Replace(")", "")
                                .Replace(".", ","), out var doubleValue)
                                ? doubleValue
                                : repForm.Activity_DB;
                        worksheet.Cells[currentRow, 18].Value = repForm.CreatorOKPO_DB;
                        worksheet.Cells[currentRow, 19].Value = repForm.CreationDate_DB;
                        worksheet.Cells[currentRow, 20].Value = repForm.Category_DB;
                        worksheet.Cells[currentRow, 21].Value = repForm.SignedServicePeriod_DB;
                        worksheet.Cells[currentRow, 22].Value = repForm.PropertyCode_DB;
                        worksheet.Cells[currentRow, 23].Value = repForm.Owner_DB;
                        worksheet.Cells[currentRow, 24].Value = repForm.DocumentVid_DB;
                        worksheet.Cells[currentRow, 25].Value = repForm.DocumentNumber_DB;
                        worksheet.Cells[currentRow, 26].Value = repForm.DocumentDate_DB;
                        worksheet.Cells[currentRow, 27].Value = repForm.ProviderOrRecieverOKPO_DB;
                        worksheet.Cells[currentRow, 28].Value = repForm.TransporterOKPO_DB;
                        worksheet.Cells[currentRow, 29].Value = repForm.PackName_DB;
                        worksheet.Cells[currentRow, 30].Value = repForm.PackType_DB;
                        worksheet.Cells[currentRow, 31].Value = repForm.PackNumber_DB;

                        #endregion

                        currentRow++;
                    }
                }
            }
        }

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region ExcelPasWithoutRep //Excel-Паспорта-Паспорта без отчетов

    public ReactiveCommand<object, Unit> ExcelPasWithoutRep { get; protected set; }

    private async Task _ExcelPasWithoutRep(object param)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

        #region MessageInputCategoryNums

        var res = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отмена", IsCancel = true }
                },
                ContentTitle = "Выбор категории",
                ContentMessage = "Введите через запятую номера категорий (допускается несколько значений)",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow);

        #endregion

        List<short?> categories = new() { 1, 2, 3, 4, 5 };
        if (res.Button is null or "Отмена") return;
        try
        {
            categories = Regex.Replace(res.Message, "[^\\d,]", "")
                .Split(',').Select(short.Parse).Cast<short?>().ToList();
        }
        catch (Exception)
        {
            #region MessageInvalidCategoryNums

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        "Номера категорий не были введены, либо были введены некорректно" +
                        $"{Environment.NewLine}Выгрузка будет осуществлена по всем категориям (1-5)",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion
        }

        var cts = new CancellationTokenSource();
        var exportType = "Паспорта без отчетов";
        var fileName = $"{exportType}_{dbFileName}_{Version}";
        (string fullPath, bool openTemp) result;
        try
        {
            result = await ExcelGetFullPath(fileName, cts);
        }
        catch
        {
            return;
        }
        finally
        {
            cts.Dispose();
        }
        var fullPath = result.fullPath;
        var openTemp = result.openTemp;
        if (string.IsNullOrEmpty(fullPath)) return;

        using ExcelPackage excelPackage = new(new FileInfo(fullPath));
        excelPackage.Workbook.Properties.Author = "RAO_APP";
        excelPackage.Workbook.Properties.Title = "Report";
        excelPackage.Workbook.Properties.Created = DateTime.Now;
        worksheet = excelPackage.Workbook.Worksheets.Add($"Список паспортов без отчетов");

        worksheet.Cells[1, 1].Value = "Полное имя файла";
        worksheet.Cells[1, 2].Value = "Код ОКПО изготовителя";
        worksheet.Cells[1, 3].Value = "Тип";
        worksheet.Cells[1, 4].Value = "Год выпуска";
        worksheet.Cells[1, 5].Value = "Номер паспорта";
        worksheet.Cells[1, 6].Value = "Номер";

        List<string> pasNames = new();
        List<string[]> pasUniqParam = new();
        DirectoryInfo directory = new(PasFolderPath);
        FileInfo[] files;
        try
        {
            files = directory.GetFiles("*#*#*#*#*.pdf");
        }
        catch (Exception)
        {
            #region MessageFailedToOpenPassportDirectory

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage =
                        "Не удалось открыть сетевое хранилище паспортов:" +
                        $"{Environment.NewLine}{directory.FullName}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        }

        pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
        pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));
        foreach (var key in Local_Reports.Reports_Collection10)
        {
            var reps = (Reports)key;
            var form11 = reps.Report_Collection
                .Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
            foreach (var rep in form11)
            {
                List<Form11> repPas = rep.Rows11
                    .Where(x => x.OperationCode_DB is ("11" or "85") && categories.Contains(x.Category_DB))
                    .ToList();
                foreach (var repForm in repPas)
                {
                    foreach (var pasParam in pasUniqParam.Where(pasParam =>
                                 ComparePasParam(ConvertPrimToDash(repForm.CreatorOKPO_DB), pasParam[0])
                                 && ComparePasParam(ConvertPrimToDash(repForm.Type_DB), pasParam[1])
                                 && ComparePasParam(ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                                 && ComparePasParam(ConvertPrimToDash(repForm.PassportNumber_DB), pasParam[3])
                                 && ComparePasParam(ConvertPrimToDash(repForm.FactoryNumber_DB), pasParam[4])))
                    {
                        pasNames.Remove($"{pasParam[0]}#{pasParam[1]}#{pasParam[2]}#{pasParam[3]}#{pasParam[4]}");
                        break;
                    }
                }
            }
        }

        var currentRow = 2;
        foreach (var pasName in pasNames)
        {
            worksheet.Cells[currentRow, 1].Value = pasName;
            worksheet.Cells[currentRow, 2].Value = pasName.Split('#')[0];
            worksheet.Cells[currentRow, 3].Value = pasName.Split('#')[1];
            worksheet.Cells[currentRow, 4].Value = pasName.Split('#')[2];
            worksheet.Cells[currentRow, 5].Value = pasName.Split('#')[3];
            worksheet.Cells[currentRow, 6].Value = pasName.Split('#')[4];
            currentRow++;
        }

        if (OperatingSystem.IsWindows()) // Под Astra Linux эта команда крашит программу без GDI дров
        {
            worksheet.Cells.AutoFitColumns();
        }

        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp);
    }

    #endregion

    #region ChangePasDir //Excel-Паспорта-Изменить расположение

    public ReactiveCommand<object, Unit> ChangePasDir { get; protected set; }

    private async Task _ChangePasDir(object param)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            OpenFolderDialog openFolderDialog = new() { Directory = PasFolderPath };
            PasFolderPath = await openFolderDialog.ShowAsync(desktop.MainWindow) ?? PasFolderPath;
        }
    }

    #endregion

    #endregion

    #region ExcelExportCommonImplementation

    #region ExcelExportNotes

    private int _Excel_Export_Notes(string param, int startRow, int startColumn, ExcelWorksheet worksheetPrim,
        List<Report> forms, bool printId = false)
    {
        foreach (var item in forms)
        {
            var findReports = Local_Reports.Reports_Collection
                .Where(t => t.Report_Collection.Contains(item));
            var reps = findReports.FirstOrDefault();
            if (reps == null) continue;
            var curRow = startRow;
            foreach (var i in item.Notes)
            {
                var mstRep = reps.Master_DB;
                i.ExcelRow(worksheetPrim, curRow, startColumn + 1);
                var yu = printId
                    ? param.Split('.')[0] == "1"
                        ? mstRep.Rows10[1].RegNo_DB != "" && mstRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows10[0]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                        : mstRep.Rows20[1].RegNo_DB != "" && mstRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows20[0]
                                .ExcelRow(worksheetPrim, curRow, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                    : param.Split('.')[0] == "1"
                        ? mstRep.Rows10[1].RegNo_DB != "" && mstRep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1].ExcelRow(worksheetPrim, curRow, 1) + 1
                            : reps.Master_DB.Rows10[0].ExcelRow(worksheetPrim, curRow, 1) + 1
                        : mstRep.Rows20[1].RegNo_DB != "" && mstRep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1].ExcelRow(worksheetPrim, curRow, 1) + 1
                            : reps.Master_DB.Rows20[0].ExcelRow(worksheetPrim, curRow, 1) + 1;

                item.ExcelRow(worksheetPrim, curRow, yu);
                curRow++;
            }

            startRow = curRow;
        }

        return startRow;
    }

    #endregion

    #region ExcelExportRows

    private int _Excel_Export_Rows(string param, int startRow, int startColumn, ExcelWorksheet worksheet,
        List<Report> forms, bool id = false)
    {
        foreach (var item in forms)
        {
            var findReports = Local_Reports.Reports_Collection
                .Where(t => t.Report_Collection.Contains(item));
            var reps = findReports.FirstOrDefault();
            if (reps is null) continue;
            IEnumerable<IKey> t = null;
            switch (param)
            {
                case "2.1":
                {
                    t = item[param].ToList<IKey>().Where(x => ((Form21)x).Sum_DB || ((Form21)x).SumGroup_DB);
                    if (item[param].ToList<IKey>().Any() && !t.Any())
                    {
                        t = item[param].ToList<IKey>();
                    }

                    break;
                }
                case "2.2":
                {
                    t = item[param].ToList<IKey>().Where(x => ((Form22)x).Sum_DB || ((Form22)x).SumGroup_DB);
                    if (item[param].ToList<IKey>().Any() && !t.Any())
                    {
                        t = item[param].ToList<IKey>();
                    }

                    break;
                }
            }

            if (param != "2.1" && param != "2.2")
            {
                t = item[param].ToList<IKey>();
            }

            var lst = t.Any()
                ? item[param].ToList<IKey>().ToList()
                : item[param].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
            if (lst.Count <= 0) continue;
            var count = startRow;
            startRow--;
            foreach (var it in lst.Where(it => it != null).OrderBy(x => x.Order))
            {
                switch (it)
                {
                    case Form11 form11:
                        form11.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form12 form12:
                        form12.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form13 form13:
                        form13.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form14 form14:
                        form14.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form15 form15:
                        form15.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form16 form16:
                        form16.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form17 form17:
                        form17.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form18 form18:
                        form18.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form19 form19:
                        form19.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form21 form21:
                        form21.ExcelRow(worksheet, count, startColumn + 1, sumNumber: form21.NumberInOrderSum_DB);
                        break;
                    case Form22 form22:
                        form22.ExcelRow(worksheet, count, startColumn + 1, sumNumber: form22.NumberInOrderSum_DB);
                        break;
                    case Form23 form23:
                        form23.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form24 form24:
                        form24.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form25 form25:
                        form25.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form26 form26:
                        form26.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form27 form27:
                        form27.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form28 form28:
                        form28.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form29 form29:
                        form29.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form210 form210:
                        form210.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form211 form211:
                        form211.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                    case Form212 form212:
                        form212.ExcelRow(worksheet, count, startColumn + 1);
                        break;
                }

                var mstrep = reps.Master_DB;

                var yu = id
                    ? param.Split('.')[0] == "1"
                        ? mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows10[0]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows10[0].Id.ToString()) + 1
                        : mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1
                            : reps.Master_DB.Rows20[0]
                                .ExcelRow(worksheet, count, 1, sumNumber: reps.Master_DB.Rows20[0].Id.ToString()) + 1
                    : param.Split('.')[0] == "1"
                        ? mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1) + 1
                            : reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1) + 1
                        : mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != ""
                            ? reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1) + 1
                            : reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1) + 1;

                item.ExcelRow(worksheet, count, yu);
                count++;
            }

            //if (param.Split('.')[0] == "2")
            //{
            //    var new_number = 2;
            //    while (worksheet.Cells[new_number, 6].Value != null)
            //    {
            //        worksheet.Cells[new_number, 6].Value = new_number - 1;
            //        new_number++;
            //    }
            //}
            startRow = count;
        }

        return startRow;
    }

    #endregion

    #region ExcelPrintTitulExport

    private void _Excel_Print_Titul_Export(string param, ExcelWorksheet worksheet, Report form)
    {
        var findReports = Local_Reports.Reports_Collection
            .Where(t => t.Report_Collection.Contains(form));
        var reps = findReports.FirstOrDefault();
        var master = reps.Master_DB;
        if (param.Split('.')[0] == "2")
        {
            var frmYur = master.Rows20[0];
            var frmObosob = master.Rows20[1];
            worksheet.Cells["G10"].Value = form.Year_DB;

            worksheet.Cells["F6"].Value = frmYur.RegNo_DB;
            worksheet.Cells["F15"].Value = frmYur.OrganUprav_DB;
            worksheet.Cells["F16"].Value = frmYur.SubjectRF_DB;
            worksheet.Cells["F17"].Value = frmYur.JurLico_DB;
            worksheet.Cells["F18"].Value = frmYur.ShortJurLico_DB;
            worksheet.Cells["F19"].Value = frmYur.JurLicoAddress_DB;
            worksheet.Cells["F20"].Value = frmYur.JurLicoFactAddress_DB;
            worksheet.Cells["F21"].Value = frmYur.GradeFIO_DB;
            worksheet.Cells["F22"].Value = frmYur.Telephone_DB;
            worksheet.Cells["F23"].Value = frmYur.Fax_DB;
            worksheet.Cells["F24"].Value = frmYur.Email_DB;

            worksheet.Cells["F25"].Value = frmObosob.SubjectRF_DB;
            worksheet.Cells["F26"].Value = frmObosob.JurLico_DB;
            worksheet.Cells["F27"].Value = frmObosob.ShortJurLico_DB;
            worksheet.Cells["F28"].Value = frmObosob.JurLicoAddress_DB;
            worksheet.Cells["F29"].Value = frmObosob.GradeFIO_DB;
            worksheet.Cells["F30"].Value = frmObosob.Telephone_DB;
            worksheet.Cells["F31"].Value = frmObosob.Fax_DB;
            worksheet.Cells["F32"].Value = frmObosob.Email_DB;

            worksheet.Cells["B36"].Value = frmYur.Okpo_DB;
            worksheet.Cells["C36"].Value = frmYur.Okved_DB;
            worksheet.Cells["D36"].Value = frmYur.Okogu_DB;
            worksheet.Cells["E36"].Value = frmYur.Oktmo_DB;
            worksheet.Cells["F36"].Value = frmYur.Inn_DB;
            worksheet.Cells["G36"].Value = frmYur.Kpp_DB;
            worksheet.Cells["H36"].Value = frmYur.Okopf_DB;
            worksheet.Cells["I36"].Value = frmYur.Okfs_DB;

            worksheet.Cells["B37"].Value = frmObosob.Okpo_DB;
            worksheet.Cells["C37"].Value = frmObosob.Okved_DB;
            worksheet.Cells["D37"].Value = frmObosob.Okogu_DB;
            worksheet.Cells["E37"].Value = frmObosob.Oktmo_DB;
            worksheet.Cells["F37"].Value = frmObosob.Inn_DB;
            worksheet.Cells["G37"].Value = frmObosob.Kpp_DB;
            worksheet.Cells["H37"].Value = frmObosob.Okopf_DB;
            worksheet.Cells["I37"].Value = frmObosob.Okfs_DB;
        }
        else
        {
            var frmYur = master.Rows10[0];
            var frmObosob = master.Rows10[1];

            worksheet.Cells["F6"].Value = frmYur.RegNo_DB;
            worksheet.Cells["F15"].Value = frmYur.OrganUprav_DB;
            worksheet.Cells["F16"].Value = frmYur.SubjectRF_DB;
            worksheet.Cells["F17"].Value = frmYur.JurLico_DB;
            worksheet.Cells["F18"].Value = frmYur.ShortJurLico_DB;
            worksheet.Cells["F19"].Value = frmYur.JurLicoAddress_DB;
            worksheet.Cells["F20"].Value = frmYur.JurLicoFactAddress_DB;
            worksheet.Cells["F21"].Value = frmYur.GradeFIO_DB;
            worksheet.Cells["F22"].Value = frmYur.Telephone_DB;
            worksheet.Cells["F23"].Value = frmYur.Fax_DB;
            worksheet.Cells["F24"].Value = frmYur.Email_DB;

            worksheet.Cells["F25"].Value = frmObosob.SubjectRF_DB;
            worksheet.Cells["F26"].Value = frmObosob.JurLico_DB;
            worksheet.Cells["F27"].Value = frmObosob.ShortJurLico_DB;
            worksheet.Cells["F28"].Value = frmObosob.JurLicoAddress_DB;
            worksheet.Cells["F29"].Value = frmObosob.GradeFIO_DB;
            worksheet.Cells["F30"].Value = frmObosob.Telephone_DB;
            worksheet.Cells["F31"].Value = frmObosob.Fax_DB;
            worksheet.Cells["F32"].Value = frmObosob.Email_DB;

            worksheet.Cells["B36"].Value = frmYur.Okpo_DB;
            worksheet.Cells["C36"].Value = frmYur.Okved_DB;
            worksheet.Cells["D36"].Value = frmYur.Okogu_DB;
            worksheet.Cells["E36"].Value = frmYur.Oktmo_DB;
            worksheet.Cells["F36"].Value = frmYur.Inn_DB;
            worksheet.Cells["G36"].Value = frmYur.Kpp_DB;
            worksheet.Cells["H36"].Value = frmYur.Okopf_DB;
            worksheet.Cells["I36"].Value = frmYur.Okfs_DB;

            worksheet.Cells["B37"].Value = frmObosob.Okpo_DB;
            worksheet.Cells["C37"].Value = frmObosob.Okved_DB;
            worksheet.Cells["D37"].Value = frmObosob.Okogu_DB;
            worksheet.Cells["E37"].Value = frmObosob.Oktmo_DB;
            worksheet.Cells["F37"].Value = frmObosob.Inn_DB;
            worksheet.Cells["G37"].Value = frmObosob.Kpp_DB;
            worksheet.Cells["H37"].Value = frmObosob.Okopf_DB;
            worksheet.Cells["I37"].Value = frmObosob.Okfs_DB;
        }
    }

    #endregion

    #region ExcelPrintSubMainExport

    private void _Excel_Print_SubMain_Export(string param, ExcelWorksheet worksheet, Report form)
    {
        var findReports = Local_Reports.Reports_Collection
            .Where(t => t.Report_Collection.Contains(form));
        var reps = findReports.FirstOrDefault();
        var master = reps.Master_DB;

        if (param.Split('.')[0] == "1")
        {
            worksheet.Cells["G3"].Value = form.StartPeriod_DB;
            worksheet.Cells["G4"].Value = form.EndPeriod_DB;
            worksheet.Cells["G5"].Value = form.CorrectionNumber_DB;
        }
        else
        {
            switch (param)
            {
                case "2.6":
                {
                    worksheet.Cells["G4"].Value = form.CorrectionNumber_DB;
                    worksheet.Cells["G5"].Value = form.SourcesQuantity26_DB;
                    break;
                }
                case "2.7":
                {
                    worksheet.Cells["G3"].Value = form.CorrectionNumber_DB;
                    worksheet.Cells["G4"].Value = form.PermissionNumber27_DB;
                    worksheet.Cells["G5"].Value = form.ValidBegin27_DB;
                    worksheet.Cells["J5"].Value = form.ValidThru27_DB;
                    worksheet.Cells["G6"].Value = form.PermissionDocumentName27_DB;
                    break;
                }
                case "2.8":
                {
                    worksheet.Cells["G3"].Value = form.CorrectionNumber_DB;
                    worksheet.Cells["G4"].Value = form.PermissionNumber_28_DB;
                    worksheet.Cells["K4"].Value = form.ValidBegin_28_DB;
                    worksheet.Cells["N4"].Value = form.ValidThru_28_DB;
                    worksheet.Cells["G5"].Value = form.PermissionDocumentName_28_DB;

                    worksheet.Cells["G6"].Value = form.PermissionNumber1_28_DB;
                    worksheet.Cells["K6"].Value = form.ValidBegin1_28_DB;
                    worksheet.Cells["N6"].Value = form.ValidThru1_28_DB;
                    worksheet.Cells["G7"].Value = form.PermissionDocumentName1_28_DB;

                    worksheet.Cells["G8"].Value = form.ContractNumber_28_DB;
                    worksheet.Cells["K8"].Value = form.ValidBegin2_28_DB;
                    worksheet.Cells["N8"].Value = form.ValidThru2_28_DB;
                    worksheet.Cells["G9"].Value = form.OrganisationReciever_28_DB;

                    worksheet.Cells["D21"].Value = form.GradeExecutor_DB;
                    worksheet.Cells["F21"].Value = form.FIOexecutor_DB;
                    worksheet.Cells["I21"].Value = form.ExecPhone_DB;
                    worksheet.Cells["K21"].Value = form.ExecEmail_DB;
                    return;
                }
                default:
                {
                    worksheet.Cells["G4"].Value = form.CorrectionNumber_DB;
                    break;
                }
            }
        }

        worksheet.Cells["D18"].Value = form.GradeExecutor_DB;
        worksheet.Cells["F18"].Value = form.FIOexecutor_DB;
        worksheet.Cells["I18"].Value = form.ExecPhone_DB;
        worksheet.Cells["K18"].Value = form.ExecEmail_DB;
    }

    #endregion

    #region ExcelPrintNotesExport

    private void _Excel_Print_Notes_Export(string param, ExcelWorksheet worksheet, Report form)
    {
        var start = param is "2.8"
            ? 18
            : 15;

        for (var i = 0; i < form.Notes.Count - 1; i++)
        {
            worksheet.InsertRow(start + 1, 1, start);
            var cells = worksheet.Cells[$"A{start + 1}:B{start + 1}"];
            foreach (var cell in cells)
            {
                var btm = cell.Style.Border.Bottom;
                var lft = cell.Style.Border.Left;
                var rgt = cell.Style.Border.Right;
                var top = cell.Style.Border.Top;
                btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                btm.Color.SetColor(255, 0, 0, 0);
                lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                lft.Color.SetColor(255, 0, 0, 0);
                rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                rgt.Color.SetColor(255, 0, 0, 0);
                top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                top.Color.SetColor(255, 0, 0, 0);
            }

            var cellCL = worksheet.Cells[$"C{start + 1}:L{start + 1}"];
            cellCL.Merge = true;
            var btmCL = cellCL.Style.Border.Bottom;
            var lftCL = cellCL.Style.Border.Left;
            var rgtCL = cellCL.Style.Border.Right;
            var topCL = cellCL.Style.Border.Top;
            btmCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            btmCL.Color.SetColor(255, 0, 0, 0);
            lftCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            lftCL.Color.SetColor(255, 0, 0, 0);
            rgtCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            rgtCL.Color.SetColor(255, 0, 0, 0);
            topCL.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
            topCL.Color.SetColor(255, 0, 0, 0);
        }

        var count = start;
        foreach (var note in form.Notes)
        {
            note.ExcelRow(worksheet, count, 1);
            count++;
        }
    }

    #endregion

    #region ExcelPrintRowsExport

    private void _Excel_Print_Rows_Export(string param, ExcelWorksheet worksheet, Report form)
    {
        var start = param is "2.8"
            ? 14
            : 11;

        for (var i = 0; i < form[param].Count - 1; i++)
        {
            worksheet.InsertRow(start + 1, 1, start);
            var cells = worksheet.Cells[$"A{start + 1}:B{start + 1}"];
            foreach (var cell in cells)
            {
                var btm = cell.Style.Border.Bottom;
                var lft = cell.Style.Border.Left;
                var rgt = cell.Style.Border.Right;
                var top = cell.Style.Border.Top;
                btm.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                btm.Color.SetColor(255, 0, 0, 0);
                lft.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                lft.Color.SetColor(255, 0, 0, 0);
                rgt.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                rgt.Color.SetColor(255, 0, 0, 0);
                top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                top.Color.SetColor(255, 0, 0, 0);
            }
        }

        var count = start;
        foreach (var it in form[param])
        {
            switch (it)
            {
                case Form11 form11:
                    form11.ExcelRow(worksheet, count, 1);
                    break;
                case Form12 form12:
                    form12.ExcelRow(worksheet, count, 1);
                    break;
                case Form13 form13:
                    form13.ExcelRow(worksheet, count, 1);
                    break;
                case Form14 form14:
                    form14.ExcelRow(worksheet, count, 1);
                    break;
                case Form15 form15:
                    form15.ExcelRow(worksheet, count, 1);
                    break;
                case Form16 form16:
                    form16.ExcelRow(worksheet, count, 1);
                    break;
                case Form17 form17:
                    form17.ExcelRow(worksheet, count, 1);
                    break;
                case Form18 form18:
                    form18.ExcelRow(worksheet, count, 1);
                    break;
                case Form19 form19:
                    form19.ExcelRow(worksheet, count, 1);
                    break;
                case Form21 form21:
                    form21.ExcelRow(worksheet, count, 1);
                    break;
                case Form22 form22:
                    form22.ExcelRow(worksheet, count, 1);
                    break;
                case Form23 form23:
                    form23.ExcelRow(worksheet, count, 1);
                    break;
                case Form24 form24:
                    form24.ExcelRow(worksheet, count, 1);
                    break;
                case Form25 form25:
                    form25.ExcelRow(worksheet, count, 1);
                    break;
                case Form26 form26:
                    form26.ExcelRow(worksheet, count, 1);
                    break;
                case Form27 form27:
                    form27.ExcelRow(worksheet, count, 1);
                    break;
                case Form28 form28:
                    form28.ExcelRow(worksheet, count, 1);
                    break;
                case Form29 form29:
                    form29.ExcelRow(worksheet, count, 1);
                    break;
                case Form210 form210:
                    form210.ExcelRow(worksheet, count, 1);
                    break;
                case Form211 form211:
                    form211.ExcelRow(worksheet, count, 1);
                    break;
                case Form212 form212:
                    form212.ExcelRow(worksheet, count, 1);
                    break;
            }

            count++;
        }
        //var new_number = 1;
        //var row = 10;
        //while (worksheet.Cells[row, 1].Value != null)
        //{
        //    worksheet.Cells[row, 1].Value = new_number;
        //    new_number++;
        //    row++;
        //}
    }

    #endregion

    #region ExcelGetFullPath

    private async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) 
        {
            cts.Cancel();
            cts.Token.ThrowIfCancellationRequested();
            return ("", false);
        }

        #region MessageSaveOrOpenTemp

        var res = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                },
                ContentTitle = "Выгрузка в Excel",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать" +
                                 $"{Environment.NewLine} с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow);

        #endregion

        var fullPath = "";
        var openTemp = res is "Открыть временную копию";

        switch (res)
        {
            case "Открыть временную копию":
            {
                DirectoryInfo tmpFolder = new(Path.Combine(Path.Combine(Path.GetPathRoot(GetSystemDirectory().Result)!, "RAO"), "temp"));
                var count = 0;
                do
                {
                    fullPath = Path.Combine(tmpFolder.FullName, fileName + $"_{++count}.xlsx");
                } while (File.Exists(fullPath));

                break;
            }
            case "Сохранить":
            {
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel",
                    Extensions = { "xlsx" }
                };
                dial.Filters.Add(filter);
                dial.InitialFileName = fileName;
                fullPath = await dial.ShowAsync(desktop.MainWindow);
                if (string.IsNullOrEmpty(fullPath))
                {
                    cts.Cancel();
                    cts.Token.ThrowIfCancellationRequested();
                }
                if (fullPath!.EndsWith(".xlsx"))
                {
                    fullPath += ".xlsx";
                }

                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception)
                    {
                        #region MessageFailedToSaveFile

                        await MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                ContentTitle = "Выгрузка в Excel",
                                ContentHeader = "Ошибка",
                                ContentMessage =
                                    $"Не удалось сохранить файл по пути: {fullPath}" +
                                    $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                    $"{Environment.NewLine}и используется другим процессом.",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(desktop.MainWindow);

                        #endregion

                        cts.Cancel();
                        cts.Token.ThrowIfCancellationRequested();
                    }
                }

                break;
            }
            default:
            {
                    cts.Cancel();
                    cts.Token.ThrowIfCancellationRequested();
                    break;
            }
        }

        return (fullPath, openTemp);
    }

    #endregion

    #region ExcelSaveAndOpen

    private static async Task ExcelSaveAndOpen(ExcelPackage excelPackage, string fullPath, bool openTemp)
    {
        if (Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        try
        {
            excelPackage.Save();
        }
        catch (Exception)
        {
            #region MessageFailedToSaveFile

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Не удалось сохранить файл по указанному пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            return;
        }

        if (openTemp)
        {
            Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
        }
        else
        {
            #region MessageExcelExportComplete

            var answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Ок" },
                        new ButtonDefinition { Name = "Открыть выгрузку" }
                    },
                    ContentTitle = "Выгрузка в Excel",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка всех форм выбранной организации сохранена по пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow);

            #endregion

            if (answer is "Открыть выгрузку")
            {
                Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
            }
        }
    }

    #endregion

    #endregion

    #endregion

    #region INotifyPropertyChanged

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}