using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using DynamicData;
using Models.Abstracts;
using Models;
using OfficeOpenXml;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        private DBObservable _local_Reports = new();

        System.Timers.Timer tm = new System.Timers.Timer(5000);

        public MainWindowVM()
        {
            //tm.Elapsed += (x, y) =>
            //{
            //    GC.Collect(1000, GCCollectionMode.Forced);
            //};
            //tm.AutoReset = true;
            //tm.Start();

            string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string path = Path.GetPathRoot(system);
            var tmp = Path.Combine(path, "RAO");
            var pty = tmp;
            tmp = Path.Combine(tmp, "temp");
            Directory.CreateDirectory(tmp);
            var fl = Directory.GetFiles(tmp);
            foreach (var file in fl)
            {
                File.Delete(file);
            }

            var a = Spravochniks.SprRadionuclids;
            var b = Spravochniks.SprTypesToRadionuclids;
            var i = 0;
            bool flag = false;

            DBModel dbm = null;
            foreach (var file in Directory.GetFiles(pty))
            {
                try
                {
                    StaticConfiguration.DBPath = file;
                    StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);

                    dbm = StaticConfiguration.DBModel;
                    dbm.Database.Migrate();
                    flag = true;
                    break;
                }
                catch
                {
                    i++;
                }
            }
            if (!flag)
            {
                StaticConfiguration.DBPath = Path.Combine(pty, "Local" + "_" + i + ".raodb");
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;
                dbm.Database.Migrate();
            }

            dbm.LoadTables();
            if (dbm.DBObservableDbSet.Local.Count() == 0) dbm.DBObservableDbSet.Add(new DBObservable());

            foreach (var item in dbm.DBObservableDbSet)
            {
                foreach (var it in item.Reports_Collection)
                {
                    if (it.Master_DB.FormNum_DB != "")
                    {
                        if (it.Master_DB.FormNum_DB == "1.0")
                        {
                            if (it.Master.Rows10.Count == 0)
                            {
                                it.Master.Rows10.Add((Form10)FormCreator.Create("1.0"));
                                it.Master.Rows10.Add((Form10)FormCreator.Create("1.0"));
                            }
                        }
                        if (it.Master_DB.FormNum_DB == "2.0")
                        {
                            if (it.Master.Rows20.Count == 0)
                            {
                                it.Master.Rows20.Add((Form20)FormCreator.Create("2.0"));
                                it.Master.Rows20.Add((Form20)FormCreator.Create("2.0"));
                            }
                        }
                    }
                }
            }

            dbm.SaveChanges();

            Local_Reports = dbm.DBObservableDbSet.Local.First();

            Local_Reports.PropertyChanged += Local_ReportsChanged;

            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddReport = ReactiveCommand.CreateFromTask<string>(_AddReport);
            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);

            ImportForm =
                ReactiveCommand.CreateFromTask(_ImportForm);

            ExportForm =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_ExportForm);

            ChangeForm =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_ChangeForm);
            ChangeReport =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_ChangeReport);
            DeleteForm =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_DeleteForm);
            DeleteReport =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_DeleteReport);

            Excel_Export =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_Excel_Export);
            All_Excel_Export =
                ReactiveCommand.CreateFromTask<string>(_All_Excel_Export);
        }

        private IEnumerable<Reports> _selectedReports = new ObservableCollectionWithItemPropertyChanged<Reports>();
        public IEnumerable<Reports> SelectedReports
        {
            get => _selectedReports;
            set
            {
                if (_selectedReports != value)
                {
                    _selectedReports = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public DBObservable Local_Reports
        {
            get => _local_Reports;
            set
            {
                if (_local_Reports != value)
                {
                    _local_Reports = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ReactiveCommand<Unit, Unit> OpenSettings { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }

        public ReactiveCommand<string, Unit> ChooseForm { get; }

        public ReactiveCommand<string, Unit> AddReport { get; }
        public ReactiveCommand<string, Unit> AddForm { get; }

        public ReactiveCommand<Unit, Unit> ImportForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> ExportForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> ChangeForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> ChangeReport { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> DeleteForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> DeleteReport { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> Excel_Export { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IKey>, Unit> Print_Excel_Export { get; }
        public ReactiveCommand<string, Unit> All_Excel_Export { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void _AddSort(string param)
        {
            var type = param.Split('.')[0];
            var path = param.Split('.')[1];

            //FormModel_Local.Dictionary.Filters.SortPath = path;
        }

        private async Task _AddForm(string param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = desktop.MainWindow as MainWindow;
                if (t.SelectedReports.Count() != 0)
                {
                    var y = t.SelectedReports.First() as Reports;
                    if (y.Master.FormNum_DB.Split(".")[0] == param.Split(".")[0])
                    {
                        //y.Report_Collection.Add(rt);
                        var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                        FormChangeOrCreate frm = new(param, y);
                        await frm.ShowDialog<Form>(desktop.MainWindow);
                        frm.Close();

                        t.SelectedReports = tmp;
                    }
                }
            }
        }
        private async Task _AddReport(string param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (param.Split('.')[1] == "0")
                {
                    var t = desktop.MainWindow as MainWindow;
                    var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                    FormChangeOrCreate frm = new(param, Local_Reports);
                    await frm.ShowDialog<Form>(desktop.MainWindow);
                    frm.Close();

                    t.SelectedReports = tmp;
                }
            }
        }

        private async Task _ExportForm(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        var a = DateTime.Now.Date;
                        ((Report)item).ExportDate.Value = a.Day + "." + a.Month + "." + a.Year;
                    }
                    var obj = param.First();
                    OpenFolderDialog dial = new OpenFolderDialog();
                    var res = await dial.ShowAsync(desktop.MainWindow);
                    if (res != null)
                    {
                        if (res != "")
                        {
                            var dt = DateTime.Now;
                            var filename = "Report_" +
                                           dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute +
                                           "_" + dt.Second;
                            var rep = (Report)obj;
                            rep.ExportDate.Value = dt.Day + "." + dt.Month + "." + dt.Year;
                            var findReports = from t in Local_Reports.Reports_Collection
                                              where t.Report_Collection.Contains(rep)
                                              select t;
                            var rt = findReports.FirstOrDefault();
                            if (rt != null)
                            {
                                string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                                string path = Path.GetPathRoot(system);
                                var tmp = Path.Combine(path, "RAO");
                                tmp = Path.Combine(tmp, "temp");
                                Directory.CreateDirectory(tmp);
                                tmp = Path.Combine(tmp, filename + "_exp" + ".raodb");

                                using (DBModel db = new DBModel(tmp))
                                {
                                    try
                                    {
                                        Reports rp = new Reports();
                                        rp.Master = rt.Master;
                                        rp.Report_Collection.Add(rep);
                                        if (File.Exists(tmp))
                                        {
                                            db.Database.Migrate();
                                        }
                                        else
                                        {
                                            db.Database.Migrate();
                                        }
                                        db.ReportsCollectionDbSet.Add(rp);
                                        db.SaveChanges();

                                        string filename2 = "";
                                        if (rp.Master.Rows10.Count > 0)
                                        {
                                            if (rp.Master.Rows10[1].RegNo_DB == "")
                                            {
                                                filename2 += rp.Master.Rows10[0].RegNo_DB;
                                            }
                                            else
                                            {
                                                filename2 += rp.Master.Rows10[1].RegNo_DB;
                                            }
                                            if (rp.Master.Rows10[1].Okpo_DB == "")
                                            {
                                                filename2 += "_" + rp.Master.Rows10[0].Okpo_DB;
                                            }
                                            else
                                            {
                                                filename2 += "_" + rp.Master.Rows10[1].Okpo_DB;
                                            }
                                            filename2 += "_" + rep.FormNum_DB;
                                            filename2 += "_" + rep.StartPeriod_DB;
                                            filename2 += "_" + rep.EndPeriod_DB;
                                        }
                                        else
                                        {
                                            if (rp.Master.Rows20.Count > 0)
                                            {
                                                if (rp.Master.Rows20[1].RegNo_DB == "")
                                                {
                                                    filename2 += rp.Master.Rows20[0].RegNo_DB;
                                                }
                                                else
                                                {
                                                    filename2 += rp.Master.Rows20[1].RegNo_DB;
                                                }
                                                if (rp.Master.Rows20[1].Okpo_DB == "")
                                                {
                                                    filename2 += "_" + rp.Master.Rows20[0].Okpo_DB;
                                                }
                                                else
                                                {
                                                    filename2 += "_" + rp.Master.Rows20[1].Okpo_DB;
                                                }
                                                filename2 += "_" + rep.FormNum_DB;
                                                filename2 += "_" + rep.Year_DB;
                                            }
                                        }

                                        res = Path.Combine(res, filename2 + ".raodb");

                                        StaticConfiguration.DBModel.SaveChanges();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                        throw;
                                    }
                                }
                                try
                                {
                                    File.Copy(tmp, res);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
        }

        private async Task _ImportForm()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                OpenFileDialog dial = new OpenFileDialog();
                dial.AllowMultiple = true;
                var answ = await dial.ShowAsync(desktop.MainWindow);
                foreach (var res in answ)
                {
                    if (res != null)
                    {
                        if (res != "")
                        {
                            string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                            string path = Path.GetPathRoot(system);
                            var tmp = Path.Combine(path, "RAO");
                            tmp = Path.Combine(tmp, "temp");
                            Directory.CreateDirectory(tmp);
                            tmp = Path.Combine(tmp, Path.GetFileNameWithoutExtension(res)) + "_imp_1" + ".raodb";

                            while (true)
                            {
                                if (File.Exists(tmp))
                                {
                                    var rt = Path.GetFileNameWithoutExtension(tmp).Split('_');
                                    var num = Convert.ToInt32(rt.Last());
                                    tmp = "";
                                    tmp = Path.Combine(path, "RAO");
                                    tmp = Path.Combine(tmp, "temp");
                                    tmp = Path.Combine(tmp, Path.GetFileNameWithoutExtension(res) + "_imp_" + (num + 1) + ".raodb");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            try
                            {
                                File.Copy(res, tmp, true);
                                using (DBModel db = new DBModel(tmp))
                                {
                                    try
                                    {
                                        if (File.Exists(tmp))
                                        {
                                            db.Database.Migrate();
                                        }
                                        else
                                        {
                                            db.Database.Migrate();
                                        }
                                        db.LoadTables();
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                        throw;
                                    }

                                    foreach (var item in db.ReportsCollectionDbSet)
                                    {
                                        var tb11 = from t in Local_Reports.Reports_Collection10
                                                   where (item.Master.Rows10[0].Okpo_DB != "") &&
                                                   (t.Master.Rows10[0].Okpo_DB != "") &&
                                                   (t.Master.Rows10[0].Okpo_DB == item.Master.Rows10[0].Okpo_DB) &&
                                                   (t.Master.Rows10[1].Okpo_DB == item.Master.Rows10[1].Okpo_DB)
                                                   select t;
                                        var tb21 = from t in Local_Reports.Reports_Collection20
                                                   where (item.Master.Rows20[0].Okpo_DB != "") &&
                                                   (t.Master.Rows20[0].Okpo_DB != "") &&
                                                   (t.Master.Rows20[0].Okpo_DB == item.Master.Rows20[0].Okpo_DB) &&
                                                   (t.Master.Rows20[1].Okpo_DB == item.Master.Rows20[1].Okpo_DB)
                                                   select t;

                                        Reports first11 = null;
                                        Reports first21 = null;
                                        try
                                        {
                                            first11 = tb11.FirstOrDefault();
                                        }
                                        catch
                                        {
                                        }

                                        try
                                        {
                                            first21 = tb21.FirstOrDefault();
                                        }
                                        catch
                                        {
                                        }

                                        item.CleanIds();
                                        if (first11 != null)
                                        {
                                            foreach (var it in item.Report_Collection.OrderBy(x => x.NumberInOrder_DB))
                                            {
                                                first11.Report_Collection.Add(it);
                                            }
                                        }
                                        else
                                        {
                                            if (first21 != null)
                                            {
                                                foreach (var it in item.Report_Collection.OrderBy(x => x.NumberInOrder_DB))
                                                {
                                                    first21.Report_Collection.Add(it);
                                                }
                                            }
                                            else
                                            {
                                                Local_Reports.Reports_Collection.Add(item);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }
            StaticConfiguration.DBModel.SaveChanges();
        }

        private async Task _ChangeForm(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    if (obj != null)
                    {
                        var t = desktop.MainWindow as MainWindow;
                        var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                        var rep = (Report)obj;
                        FormChangeOrCreate frm = new(rep.FormNum.Value, rep);
                        await frm.ShowDialog(desktop.MainWindow);

                        t.SelectedReports = tmp;
                    }
                }
        }

        private async Task _ChangeReport(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    if (obj != null)
                    {

                        var t = desktop.MainWindow as MainWindow;
                        var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                        var rep = (Reports)obj;

                        FormChangeOrCreate frm = new FormChangeOrCreate(rep.Master.FormNum.Value, rep.Master);
                        await frm.ShowDialog(desktop.MainWindow);

                        t.SelectedReports = tmp;
                    }
                }
        }

        private async Task _DeleteForm(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = desktop.MainWindow as MainWindow;
                if (t.SelectedReports.Count() != 0)
                {
                    var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
                    var y = t.SelectedReports.First() as Reports;
                    if (param != null)
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

        private async Task _DeleteReport(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (param != null)
                foreach (var item in param)
                    Local_Reports.Reports_Collection.Remove((Reports)item);
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        private int _Excel_Export_Notes(string param, int StartRow, int StartColumn, ExcelWorksheet worksheetPrim, List<Report> forms)
        {
            foreach (Report item in forms)
            {

                var findReports = from t in Local_Reports.Reports_Collection
                                  where t.Report_Collection.Contains(item)
                                  select t;
                var reps = findReports.FirstOrDefault();
                if (reps != null)
                {
                    var cnty = StartRow;
                    foreach (Note i in item.Notes)
                    {
                        var mstrep = reps.Master_DB;
                        i.ExcelRow(worksheetPrim, cnty, StartColumn + 1);
                        var yu = 0;
                        if (param.Split('.')[0] == "1")
                        {
                            if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                            {
                                yu = reps.Master_DB.Rows10[1].ExcelRow(worksheetPrim, cnty, 1);
                            }
                            else
                            {
                                yu = reps.Master_DB.Rows10[0].ExcelRow(worksheetPrim, cnty, 1);
                            }
                        }
                        else
                        {
                            if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                            {
                                yu = reps.Master_DB.Rows20[1].ExcelRow(worksheetPrim, cnty, 1);
                            }
                            else
                            {
                                yu = reps.Master_DB.Rows20[0].ExcelRow(worksheetPrim, cnty, 1);
                            }
                        }

                        item.ExcelRow(worksheetPrim, cnty, yu);
                        cnty++;
                    }
                    StartRow = cnty;
                }
            }
            return StartRow;
        }
        private int _Excel_Export_Rows(string param, int StartRow, int StartColumn, ExcelWorksheet worksheet, List<Report> forms)
        {
            foreach (Report item in forms)
            {

                var findReports = from t in Local_Reports.Reports_Collection
                                  where t.Report_Collection.Contains(item)
                                  select t;
                var reps = findReports.FirstOrDefault();
                if (reps != null)
                {
                    List<IKey> lst = item[param].OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();

                    if (lst.Count > 0)
                    {
                        var count = StartRow;
                        foreach (var it in lst)
                        {
                            if (it != null)
                            {
                                if (it is Form11)
                                {
                                    ((Form11)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form12)
                                {
                                    ((Form12)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form13)
                                {
                                    ((Form13)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form14)
                                {
                                    ((Form14)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form15)
                                {
                                    ((Form15)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form16)
                                {
                                    ((Form16)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form17)
                                {
                                    ((Form17)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form18)
                                {
                                    ((Form18)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form19)
                                {
                                    ((Form19)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }

                                if (it is Form21)
                                {
                                    ((Form21)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form22)
                                {
                                    ((Form22)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form23)
                                {
                                    ((Form23)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form24)
                                {
                                    ((Form24)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form25)
                                {
                                    ((Form25)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form26)
                                {
                                    ((Form26)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form27)
                                {
                                    ((Form27)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form28)
                                {
                                    ((Form28)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form29)
                                {
                                    ((Form29)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form210)
                                {
                                    ((Form210)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form211)
                                {
                                    ((Form211)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                if (it is Form212)
                                {
                                    ((Form212)(it)).ExcelRow(worksheet, count, StartColumn + 1);
                                }
                                var mstrep = reps.Master_DB;
                                var yu = 0;
                                if (param.Split('.')[0] == "1")
                                {
                                    if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                    {
                                        yu = reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1);
                                    }
                                    else
                                    {
                                        yu = reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1);
                                    }
                                }
                                else
                                {
                                    if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                    {
                                        yu = reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1);
                                    }
                                    else
                                    {
                                        yu = reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1);
                                    }
                                }

                                item.ExcelRow(worksheet, count, yu);
                                count++;
                            }
                        }
                        StartRow = count;
                    }
                }
            }
            return StartRow;
        }

        private void _Excel_Print_Titul_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = from t in Local_Reports.Reports_Collection
                              where t.Report_Collection.Contains(form)
                              select t;
            var reps = findReports.FirstOrDefault();


        }
        private async Task _Print_Excel_Export(ObservableCollectionWithItemPropertyChanged<IKey> forms)
        {
            try
            {
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = {
                        "xlsx"
                    }
                    };
                    var param = "";
                    if (forms != null)
                    {
                        if (forms.Count > 0)
                        {
                            var t = (Report)forms.First();
                            param = t.FormNum_DB;
                        }
                    }
                    dial.Filters.Add(filter);
                    if (param != "")
                    {
                        var res = await dial.ShowAsync(desktop.MainWindow);
                        if (res != null)
                        {
                            if (res.Count() != 0)
                            {
                                var path = res;
                                if (!path.Contains(".xlsx"))
                                {
                                    path += ".xlsx";
                                }
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
#if DEBUG
                                string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data"), "Excel"), param+".xlsx");
#else
                                string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),"data"), "Excel"), param+".xlsx");
#endif
                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path),new FileInfo(pth)))
                                    {
                                        
                                        excelPackage.Save();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async Task _Excel_Export(ObservableCollectionWithItemPropertyChanged<IKey> forms)
        {
            try
            {
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = {
                        "xlsx"
                    }
                    };
                    var param = "";
                    if (forms != null)
                    {
                        if (forms.Count > 0)
                        {
                            var t = (Report)forms.First();
                            param = t.FormNum_DB;
                        }
                    }
                    dial.Filters.Add(filter);
                    if (param != "")
                    {
                        var res = await dial.ShowAsync(desktop.MainWindow);
                        if (res != null)
                        {
                            if (res.Count() != 0)
                            {
                                var path = res;
                                if (!path.Contains(".xlsx"))
                                {
                                    path += ".xlsx";
                                }
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
                                    {
                                        //Set some properties of the Excel document
                                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                                        excelPackage.Workbook.Properties.Title = "Report";
                                        excelPackage.Workbook.Properties.Created = DateTime.Now;

                                        if (forms.Count > 0)
                                        {
                                            ExcelWorksheet worksheet =
                                                excelPackage.Workbook.Worksheets.Add("Отчеты " + param);
                                            ExcelWorksheet worksheetPrim =
                                                excelPackage.Workbook.Worksheets.Add("Примечания " + param);

                                            var masterheaderlength = 0;
                                            if (param.Split('.')[0] == "1")
                                            {
                                                masterheaderlength = Form10.ExcelHeader(worksheet, 1, 1);
                                                masterheaderlength = Form10.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            else
                                            {
                                                masterheaderlength = Form20.ExcelHeader(worksheet, 1, 1);
                                                masterheaderlength = Form20.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            var t = Report.ExcelHeader(worksheet, param, 1, masterheaderlength);
                                            Report.ExcelHeader(worksheetPrim, param, 1, masterheaderlength);
                                            masterheaderlength += t;
                                            if (param == "1.1")
                                            {
                                                Form11.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.2")
                                            {
                                                Form12.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.3")
                                            {
                                                Form13.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.4")
                                            {
                                                Form14.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.5")
                                            {
                                                Form15.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.6")
                                            {
                                                Form16.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.7")
                                            {
                                                Form17.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.8")
                                            {
                                                Form18.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.9")
                                            {
                                                Form19.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }

                                            if (param == "2.1")
                                            {
                                                Form21.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.2")
                                            {
                                                Form22.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.3")
                                            {
                                                Form23.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.4")
                                            {
                                                Form24.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.5")
                                            {
                                                Form25.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.6")
                                            {
                                                Form26.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.7")
                                            {
                                                Form27.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.8")
                                            {
                                                Form28.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.9")
                                            {
                                                Form29.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.10")
                                            {
                                                Form210.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.11")
                                            {
                                                Form211.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.12")
                                            {
                                                Form212.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            Note.ExcelHeader(worksheetPrim, 1, masterheaderlength + 1);
                                            var lst = new List<Report>();
                                            foreach (Report item in forms)
                                            {
                                                lst.Add(item);
                                            }
                                            _Excel_Export_Rows(param, 2, masterheaderlength, worksheet, lst);
                                            _Excel_Export_Notes(param, 2, masterheaderlength, worksheetPrim, lst);

                                            excelPackage.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async Task _All_Excel_Export(string param)
        {
            try
            {
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = {
                        "xlsx"
                        }
                    };
                    dial.Filters.Add(filter);
                    if (param != "")
                    {
                        var res = await dial.ShowAsync(desktop.MainWindow);
                        if (res != null)
                        {
                            if (res.Count() != 0)
                            {
                                var path = res;
                                if (!path.Contains(".xlsx"))
                                {
                                    path += ".xlsx";
                                }
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
                                    {
                                        //Set some properties of the Excel document
                                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                                        excelPackage.Workbook.Properties.Title = "Report";
                                        excelPackage.Workbook.Properties.Created = DateTime.Now;

                                        if (Local_Reports.Reports_Collection.Count > 0)
                                        {
                                            ExcelWorksheet worksheet =
                                                excelPackage.Workbook.Worksheets.Add("Отчеты " + param);
                                            ExcelWorksheet worksheetPrim =
                                                excelPackage.Workbook.Worksheets.Add("Примечания " + param);

                                            var masterheaderlength = 0;
                                            if (param.Split('.')[0] == "1")
                                            {
                                                masterheaderlength = Form10.ExcelHeader(worksheet, 1, 1);
                                                masterheaderlength = Form10.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            else
                                            {
                                                masterheaderlength = Form20.ExcelHeader(worksheet, 1, 1);
                                                masterheaderlength = Form20.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            var t = Report.ExcelHeader(worksheet, param, 1, masterheaderlength);
                                            Report.ExcelHeader(worksheetPrim, param, 1, masterheaderlength);
                                            masterheaderlength += t;
                                            if (param == "1.1")
                                            {
                                                Form11.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.2")
                                            {
                                                Form12.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.3")
                                            {
                                                Form13.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.4")
                                            {
                                                Form14.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.5")
                                            {
                                                Form15.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.6")
                                            {
                                                Form16.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.7")
                                            {
                                                Form17.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.8")
                                            {
                                                Form18.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "1.9")
                                            {
                                                Form19.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }

                                            if (param == "2.1")
                                            {
                                                Form21.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.2")
                                            {
                                                Form22.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.3")
                                            {
                                                Form23.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.4")
                                            {
                                                Form24.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.5")
                                            {
                                                Form25.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.6")
                                            {
                                                Form26.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.7")
                                            {
                                                Form27.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.8")
                                            {
                                                Form28.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.9")
                                            {
                                                Form29.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.10")
                                            {
                                                Form210.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.11")
                                            {
                                                Form211.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            if (param == "2.12")
                                            {
                                                Form212.ExcelHeader(worksheet, 1, masterheaderlength + 1);
                                            }
                                            Note.ExcelHeader(worksheetPrim, 1, masterheaderlength + 1);

                                            var tyu = 2;
                                            var lst = new List<Report>();
                                            foreach (Reports item in Local_Reports.Reports_Collection)
                                            {
                                                lst.AddRange(item.Report_Collection);
                                            }

                                            _Excel_Export_Rows(param, tyu, masterheaderlength, worksheet, lst);
                                            _Excel_Export_Notes(param, tyu, masterheaderlength, worksheetPrim, lst);

                                            excelPackage.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Local_Reports");
        }
    }
}