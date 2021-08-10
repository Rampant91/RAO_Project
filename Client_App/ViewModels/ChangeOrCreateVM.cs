using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Collections;
using Models;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DBRealization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;

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

        public ReactiveCommand<Unit, Unit> CheckReport { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }
        public ReactiveCommand<string, Unit> AddNote { get; }
        public ReactiveCommand<Unit, Unit> AddRow { get; }
        public ReactiveCommand<IList, Unit> DeleteRow { get; }

        public ReactiveCommand<Unit, Unit> PasteRows { get; }

        public ReactiveCommand<IList, Unit> DeleteNote { get; }

        public ReactiveCommand<Unit, Unit> PasteNotes { get; }

        public ChangeOrCreateVM()
        {
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            AddRow = ReactiveCommand.Create(_AddRow);
            DeleteRow = ReactiveCommand.Create<IList>(_DeleteRow);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            PasteRows = ReactiveCommand.CreateFromTask(_PasteRows);
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
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (Avalonia.Controls.Window? item in desktop.Windows)
                {
                    if (item is Views.FormChangeOrCreate)
                    {
                        var dbm = StaticConfiguration.DBModel;
                        dbm.SaveChanges();
                        item.Close();
                    }
                }
            }
        }

        private void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
        }

        private void _AddRow()
        {
            if (FormType == "1.0") { Form10? frm = new Form10();
                frm.FormNum.Value = FormType; 
                Storage.Rows10.Add(frm); }
            if (FormType == "1.1") { Form11? frm = new Form11();
                frm.FormNum.Value = FormType; 
                Storage.Rows11.Add(frm); }
            if (FormType == "1.2") { Form12? frm = new Form12();
                frm.FormNum.Value = FormType; 
                Storage.Rows12.Add(frm); }
            if (FormType == "1.3") { Form13? frm = new Form13();
                frm.FormNum.Value = FormType; 
                Storage.Rows13.Add(frm); }
            if (FormType == "1.4") { Form14? frm = new Form14();
                frm.FormNum.Value = FormType; 
                Storage.Rows14.Add(frm); }
            if (FormType == "1.5") { Form15? frm = new Form15();
                frm.FormNum.Value = FormType; 
                Storage.Rows15.Add(frm); }
            if (FormType == "1.6") { Form16? frm = new Form16();
                frm.FormNum.Value = FormType; 
                Storage.Rows16.Add(frm); }
            if (FormType == "1.7") { Form17? frm = new Form17();
                frm.FormNum.Value = FormType; 
                Storage.Rows17.Add(frm); }
            if (FormType == "1.8") { Form18? frm = new Form18();
                frm.FormNum.Value = FormType; 
                Storage.Rows18.Add(frm); }
            if (FormType == "1.9") { Form19? frm = new Form19();
                frm.FormNum.Value = FormType; 
                Storage.Rows19.Add(frm); }

            if (FormType == "2.0") { Form20? frm = new Form20();
                frm.FormNum.Value = FormType; 
                Storage.Rows20.Add(frm); }
            if (FormType == "2.1") { Form21? frm = new Form21();
                frm.FormNum.Value = FormType; 
                Storage.Rows21.Add(frm); }
            if (FormType == "2.2") { Form22? frm = new Form22();
                frm.FormNum.Value = FormType; 
                Storage.Rows22.Add(frm); }
            if (FormType == "2.3") { Form23? frm = new Form23();
                frm.FormNum.Value = FormType; 
                Storage.Rows23.Add(frm); }
            if (FormType == "2.4") { Form24? frm = new Form24();
                frm.FormNum.Value = FormType; 
                Storage.Rows24.Add(frm); }
            if (FormType == "2.5") { Form25? frm = new Form25();
                frm.FormNum.Value = FormType; 
                Storage.Rows25.Add(frm); }
            if (FormType == "2.6") { Form26? frm = new Form26();
                frm.FormNum.Value = FormType; 
                Storage.Rows26.Add(frm); }
            if (FormType == "2.7") { Form27? frm = new Form27();
                frm.FormNum.Value = FormType; 
                Storage.Rows27.Add(frm); }
            if (FormType == "2.8") { Form28? frm = new Form28();
                frm.FormNum.Value = FormType; 
                Storage.Rows28.Add(frm); }
            if (FormType == "2.9") { Form29? frm = new Form29();
                frm.FormNum.Value = FormType; 
                Storage.Rows29.Add(frm); }
            if (FormType == "2.10") { Form210? frm = new Form210();
                frm.FormNum.Value = FormType; 
                Storage.Rows210.Add(frm); }
            if (FormType == "2.11") { Form211? frm = new Form211();
                frm.FormNum.Value = FormType; 
                Storage.Rows211.Add(frm); }
            if (FormType == "2.12") { Form212? frm = new Form212();
                frm.FormNum.Value = FormType; 
                Storage.Rows212.Add(frm); }
        }
        private void _AddRow(Form frm)
        {
            if (FormType == "1.0")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows10.Add((Form10)frm);
            }
            if (FormType == "1.1")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows11.Add((Form11)frm);
            }
            if (FormType == "1.2")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows12.Add((Form12)frm);
            }
            if (FormType == "1.3")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows13.Add((Form13)frm);
            }
            if (FormType == "1.4")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows14.Add((Form14)frm);
            }
            if (FormType == "1.5")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows15.Add((Form15)frm);
            }
            if (FormType == "1.6")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows16.Add((Form16)frm);
            }
            if (FormType == "1.7")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows17.Add((Form17)frm);
            }
            if (FormType == "1.8")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows18.Add((Form18)frm);
            }
            if (FormType == "1.9")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows19.Add((Form19)frm);
            }

            if (FormType == "2.0")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows20.Add((Form20)frm);
            }
            if (FormType == "2.1")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows21.Add((Form21)frm);
            }
            if (FormType == "2.2")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows22.Add((Form22)frm);
            }
            if (FormType == "2.3")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows23.Add((Form23)frm);
            }
            if (FormType == "2.4")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows24.Add((Form24)frm);
            }
            if (FormType == "2.5")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows25.Add((Form25)frm);
            }
            if (FormType == "2.6")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows26.Add((Form26)frm);
            }
            if (FormType == "2.7")
            { 
                frm.FormNum.Value = FormType;
                Storage.Rows27.Add((Form27)frm);
            }
            if (FormType == "2.8")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows28.Add((Form28)frm);
            }
            if (FormType == "2.9")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows29.Add((Form29)frm);
            }
            if (FormType == "2.10")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows210.Add((Form210)frm);
            }
            if (FormType == "2.11")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows211.Add((Form211)frm);
            }
            if (FormType == "2.12")
            {
                frm.FormNum.Value = FormType;
                Storage.Rows212.Add((Form212)frm);
            }
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
                if (item.FormNum.Value == "1.0") { Storage.Rows10.Remove((Form10)item); }
                if (item.FormNum.Value == "1.1") { Storage.Rows11.Remove((Form11)item); }
                if (item.FormNum.Value == "1.2") { Storage.Rows12.Remove((Form12)item); }
                if (item.FormNum.Value == "1.3") { Storage.Rows13.Remove((Form13)item); }
                if (item.FormNum.Value == "1.4") { Storage.Rows14.Remove((Form14)item); }
                if (item.FormNum.Value == "1.5") { Storage.Rows15.Remove((Form15)item); }
                if (item.FormNum.Value == "1.6") { Storage.Rows16.Remove((Form16)item); }
                if (item.FormNum.Value == "1.7") { Storage.Rows17.Remove((Form17)item); }
                if (item.FormNum.Value == "1.8") { Storage.Rows18.Remove((Form18)item); }
                if (item.FormNum.Value == "1.9") { Storage.Rows19.Remove((Form19)item); }

                if (item.FormNum.Value == "2.0") { Storage.Rows20.Remove((Form20)item); }
                if (item.FormNum.Value == "2.1") { Storage.Rows21.Remove((Form21)item); }
                if (item.FormNum.Value == "2.2") { Storage.Rows22.Remove((Form22)item); }
                if (item.FormNum.Value == "2.3") { Storage.Rows23.Remove((Form23)item); }
                if (item.FormNum.Value == "2.4") { Storage.Rows24.Remove((Form24)item); }
                if (item.FormNum.Value == "2.5") { Storage.Rows25.Remove((Form25)item); }
                if (item.FormNum.Value == "2.6") { Storage.Rows26.Remove((Form26)item); }
                if (item.FormNum.Value == "2.7") { Storage.Rows27.Remove((Form27)item); }
                if (item.FormNum.Value == "2.8") { Storage.Rows28.Remove((Form28)item); }
                if (item.FormNum.Value == "2.9") { Storage.Rows29.Remove((Form29)item); }
                if (item.FormNum.Value == "2.10") { Storage.Rows210.Remove((Form210)item); }
                if (item.FormNum.Value == "2.11") { Storage.Rows211.Remove((Form211)item); }
                if (item.FormNum.Value == "2.12") { Storage.Rows212.Remove((Form212)item); }
            }
        }

        private void _AddSort(string param)
        {
            //Storage.Filters.SortPath = param;
        }

        private async Task _PasteRows()
        {
            PasteRealization.Excel ex = new PasteRealization.Excel();

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string? text = await clip.GetTextAsync();
                List<Models.Abstracts.Form>? lt = ex.Convert(text, FormType);
                if (lt != null)
                {
                    foreach (Models.Abstracts.Form? item in lt)
                    {
                        _AddRow(item);
                    }
                }
            }
        }
    }
}
