using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Collections;
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
using DBRealization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
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

        public ReactiveCommand<Unit, Unit> CheckReport { get; }
        public ReactiveCommand<string, Unit> AddSort { get; }
        public ReactiveCommand<string, Unit> AddNote { get; }
        public ReactiveCommand<Unit, Unit> AddRow { get; }
        public ReactiveCommand<IList, Unit> DeleteRow { get; }

        public ReactiveCommand<IList, Unit> DuplicateRowsx1 { get; }
        public ReactiveCommand<IList, Unit> DuplicateRowsx10 { get; }
        public ReactiveCommand<IList, Unit> DuplicateRowsx100 { get; }
        public ReactiveCommand<IList, Unit> DuplicateRowsx1000 { get; }
        public ReactiveCommand<IList, Unit> DuplicateRowsx10_000 { get; }
        public ReactiveCommand<IList, Unit> DuplicateRowsx100_000 { get; }
        public ReactiveCommand<IList, Unit> CopyRows { get; }
        public ReactiveCommand<IList, Unit> PasteRows { get; }
        public ReactiveCommand<IList, Unit> DeleteNote { get; }
        public ReactiveCommand<Unit, Unit> PasteNotes { get; }
        public ChangeOrCreateVM()
        {
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            AddRow = ReactiveCommand.Create(_AddRow);
            DeleteRow = ReactiveCommand.Create<IList>(_DeleteRow);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            PasteRows = ReactiveCommand.CreateFromTask<IList>(_PasteRows);
            DuplicateRowsx1 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx1);
            DuplicateRowsx10 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx10);
            DuplicateRowsx100 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx100);
            DuplicateRowsx1000 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx1000);
            DuplicateRowsx10_000 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx10_000);
            DuplicateRowsx100_000 = ReactiveCommand.CreateFromTask<IList>(_DuplicateRowsx100_000);
            CopyRows = ReactiveCommand.CreateFromTask<IList>(_CopyRows);
            AddNote = ReactiveCommand.Create<string>(_AddNote);
            DeleteNote = ReactiveCommand.Create<IList>(_DeleteNote);
            //PasteNotes = ReactiveCommand.CreateFromTask(_PasteNotes);
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
            #region SomeShit
            int k = 1;
            switch (Storage.LastAddedForm)
            {
                case Report.Forms.Form11:
                    foreach (var item in Storage.Rows11)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form12:
                    foreach (var item in Storage.Rows12)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form13:
                    foreach (var item in Storage.Rows13)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form14:
                    foreach (var item in Storage.Rows14)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form15:
                    foreach (var item in Storage.Rows15)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form16:
                    foreach (var item in Storage.Rows16)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form17:
                    foreach (var item in Storage.Rows17)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form18:
                    foreach (var item in Storage.Rows18)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form19:
                    foreach (var item in Storage.Rows19)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form21:
                    foreach (var item in Storage.Rows21)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form22:
                    foreach (var item in Storage.Rows22)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form23:
                    foreach (var item in Storage.Rows23)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form24:
                    foreach (var item in Storage.Rows24)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form25:
                    foreach (var item in Storage.Rows25)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form26:
                    foreach (var item in Storage.Rows26)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form27:
                    foreach (var item in Storage.Rows27)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form28:
                    foreach (var item in Storage.Rows28)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form29:
                    foreach (var item in Storage.Rows29)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form210:
                    foreach (var item in Storage.Rows210)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form211:
                    foreach (var item in Storage.Rows211)
                        item.NumberInOrder.Value = k++;
                    break;
                case Report.Forms.Form212:
                    foreach (var item in Storage.Rows212)
                        item.NumberInOrder.Value = k++;
                    break;
                default: break;
            }
