using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Collections;
using Models;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _FormType;
        public string FormType
        {
            get
            {
                return _FormType;
            }
            set
            {
                if (_FormType != value)
                {
                    _FormType = value;
                    NotifyPropertyChanged("FormType");
                }
            }
        }

        string _DBPath = @"C:\Databases\local.raodb";
        public string DBPath
        {
            get
            {
                return _DBPath;
            }
            set
            {
                if (_DBPath != value)
                {
                    _DBPath = value;
                    NotifyPropertyChanged("DBPath");
                }
            }
        }

        Report _Storage;
        public Report Storage
        {
            get
            {
                return _Storage;
            }
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
        public ReactiveCommand<Unit, Unit> AddRow { get; }
        public ReactiveCommand<IList, Unit> DeleteRow { get; }

        public ReactiveCommand<Unit, Unit> PasteRows { get; }

        public ChangeOrCreateVM()
        {
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            AddRow = ReactiveCommand.Create(_AddRow);
            DeleteRow = ReactiveCommand.Create<IList>(_DeleteRow);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            PasteRows = ReactiveCommand.CreateFromTask(_PasteRows);
        }
        bool _isCanSaveReportEnabled = false;
        bool IsCanSaveReportEnabled
        {
            get
            {
                return _isCanSaveReportEnabled;
            }
            set
            {
                if (value == _isCanSaveReportEnabled)
                    return;
                _isCanSaveReportEnabled = value;
                PropertyChanged?
                    .Invoke(this, new PropertyChangedEventArgs(nameof(IsCanSaveReportEnabled)));
            }
        }

        [DependsOn(nameof(IsCanSaveReportEnabled))]
        bool CanSaveReport(object parameter)
        {
            return _isCanSaveReportEnabled;
        }
        public void SaveReport()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                foreach (var item in desktop.Windows)
                {
                    if (item is Views.FormChangeOrCreate)
                    {
                        item.Close();
                    }
                }
            }

        }
        void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
        }

        void _AddRow()
        {
            var frm = new Form11();
            Storage.Rows11.Add(frm);
        }

        void _DeleteRow(IList param)
        {
            List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
            foreach (var item in param)
            {
                lst.Add((Models.Abstracts.Form)item);
            }
            foreach (var item in lst)
            {
                //Storage.Rows.Remove(item);
            }
        }

        void _AddSort(string param)
        {
            //Storage.Filters.SortPath = param;
        }

        async Task _PasteRows()
        {
            PasteRealization.Excel ex = new PasteRealization.Excel();

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                var text = await clip.GetTextAsync();
                var lt = ex.Convert(text, FormType);
                if (lt != null)
                {
                    foreach (var item in lt)
                    {
                        //Storage.Rows.Add(item);
                    }
                }
            }
        }
    }
}
