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

            foreach(var item in dbm.DBObservableDbSet)
            {
                foreach(var it in item.Reports_Collection)
                {
                    if(it.Master_DB.FormNum_DB!="")
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

                                using (DBExportModel db = new DBExportModel(tmp))
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
                                using (DBExportModel db = new DBExportModel(tmp))
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
                        //var to= dial.ShowAsync(desktop.MainWindow);
                        //to.Start();
                        //to.Wait();
                        //var res=to.Result;
                        if (res != null)
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
#if DEBUG
                                    string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data"), "Excel"), param + ".xlsx");
#else
                                    string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),"data"),"Excel"),param+".xlsx");
#endif
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path), new FileInfo(pth)))
                                    {
                                        //Set some properties of the Excel document
                                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                                        excelPackage.Workbook.Properties.Title = "Report";
                                        excelPackage.Workbook.Properties.Created = DateTime.Now;

                                        if (forms.Count > 0)
                                        {
                                            ExcelWorksheet worksheet10 =
                                                excelPackage.Workbook.Worksheets[param.Split('.')[0] + ".0"];
                                            ExcelWorksheet worksheet =
                                                excelPackage.Workbook.Worksheets[param];

                                            foreach (Report item in forms)
                                            {

                                                var findReports = from t in Local_Reports.Reports_Collection
                                                                  where t.Report_Collection.Contains(item)
                                                                  select t;
                                                var reps = findReports.FirstOrDefault();
                                                foreach (var cell in worksheet.Cells[1, 1, 50, 50])
                                                {
                                                    var text = cell.Text;
                                                    var address = cell.Address;
                                                    string RowStr = address;
                                                    string ColumnStr = address;
                                                    foreach (var ch in address)
                                                    {
                                                        try
                                                        {
                                                            var num = Convert.ToInt32(ch + "");
                                                            ColumnStr = ColumnStr.Replace(ch + "", "");
                                                        }
                                                        catch
                                                        {
                                                            RowStr = RowStr.Replace(ch + "", "");
                                                        }
                                                    }
                                                    int Row = Convert.ToInt32(RowStr);
                                                    int Column = 0;
                                                    for (int i = ColumnStr.Length - 1; i >= 0; i--)
                                                    {
                                                        int num = ColumnStr[i] - 'A' + 1;
                                                        Column += num * Convert.ToInt32(Math.Pow(26, i));
                                                    }

                                                    var inRow = Row;
                                                    var inColumn = Column;
                                                    if (cell.Merge == true)
                                                    {
                                                        for (int i = 0; i < 100; i++)
                                                        {
                                                            var tmpCell = worksheet.Cells[inRow, inColumn];
                                                            if (tmpCell.Merge == true)
                                                            {
                                                                inColumn++;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }

                                                    }

                                                    #region Top
                                                    if (text == "Дата окончания предыдущего отчетного периода")
                                                    {
                                                        worksheet.Cells[inRow, inColumn].Value=item.StartPeriod_DB;
                                                    }
                                                    if (text == "Дата окончания настящего отчетного периода")
                                                    {
                                                        worksheet.Cells[inRow, inColumn].Value = item.EndPeriod_DB;
                                                    }
                                                    if (text == "Номер корректировки")
                                                    {
                                                        worksheet.Cells[inRow, inColumn].Value = item.CorrectionNumber_DB;
                                                    }
                                                    #endregion

                                                    #region Bottom
                                                    if (text == "(Должность)")
                                                    {
                                                        worksheet.Cells[inRow-1, inColumn].Value = item.GradeExecutor_DB;
                                                    }
                                                    if (text == "(Фамилия, имя, отчество (при наличии))")
                                                    {
                                                        worksheet.Cells[inRow - 1, inColumn].Value = item.FIOexecutor_DB;
                                                    }
                                                    if (text == "(Телефон)")
                                                    {
                                                        worksheet.Cells[inRow - 1, inColumn].Value = item.ExecPhone_DB;
                                                    }
                                                    if (text == "(Электронная почта)")
                                                    {
                                                        worksheet.Cells[inRow - 1, inColumn].Value = item.ExecEmail_DB;
                                                    }
                                                    #endregion
                                                }

                                                if (reps != null)
                                                {
                                                    List<IKey> lst = item[param].OrderBy(x=>((Form)x).NumberInOrder_DB).ToList();

                                                    if (lst.Count > 0)
                                                    {
                                                        var count = 0;

                                                        foreach (var it in lst)
                                                        {
                                                            if (it != null)
                                                            {
                                                                if (it is Form11)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form11)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form12)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form12)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form13)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form13)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form14)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(13 + count, 1, 13 + count - 1);
                                                                    }
                                                                    ((Form14)(it)).ExcelRow(worksheet, 13 + count, 1);
                                                                }
                                                                if (it is Form15)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(10 + count, 1, 10 + count - 1);
                                                                    }
                                                                    ((Form15)(it)).ExcelRow(worksheet, 10 + count, 1);
                                                                }
                                                                if (it is Form16)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form16)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form17)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(13 + count, 1, 13 + count - 1);
                                                                    }
                                                                    ((Form17)(it)).ExcelRow(worksheet, 13 + count, 1);
                                                                }
                                                                if (it is Form18)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(13 + count, 1, 13 + count - 1);
                                                                    }
                                                                    ((Form18)(it)).ExcelRow(worksheet, 13 + count, 1);
                                                                }
                                                                if (it is Form19)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(12 + count, 1, 12 + count - 1);
                                                                    }
                                                                    ((Form19)(it)).ExcelRow(worksheet, 12 + count, 1);
                                                                }

                                                                if (it is Form21)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(10 + count, 1, 10 + count - 1);
                                                                    }
                                                                    ((Form21)(it)).ExcelRow(worksheet, 10 + count, 1);
                                                                }
                                                                if (it is Form22)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form22)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form23)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form23)(it)).ExcelRow(worksheet, 11 + count, 1);
                                                                }
                                                                if (it is Form24)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(12 + count, 1, 12 + count - 1);
                                                                    }
                                                                    ((Form24)(it)).ExcelRow(worksheet, 12 + count, 1);
                                                                }
                                                                if (it is Form25)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(12 + count, 1, 12 + count - 1);
                                                                    }
                                                                    ((Form25)(it)).ExcelRow(worksheet, 12 + count, 1);
                                                                }
                                                                if (it is Form26)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(9 + count, 1, 9 + count - 1);
                                                                    }
                                                                    ((Form26)(it)).ExcelRow(worksheet, 9 + count, 1);
                                                                }
                                                                if (it is Form27)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(12 + count, 1, 12 + count - 1);
                                                                    }
                                                                    ((Form27)(it)).ExcelRow(worksheet, 12 + count, 1);
                                                                }
                                                                if (it is Form28)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(15 + count, 1, 15 + count - 1);
                                                                    }
                                                                    ((Form28)(it)).ExcelRow(worksheet, 15 + count, 1);
                                                                }
                                                                if (it is Form29)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(9 + count, 1, 9 + count - 1);
                                                                    }
                                                                    ((Form29)(it)).ExcelRow(worksheet, 9 + count, 1);
                                                                }
                                                                if (it is Form210)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(8 + count, 1, 8 + count - 1);
                                                                    }
                                                                    ((Form210)(it)).ExcelRow(worksheet, 8 + count, 1);
                                                                }
                                                                if (it is Form211)
                                                                {
                                                                    if (count >= 1 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(9 + count, 1, 9 + count - 1);
                                                                    }
                                                                    ((Form211)(it)).ExcelRow(worksheet, count, 1);
                                                                }
                                                                if (it is Form212)
                                                                {
                                                                    if (count >= 5 && count != lst.Count - 1)
                                                                    {
                                                                        worksheet.InsertRow(11 + count, 1, 11 + count - 1);
                                                                    }
                                                                    ((Form212)(it)).ExcelRow(worksheet, count, 1);
                                                                }

                                                                count++;
                                                            }
                                                        }
                                                        var primcnt = 1;
                                                        while (true)
                                                        {
                                                            if (worksheet.Cells[primcnt, 1].Value != null)
                                                            {
                                                                if (worksheet.Cells[primcnt, 1].Value.ToString() == "Примечание:"||
                                                                    worksheet.Cells[primcnt, 1].Value.ToString() == "Примечания:" )
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                            primcnt++;
                                                        }
                                                        primcnt += 2;

                                                        var ty = 0;
                                                        foreach (Note i in item.Notes)
                                                        {
                                                            if (ty >= 1 && ty != item.Notes.Count - 1)
                                                            {
                                                                worksheet.InsertRow(primcnt + ty, 1, primcnt + ty - 1);
                                                                var l = worksheet.MergedCells.ToList();
                                                                foreach (var t in l)
                                                                {
                                                                    if (t.Split(':')[0].Contains((primcnt + ty - 1).ToString()))
                                                                    {
                                                                        var fst = t.Split(':')[0].Replace((primcnt + ty - 1).ToString(), (primcnt + ty).ToString());
                                                                        var sec = t.Split(':')[1].Replace((primcnt + ty - 1).ToString(), (primcnt + ty).ToString());
                                                                        worksheet.Cells[fst + ":" + sec].Merge = true;
                                                                    }
                                                                }
                                                            }
                                                            var next = 'A';
                                                            bool flag1 = true;
                                                            bool flag2 = false;
                                                            bool flag3 = false;
                                                            foreach (var t in worksheet.MergedCells)
                                                            {
                                                                if (t.Split(':')[0].Contains((primcnt + ty).ToString()))
                                                                {
                                                                    var fst = t.Split(':')[0].Replace((primcnt + ty - 1).ToString(), (primcnt + ty).ToString());
                                                                    var sec = t.Split(':')[1].Replace((primcnt + ty - 1).ToString(), (primcnt + ty).ToString());

                                                                    if ((fst + ":" + sec).Contains(next))
                                                                    {
                                                                        if (flag1)
                                                                        {
                                                                            worksheet.Cells[fst + ":" + sec].Value = i.RowNumber_DB;
                                                                            flag1 = false;
                                                                            flag2 = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (flag3)
                                                                            {
                                                                                worksheet.Cells[fst + ":" + sec].Value = i.Comment_DB;
                                                                                flag3 = false;
                                                                                flag1 = true;
                                                                                break;
                                                                            }
                                                                        }

                                                                        next = Convert.ToChar(Convert.ToInt32(next) + worksheet.Cells[fst + ":" + sec].Columns);
                                                                    }
                                                                    if (flag2)
                                                                    {
                                                                        worksheet.Cells[next +(primcnt + ty).ToString()].Value = i.GraphNumber_DB;
                                                                        flag2 = false;
                                                                        flag3 = true;
                                                                        next = Convert.ToChar(Convert.ToInt32(next) + 1);
                                                                    }
                                                                }
                                                            }
                                                            ty++;
                                                        }
                                                    }
                                                }
                                            }
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
                                                //Form10.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            else
                                            {
                                                masterheaderlength = Form20.ExcelHeader(worksheet, 1, 1);
                                                //Form10.ExcelHeader(worksheetPrim, 1, 1);
                                            }
                                            foreach (Report item in forms)
                                            {

                                                var findReports = from t in Local_Reports.Reports_Collection
                                                                  where t.Report_Collection.Contains(item)
                                                                  select t;
                                                var reps = findReports.FirstOrDefault();
                                                if (reps != null)
                                                {
                                                    List<IKey> lst = item[param].OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
                                                    Note.ExcelHeader(worksheetPrim, 1, 1);
                                                    var cnty = 2;
                                                    foreach (var i in item.Notes)
                                                    {
                                                        var mstrep = reps.Master_DB;
                                                        i.ExcelRow(worksheetPrim, cnty,1);
                                                        cnty++;
                                                    }
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

                                                    if (lst.Count > 0)
                                                    {
                                                        var count = 2;
                                                        foreach (var it in lst)
                                                        {
                                                            if (it != null)
                                                            {
                                                                if (it is Form11)
                                                                {
                                                                    ((Form11)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form12)
                                                                {
                                                                    ((Form12)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form13)
                                                                {
                                                                    ((Form13)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form14)
                                                                {
                                                                    ((Form14)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form15)
                                                                {
                                                                    ((Form15)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form16)
                                                                {
                                                                    ((Form16)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form17)
                                                                {
                                                                    ((Form17)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form18)
                                                                {
                                                                    ((Form18)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form19)
                                                                {
                                                                    ((Form19)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }

                                                                if (it is Form21)
                                                                {
                                                                    ((Form21)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form22)
                                                                {
                                                                    ((Form22)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form23)
                                                                {
                                                                    ((Form23)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form24)
                                                                {
                                                                    ((Form24)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form25)
                                                                {
                                                                    ((Form25)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form26)
                                                                {
                                                                    ((Form26)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form27)
                                                                {
                                                                    ((Form27)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form28)
                                                                {
                                                                    ((Form28)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form29)
                                                                {
                                                                    ((Form29)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form210)
                                                                {
                                                                    ((Form210)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form211)
                                                                {
                                                                    ((Form211)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form212)
                                                                {
                                                                    ((Form212)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                var mstrep = reps.Master_DB;
                                                                if (param.Split('.')[0] == "1")
                                                                {
                                                                    if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                                                    {
                                                                        reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                                                    {
                                                                        reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                }

                                                                count++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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

                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path)))
                                    {
                                        //Set some properties of the Excel document
                                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                                        excelPackage.Workbook.Properties.Title = "Report";
                                        excelPackage.Workbook.Properties.Created = DateTime.Now;

                                        List<Report> rep_lst = new List<Report>();
                                        if (param.Split('.')[0] == "1")
                                        {
                                            foreach (var item in Local_Reports.Reports_Collection10)
                                            {
                                                foreach (var it in item.Report_Collection)
                                                {
                                                    if (it.FormNum_DB == param)
                                                    {
                                                        rep_lst.Add(it);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var item in Local_Reports.Reports_Collection20)
                                            {
                                                foreach (var it in item.Report_Collection)
                                                {
                                                    if (it.FormNum_DB == param)
                                                    {
                                                        rep_lst.Add(it);
                                                    }
                                                }
                                            }
                                        }

                                        if (rep_lst.Count > 0)
                                        {

                                            ExcelWorksheet worksheet =
                                                excelPackage.Workbook.Worksheets.Add("Отчеты " + param);
                                            ExcelWorksheet worksheetPrim =
                                                excelPackage.Workbook.Worksheets.Add("Примечания " + param);

                                            var masterheaderlength = 0;
                                            if (param.Split('.')[0] == "1")
                                            {
                                                masterheaderlength = Form10.ExcelHeader(worksheet, 1, 1);
                                            }
                                            else
                                            {
                                                masterheaderlength = Form20.ExcelHeader(worksheet, 1, 1);
                                            }
                                            var count = 2;
                                            foreach (var item in rep_lst)
                                            {

                                                var findReports = from t in Local_Reports.Reports_Collection
                                                                  where t.Report_Collection.Contains(item)
                                                                  select t;
                                                var reps = findReports.FirstOrDefault();
                                                if (reps != null)
                                                {
                                                    List<IKey> lst = item[param].OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
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

                                                    if (lst.Count > 0)
                                                    {
                                                        foreach (var it in lst)
                                                        {
                                                            if (it != null)
                                                            {
                                                                if (it is Form11)
                                                                {
                                                                    ((Form11)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form12)
                                                                {
                                                                    ((Form12)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form13)
                                                                {
                                                                    ((Form13)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form14)
                                                                {
                                                                    ((Form14)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form15)
                                                                {
                                                                    ((Form15)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form16)
                                                                {
                                                                    ((Form16)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form17)
                                                                {
                                                                    ((Form17)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form18)
                                                                {
                                                                    ((Form18)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form19)
                                                                {
                                                                    ((Form19)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }

                                                                if (it is Form21)
                                                                {
                                                                    ((Form21)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form22)
                                                                {
                                                                    ((Form22)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form23)
                                                                {
                                                                    ((Form23)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form24)
                                                                {
                                                                    ((Form24)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form25)
                                                                {
                                                                    ((Form25)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form26)
                                                                {
                                                                    ((Form26)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form27)
                                                                {
                                                                    ((Form27)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form28)
                                                                {
                                                                    ((Form28)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form29)
                                                                {
                                                                    ((Form29)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form210)
                                                                {
                                                                    ((Form210)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form211)
                                                                {
                                                                    ((Form211)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                if (it is Form212)
                                                                {
                                                                    ((Form212)(it)).ExcelRow(worksheet, count, masterheaderlength + 1);
                                                                }
                                                                var mstrep = reps.Master_DB;
                                                                if (param.Split('.')[0] == "1")
                                                                {
                                                                    if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                                                    {
                                                                        reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                                                    {
                                                                        reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1);
                                                                    }
                                                                }

                                                                count++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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