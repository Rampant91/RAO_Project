using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Collections;
using DBRealization;
using DynamicData;
using Models.Abstracts;
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

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        private DBObservable _local_Reports = new();

        System.Timers.Timer tm = new System.Timers.Timer(5000);

        public MainWindowVM()
        {
            tm.Elapsed += (x, y) =>
            {
                GC.Collect(1000,GCCollectionMode.Forced);
            };
            tm.AutoReset = true;
            tm.Start();

            string system = Environment.GetFolderPath(Environment.SpecialFolder.System);
            string path = Path.GetPathRoot(system);
            var tmp = Path.Combine(path, "RAO");
            tmp = Path.Combine(tmp, "temp");
            Directory.CreateDirectory(tmp);
            var fl=Directory.GetFiles(tmp);
            foreach (var file in fl)
            {   
                File.Delete(file);
            }

            var a = Spravochniks.SprRadionuclids;
            var b = Spravochniks.SprTypesToRadionuclids;

            Local_Reports = new DBObservable();
            var dbm = StaticConfiguration.DBModel;
            var t = dbm.Database.EnsureCreated();
            
            dbm.LoadTables();
            if (dbm.DBObservableDbSet.Local.Count() == 0) dbm.DBObservableDbSet.Add(new DBObservable());

            dbm.SaveChanges();

            Local_Reports = dbm.DBObservableDbSet.Local.First();
            //foreach(var report in Local_Reports.Reports_Collection)
            //{
            //    switch (report.Master.FormNum.Value[0])
            //    {
            //        case '1':
            //            report.Master.OkpoRep = report.Master.OkpoRep;
            //            report.Master.RegNoRep = report.Master.RegNoRep;
            //            report.Master.ShortJurLicoRep = report.Master.ShortJurLicoRep;
            //            break;
            //        case '2':
            //            report.Master.OkpoRep1 = report.Master.OkpoRep1;
            //            report.Master.RegNoRep1 = report.Master.RegNoRep1;
            //            report.Master.ShortJurLicoRep1 = report.Master.ShortJurLicoRep1;
            //            break;
            //    }
            //}
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

                                using (DBExportModel db = new DBExportModel(tmp))
                                {
                                    try
                                    {
                                        Reports rp = new Reports();
                                        rp.Master = rt.Master;
                                        rp.Report_Collection.Add(rep);
                                        db.Database.EnsureCreated();
                                        db.ReportsCollectionDbSet.Add(rp);
                                        db.SaveChanges();

                                        string filename2 = "";
                                        if(rp.Master.Rows10.Count>0)
                                        {
                                            if(rp.Master.Rows10[1].RegNo_DB=="")
                                            {
                                                filename2 += rp.Master.Rows10[0].RegNo_DB;
                                            }
                                            else
                                            {
                                                filename2 += rp.Master.Rows10[1].RegNo_DB;
                                            }
                                            if (rp.Master.Rows10[1].Okpo_DB == "")
                                            {
                                                filename2 += "_"+rp.Master.Rows10[0].Okpo_DB;
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
                                using (DBExportModel db = new DBExportModel(tmp))
                                {
                                    try
                                    {
                                        db.Database.EnsureCreated();
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
                                            first11.Report_Collection.AddRange(item.Report_Collection);
                                        }
                                        else
                                        {
                                            if (first21 != null)
                                            {
                                                first21.Report_Collection.AddRange(item.Report_Collection);
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
                        Local_Reports.Reports_Collection.Remove((Reports) item);
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }

        private async Task _Excel_Export(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel"
                };
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                if (param.Count != null)
                {
                    var res = await dial.ShowAsync(desktop.MainWindow);
                    if(res!=null)
                    {
                        if (res.Count() != 0)
                        {
                            var path = res;
                            if (!path.Contains(".xlsx"))
                            {
                                path += ".xlsx";
                            }

                            if (path != null)
                            {
                                using (ExcelPackage excelPackage = new ExcelPackage())
                                {
                                    //Set some properties of the Excel document
                                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                                    excelPackage.Workbook.Properties.Title = "Report";
                                    excelPackage.Workbook.Properties.Created = DateTime.Now;

                                    var rep = (Report) param.FirstOrDefault();
                                    //Create the WorkSheet
                                    ExcelWorksheet worksheetOrg = excelPackage.Workbook.Worksheets.Add("Организация");
                                    ExcelWorksheet worksheet =
                                        excelPackage.Workbook.Worksheets.Add("Отчет " + rep.FormNum_DB);
                                    ExcelWorksheet worksheetPrim =
                                        excelPackage.Workbook.Worksheets.Add("Примечания" + rep.FormNum_DB);

                                    var findReports = from t in Local_Reports.Reports_Collection
                                        where t.Report_Collection.Contains(rep)
                                        select t;
                                    var reps = findReports.FirstOrDefault();
                                    if (reps != null)
                                    {
                                        Report.ExcelHeader(worksheetOrg, reps.Master.FormNum_DB);
                                        reps.Master.ExcelRow(worksheetOrg, -1);

                                        if (rep != null)
                                        {
                                            Report.ExcelHeader(worksheet, ((Report) rep).FormNum_DB);
                                            rep.ExcelRow(worksheet, -1);

                                            Report.ExcelHeader(worksheetPrim, "Notes");
                                            rep.ExcelRow(worksheetPrim, -2);
                                        }

                                    }

                                    FileInfo fi = new FileInfo(path);

                                    excelPackage.SaveAs(fi);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Local_Reports");
        }
    }
}