using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Views;
using Collections;
using DBRealization;
using Models.Abstracts;
using ReactiveUI;

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {
        private DBObservable _local_Reports = new();

        public MainWindowVM()
        {
            Local_Reports = new DBObservable();
            var dbm = StaticConfiguration.DBModel;
            var t = dbm.Database.EnsureCreated();

            dbm.LoadTables();
            if (dbm.DBObservableDbSet.Local.Count() == 0) dbm.DBObservableDbSet.Add(new DBObservable());

            dbm.SaveChanges();
            Local_Reports = dbm.DBObservableDbSet.Local.First();
            if (dbm.ReportsCollectionDbSet.Count() == 0) Local_Reports.Reports_Collection.Add(new Reports());

            dbm.SaveChanges();
            //Local_Reports = new DBObservable();
            //var rpt = new Reports();
            //rpt.Report_Collection.Add(new Report());
            //Local_Reports.Reports_Collection.Add(rpt);


            Local_Reports.PropertyChanged += Local_ReportsChanged;

            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);
            ChangeForm =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IChanged>>(_ChangeForm);
            DeleteForm =
                ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IChanged>>(_DeleteForm);

            Excel_Export = ReactiveCommand.CreateFromTask(_Excel_Export);
        }

        public DBObservable Local_Reports
        {
            get => _local_Reports;
            set
            {
                if (_local_Reports != value)
                {
                    _local_Reports = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ReactiveCommand<Unit, Unit> OpenSettings { get; }

        public ReactiveCommand<string, Unit> AddSort { get; }

        public ReactiveCommand<string, Unit> ChooseForm { get; }

        public ReactiveCommand<string, Unit> AddForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IChanged>, Unit> ChangeForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IChanged>, Unit> DeleteForm { get; }
        public ReactiveCommand<Unit, Unit> Excel_Export { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void _AddSort(string param)
        {
            var type = param.Split('/')[0];
            var path = param.Split('/')[1];

            //FormModel_Local.Dictionary.Filters.SortPath = path;
        }

        private async Task _AddForm(string param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var rt = new Report();
                Local_Reports.Reports_Collection.First().Report_Collection.Add(rt);
                FormChangeOrCreate frm = new(param, rt);
                await frm.ShowDialog<Form>(desktop.MainWindow);
            }
        }

        private async Task _ChangeForm(ObservableCollectionWithItemPropertyChanged<IChanged> param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                if (param != null)
                {
                    var obj = param.First();
                    if (obj != null)
                    {
                        var rep = (Report) obj;
                        FormChangeOrCreate frm = new(rep.FormNum.Value, rep);
                        await frm.ShowDialog(desktop.MainWindow);
                    }
                }
        }

        private async Task _DeleteForm(ObservableCollectionWithItemPropertyChanged<IChanged> param)
        {
            if (param != null)
                foreach (var item in param)
                    Local_Reports.Reports_Collection.First().Report_Collection.Remove((Report) item);
        }

        private async Task _Excel_Export()
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel"
                };
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

        private void Local_ReportsChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("Local_Reports");
        }
    }
}