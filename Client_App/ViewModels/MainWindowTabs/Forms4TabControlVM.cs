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

    private protected override byte DefaultOrgsPerPage => 7;

    private protected override byte DefaultFormsPerPage => 8;

    private protected override char FormNum => '4';

    public int FilteredRowsOrgs
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "4.0")
                    .Count(reps => reps.Master_DB.Rows40[0].CodeSubjectRF_DB.ToString().Contains(search)
                                   || reps.Master_DB.Rows40[0].SubjectRF_DB.ToLower().Contains(search)
                                   || (!string.IsNullOrEmpty(reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB)
                                       && reps.Master_DB.Rows40[0].ShortNameOrganUprav_DB.ToLower().Contains(search)));
            }
            return TotalRowsOrgs;
        }
    }

    private protected override int InSelectedReportFormsCount
    {
        get
        {
            if (SelectedReport is null) return 0;
            return StaticConfiguration.DBModel.ReportCollectionDbSet
                .Include(rep => rep.Rows41)
                .FirstOrDefault(rep => rep.Id == SelectedReport.Id)
                ?.Rows.Count ?? 0;
        }
    }

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

    private protected override int TotalRowsForms
    {
        get
        {
            if (SelectedReports != null)
                return SelectedReports.Report_Collection.Count;
            return 0;
        }
    }

    #endregion

    #region Functions

    public override void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));

    }

    #endregion
}