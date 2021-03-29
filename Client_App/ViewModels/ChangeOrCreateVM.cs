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
using Models.Storage;
using System.IO;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        Models.Storage.LocalDictionary _Forms;
        public Models.Storage.LocalDictionary Forms
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

        public ReactiveCommand<string, Unit> AddSort { get; }

        public ChangeOrCreateVM()
        {
            AddSort = ReactiveCommand.Create<string>(_AddSort);
            _Storage = new Report();
            _Forms = new LocalDictionary();
        }
        void _AddSort(string param)
        {
            //Storage.Filters.SortPath = param;
        }
    }
}
