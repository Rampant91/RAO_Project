using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using Avalonia;
using Models.Client_Model;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Reflection;
using Avalonia.Collections;
using Avalonia.Markup.Xaml;
using System.Collections;
using Models.Attributes;
using Models;
using System.IO;
using Avalonia.Metadata;
using System.Windows;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Form _SavingStorage;
        public Form SavingStorage
        {
            get
            {
                return _SavingStorage;
            }
            set
            {
                if (_SavingStorage != value)
                {
                    _SavingStorage = value;
                    NotifyPropertyChanged("SavingStorage");
                }
            }
        }

        Form _Storage;
        public Form Storage
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

        Forms _Forms;
        public Forms Forms
        {
            get
            {
                return _Forms;
            }
            set
            {
                if (_Forms != value)
                {
                    _Forms = value;
                    NotifyPropertyChanged("Forms");
                }
            }
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

        public ReactiveCommand<Unit, Unit> CheckReport { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }
        public ReactiveCommand<Unit, Unit> AddRow { get; }
        public ReactiveCommand<IList, Unit> DeleteRow { get; }

        public ReactiveCommand<Unit, Unit> PasteRows { get; }

        public ChangeOrCreateVM()
        {
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            AddRow= ReactiveCommand.Create(_AddRow);
            DeleteRow = ReactiveCommand.Create<IList>(_DeleteRow);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            PasteRows = ReactiveCommand.CreateFromTask(_PasteRows);


            _Storage = new Form();
            _Forms = new Forms();
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
            SavingStorage = Storage;
            if (!Forms[FormType].Storage.Contains(_SavingStorage))
            {
                Forms.Forms[FormType].Storage.Add(_SavingStorage);

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
            else
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
        }
        void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
        }

        void _AddRow()
        {
            Storage.Rows.Add(Models.Client_Model.FormCreator.Create(false,FormType));
        }

        void _DeleteRow(IList param)
        {
            List<Form> lst = new List<Form>();
            foreach (var item in param)
            {
                lst.Add((Form)item);
            }
            foreach (var item in lst)
            {
                Storage.Rows.Remove((Form)item);
            }
        }

        void _AddSort(string param)
        {
            Storage.Filters.SortPath = param;
        }

        async Task _PasteRows()
        {
            PasteRealization.Excel ex = new PasteRealization.Excel();

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                var text = await clip.GetTextAsync();
                var lt=ex.Convert(text,FormType);
                if(lt!=null)
                {
                    foreach(var item in lt)
                    {
                        Storage.Rows.Add(item);
                    }
                }
            }
        }
    }
}
