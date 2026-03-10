using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms4TabControlVM : FormsTabControlBaseVM
{
    #region Constructor

    public Forms4TabControlVM() { }

    public Forms4TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;

            _searchText = value;
            OnPropertyChanged();

            // Отменяем предыдущий таймер
            DebounceCts?.Cancel();

            // Создаем новый таймер
            DebounceCts = new CancellationTokenSource();

            // Задержка 300мс перед фильтрацией
            Task.Delay(300, DebounceCts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (CurrentPageOrgs != 1)
                                CurrentPageOrgs = 1;

                            OnPropertyChanged(nameof(ReportsCollection));
                            OnPropertyChanged(nameof(FilteredRowsOrgs));
                            OnPropertyChanged(nameof(TotalPagesOrgs));
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    private protected override ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(reps => reps.Master_DB.FormNum_DB == "4.0")
                    .Where(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB.ToString().Contains(search)
                                   || reps.Master_DB.Rows40[0].SubjectRF_DB.ToLower().Contains(search)
                                   || (!string.IsNullOrEmpty(reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB)
                                       && reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB.ToLower().Contains(search)))
                    .OrderBy(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
            else
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(reps => reps.Master_DB.FormNum_DB == "4.0")
                    .OrderBy(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
        }
    }

    #region PaginationOrgs

    private protected override int TotalPagesOrgs
    {
        get
        {
            var result = FilteredRowsOrgs / RowsCountOrgs;
            if (FilteredRowsOrgs % RowsCountOrgs > 0)
                result++;
            return result;
        }
    }

    private protected override int TotalRowsOrgs => StaticConfiguration.DBModel.ReportsCollectionDbSet
        .Where(x => x.DBObservable != null)
        .Count(reps => reps.Master_DB.FormNum_DB == "4.0");

    public int FilteredRowsOrgs
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(reps => reps.Master_DB.FormNum_DB == "4.0")
                    .Where(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB.ToString().Contains(search)
                                   || reps.Master_DB.Rows40[0].SubjectRF_DB.ToLower().Contains(search)
                                   || (!string.IsNullOrEmpty(reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB)
                                       && reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB.ToLower().Contains(search)))
                    .Count();
            }
            return TotalRowsOrgs;
        }
    }

    private new int _rowsCountOrgs = 10;
    public override int RowsCountOrgs
    {
        get => _rowsCountOrgs;
        set
        {
            if (_rowsCountOrgs != value)
            {
                _rowsCountOrgs = value;
                NotifyRowsChanged();
            }
        }
    }

    #endregion

    #region ReportCollection

    private protected override ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;

            return new ObservableCollection<Report>(
                SelectedReports
                    .Report_Collection
                    .AsEnumerable()
                    .OrderBy(x => x.FormNum_DB)
                    .ThenByDescending(x => x.Year_DB == null ||
                                           !int.TryParse(x.Year_DB, out _) ?
                        int.MaxValue :
                        int.Parse(x.Year_DB))
                    .ThenBy(rep => rep.CorrectionNumber_DB)
                    .Skip((CurrentPageForms - 1) * RowsCountForms)
                    .Take(RowsCountForms));
        }
    }

    #endregion

    #region SelectedReport

    private Report? _selectedReport;
    public Report? SelectedReport
    {
        get
        {
            return _selectedReport;
        }
        set
        {
            _selectedReport = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(InSelectedReportFormsCount));
        }
    }

    #endregion

    #region PaginationForms
    private protected override int TotalPagesForms
    {
        get
        {
            var result = TotalRowsForms / RowsCountForms;
            if (TotalRowsForms % RowsCountForms > 0)
                result++;
            return result;
        }
    }
    private protected override int TotalRowsForms
    {
        get
        {
            if (SelectedReports != null)
                return SelectedReports.Report_Collection.Count;
            return 0;
        }
    }


    private int _rowsCountForms = 10;
    public int RowsCountForms
    {
        get
        {
            return _rowsCountForms;
        }
        set
        {
            _rowsCountForms = value;
            OnPropertyChanged();
            UpdateReportCollection();
            OnPropertyChanged(nameof(TotalPagesForms));
        }
    }

    #endregion

    #region InSelectedReportFormsCount
    public int InSelectedReportFormsCount
    {
        get
        {
            if (SelectedReport is null) return 0;
            return StaticConfiguration.DBModel.ReportCollectionDbSet
                .Include(rep => rep.Rows41)
                .FirstOrDefault(rep => rep.Id == SelectedReport.Id)
                .Rows.Count;
        }
    }
    #endregion

    #endregion

    #region Functions

    public override void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

    }

    #endregion
}