using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Collections;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        string _DBPath = @"C:\Databases\local.raodb";
        string DBPath
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

        private DBObservable _local_Reports = new DBObservable();
        public DBObservable Local_Reports
        {
            get
            {
                return _local_Reports;
            }
            set
            {
                if (_local_Reports != value)
                {
                    _local_Reports = value;
                    NotifyPropertyChanged("Local_Reports");
                }
            }
        }

        public ReactiveCommand<Unit, Unit> OpenSettings { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }

        public ReactiveCommand<string, Unit> ChooseForm { get; }

        public ReactiveCommand<string, Unit> AddForm { get; }
        public ReactiveCommand<ObservableCollection<object>, Unit> ChangeForm { get; }
        public ReactiveCommand<ObservableCollection<object>, Unit> DeleteForm { get; }
        public ReactiveCommand<Unit, Unit> Excel_Export { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DBRealization.DBModel dbm { get; set; }
        public MainWindowVM()
        {
            dbm = new DBRealization.DBModel(_DBPath);
            var t = dbm.Database.EnsureCreated();

            dbm.LoadAllTables();

            if (dbm.coll_reports.Count() != 0)
            {
                Local_Reports = dbm.coll_reports.First();
            }
            else
            {
                Local_Reports = new DBObservable();
                Local_Reports.Reports_Collection.Add(new Reports());
                dbm.coll_reports.Add(Local_Reports);
            }
            dbm.SaveChanges();

            Local_Reports.PropertyChanged += Local_ReportsChanged;

            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);
            ChangeForm = ReactiveCommand.CreateFromTask<ObservableCollection<object>>(_ChangeForm);
            DeleteForm = ReactiveCommand.CreateFromTask<ObservableCollection<object>>(_DeleteForm);

            Excel_Export = ReactiveCommand.CreateFromTask(_Excel_Export);

        }

        void _AddSort(string param)
        {
            var type = param.Split('/')[0];
            var path = param.Split('/')[1];

            //FormModel_Local.Dictionary.Filters.SortPath = path;
        }

        async Task _AddForm(string param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var rt = new Report();
                rt.FormNum = param;
                //var obj= dbm.coll_reports
                //if (dbm.coll_reports.Count() == 0)
                //{
                //    var tmp = new DBObservable();
                //    dbm.coll_reports.Add(new DBObservable());
                //    tmp.Reps.Add(new Reports());
                //    dbm.SaveChanges();
                //}
                var obj = dbm.coll_reports.Find(1).Reports_Collection[0];
                obj.Report_Collection.Add(rt);
                ObservableCollection<object> lst = new ObservableCollection<object>();
                lst.Add(rt);
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(param, DBPath, lst);
                await frm.ShowDialog<Models.Abstracts.Form>(desktop.MainWindow);
                dbm.SaveChanges();
            }
        }

        async Task _ChangeForm(ObservableCollection<object> param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (param != null)
                {
                    if (param.Count != 0)
                    {
                        Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate("11", DBPath, param);
                        await frm.ShowDialog(desktop.MainWindow);
                    }
                }
            }
        }
        async Task _DeleteForm(ObservableCollection<object> param)
        {
            if (param != null)
            {
                if (param.Count != 0)
                {
                    foreach (var item in param)
                    {
                        dbm.coll_reports.Find(1).Reports_Collection[0].Report_Collection.Remove((Report)item);
                    }
                    dbm.SaveChanges();
                }
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
                if (res.Count() != 0)
                {
                    //Models.Saving.Excel exp = new Models.Saving.Excel();
                    //await exp.Save(FormModel_Local.Dictionary,res);
                }
            }
        }

        void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Local_Reports");
        }
    }
}
