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
using OfficeOpenXml.Style;
using ReactiveUI;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Collections;
using FirebirdSql.Data.FirebirdClient;


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
            string system = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                system = Environment.GetFolderPath(Environment.SpecialFolder.System);
            }
            else
            {
                system = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
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
                catch (Exception e)
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
            if (dbm.DBObservableDbSet.Count() == 0) dbm.DBObservableDbSet.Add(new DBObservable());

            foreach (var item in dbm.DBObservableDbSet)
            {
                foreach (var it in item.Reports_Collection)
                {
                    if (it.Master_DB.FormNum_DB != "")
                    {
                        if (it.Master_DB.Rows10.Count==0)
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
                    }
                }
            }

            dbm.SaveChanges();

            Local_Reports = dbm.DBObservableDbSet.Local.First();

            Local_Reports.PropertyChanged += Local_ReportsChanged;

            foreach (var item in Local_Reports.Reports_Collection) 
            {
                item.Sort();
                foreach (var it in item.Report_Collection) 
                {
                    foreach (var _i in it.Notes) 
                    {
                        if (_i.Order == 0) 
                        {
                            _i.Order = GetNumberInOrder(it.Notes);
                        }
                    }
                }
            }

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
                ReactiveCommand.CreateFromTask<IEnumerable>(_DeleteForm);
            DeleteReport =
                ReactiveCommand.CreateFromTask<IEnumerable>(_DeleteReport);

            Excel_Export =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IKey>>(_Excel_Export);
            All_Excel_Export =
                ReactiveCommand.CreateFromTask<string>(_All_Excel_Export);

            ShowDialog = new Interaction<ChangeOrCreateVM, object>();
            ShowMessage = new Interaction<string, string>();
            ShowMessageT = new Interaction<List<string>, string>();

        }

        public Interaction<ChangeOrCreateVM, object> ShowDialog { get; }
        public Interaction<string, string> ShowMessage { get; }

        public Interaction<List<string>, string> ShowMessageT { get; }

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
        public ReactiveCommand<IEnumerable, Unit> DeleteForm { get; }
        public ReactiveCommand<IEnumerable, Unit> DeleteReport { get; }
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
            try
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

                            ChangeOrCreateVM frm = new(param, y);
                            await ShowDialog.Handle(frm);

                            t.SelectedReports = tmp;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                int y = 10;
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
                    ChangeOrCreateVM frm = new(param, Local_Reports);
                    await ShowDialog.Handle(frm);
                    t.SelectedReports = tmp;
                }
            }
        }

        private async Task _ExportForm(ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    OpenFolderDialog dial = new OpenFolderDialog();

                    var res = await dial.ShowAsync(desktop.MainWindow);
                    if (res != null)
                    {
                        foreach (var item in param)
                        {
                            var a = DateTime.Now.Date;
                            ((Report)item).ExportDate.Value = a.Day + "." + a.Month + "." + a.Year;
                        }
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
                                string system = "";
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                                }
                                else
                                {
                                    system = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                }
                                string path = Path.GetPathRoot(system);
                                var tmp = Path.Combine(path, "RAO");
                                tmp = Path.Combine(tmp, "temp");
                                Directory.CreateDirectory(tmp);
                                tmp = Path.Combine(tmp, filename + "_exp" + ".raodb");

                                var tsk = new Task(() =>
                                  {
                                      DBModel db = new DBModel(tmp);
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
                                          if (rp.Master_DB.FormNum_DB == "1.0")
                                          {
                                              filename2 += rp.Master.RegNoRep.Value;
                                              filename2 += "_" + rp.Master.OkpoRep.Value;

                                              filename2 += "_" + rep.CorrectionNumber_DB;
                                              filename2 += "_" + rep.FormNum_DB;
                                              filename2 += "_" + rep.StartPeriod_DB;
                                              filename2 += "_" + rep.EndPeriod_DB;
                                          }
                                          else
                                          {
                                              if (rp.Master.Rows20.Count > 0)
                                              {
                                                  filename2 += rp.Master.RegNoRep1.Value;
                                                  filename2 += rp.Master.OkpoRep1.Value;

                                                  filename2 += "_" + rep.CorrectionNumber_DB;
                                                  filename2 += "_" + rep.FormNum_DB;
                                                  filename2 += "_" + rep.Year_DB;
                                              }
                                          }

                                          res = Path.Combine(res, filename2 + ".raodb");


                                          var t = db.Database.GetDbConnection() as FbConnection;
                                          t.Close();
                                          t.Dispose();

                                          db.Database.CloseConnection();
                                          db.Dispose();

                                      }
                                      catch (Exception e)
                                      {
                                          Console.WriteLine(e);
                                          throw;
                                      }
                                  });
                                tsk.Start();
                                await tsk.ContinueWith((a) => {
                                    try
                                    {
                                        //var sourceFile = new FileInfo(tmp);
                                        //sourceFile.CopyTo(res, true);
                                        using (var inputFile = new FileStream(
                                                tmp,
                                                FileMode.Open,
                                                FileAccess.Read,
                                                FileShare.ReadWrite))
                                            using (var outputFile = new FileStream(res, FileMode.Create))
                                            {
                                                var buffer = new byte[0x10000];
                                                int bytes;

                                                while ((bytes = inputFile.Read(buffer, 0, buffer.Length)) > 0)
                                                {
                                                    outputFile.Write(buffer, 0, bytes);
                                                }
                                            }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                });
                            }
                        }
                    }
                }
        }

        int GetNumberInOrder(IEnumerable lst)
        {
            int maxNum = 0;

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

        private async Task _ImportForm()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                OpenFileDialog dial = new OpenFileDialog();
                dial.AllowMultiple = true;
                var filter = new FileDialogFilter
                {
                    Name = "RAODB",
                    Extensions = {
                        "raodb"
                    }
                };
                dial.Filters = new List<FileDialogFilter>() { filter };
                var answ = await dial.ShowAsync(desktop.MainWindow);
                if (answ != null)
                {
                    foreach (var res in answ)
                    {
                        if (res != null)
                        {
                            if (res != "")
                            {
                                string system = "";
                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    system = Environment.GetFolderPath(Environment.SpecialFolder.System);
                                }
                                else
                                {
                                    system = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                }
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
                                    var sourceFile = new FileInfo(res);
                                    sourceFile.CopyTo(tmp, true);
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
                                            if (item != null)
                                            {
                                                if (item.Master_DB.FormNum_DB == "1.0")
                                                {
                                                    if (item.Master_DB.Rows10[0].Id > item.Master_DB.Rows10[1].Id)
                                                    {
                                                        item.Master_DB.Rows10[0].NumberInOrder_DB = 2;
                                                        item.Master_DB.Rows10[1].NumberInOrder_DB = 1;
                                                        item.Master_DB.Rows10.QuickSort();
                                                    }
                                                    else
                                                    {
                                                        item.Master_DB.Rows10[0].NumberInOrder_DB = 1;
                                                        item.Master_DB.Rows10[1].NumberInOrder_DB = 2;
                                                        item.Master_DB.Rows10.QuickSort();
                                                    }
                                                }
                                                if (item.Master_DB.FormNum_DB == "2.0")
                                                {
                                                    if (item.Master_DB.Rows20[0].Id > item.Master_DB.Rows20[1].Id)
                                                    {
                                                        item.Master_DB.Rows20[0].NumberInOrder_DB = 2;
                                                        item.Master_DB.Rows20[1].NumberInOrder_DB = 1;
                                                        item.Master_DB.Rows20.QuickSort();
                                                    }
                                                    else
                                                    {
                                                        item.Master_DB.Rows20[0].NumberInOrder_DB = 1;
                                                        item.Master_DB.Rows20[1].NumberInOrder_DB = 2;
                                                        item.Master_DB.Rows20.QuickSort();
                                                    }
                                                }
                                                item.CleanIds();
                                            }
                                            if (first11 != null)
                                            {
                                                foreach (var it in item.Report_Collection)
                                                {
                                                    foreach (var note in it.Notes)
                                                    {
                                                        if (note.Order == 0)
                                                        {
                                                            note.Order = GetNumberInOrder(it.Notes);
                                                        }
                                                    }
                                                    var lst = first11.Report_Collection.ToList();
                                                    if (lst.Count != 0)
                                                    {
                                                        foreach (var elem in lst)
                                                        {
                                                            try
                                                            {
                                                                var st_elem = DateTime.Parse(elem.StartPeriod.Value);
                                                                var en_elem = DateTime.Parse(elem.EndPeriod.Value);
                                                                if (st_elem > en_elem)
                                                                {
                                                                    var _e = st_elem;
                                                                    st_elem = en_elem;
                                                                    en_elem = _e;
                                                                }

                                                                DateTimeOffset st_it = DateTimeOffset.Now;
                                                                DateTimeOffset en_it = DateTimeOffset.Now;
                                                                try
                                                                {
                                                                    st_it = DateTime.Parse(it.StartPeriod.Value);
                                                                    en_it = DateTime.Parse(it.EndPeriod.Value);
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    first11.Report_Collection.Add(it);
                                                                    throw e;
                                                                }
                                                                if (st_it > en_it)
                                                                {
                                                                    var _e = st_it;
                                                                    st_it = en_it;
                                                                    en_it = _e;
                                                                }
                                                                if (st_elem == st_it && en_elem == en_it)
                                                                {
                                                                    var str = "Совпадение даты в " + elem.FormNum.Value + " " +
                                                                        elem.StartPeriod.Value + "-" +
                                                                        elem.EndPeriod.Value + " \n" +
                                                                        first11.Master.RegNoRep.Value + " " +
                                                                        first11.Master.ShortJurLicoRep.Value + " " +
                                                                        first11.Master.OkpoRep.Value;
                                                                    var an = await ShowMessageT.Handle(new List<string>()
                                                            {
                                                                str,
                                                                "Заменить",
                                                                "Сохранить оба",
                                                                "Отменить" });
                                                                    if (an == "Сохранить оба")
                                                                    {
                                                                        first11.Report_Collection.Add(it);
                                                                    }
                                                                    if (an == "Заменить")
                                                                    {
                                                                        first11.Report_Collection.Remove(elem);
                                                                        first11.Report_Collection.Add(it);
                                                                    }
                                                                }
                                                                if (st_elem < st_it && st_it < en_elem || st_elem < en_it && en_it < en_elem)
                                                                {
                                                                    var str = "Пересечение даты в " + elem.FormNum.Value + " " +
                                                                        elem.StartPeriod.Value + "-" +
                                                                        elem.EndPeriod.Value + " \n" +
                                                                        first11.Master.RegNoRep.Value + " " +
                                                                        first11.Master.ShortJurLicoRep.Value + " " +
                                                                        first11.Master.OkpoRep.Value;
                                                                    var an = await ShowMessageT.Handle(new List<string>()
                                                            {
                                                                str,
                                                                "Сохранить оба",
                                                                "Отменить" });
                                                                    if (an == "Сохранить оба")
                                                                    {
                                                                        first11.Report_Collection.Add(it);
                                                                    }
                                                                }
                                                                first11.Sort();
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                    else 
                                                    {
                                                        first11.Report_Collection.Add(it);
                                                        first11.Sort();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (first21 != null)
                                                {
                                                    foreach (var it in item.Report_Collection)
                                                    {
                                                        foreach (var note in it.Notes)
                                                        {
                                                            if (note.Order == 0)
                                                            {
                                                                note.Order = GetNumberInOrder(it.Notes);
                                                            }
                                                        }

                                                        var lst = first21.Report_Collection.ToList();
                                                        if (lst.Count != 0)
                                                        {
                                                            foreach (var elem in lst)
                                                            {
                                                                if (elem.Year == it.Year)
                                                                {
                                                                    var str = "Совпадение даты в " + elem.FormNum.Value + " " +
                                                                        elem.Year.Value + " " +
                                                                        first21.Master.RegNoRep1.Value + " \n" +
                                                                        first21.Master.ShortJurLicoRep1.Value + " " +
                                                                        first21.Master.OkpoRep1.Value;
                                                                    var an = await ShowMessageT.Handle(new List<string>()
                                                            {
                                                                str,
                                                                "Заменить",
                                                                "Сохранить оба",
                                                                "Отменить" });
                                                                    if (an == "Сохранить оба")
                                                                    {
                                                                        first21.Report_Collection.Add(it);
                                                                    }
                                                                    if (an == "Заменить")
                                                                    {
                                                                        first21.Report_Collection.Remove(elem);
                                                                        first21.Report_Collection.Add(it);
                                                                    }
                                                                }
                                                                first21.Sort();
                                                            }
                                                        }
                                                        else 
                                                        {
                                                            first21.Report_Collection.Add(it);
                                                            first21.Sort();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (var form in item.Report_Collection)
                                                    {
                                                        foreach (var note in form.Notes)
                                                        {
                                                            if (note.Order == 0)
                                                            {
                                                                note.Order = GetNumberInOrder(form.Notes);
                                                            }
                                                        }
                                                    }
                                                    Local_Reports.Reports_Collection.Add(item);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }
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

                        var tre = (from i in Local_Reports.Reports_Collection where i.Report_Collection.Contains(rep) select i).FirstOrDefault();

                        ChangeOrCreateVM frm = new(rep.FormNum.Value, rep,tre);
                        await ShowDialog.Handle(frm);

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

                        ChangeOrCreateVM frm = new(rep.Master.FormNum.Value, rep.Master,rep);
                        await ShowDialog.Handle(frm);

                        t.SelectedReports = tmp;
                    }
                }
        }

        private async Task _DeleteForm(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = (string)await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить отчет?", "Да", "Нет" });
                if (answ == "Да")
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
        }

        private async Task _DeleteReport(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = (string)await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить организацию?", "Да", "Нет" });
                if (answ == "Да")
                {
                    if (param != null)
                        foreach (var item in param)
                            Local_Reports.Reports_Collection.Remove((Reports)item);
                    Local_Reports.Reports_Collection.QuickSort();
                    await StaticConfiguration.DBModel.SaveChangesAsync();
                }
            }
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

            Report master = reps.Master_DB;

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
        private void _Excel_Print_SubMain_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = from t in Local_Reports.Reports_Collection
                              where t.Report_Collection.Contains(form)
                              select t;
            var reps = findReports.FirstOrDefault();
            Report master = reps.Master_DB;

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
        private void _Excel_Print_Notes_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            int Start = 15;
            if(param=="2.8")
            {
                Start = 18;
            }

            for(int i =0;i<form.Notes.Count-1;i++)
            {
                worksheet.InsertRow(Start+1, 1, Start);
                var cells=worksheet.Cells["A" + (Start + 1) + ":B" + (Start + 1)];
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
                var cellCL = worksheet.Cells["C" + (Start + 1) + ":L" + (Start + 1)];
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

            int Count = Start;
            foreach (var note in form.Notes)
            {
                note.ExcelRow(worksheet,Count,1);
                Count++;
            }
        }
        private void _Excel_Print_Rows_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            int Start = 11;
            if (param == "2.8")
            {
                Start = 14;
            }

            for (int i = 0; i < form[param].Count - 1; i++)
            {
                worksheet.InsertRow(Start + 1, 1, Start);
                var cells = worksheet.Cells["A" + (Start + 1) + ":B" + (Start + 1)];
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

            int Count = Start;
            foreach (var it in form[param])
            {
                if (it is Form11)
                {
                    ((Form11)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form12)
                {
                    ((Form12)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form13)
                {
                    ((Form13)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form14)
                {
                    ((Form14)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form15)
                {
                    ((Form15)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form16)
                {
                    ((Form16)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form17)
                {
                    ((Form17)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form18)
                {
                    ((Form18)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form19)
                {
                    ((Form19)(it)).ExcelRow(worksheet, Count, 1);
                }

                if (it is Form21)
                {
                    ((Form21)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form22)
                {
                    ((Form22)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form23)
                {
                    ((Form23)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form24)
                {
                    ((Form24)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form25)
                {
                    ((Form25)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form26)
                {
                    ((Form26)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form27)
                {
                    ((Form27)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form28)
                {
                    ((Form28)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form29)
                {
                    ((Form29)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form210)
                {
                    ((Form210)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form211)
                {
                    ((Form211)(it)).ExcelRow(worksheet, Count, 1);
                }
                if (it is Form212)
                {
                    ((Form212)(it)).ExcelRow(worksheet, Count, 1);
                }
                Count++;
            }
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
                                        var form = (Report)forms.FirstOrDefault();
                                        ExcelWorksheet worksheetTitul =
                                            excelPackage.Workbook.Worksheets[param.Split('.')[0]+".0"];
                                        ExcelWorksheet worksheetMain =
                                            excelPackage.Workbook.Worksheets[param];
                                        _Excel_Print_Titul_Export(param, worksheetTitul,form);
                                        _Excel_Print_SubMain_Export(param, worksheetMain, form);
                                        _Excel_Print_Notes_Export(param, worksheetMain, form);
                                        _Excel_Print_Rows_Export(param, worksheetMain, form);
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
                int k = 10;
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
                int l = 10;
            }
        }

        private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Local_Reports");
        }
    }
}