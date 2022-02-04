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
using Avalonia.Media;

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
        public ReactiveCommand<Unit, Unit> ChangeReportOrder { get; protected set; }
        public ReactiveCommand<Unit, Unit> SumRow { get; protected set; }
        public ReactiveCommand<string, Unit> AddSort { get; protected set; }
        public ReactiveCommand<string, Unit> AddNote { get; protected set; }
        public ReactiveCommand<string, Unit> AddRow { get; protected set; }
        public ReactiveCommand<IEnumerable, Unit> AddRowIn { get; protected set; }
        public ReactiveCommand<IEnumerable, Unit> DeleteRow { get; protected set; }
        public ReactiveCommand<Unit, Unit> DuplicateRowsx1 { get; protected set; }
        public ReactiveCommand<Unit, Unit> DuplicateNotes { get; protected set; }
        public ReactiveCommand<IEnumerable<Control>, Unit> CopyRows { get; protected set; }
        public ReactiveCommand<IEnumerable<Control>, Unit> PasteRows { get; protected set; }
        public ReactiveCommand<IEnumerable, Unit> DeleteNote { get; protected set; }
        public ReactiveCommand<Unit, Unit> PasteNotes { get; protected set; }

        public ChangeOrCreateVM(string param, in Report rep,Reports reps)
        {
            Storage = rep;
            Storages = reps;
            FormType = param;

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
                        var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.EndPeriod_DB != "" orderby DateTimeOffset.Parse(t.EndPeriod_DB) select t.EndPeriod_DB).LastOrDefault();

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
                        var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.Year_DB != null orderby t.Year_DB select t.Year_DB).LastOrDefault();

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

        public Interaction<int, int> ShowDialogIn { get; protected set; }
        public Interaction<object, int> ShowDialog { get; protected set; }
        public Interaction<string, string> ShowMessage { get; protected set; }
        public Interaction<List<string>, string> ShowMessageT { get; protected set; }
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
            AddRow = ReactiveCommand.CreateFromTask<string>(_AddRow);
            AddRowIn = ReactiveCommand.CreateFromTask<IEnumerable>(_AddRowIn);
            DeleteRow = ReactiveCommand.CreateFromTask<IEnumerable>(_DeleteRow);
            ChangeReportOrder = ReactiveCommand.Create(_ChangeReportOrder);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            SumRow = ReactiveCommand.Create(_SumRow);
            PasteRows = ReactiveCommand.CreateFromTask<IEnumerable<Control>>(_PasteRows);
            DuplicateRowsx1 = ReactiveCommand.CreateFromTask(_DuplicateRowsx1);
            CopyRows = ReactiveCommand.CreateFromTask<IEnumerable<Control>>(_CopyRows);
            AddNote = ReactiveCommand.CreateFromTask<string>(_AddNote);
            DeleteNote = ReactiveCommand.CreateFromTask<IEnumerable>(_DeleteNote);
            DuplicateNotes = ReactiveCommand.CreateFromTask(_DuplicateNotes);
            //PasteNotes = ReactiveCommand.CreateFromTask(_PasteNotes);

            ShowDialog = new Interaction<object,int>();
            ShowDialogIn = new Interaction<int, int>();
            ShowMessage = new Interaction<string, string>();
            ShowMessageT = new Interaction<List<string>, string>();

            Storages.Master_DB.Rows10.QuickSort();
            Storages.Master_DB.Rows20.QuickSort();
            Storage.Sort();
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
                    if (tmp.Master.Rows10.Count != 0)
                    {
                        tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                        tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
                    }
                    if (tmp.Master.Rows20.Count != 0)
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
                if (Storages != null)
                {
                    Storages.Report_Collection.QuickSort();
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
        private void _ChangeReportOrder()
        {
            Storage.Rows10.Sorted = false;
            Storage.Rows20.Sorted = false;

            var tmp = Storage.Rows10[0].Order;
            Storage.Rows10[0].Order= Storage.Rows10[1].Order;
            Storage.Rows10[1].Order=tmp;
            Storage.Rows10.QuickSort();

            tmp = Storage.Rows20[0].Order;
            Storage.Rows20[0].Order = Storage.Rows20[1].Order;
            Storage.Rows20[1].Order = tmp;
            Storage.Rows20.QuickSort();
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

        int GetNumberInOrder(IKeyCollection lst)
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

        private async Task _AddRow(string Param)
        {
            var frm = FormCreator.Create(FormType);
            frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
            Storage[Storage.FormNum_DB].Add(frm);
            Storage.Sort();
        }

        private async Task _AddRowIn(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
                foreach (object? item1 in param)
                {
                    lst.Add((Models.Abstracts.Form)item1);
                }

                var item = lst.FirstOrDefault();

                if (item != null)
                {
                    var number_cell = item.NumberInOrder_DB;
                    var t2 = await ShowDialogIn.Handle(number_cell);

                    if (item != null)
                    {
                        foreach (Form it in Storage[item.FormNum_DB])
                        {
                            if (it.NumberInOrder_DB > number_cell - 1)
                            {
                                it.NumberInOrder.Value = it.NumberInOrder_DB + t2;
                            }
                        }
                    }


                    if (t2 > 0)
                    {
                        List<Form> _lst = new List<Form>();
                        for (int i = 0; i < t2; i++)
                        {
                            var frm = FormCreator.Create(FormType);
                            frm.NumberInOrder_DB = number_cell;

                            _lst.Add(frm);
                            number_cell++;
                        }
                        Storage[Storage.FormNum_DB].AddRange(_lst);
                        Storage.Sort();
                    }
                }
            }
        }
        private async Task _AddNote(string Param)
        {
            Note? nt = new Note();
            nt.Order = GetNumberInOrder(Storage.Notes);
            Storage.Notes.Add(nt);
            Storage.Sort();
        }

        private async Task _DeleteNote(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить комментарий?", "Да", "Нет" });
                if (answ == "Да")
                {
                    List<Note> lst = new List<Note>();
                    foreach (object? item in param)
                    {
                        lst.Add((Note)item);
                    }
                    var grp = lst.GroupBy(x => x.Order);
                    foreach (var group in grp)
                    {
                        var item = group.FirstOrDefault();
                        if (item != null)
                        {
                            foreach (Note it in Storage.Notes)
                            {
                                if (it.Order > item.Order)
                                {
                                    it.Order = it.Order - 1;
                                }
                            }
                            foreach (Note nt in lst)
                            {
                                Storage.Notes.Remove((Note)nt);
                            }
                        }
                    }
                    Storage.Sort();
                }
            }
        }

        private async Task _DeleteRow(IEnumerable param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить строчку?", "Да", "Нет" });
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
                            Storage[Storage.FormNum_DB].Remove(item);
                        }
                    }

                    Storage.Sort();
                }
            }
        }

        private async Task _PasteRows(IEnumerable<Control> param)
        {
            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                var first = param.FirstOrDefault();
                if (first is Cell)
                {
                    string? text = await clip.GetTextAsync();
                    bool _flag = false;
                    int Row = param.Min(x => ((Cell)x).CellRow);
                    int MinRow = Row;
                    int Column = param.Min(x => ((Cell)x).CellColumn);
                    int MinColumn = Column;

                    if (text != null && text != "")
                    {
                        string rt = "";
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text[i] == '\"')
                            {
                                _flag = !_flag;
                            }
                            else
                            {
                                if (text[i] == '\r' || text[i] == '\n')
                                {
                                    if (text[i] == '\r')
                                    {
                                        if (i + 1 < text.Length)
                                        {
                                            if (text[i + 1] == '\n')
                                            {
                                                if (_flag)
                                                {
                                                    rt += text[i + 1];
                                                }
                                                else
                                                {
                                                    var cell = param.Where(x => ((Cell)x).CellRow == Row && ((Cell)x).CellColumn == Column).FirstOrDefault();
                                                    if (cell != null)
                                                    {
                                                        var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                                                        if (child != null)
                                                        {
                                                            var panel = (Panel)child.Child;
                                                            var textbox = (TextBox)panel.Children.FirstOrDefault();

                                                            if (textbox.TextWrapping == TextWrapping.Wrap)
                                                            {
                                                                textbox.Text = rt;
                                                            }
                                                            else
                                                            {
                                                                textbox.Text = rt.Replace("\t", "").Replace("\r", "").Replace("\n", "");
                                                            }
                                                        }
                                                    }
                                                    rt = "";
                                                    Row++;
                                                    Column = MinColumn;
                                                }
                                                i++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!_flag)
                                        {
                                            var cell = param.Where(x => ((Cell)x).CellRow == Row && ((Cell)x).CellColumn == Column).FirstOrDefault();
                                            if (cell != null)
                                            {
                                                var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                                                if (child != null)
                                                {
                                                    var panel = (Panel)child.Child;
                                                    var textbox = (TextBox)panel.Children.FirstOrDefault();

                                                    if (textbox.TextWrapping == TextWrapping.Wrap)
                                                    {
                                                        textbox.Text = rt;
                                                    }
                                                    else
                                                    {
                                                        textbox.Text = rt.Replace("\t", "").Replace("\r", "").Replace("\n", "");
                                                    }
                                                }
                                            }
                                            rt = "";
                                            Row++;
                                            Column = MinColumn;
                                        }
                                        else
                                        {
                                            rt += text[i];
                                        }
                                    }
                                }
                                else
                                {
                                    if (text[i] == '\t')
                                    {
                                        var cell = param.Where(x => ((Cell)x).CellRow == Row && ((Cell)x).CellColumn == Column).FirstOrDefault();
                                        if (cell != null)
                                        {
                                            var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                                            if (child != null)
                                            {
                                                var panel = (Panel)child.Child;
                                                var textbox = (TextBox)panel.Children.FirstOrDefault();
                                                if (textbox.TextWrapping == TextWrapping.Wrap)
                                                {
                                                    textbox.Text = rt;
                                                }
                                                else
                                                {
                                                    textbox.Text = rt.Replace("\t", "").Replace("\r", "").Replace("\n", "");
                                                }
                                            }
                                        }
                                        rt = "";
                                        Column++;
                                    }
                                    else
                                    {
                                        rt += text[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private async Task _CopyRows(IEnumerable<Control> param)
        {
            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string txt = "";

                var first = param.FirstOrDefault();
                if (first is Cell)
                {

                    var ord = param.GroupBy(x => ((Cell)x).CellRow);
                    foreach (var item in ord)
                    {
                        var t = item.OrderBy(x => ((Cell)x).CellColumn);
                        foreach (var it in t)
                        {
                            var cell = (Cell)it;
                            var child = (Border)cell.GetLogicalChildren().FirstOrDefault();
                            if (child != null)
                            {
                                var panel = (Panel)child.Child;
                                var textbox = (TextBox)panel.Children.FirstOrDefault();
                                if (textbox != null)
                                {
                                    if (textbox.Text != null)
                                    {
                                        if (textbox.Text.Contains("\n"))
                                        {
                                            txt += "\"" + textbox.Text.Replace("\t", "").Replace("\r", "").Replace("\n", "\r\n") + "\"";
                                        }
                                        else
                                        {
                                            txt += textbox.Text;
                                        }
                                    }
                                    txt += "\t";
                                }
                            }
                        }
                        txt += "\n";
                    }
                }
                await clip.ClearAsync();
                await clip.SetTextAsync(txt);
            }
        }

        private async Task _DuplicateNotes()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = await ShowDialog.Handle(desktop.MainWindow);

                if (t > 0)
                {
                    List<Note> lst = new List<Note>();
                    var number = 0;
                    var r= GetNumberInOrder(Storage.Notes);
                    for (int i = 0; i < t; i++)
                    {
                        var frm = new Note();
                        frm.Order = r;
                        lst.Add(frm);
                        number++;
                        r++;
                    }
                    Storage.Notes.AddRange(lst);
                }
            }
        }

        private async Task _DuplicateRowsx1()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = await ShowDialog.Handle(desktop.MainWindow);
                if (t > 0)
                {
                    List<Form> lst = new List<Form>();
                    var number = GetNumberInOrder(Storage.Rows);
                    for (int i = 0; i < t; i++)
                    {
                        var frm = FormCreator.Create(FormType);
                        frm.NumberInOrder_DB = number;
                        lst.Add(frm);
                        number++;
                    }

                    Storage.Rows.AddRange(lst);
                    Storage.Sort();
                }
            }
        }
    }
}