#endregion

            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (Avalonia.Controls.Window? item in desktop.Windows)
                {
                    if (item is Views.FormChangeOrCreate)
                    {
                        var t =item as Views.FormChangeOrCreate;
                        if (t.Str != null)
                        {
                            t.Str.Report_Collection.Add(Storage);
                        }

                        if (t.DBO != null)
                        {
                            var tmp = new Reports();
                            tmp.Master = Storage;
                            t.DBO.Reports_Collection.Add(tmp);
                        }

                        var dbm = StaticConfiguration.DBModel;
                        dbm.SaveChanges();
                        item.Close();
                    }
                }
            }


            switch (Storage.LastAddedForm)
            {
                case Report.Forms.Form10:
                    Storage.OkpoRep = Storage.OkpoRep;
                    Storage.RegNoRep = Storage.RegNoRep;
                    Storage.ShortJurLicoRep = Storage.ShortJurLicoRep;
                    break;
                case Report.Forms.Form20:
                    Storage.OkpoRep1 = Storage.OkpoRep1;
                    Storage.RegNoRep1 = Storage.RegNoRep1;
                    Storage.ShortJurLicoRep1 = Storage.ShortJurLicoRep1;
                    break;
            }
        }

        private void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
            //switch (FormType)
            //{
            //    case "1.0":
            //        foreach (var item in Storage.Rows10)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.1":
            //        foreach (var item in Storage.Rows11)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.2":
            //        foreach (var item in Storage.Rows12)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.3":
            //        foreach (var item in Storage.Rows13)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.4":
            //        foreach (var item in Storage.Rows14)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.5":
            //        foreach (var item in Storage.Rows15)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.6":
            //        foreach (var item in Storage.Rows16)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.7":
            //        foreach (var item in Storage.Rows17)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.8":
            //        foreach (var item in Storage.Rows18)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "1.9":
            //        foreach (var item in Storage.Rows19)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.0":
            //        foreach (var item in Storage.Rows20)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.1":
            //        foreach (var item in Storage.Rows21)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.2":
            //        foreach (var item in Storage.Rows22)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.3":
            //        foreach (var item in Storage.Rows23)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.4":
            //        foreach (var item in Storage.Rows24)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.5":
            //        foreach (var item in Storage.Rows25)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.6":
            //        foreach (var item in Storage.Rows26)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.7":
            //        foreach (var item in Storage.Rows27)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.8":
            //        foreach (var item in Storage.Rows28)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.9":
            //        foreach (var item in Storage.Rows29)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.10":
            //        foreach (var item in Storage.Rows210)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.11":
            //        foreach (var item in Storage.Rows211)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //    case "2.12":
            //        foreach (var item in Storage.Rows212)
            //        {
            //            if (!item.Object_Validation())
            //            {
            //                IsCanSaveReportEnabled = false;
            //                return;
            //            }
            //        }
            //        IsCanSaveReportEnabled = true;
            //        return;
            //}
            Storage.Rows17.Sum();
            Storage.Rows18.Sum();
            Storage.Rows21.Sum();
            Storage.Rows22.Sum();
        }
        public void _AddRow10()
        {
            Form10? frm = new Form10(); Storage.Rows10.Add(frm);
        }
        public void _AddRow20()
        {
            Form20? frm = new Form20(); Storage.Rows20.Add(frm);
        }

        private void _AddRow()
        {
            var frm = FormCreator.Create(FormType);
            if (FormType == "1.1") { Storage.Rows11.Add((Form11)frm); Storage.LastAddedForm = Report.Forms.Form11; }
            if (FormType == "1.2") { Storage.Rows12.Add((Form12)frm); Storage.LastAddedForm = Report.Forms.Form12; }
            if (FormType == "1.3") { Storage.Rows13.Add((Form13)frm); Storage.LastAddedForm = Report.Forms.Form13; }
            if (FormType == "1.4") { Storage.Rows14.Add((Form14)frm); Storage.LastAddedForm = Report.Forms.Form14; }
            if (FormType == "1.5") { Storage.Rows15.Add((Form15)frm); Storage.LastAddedForm = Report.Forms.Form15; }
            if (FormType == "1.6") { Storage.Rows16.Add((Form16)frm); Storage.LastAddedForm = Report.Forms.Form16; }
            if (FormType == "1.7") { Storage.Rows17.Add((Form17)frm); Storage.LastAddedForm = Report.Forms.Form17; }
            if (FormType == "1.8") { Storage.Rows18.Add((Form18)frm); Storage.LastAddedForm = Report.Forms.Form18; }
            if (FormType == "1.9") { Storage.Rows19.Add((Form19)frm); Storage.LastAddedForm = Report.Forms.Form19; }

            if (FormType == "2.1") { Storage.Rows21.Add((Form21)frm); Storage.LastAddedForm = Report.Forms.Form21; }
            if (FormType == "2.2") { Storage.Rows22.Add((Form22)frm); Storage.LastAddedForm = Report.Forms.Form22; }
            if (FormType == "2.3") { Storage.Rows23.Add((Form23)frm); Storage.LastAddedForm = Report.Forms.Form23; }
            if (FormType == "2.4") { Storage.Rows24.Add((Form24)frm); Storage.LastAddedForm = Report.Forms.Form24; }
            if (FormType == "2.5") { Storage.Rows25.Add((Form25)frm); Storage.LastAddedForm = Report.Forms.Form25; }
            if (FormType == "2.6") { Storage.Rows26.Add((Form26)frm); Storage.LastAddedForm = Report.Forms.Form26; }
            if (FormType == "2.7") { Storage.Rows27.Add((Form27)frm); Storage.LastAddedForm = Report.Forms.Form27; }
            if (FormType == "2.8") { Storage.Rows28.Add((Form28)frm); Storage.LastAddedForm = Report.Forms.Form28; }
            if (FormType == "2.9") { Storage.Rows29.Add((Form29)frm); Storage.LastAddedForm = Report.Forms.Form29; }
            if (FormType == "2.10") { Storage.Rows210.Add((Form210)frm); Storage.LastAddedForm = Report.Forms.Form210; }
            if (FormType == "2.11") { Storage.Rows211.Add((Form211)frm); Storage.LastAddedForm = Report.Forms.Form211; }
            if (FormType == "2.12") { Storage.Rows212.Add((Form212)frm); Storage.LastAddedForm = Report.Forms.Form212; }
        }

        private void _AddNote(string Param)
        {
            Note? nt = new Note(); 
            Storage.Notes.Add(nt);
        }

        private void _DeleteNote(IEnumerable param)
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

        private void _DeleteRow(IEnumerable param)
        {
            List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
            foreach (object? item in param)
            {
                lst.Add((Models.Abstracts.Form)item);
            }
            foreach (Models.Abstracts.Form? item in lst)
            {
                if (item != null)
                {
                    if (item.FormNum.Value == "1.1")
                    {
                        Storage.Rows11.Remove((Form11) item);
                    }

                    if (item.FormNum.Value == "1.2")
                    {
                        Storage.Rows12.Remove((Form12) item);
                    }

                    if (item.FormNum.Value == "1.3")
                    {
                        Storage.Rows13.Remove((Form13) item);
                    }

                    if (item.FormNum.Value == "1.4")
                    {
                        Storage.Rows14.Remove((Form14) item);
                    }

                    if (item.FormNum.Value == "1.5")
                    {
                        Storage.Rows15.Remove((Form15) item);
                    }

                    if (item.FormNum.Value == "1.6")
                    {
                        Storage.Rows16.Remove((Form16) item);
                    }

                    if (item.FormNum.Value == "1.7")
                    {
                        Storage.Rows17.Remove((Form17) item);
                    }

                    if (item.FormNum.Value == "1.8")
                    {
                        Storage.Rows18.Remove((Form18) item);
                    }

                    if (item.FormNum.Value == "1.9")
                    {
                        Storage.Rows19.Remove((Form19) item);
                    }

                    if (item.FormNum.Value == "2.1")
                    {
                        Storage.Rows21.Remove((Form21) item);
                    }

                    if (item.FormNum.Value == "2.2")
                    {
                        Storage.Rows22.Remove((Form22) item);
                    }

                    if (item.FormNum.Value == "2.3")
                    {
                        Storage.Rows23.Remove((Form23) item);
                    }

                    if (item.FormNum.Value == "2.4")
                    {
                        Storage.Rows24.Remove((Form24) item);
                    }

                    if (item.FormNum.Value == "2.5")
                    {
                        Storage.Rows25.Remove((Form25) item);
                    }

                    if (item.FormNum.Value == "2.6")
                    {
                        Storage.Rows26.Remove((Form26) item);
                    }

                    if (item.FormNum.Value == "2.7")
                    {
                        Storage.Rows27.Remove((Form27) item);
                    }

                    if (item.FormNum.Value == "2.8")
                    {
                        Storage.Rows28.Remove((Form28) item);
                    }

                    if (item.FormNum.Value == "2.9")
                    {
                        Storage.Rows29.Remove((Form29) item);
                    }

                    if (item.FormNum.Value == "2.10")
                    {
                        Storage.Rows210.Remove((Form210) item);
                    }

                    if (item.FormNum.Value == "2.11")
                    {
                        Storage.Rows211.Remove((Form211) item);
                    }

                    if (item.FormNum.Value == "2.12")
                    {
                        Storage.Rows212.Remove((Form212) item);
                    }
                }
            }
            Storage.Rows17.Sum();
            Storage.Rows18.Sum();
            Storage.Rows21.Sum();
            Storage.Rows22.Sum();
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
                    cl = (Cell) item;
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

        private async Task _DuplicateRowsx1(IEnumerable param)
        {
            var frm = FormCreator.Create(FormType);
            foreach (var it in param)
            {
                var item = frm;
                if (FormType == "1.1") { Storage.Rows11.Add((Form11)item); Storage.LastAddedForm = Report.Forms.Form11;}
                if (FormType == "1.2") { Storage.Rows12.Add((Form12)item); Storage.LastAddedForm = Report.Forms.Form12;}
                if (FormType == "1.3") { Storage.Rows13.Add((Form13)item); Storage.LastAddedForm = Report.Forms.Form13;}
                if (FormType == "1.4") { Storage.Rows14.Add((Form14)item); Storage.LastAddedForm = Report.Forms.Form14;}
                if (FormType == "1.5") { Storage.Rows15.Add((Form15)item); Storage.LastAddedForm = Report.Forms.Form15;}
                if (FormType == "1.6") { Storage.Rows16.Add((Form16)item); Storage.LastAddedForm = Report.Forms.Form16;}
                if (FormType == "1.7") { Storage.Rows17.Add((Form17)item); Storage.LastAddedForm = Report.Forms.Form17;}
                if (FormType == "1.8") { Storage.Rows18.Add((Form18)item); Storage.LastAddedForm = Report.Forms.Form18;}
                if (FormType == "1.9") { Storage.Rows19.Add((Form19)item); Storage.LastAddedForm = Report.Forms.Form19;}

                if (FormType == "2.1") { Storage.Rows21.Add((Form21)item); Storage.LastAddedForm = Report.Forms.Form21;}
                if (FormType == "2.2") { Storage.Rows22.Add((Form22)item); Storage.LastAddedForm = Report.Forms.Form22;}
                if (FormType == "2.3") { Storage.Rows23.Add((Form23)item); Storage.LastAddedForm = Report.Forms.Form23;}
                if (FormType == "2.4") { Storage.Rows24.Add((Form24)item); Storage.LastAddedForm = Report.Forms.Form24;}
                if (FormType == "2.5") { Storage.Rows25.Add((Form25)item); Storage.LastAddedForm = Report.Forms.Form25;}
                if (FormType == "2.6") { Storage.Rows26.Add((Form26)item); Storage.LastAddedForm = Report.Forms.Form26;}
                if (FormType == "2.7") { Storage.Rows27.Add((Form27)item); Storage.LastAddedForm = Report.Forms.Form27;}
                if (FormType == "2.8") { Storage.Rows28.Add((Form28)item); Storage.LastAddedForm = Report.Forms.Form28;}
                if (FormType == "2.9") { Storage.Rows29.Add((Form29)item); Storage.LastAddedForm = Report.Forms.Form29;}
                if (FormType == "2.10") { Storage.Rows210.Add((Form210)item); Storage.LastAddedForm = Report.Forms.Form210; }
                if (FormType == "2.11") { Storage.Rows211.Add((Form211)item); Storage.LastAddedForm = Report.Forms.Form211; }
                if (FormType == "2.12") { Storage.Rows212.Add((Form212)item); Storage.LastAddedForm = Report.Forms.Form212; }
            }
        }
        private async Task _DuplicateRowsx10(IEnumerable param)
        {
            for(int i=0;i<10;i++)
            {
                _DuplicateRowsx1(param);
            }
        }
        private async Task _DuplicateRowsx100(IEnumerable param)
        {
            for (int i = 0; i < 100; i++)
            {
                _DuplicateRowsx1(param);
            }
        }
        private async Task _DuplicateRowsx1000(IEnumerable param)
        {
            for (int i = 0; i < 1000; i++)
            {
                _DuplicateRowsx1(param);
            }
        }
        private async Task _DuplicateRowsx10_000(IEnumerable param)
        {
            for (int i = 0; i < 10000; i++)
            {
                _DuplicateRowsx1(param);
            }
        }
        private async Task _DuplicateRowsx100_000(IEnumerable param)
        {
            for (int i = 0; i < 100000; i++)
            {
                _DuplicateRowsx1(param);
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
                    var child=(Border)cell.GetLogicalChildren().FirstOrDefault();
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
    }
}
