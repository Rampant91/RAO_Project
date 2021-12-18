using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Models.Collections;
using Models;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Client_App.Controls.DataGrid;
using Models.DBRealization;
using System;
using Models.Attributes;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Avalonia;
using Client_App.Views;
using DynamicData;
using OfficeOpenXml;
using Spravochniki;
using System.IO;
using System.Threading;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
        public string WindowHeader { get; set; } = "default";
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _FormType;
        public string FormType
        {
            get => _FormType;
            set
            {
                if (_FormType != value)
                {
                    _FormType = value;
                    NotifyPropertyChanged("FormType");
                }
            }
        }

        private Report _Storage;
        public Report Storage
        {
            get => _Storage;
            set
            {
                if (_Storage != value)
                {
                    _Storage = value;
                    NotifyPropertyChanged("Storage");
                }
            }
        }
        private Reports _Storages;
        public Reports Storages
        {
            get => _Storages;
            set
            {
                if (_Storages != value)
                {
                    _Storages = value;
                    NotifyPropertyChanged("Storages");
                }
            }
        }
        private DBObservable _DBO;
        public DBObservable DBO
        {
            get => _DBO;
            set
            {
                if (_DBO != value)
                {
                    _DBO = value;
                    NotifyPropertyChanged("DBO");
                }
            }
        }

        private Form10 _Storage10;
        public Form10 Storage10
        {
            get
            {
                int count = Storage.Rows10.Count;
                return Storage.Rows10[count - 1];
            }
            set
            {
                if (_Storage10 != value)
                {
                    _Storage10 = value;
                    NotifyPropertyChanged("Storage10");
                }
            }
        }

        private Form20 _Storage20;
        public Form20 Storage20
        {
            get
            {
                int count = Storage.Rows20.Count;
                return Storage.Rows20[count - 1];
            }
            set
            {
                if (_Storage20 != value)
                {
                    _Storage20 = value;
                    NotifyPropertyChanged("Storage20");
                }
            }
        }

        public ReactiveCommand<Unit, Unit> CheckReport { get; protected set; }
        public ReactiveCommand<Unit, Unit> SumRow { get; protected set; }
        public ReactiveCommand<string, Unit> AddSort { get; protected set; }
        public ReactiveCommand<string, Unit> AddNote { get; protected set; }
        public ReactiveCommand<Unit, Unit> AddRow { get; protected set; }
        public ReactiveCommand<IList, Unit> DeleteRow { get; protected set; }
        public ReactiveCommand<Unit, Unit> DuplicateRowsx1 { get; protected set; }
        public ReactiveCommand<IList, Unit> CopyRows { get; protected set; }
        public ReactiveCommand<IList, Unit> PasteRows { get; protected set; }
        public ReactiveCommand<IList, Unit> DeleteNote { get; protected set; }
        public ReactiveCommand<Unit, Unit> PasteNotes { get; protected set; }

        public ChangeOrCreateVM(string param, in Report rep,Reports reps)
        {
            Storage = rep;
            Storages = reps;
            FormType = param;
            Init();
        }
        public ChangeOrCreateVM(string param, in Reports reps)
        {
            Storage = new Report()
            {
                FormNum_DB = param
            };

            if (param.Split('.')[0] == "1")
            {
                if (param != "1.0")
                {
                    try
                    {
                        var ty = (from t in reps.Report_Collection where t.FormNum_DB == param && t.EndPeriod_DB != "" orderby DateTimeOffset.Parse(t.EndPeriod_DB) select t.EndPeriod_DB).LastOrDefault();

                        FormType = param;
                        Storage.StartPeriod.Value = ty;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                if (param != "2.0")
                {
                    try
                    {
                        var ty = (from t in reps.Report_Collection where t.FormNum_DB == param && t.Year_DB != null orderby t.Year_DB select t.Year_DB).LastOrDefault();

                        FormType = param;
                        Storage.Year.Value = ty + 1;
                    }
                    catch
                    {

                    }
                }
            }

            if (param == "1.0")
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
            }
            if (param == "2.0")
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
            }
            Storages = reps;
            FormType = param;
            Init();
        }
        public ChangeOrCreateVM(string param, in DBObservable reps)
        {
            Storage = new Report()
            {
                FormNum_DB = param
            };

            if (param == "1.0")
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
            }
            if (param == "2.0")
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
            }

            FormType = param;
            DBO = reps;
            Init();
        }
        public Interaction<object, int> ShowDialog { get; protected set; }
        public Interaction<string, string> ShowMessage { get; protected set; }
        public void Init()
        {
            string a = FormType.Replace(".", "");
            if (FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "1")
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name +
                    " " + Storages.Master_DB.RegNoRep.Value + " " + Storages.Master_DB.ShortJurLicoRep.Value + " " + Storages.Master_DB.OkpoRep.Value;
            }
            if (FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "2")
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name +
                    " " + Storages.Master_DB.RegNoRep1.Value + " " + Storages.Master_DB.ShortJurLicoRep1.Value + " " + Storages.Master_DB.OkpoRep1.Value;
            }
            if(FormType=="1.0"||FormType=="2.0")
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            }
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            AddRow = ReactiveCommand.Create(_AddRow);
            DeleteRow = ReactiveCommand.CreateFromTask<IList>(_DeleteRow);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            SumRow = ReactiveCommand.Create(_SumRow);
            PasteRows = ReactiveCommand.CreateFromTask<IList>(_PasteRows);
            DuplicateRowsx1 = ReactiveCommand.CreateFromTask(_DuplicateRowsx1);
            CopyRows = ReactiveCommand.CreateFromTask<IList>(_CopyRows);
            AddNote = ReactiveCommand.Create<string>(_AddNote);
            DeleteNote = ReactiveCommand.CreateFromTask<IList>(_DeleteNote);
            //PasteNotes = ReactiveCommand.CreateFromTask(_PasteNotes);

            ShowDialog = new Interaction<object,int>();
            ShowMessage = new Interaction<string, string>();
        }

        private bool _isCanSaveReportEnabled = false;

        private bool IsCanSaveReportEnabled
        {
            get => _isCanSaveReportEnabled;
            set
            {
                if (value == _isCanSaveReportEnabled)
                {
                    return;
                }

                _isCanSaveReportEnabled = value;
                PropertyChanged?
                    .Invoke(this, new PropertyChangedEventArgs(nameof(IsCanSaveReportEnabled)));
            }
        }

        [DependsOn(nameof(IsCanSaveReportEnabled))]
        private bool CanSaveReport(object parameter)
        {
            return _isCanSaveReportEnabled;
        }

        public void SaveReport()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (DBO != null)
                {
                    var tmp = new Reports();
                    tmp.Master = Storage;
                    if (tmp.Master.Rows10.Count() != 0)
                    {
                        tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                        tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
                    }
                    if (tmp.Master.Rows20.Count() != 0)
                    {
                        tmp.Master.Rows20[1].OrganUprav.Value = tmp.Master.Rows20[0].OrganUprav.Value;
                        tmp.Master.Rows20[1].RegNo.Value = tmp.Master.Rows20[0].RegNo.Value;
                    }

                    DBO.Reports_Collection.Add(tmp);
                    DBO = null;
                    Storages = null;
                }
                else
                {
                    if (Storages != null)
                    {
                        if (FormType != "1.0" && FormType != "2.0")
                        {
                            if (!Storages.Report_Collection.Contains(Storage))
                            {
                                Storages.Report_Collection.Add(Storage);
                            }
                        }
                    }
                    else
                    {

                    }
                }

                var dbm = StaticConfiguration.DBModel;
                dbm.SaveChanges();
                IsCanSaveReportEnabled = false;
            }
        }

        private void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
        }
        private void _SumRow()
        {
            Storage.Rows21.Sum();
            Storage.Rows22.Sum();
        }
        public void _AddRow10()
        {
            Form10? frm = new Form10(); Storage.Rows10.Add(frm); Storage.LastAddedForm = Report.Forms.Form10;
        }
        public void _AddRow20()
        {
            Form20? frm = new Form20(); Storage.Rows20.Add(frm); Storage.LastAddedForm = Report.Forms.Form20;
        }

        int GetNumberInOrder(IEnumerable lst)
        {
            int maxNum = 0;

            foreach (var item in lst)
            {
                var frm = (Form)item;
                if (frm.NumberInOrder_DB >= maxNum)
                {
                    maxNum++;
                }
            }

            return maxNum + 1;
        }

        private void _AddRow()
        {
            var frm = FormCreator.Create(FormType);
            if (FormType == "1.1") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows11); Storage.Rows11.Add((Form11)frm); Storage.LastAddedForm = Report.Forms.Form11; }
            if (FormType == "1.2") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows12); Storage.Rows12.Add((Form12)frm); Storage.LastAddedForm = Report.Forms.Form12; }
            if (FormType == "1.3") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows13); Storage.Rows13.Add((Form13)frm); Storage.LastAddedForm = Report.Forms.Form13; }
            if (FormType == "1.4") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows14); Storage.Rows14.Add((Form14)frm); Storage.LastAddedForm = Report.Forms.Form14; }
            if (FormType == "1.5") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows15); Storage.Rows15.Add((Form15)frm); Storage.LastAddedForm = Report.Forms.Form15; }
            if (FormType == "1.6") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows16); Storage.Rows16.Add((Form16)frm); Storage.LastAddedForm = Report.Forms.Form16; }
            if (FormType == "1.7") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows17); Storage.Rows17.Add((Form17)frm); Storage.LastAddedForm = Report.Forms.Form17; }
            if (FormType == "1.8") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows18); Storage.Rows18.Add((Form18)frm); Storage.LastAddedForm = Report.Forms.Form18; }
            if (FormType == "1.9") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows19); Storage.Rows19.Add((Form19)frm); Storage.LastAddedForm = Report.Forms.Form19; }

            if (FormType == "2.1") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows21); Storage.Rows21.Add((Form21)frm); Storage.LastAddedForm = Report.Forms.Form21; }
            if (FormType == "2.2") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows22); Storage.Rows22.Add((Form22)frm); Storage.LastAddedForm = Report.Forms.Form22; }
            if (FormType == "2.3") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows23); Storage.Rows23.Add((Form23)frm); Storage.LastAddedForm = Report.Forms.Form23; }
            if (FormType == "2.4") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows24); Storage.Rows24.Add((Form24)frm); Storage.LastAddedForm = Report.Forms.Form24; }
            if (FormType == "2.5") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows25); Storage.Rows25.Add((Form25)frm); Storage.LastAddedForm = Report.Forms.Form25; }
            if (FormType == "2.6") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows26); Storage.Rows26.Add((Form26)frm); Storage.LastAddedForm = Report.Forms.Form26; }
            if (FormType == "2.7") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows27); Storage.Rows27.Add((Form27)frm); Storage.LastAddedForm = Report.Forms.Form27; }
            if (FormType == "2.8") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows28); Storage.Rows28.Add((Form28)frm); Storage.LastAddedForm = Report.Forms.Form28; }
            if (FormType == "2.9") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows29); Storage.Rows29.Add((Form29)frm); Storage.LastAddedForm = Report.Forms.Form29; }
            if (FormType == "2.10") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows210); Storage.Rows210.Add((Form210)frm); Storage.LastAddedForm = Report.Forms.Form210; }
            if (FormType == "2.11") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows211); Storage.Rows211.Add((Form211)frm); Storage.LastAddedForm = Report.Forms.Form211; }
            if (FormType == "2.12") { frm.NumberInOrder_DB = GetNumberInOrder(Storage.Rows212); Storage.Rows212.Add((Form212)frm); Storage.LastAddedForm = Report.Forms.Form212; }
        }

        private void _AddNote(string Param)
        {
            Note? nt = new Note();
            Storage.Notes.Add(nt);
        }

        private async Task _DeleteNote(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var str = "Вы действительно хотите удалить комментарий?";
                var answ=await ShowMessage.Handle(str);
                if (answ == "Да")
                {
                    List<Note> lst = new List<Note>();
                    foreach (object? item in param)
                    {
                        lst.Add((Note)item);
                    }
                    foreach (Note nt in lst)
                    {
                        Storage.Notes.Remove((Note)nt);
                    }
                }
            }
        }

        private async Task _DeleteRow(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var str = "Вы действительно хотите удалить строчку?";
                var answ = await ShowMessage.Handle(str);
                if (answ == "Да")
                {
                    List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
                    foreach (object? item in param)
                    {
                        lst.Add((Models.Abstracts.Form)item);
                    }
                    var grp = lst.GroupBy(x => x.NumberInOrder_DB);
                    foreach (var group in grp)
                    {
                        var item = group.FirstOrDefault();
                        if (item != null)
                        {
                            foreach (Form it in Storage[item.FormNum_DB])
                            {
                                if (it.NumberInOrder_DB > item.NumberInOrder_DB)
                                {
                                    it.NumberInOrder.Value = it.NumberInOrder_DB - 1;
                                }
                            }
                            if (item.FormNum.Value == "1.1")
                            {
                                Storage.Rows11.Remove((Form11)item);
                            }

                            if (item.FormNum.Value == "1.2")
                            {
                                Storage.Rows12.Remove((Form12)item);
                            }

                            if (item.FormNum.Value == "1.3")
                            {
                                Storage.Rows13.Remove((Form13)item);
                            }

                            if (item.FormNum.Value == "1.4")
                            {
                                Storage.Rows14.Remove((Form14)item);
                            }

                            if (item.FormNum.Value == "1.5")
                            {
                                Storage.Rows15.Remove((Form15)item);
                            }

                            if (item.FormNum.Value == "1.6")
                            {
                                Storage.Rows16.Remove((Form16)item);
                            }

                            if (item.FormNum.Value == "1.7")
                            {
                                Storage.Rows17.Remove((Form17)item);
                            }

                            if (item.FormNum.Value == "1.8")
                            {
                                Storage.Rows18.Remove((Form18)item);
                            }

                            if (item.FormNum.Value == "1.9")
                            {
                                Storage.Rows19.Remove((Form19)item);
                            }

                            if (item.FormNum.Value == "2.1")
                            {
                                Storage.Rows21.Remove((Form21)item);
                            }

                            if (item.FormNum.Value == "2.2")
                            {
                                Storage.Rows22.Remove((Form22)item);
                            }

                            if (item.FormNum.Value == "2.3")
                            {
                                Storage.Rows23.Remove((Form23)item);
                            }

                            if (item.FormNum.Value == "2.4")
                            {
                                Storage.Rows24.Remove((Form24)item);
                            }

                            if (item.FormNum.Value == "2.5")
                            {
                                Storage.Rows25.Remove((Form25)item);
                            }

                            if (item.FormNum.Value == "2.6")
                            {
                                Storage.Rows26.Remove((Form26)item);
                            }

                            if (item.FormNum.Value == "2.7")
                            {
                                Storage.Rows27.Remove((Form27)item);
                            }

                            if (item.FormNum.Value == "2.8")
                            {
                                Storage.Rows28.Remove((Form28)item);
                            }

                            if (item.FormNum.Value == "2.9")
                            {
                                Storage.Rows29.Remove((Form29)item);
                            }

                            if (item.FormNum.Value == "2.10")
                            {
                                Storage.Rows210.Remove((Form210)item);
                            }

                            if (item.FormNum.Value == "2.11")
                            {
                                Storage.Rows211.Remove((Form211)item);
                            }

                            if (item.FormNum.Value == "2.12")
                            {
                                Storage.Rows212.Remove((Form212)item);
                            }
                        }
                    }
                }
            }
        }

        private void _AddSort(string param)
        {
            //Storage.Filters.SortPath = param;
        }

        private async Task _PasteRows(IEnumerable param)
        {
            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string? text = await clip.GetTextAsync();
                Cell cl = null;
                foreach (var item in param)
                {
                    cl = (Cell)item;
                    break;
                }

                if (cl != null)
                {
                    int Row = cl.CellRow;
                    int Column = cl.CellColumn;

                    if (text != null && text != "")
                    {
                        string rt = "";
                        foreach (var item in text)
                        {
                            if (item == '\n')
                            {
                                foreach (var it in param)
                                {
                                    var cell = (Cell)it;
                                    if (cell.CellColumn == Column && cell.CellRow == Row)
                                    {
                                        var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                                        if (child != null)
                                        {
                                            var panel = (Panel)child.Child;
                                            var textbox = (TextBox)panel.Children.FirstOrDefault();
                                            textbox.Text = rt;
                                        }
                                        break;
                                    }
                                }
                                rt = "";
                                Row++;
                                Column = cl.CellColumn;
                            }
                            else
                            {
                                if (item == '\t')
                                {
                                    foreach (var it in param)
                                    {
                                        var cell = (Cell)it;
                                        if (cell.CellColumn == Column && cell.CellRow == Row)
                                        {
                                            var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                                            if (child != null)
                                            {
                                                var panel = (Panel)child.Child;
                                                var textbox = (TextBox)panel.Children.FirstOrDefault();
                                                textbox.Text = rt;
                                            }
                                            break;
                                        }
                                    }
                                    rt = "";
                                    Column++;
                                }
                                else
                                {
                                    rt += item;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task _CopyRows(IEnumerable param)
        {
            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string txt = "";

                var Column = 1;
                var Row = 1;

                bool flag = true;
                foreach (var item in param)
                {
                    var cell = (Cell)item;
                    if (flag)
                    {
                        Column = cell.CellColumn;
                        Row = cell.CellRow;
                        flag = false;
                    }
                    var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                    if (child != null)
                    {
                        var panel = (Panel)child.Child;
                        var textbox = (TextBox)panel.Children.FirstOrDefault();
                        if (Row != cell.CellRow)
                        {
                            txt += "\n";
                            Row = cell.CellRow;
                            Column = cell.CellColumn;
                        }
                        if (Column != cell.CellColumn)
                        {
                            txt += "\t";
                            Column = cell.CellColumn;
                        }
                        txt += textbox.Text;
                    }
                }

                txt += "\t";
                await clip.ClearAsync();
                await clip.SetTextAsync(txt);
            }
        }

        private async Task _DuplicateRowsx1()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                RowNumber rw = new RowNumber();
                await rw.ShowDialog(desktop.MainWindow);
                var t = Convert.ToInt32(rw.Number);
                if (t > 0)
                {
                    List<Form> lst = new List<Form>();
                    var number = 0;
                    if (FormType == "1.1") { number = GetNumberInOrder(Storage.Rows11); }
                    if (FormType == "1.2") { number = GetNumberInOrder(Storage.Rows12); }
                    if (FormType == "1.3") { number = GetNumberInOrder(Storage.Rows13); }
                    if (FormType == "1.4") { number = GetNumberInOrder(Storage.Rows14); }
                    if (FormType == "1.5") { number = GetNumberInOrder(Storage.Rows15); }
                    if (FormType == "1.6") { number = GetNumberInOrder(Storage.Rows16); }
                    if (FormType == "1.7") { number = GetNumberInOrder(Storage.Rows17); }
                    if (FormType == "1.8") { number = GetNumberInOrder(Storage.Rows18); }
                    if (FormType == "1.9") { number = GetNumberInOrder(Storage.Rows19); }

                    if (FormType == "2.1") { number = GetNumberInOrder(Storage.Rows21); }
                    if (FormType == "2.2") { number = GetNumberInOrder(Storage.Rows22); }
                    if (FormType == "2.3") { number = GetNumberInOrder(Storage.Rows23); }
                    if (FormType == "2.4") { number = GetNumberInOrder(Storage.Rows24); }
                    if (FormType == "2.5") { number = GetNumberInOrder(Storage.Rows25); }
                    if (FormType == "2.6") { number = GetNumberInOrder(Storage.Rows26); }
                    if (FormType == "2.7") { number = GetNumberInOrder(Storage.Rows27); }
                    if (FormType == "2.8") { number = GetNumberInOrder(Storage.Rows28); }
                    if (FormType == "2.9") { number = GetNumberInOrder(Storage.Rows29); }
                    if (FormType == "2.10") { number = GetNumberInOrder(Storage.Rows210); }
                    if (FormType == "2.11") { number = GetNumberInOrder(Storage.Rows211); }
                    if (FormType == "2.12") { number = GetNumberInOrder(Storage.Rows212); }
                    for (int i = 0; i < t; i++)
                    {
                        var frm = FormCreator.Create(FormType);
                        if (FormType == "1.1") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form11; }
                        if (FormType == "1.2") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form12; }
                        if (FormType == "1.3") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form13; }
                        if (FormType == "1.4") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form14; }
                        if (FormType == "1.5") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form15; }
                        if (FormType == "1.6") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form16; }
                        if (FormType == "1.7") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form17; }
                        if (FormType == "1.8") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form18; }
                        if (FormType == "1.9") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form19; }

                        if (FormType == "2.1") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form21; }
                        if (FormType == "2.2") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form22; }
                        if (FormType == "2.3") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form23; }
                        if (FormType == "2.4") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form24; }
                        if (FormType == "2.5") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form25; }
                        if (FormType == "2.6") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form26; }
                        if (FormType == "2.7") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form27; }
                        if (FormType == "2.8") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form28; }
                        if (FormType == "2.9") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form29; }
                        if (FormType == "2.10") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form210; }
                        if (FormType == "2.11") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form211; }
                        if (FormType == "2.12") { frm.NumberInOrder_DB = number; Storage.LastAddedForm = Report.Forms.Form212; }
                        lst.Add(frm);
                        number++;
                    }
                    if (FormType == "1.1") { var tmp = from i in lst select (Form11)i; Storage.Rows11.AddRange(tmp); }
                    if (FormType == "1.2") { var tmp = from i in lst select (Form12)i; Storage.Rows12.AddRange(tmp); }
                    if (FormType == "1.3") { var tmp = from i in lst select (Form13)i; Storage.Rows13.AddRange(tmp); }
                    if (FormType == "1.4") { var tmp = from i in lst select (Form14)i; Storage.Rows14.AddRange(tmp); }
                    if (FormType == "1.5") { var tmp = from i in lst select (Form15)i; Storage.Rows15.AddRange(tmp); }
                    if (FormType == "1.6") { var tmp = from i in lst select (Form16)i; Storage.Rows16.AddRange(tmp); }
                    if (FormType == "1.7") { var tmp = from i in lst select (Form17)i; Storage.Rows17.AddRange(tmp); }
                    if (FormType == "1.8") { var tmp = from i in lst select (Form18)i; Storage.Rows18.AddRange(tmp); }
                    if (FormType == "1.9") { var tmp = from i in lst select (Form19)i; Storage.Rows19.AddRange(tmp); }

                    if (FormType == "2.1") { var tmp = from i in lst select (Form21)i; Storage.Rows21.AddRange(tmp); }
                    if (FormType == "2.2") { var tmp = from i in lst select (Form22)i; Storage.Rows22.AddRange(tmp); }
                    if (FormType == "2.3") { var tmp = from i in lst select (Form23)i; Storage.Rows23.AddRange(tmp); }
                    if (FormType == "2.4") { var tmp = from i in lst select (Form24)i; Storage.Rows24.AddRange(tmp); }
                    if (FormType == "2.5") { var tmp = from i in lst select (Form25)i; Storage.Rows25.AddRange(tmp); }
                    if (FormType == "2.6") { var tmp = from i in lst select (Form26)i; Storage.Rows26.AddRange(tmp); }
                    if (FormType == "2.7") { var tmp = from i in lst select (Form27)i; Storage.Rows27.AddRange(tmp); }
                    if (FormType == "2.8") { var tmp = from i in lst select (Form28)i; Storage.Rows28.AddRange(tmp); }
                    if (FormType == "2.9") { var tmp = from i in lst select (Form29)i; Storage.Rows29.AddRange(tmp); }
                    if (FormType == "2.10") { var tmp = from i in lst select (Form210)i; Storage.Rows210.AddRange(tmp); }
                    if (FormType == "2.11") { var tmp = from i in lst select (Form211)i; Storage.Rows211.AddRange(tmp); }
                    if (FormType == "2.12") { var tmp = from i in lst select (Form212)i; Storage.Rows212.AddRange(tmp); }
                }
            }
        }
    }
}
