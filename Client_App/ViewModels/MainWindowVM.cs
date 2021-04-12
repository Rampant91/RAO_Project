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
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        LocalDictionary _FormModel_Local;
        public LocalDictionary FormModel_Local 
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

        public ReactiveCommand<Unit, Unit> Save_Local { get; }
        public ReactiveCommand<Unit, Unit> Load_Local { get; }

        public ReactiveCommand<Unit, Unit> Save_ToFile { get; }

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
            _FormModel_Local = new LocalDictionary();
            FormModel_Local.PropertyChanged += FormModelChanged;

            Save_Local = ReactiveCommand.CreateFromTask(SaveForms);
            Load_Local = ReactiveCommand.CreateFromTask(LoadForms);
            Save_ToFile = ReactiveCommand.CreateFromTask(_SaveToFile);
            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);
            ChangeForm = ReactiveCommand.CreateFromTask<Form>(_ChangeForm);
            DeleteForm = ReactiveCommand.CreateFromTask<Form>(_DeleteForm);

            Excel_Export= ReactiveCommand.CreateFromTask(_Excel_Export);

            _FormModel_Local = new LocalDictionary();
        }

        public async Task SaveForms()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new SaveFileDialog();
                var filter = new FileDialogFilter();
                filter.Name = "Excel";
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                var res = await dial.ShowAsync(desktop.MainWindow);
                if (res.Count() != 0)
                {
                    Models.Storage.File fl = new Models.Storage.File();
                    await fl.Save(FormModel_Local, res);
                }
            }
        }
        public async Task LoadForms()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                OpenFileDialog dial = new OpenFileDialog();
                var filter = new FileDialogFilter();
                filter.Name = "Excel";
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                dial.AllowMultiple = false;
                var res = await dial.ShowAsync(desktop.MainWindow);
                if (res.Count() != 0)
                {
                    Models.Storage.File fl = new Models.Storage.File();
                    FormModel_Local=(await fl.Load(res[0]));
                }
            }
        }
        public async Task _SaveToFile()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new SaveFileDialog();
                var filter = new FileDialogFilter();
                filter.Name = "Excel";
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                var res = await dial.ShowAsync(desktop.MainWindow);
                if (res.Count() != 0)
                {
                    Models.Storage.File fl = new Models.Storage.File();
                    await fl.Save(FormModel_Local, res);
                }
            }
        }

        void _AddSort(string param)
        {
            var type = param.Split('/')[0];
            var path = param.Split('/')[1];

            if (type.Length == 1)
            {
                foreach(var item in FormModel_Local.Forms)
                {
                    var ty = item.Key.Replace("Forms", "");
                    if (ty[0]==type[0])
                    {
                        if (ty.Count() > 1)
                        {
                            if (ty[1] != '0')
                            {
                                item.Value.Filters.SortPath = path;
                            }
                        }
                    }
                }
            }
            else
            {
                var str = FormModel_Local.Forms[type];
                str.Filters.SortPath = path;
            }
        }

        async Task _AddForm(string param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(FormModel_Local, null,param);
                await frm.ShowDialog<Form>(desktop.MainWindow);
            }
        }

        async Task _ChangeForm(Form param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(FormModel_Local,param, null);
                await frm.ShowDialog(desktop.MainWindow);
            }
        }
        async Task _DeleteForm(Form param)
        {
            if (param != null)
            {
                FormModel_Local.Forms[param.FormNum].Storage.Remove(param);
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
                    Excel exp = new Excel();
                    await exp.Save(FormModel_Local,res);
                }
            }
        }

        void FormModelChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("FormModel_Local");
        }
    }
}
