using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Long_Visual;
using Client_App.Views;
using DynamicData;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Abstracts;
using Models.Collections;
using Models.DBRealization;
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
using System.Threading.Tasks;

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
                    string[] names = file.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    Current_Db = "Интерактивное пособие по вводу данных ver.1.2.1 Текущая база данных - " + names[names.Length - 1];
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
                Current_Db = "Интерактивное пособие по вводу данных ver.1.2.1 Текущая база данных - " + "Local" + "_" + i + ".raodb";
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
            ImportFrom = ReactiveCommand.CreateFromTask(_ImportFrom);
            ExportForm = ReactiveCommand.CreateFromTask<object>(_ExportForm);
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
            AllOrganization_Excel_Export = ReactiveCommand.CreateFromTask(_AllOrganization_Excel_Export);
            ExcelMissingPas = ReactiveCommand.CreateFromTask<object>(_ExcelMissingPas);
            ExcelPasWithoutRep = ReactiveCommand.CreateFromTask<object>(_ExcelPasWithoutRep);
            ChangePasDir = ReactiveCommand.CreateFromTask<object>(_ChangePasDir);
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
            OnStartProgressBar = 1;
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

        #region ImportFromEx
        public ReactiveCommand<Unit, Unit> ImportFrom { get; private set; }
        private async Task GetDataFromRow(string param1, ExcelWorksheet worksheet1, int start, Report repFromEx)
        {
            if (param1 == "1.1")
            {
                Form11 form11 = new Form11();
                form11.ExcelGetRow(worksheet1, start);
                repFromEx.Rows11.Add(form11);
            }
            if (param1 == "1.2")
            {
                Form12 form12 = new Form12();
                form12.ExcelGetRow(worksheet1, start);
                repFromEx.Rows12.Add(form12);
            }
            if (param1 == "1.3")
            {
                Form13 form13 = new Form13();
                form13.ExcelGetRow(worksheet1, start);
                repFromEx.Rows13.Add(form13);
            }
            if (param1 == "1.4")
            {
                Form14 form14 = new Form14();
                form14.ExcelGetRow(worksheet1, start);
                repFromEx.Rows14.Add(form14);
            }
            if (param1 == "1.5")
            {
                Form15 form15 = new Form15();
                form15.ExcelGetRow(worksheet1, start);
                repFromEx.Rows15.Add(form15);
            }
            if (param1 == "1.6")
            {
                Form16 form16 = new Form16();
                form16.ExcelGetRow(worksheet1, start);
                repFromEx.Rows16.Add(form16);
            }
            if (param1 == "1.7")
            {
                Form17 form17 = new Form17();
                form17.ExcelGetRow(worksheet1, start);
                repFromEx.Rows17.Add(form17);
            }
            if (param1 == "1.8")
            {
                Form18 form18 = new Form18();
                form18.ExcelGetRow(worksheet1, start);
                repFromEx.Rows18.Add(form18);
            }
            if (param1 == "1.9")
            {
                Form19 form19 = new Form19();
                form19.ExcelGetRow(worksheet1, start);
                repFromEx.Rows19.Add(form19);
            }
            if (param1 == "2.1")
            {
                Form21 form21 = new Form21();
                form21.ExcelGetRow(worksheet1, start);
                repFromEx.Rows21.Add(form21);
            }
            if (param1 == "2.2")
            {
                Form22 form22 = new Form22();
                form22.ExcelGetRow(worksheet1, start);
                repFromEx.Rows22.Add(form22);
            }
            if (param1 == "2.3")
            {
                Form23 form23 = new Form23();
                form23.ExcelGetRow(worksheet1, start);
                repFromEx.Rows23.Add(form23);
            }
            if (param1 == "2.4")
            {
                Form24 form24 = new Form24();
                form24.ExcelGetRow(worksheet1, start);
                repFromEx.Rows24.Add(form24);
            }
            if (param1 == "2.5")
            {
                Form25 form25 = new Form25();
                form25.ExcelGetRow(worksheet1, start);
                repFromEx.Rows25.Add(form25);
            }
            if (param1 == "2.6")
            {
                Form26 form26 = new Form26();
                form26.ExcelGetRow(worksheet1, start);
                repFromEx.Rows26.Add(form26);
            }
            if (param1 == "2.7")
            {
                Form27 form27 = new Form27();
                form27.ExcelGetRow(worksheet1, start);
                repFromEx.Rows27.Add(form27);
            }
            if (param1 == "2.8")
            {
                Form28 form28 = new Form28();
                form28.ExcelGetRow(worksheet1, start);
                repFromEx.Rows28.Add(form28);
            }
            if (param1 == "2.9")
            {
                Form29 form29 = new Form29();
                form29.ExcelGetRow(worksheet1, start);
                repFromEx.Rows29.Add(form29);
            }
            if (param1 == "2.10")
            {
                Form210 form210 = new Form210();
                form210.ExcelGetRow(worksheet1, start);
                repFromEx.Rows210.Add(form210);
            }
            if (param1 == "2.11")
            {
                Form211 form211 = new Form211();
                form211.ExcelGetRow(worksheet1, start);
                repFromEx.Rows211.Add(form211);
            }
            if (param1 == "2.12")
            {
                Form212 form212 = new Form212();
                form212.ExcelGetRow(worksheet1, start);
                repFromEx.Rows212.Add(form212);
            }
        }
        private async Task GetDataTitleReps(Reports newRepsFromExcel, ExcelWorksheet worksheet0)
        {
            if (worksheet0.Name == "1.0")
            {
                newRepsFromExcel.Master_DB.Rows10[0].RegNo_DB = Convert.ToString(worksheet0.Cells["F6"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].OrganUprav_DB = Convert.ToString(worksheet0.Cells["F15"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F16"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLico_DB = Convert.ToString(worksheet0.Cells["F17"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].ShortJurLico_DB = worksheet0.Cells["F18"].Value == null ? "" : Convert.ToString(worksheet0.Cells["F18"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F19"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].JurLicoFactAddress_DB = Convert.ToString(worksheet0.Cells["F20"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F21"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Telephone_DB = Convert.ToString(worksheet0.Cells["F22"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Fax_DB = Convert.ToString(worksheet0.Cells["F23"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Email_DB = Convert.ToString(worksheet0.Cells["F24"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].SubjectRF_DB = Convert.ToString(worksheet0.Cells["F25"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLico_DB = Convert.ToString(worksheet0.Cells["F26"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].ShortJurLico_DB = worksheet0.Cells["F27"].Value == null ? "" : Convert.ToString(worksheet0.Cells["F27"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].JurLicoAddress_DB = Convert.ToString(worksheet0.Cells["F28"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].GradeFIO_DB = Convert.ToString(worksheet0.Cells["F29"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Telephone_DB = Convert.ToString(worksheet0.Cells["F30"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Fax_DB = Convert.ToString(worksheet0.Cells["F31"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Email_DB = Convert.ToString(worksheet0.Cells["F32"].Value);

                newRepsFromExcel.Master_DB.Rows10[0].Okpo_DB = worksheet0.Cells["B36"].Value == null ? "" : Convert.ToString(worksheet0.Cells["B36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okved_DB = Convert.ToString(worksheet0.Cells["C36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okogu_DB = Convert.ToString(worksheet0.Cells["D36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Oktmo_DB = Convert.ToString(worksheet0.Cells["E36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Inn_DB = Convert.ToString(worksheet0.Cells["F36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Kpp_DB = Convert.ToString(worksheet0.Cells["G36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okopf_DB = Convert.ToString(worksheet0.Cells["H36"].Value);
                newRepsFromExcel.Master_DB.Rows10[0].Okfs_DB = Convert.ToString(worksheet0.Cells["I36"].Value);

                newRepsFromExcel.Master_DB.Rows10[1].Okpo_DB = worksheet0.Cells["B37"].Value == null ? "" : Convert.ToString(worksheet0.Cells["B37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okved_DB = Convert.ToString(worksheet0.Cells["C37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okogu_DB = Convert.ToString(worksheet0.Cells["D37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Oktmo_DB = Convert.ToString(worksheet0.Cells["E37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Inn_DB = Convert.ToString(worksheet0.Cells["F37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Kpp_DB = Convert.ToString(worksheet0.Cells["G37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okopf_DB = Convert.ToString(worksheet0.Cells["H37"].Value);
                newRepsFromExcel.Master_DB.Rows10[1].Okfs_DB = Convert.ToString(worksheet0.Cells["I37"].Value);
            }
            if (worksheet0.Name == "2.0")
            {
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
            }
        }
        private async Task<Reports> CheckReps(ExcelWorksheet worksheet0)
        {
            IEnumerable<Reports>? reps = null;

            if (worksheet0.Name == "1.0")
            {
                reps = from Reports t in Local_Reports.Reports_Collection10
                       where ((Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[0].Okpo_DB &&
                       Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[0].RegNo_DB))
                       select t;
            }
            if (worksheet0.Name == "2.0")
            {
                reps = from Reports t in Local_Reports.Reports_Collection20
                       where ((Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows20[0].Okpo_DB &&
                       Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows20[0].RegNo_DB))
                       select t;
            }

            if (reps.Count() != 0)
            {
                return reps.FirstOrDefault();
            }
            else
            {
                var newRepsFromExcel = new Reports();
                var param0 = worksheet0.Name;
                newRepsFromExcel.Master_DB = new Report()
                {
                    FormNum_DB = param0
                };
                if (param0 == "1.0")
                {
                    var ty1 = (Form10)FormCreator.Create(param0);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create(param0);
                    ty2.NumberInOrder_DB = 2;
                    newRepsFromExcel.Master_DB.Rows10.Add(ty1);
                    newRepsFromExcel.Master_DB.Rows10.Add(ty2);
                }
                if (param0 == "2.0")
                {
                    var ty1 = (Form20)FormCreator.Create(param0);
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create(param0);
                    ty2.NumberInOrder_DB = 2;
                    newRepsFromExcel.Master_DB.Rows20.Add(ty1);
                    newRepsFromExcel.Master_DB.Rows20.Add(ty2);
                }
                await GetDataTitleReps(newRepsFromExcel, worksheet0);
                Local_Reports.Reports_Collection.Add(newRepsFromExcel);
                return newRepsFromExcel;
            }
        }
        private async Task _ImportFrom()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = await GetSelectedFilesFromDialog("Excel", "xlsx");
                if (answ != null)
                {
                    foreach (var res in answ)
                    {
                        if (res != "")
                        {
                            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(res)))
                            {
                                ExcelWorksheet worksheet0 = excelPackage.Workbook.Worksheets[0];



                                bool val = false;
                                if (worksheet0.Name == "1.0" &&
                                    (Convert.ToString(worksheet0.Cells["A3"].Value) == "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ" ||
                                     Convert.ToString(worksheet0.Cells["A3"].Value) == "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"))
                                {
                                    val = true;
                                }
                                if (worksheet0.Name == "2.0" &&
                                    (Convert.ToString(worksheet0.Cells["A4"].Value) == "ГОСУДАОСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ" ||
                                     Convert.ToString(worksheet0.Cells["A4"].Value) == "ГОСУДАРСТВЕННЫЙ УЧЕТ И КОНТРОЛЬ РАДИОАКТИВНЫХ ВЕЩЕСТВ И РАДИОАКТИВНЫХ ОТХОДОВ"))
                                {
                                    val = true;
                                }

                                if (val)
                                {
                                    var timeCreate = new List<string>() { excelPackage.File.CreationTime.Day.ToString(), excelPackage.File.CreationTime.Month.ToString(), excelPackage.File.CreationTime.Year.ToString() };
                                    if (timeCreate[0].Length == 1)
                                    {
                                        timeCreate[0] = "0" + timeCreate[0];
                                    }
                                    if (timeCreate[1].Length == 1)
                                    {
                                        timeCreate[1] = "0" + timeCreate[1];
                                    }

                                    Reports newRepsFromExcel = await CheckReps(worksheet0);

                                    ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets[1];
                                    var param1 = worksheet1.Name;
                                    var repFromEx = new Report()
                                    {
                                        FormNum_DB = param1
                                    };
                                    repFromEx.ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}";
                                    if (param1.Split('.')[0] == "1")
                                    {
                                        repFromEx.StartPeriod_DB = Convert.ToString(worksheet1.Cells["G3"].Text).Replace("/", ".");
                                        repFromEx.EndPeriod_DB = Convert.ToString(worksheet1.Cells["G4"].Text).Replace("/", ".");
                                        repFromEx.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G5"].Value);
                                    }
                                    else
                                    {
                                        switch (param1)
                                        {
                                            case "2.6":
                                                {
                                                    repFromEx.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                                                    repFromEx.SourcesQuantity26_DB = Convert.ToInt32(worksheet1.Cells["G5"].Value);
                                                    repFromEx.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                                                    break;
                                                }
                                            case "2.7":
                                                {
                                                    repFromEx.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                                                    repFromEx.PermissionNumber27_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                                                    repFromEx.ValidBegin27_DB = Convert.ToString(worksheet1.Cells["G5"].Value);
                                                    repFromEx.ValidThru27_DB = Convert.ToString(worksheet1.Cells["J5"].Value);
                                                    repFromEx.PermissionDocumentName27_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                                                    repFromEx.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                                                    break;
                                                }
                                            case "2.8":
                                                {
                                                    repFromEx.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G3"].Value);
                                                    repFromEx.PermissionNumber_28_DB = Convert.ToString(worksheet1.Cells["G4"].Value);
                                                    repFromEx.ValidBegin_28_DB = Convert.ToString(worksheet1.Cells["K4"].Value);
                                                    repFromEx.ValidThru_28_DB = Convert.ToString(worksheet1.Cells["N4"].Value);
                                                    repFromEx.PermissionDocumentName_28_DB = Convert.ToString(worksheet1.Cells["G5"].Value);

                                                    repFromEx.PermissionNumber1_28_DB = Convert.ToString(worksheet1.Cells["G6"].Value);
                                                    repFromEx.ValidBegin1_28_DB = Convert.ToString(worksheet1.Cells["K6"].Value);
                                                    repFromEx.ValidThru1_28_DB = Convert.ToString(worksheet1.Cells["N6"].Value);
                                                    repFromEx.PermissionDocumentName1_28_DB = Convert.ToString(worksheet1.Cells["G7"].Value);

                                                    repFromEx.ContractNumber_28_DB = Convert.ToString(worksheet1.Cells["G8"].Value);
                                                    repFromEx.ValidBegin2_28_DB = Convert.ToString(worksheet1.Cells["K8"].Value);
                                                    repFromEx.ValidThru2_28_DB = Convert.ToString(worksheet1.Cells["N8"].Value);
                                                    repFromEx.OrganisationReciever_28_DB = Convert.ToString(worksheet1.Cells["G9"].Value);

                                                    repFromEx.GradeExecutor_DB = Convert.ToString(worksheet1.Cells["D21"].Value);
                                                    repFromEx.FIOexecutor_DB = Convert.ToString(worksheet1.Cells["F21"].Value);
                                                    repFromEx.ExecPhone_DB = Convert.ToString(worksheet1.Cells["I21"].Value);
                                                    repFromEx.ExecEmail_DB = Convert.ToString(worksheet1.Cells["K21"].Value);
                                                    repFromEx.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Value);
                                                    break;
                                                }
                                            default:
                                                {
                                                    repFromEx.CorrectionNumber_DB = Convert.ToByte(worksheet1.Cells["G4"].Value);
                                                    repFromEx.Year_DB = Convert.ToString(worksheet0.Cells["G10"].Text);
                                                    break;
                                                }
                                        }
                                    }
                                    repFromEx.GradeExecutor_DB = (string)worksheet1.Cells[$"D{worksheet1.Dimension.Rows - 1}"].Value;
                                    repFromEx.FIOexecutor_DB = (string)worksheet1.Cells[$"F{worksheet1.Dimension.Rows - 1}"].Value;
                                    repFromEx.ExecPhone_DB = (string)worksheet1.Cells[$"I{worksheet1.Dimension.Rows - 1}"].Value;
                                    repFromEx.ExecEmail_DB = (string)worksheet1.Cells[$"K{worksheet1.Dimension.Rows - 1}"].Value;
                                    int start = 11;
                                    if (param1 == "2.8")
                                    {
                                        start = param1 switch
                                        {
                                            "2.8" => 14,
                                            _ => 11
                                        };
                                    }
                                    var end = $"A{start}";
                                    while (worksheet1.Cells[end].Value != null && worksheet1.Cells[end].Value.ToString().ToLower() != "примечание:")
                                    {
                                        await GetDataFromRow(param1, worksheet1, start, repFromEx);
                                        start++;
                                        end = $"A{start}";
                                    }

                                    if (worksheet1.Cells[end].Value == null)
                                        start += 3;
                                    else if (worksheet1.Cells[end].Value.ToString().ToLower() == "примечание:")
                                        start += 2;

                                    while (worksheet1.Cells[$"A{start}"].Value != null || worksheet1.Cells[$"B{start}"].Value != null || worksheet1.Cells[$"C{start}"].Value != null)
                                    {
                                        Note newNote = new Note();

                                        newNote.ExcelGetRow(worksheet1, start);
                                        repFromEx.Notes.Add(newNote);
                                        start++;
                                    }

                                    if (newRepsFromExcel.Report_Collection.Count != 0)
                                    {
                                        if (worksheet0.Name == "1.0")
                                        {
                                            var not_in = false;
                                            var skipLess = false;
                                            var skipNew = false;
                                            var _skipNew = false;
                                            var skipInter = false;

                                            foreach (Report rep in newRepsFromExcel.Report_Collection)
                                            {

                                                DateTimeOffset st_elem = DateTimeOffset.Now;
                                                DateTimeOffset en_elem = DateTimeOffset.Now;
                                                try
                                                {
                                                    st_elem = DateTime.Parse(rep.StartPeriod_DB) > DateTime.Parse(rep.EndPeriod_DB) ? DateTime.Parse(rep.EndPeriod_DB) : DateTime.Parse(rep.StartPeriod_DB);
                                                    en_elem = DateTime.Parse(rep.StartPeriod_DB) < DateTime.Parse(rep.EndPeriod_DB) ? DateTime.Parse(rep.EndPeriod_DB) : DateTime.Parse(rep.StartPeriod_DB);
                                                }
                                                catch (Exception ex)
                                                { }

                                                DateTimeOffset st_it = DateTimeOffset.Now;
                                                DateTimeOffset en_it = DateTimeOffset.Now;
                                                try
                                                {
                                                    st_it = DateTime.Parse(repFromEx.StartPeriod_DB) > DateTime.Parse(repFromEx.EndPeriod_DB) ? DateTime.Parse(repFromEx.EndPeriod_DB) : DateTime.Parse(repFromEx.StartPeriod_DB);
                                                    en_it = DateTime.Parse(repFromEx.StartPeriod_DB) < DateTime.Parse(repFromEx.EndPeriod_DB) ? DateTime.Parse(repFromEx.EndPeriod_DB) : DateTime.Parse(repFromEx.StartPeriod_DB);
                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                                if (st_elem == st_it && en_elem == en_it && repFromEx.FormNum_DB == rep.FormNum_DB)
                                                {
                                                    not_in = true;
                                                    if (repFromEx.CorrectionNumber_DB < rep.CorrectionNumber_DB)
                                                    {
                                                        if (!skipLess)
                                                        {
                                                            var str = " Вы пытаетесь загрузить форму с наименьщим номером корректировки - " +
                                                                repFromEx.CorrectionNumber_DB + ",\n" +
                                                                "при текущем значении корректировки - " +
                                                                rep.CorrectionNumber_DB + ".\n" +
                                                                "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                "Начало отчетного периода - " + repFromEx.StartPeriod_DB + "\n" +
                                                                "Конец отчетного периода - " + repFromEx.EndPeriod_DB + "\n" +
                                                                "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                "Количество строк - " + repFromEx.Rows.Count;
                                                            var an = await ShowMessage.Handle(new List<string>() { str, "Отчет", "OK", "Пропустить для всех" });
                                                            if (an == "Пропустить для всех")
                                                            {
                                                                skipLess = true;
                                                            }
                                                        }
                                                    }
                                                    else if (repFromEx.CorrectionNumber_DB == rep.CorrectionNumber_DB)
                                                    {
                                                        var str = "Совпадение даты в " + rep.FormNum_DB + " " +
                                                            rep.StartPeriod_DB + "-" +
                                                            rep.EndPeriod_DB + " .\n" +
                                                            "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                            newRepsFromExcel.Master.RegNoRep.Value + " " +
                                                            newRepsFromExcel.Master.ShortJurLicoRep.Value + " " +
                                                            newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        var an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                    "Заменить",
                                                    "Дополнить",
                                                    "Сохранить оба",
                                                    "Отменить"
                                                });
                                                        await ChechAanswer(an, newRepsFromExcel, rep, repFromEx);
                                                    }
                                                    else
                                                    {
                                                        var an = "Загрузить новую";
                                                        if (!skipNew)
                                                        {
                                                            if (newRepsFromExcel.Report_Collection.Count() > 1)
                                                            {
                                                                var str = "Загрузить новую форму? \n" +
                                                                    "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                    "Начало отчетного периода - " + repFromEx.StartPeriod_DB + "\n" +
                                                                    "Конец отчетного периода - " + repFromEx.EndPeriod_DB + "\n" +
                                                                    "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                                    "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                    "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                    "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                    "Форма с предыдущим номером корректировки №" +
                                                                    rep.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                                                    "Сделайте резервную копию." + "\n" +
                                                                    "Количество строк - " + repFromEx.Rows.Count;
                                                                an = await ShowMessage.Handle(new List<string>() {str, "Отчет",
                                                        "Загрузить новую",
                                                        "Отмена",
                                                        "Загрузить для все"
                                                        });
                                                                if (an == "Загрузить для всех") skipNew = true;
                                                                an = "Загрузить новую";
                                                            }
                                                            else
                                                            {
                                                                var str = "Загрузить новую форму? \n" +
                                                                    "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                    "Начало отчетного периода - " + repFromEx.StartPeriod_DB + "\n" +
                                                                    "Конец отчетного периода - " + repFromEx.EndPeriod_DB + "\n" +
                                                                    "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                                    "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                    "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                    "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                    "Форма с предыдущим номером корректировки №" +
                                                                    rep.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                                                    "Сделайте резервную копию." + "\n" +
                                                                    "Количество строк - " + repFromEx.Rows.Count;
                                                                an = await ShowMessage.Handle(new List<string>() {str, "Отчет",
                                                                "Загрузить новую",
                                                                "Отмена"
                                                                });
                                                            }
                                                        }
                                                        await ChechAanswer(an, newRepsFromExcel, rep, repFromEx);
                                                    }
                                                }
                                                if ((st_elem > st_it && st_elem < en_it || en_elem > st_it && en_elem < en_it) && repFromEx.FormNum.Value == rep.FormNum.Value)
                                                {
                                                    not_in = true;
                                                    var an = "Отменить";
                                                    if (!skipInter)
                                                    {
                                                        var str = "Пересечение даты в " + rep.FormNum_DB + " " +
                                                            rep.StartPeriod_DB + "-" +
                                                            rep.EndPeriod_DB + " \n" +
                                                            newRepsFromExcel.Master.RegNoRep.Value + " " +
                                                            newRepsFromExcel.Master.ShortJurLicoRep.Value + " " +
                                                            newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        an = await ShowMessage.Handle(new List<string>(){str,"Отчет",
                                                        "Сохранить оба",
                                                        "Отменить"
                                                        });
                                                        skipInter = true;
                                                    }
                                                    await ChechAanswer(an, newRepsFromExcel, null, repFromEx);
                                                }
                                            }
                                            if (!not_in)
                                            {
                                                var an = "Да";
                                                if (!_skipNew)
                                                {
                                                    if (newRepsFromExcel.Report_Collection.Count() > 1)
                                                    {
                                                        var str = "Загрузить новую форму?\n" +
                                                            "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                            "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                            "Начало отчетного периода - " + repFromEx.StartPeriod_DB + "\n" +
                                                            "Конец отчетного периода - " + repFromEx.EndPeriod_DB + "\n" +
                                                            "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                            "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                            "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                        "Да",
                                                        "Нет",
                                                        "Загрузить для всех"
                                                        });
                                                    }
                                                    else
                                                    {
                                                        var str = "Загрузить новую форму?\n" +
                                                            "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                            "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                            "Начало отчетного периода - " + repFromEx.StartPeriod_DB + "\n" +
                                                            "Конец отчетного периода - " + repFromEx.EndPeriod_DB + "\n" +
                                                            "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                            "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                            "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                        "Да",
                                                        "Нет"
                                                        });
                                                    }
                                                }
                                                await ChechAanswer(an, newRepsFromExcel, null, repFromEx);
                                            }
                                        }
                                        if (worksheet0.Name == "2.0")
                                        {
                                            var not_in = false;
                                            var skipLess = false;
                                            var skipNew = false;
                                            var _skipNew = false;
                                            var skipInter = false;
                                            foreach (Report rep in newRepsFromExcel.Report_Collection)
                                            {

                                                if (rep.Year_DB == repFromEx.Year_DB && repFromEx.FormNum_DB == rep.FormNum_DB)
                                                {
                                                    not_in = true;
                                                    if (repFromEx.CorrectionNumber_DB < rep.CorrectionNumber_DB)
                                                    {
                                                        if (!skipLess)
                                                        {
                                                            var str = " Вы пытаетесь загрузить форму с наименьщим номером корректировки - " +
                                                                repFromEx.CorrectionNumber_DB + ",\n" +
                                                                "при текущем значении корректировки - " +
                                                                rep.CorrectionNumber_DB + ".\n" +
                                                                "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                "Отчетный год - " + repFromEx.Year_DB + "\n" +
                                                                "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                "Количество строк - " + repFromEx.Rows.Count;
                                                            var an = await ShowMessage.Handle(new List<string>() { str, "Отчет", "OK", "Пропустить для всех" });
                                                            if (an == "Пропустить для всех") skipLess = true;
                                                        }
                                                    }
                                                    else if (repFromEx.CorrectionNumber_DB == rep.CorrectionNumber_DB)
                                                    {
                                                        var str = "Совпадение даты в " + rep.FormNum_DB + " " +
                                                        rep.Year_DB + " .\n" +
                                                        "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                        newRepsFromExcel.Master.RegNoRep.Value + " \n" +
                                                        newRepsFromExcel.Master.ShortJurLicoRep.Value + " " +
                                                        newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                        "Количество строк - " + repFromEx.Rows.Count;
                                                        var an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                        "Заменить",
                                                        "Сохранить оба",
                                                        "Отменить"
                                                    });
                                                        await ChechAanswer(an, newRepsFromExcel, rep, repFromEx);
                                                    }
                                                    else
                                                    {
                                                        var an = "Загрузить новую";
                                                        if (!skipNew)
                                                        {
                                                            if (newRepsFromExcel.Report_Collection.Count() > 1)
                                                            {
                                                                var str = "Загрузить новую форму? \n" +
                                                                "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                "Отчетный год - " + repFromEx.Year_DB + "\n" +
                                                                "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                                "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                "Форма с предыдущим номером корректировки №" +
                                                                rep.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                                                "Сделайте резервную копию." + "\n" +
                                                                "Количество строк - " + repFromEx.Rows.Count;
                                                                an = await ShowMessage.Handle(new List<string>() {
                                                                        str,
                                                                        "Отчет",
                                                                        "Загрузить новую",
                                                                        "Отмена",
                                                                        "Загрузить для всех"
                                                                        });
                                                                if (an == "Загрузить для всех") skipNew = true;
                                                                an = "Загрузить новую";
                                                            }
                                                            else
                                                            {
                                                                var str = "Загрузить новую форму? \n" +
                                                                    "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                                    "Отчетный год - " + repFromEx.Year_DB + "\n" +
                                                                    "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                                    "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                                    "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                                    "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                                    "Форма с предыдущим номером корректировки №" +
                                                                    rep.CorrectionNumber_DB + " будет безвозвратно удалена.\n" +
                                                                    "Сделайте резервную копию." + "\n" +
                                                                    "Количество строк - " + repFromEx.Rows.Count;
                                                                an = await ShowMessage.Handle(new List<string>() {
                                                                        str,
                                                                        "Отчет",
                                                                        "Загрузить новую",
                                                                        "Отмена"
                                                                        });
                                                            }
                                                        }
                                                        await ChechAanswer(an, newRepsFromExcel, rep, repFromEx);
                                                    }
                                                }
                                            }
                                            if (!not_in)
                                            {
                                                var an = "Да";
                                                if (!_skipNew)
                                                {
                                                    if (newRepsFromExcel.Report_Collection.Count() > 1)
                                                    {
                                                        var str = "Загрузить новую форму? \n" +
                                                            "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                            "Отчетный год - " + repFromEx.Year_DB + "\n" +
                                                            "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                            "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                            "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                            "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                            "Да",
                                                            "Нет",
                                                            "Загрузить для всех"
                                                        });
                                                        if (an == "Загрузить для всех") _skipNew = true;
                                                        an = "Да";
                                                    }
                                                    else
                                                    {
                                                        var str = "Загрузить новую форму? \n" +
                                                            "Номер формы - " + repFromEx.FormNum_DB + "\n" +
                                                            "Отчетный год - " + repFromEx.Year_DB + "\n" +
                                                            "Номер корректировки -" + repFromEx.CorrectionNumber_DB + "\n" +
                                                            "Регистрационный номер - " + newRepsFromExcel.Master.RegNoRep.Value + "\n" +
                                                            "Сокращенное наименование - " + newRepsFromExcel.Master.ShortJurLicoRep.Value + "\n" +
                                                            "ОКПО - " + newRepsFromExcel.Master.OkpoRep.Value + "\n" +
                                                            "Количество строк - " + repFromEx.Rows.Count;
                                                        an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                                            "Да",
                                                            "Нет"
                                                        });
                                                    }
                                                }
                                                await ChechAanswer(an, newRepsFromExcel, null, repFromEx);
                                                not_in = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        newRepsFromExcel.Report_Collection.Add(repFromEx);
                                    }
                                    var dbm = StaticConfiguration.DBModel;
                                    dbm.SaveChanges();
                                }
                                else
                                {
                                    var str = "Не соответствует формат данных!";
                                    var an = await ShowMessage.Handle(new List<string>(){str, "Формат данных",
                                                    "Ок"
                                                });
                                }

                            }


                        }
                    }
                }
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
                #region Test Version
                var t = db.Database.GetPendingMigrations();
                var a = db.Database.GetMigrations();
                var b = db.Database.GetAppliedMigrations();
                #endregion

                await db.Database.MigrateAsync();
                await db.LoadTablesAsync();
                await ProcessDataBaseFillEmpty(db);

                if (db.DBObservableDbSet.Local.First().Reports_Collection.ToList().Count != 0)
                {
                    lst = db.DBObservableDbSet.Local.First().Reports_Collection.ToList();
                }
                else
                {
                    lst = await db.ReportsCollectionDbSet.ToListAsync();
                }
            }
            return lst;
        }
        private async Task<Reports> GetReports11FromLocalEqual(Reports item)
        {
            try
            {
                var tb1 = item.Report_Collection.Where(x => x.FormNum_DB[0].ToString().Equals("1"));
                if (tb1.Count() != 0)
                {
                    var tb11 = from Reports t in Local_Reports.Reports_Collection10
                               where ((item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB &&
                               item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[0].RegNo_DB &&
                               item.Master.Rows10[1].Okpo_DB == "") ||
                               (item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB &&
                               item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[1].RegNo_DB &&
                               item.Master.Rows10[1].Okpo_DB != ""))
                               select t;
                    return tb11.FirstOrDefault();
                }
                return null;
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
                               where ((item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB &&
                               item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[0].RegNo_DB &&
                               item.Master.Rows20[1].Okpo_DB == "") ||
                               (item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB &&
                               item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[1].RegNo_DB &&
                               item.Master.Rows20[1].Okpo_DB != ""))
                               select t;
                    //var tb21 = from Reports t in Local_Reports.Reports_Collection20
                    //           where (((item.Master.Rows10[0].Okpo_DB == "") &&
                    //           (t.Master.Rows20[0].Okpo_DB == "")) ||
                    //           ((t.Master.Rows20[0].Okpo_DB == item.Master.Rows20[0].Okpo_DB) &&
                    //           (t.Master.Rows20[1].Okpo_DB == item.Master.Rows20[1].Okpo_DB))) &&
                    //           (((item.Master.Rows20[0].RegNo_DB == "") &&
                    //           (t.Master.Rows20[0].RegNo_DB == "")) ||
                    //           ((t.Master.Rows20[0].RegNo_DB == item.Master.Rows20[0].RegNo_DB) &&
                    //           (t.Master.Rows20[1].RegNo_DB == item.Master.Rows20[1].RegNo_DB))) &&
                    //           (((item.Master.Rows20[0].ShortJurLico_DB == "") &&
                    //           (t.Master.Rows20[0].ShortJurLico_DB == "")) ||
                    //           ((t.Master.Rows20[0].ShortJurLico_DB == item.Master.Rows20[0].ShortJurLico_DB) &&
                    //           (t.Master.Rows20[1].ShortJurLico_DB == item.Master.Rows20[1].ShortJurLico_DB)))
                    //           select t;
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

            var skipLess = false;
            var doSomething = false;
            var skipNew = false;
            var _skipNew = false;
            var skipInter = false;

            foreach (Report it in item.Report_Collection)
            {
                if (first11.Report_Collection.Count != 0)
                {
                    foreach (Report elem in first11.Report_Collection)
                    {
                        DateTime st_elem = DateTime.Parse(DateTime.Now.ToShortDateString());
                        DateTime en_elem = DateTime.Parse(DateTime.Now.ToShortDateString());
                        try
                        {
                            st_elem = DateTime.Parse(elem.StartPeriod_DB) > DateTime.Parse(elem.EndPeriod_DB) ? DateTime.Parse(elem.EndPeriod_DB) : DateTime.Parse(elem.StartPeriod_DB);
                            en_elem = DateTime.Parse(elem.StartPeriod_DB) < DateTime.Parse(elem.EndPeriod_DB) ? DateTime.Parse(elem.EndPeriod_DB) : DateTime.Parse(elem.StartPeriod_DB);
                        }
                        catch (Exception ex)
                        { }

                        DateTime st_it = DateTime.Parse(DateTime.Now.ToShortDateString());
                        DateTime en_it = DateTime.Parse(DateTime.Now.ToShortDateString());
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
                                if (!skipLess)
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
                                        "ОКПО - " + first11.Master.OkpoRep.Value + "\n" +
                                        "Количество строк - " + it.Rows.Count;
                                    var an = await ShowMessage.Handle(new List<string>() { str, "Отчет", "OK", "Пропустить для всех" });
                                    if (an == "Пропустить для всех")
                                    {
                                        skipLess = true;
                                    }
                                }
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB && it.ExportDate_DB == elem.ExportDate_DB)
                            {
                                var str = "Совпадение даты в " + elem.FormNum_DB + " " +
                                    elem.StartPeriod_DB + "-" +
                                    elem.EndPeriod_DB + " .\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    first11.Master.RegNoRep.Value + " " +
                                    first11.Master.ShortJurLicoRep.Value + " " +
                                    first11.Master.OkpoRep.Value + "\n" +
                                    "Количество строк - " + it.Rows.Count;
                                doSomething = true;
                                var an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                    "Заменить",
                                    "Дополнить",
                                    "Сохранить оба",
                                    "Отменить"
                                });
                                await ChechAanswer(an, first11, elem, it, doSomething);
                                doSomething = true;
                            }
                            else
                            {
                                var an = "Загрузить новую";
                                if (!skipNew)
                                {
                                    if (item.Report_Collection.Count() > 1)
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
                                            "Сделайте резервную копию." + "\n" +
                                            "Количество строк - " + it.Rows.Count;
                                        doSomething = true;
                                        an = await ShowMessage.Handle(new List<string>() {str, "Отчет",
                                            "Загрузить новую",
                                            "Отмена",
                                            "Загрузить для все"
                                            });
                                        if (an == "Загрузить для всех") skipNew = true;
                                        an = "Загрузить новую";
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
                                            "Сделайте резервную копию." + "\n" +
                                            "Количество строк - " + it.Rows.Count;
                                        doSomething = true;
                                        an = await ShowMessage.Handle(new List<string>() {str, "Отчет",
                                            "Загрузить новую",
                                            "Отмена"
                                            });
                                    }
                                }
                                await ChechAanswer(an, first11, elem, it, doSomething);
                                doSomething = true;
                            }
                        }
                        else
                        {
                            if ((st_elem > st_it && st_elem < en_it || en_elem > st_it && en_elem < en_it) && it.FormNum.Value == elem.FormNum.Value)
                            {
                                not_in = true;
                                var an = "Отменить";
                                if (!skipInter)
                                {
                                    var str = "Пересечение даты в " + elem.FormNum_DB + " " +
                                        elem.StartPeriod_DB + "-" +
                                        elem.EndPeriod_DB + " \n" +
                                        first11.Master.RegNoRep.Value + " " +
                                        first11.Master.ShortJurLicoRep.Value + " " +
                                        first11.Master.OkpoRep.Value + "\n" +
                                        "Количество строк - " + it.Rows.Count;
                                    an = await ShowMessage.Handle(new List<string>(){str,"Отчет",
                                "Сохранить оба",
                                "Отменить"
                                });
                                    skipInter = true;
                                }
                                await ChechAanswer(an, first11, null, it, doSomething);
                                doSomething = true;
                            }
                        }
                    }
                    if (!not_in)
                    {
                        var an = "Да";
                        if (!_skipNew)
                        {
                            if (item.Report_Collection.Count() > 1)
                            {
                                var str = "Загрузить новую форму?\n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    "Начало отчетного периода - " + it.StartPeriod_DB + "\n" +
                                    "Конец отчетного периода - " + it.EndPeriod_DB + "\n" +
                                    "Регистрационный номер - " + first11.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first11.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first11.Master.OkpoRep.Value + "\n" +
                                    "Количество строк - " + it.Rows.Count;
                                an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                    "Да",
                                    "Нет",
                                    "Загрузить для всех"
                                    });
                                an = "Да";
                            }
                            else
                            {
                                var str = "Загрузить новую форму?\n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    "Начало отчетного периода - " + it.StartPeriod_DB + "\n" +
                                    "Конец отчетного периода - " + it.EndPeriod_DB + "\n" +
                                    "Регистрационный номер - " + first11.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first11.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first11.Master.OkpoRep.Value + "\n" +
                                    "Количество строк - " + it.Rows.Count;
                                an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                "Да",
                                "Нет"
                                });
                            }
                        }
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

            var skipLess = false;
            var skipNew = false;
            var _skipNew = false;

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
                                if (!skipLess)
                                {
                                    var str = " Вы пытаетесь загрузить форму с наименьщим номером корректировки - " +
                                        it.CorrectionNumber_DB + ",\n" +
                                        "при текущем значении корректировки - " +
                                        elem.CorrectionNumber_DB + ".\n" +
                                        "Номер формы - " + it.FormNum_DB + "\n" +
                                        "Отчетный год - " + it.Year_DB + "\n" +
                                        "Регистрационный номер - " + first21.Master.RegNoRep.Value + "\n" +
                                        "Сокращенное наименование - " + first21.Master.ShortJurLicoRep.Value + "\n" +
                                        "ОКПО - " + first21.Master.OkpoRep.Value + "\n" +
                                        "Количество строк - " + it.Rows.Count;
                                    var an = await ShowMessage.Handle(new List<string>() { str, "Отчет", "OK", "Пропустить для всех" });
                                    if (an == "Пропустить для всех") skipLess = true;
                                }
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB && it.ExportDate_DB == elem.ExportDate_DB)
                            {
                                var str = "Совпадение даты в " + elem.FormNum_DB + " " +
                                elem.Year_DB + " .\n" +
                                "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                first21.Master.RegNoRep.Value + " \n" +
                                first21.Master.ShortJurLicoRep.Value + " " +
                                first21.Master.OkpoRep.Value + "\n" +
                                "Количество строк - " + it.Rows.Count;
                                var an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                    "Заменить",
                                    "Сохранить оба",
                                    "Отменить"
                                });
                                await ChechAanswer(an, first21, elem, it);
                            }
                            else
                            {
                                var an = "Загрузить новую";
                                if (!skipNew)
                                {
                                    if (item.Report_Collection.Count() > 1)
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
                                        "Сделайте резервную копию." + "\n" +
                                        "Количество строк - " + it.Rows.Count;
                                        an = await ShowMessage.Handle(new List<string>() {
                                                                        str,
                                                                        "Отчет",
                                                                        "Загрузить новую",
                                                                        "Отмена",
                                                                        "Загрузить для всех"
                                                                        });
                                        if (an == "Загрузить для всех") skipNew = true;
                                        an = "Загрузить новую";
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
                                            "Сделайте резервную копию." + "\n" +
                                            "Количество строк - " + it.Rows.Count;
                                        an = await ShowMessage.Handle(new List<string>() {
                                                                        str,
                                                                        "Отчет",
                                                                        "Загрузить новую",
                                                                        "Отмена"
                                                                        });
                                    }
                                }
                                await ChechAanswer(an, first21, elem, it);
                            }
                        }
                    }
                    if (!not_in)
                    {
                        var an = "Да";
                        if (!_skipNew)
                        {
                            if (item.Report_Collection.Count() > 1)
                            {
                                var str = "Загрузить новую форму? \n" +
                                    "Номер формы - " + it.FormNum_DB + "\n" +
                                    "Отчетный год - " + it.Year_DB + "\n" +
                                    "Номер корректировки -" + it.CorrectionNumber_DB + "\n" +
                                    "Регистрационный номер - " + first21.Master.RegNoRep.Value + "\n" +
                                    "Сокращенное наименование - " + first21.Master.ShortJurLicoRep.Value + "\n" +
                                    "ОКПО - " + first21.Master.OkpoRep.Value + "\n" +
                                    "Количество строк - " + it.Rows.Count;
                                an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                    "Да",
                                    "Нет",
                                    "Загрузить для всех"
                                });
                                if (an == "Загрузить для всех") _skipNew = true;
                                an = "Да";
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
                                    "Количество строк - " + it.Rows.Count;
                                an = await ShowMessage.Handle(new List<string>(){str, "Отчет",
                                    "Да",
                                    "Нет"
                                });
                            }
                        }
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
        private async Task ChechAanswer(string an, Reports first, Report elem = null, Report it = null, bool doSomething = false)
        {
            if (an == "Сохранить оба" || an == "Да")
            {
                if (doSomething)
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
                        var skipAll = false;
                        foreach (var item in reportsCollection)
                        {
                            if (item.Master.Rows10.Count != 0)
                                item.Master.Rows10[1].RegNo_DB = item.Master.Rows10[0].RegNo_DB;
                            else
                                item.Master.Rows20[1].RegNo_DB = item.Master.Rows20[0].RegNo_DB;
                            Reports first11 = await GetReports11FromLocalEqual(item);
                            Reports first21 = await GetReports21FromLocalEqual(item);
                            await RestoreReportsOrders(item);
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
                            else if (first21 == null && first11 == null)
                            {
                                var rep = item.Report_Collection.FirstOrDefault();
                                string? an = null;
                                if (rep != null)
                                {
                                    if (!skipAll)
                                    {
                                        var str = "Был добавлен отчет по форме " + rep.FormNum_DB + " за период " + rep.StartPeriod_DB + "-" + rep.EndPeriod_DB + ",\n" +
                                            "номер корректировки " + rep.CorrectionNumber_DB + ", количество строк " + rep.Rows.Count + ".\n" +
                                            "Организации:" + "\n" +
                                            "   1.Регистрационный номер  " + item.Master.RegNoRep.Value + "\n" +
                                            "   2.Сокращенное наименование  " + item.Master.ShortJurLicoRep.Value + "\n" +
                                            "   3.ОКПО  " + item.Master.OkpoRep.Value + "\n"; ;
                                        an = await ShowMessage.Handle(new List<string>() { str, "Новая организация", "Ок", "Пропустить для всех" });
                                        if (an == "Пропустить для всех") skipAll = true;
                                    }
                                }
                                else
                                {
                                    Local_Reports.Reports_Collection.Add(item);
                                }
                                if (an == "Пропустить для всех" || an == "Ок" || skipAll)
                                {
                                    Local_Reports.Reports_Collection.Add(item);
                                }
                            }
                        }
                        await Local_Reports.Reports_Collection.QuickSortAsync();
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
                                                filename2 += "_" + rp.Master.OkpoRep.Value;

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
                        ChangeOrCreateVM frm = new(numForm, rep, tre, Local_Reports);
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
                                Dictionary<long, List<string>> dic = new Dictionary<long, List<string>>();
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
                        ChangeOrCreateVM frm = new(rep.Master.FormNum.Value, rep.Master, rep, Local_Reports);
                        await ShowDialog.Handle(frm);

                        //Local_Reports.Reports_Collection.Sorted = false;
                        //await Local_Reports.Reports_Collection.QuickSortAsync();
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
                var answ = (string)await ShowMessage.Handle(new List<string>() { "Вы действительно хотите удалить отчет?", "Уведомление", "Да", "Нет" });
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
                var answ = (string)await ShowMessage.Handle(new List<string>() { "Вы действительно хотите удалить организацию?", "Уведомление", "Да", "Нет" });
                if (answ == "Да")
                {
                    if (param != null)
                        foreach (var item in param)
                            Local_Reports.Reports_Collection.Remove((Reports)item);

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
            var param = par as IEnumerable;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                await StaticConfiguration.DBModel.SaveChangesAsync();
                await Local_Reports.Reports_Collection.QuickSortAsync();
            }
        }
        #endregion

        #region Excel
        private string StringReverse(string _string)
        {
            var charArray = _string.Replace("_", "0").Split(".");
            Array.Reverse(charArray);
            return string.Join("", charArray);
        }

        #region StatisticExcelExport
        public ReactiveCommand<Unit, Unit> Statistic_Excel_Export { get; private set; }
        private async Task _Statistic_Excel_Export()
        {
            var find_rep = 0;
            foreach (Reports reps in Local_Reports.Reports_Collection)
            {
                foreach (Report rep in reps.Report_Collection)
                {
                    if (rep.FormNum_DB.Split('.')[0] == "1")
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

                                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                                    excelPackage.Workbook.Properties.Title = "Report";
                                    excelPackage.Workbook.Properties.Created = DateTime.Now;

                                    if (Local_Reports.Reports_Collection.Count > 0)
                                    {
                                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Статистика");
                                        worksheet.Cells[1, 1].Value = "Рег.№";
                                        worksheet.Cells[1, 2].Value = "ОКПО";
                                        worksheet.Cells[1, 3].Value = "Сокращенное наименование";
                                        worksheet.Cells[1, 4].Value = "Форма";
                                        worksheet.Cells[1, 5].Value = "Начало предыдущего периода";
                                        worksheet.Cells[1, 6].Value = "Конец предыдущего периода";
                                        worksheet.Cells[1, 7].Value = "Начало периода";
                                        worksheet.Cells[1, 8].Value = "Конец периода";
                                        worksheet.Cells[1, 9].Value = "Зона разрыва";
                                        worksheet.Cells[1, 10].Value = "Вид несоответствия";

                                        var lst = new List<Reports>();
                                        var listSotrRep = new List<ReportForSort>();

                                        foreach (Reports item in Local_Reports.Reports_Collection)
                                        {
                                            if (item.Master_DB.FormNum_DB.Split('.')[0] == "1")
                                            {
                                                lst.Add(item);


                                                foreach (Report rep in item.Report_Collection)
                                                {
                                                    var start = StringReverse(rep.StartPeriod_DB);
                                                    var end = StringReverse(rep.EndPeriod_DB);
                                                    listSotrRep.Add(new ReportForSort()
                                                    {
                                                        RegNoRep = item.Master_DB.RegNoRep.Value == null ? "" : item.Master_DB.RegNoRep.Value,
                                                        OkpoRep = item.Master_DB.OkpoRep.Value == null ? "" : item.Master_DB.OkpoRep.Value,
                                                        FormNum = rep.FormNum_DB,
                                                        StartPeriod = Convert.ToInt64(start),
                                                        EndPeriod = Convert.ToInt64(end),
                                                        ShortYr = item.Master_DB.ShortJurLicoRep.Value
                                                    });
                                                }
                                            }
                                        }

                                        var newGen = listSotrRep.GroupBy(x => x.RegNoRep).ToDictionary(gr => gr.Key, gr => gr.ToList().GroupBy(x => x.FormNum).ToDictionary(gr => gr.Key, gr => gr.ToList().OrderBy(elem => elem.EndPeriod)));
                                        var row = 2;
                                        foreach (var grp in newGen)
                                        {
                                            foreach (var gr in grp.Value)
                                            {
                                                var prev_end = gr.Value.FirstOrDefault().EndPeriod;
                                                var prev_start = gr.Value.FirstOrDefault().StartPeriod;
                                                var newGr = gr.Value.Skip(1).ToList();
                                                foreach (var g in newGr)
                                                {
                                                    if (g.StartPeriod != prev_end && g.StartPeriod != prev_start && g.EndPeriod != prev_end)
                                                    {
                                                        if (g.StartPeriod < prev_end)
                                                        {
                                                            var prev_end_n = prev_end.ToString().Length == 8 ? prev_end.ToString() : prev_end == 0 ? "нет даты конца периода" : prev_end.ToString().Insert(6, "0");
                                                            var prev_start_n = prev_start.ToString().Length == 8 ? prev_start.ToString() : prev_start == 0 ? "нет даты начала периода" : prev_start.ToString().Insert(6, "0");


                                                            var st_per = g.StartPeriod.ToString().Length == 8 ? g.StartPeriod.ToString() : g.StartPeriod.ToString().Insert(6, "0");
                                                            var end_per = g.EndPeriod.ToString().Length == 8 ? g.EndPeriod.ToString() : g.EndPeriod.ToString().Insert(6, "0");

                                                            worksheet.Cells[row, 1].Value = g.RegNoRep;
                                                            worksheet.Cells[row, 2].Value = g.OkpoRep;
                                                            worksheet.Cells[row, 3].Value = g.ShortYr;
                                                            worksheet.Cells[row, 4].Value = g.FormNum;

                                                            worksheet.Cells[row, 5].Value = prev_start_n.Equals("нет даты начала периода") ? prev_start_n : $"{prev_start_n[6..8]}.{prev_start_n[4..6]}.{prev_start_n[0..4]}";
                                                            worksheet.Cells[row, 6].Value = prev_end_n.Equals("нет даты конца периода") ? prev_end_n : $"{prev_end_n[6..8]}.{prev_end_n[4..6]}.{prev_end_n[0..4]}";

                                                            worksheet.Cells[row, 7].Value = $"{st_per[6..8]}.{st_per[4..6]}.{st_per[0..4]}";
                                                            worksheet.Cells[row, 8].Value = $"{end_per[6..8]}.{end_per[4..6]}.{end_per[0..4]}";

                                                            worksheet.Cells[row, 9].Value = $"{worksheet.Cells[row, 6].Value}-{worksheet.Cells[row, 7].Value}";

                                                            worksheet.Cells[row, 10].Value = "пересечение";

                                                            row++;
                                                        }
                                                        else
                                                        {
                                                            var prev_end_n = prev_end.ToString().Length == 8 ? prev_end.ToString() : prev_end == 0 ? "нет даты конца периода" : prev_end.ToString().Insert(6, "0");
                                                            var prev_start_n = prev_start.ToString().Length == 8 ? prev_start.ToString() : prev_start == 0 ? "нет даты начала периода" : prev_start.ToString().Insert(6, "0");

                                                            var st_per = g.StartPeriod.ToString().Length == 8 ? g.StartPeriod.ToString() : g.StartPeriod.ToString().Insert(6, "0");
                                                            var end_per = g.EndPeriod.ToString().Length == 8 ? g.EndPeriod.ToString() : g.EndPeriod.ToString().Insert(6, "0");

                                                            worksheet.Cells[row, 1].Value = g.RegNoRep;
                                                            worksheet.Cells[row, 2].Value = g.OkpoRep;
                                                            worksheet.Cells[row, 3].Value = g.ShortYr;
                                                            worksheet.Cells[row, 4].Value = g.FormNum;

                                                            worksheet.Cells[row, 5].Value = prev_start_n.Equals("нет даты начала периода") ? prev_start_n : $"{prev_start_n[6..8]}.{prev_start_n[4..6]}.{prev_start_n[0..4]}";
                                                            worksheet.Cells[row, 6].Value = prev_end_n.Equals("нет даты конца периода") ? prev_end_n : $"{prev_end_n[6..8]}.{prev_end_n[4..6]}.{prev_end_n[0..4]}";

                                                            worksheet.Cells[row, 7].Value = $"{st_per[6..8]}.{st_per[4..6]}.{st_per[0..4]}";
                                                            worksheet.Cells[row, 8].Value = $"{end_per[6..8]}.{end_per[4..6]}.{end_per[0..4]}";

                                                            worksheet.Cells[row, 9].Value = $"{worksheet.Cells[row, 6].Value}-{worksheet.Cells[row, 7].Value}";

                                                            worksheet.Cells[row, 10].Value = "разрыв";

                                                            row++;
                                                        }
                                                    }

                                                    prev_end = g.EndPeriod;
                                                    prev_start = g.StartPeriod;
                                                }
                                            }
                                        }
                                        worksheet.Column(1).AutoFit();
                                        worksheet.Column(2).AutoFit();
                                        worksheet.Column(4).AutoFit();
                                        worksheet.Column(5).AutoFit();
                                        worksheet.Column(6).AutoFit();
                                        worksheet.Column(7).AutoFit();
                                        worksheet.Column(8).AutoFit();
                                        worksheet.Column(9).AutoFit();
                                        worksheet.Column(10).AutoFit();

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
                                        await form!.SortAsync();
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
            bool forSelectedOrg = false;
            bool AllForms1 = false;
            bool AllForms2 = false;
            if (par.ToString().Contains("Org"))
            {
                forSelectedOrg = true;
                if ()
            }
            var param = Regex.Replace(par.ToString(), "[\\d.]", "");

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
                    var mainWindow = desktop.MainWindow as MainWindow;
                    var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
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
                                    using ExcelPackage excelPackage = new(new FileInfo(path));
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
                                            masterheaderlength = Form10.ExcelHeader(worksheet, 1, 1, ID: "ID") + 1;
                                            masterheaderlength = Form10.ExcelHeader(worksheetPrim, 1, 1, ID: "ID") + 1;
                                        }
                                        else
                                        {
                                            masterheaderlength = Form20.ExcelHeader(worksheet, 1, 1, ID: "ID") + 1;
                                            masterheaderlength = Form20.ExcelHeader(worksheetPrim, 1, 1, ID: "ID") + 1;
                                        }
                                        var t = Report.ExcelHeader(worksheet, param, 1, masterheaderlength);
                                        Report.ExcelHeader(worksheetPrim, param, 1, masterheaderlength);
                                        masterheaderlength += t;
                                        masterheaderlength--;
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
                                            var newItem = item.Report_Collection.Where(x => x.FormNum_DB.Equals(param));
                                            lst.AddRange(newItem);
                                        }

                                        //foreach (Reports item in Local_Reports.Reports_Collection)
                                        //{
                                        //    lst.AddRange(item.Report_Collection);
                                        //}

                                        _Excel_Export_Rows(param, tyu, masterheaderlength, worksheet, lst, true);
                                        _Excel_Export_Notes(param, tyu, masterheaderlength, worksheetPrim, lst, true);

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
                int l = 10;
            }
        }
        #endregion

        #region AllForms1_Excel_Export
        public ReactiveCommand<Unit, Unit> AllForms1_Excel_Export { get; private set; }
        private async Task _AllForms1_Excel_Export()
        {
            var find_rep = 0;
            foreach (Reports reps in Local_Reports.Reports_Collection)
            {
                foreach (Report rep in reps.Report_Collection)
                {
                    if (rep.FormNum_DB.Split('.')[0] == "1")
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

                                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                                    excelPackage.Workbook.Properties.Title = "Report";
                                    excelPackage.Workbook.Properties.Created = DateTime.Now;

                                    if (Local_Reports.Reports_Collection.Count > 0)
                                    {
                                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм");
                                        worksheet.Cells[1, 1].Value = "Рег.№";
                                        worksheet.Cells[1, 2].Value = "ОКПО";
                                        worksheet.Cells[1, 3].Value = "Форма";
                                        worksheet.Cells[1, 4].Value = "Дата начала";
                                        worksheet.Cells[1, 5].Value = "Дата конца";
                                        worksheet.Cells[1, 6].Value = "Номер кор.";
                                        worksheet.Cells[1, 7].Value = "Количество строк";

                                        var lst = new List<Reports>();

                                        foreach (Reports item in Local_Reports.Reports_Collection)
                                        {
                                            if (item.Master_DB.FormNum_DB.Split('.')[0] == "1")
                                            {
                                                lst.Add(item);
                                            }
                                        }

                                        var row = 2;
                                        foreach (Reports reps in lst)
                                        {
                                            foreach (Report rep in reps.Report_Collection)
                                            {
                                                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                                                worksheet.Cells[row, 2].Value = reps.Master.OkpoRep.Value;
                                                worksheet.Cells[row, 3].Value = rep.FormNum_DB;
                                                worksheet.Cells[row, 4].Value = rep.StartPeriod_DB;
                                                worksheet.Cells[row, 5].Value = rep.EndPeriod_DB;
                                                worksheet.Cells[row, 6].Value = rep.CorrectionNumber_DB;
                                                worksheet.Cells[row, 7].Value = rep.Rows.Count;
                                                row++;
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
            catch (Exception ex)
            {
                int l = 10;
            }
        }
        #endregion

        #region AllForms2_Excel_Export
        public ReactiveCommand<Unit, Unit> AllForms2_Excel_Export { get; private set; }
        private async Task _AllForms2_Excel_Export()
        {
            var find_rep = 0;
            foreach (Reports reps in Local_Reports.Reports_Collection)
            {
                foreach (Report rep in reps.Report_Collection)
                {
                    if (rep.FormNum_DB.Split('.')[0] == "2")
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

                                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                                    excelPackage.Workbook.Properties.Title = "Report";
                                    excelPackage.Workbook.Properties.Created = DateTime.Now;

                                    if (Local_Reports.Reports_Collection.Count > 0)
                                    {
                                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Список всех форм");
                                        worksheet.Cells[1, 1].Value = "Рег.№";
                                        worksheet.Cells[1, 2].Value = "ОКПО";
                                        worksheet.Cells[1, 3].Value = "Форма";
                                        worksheet.Cells[1, 4].Value = "Отчетный год";
                                        worksheet.Cells[1, 5].Value = "Номер кор.";
                                        worksheet.Cells[1, 6].Value = "Количество строк";

                                        var lst = new List<Reports>();

                                        foreach (Reports item in Local_Reports.Reports_Collection)
                                        {
                                            if (item.Master_DB.FormNum_DB.Split('.')[0] == "2")
                                            {
                                                lst.Add(item);
                                            }
                                            var gr = item.Report_Collection.GroupBy(x => x.FormNum_DB);
                                        }

                                        var row = 2;
                                        foreach (Reports reps in lst)
                                        {
                                            foreach (Report rep in reps.Report_Collection)
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
                int l = 10;
            }
        }
        #endregion

        #region AllOrganization_Excel_Export
        public ReactiveCommand<Unit, Unit> AllOrganization_Excel_Export { get; private set; }
        private async Task _AllOrganization_Excel_Export()
        {
            var find_reps = Local_Reports.Reports_Collection.Count;

            if (find_reps == 0) return;
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

                                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                                    excelPackage.Workbook.Properties.Title = "Report";
                                    excelPackage.Workbook.Properties.Created = DateTime.Now;

                                    if (Local_Reports.Reports_Collection.Count > 0)
                                    {
                                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Список всех организаций");
                                        worksheet.Cells[1, 1].Value = "Рег.№";
                                        worksheet.Cells[1, 2].Value = "Регион";
                                        worksheet.Cells[1, 3].Value = "ОКПО";
                                        worksheet.Cells[1, 4].Value = "Сокращенное наименование";
                                        worksheet.Cells[1, 5].Value = "Адрес";
                                        worksheet.Cells[1, 6].Value = "ИНН";
                                        worksheet.Cells[1, 7].Value = "Форма 1.1";
                                        worksheet.Cells[1, 8].Value = "Форма 1.2";
                                        worksheet.Cells[1, 9].Value = "Форма 1.3";
                                        worksheet.Cells[1, 10].Value = "Форма 1.4";
                                        worksheet.Cells[1, 11].Value = "Форма 1.5";
                                        worksheet.Cells[1, 12].Value = "Форма 1.6";
                                        worksheet.Cells[1, 13].Value = "Форма 1.7";
                                        worksheet.Cells[1, 14].Value = "Форма 1.8";
                                        worksheet.Cells[1, 15].Value = "Форма 1.9";
                                        worksheet.Cells[1, 16].Value = "Форма 2.1";
                                        worksheet.Cells[1, 17].Value = "Форма 2.2";
                                        worksheet.Cells[1, 18].Value = "Форма 2.3";
                                        worksheet.Cells[1, 19].Value = "Форма 2.4";
                                        worksheet.Cells[1, 20].Value = "Форма 2.5";
                                        worksheet.Cells[1, 21].Value = "Форма 2.6";
                                        worksheet.Cells[1, 22].Value = "Форма 2.7";
                                        worksheet.Cells[1, 23].Value = "Форма 2.8";
                                        worksheet.Cells[1, 24].Value = "Форма 2.9";
                                        worksheet.Cells[1, 25].Value = "Форма 2.10";
                                        worksheet.Cells[1, 26].Value = "Форма 2.11";
                                        worksheet.Cells[1, 27].Value = "Форма 2.12";

                                        var lst = new List<Reports>();
                                        var chekedLst = new List<Reports>();

                                        foreach (Reports item in Local_Reports.Reports_Collection)
                                        {
                                            lst.Add(item);
                                        }
                                        var row = 2;
                                        foreach (Reports reps in lst)
                                        {
                                            if (chekedLst.FirstOrDefault(x => x.Master_DB.RegNoRep == reps.Master_DB.RegNoRep) != null)
                                            {
                                                row--;
                                                worksheet.Cells[row, 7].Value = (int)worksheet.Cells[row, 7].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1")).Count();
                                                worksheet.Cells[row, 8].Value = (int)worksheet.Cells[row, 8].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.2")).Count();
                                                worksheet.Cells[row, 9].Value = (int)worksheet.Cells[row, 9].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.3")).Count();
                                                worksheet.Cells[row, 10].Value = (int)worksheet.Cells[row, 10].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.4")).Count();
                                                worksheet.Cells[row, 11].Value = (int)worksheet.Cells[row, 11].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.5")).Count();
                                                worksheet.Cells[row, 12].Value = (int)worksheet.Cells[row, 12].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.6")).Count();
                                                worksheet.Cells[row, 13].Value = (int)worksheet.Cells[row, 13].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.7")).Count();
                                                worksheet.Cells[row, 14].Value = (int)worksheet.Cells[row, 14].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.8")).Count();
                                                worksheet.Cells[row, 15].Value = (int)worksheet.Cells[row, 15].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.9")).Count();
                                                worksheet.Cells[row, 16].Value = (int)worksheet.Cells[row, 16].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.1")).Count();
                                                worksheet.Cells[row, 17].Value = (int)worksheet.Cells[row, 17].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.2")).Count();
                                                worksheet.Cells[row, 18].Value = (int)worksheet.Cells[row, 18].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.3")).Count();
                                                worksheet.Cells[row, 19].Value = (int)worksheet.Cells[row, 19].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.4")).Count();
                                                worksheet.Cells[row, 20].Value = (int)worksheet.Cells[row, 20].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.5")).Count();
                                                worksheet.Cells[row, 21].Value = (int)worksheet.Cells[row, 21].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.6")).Count();
                                                worksheet.Cells[row, 22].Value = (int)worksheet.Cells[row, 22].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.7")).Count();
                                                worksheet.Cells[row, 23].Value = (int)worksheet.Cells[row, 23].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.8")).Count();
                                                worksheet.Cells[row, 24].Value = (int)worksheet.Cells[row, 24].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.9")).Count();
                                                worksheet.Cells[row, 25].Value = (int)worksheet.Cells[row, 25].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.10")).Count();
                                                worksheet.Cells[row, 26].Value = (int)worksheet.Cells[row, 26].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.11")).Count();
                                                worksheet.Cells[row, 27].Value = (int)worksheet.Cells[row, 27].Value + reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.12")).Count();
                                                row++;
                                            }
                                            else
                                            {
                                                var inn = !string.IsNullOrEmpty(reps.Master.Rows10[0].Inn_DB) ? reps.Master.Rows10[0].Inn_DB :
                                                          !string.IsNullOrEmpty(reps.Master.Rows10[1].Inn_DB) ? reps.Master.Rows10[1].Inn_DB :
                                                          !string.IsNullOrEmpty(reps.Master.Rows20[0].Inn_DB) ? reps.Master.Rows20[0].Inn_DB :
                                                          reps.Master.Rows20[1].Inn_DB;

                                                var address = !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoFactAddress_DB) && !reps.Master.Rows10[1].JurLicoFactAddress_DB.Equals("-") ? reps.Master.Rows10[1].JurLicoFactAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoFactAddress_DB) && !reps.Master.Rows20[1].JurLicoFactAddress_DB.Equals("-") ? reps.Master.Rows20[1].JurLicoFactAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows10[1].JurLicoAddress_DB) && !reps.Master.Rows10[1].JurLicoAddress_DB.Equals("-") ? reps.Master.Rows10[1].JurLicoAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows20[1].JurLicoAddress_DB) && !reps.Master.Rows20[1].JurLicoAddress_DB.Equals("-") ? reps.Master.Rows20[1].JurLicoAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoFactAddress_DB) && !reps.Master.Rows10[0].JurLicoFactAddress_DB.Equals("-") ? reps.Master.Rows10[0].JurLicoFactAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows20[0].JurLicoFactAddress_DB) && !reps.Master.Rows20[0].JurLicoFactAddress_DB.Equals("-") ? reps.Master.Rows20[0].JurLicoFactAddress_DB :
                                                         !string.IsNullOrEmpty(reps.Master.Rows10[0].JurLicoAddress_DB) && !reps.Master.Rows10[0].JurLicoAddress_DB.Equals("-") ? reps.Master.Rows10[0].JurLicoAddress_DB :
                                                         reps.Master.Rows20[0].JurLicoAddress_DB;

                                                worksheet.Cells[row, 1].Value = reps.Master.RegNoRep.Value;
                                                worksheet.Cells[row, 2].Value = reps.Master.RegNoRep.Value.Length >= 2 ? reps.Master.RegNoRep.Value.Substring(0, 2) : reps.Master.RegNoRep.Value;
                                                worksheet.Cells[row, 3].Value = reps.Master.OkpoRep.Value;
                                                worksheet.Cells[row, 4].Value = reps.Master.ShortJurLicoRep.Value;
                                                worksheet.Cells[row, 5].Value = address;
                                                worksheet.Cells[row, 6].Value = inn;
                                                worksheet.Cells[row, 7].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1")).Count();
                                                worksheet.Cells[row, 8].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.2")).Count();
                                                worksheet.Cells[row, 9].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.3")).Count();
                                                worksheet.Cells[row, 10].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.4")).Count();
                                                worksheet.Cells[row, 11].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.5")).Count();
                                                worksheet.Cells[row, 12].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.6")).Count();
                                                worksheet.Cells[row, 13].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.7")).Count();
                                                worksheet.Cells[row, 14].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.8")).Count();
                                                worksheet.Cells[row, 15].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.9")).Count();
                                                worksheet.Cells[row, 16].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.1")).Count();
                                                worksheet.Cells[row, 17].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.2")).Count();
                                                worksheet.Cells[row, 18].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.3")).Count();
                                                worksheet.Cells[row, 19].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.4")).Count();
                                                worksheet.Cells[row, 20].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.5")).Count();
                                                worksheet.Cells[row, 21].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.6")).Count();
                                                worksheet.Cells[row, 22].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.7")).Count();
                                                worksheet.Cells[row, 23].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.8")).Count();
                                                worksheet.Cells[row, 24].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.9")).Count();
                                                worksheet.Cells[row, 25].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.10")).Count();
                                                worksheet.Cells[row, 26].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.11")).Count();
                                                worksheet.Cells[row, 27].Value = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.12")).Count();
                                                row++;
                                                chekedLst.Add(reps);
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
            catch (Exception ex)
            {
                int l = 10;
            }
        }
        #endregion

        #region Pasports
        #region ExcelMissingPas
        public ReactiveCommand<object, Unit> ExcelMissingPas { get; protected set; }
        private async Task _ExcelMissingPas(object param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog saveFileDialog = new();
                FileDialogFilter filter = new() { Name = "Excel", Extensions = { "xlsx" } };
                saveFileDialog.Filters.Add(filter);
                var res = await saveFileDialog.ShowAsync(desktop.MainWindow);
                if (res != null)
                {
                    if (res.Length != 0)
                    {
                        var path = res;
                        if (!path.Contains(".xlsx"))
                        {
                            path += ".xlsx";
                        }
                        if (File.Exists(path))
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { "Не удалось сохранить файл по пути: " + path + Environment.NewLine
                                    + "Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                                return;
                            }
                        }
                        if (path != null)
                        {
                            using ExcelPackage excelPackage = new(new FileInfo(path));
                            excelPackage.Workbook.Properties.Author = "RAO_APP";
                            excelPackage.Workbook.Properties.Title = "Report";
                            excelPackage.Workbook.Properties.Created = DateTime.Now;
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add($"Список отчётов без файла паспорта");

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
                            #endregion

                            List<string> pasNames = new();
                            List<string[]> pasUniqParam = new();
                            DirectoryInfo directory = new(@"C:\Test");
                            FileInfo[] Files;
                            try
                            {
                                Files = directory.GetFiles("*#*#*#*#*.pdf");
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { $"Не удалось открыть сетевое хранилище паспортов:"
                                        + Environment.NewLine + directory.FullName, "Ошибка", "Ок" });
                                return;
                            }
                            pasNames.AddRange(Files.Select(file => file.Name.Remove(file.Name.Length - 4)));
                            pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

                            int currentRow = 2;
                            bool findPasFile = false;
                            foreach (Reports reps in Local_Reports.Reports_Collection10)
                            {
                                var form11 = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
                                foreach (Report rep in form11)
                                {
                                    List<Form11> repPas = rep.Rows11.Where(x => x.OperationCode_DB == "11"
                                    && (x.Category_DB == 1 || x.Category_DB == 2 || x.Category_DB == 3)).ToList();

                                    foreach (Form11 repForm in repPas)
                                    {
                                        findPasFile = false;
                                        foreach (string[] pasParam in pasUniqParam)
                                        {
                                            if (ComparePasParam(repForm.CreatorOKPO_DB, pasParam[0])
                                                && ComparePasParam(repForm.Type_DB, pasParam[1])
                                                && ComparePasParam(ConvertDBDateToYear(repForm.CreationDate_DB), pasParam[2])
                                                && ComparePasParam(ConvertPasNumAndFactNum(repForm.PassportNumber_DB), pasParam[3])
                                                && ComparePasParam(ConvertPasNumAndFactNum(repForm.FactoryNumber_DB), pasParam[4]))
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
                                            #endregion
                                            currentRow++;
                                        }
                                    }
                                }
                            }
                            try
                            {
                                excelPackage.Save();
                                res = await ShowMessage.Handle(new List<string>() { "Выгрузка всех записей паспортов с кодом 11 категорий 1, 2, 3,"
                                        + Environment.NewLine + $"для которых отсутствуют файлы паспортов по пути: {directory.FullName}"
                                        + Environment.NewLine + $"сохранена по пути:" + Environment.NewLine + path, "", "Ок", "Открыть выгрузку" });
                                if (res is null || res.Equals("Ок"))
                                    return;
                                if (res.Equals("Открыть выгрузку"))
                                {
                                    ProcessStartInfo procInfo = new() { FileName = path, UseShellExecute = true };
                                    Process.Start(procInfo);
                                }
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { $"Не удалось сохранить файл по указанному пути", "Ок" });
                                return;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region ExcelPasWithoutRep
        public ReactiveCommand<object, Unit> ExcelPasWithoutRep { get; protected set; }
        private async Task _ExcelPasWithoutRep(object param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog saveFileDialog = new();
                FileDialogFilter filter = new() { Name = "Excel", Extensions = { "xlsx" } };
                saveFileDialog.Filters.Add(filter);
                var messageBoxWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxInputWindow(new MessageBox.Avalonia.DTO.MessageBoxInputParams
                    {
                        ButtonDefinitions = new[]
                        {
                            new ButtonDefinition {Name = "Ок", IsDefault = true},
                            new ButtonDefinition {Name = "Отмена", IsCancel = true}
                        },
                        ContentTitle = "Выбор категории",
                        ContentMessage = "Введите через запятую номера категорий (допускается несколько значений)",
                        MinWidth = 600,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    });
                var result = await messageBoxWindow.ShowDialog(desktop.MainWindow);
                List<short?> categories = new() { 1, 2, 3, 4, 5 };
                try
                {
                    if (!result.Button.Equals("Ок"))
                        throw new Exception();
                    categories = Regex.Replace(result.Message, "[^\\d,]", "").Split(',').Select((short.Parse)).Cast<short?>().ToList();
                }
                catch (Exception e)
                {
                    await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Уведомление", "Номера категорий не были введены, либо были введены некорректно"
                        + Environment.NewLine + "Выгрузка будет осуществленна по всем категориям").ShowDialog(desktop.MainWindow);
                }
                var res = await saveFileDialog.ShowAsync(desktop.MainWindow);
                if (res != null)
                {
                    if (res.Length != 0)
                    {
                        var path = res;
                        if (!path.Contains(".xlsx"))
                        {
                            path += ".xlsx";
                        }
                        if (File.Exists(path))
                        {
                            try
                            {
                                File.Delete(path);
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { "Не удалось сохранить файл по пути: " + path + Environment.NewLine
                                    + "Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                                return;
                            }
                        }
                        if (path != null)
                        {
                            using ExcelPackage excelPackage = new(new FileInfo(path));
                            excelPackage.Workbook.Properties.Author = "RAO_APP";
                            excelPackage.Workbook.Properties.Title = "Report";
                            excelPackage.Workbook.Properties.Created = DateTime.Now;
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add($"Список паспортов без отчётов");

                            worksheet.Cells[1, 1].Value = "Полное имя файла";
                            worksheet.Cells[1, 2].Value = "Код ОКПО изготовителя";
                            worksheet.Cells[1, 3].Value = "Тип";
                            worksheet.Cells[1, 4].Value = "Год выпуска";
                            worksheet.Cells[1, 5].Value = "Номер паспорта";
                            worksheet.Cells[1, 6].Value = "Номер";

                            List<string> pasNames = new();
                            List<string[]> pasUniqParam = new();
                            DirectoryInfo directory = new(@"C:\Test");
                            FileInfo[] Files;
                            try
                            {
                                Files = directory.GetFiles("*#*#*#*#*.pdf");
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { $"Не удалось открыть сетевое хранилище паспортов:"
                                        + Environment.NewLine + directory.FullName, "Ошибка", "Ок" });
                                return;
                            }
                            pasNames.AddRange(Files.Select(file => file.Name.Remove(file.Name.Length - 4)));
                            pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

                            foreach (Reports reps in Local_Reports.Reports_Collection10)
                            {
                                var form11 = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
                                foreach (Report rep in form11)
                                {
                                    List<Form11> repPas = rep.Rows11.Where(x => x.OperationCode_DB == "11" && categories.Contains(x.Category_DB)).ToList();
                                    foreach (Form11 repForm in repPas)
                                    {
                                        foreach (string[] pasParam in pasUniqParam)
                                        {
                                            if (ComparePasParam(repForm.CreatorOKPO_DB, pasParam[0])
                                                && ComparePasParam(repForm.Type_DB, pasParam[1])
                                                && ComparePasParam(ConvertDBDateToYear(repForm.CreationDate_DB), pasParam[2])
                                                && ComparePasParam(ConvertPasNumAndFactNum(repForm.PassportNumber_DB), pasParam[3])
                                                && ComparePasParam(ConvertPasNumAndFactNum(repForm.FactoryNumber_DB), pasParam[4]))
                                            {
                                                pasNames.Remove(pasParam[0] + '#' + pasParam[1] + '#' + pasParam[2] + '#' + pasParam[3] + '#' + pasParam[4]);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            int currentRow = 2;
                            foreach (string pasName in pasNames)
                            {
                                worksheet.Cells[currentRow, 1].Value = pasName;
                                worksheet.Cells[currentRow, 2].Value = pasName.Split('#')[0];
                                worksheet.Cells[currentRow, 3].Value = pasName.Split('#')[1];
                                worksheet.Cells[currentRow, 4].Value = pasName.Split('#')[2];
                                worksheet.Cells[currentRow, 5].Value = pasName.Split('#')[3];
                                worksheet.Cells[currentRow, 6].Value = pasName.Split('#')[4];
                                currentRow++;
                            }
                            worksheet.Cells.AutoFitColumns();
                            try
                            {
                                excelPackage.Save();
                                res = await ShowMessage.Handle(new List<string>() { "Выгрузка всех записей паспортов с кодом 11 категорий 1, 2, 3,"
                                        + Environment.NewLine + $"для которых отсутствуют файлы паспортов по пути: {directory.FullName}"
                                        + Environment.NewLine + $"сохранена по пути:" + Environment.NewLine + path, "", "Ок", "Открыть выгрузку" });
                                if (res is null || res.Equals("Ок"))
                                    return;
                                if (res.Equals("Открыть выгрузку"))
                                {
                                    ProcessStartInfo procInfo = new() { FileName = path, UseShellExecute = true };
                                    Process.Start(procInfo);
                                }
                            }
                            catch (Exception e)
                            {
                                await ShowMessage.Handle(new List<string>() { $"Не удалось сохранить файл по указанному пути", "Ок" });
                                return;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public static bool ComparePasParam(string nameDB, string namePas)
        {
            nameDB = Regex.Replace(nameDB, "[\\\\/:*?\"<>|]", "_");
            nameDB = Regex.Replace(nameDB, "\\s+", "");
            namePas = Regex.Replace(namePas, "[\\\\/:*?\"<>|]", "_");
            namePas = Regex.Replace(namePas, "\\s+", "");

            return nameDB.Equals(namePas, StringComparison.OrdinalIgnoreCase)
                || ChangeOrCreateVM.TransliteToEng(nameDB).Equals(ChangeOrCreateVM.TransliteToEng(namePas), StringComparison.OrdinalIgnoreCase)
                || ChangeOrCreateVM.TransliteToRus(nameDB).Equals(ChangeOrCreateVM.TransliteToRus(namePas), StringComparison.OrdinalIgnoreCase);
        }

        public static string ConvertDBDateToYear(string DBDate)
        {
            Regex r = new(@"(\d{1,2}[.\/]){1,2}\d{4}");
            if (!r.IsMatch(DBDate))
                return "0000";
            var matches = r.Matches(DBDate);
            return matches.FirstOrDefault().Value.Substring(matches.FirstOrDefault().Value.Length - 4);
        }

        public static string ConvertPasNumAndFactNum(string num)
        {
            if (string.IsNullOrEmpty(num)
                || num.Contains("прим", StringComparison.OrdinalIgnoreCase)
                || num.Equals("бн", StringComparison.OrdinalIgnoreCase)
                || num.Equals("бп", StringComparison.OrdinalIgnoreCase)
                || num.Contains("без", StringComparison.OrdinalIgnoreCase)
                || num.Contains("нет", StringComparison.OrdinalIgnoreCase)
                || num.Contains("отсут", StringComparison.OrdinalIgnoreCase))
            {
                return "-";
            }
            else return num;
        }

        #region ChangePasDir
        public ReactiveCommand<object, Unit> ChangePasDir { get; protected set; }
        private async Task _ChangePasDir(object param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                OpenFolderDialog openFolderDialog = new() { DefaultDirectory = defaultPasFolder };
                defaultPasFolder = await openFolderDialog.ShowAsync(desktop.MainWindow) ?? defaultPasFolder;
            }
        }
        #endregion
        #endregion

        #region ExcelExportNotes
        private int _Excel_Export_Notes(string param, int StartRow, int StartColumn, ExcelWorksheet worksheetPrim, List<Report> forms, bool printID = false)
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
                        if (printID)
                        {
                            if (param.Split('.')[0] == "1")
                            {
                                if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                {
                                    yu = reps.Master_DB.Rows10[1].ExcelRow(worksheetPrim, cnty, 1, SumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1;
                                }
                                else
                                {
                                    yu = reps.Master_DB.Rows10[0].ExcelRow(worksheetPrim, cnty, 1, SumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1;
                                }
                            }
                            else
                            {
                                if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                {
                                    yu = reps.Master_DB.Rows20[1].ExcelRow(worksheetPrim, cnty, 1, SumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1;
                                }
                                else
                                {
                                    yu = reps.Master_DB.Rows20[0].ExcelRow(worksheetPrim, cnty, 1, SumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1;
                                }
                            }
                        }
                        else
                        {
                            if (param.Split('.')[0] == "1")
                            {
                                if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                {
                                    yu = reps.Master_DB.Rows10[1].ExcelRow(worksheetPrim, cnty, 1) + 1;
                                }
                                else
                                {
                                    yu = reps.Master_DB.Rows10[0].ExcelRow(worksheetPrim, cnty, 1) + 1;
                                }
                            }
                            else
                            {
                                if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                {
                                    yu = reps.Master_DB.Rows20[1].ExcelRow(worksheetPrim, cnty, 1) + 1;
                                }
                                else
                                {
                                    yu = reps.Master_DB.Rows20[0].ExcelRow(worksheetPrim, cnty, 1) + 1;
                                }
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
        #endregion

        #region ExcelExportRows
        private int _Excel_Export_Rows(string param, int StartRow, int StartColumn, ExcelWorksheet worksheet, List<Report> forms, bool ID = false)
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
                    //List<IKey> lst = t.Count() > 0 ? item[param].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList() : new();
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
                                    ((Form21)(it)).ExcelRow(worksheet, count, StartColumn + 1, SumNumber: ((Form21)it).NumberInOrderSum_DB);
                                }
                                if (it is Form22)
                                {
                                    ((Form22)(it)).ExcelRow(worksheet, count, StartColumn + 1, SumNumber: ((Form22)it).NumberInOrderSum_DB);
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
                                if (ID)
                                {
                                    if (param.Split('.')[0] == "1")
                                    {
                                        if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                        {
                                            yu = reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1, SumNumber: reps.Master_DB.Rows10[1].Id.ToString()) + 1;
                                        }
                                        else
                                        {
                                            yu = reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1, SumNumber: reps.Master_DB.Rows10[0].Id.ToString()) + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                        {
                                            yu = reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1, SumNumber: reps.Master_DB.Rows20[1].Id.ToString()) + 1;
                                        }
                                        else
                                        {
                                            yu = reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1, SumNumber: reps.Master_DB.Rows20[0].Id.ToString()) + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (param.Split('.')[0] == "1")
                                    {
                                        if (mstrep.Rows10[1].RegNo_DB != "" && mstrep.Rows10[1].Okpo_DB != "")
                                        {
                                            yu = reps.Master_DB.Rows10[1].ExcelRow(worksheet, count, 1) + 1;
                                        }
                                        else
                                        {
                                            yu = reps.Master_DB.Rows10[0].ExcelRow(worksheet, count, 1) + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (mstrep.Rows20[1].RegNo_DB != "" && mstrep.Rows20[1].Okpo_DB != "")
                                        {
                                            yu = reps.Master_DB.Rows20[1].ExcelRow(worksheet, count, 1) + 1;
                                        }
                                        else
                                        {
                                            yu = reps.Master_DB.Rows20[0].ExcelRow(worksheet, count, 1) + 1;
                                        }
                                    }
                                }

                                item.ExcelRow(worksheet, count, yu);
                                count++;
                            }
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
                        StartRow = count;
                    }
                }
            }
            return StartRow;
        }
        #endregion

        #region ExcelPrintTitulExport
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
        #endregion

        #region ExcelPrintSubMainExport
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
        #endregion

        #region ExcelPrintNotesExport
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
        #endregion

        #region ExcelPrintRowsExport
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