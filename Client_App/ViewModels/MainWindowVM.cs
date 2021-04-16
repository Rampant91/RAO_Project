using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using System.Reactive;
using Avalonia;
using Models;
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

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        FormsDictionary _FormModel_Local;
        public FormsDictionary FormModel_Local 
        {
            get
            {
                return _FormModel_Local;
            }
            set
            {
                if(_FormModel_Local!=value)
                {
                    _FormModel_Local = value;
                    NotifyPropertyChanged("FormModel_Local");
                }
            }
        }

        public ReactiveCommand<Unit, Unit> OpenSettings { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }

        public ReactiveCommand<string, Unit> ChooseForm { get;}

        public ReactiveCommand<string, Unit> AddForm { get; }
        public ReactiveCommand<Form, Unit> ChangeForm { get; }
        public ReactiveCommand<Form, Unit> DeleteForm { get; }
        public ReactiveCommand<Unit, Unit> Excel_Export { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindowVM()
        {
            _FormModel_Local = new FormsDictionary("some.raodb");
            FormModel_Local.PropertyChanged += FormModelChanged;

            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);
            ChangeForm = ReactiveCommand.CreateFromTask<Form>(_ChangeForm);
            DeleteForm = ReactiveCommand.CreateFromTask<Form>(_DeleteForm);

            Excel_Export= ReactiveCommand.CreateFromTask(_Excel_Export);

        }

        void _AddSort(string param)
        {
            var type = param.Split('/')[0];
            var path = param.Split('/')[1];

            FormModel_Local.Dictionary.Filters.SortPath = path;
        }

        async Task _AddForm(string param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(FormModel_Local.Dictionary, null,param);
                await frm.ShowDialog<Form>(desktop.MainWindow);
            }
        }

        async Task _ChangeForm(Form param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(FormModel_Local.Dictionary,param, null);
                await frm.ShowDialog(desktop.MainWindow);
            }
        }
        async Task _DeleteForm(Form param)
        {
            if (param != null)
            {
                FormModel_Local.Dictionary.Forms_Collection.Remove(param);
            }
        }

        async Task _Excel_Export()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new SaveFileDialog();
                var filter = new FileDialogFilter();
                filter.Name = "Excel";
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                var res = await dial.ShowAsync(desktop.MainWindow);
                if (res.Count()!=0)
                {
                    Models.Saving.Excel exp = new Models.Saving.Excel();
                    await exp.Save(FormModel_Local.Dictionary,res);
                }
            }
        }

        void FormModelChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("FormModel_Local");
        }
    }
}
