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
using Client_App.Long_Visual;


namespace Client_App.ViewModels
{
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

        public MainWindowVM() { }

        #region Init
        private async Task<string> ProcessRaoDirectory(string systemDirectory)
        {
            var tmp = "";
            var pty = "";
            try
            {
                string path = Path.GetPathRoot(systemDirectory);
                tmp = Path.Combine(path, "RAO");
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
            try
            {
                var fl = Directory.GetFiles(tmp);
                foreach (var file in fl)
                {
                    File.Delete(file);
                }
                return pty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await ShowMessage.Handle(ErrorMessages.Error3);
                throw new Exception(ErrorMessages.Error3[0]);
            }
        }
        private async Task<string> GetSystemDirectory()
        {
            try
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
                return system;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await ShowMessage.Handle(ErrorMessages.Error1);
                throw new Exception(ErrorMessages.Error1[0]);
            }
        }
        private async Task ProcessSpravochniks()
        {
            var a = Spravochniks.SprRadionuclids;
            var b = Spravochniks.SprTypesToRadionuclids;
        }
        private async Task ProcessDataBaseCreate(string tempDirectory)
        {
            var i = 0;
            bool flag = false;
            DBModel dbm = null;
            foreach (var file in Directory.GetFiles(tempDirectory))
            {
                try
                {
                    StaticConfiguration.DBPath = file;
                    StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                    dbm = StaticConfiguration.DBModel;
                    await dbm.Database.MigrateAsync();
                    flag = true;
                    break;
                }
                catch (Exception e)
                {
                }
            }
            if (!flag)
            {
                StaticConfiguration.DBPath = Path.Combine(tempDirectory, "Local" + "_" + i + ".raodb");
                StaticConfiguration.DBModel = new DBModel(StaticConfiguration.DBPath);
                dbm = StaticConfiguration.DBModel;
                await dbm.Database.MigrateAsync();
            }
        }
        private async Task ProcessDataBaseFillEmpty(DataContext dbm)
        {
            if (dbm.DBObservableDbSet.Count() == 0) dbm.DBObservableDbSet.Add(new DBObservable());
            foreach (var item in dbm.DBObservableDbSet)
            {
                foreach (Reports it in item.Reports_Collection)
                {
                    if (it.Master_DB.FormNum_DB != "")
                    {
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
        }
        private async Task ProcessDataBaseFillNullOrder()
        {
            foreach (Reports item in Local_Reports.Reports_Collection)
            {
                foreach (Report it in item.Report_Collection)
                {
                    foreach (Note _i in it.Notes)
                    {
                        if (_i.Order == 0)
                        {
                            _i.Order = GetNumberInOrder(it.Notes);
                        }
                    }
                }
                await item.SortAsync();
            }
            await Local_Reports.Reports_Collection.QuickSortAsync();
        }
        private async Task PropertiesInit()
        {
            AddReport = ReactiveCommand.CreateFromTask<object>(_AddReport);
            AddForm = ReactiveCommand.CreateFromTask<object>(_AddForm);
            ImportForm = ReactiveCommand.CreateFromTask(_ImportForm);
            ExportForm = ReactiveCommand.CreateFromTask<object>(_ExportForm);
            ChangeForm = ReactiveCommand.CreateFromTask<object>(_ChangeForm);
            ChangeReport = ReactiveCommand.CreateFromTask<object>(_ChangeReport);
            DeleteForm = ReactiveCommand.CreateFromTask<object>(_DeleteForm);
            DeleteReport = ReactiveCommand.CreateFromTask<object>(_DeleteReport);
            Print_Excel_Export = ReactiveCommand.CreateFromTask<object>(_Print_Excel_Export);
            Excel_Export = ReactiveCommand.CreateFromTask<object>(_Excel_Export);
            All_Excel_Export = ReactiveCommand.CreateFromTask<object>(_All_Excel_Export);
            ShowDialog = new Interaction<ChangeOrCreateVM, object>();
            ShowMessage = new Interaction<List<string>, string>();
        }
        private int GetNumberInOrder(IEnumerable lst)
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
        public async Task Init()
        {
            OnStartProgressBar = 0;
            var systemDirectory = await GetSystemDirectory();

            OnStartProgressBar = 5;
            var raoDirectory = await ProcessRaoDirectory(systemDirectory);

            OnStartProgressBar = 10;
            await ProcessSpravochniks();

            OnStartProgressBar = 15;
            await ProcessDataBaseCreate(raoDirectory);

            OnStartProgressBar = 25;
            var dbm = StaticConfiguration.DBModel;
            await dbm.LoadTablesAsync();

            OnStartProgressBar = 55;
            await ProcessDataBaseFillEmpty(dbm);

            OnStartProgressBar = 70;
            Local_Reports = dbm.DBObservableDbSet.Local.First();
            await ProcessDataBaseFillNullOrder();

            OnStartProgressBar = 75;
            dbm.SaveChanges();
            Local_Reports.PropertyChanged += Local_ReportsChanged;

            OnStartProgressBar = 80;
            await PropertiesInit();

            OnStartProgressBar = 100;
        }
        private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Local_Reports");
        }
        #endregion

        #region OnStartProgressBar
        private double _OnStartProgressBar = 0;
        public double OnStartProgressBar
        {
            get => _OnStartProgressBar;
            set
            {
                if (_OnStartProgressBar != value)
                {
                    _OnStartProgressBar = value;
                    OnPropertyChanged(nameof(OnStartProgressBar));
                }
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
            var param = par as string;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
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
            var param = par as string;
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
                            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                            ChangeOrCreateVM frm = new(param, y);
                            if ((string)param == "2.1")
                            {
                                Form2_Visual.tmpVM = frm;
                            }
                            if ((string)param == "2.2")
                            {
                                Form2_Visual.tmpVM = frm;
                            }
                            await ShowDialog.Handle(frm);

                            t.SelectedReports = tmp;
                            await y.Report_Collection.QuickSortAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region ImportForm
        public ReactiveCommand<Unit, Unit> ImportForm { get; private set; }
        private async Task<string[]> GetSelectedFilesFromDialog(string Name, params string[] Extensions)
        {
            string[]? answ = null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                OpenFileDialog dial = new OpenFileDialog();
                dial.AllowMultiple = true;
                var filter = new FileDialogFilter
                {
                    Name = Name,
                    Extensions = new List<string>(Extensions)
                };
                dial.Filters = new List<FileDialogFilter>() { filter };

                answ = await dial.ShowAsync(desktop.MainWindow);
            }
            return answ;
        }
        private async Task<string> GetTempDirectory(string systemDirectory)
        {
            var tmp = "";
            var pty = "";
            try
            {
                string path = Path.GetPathRoot(systemDirectory);
                tmp = Path.Combine(path, "RAO");
                pty = tmp;
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

            var file = "";
            var count = 0;
            do
            {
                file = Path.Combine(tmp, "file_imp_" + (count++) + ".raodb");
            }
            while (File.Exists(file));

            return file;
        }
        private async Task<List<Reports>> GetReportsFromDataBase(string file)
        {
            var lst = new List<Reports>();
            using (DBModel db = new DBModel(file))
            {
                await db.Database.MigrateAsync();
                await db.LoadTablesAsync();
                lst = await db.ReportsCollectionDbSet.ToListAsync();
            }
            return lst;
        }
        private async Task<Reports> GetReports11FromLocalEqual(Reports item)
        {
            try
            {
                var tb11 = from Reports t in Local_Reports.Reports_Collection10
                           where (((item.Master.Rows10[0].Okpo_DB == "") &&
                           (t.Master.Rows10[0].Okpo_DB == "")) ||
                           ((t.Master.Rows10[0].Okpo_DB == item.Master.Rows10[0].Okpo_DB) &&
                           (t.Master.Rows10[1].Okpo_DB == item.Master.Rows10[1].Okpo_DB))) &&
                           (((item.Master.Rows10[0].RegNo_DB == "") &&
                           (t.Master.Rows10[0].RegNo_DB == "")) ||
                           ((t.Master.Rows10[0].RegNo_DB == item.Master.Rows10[0].RegNo_DB) &&
                           (t.Master.Rows10[1].RegNo_DB == item.Master.Rows10[1].RegNo_DB))) &&
                           (((item.Master.Rows10[0].ShortJurLico_DB == "") &&
                           (t.Master.Rows10[0].ShortJurLico_DB == "")) ||
                           ((t.Master.Rows10[0].ShortJurLico_DB == item.Master.Rows10[0].ShortJurLico_DB) &&
                           (t.Master.Rows10[1].ShortJurLico_DB == item.Master.Rows10[1].ShortJurLico_DB)))
                           select t;
                return tb11.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
        private async Task<Reports> GetReports21FromLocalEqual(Reports item)
        {
            try
            {
                var tb2 = item.Report_Collection.Where(x => x.FormNum_DB[0].ToString().Equals("2"));
                if (tb2.Count() != 0)
                {
                    var tb21 = from Reports t in Local_Reports.Reports_Collection20
                               where (((item.Master.Rows10[0].Okpo_DB == "") &&
                               (t.Master.Rows20[0].Okpo_DB == "")) ||
                               ((t.Master.Rows20[0].Okpo_DB == item.Master.Rows20[0].Okpo_DB) &&
                               (t.Master.Rows20[1].Okpo_DB == item.Master.Rows20[1].Okpo_DB))) &&
                               (((item.Master.Rows20[0].RegNo_DB == "") &&
                               (t.Master.Rows20[0].RegNo_DB == "")) ||
                               ((t.Master.Rows20[0].RegNo_DB == item.Master.Rows20[0].RegNo_DB) &&
                               (t.Master.Rows20[1].RegNo_DB == item.Master.Rows20[1].RegNo_DB))) &&
                               (((item.Master.Rows20[0].ShortJurLico_DB == "") &&
                               (t.Master.Rows20[0].ShortJurLico_DB == "")) ||
                               ((t.Master.Rows20[0].ShortJurLico_DB == item.Master.Rows20[0].ShortJurLico_DB) &&
                               (t.Master.Rows20[1].ShortJurLico_DB == item.Master.Rows20[1].ShortJurLico_DB)))
                               select t;
                    return tb21.FirstOrDefault();
                }
                return null;
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
                        if (item.Master_DB.Rows10[1].NumberInOrder_DB == 2)
                        {
                            item.Master_DB.Rows10[1].NumberInOrder_DB = 1;
                        }
                        else
                        {
                            item.Master_DB.Rows10[1].NumberInOrder_DB = 2;
                        }
                    }
                    item.Master_DB.Rows10.Sorted = false;
                    item.Master_DB.Rows10.QuickSort();
                }
                else
                {
                    if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                    {
                        item.Master_DB.Rows10[0].NumberInOrder_DB = 1;
                    }
                    if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                    {
                        if (item.Master_DB.Rows10[1].NumberInOrder_DB == 2)
                        {
                            item.Master_DB.Rows10[1].NumberInOrder_DB = 1;
                        }
                        else
                        {
                            item.Master_DB.Rows10[1].NumberInOrder_DB = 2;
                        }
                    }
                    item.Master_DB.Rows10.Sorted = false;
                    item.Master_DB.Rows10.QuickSort();
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
                        if (item.Master_DB.Rows20[1].NumberInOrder_DB == 2)
                        {
                            item.Master_DB.Rows20[1].NumberInOrder_DB = 1;
                        }
                        else
                        {
                            item.Master_DB.Rows20[1].NumberInOrder_DB = 2;
                        }
                    }
                    item.Master_DB.Rows20.Sorted = false;
                    item.Master_DB.Rows20.QuickSort();
                }
                else
                {
                    if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                    {
                        item.Master_DB.Rows20[0].NumberInOrder_DB = 1;
                    }
                    if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                    {
                        if (item.Master_DB.Rows20[1].NumberInOrder_DB == 2)
                        {
                            item.Master_DB.Rows20[1].NumberInOrder_DB = 1;
                        }
                        else
                        {
                            item.Master_DB.Rows20[1].NumberInOrder_DB = 2;
                        }
                    }
                    item.Master_DB.Rows20.Sorted = false;
                    item.Master_DB.Rows20.QuickSort();
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
        private async Task ProcessIfHasReports11(Reports first11, Reports item)
        {
            var not_in = false;
            foreach (Report it in item.Report_Collection)
            {
                if (first11.Report_Collection.Count != 0)
                {
                    foreach (Report elem in first11.Report_Collection)
                    {
                        DateTimeOffset st_elem = DateTimeOffset.Now;
                        DateTimeOffset en_elem = DateTimeOffset.Now;
                        try
                        {
                            st_elem = DateTime.Parse(elem.StartPeriod_DB) > DateTime.Parse(elem.EndPeriod_DB) ? DateTime.Parse(elem.EndPeriod_DB) : DateTime.Parse(elem.StartPeriod_DB);
                            en_elem = DateTime.Parse(elem.StartPeriod_DB) < DateTime.Parse(elem.EndPeriod_DB) ? DateTime.Parse(elem.EndPeriod_DB) : DateTime.Parse(elem.StartPeriod_DB);
                        }
                        catch (Exception ex)
                        { }

                        DateTimeOffset st_it = DateTimeOffset.Now;
                        DateTimeOffset en_it = DateTimeOffset.Now;
                        try
                        {
                            st_it = DateTime.Parse(it.StartPeriod_DB) > DateTime.Parse(it.EndPeriod_DB) ? DateTime.Parse(it.EndPeriod_DB) : DateTime.Parse(it.StartPeriod_DB);
                            en_it = DateTime.Parse(it.StartPeriod_DB) < DateTime.Parse(it.EndPeriod_DB) ? DateTime.Parse(it.EndPeriod_DB) : DateTime.Parse(it.StartPeriod_DB);
                        }
                        catch (Exception ex)
                        {
                        }

                        if (st_elem == st_it && en_elem == en_it && it.FormNum_DB == elem.FormNum_DB)
                        {
                            not_in = true;
                            if (it.CorrectionNumber_DB < elem.CorrectionNumber_DB)
                            {
                                var str = " Вы пытаетесь загрузить форму с наименьщим номером корректировки - " +
                                    it.CorrectionNumber_DB + ",\n" +
                                    "при текущем значении корректировки - " +
                                    elem.CorrectionNumber_DB + ".\n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Начало отчетного периода - " + it.StartPeriod_DB + "\n" +
                                    "Конец отчетного периода - " + it.EndPeriod_DB + "\n" +
                                    "Регистрационный номер - " + first11.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first11.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first11.Master.OkpoRep.Value;
                                var an = await ShowMessage.Handle(new List<string>(){str,"OK"});
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB)
                            {
                                var str = "Совпадение даты в " + elem.FormNum_DB + " " +
                                    elem.StartPeriod_DB + "-" +
                                    elem.EndPeriod_DB + " .\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    first11.Master.RegNoRep.Value + " " +
                                    first11.Master.ShortJurLicoRep.Value + " " +
                                    first11.Master.OkpoRep.Value;
                                var an = await ShowMessage.Handle(new List<string>(){str,
                                    "Заменить",
                                    "Дополнить",
                                    "Сохранить оба",
                                    "Отменить" 
                                });
                                await ChechAanswer(an, first11, elem, it);
                            }
                            else
                            {
                                var str = "Загрузить новую форму? \n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Начало отчетного периода - " + it.StartPeriod_DB + "\n" +
                                    "Конец отчетного периода - " + it.EndPeriod_DB + "\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    "Регистрационный номер - " + first11.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first11.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first11.Master.OkpoRep.Value + "\n" +
                                    "Форма с предыдущим номером корректировки №" +
                                    elem.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                    "Сделайте резервную копию.";
                                var an = await ShowMessage.Handle(new List<string>() {str,
                                    "Загрузить новую",
                                    "Отмена"
                                });
                                await ChechAanswer(an, first11, elem, it);
                            }
                        }
                        if ((st_elem > st_it && st_elem < en_it || en_elem > st_it && en_elem < en_it) && it.FormNum.Value == elem.FormNum.Value)
                        {
                            not_in = true;
                            var str = "Пересечение даты в " + elem.FormNum_DB + " " +
                                elem.StartPeriod_DB + "-" +
                                elem.EndPeriod_DB + " \n" +
                                first11.Master.RegNoRep.Value + " " +
                                first11.Master.ShortJurLicoRep.Value + " " +
                                first11.Master.OkpoRep.Value;
                            var an = await ShowMessage.Handle(new List<string>(){str,
                                "Сохранить оба",
                                "Отменить" 
                            });
                            await ChechAanswer(an, first11, null, it);
                        }
                    }
                    if (!not_in)
                    {
                        var str = "Загрузить новую форму?\n" +
                            "Номер формы - " + it.FormNum_DB + "\n" +
                            "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                            "Начало отчетного периода - " + it.StartPeriod_DB + "\n" +
                            "Конец отчетного периода - " + it.EndPeriod_DB + "\n" +
                            "Регистрационный номер - " + first11.Master.RegNoRep.Value + "\n" +
                            "Сокращенное наименование - " + first11.Master.ShortJurLicoRep.Value + "\n" +
                            "ОКПО - " + first11.Master.OkpoRep.Value + "\n";
                        var an = await ShowMessage.Handle(new List<string>(){str,
                            "Да",
                            "Нет"
                        });
                        await ChechAanswer(an, first11, null, it);
                    }
                }
                else
                {
                    first11.Report_Collection.Add(it);
                }
                await first11.SortAsync();
            }
        }
        private async Task ProcessIfHasReports21(Reports first21, Reports item)
        {
            var not_in = false;
            foreach (Report it in item.Report_Collection)
            {
                if (first21.Report_Collection.Count != 0)
                {
                    foreach (Report elem in first21.Report_Collection)
                    {
                        if (elem.Year_DB == it.Year_DB && it.FormNum_DB == elem.FormNum_DB)
                        {
                            not_in = true;
                            if (it.CorrectionNumber_DB < elem.CorrectionNumber_DB)
                            {
                                var str = " Вы пытаетесь загрузить форму с наименьщим номером корректировки - " +
                                    it.CorrectionNumber_DB + ",\n" +
                                    "при текущем значении корректировки - " +
                                    elem.CorrectionNumber_DB + ".\n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Отчетный год - " + it.Year_DB + "\n" +
                                    "Регистрационный номер - " + first21.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first21.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first21.Master.OkpoRep.Value;
                                var an = await ShowMessage.Handle(new List<string>(){str,"OK"});
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB)
                            {
                                var str = "Совпадение даты в " + elem.FormNum_DB + " " +
                                elem.Year_DB + " .\n" +
                                "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                first21.Master.RegNoRep.Value + " \n" +
                                first21.Master.ShortJurLicoRep.Value + " " +
                                first21.Master.OkpoRep.Value;
                                var an = await ShowMessage.Handle(new List<string>(){str,
                                    "Заменить",
                                    "Сохранить оба",
                                    "Отменить" 
                                });
                                await ChechAanswer(an, first21, elem, it);
                            }
                            else
                            {
                                var str = "Загрузить новую форму? \n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Отчетный год - " + it.Year_DB + "\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    "Регистрационный номер - " + first21.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first21.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first21.Master.OkpoRep.Value + "\n" +
                                    "Форма с предыдущим номером корректировки №" +
                                    elem.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                    "Сделайте резервную копию.";
                                var an = await ShowMessage.Handle(new List<string>() {
                                                                        str,
                                                                        "Загрузить новую",
                                                                        "Отмена"
                                                                        });
                                await ChechAanswer(an, first21, elem, it);
                            }
                        }
                    }
                    if (!not_in)
                    {
                        var str = "Загрузить новую форму? \n" +
                            "Номер формы - " + it.FormNum_DB + "\n" +
                            "Отчетный год - " + it.Year_DB + "\n" +
                            "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                            "Регистрационный номер - " + first21.Master.RegNoRep.Value + "\n" +
                            "Сокращенное наименование - " + first21.Master.ShortJurLicoRep.Value + "\n" +
                            "ОКПО - " + first21.Master.OkpoRep.Value + "\n";
                        var an = await ShowMessage.Handle(new List<string>(){str,
                            "Да",
                            "Нет"
                        });
                        await ChechAanswer(an, first21, null, it);
                        not_in = false;
                    }
                }
                else
                {
                    first21.Report_Collection.Add(it);
                }
                await first21.SortAsync();
            }
        }
        private async Task ChechAanswer(string an, Reports first, Report elem = null, Report it = null)
        {
            if (an == "Сохранить оба" || an == "Да")
            {
                first.Report_Collection.Add(it);
            }
            if (an == "Заменить" || an == "Загрузить новую")
            {
                first.Report_Collection.Remove(elem);
                first.Report_Collection.Add(it);
            }
            if (an == "Дополнить")
            {
                first.Report_Collection.Remove(elem);
                it.Rows.AddRange<IKey>(0, elem.Rows.GetEnumerable());
                it.Notes.AddRange<IKey>(0, elem.Notes);
                first.Report_Collection.Add(it);
            }
        }
        private async Task _ImportForm()
        {
            var answ = await GetSelectedFilesFromDialog("RAODB", "raodb");
            if (answ != null)
            {
                foreach (var res in answ)
                {
                    if (res != "")
                    {
                        var file = await GetRaoFileName();
                        var sourceFile = new FileInfo(res);
                        sourceFile.CopyTo(file, true);

                        var reportsCollection = await GetReportsFromDataBase(file);
                        foreach (var item in reportsCollection)
                        {
                            Reports first11 = await GetReports11FromLocalEqual(item);
                            Reports first21 = await GetReports21FromLocalEqual(item);
                            await RestoreReportsOrders(item);
                            item.CleanIds();

                            await ProcessIfNoteOrder0(item);

                            if (first11 != null)
                            {
                                await ProcessIfHasReports11(first11, item);
                            }
                            if (first21 != null)
                            {
                                await ProcessIfHasReports21(first21, item);
                            }
                            if (first21 == null && first11 == null)
                            {
                                var rep = item.Report_Collection.FirstOrDefault();
                                if (rep != null)
                                {
                                    var str = "Был добавлен отчет по форме " + rep.FormNum_DB + " за период " + rep.StartPeriod_DB + "-" + rep.EndPeriod_DB + ",\n" +
                                        "номер корректировки " + rep.CorrectionNumber_DB + ", количество строк " + rep.Rows.Count + ".\n" +
                                        "Организации:" + "\n" +
                                        "   1.Регистрационный номер  " + item.Master.RegNoRep.Value + "\n" +
                                        "   2.Сокращенное наименование  " + item.Master.ShortJurLicoRep.Value + "\n" +
                                        "   3.ОКПО  " + item.Master.OkpoRep.Value + "\n"; ;
                                    var an = await ShowMessage.Handle(new List<string>() { str, "Ок" });
                                }
                                Local_Reports.Reports_Collection.Add(item);
                                await Local_Reports.Reports_Collection.QuickSortAsync();
                            }
                        }
                    }
                }
            }
            StaticConfiguration.DBModel.SaveChanges();
        }
        #endregion

        #region ExportForm
        public ReactiveCommand<object, Unit> ExportForm { get; private set; }
        private async Task _ExportForm(object par)
        {
            var param = par as ObservableCollectionWithItemPropertyChanged<IKey>;
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
                            var aDay = a.Day.ToString();
                            var aMonth = a.Month.ToString();
                            if (aDay.Length < 2) aDay = "0" + aDay;
                            if (aMonth.Length < 2) aMonth = "0" + aMonth;
                            ((Report)item).ExportDate.Value = aDay + "." + aMonth + "." + a.Year;
                        }
                        if (res != "")
                        {
                            var dt = DateTime.Now;
                            var filename = "Report_" +
                                           dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + "_" + dt.Minute +
                                           "_" + dt.Second;
                            var rep = (Report)obj;

                            var dtDay = dt.Day.ToString();
                            var dtMonth = dt.Month.ToString();
                            if (dtDay.Length < 2) dtDay = "0" + dtDay;
                            if (dtMonth.Length < 2) dtMonth = "0" + dtMonth;

                            rep.ExportDate.Value = dtDay + "." + dtMonth + "." + dt.Year;
                            var findReports = from Reports t in Local_Reports.Reports_Collection
                                              where t.Report_Collection.Contains(rep)
                                              select t;

                            await StaticConfiguration.DBModel.SaveChangesAsync();

                            var rt = findReports.FirstOrDefault();
                            if (rt != null)
                            {
                                var tmp = Path.Combine(await GetTempDirectory(await GetSystemDirectory()), filename + "_exp" + ".raodb");

                                var tsk = new Task(() =>
                                {
                                    DBModel db = new DBModel(tmp);
                                    try
                                    {
                                        Reports rp = new Reports();
                                        rp.Master = rt.Master;
                                        rp.Report_Collection.Add(rep);
                                        db.Database.MigrateAsync();
                                        db.ReportsCollectionDbSet.Add(rp);
                                        db.SaveChangesAsync(); 

                                        string filename2 = "";
                                        if (rp.Master_DB.FormNum_DB == "1.0")
                                        {
                                            filename2 += rp.Master.RegNoRep.Value;
                                            filename2 += "_" + rp.Master.OkpoRep.Value;

                                            filename2 += "_" + rep.FormNum_DB;
                                            filename2 += "_" + rep.StartPeriod_DB;
                                            filename2 += "_" + rep.EndPeriod_DB;
                                            filename2 += "_" + rep.CorrectionNumber_DB;
                                        }
                                        else
                                        {
                                            if (rp.Master.Rows20.Count > 0)
                                            {
                                                filename2 += rp.Master.RegNoRep.Value;
                                                filename2 += rp.Master.OkpoRep.Value;

                                                filename2 += "_" + rep.FormNum_DB;
                                                filename2 += "_" + rep.Year_DB;
                                                filename2 += "_" + rep.CorrectionNumber_DB;
                                            }
                                        }

                                        res = Path.Combine(res, filename2 + ".raodb");


                                        var t = db.Database.GetDbConnection() as FbConnection;
                                        t.CloseAsync();
                                        t.DisposeAsync();

                                        db.Database.CloseConnectionAsync();
                                        db.DisposeAsync();

                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                        throw;
                                    }
                                });
                                tsk.Start();
                                await tsk.ContinueWith((a) =>
                                {
                                    try
                                    {
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
        #endregion

        #region ChangeForm
        public ReactiveCommand<object, Unit> ChangeForm { get; private set; }
        private async Task _ChangeForm(object par)
        {
            var param = par as ObservableCollectionWithItemPropertyChanged<IKey>;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    if (obj != null)
                    {
                        var t = desktop.MainWindow as MainWindow;
                        var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
                        var rep = (Report)obj;
                        var tre = (from Reports i in Local_Reports.Reports_Collection where i.Report_Collection.Contains(rep) select i).FirstOrDefault();
                        string numForm = rep.FormNum.Value;
                        ChangeOrCreateVM frm = new(numForm, rep, tre);
                        if (numForm == "2.1")
                        {
                            Form2_Visual.tmpVM = frm;
                            if (frm.isSum)
                            {
                                //var sumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                                await frm.UnSum21();
                                await frm.Sum21();
                                //var newSumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                            }
                        }
                        if (numForm == "2.2")
                        {
                            Form2_Visual.tmpVM = frm;
                            if (frm.isSum)
                            {
                                var sumRow = frm.Storage.Rows22.Where(x => x.Sum_DB == true).ToList();
                                Dictionary<long, List<string>> dic = new Dictionary<long, List<string>> ();
                                foreach (var oldR in sumRow)
                                {
                                    dic[oldR.NumberInOrder_DB] = new List<String>() { oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB };
                                }

                                await frm.UnSum22();
                                await frm.Sum22();
                                var newSumRow = frm.Storage.Rows22.Where(x => x.Sum_DB == true);
                                foreach (var newR in newSumRow)
                                {
                                    foreach (var oldR in dic)
                                    {
                                        if (newR.NumberInOrder_DB == oldR.Key)
                                        {
                                            newR.PackQuantity_DB = oldR.Value[0];
                                            newR.VolumeInPack_DB = oldR.Value[1];
                                            newR.MassInPack_DB = oldR.Value[2];
                                        }
                                    }
                                }
                            }
                        }
                        await ShowDialog.Handle(frm);
                        t.SelectedReports = tmp;
                    }
                }
        }
        #endregion

        #region ChangeReport
        public ReactiveCommand<object, Unit> ChangeReport { get; private set; }
        private async Task _ChangeReport(object par)
        {
            var param = par as ObservableCollectionWithItemPropertyChanged<IKey>;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    if (obj != null)
                    {
                        var t = desktop.MainWindow as MainWindow;
                        var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
                        var rep = (Reports)obj;
                        ChangeOrCreateVM frm = new(rep.Master.FormNum.Value, rep.Master, rep);
                        await ShowDialog.Handle(frm);

                        Local_Reports.Reports_Collection.Sorted = false;
                        await Local_Reports.Reports_Collection.QuickSortAsync();
                        t.SelectedReports = tmp;
                    }
                }
        }
        #endregion

        #region DeleteForm
        public ReactiveCommand<object, Unit> DeleteForm { get; private set; }
        private async Task _DeleteForm(object par)
        {
            var param = par as IEnumerable;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = (string)await ShowMessage.Handle(new List<string>() { "Вы действительно хотите удалить отчет?", "Да", "Нет" });
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
        #endregion

        #region DeleteReport
        public ReactiveCommand<object, Unit> DeleteReport { get; private set; }
        private async Task _DeleteReport(object par)
        {
            var param = par as IEnumerable;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = (string)await ShowMessage.Handle(new List<string>() { "Вы действительно хотите удалить организацию?", "Да", "Нет" });
                if (answ == "Да")
                {
                    if (param != null)
                        foreach (var item in param)
                            Local_Reports.Reports_Collection.Remove((Reports)item);

                    await StaticConfiguration.DBModel.SaveChangesAsync();
                }
                await Local_Reports.Reports_Collection.QuickSortAsync();
            }
        }
        #endregion

        #region Excel

        #region Excel_Export
        public ReactiveCommand<object, Unit> Excel_Export { get; private set; }
        private async Task _Excel_Export(object par)
        {
            var forms = par as ObservableCollectionWithItemPropertyChanged<IKey>;
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
                                            var t = Report.ExcelHeader(worksheet, param, 1, masterheaderlength + 1);
                                            Report.ExcelHeader(worksheetPrim, param, 1, masterheaderlength + 1);
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

                                            var form = forms.FirstOrDefault() as Report;
                                            lst.Add(form);

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
        #endregion

        #region Print_Excel_export
        public ReactiveCommand<object, Unit> Print_Excel_Export { get; private set; }
        private async Task _Print_Excel_Export(object par)
        {
            var forms = par as ObservableCollectionWithItemPropertyChanged<IKey>;
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
                                string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data"), "Excel"), param + ".xlsx");
#else
                                string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),"data"), "Excel"), param+".xlsx");
#endif
                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path), new FileInfo(pth)))
                                    {
                                        var form = (Report)forms.FirstOrDefault();
                                        ExcelWorksheet worksheetTitul =
                                            excelPackage.Workbook.Worksheets[param.Split('.')[0] + ".0"];
                                        ExcelWorksheet worksheetMain =
                                            excelPackage.Workbook.Worksheets[param];
                                        worksheetTitul.Cells.Style.ShrinkToFit = true;
                                        worksheetMain.Cells.Style.ShrinkToFit = true;
                                        
                                        _Excel_Print_Titul_Export(param, worksheetTitul, form);
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
        #endregion

        #region All_Excel_Export
        public ReactiveCommand<object, Unit> All_Excel_Export { get; private set; }
        private async Task _All_Excel_Export(object par)
        {
            var param = par as string;
            var find_rep = 0;
            foreach (Reports reps in Local_Reports.Reports_Collection) 
            {
                foreach (Report rep in reps.Report_Collection)
                {
                    if (rep.FormNum_DB == param)
                    {
                        find_rep += 1;
                    }
                }
            }
            if (find_rep == 0) return;
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
        #endregion

        private int _Excel_Export_Notes(string param, int StartRow, int StartColumn, ExcelWorksheet worksheetPrim, List<Report> forms)
        {
            foreach (Report item in forms)
            {

                var findReports = from Reports t in Local_Reports.Reports_Collection
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

                var findReports = from Reports t in Local_Reports.Reports_Collection
                                  where t.Report_Collection.Contains(item)
                                  select t;
                var reps = findReports.FirstOrDefault();
                if (reps != null)
                {
                    IEnumerable<IKey> t = null;
                    if (param == "2.1")
                    {
                        t = item[param].ToList<IKey>().Where(x => ((Form21)x).Sum_DB == true || ((Form21)x).SumGroup_DB == true);
                        if (item[param].ToList<IKey>().Count() > 0 && t.Count() == 0)
                        {
                            t = item[param].ToList<IKey>();
                        }
                    }
                    if (param == "2.2")
                    {
                        t = item[param].ToList<IKey>().Where(x => ((Form22)x).Sum_DB == true || ((Form22)x).SumGroup_DB == true);
                        if (item[param].ToList<IKey>().Count() > 0 && t.Count() == 0) 
                        {
                            t = item[param].ToList<IKey>();
                        }
                    }
                    if (param != "2.1" && param != "2.2") 
                    {
                        t = item[param].ToList<IKey>();
                    }
                    
                    List<IKey> lst = t.Count() > 0 ? item[param].ToList<IKey>().ToList() : item[param].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
                    if (lst.Count > 0)
                    {
                        var count = StartRow;
                        StartRow--;
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
                        var new_number = 2;
                        while (worksheet.Cells[new_number, 6].Value != null)
                        {
                            worksheet.Cells[new_number, 6].Value = new_number - 1;
                            new_number++;
                        }

                        StartRow = count;
                    }
                }
            }
            return StartRow;
        }
        private void _Excel_Print_Titul_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = from Reports t in Local_Reports.Reports_Collection
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
            var findReports = from Reports t in Local_Reports.Reports_Collection
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
            if (param == "2.8")
            {
                Start = 18;
            }

            for (int i = 0; i < form.Notes.Count - 1; i++)
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
                note.ExcelRow(worksheet, Count, 1);
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

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}