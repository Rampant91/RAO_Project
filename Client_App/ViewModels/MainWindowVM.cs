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
                var path = Path.GetPathRoot(systemDirectory);
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
                    Current_Db =
                        $"Интерактивное пособие по вводу данных ver.1.2.2.2 Текущая база данных - {names[names.Length - 1]}";
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
                Current_Db = $"Интерактивное пособие по вводу данных ver.1.2.2.2 Текущая база данных - Local_{i}.raodb";
                StaticConfiguration.DBPath = Path.Combine(tempDirectory, $"Local_{i}.raodb");
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
            SelectOrgExcelExport = ReactiveCommand.CreateFromTask(_SelectOrgExcelExport);
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
        private double _OnStartProgressBar;
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
                Form11 form11 = new();
                form11.ExcelGetRow(worksheet1, start);
                repFromEx.Rows11.Add(form11);
            }
            if (param1 == "1.2")
            {
                Form12 form12 = new();
                form12.ExcelGetRow(worksheet1, start);
                repFromEx.Rows12.Add(form12);
            }
            if (param1 == "1.3")
            {
                Form13 form13 = new();
                form13.ExcelGetRow(worksheet1, start);
                repFromEx.Rows13.Add(form13);
            }
            if (param1 == "1.4")
            {
                Form14 form14 = new();
                form14.ExcelGetRow(worksheet1, start);
                repFromEx.Rows14.Add(form14);
            }
            if (param1 == "1.5")
            {
                Form15 form15 = new();
                form15.ExcelGetRow(worksheet1, start);
                repFromEx.Rows15.Add(form15);
            }
            if (param1 == "1.6")
            {
                Form16 form16 = new();
                form16.ExcelGetRow(worksheet1, start);
                repFromEx.Rows16.Add(form16);
            }
            if (param1 == "1.7")
            {
                Form17 form17 = new();
                form17.ExcelGetRow(worksheet1, start);
                repFromEx.Rows17.Add(form17);
            }
            if (param1 == "1.8")
            {
                Form18 form18 = new();
                form18.ExcelGetRow(worksheet1, start);
                repFromEx.Rows18.Add(form18);
            }
            if (param1 == "1.9")
            {
                Form19 form19 = new();
                form19.ExcelGetRow(worksheet1, start);
                repFromEx.Rows19.Add(form19);
            }
            if (param1 == "2.1")
            {
                Form21 form21 = new();
                form21.ExcelGetRow(worksheet1, start);
                repFromEx.Rows21.Add(form21);
            }
            if (param1 == "2.2")
            {
                Form22 form22 = new();
                form22.ExcelGetRow(worksheet1, start);
                repFromEx.Rows22.Add(form22);
            }
            if (param1 == "2.3")
            {
                Form23 form23 = new();
                form23.ExcelGetRow(worksheet1, start);
                repFromEx.Rows23.Add(form23);
            }
            if (param1 == "2.4")
            {
                Form24 form24 = new();
                form24.ExcelGetRow(worksheet1, start);
                repFromEx.Rows24.Add(form24);
            }
            if (param1 == "2.5")
            {
                Form25 form25 = new();
                form25.ExcelGetRow(worksheet1, start);
                repFromEx.Rows25.Add(form25);
            }
            if (param1 == "2.6")
            {
                Form26 form26 = new();
                form26.ExcelGetRow(worksheet1, start);
                repFromEx.Rows26.Add(form26);
            }
            if (param1 == "2.7")
            {
                Form27 form27 = new();
                form27.ExcelGetRow(worksheet1, start);
                repFromEx.Rows27.Add(form27);
            }
            if (param1 == "2.8")
            {
                Form28 form28 = new();
                form28.ExcelGetRow(worksheet1, start);
                repFromEx.Rows28.Add(form28);
            }
            if (param1 == "2.9")
            {
                Form29 form29 = new();
                form29.ExcelGetRow(worksheet1, start);
                repFromEx.Rows29.Add(form29);
            }
            if (param1 == "2.10")
            {
                Form210 form210 = new();
                form210.ExcelGetRow(worksheet1, start);
                repFromEx.Rows210.Add(form210);
            }
            if (param1 == "2.11")
            {
                Form211 form211 = new();
                form211.ExcelGetRow(worksheet1, start);
                repFromEx.Rows211.Add(form211);
            }
            if (param1 == "2.12")
            {
                Form212 form212 = new();
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
                       where Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows10[0].Okpo_DB &&
                             Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows10[0].RegNo_DB
                       select t;
            }
            if (worksheet0.Name == "2.0")
            {
                reps = from Reports t in Local_Reports.Reports_Collection20
                       where Convert.ToString(worksheet0.Cells["B36"].Value) == t.Master.Rows20[0].Okpo_DB &&
                             Convert.ToString(worksheet0.Cells["F6"].Value) == t.Master.Rows20[0].RegNo_DB
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
                newRepsFromExcel.Master_DB = new Report
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
                            using (ExcelPackage excelPackage = new(new FileInfo(res)))
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
                                        timeCreate[0] = $"0{timeCreate[0]}";
                                    }
                                    if (timeCreate[1].Length == 1)
                                    {
                                        timeCreate[1] = $"0{timeCreate[1]}";
                                    }

                                    Reports newRepsFromExcel = await CheckReps(worksheet0);

                                    ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets[1];
                                    var param1 = worksheet1.Name;
                                    var repFromEx = new Report
                                    {
                                        FormNum_DB = param1,
                                        ExportDate_DB = $"{timeCreate[0]}.{timeCreate[1]}.{timeCreate[2]}"
                                    };
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
                                        Note newNote = new();

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
                                                            var str =
                                                                $" Вы пытаетесь загрузить форму с наименьщим номером корректировки - {repFromEx.CorrectionNumber_DB},\nпри текущем значении корректировки - {rep.CorrectionNumber_DB}.\nНомер формы - {repFromEx.FormNum_DB}\nНачало отчетного периода - {repFromEx.StartPeriod_DB}\nКонец отчетного периода - {repFromEx.EndPeriod_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                            var an = await ShowMessage.Handle(new List<string> { str, "Отчет", "OK", "Пропустить для всех" });
                                                            if (an == "Пропустить для всех")
                                                            {
                                                                skipLess = true;
                                                            }
                                                        }
                                                    }
                                                    else if (repFromEx.CorrectionNumber_DB == rep.CorrectionNumber_DB)
                                                    {
                                                        var str =
                                                            $"Совпадение даты в {rep.FormNum_DB} {rep.StartPeriod_DB}-{rep.EndPeriod_DB} .\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\n{newRepsFromExcel.Master.RegNoRep.Value} {newRepsFromExcel.Master.ShortJurLicoRep.Value} {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        var an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
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
                                                                var str =
                                                                    $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nНачало отчетного периода - {repFromEx.StartPeriod_DB}\nКонец отчетного периода - {repFromEx.EndPeriod_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{rep.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {repFromEx.Rows.Count}";
                                                                an = await ShowMessage.Handle(new List<string>
                                                                {str, "Отчет",
                                                        "Загрузить новую",
                                                        "Отмена",
                                                        "Загрузить для все"
                                                        });
                                                                if (an == "Загрузить для всех") skipNew = true;
                                                                an = "Загрузить новую";
                                                            }
                                                            else
                                                            {
                                                                var str =
                                                                    $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nНачало отчетного периода - {repFromEx.StartPeriod_DB}\nКонец отчетного периода - {repFromEx.EndPeriod_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{rep.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {repFromEx.Rows.Count}";
                                                                an = await ShowMessage.Handle(new List<string>
                                                                {str, "Отчет",
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
                                                        var str =
                                                            $"Пересечение даты в {rep.FormNum_DB} {rep.StartPeriod_DB}-{rep.EndPeriod_DB} \n{newRepsFromExcel.Master.RegNoRep.Value} {newRepsFromExcel.Master.ShortJurLicoRep.Value} {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        an = await ShowMessage.Handle(new List<string>
                                                        {str,"Отчет",
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
                                                        var str =
                                                            $"Загрузить новую форму?\nНомер формы - {repFromEx.FormNum_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nНачало отчетного периода - {repFromEx.StartPeriod_DB}\nКонец отчетного периода - {repFromEx.EndPeriod_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
                                                        "Да",
                                                        "Нет",
                                                        "Загрузить для всех"
                                                        });
                                                    }
                                                    else
                                                    {
                                                        var str =
                                                            $"Загрузить новую форму?\nНомер формы - {repFromEx.FormNum_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nНачало отчетного периода - {repFromEx.StartPeriod_DB}\nКонец отчетного периода - {repFromEx.EndPeriod_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
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
                                                            var str =
                                                                $" Вы пытаетесь загрузить форму с наименьщим номером корректировки - {repFromEx.CorrectionNumber_DB},\nпри текущем значении корректировки - {rep.CorrectionNumber_DB}.\nНомер формы - {repFromEx.FormNum_DB}\nОтчетный год - {repFromEx.Year_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                            var an = await ShowMessage.Handle(new List<string> { str, "Отчет", "OK", "Пропустить для всех" });
                                                            if (an == "Пропустить для всех") skipLess = true;
                                                        }
                                                    }
                                                    else if (repFromEx.CorrectionNumber_DB == rep.CorrectionNumber_DB)
                                                    {
                                                        var str =
                                                            $"Совпадение даты в {rep.FormNum_DB} {rep.Year_DB} .\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\n{newRepsFromExcel.Master.RegNoRep.Value} \n{newRepsFromExcel.Master.ShortJurLicoRep.Value} {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        var an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
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
                                                                var str =
                                                                    $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nОтчетный год - {repFromEx.Year_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{rep.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {repFromEx.Rows.Count}";
                                                                an = await ShowMessage.Handle(new List<string>
                                                                {
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
                                                                var str =
                                                                    $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nОтчетный год - {repFromEx.Year_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{rep.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {repFromEx.Rows.Count}";
                                                                an = await ShowMessage.Handle(new List<string>
                                                                {
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
                                                        var str =
                                                            $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nОтчетный год - {repFromEx.Year_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
                                                            "Да",
                                                            "Нет",
                                                            "Загрузить для всех"
                                                        });
                                                        if (an == "Загрузить для всех") _skipNew = true;
                                                        an = "Да";
                                                    }
                                                    else
                                                    {
                                                        var str =
                                                            $"Загрузить новую форму? \nНомер формы - {repFromEx.FormNum_DB}\nОтчетный год - {repFromEx.Year_DB}\nНомер корректировки -{repFromEx.CorrectionNumber_DB}\nРегистрационный номер - {newRepsFromExcel.Master.RegNoRep.Value}\nСокращенное наименование - {newRepsFromExcel.Master.ShortJurLicoRep.Value}\nОКПО - {newRepsFromExcel.Master.OkpoRep.Value}\nКоличество строк - {repFromEx.Rows.Count}";
                                                        an = await ShowMessage.Handle(new List<string>
                                                        {str, "Отчет",
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
                                    var an = await ShowMessage.Handle(new List<string>
                                    {str, "Формат данных",
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
                OpenFileDialog dial = new()
                {
                    AllowMultiple = true
                };
                var filter = new FileDialogFilter
                {
                    Name = Name,
                    Extensions = new List<string>(Extensions)
                };
                dial.Filters = new List<FileDialogFilter> { filter };

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
                file = Path.Combine(tmp, $"file_imp_{count++}.raodb");
            }
            while (File.Exists(file));

            return file;
        }
        private async Task<List<Reports>> GetReportsFromDataBase(string file)
        {
            var lst = new List<Reports>();
            using (DBModel db = new(file))
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
                    IEnumerable<Reports> enumerable()
                    {
                        return Local_Reports.Reports_Collection10.Where(t => 
                            (item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB 
                            && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[0].RegNo_DB 
                            && item.Master.Rows10[1].Okpo_DB == "" || t.Master.Rows10[1].Okpo_DB == "" && item.Master.Rows10[0].Okpo_DB == "") 
                            || (item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB 
                            && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[1].RegNo_DB 
                            && item.Master.Rows10[1].Okpo_DB != ""));
                    }

                    var tb11 = enumerable();
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
                               where (item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB &&
                                      item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[0].RegNo_DB &&
                                      item.Master.Rows20[1].Okpo_DB == "") ||
                                     (item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB &&
                                      item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[1].RegNo_DB &&
                                      item.Master.Rows20[1].Okpo_DB != "")
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
                                    var str =
                                        $" Вы пытаетесь загрузить форму с наименьщим номером корректировки - {it.CorrectionNumber_DB},\nпри текущем значении корректировки - {elem.CorrectionNumber_DB}.\nНомер формы - {it.FormNum_DB}\nНачало отчетного периода - {it.StartPeriod_DB}\nКонец отчетного периода - {it.EndPeriod_DB}\nРегистрационный номер - {first11.Master.RegNoRep.Value}\nСокращенное наименование - {first11.Master.ShortJurLicoRep.Value}\nОКПО - {first11.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                    var an = await ShowMessage.Handle(new List<string> { str, "Отчет", "OK", "Пропустить для всех" });
                                    if (an == "Пропустить для всех")
                                    {
                                        skipLess = true;
                                    }
                                }
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB && it.ExportDate_DB == elem.ExportDate_DB)
                            {
                                var str =
                                    $"Совпадение даты в {elem.FormNum_DB} {elem.StartPeriod_DB}-{elem.EndPeriod_DB} .\nНомер корректировки -{it.CorrectionNumber_DB}\n{first11.Master.RegNoRep.Value} {first11.Master.ShortJurLicoRep.Value} {first11.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                doSomething = true;
                                var an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
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
                                        var str =
                                            $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nНачало отчетного периода - {it.StartPeriod_DB}\nКонец отчетного периода - {it.EndPeriod_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first11.Master.RegNoRep.Value}\nСокращенное наименование - {first11.Master.ShortJurLicoRep.Value}\nОКПО - {first11.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{elem.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {it.Rows.Count}";
                                        doSomething = true;
                                        an = await ShowMessage.Handle(new List<string>
                                        {str, "Отчет",
                                            "Загрузить новую",
                                            "Отмена",
                                            "Загрузить для все"
                                            });
                                        if (an == "Загрузить для всех") skipNew = true;
                                        an = "Загрузить новую";
                                    }
                                    else
                                    {
                                        var str =
                                            $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nНачало отчетного периода - {it.StartPeriod_DB}\nКонец отчетного периода - {it.EndPeriod_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first11.Master.RegNoRep.Value}\nСокращенное наименование - {first11.Master.ShortJurLicoRep.Value}\nОКПО - {first11.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{elem.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {it.Rows.Count}";
                                        doSomething = true;
                                        an = await ShowMessage.Handle(new List<string>
                                        {str, "Отчет",
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
                                    var str =
                                        $"Пересечение даты в {elem.FormNum_DB} {elem.StartPeriod_DB}-{elem.EndPeriod_DB} \n{first11.Master.RegNoRep.Value} {first11.Master.ShortJurLicoRep.Value} {first11.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                    an = await ShowMessage.Handle(new List<string>
                                    {str,"Отчет",
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
                                var str =
                                    $"Загрузить новую форму?\nНомер формы - {it.FormNum_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nНачало отчетного периода - {it.StartPeriod_DB}\nКонец отчетного периода - {it.EndPeriod_DB}\nРегистрационный номер - {first11.Master.RegNoRep.Value}\nСокращенное наименование - {first11.Master.ShortJurLicoRep.Value}\nОКПО - {first11.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
                                    "Да",
                                    "Нет",
                                    "Загрузить для всех"
                                    });
                                an = "Да";
                            }
                            else
                            {
                                var str =
                                    $"Загрузить новую форму?\nНомер формы - {it.FormNum_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nНачало отчетного периода - {it.StartPeriod_DB}\nКонец отчетного периода - {it.EndPeriod_DB}\nРегистрационный номер - {first11.Master.RegNoRep.Value}\nСокращенное наименование - {first11.Master.ShortJurLicoRep.Value}\nОКПО - {first11.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
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
                                    var str =
                                        $" Вы пытаетесь загрузить форму с наименьщим номером корректировки - {it.CorrectionNumber_DB},\nпри текущем значении корректировки - {elem.CorrectionNumber_DB}.\nНомер формы - {it.FormNum_DB}\nОтчетный год - {it.Year_DB}\nРегистрационный номер - {first21.Master.RegNoRep.Value}\nСокращенное наименование - {first21.Master.ShortJurLicoRep.Value}\nОКПО - {first21.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                    var an = await ShowMessage.Handle(new List<string> { str, "Отчет", "OK", "Пропустить для всех" });
                                    if (an == "Пропустить для всех") skipLess = true;
                                }
                            }
                            else if (it.CorrectionNumber_DB == elem.CorrectionNumber_DB && it.ExportDate_DB == elem.ExportDate_DB)
                            {
                                var str =
                                    $"Совпадение даты в {elem.FormNum_DB} {elem.Year_DB} .\nНомер корректировки -{it.CorrectionNumber_DB}\n{first21.Master.RegNoRep.Value} \n{first21.Master.ShortJurLicoRep.Value} {first21.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                var an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
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
                                        var str =
                                            $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nОтчетный год - {it.Year_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first21.Master.RegNoRep.Value}\nСокращенное наименование - {first21.Master.ShortJurLicoRep.Value}\nОКПО - {first21.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{elem.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {it.Rows.Count}";
                                        an = await ShowMessage.Handle(new List<string>
                                        {
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
                                        var str =
                                            $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nОтчетный год - {it.Year_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first21.Master.RegNoRep.Value}\nСокращенное наименование - {first21.Master.ShortJurLicoRep.Value}\nОКПО - {first21.Master.OkpoRep.Value}\nФорма с предыдущим номером корректировки №{elem.CorrectionNumber_DB} будет безвозвратно удалена.\nСделайте резервную копию.\nКоличество строк - {it.Rows.Count}";
                                        an = await ShowMessage.Handle(new List<string>
                                        {
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
                                var str =
                                    $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nОтчетный год - {it.Year_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first21.Master.RegNoRep.Value}\nСокращенное наименование - {first21.Master.ShortJurLicoRep.Value}\nОКПО - {first21.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
                                    "Да",
                                    "Нет",
                                    "Загрузить для всех"
                                });
                                if (an == "Загрузить для всех") _skipNew = true;
                                an = "Да";
                            }
                            else
                            {
                                var str =
                                    $"Загрузить новую форму? \nНомер формы - {it.FormNum_DB}\nОтчетный год - {it.Year_DB}\nНомер корректировки -{it.CorrectionNumber_DB}\nРегистрационный номер - {first21.Master.RegNoRep.Value}\nСокращенное наименование - {first21.Master.ShortJurLicoRep.Value}\nОКПО - {first21.Master.OkpoRep.Value}\nКоличество строк - {it.Rows.Count}";
                                an = await ShowMessage.Handle(new List<string>
                                {str, "Отчет",
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
            if (an is "Сохранить оба" or "Да")
            {
                if (!doSomething)
                    first.Report_Collection.Add(it);
            }
            if (an is "Заменить" or "Загрузить новую")
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
            try
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
                                            var str =
                                                $"Был добавлен отчет по форме {rep.FormNum_DB} за период {rep.StartPeriod_DB}-{rep.EndPeriod_DB},\nномер корректировки {rep.CorrectionNumber_DB}, количество строк {rep.Rows.Count}.\nОрганизации:\n   1.Регистрационный номер  {item.Master.RegNoRep.Value}\n   2.Сокращенное наименование  {item.Master.ShortJurLicoRep.Value}\n   3.ОКПО  {item.Master.OkpoRep.Value}\n"; ;
                                            an = await ShowMessage.Handle(new List<string> { str, "Новая организация", "Ок", "Пропустить для всех" });
                                            if (an == "Пропустить для всех") skipAll = true;
                                        }
                                    }
                                    else
                                    {
                                        Local_Reports.Reports_Collection.Add(item);
                                    }
                                    if (an is "Пропустить для всех" or "Ок" || skipAll)
                                    {
                                        Local_Reports.Reports_Collection.Add(item);
                                    }
                                }
                            }
                            await Local_Reports.Reports_Collection.QuickSortAsync();
                        }
                    }
                    StaticConfiguration.DBModel.SaveChanges();
                }
            }
            catch { }
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
                    OpenFolderDialog dial = new();
                    var res = await dial.ShowAsync(desktop.MainWindow);
                    if (res != null)
                    {
                        foreach (var item in param)
                        {
                            var a = DateTime.Now.Date;
                            var aDay = a.Day.ToString();
                            var aMonth = a.Month.ToString();
                            if (aDay.Length < 2) aDay = $"0{aDay}";
                            if (aMonth.Length < 2) aMonth = $"0{aMonth}";
                            ((Report)item).ExportDate.Value = $"{aDay}.{aMonth}.{a.Year}";
                        }
                        if (res != "")
                        {
                            var dt = DateTime.Now;
                            var filename = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
                            var rep = (Report)obj;

                            var dtDay = dt.Day.ToString();
                            var dtMonth = dt.Month.ToString();
                            if (dtDay.Length < 2) dtDay = $"0{dtDay}";
                            if (dtMonth.Length < 2) dtMonth = $"0{dtMonth}";

                            rep.ExportDate.Value = $"{dtDay}.{dtMonth}.{dt.Year}";
                            var findReports = from Reports t in Local_Reports.Reports_Collection
                                              where t.Report_Collection.Contains(rep)
                                              select t;

                            await StaticConfiguration.DBModel.SaveChangesAsync();

                            var rt = findReports.FirstOrDefault();
                            if (rt != null)
                            {
                                var tmp = Path.Combine(await GetTempDirectory(await GetSystemDirectory()),
                                    $"{filename}_exp.raodb");

                                var tsk = new Task(() =>
                                {
                                    DBModel db = new(tmp);
                                    try
                                    {
                                        Reports rp = new()
                                        {
                                            Master = rt.Master
                                        };
                                        rp.Report_Collection.Add(rep);

                                        db.Database.MigrateAsync();
                                        db.ReportsCollectionDbSet.Add(rp);
                                        db.SaveChangesAsync();

                                        string filename2 = "";
                                        if (rp.Master_DB.FormNum_DB == "1.0")
                                        {
                                            filename2 += rp.Master.RegNoRep.Value;
                                            filename2 += $"_{rp.Master.OkpoRep.Value}";

                                            filename2 += $"_{rep.FormNum_DB}";
                                            filename2 += $"_{rep.StartPeriod_DB}";
                                            filename2 += $"_{rep.EndPeriod_DB}";
                                            filename2 += $"_{rep.CorrectionNumber_DB}";
                                        }
                                        else
                                        {
                                            if (rp.Master.Rows20.Count > 0)
                                            {
                                                filename2 += rp.Master.RegNoRep.Value;
                                                filename2 += $"_{rp.Master.OkpoRep.Value}";

                                                filename2 += $"_{rep.FormNum_DB}";
                                                filename2 += $"_{rep.Year_DB}";
                                                filename2 += $"_{rep.CorrectionNumber_DB}";
                                            }
                                        }

                                        res = Path.Combine(res, $"{filename2}.raodb");


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
                                await tsk.ContinueWith((_) =>
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
                                Dictionary<long, List<string>> dic = new();
                                foreach (var oldR in sumRow)
                                {
                                    dic[oldR.NumberInOrder_DB] = new List<string> { oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB };
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
                var answ = (string)await ShowMessage.Handle(new List<string> { "Вы действительно хотите удалить отчет?", "Уведомление", "Да", "Нет" });
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
                var answ = (string)await ShowMessage.Handle(new List<string> { "Вы действительно хотите удалить организацию?", "Уведомление", "Да", "Нет" });
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
        private ExcelWorksheet worksheet { get; set; }
        private ExcelWorksheet worksheetComment { get; set; }
        private static string StringReverse(string _string)
        {
            var charArray = _string.Replace("_", "0").Replace("/", ".").Split(".");
            Array.Reverse(charArray);
            return string.Join("", charArray);
        }

        #region StatisticExcelExport
        public ReactiveCommand<Unit, Unit> Statistic_Excel_Export { get; private set; }
        private async Task _Statistic_Excel_Export()
        {
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
            if (findRep == 0) return;
            try
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = { "xlsx" }
                    };
                    dial.Filters?.Add(filter);
                    var res = await dial.ShowAsync(desktop.MainWindow);
                    if (!string.IsNullOrEmpty(res))
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
                        using ExcelPackage excelPackage = new(new FileInfo(path));
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
                            var listSortRep = new List<ReportForSort>();
                            foreach (var key in Local_Reports.Reports_Collection)
                            {
                                var item = (Reports)key;
                                if (item.Master_DB.FormNum_DB.Split('.')[0] != "1") continue;
                                lst.Add(item);
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
                                                worksheet.Cells[row, 9].Value = $"{worksheet.Cells[row, 6].Value}-{worksheet.Cells[row, 7].Value}";
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
                                                var st_per = g.StartPeriod.ToString().Length == 8
                                                    ? g.StartPeriod.ToString()
                                                    : g.StartPeriod.ToString().Insert(6, "0");
                                                var end_per = g.EndPeriod.ToString().Length == 8
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
                                                worksheet.Cells[row, 7].Value = $"{st_per[6..8]}.{st_per[4..6]}.{st_per[..4]}";
                                                worksheet.Cells[row, 8].Value = $"{end_per[6..8]}.{end_per[4..6]}.{end_per[..4]}";
                                                worksheet.Cells[row, 9].Value = $"{worksheet.Cells[row, 6].Value}-{worksheet.Cells[row, 7].Value}";
                                                worksheet.Cells[row, 10].Value = "разрыв";
                                                row++;
                                            }
                                        }
                                        prevEnd = g.EndPeriod;
                                        prevStart = g.StartPeriod;
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
            catch (Exception) { }
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
                        Extensions = { "xlsx" }
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
                                    using (ExcelPackage excelPackage = new(new FileInfo(path)))
                                    {
                                        //Set some properties of the Excel document
                                        excelPackage.Workbook.Properties.Author = "RAO_APP";
                                        excelPackage.Workbook.Properties.Title = "Report";
                                        excelPackage.Workbook.Properties.Created = DateTime.Now;

                                        if (forms.Count > 0)
                                        {
                                            ExcelWorksheet worksheet =
                                                excelPackage.Workbook.Worksheets.Add($"Отчеты {param}");
                                            ExcelWorksheet worksheetPrim =
                                                excelPackage.Workbook.Worksheets.Add($"Примечания {param}");

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
                                var pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data"), "Excel"),
                                    $"{param}.xlsx");
#else
                                string pth = Path.Combine(Path.Combine(Path.Combine(Path.GetFullPath(AppContext.BaseDirectory),"data"), "Excel"), param+".xlsx");
#endif
                                if (path != null)
                                {
                                    using (ExcelPackage excelPackage = new(new FileInfo(path), new FileInfo(pth)))
                                    {
                                        var form = (Report)forms.FirstOrDefault();
                                        await form!.SortAsync();
                                        var worksheetTitul =
                                            excelPackage.Workbook.Worksheets[$"{param.Split('.')[0]}.0"];
                                        var worksheetMain =
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
            forSelectedOrg = par.ToString().Contains("Org");
            var param = Regex.Replace(par.ToString(), "[^\\d.]", "");
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
            if (findRep == 0) return;
            try
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var mainWindow = desktop.MainWindow as MainWindow;
                    var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
                    if (selectedReports is null && forSelectedOrg)
                    {
                        #region MessageExcelExportFail
                        await MessageBox.Avalonia.MessageBoxManager
                                            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                            {
                                                ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                                ContentTitle = "Выгрузка в Excel",
                                                ContentMessage = "Выгрузка не выполнена. Не выбрана организация",
                                                MinWidth = 400,
                                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                                            })
                                            .ShowDialog(mainWindow);
                        #endregion
                        return;
                    }
                    SaveFileDialog dial = new();
                    var filter = new FileDialogFilter
                    {
                        Name = "Excel",
                        Extensions = { "xlsx" }
                    };
                    dial.Filters!.Add(filter);
                    if (param != "")
                    {
                        var res = await dial.ShowAsync(desktop.MainWindow);
                        if (!string.IsNullOrEmpty(res))
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
                                catch (Exception)
                                {
                                    #region MessageFailedToSaveFile
                                    await MessageBox.Avalonia.MessageBoxManager
                                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                        {
                                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                            ContentTitle = "Выгрузка в Excel",
                                            ContentMessage = $"Не удалось сохранить файл по пути: {path}{Environment.NewLine}" +
                                                             $"Файл с таким именем уже существует в этом расположении и используется другим процессом.",
                                            MinWidth = 400,
                                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                                        })
                                        .ShowDialog(desktop.MainWindow); 
                                    #endregion
                                    return;
                                }
                            }
                            using ExcelPackage excelPackage = new(new FileInfo(path));
                            excelPackage.Workbook.Properties.Author = "RAO_APP";
                            excelPackage.Workbook.Properties.Title = "Report";
                            excelPackage.Workbook.Properties.Created = DateTime.Now;
                            if (Local_Reports.Reports_Collection.Count > 0)
                            {
                                var worksheet = excelPackage.Workbook.Worksheets.Add($"Отчеты {param}");
                                var worksheetPrim = excelPackage.Workbook.Worksheets.Add($"Примечания {param}");
                                int masterHeaderLength;
                                if (param.Split('.')[0] == "1")
                                {
                                    masterHeaderLength = Form10.ExcelHeader(worksheet, 1, 1, ID: "ID") + 1;
                                    masterHeaderLength = Form10.ExcelHeader(worksheetPrim, 1, 1, ID: "ID") + 1;
                                }
                                else
                                {
                                    masterHeaderLength = Form20.ExcelHeader(worksheet, 1, 1, ID: "ID") + 1;
                                    masterHeaderLength = Form20.ExcelHeader(worksheetPrim, 1, 1, ID: "ID") + 1;
                                }
                                var t = Report.ExcelHeader(worksheet, param, 1, masterHeaderLength);
                                Report.ExcelHeader(worksheetPrim, param, 1, masterHeaderLength);
                                masterHeaderLength += t;
                                masterHeaderLength--;
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
                                var tyu = 2;
                                var lst = new List<Report>();
                                if (forSelectedOrg)
                                {
                                    var newItem = selectedReports.Report_Collection.Where(x => x.FormNum_DB.Equals(param));
                                    lst.AddRange(newItem);
                                }
                                else
                                {
                                    foreach (var key in Local_Reports.Reports_Collection)
                                    {
                                        var item = (Reports)key;
                                        var newItem = item.Report_Collection.Where(x => x.FormNum_DB.Equals(param));
                                        lst.AddRange(newItem);
                                    }
                                }

                                //foreach (Reports item in Local_Reports.Reports_Collection)
                                //{
                                //    lst.AddRange(item.Report_Collection);
                                //}

                                _Excel_Export_Rows(param, tyu, masterHeaderLength, worksheet, lst, true);
                                _Excel_Export_Notes(param, tyu, masterHeaderLength, worksheetPrim, lst, true);

                                excelPackage.Save();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                int l = 10;
            }
        }
        #endregion

        #region SelectOrgExcelExport
        public ReactiveCommand<Unit, Unit> SelectOrgExcelExport { get; private set; }
        private async Task _SelectOrgExcelExport()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = desktop.MainWindow as MainWindow;
                var selectedReports = (Reports?)mainWindow?.SelectedReports.FirstOrDefault();
                if (selectedReports is null || !selectedReports.Report_Collection.Any())
                {
                    #region MessageExcelExportFail
                    var msg = "Выгрузка не выполнена. ";
                    msg += selectedReports is null
                        ? "Не выбрана организация."
                        : "У выбранной организации отсутствуют формы отчетности.";
                    await MessageBox.Avalonia.MessageBoxManager
                                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                    {
                                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                        ContentTitle = "Выгрузка в Excel",
                                        ContentMessage = msg,
                                        MinHeight = 125,
                                        MinWidth = 400,
                                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                                    })
                                    .ShowDialog(mainWindow);
                    #endregion
                    return;
                }
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel",
                    Extensions = { "xlsx" }
                };
                dial.Filters!.Add(filter);
                var res = await dial.ShowAsync(desktop.MainWindow);
                if (!string.IsNullOrEmpty(res))
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
                    using ExcelPackage excelPackage = new(new FileInfo(path));
                    excelPackage.Workbook.Properties.Author = "RAO_APP";
                    excelPackage.Workbook.Properties.Title = "Report";
                    excelPackage.Workbook.Properties.Created = DateTime.Now;
                    HashSet<string> formNums = new();
                    foreach (var key in selectedReports.Report_Collection)
                    {
                        var rep = (Report)key;
                        formNums.Add(rep.FormNum_DB);
                    }
                    if (formNums.Contains("1.1"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.1");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.1");
                        ExportForm11Data(selectedReports);
                    }
                    if (formNums.Contains("1.2"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.2");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.2");
                        ExportForm12Data(selectedReports);
                    }
                    if (formNums.Contains("1.3"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.3");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.3");
                        ExportForm13Data(selectedReports);
                    }
                    if (formNums.Contains("1.4"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.4");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.4");
                        ExportForm14Data(selectedReports);
                    }
                    if (formNums.Contains("1.5"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.5");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.5");
                        ExportForm15Data(selectedReports);
                    }
                    if (formNums.Contains("1.6"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.6");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.6");
                        ExportForm16Data(selectedReports);
                    }
                    if (formNums.Contains("1.7"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.7");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.7");
                        ExportForm17Data(selectedReports);
                    }
                    if (formNums.Contains("1.8"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.8");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.8");
                        ExportForm18Data(selectedReports);
                    }
                    if (formNums.Contains("1.9"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 1.9");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 1.9");
                        ExportForm19Data(selectedReports);
                    }
                    if (formNums.Contains("2.1"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.1");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.1");
                        ExportForm21Data(selectedReports);
                    }
                    if (formNums.Contains("2.2"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.2");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.2");
                        ExportForm22Data(selectedReports);
                    }
                    if (formNums.Contains("2.3"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.3");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.3");
                        ExportForm23Data(selectedReports);
                    }
                    if (formNums.Contains("2.4"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.4");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.4");
                        ExportForm24Data(selectedReports);
                    }
                    if (formNums.Contains("2.5"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.5");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.5");
                        ExportForm25Data(selectedReports);
                    }
                    if (formNums.Contains("2.6"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.6");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.6");
                        ExportForm26Data(selectedReports);
                    }
                    if (formNums.Contains("2.7"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.7");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.7");
                        ExportForm27Data(selectedReports);
                    }
                    if (formNums.Contains("2.8"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.8");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.8");
                        ExportForm28Data(selectedReports);
                    }
                    if (formNums.Contains("2.9"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.9");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.9");
                        ExportForm29Data(selectedReports);
                    }
                    if (formNums.Contains("2.10"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.10");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.10");
                        ExportForm210Data(selectedReports);
                    }
                    if (formNums.Contains("2.11"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.11");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.11");
                        ExportForm211Data(selectedReports);
                    }
                    if (formNums.Contains("2.12"))
                    {
                        worksheet = excelPackage.Workbook.Worksheets.Add($"Форма 2.12");
                        worksheetComment = excelPackage.Workbook.Worksheets.Add($"Примечания 2.12");
                        ExportForm212Data(selectedReports);
                    }
                    excelPackage.Save();
                }
            }
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows11)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows12)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows13)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows14)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows15)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows16)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows17)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows18)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows19)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows21)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows22)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows23)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows24)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows25)
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
                    foreach (var key in rep.Notes)
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
            worksheet.Cells[1, 10].Value = "Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м";
            worksheet.Cells[1, 11].Value = "Глубина отбора проб, м";
            worksheet.Cells[1, 12].Value = "Наименование радионуклида";
            worksheet.Cells[1, 13].Value = "Среднегодовое содержание радионуклида, Бк/кг";
            NotesHeaders();

            var tmp = 2;
            List<Reports> repList = new() { selectedReports };
            foreach (var reps in repList)
            {
                var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.6") && x.Rows26 != null);
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows26)
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
                    foreach (var key in rep.Notes)
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
            worksheet.Cells[1, 9].Value = "разрешенный";
            worksheet.Cells[1, 10].Value = "фактический";
            worksheet.Cells[1, 11].Value = "фактический";
            NotesHeaders();

            var tmp = 2;
            List<Reports> repList = new() { selectedReports };
            foreach (var reps in repList)
            {
                var form = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("2.7") && x.Rows27 != null);
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows27)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows28)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows29)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows210)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows211)
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
                    foreach (var key in rep.Notes)
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
                foreach (var rep in form)
                {
                    var currentRow = tmp;
                    foreach (var key in rep.Rows212)
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
                    foreach (var key in rep.Notes)
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
        }
        #endregion
        #endregion
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
                                using (ExcelPackage excelPackage = new(new FileInfo(path)))
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
                                using (ExcelPackage excelPackage = new(new FileInfo(path)))
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
                                using (ExcelPackage excelPackage = new(new FileInfo(path)))
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
                                await ShowMessage.Handle(new List<string>
                                {
                                    $"Не удалось сохранить файл по пути: {path}{Environment.NewLine}Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                                return;
                            }
                        }
                        if (path != null)
                        {
                            using ExcelPackage excelPackage = new(new FileInfo(path));
                            excelPackage.Workbook.Properties.Author = "RAO_APP";
                            excelPackage.Workbook.Properties.Title = "Report";
                            excelPackage.Workbook.Properties.Created = DateTime.Now;
                            var worksheet = excelPackage.Workbook.Worksheets.Add($"Список отчётов без файла паспорта");

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
                                await ShowMessage.Handle(new List<string>
                                {
                                    $"Не удалось открыть сетевое хранилище паспортов:{Environment.NewLine}{directory.FullName}", "Ошибка", "Ок" });
                                return;
                            }
                            pasNames.AddRange(Files.Select(file => file.Name.Remove(file.Name.Length - 4)));
                            pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));

                            var currentRow = 2;
                            var findPasFile = false;
                            foreach (Reports reps in Local_Reports.Reports_Collection10)
                            {
                                var form11 = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
                                foreach (var rep in form11)
                                {
                                    List<Form11> repPas = rep.Rows11.Where(x => x.OperationCode_DB == "11"
                                    && x.Category_DB is 1 or 2 or 3).ToList();

                                    foreach (var repForm in repPas)
                                    {
                                        findPasFile = false;
                                        foreach (var pasParam in pasUniqParam)
                                        {
                                            if (ComparePasParam(repForm.CreatorOKPO_DB, pasParam[0])
                                                && ComparePasParam(repForm.Type_DB, pasParam[1])
                                                && ComparePasParam(ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
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
                                res = await ShowMessage.Handle(new List<string>
                                {
                                    $"Выгрузка всех записей паспортов с кодом 11 категорий 1, 2, 3,{Environment.NewLine}для которых отсутствуют файлы паспортов по пути: {directory.FullName}{Environment.NewLine}сохранена по пути:{Environment.NewLine}{path}", "", "Ок", "Открыть выгрузку" });
                                switch (res)
                                {
                                    case null or "Ок":
                                        return;
                                    case "Открыть выгрузку":
                                    {
                                        ProcessStartInfo procInfo = new() { FileName = path, UseShellExecute = true };
                                        Process.Start(procInfo);
                                        break;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                await ShowMessage.Handle(new List<string> { "Не удалось сохранить файл по указанному пути", "Ок" });
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
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog saveFileDialog = new();
                FileDialogFilter filter = new() { Name = "Excel", Extensions = { "xlsx" } };
                saveFileDialog.Filters?.Add(filter);
                var messageBoxWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxInputWindow(new MessageBoxInputParams
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
                    if (result.Button is null or "Отмена") return;
                    categories = Regex.Replace(result.Message, "[^\\d,]", "").Split(',').Select(short.Parse).Cast<short?>().ToList();
                }
                catch (Exception)
                {
                    await MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow("Уведомление",
                            "Номера категорий не были введены, либо были введены некорректно" +
                            $"{Environment.NewLine}Выгрузка будет осуществлена по всем категориям")
                        .ShowDialog(desktop.MainWindow);
                }
                var res = await saveFileDialog.ShowAsync(desktop.MainWindow);
                if (!string.IsNullOrEmpty(res))
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
                            await ShowMessage.Handle(new List<string>
                            {
                                $"Не удалось сохранить файл по пути: {path}{Environment.NewLine}Файл с таким именем уже существует в этом расположении и используется другим процессом.", "Ок" });
                            return;
                        }
                    }
                    using ExcelPackage excelPackage = new(new FileInfo(path));
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
                        await ShowMessage.Handle(new List<string>
                        {
                            $"Не удалось открыть сетевое хранилище паспортов:{Environment.NewLine}{directory.FullName}",
                            "Ошибка", "Ок"
                        });
                        return;
                    }

                    pasNames.AddRange(files.Select(file => file.Name.Remove(file.Name.Length - 4)));
                    pasUniqParam.AddRange(pasNames.Select(pasName => pasName.Split('#')));
                    foreach (var key in Local_Reports.Reports_Collection10)
                    {
                        var reps = (Reports)key;
                        var form11 = reps.Report_Collection.Where(x => x.FormNum_DB.Equals("1.1") && x.Rows11 != null);
                        foreach (var rep in form11)
                        {
                            List<Form11> repPas = rep.Rows11
                                .Where(x => x.OperationCode_DB == "11" && categories.Contains(x.Category_DB))
                                .ToList();
                            foreach (var repForm in repPas)
                            {
                                foreach (var pasParam in pasUniqParam.Where(pasParam => ComparePasParam(repForm.CreatorOKPO_DB, pasParam[0])
                                             && ComparePasParam(repForm.Type_DB, pasParam[1])
                                             && ComparePasParam(ConvertDateToYear(repForm.CreationDate_DB), pasParam[2])
                                             && ComparePasParam(ConvertPasNumAndFactNum(repForm.PassportNumber_DB), pasParam[3])
                                             && ComparePasParam(ConvertPasNumAndFactNum(repForm.FactoryNumber_DB), pasParam[4])))
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
                    worksheet.Cells.AutoFitColumns();
                    try
                    {
                        excelPackage.Save();
                        res = await ShowMessage.Handle(new List<string>
                        {
                            "Выгрузка всех записей паспортов с кодом 11 категорий 1, 2, 3," +
                            $"{Environment.NewLine}для которых отсутствуют файлы паспортов по пути: {directory.FullName}" +
                            $"{Environment.NewLine}сохранена по пути:{Environment.NewLine}{path}",
                            "", "Ок", "Открыть выгрузку"
                        });
                        if (res is null or "Ок")
                            return;
                        if (res.Equals("Открыть выгрузку"))
                        {
                            ProcessStartInfo procInfo = new() { FileName = path, UseShellExecute = true };
                            Process.Start(procInfo);
                        }
                    }
                    catch (Exception)
                    {
                        await ShowMessage.Handle(new List<string>
                            { "Не удалось сохранить файл по указанному пути", "Ок" });
                    }
                }
            }
        }
        #endregion

        private static bool ComparePasParam(string nameDb, string namePas)
        {
            nameDb ??= "";
            nameDb = Regex.Replace(nameDb, "[\\\\/:*?\"<>|]", "_");
            nameDb = Regex.Replace(nameDb, "\\s+", "");
            namePas = Regex.Replace(namePas, "[\\\\/:*?\"<>|]", "_");
            namePas = Regex.Replace(namePas, "\\s+", "");
            return nameDb.Equals(namePas, StringComparison.OrdinalIgnoreCase)
                || ChangeOrCreateVM.TranslateToEng(nameDb).Equals(ChangeOrCreateVM.TranslateToEng(namePas), StringComparison.OrdinalIgnoreCase)
                || ChangeOrCreateVM.TranslateToRus(nameDb).Equals(ChangeOrCreateVM.TranslateToRus(namePas), StringComparison.OrdinalIgnoreCase);
        }

        public static string ConvertDateToYear(string? date)
        {
            Regex r = new(@"(\d{1,2}[.\/]){1,2}\d{4}");
            if (date is null || !r.IsMatch(date))
                return "0000";
            var matches = r.Matches(date);
            return matches.FirstOrDefault()!.Value[^4..];
        }

        private static string ConvertPasNumAndFactNum(string num)
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
            return num;
        }

        #region ChangePasDir
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

        #region ExcelExportNotes
        private int _Excel_Export_Notes(string param, int StartRow, int StartColumn, ExcelWorksheet worksheetPrim, List<Report> forms, bool printID = false)
        {
            foreach (var item in forms)
            {

                var findReports = Local_Reports.Reports_Collection.Where(t => t.Report_Collection.Contains(item));
                var reps = findReports.FirstOrDefault();
                if (reps != null)
                {
                    var cnty = StartRow;
                    foreach (var i in item.Notes)
                    {
                        var mstrep = reps.Master_DB;
                        i.ExcelRow(worksheetPrim, cnty, StartColumn + 1);
                        int yu;
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
        private int _Excel_Export_Rows(string param, int startRow, int startColumn, ExcelWorksheet worksheet, List<Report> forms, bool id = false)
        {
            foreach (Report item in forms)
            {

                var findReports = Local_Reports.Reports_Collection.Where(t => t.Report_Collection.Contains(item));
                var reps = findReports.FirstOrDefault();
                if (reps != null)
                {
                    IEnumerable<IKey> t = null;
                    if (param == "2.1")
                    {
                        t = item[param].ToList<IKey>().Where(x => ((Form21)x).Sum_DB || ((Form21)x).SumGroup_DB);
                        if (item[param].ToList<IKey>().Any() && !t.Any())
                        {
                            t = item[param].ToList<IKey>();
                        }
                    }
                    if (param == "2.2")
                    {
                        t = item[param].ToList<IKey>().Where(x => ((Form22)x).Sum_DB || ((Form22)x).SumGroup_DB);
                        if (item[param].ToList<IKey>().Any() && !t.Any())
                        {
                            t = item[param].ToList<IKey>();
                        }
                    }
                    if (param != "2.1" && param != "2.2")
                    {
                        t = item[param].ToList<IKey>();
                    }
                    var lst = t.Any()
                        ? item[param].ToList<IKey>().ToList()
                        : item[param].ToList<IKey>().OrderBy(x => ((Form)x).NumberInOrder_DB).ToList();
                    if (lst.Count > 0)
                    {
                        var count = startRow;
                        startRow--;
                        foreach (var it in lst.Where(it => it != null))
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
                                    form21.ExcelRow(worksheet, count, startColumn + 1, SumNumber: form21.NumberInOrderSum_DB);
                                    break;
                                case Form22 form22:
                                    form22.ExcelRow(worksheet, count, startColumn + 1, SumNumber: form22.NumberInOrderSum_DB);
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
                            var yu = 0;
                            if (id)
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
                }
            }
            return startRow;
        }
        #endregion

        #region ExcelPrintTitulExport
        private void _Excel_Print_Titul_Export(string param, ExcelWorksheet worksheet, Report form)
        {
            var findReports = Local_Reports.Reports_Collection.Where(t => t.Report_Collection.Contains(form));
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
            var findReports = Local_Reports.Reports_Collection.Where(t => t.Report_Collection.Contains(form));
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
            int Start = 15;
            if (param == "2.8")
            {
                Start = 18;
            }

            for (var i = 0; i < form.Notes.Count - 1; i++)
            {
                worksheet.InsertRow(Start + 1, 1, Start);
                var cells = worksheet.Cells[$"A{Start + 1}:B{Start + 1}"];
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
                var cellCL = worksheet.Cells[$"C{Start + 1}:L{Start + 1}"];
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
            var Start = 11;
            if (param == "2.8")
            {
                Start = 14;
            }
            for (var i = 0; i < form[param].Count - 1; i++)
            {
                worksheet.InsertRow(Start + 1, 1, Start);
                var cells = worksheet.Cells[$"A{Start + 1}:B{Start + 1}"];
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
            var count = Start;
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
        #endregion

        #region INotifyPropertyChanged

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}