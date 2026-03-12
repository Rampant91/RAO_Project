using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms2TabControlVM : FormsTabControlBaseVM
{
    #region Constructor

    public Forms2TabControlVM() { }

    public Forms2TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override byte DefaultOrgsPerPage => 6;

    private protected override byte DefaultFormsPerPage => 8;

    private protected override char FormNum => '2';

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
                    .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                    .Count(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search));
            }
            return TotalRowsOrgs;
        }
    }

    #region FormNumWhiteList

    private string _formNumWhiteList = "";
    public string FormNumWhiteList
    {
        get => _formNumWhiteList;
        set
        {
            _formNumWhiteList = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private protected override int InSelectedReportFormsCount => GetReportRowsCount(SelectedReport);

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
            var comparator = new CustomReportsComparer();
            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                    .Where(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search))
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
            else
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
        }
    }

    private protected override ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;


            var result = SelectedReports
                    .Report_Collection
                    .AsEnumerable();

            if (!string.IsNullOrEmpty(FormNumWhiteList))
            {
                result = result.Where(rep => rep.FormNum_DB == FormNumWhiteList);
            }

            result = result.OrderBy(x => 
                    {
                        if (int.TryParse(x.FormNum_DB.Split('.')[1], out var result))
                            return result;
                        return int.MinValue;
                    })
                    .ThenByDescending(x => 
                        x.Year_DB == null ||
                        !int.TryParse(x.Year_DB, out _) ?
                        int.MaxValue :
                        int.Parse(x.Year_DB))
                    .ThenBy(rep => rep.CorrectionNumber_DB)
                    .Skip((CurrentPageForms - 1) * RowsCountForms)
                    .Take(RowsCountForms);

            return new ObservableCollection<Report>(result);
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
            if (SelectedReports is null) return 0;

            if (!string.IsNullOrEmpty(FormNumWhiteList))
                return SelectedReports.Report_Collection.Count(rep => rep.FormNum_DB == FormNumWhiteList);

            return SelectedReports.Report_Collection.Count;
        }
    }

    #endregion

    #region Functions

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
    /// <param name="rep">Отчёт, у которого нужно посчитать количество строчек форм.</param>
    /// <returns>Количество строчек форм.</returns>
    private static int GetReportRowsCount(Report? rep)
    {
        if (rep == null) return 0;
        while (StaticConfiguration.IsFileLocked(null)) Thread.Sleep(50);
        using var db = new DBModel(StaticConfiguration.DBPath);

        var query = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id);

        var result = rep.FormNum_DB switch
        {
            "2.1" => query.Include(x => x.Rows21)
                .SelectMany(x => x.Rows21)
                .Count(),

            "2.2" => query.Include(x => x.Rows22)
                .SelectMany(x => x.Rows22)
                .Count(),

            "2.3" => query.Include(x => x.Rows23)
                .SelectMany(x => x.Rows23)
                .Count(),

            "2.4" => query.Include(x => x.Rows24)
                .SelectMany(x => x.Rows24)
                .Count(),

            "2.5" => query.Include(x => x.Rows25)
                .SelectMany(x => x.Rows25)
                .Count(),

            "2.6" => query.Include(x => x.Rows26)
                .SelectMany(x => x.Rows26)
                .Count(),

            "2.7" => query.Include(x => x.Rows27)
                .SelectMany(x => x.Rows27)
                .Count(),

            "2.8" => query.Include(x => x.Rows28)
                .SelectMany(x => x.Rows28)
                .Count(),

            "2.9" => query.Include(x => x.Rows29)
                .SelectMany(x => x.Rows29)
                .Count(),

            "2.10" => query.Include(x => x.Rows210)
                .SelectMany(x => x.Rows210)
                .Count(),

            "2.11" => query.Include(x => x.Rows211)
                .SelectMany(x => x.Rows211)
                .Count(),

            "2.12" => query.Include(x => x.Rows212)
                .SelectMany(x => x.Rows212)
                .Count(),

            _ => 0
        };
        return result;
    }

    public void GoToFormNum(string formNum)
    {
        FormNumWhiteList = FormNumWhiteList != formNum
            ? formNum
            : string.Empty;

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    public override void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    #endregion
}