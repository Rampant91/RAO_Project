using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Collections;
using Collections;
using ReactiveUI;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Client_App.ViewModels
{
    public class MainWindowVM : BaseVM, INotifyPropertyChanged
    {

        private DBObservable _local_Reports = new DBObservable();
        public DBObservable Local_Reports
        {
            get => _local_Reports;
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
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IChanged>, Unit> ChangeForm { get; }
        public ReactiveCommand<ObservableCollectionWithItemPropertyChanged<IChanged>, Unit> DeleteForm { get; }
        public ReactiveCommand<Unit, Unit> Excel_Export { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private DBRealization.DBModel dbm { get; set; }
        public MainWindowVM()
        {
            Local_Reports = new DBObservable();
            dbm = DBRealization.StaticConfiguration.DBModel;
            bool t = dbm.Database.EnsureCreated();

            dbm.LoadTables();
            if(dbm.DBObservable_DbSet.Local.Count()==0)
            {
                dbm.DBObservable_DbSet.Add(new DBObservable());
            }
            dbm.SaveChanges();
            Local_Reports = dbm.DBObservable_DbSet.Local.First();
            if (dbm.ReportsCollection_DbSet.Count() == 0)
            {
                Local_Reports.Reports_Collection.Add(new Reports());
            }

            dbm.SaveChanges();
            //Local_Reports = new DBObservable();
            //var rpt = new Reports();
            //rpt.Report_Collection.Add(new Report());
            //Local_Reports.Reports_Collection.Add(rpt);


            Local_Reports.PropertyChanged += Local_ReportsChanged;

            AddSort = ReactiveCommand.Create<string>(_AddSort);

            AddForm = ReactiveCommand.CreateFromTask<string>(_AddForm);
            ChangeForm = ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IChanged>>(_ChangeForm);
            DeleteForm = ReactiveCommand.CreateFromTask<ObservableCollectionWithItemPropertyChanged<IChanged>>(_DeleteForm);

            Excel_Export = ReactiveCommand.CreateFromTask(_Excel_Export);

        }

        private void _AddSort(string param)
        {
            string? type = param.Split('/')[0];
            string? path = param.Split('/')[1];

            //FormModel_Local.Dictionary.Filters.SortPath = path;
        }

        private async Task _AddForm(string param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Report? rt = new Report();
                Local_Reports.Reports_Collection.First().Report_Collection.Add(rt);
                Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(param, rt);
                await frm.ShowDialog<Models.Abstracts.Form>(desktop.MainWindow);
            }
        }

        private async Task _ChangeForm(ObservableCollectionWithItemPropertyChanged<IChanged> param)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (param != null)
                {
                    IChanged? obj = param.First();
                    if (obj != null)
                    {
                        Report? rep = (Report)obj;
                        Views.FormChangeOrCreate frm = new Views.FormChangeOrCreate(rep.FormNum.Value, rep);
                        await frm.ShowDialog(desktop.MainWindow);
                    }
                }
            }
        }

        private async Task _DeleteForm(ObservableCollectionWithItemPropertyChanged<IChanged> param)
        {
            if (param != null)
            {
                foreach (IChanged? item in param)
                {
                    Local_Reports.Reports_Collection.First().Report_Collection.Remove((Report)item);
                }
            }
        }

        private async Task _Excel_Export()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                SaveFileDialog dial = new SaveFileDialog();
                FileDialogFilter? filter = new FileDialogFilter
                {
                    Name = "Excel"
                };
                filter.Extensions.Add("*.xlsx");
                dial.Filters.Add(filter);
                string? res = await dial.ShowAsync(desktop.MainWindow);
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
