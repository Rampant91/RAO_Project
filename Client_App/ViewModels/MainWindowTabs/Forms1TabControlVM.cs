using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms1TabControlVM : FormsTabControlBaseVM
{
    #region Constructor

    public Forms1TabControlVM() { }

    public Forms1TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override byte DefaultOrgsPerPage => 6;

    private protected override byte DefaultFormsPerPage => 8;

    private protected override char FormNum => '1';

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
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .Where(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[1].ShortJurLico_DB.ToLower().Contains(search))
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
            }
            else
                return new ObservableCollection<Reports>(StaticConfiguration.DBModel.ReportsCollectionDbSet
                    .AsEnumerable()
                    .Where(x => x.DBObservable != null)
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .OrderBy(reps => reps.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(reps => reps.Master_DB.OkpoRep.Value, comparator)
                    .Skip((CurrentPageOrgs - 1) * RowsCountOrgs)
                    .Take(RowsCountOrgs));
        }
    }
    
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
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0")
                    .Count(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows10[1].ShortJurLico_DB.ToLower().Contains(search));
            }
            return TotalRowsOrgs;
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
                // Сортируем по валидным датам, некорректные уходят в начало/конец
                .ThenByDescending(x => x.StartPeriod_DB == null ||
                !DateOnly.TryParse(x.StartPeriod_DB, out _) 
                    ? DateOnly.MaxValue 
                    : DateOnly.Parse(x.StartPeriod_DB))
                .ThenByDescending(x => x.EndPeriod_DB == null ||
                    !DateOnly.TryParse(x.EndPeriod_DB, out _) 
                    ? DateOnly.MaxValue 
                    : DateOnly.Parse(x.EndPeriod_DB))
                .ThenBy(rep => rep.CorrectionNumber_DB)
                .Skip((CurrentPageForms - 1) * RowsCountForms)
                .Take(RowsCountForms);

            return new ObservableCollection<Report>(result);
        }
    }

    #region FormNumWhiteList

    private string _formNumWhiteList = string.Empty;
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
            "1.1" => query.Include(x => x.Rows11)
                .SelectMany(x => x.Rows11)
                .Count(),

            "1.2" => query.Include(x => x.Rows12)
                .SelectMany(x => x.Rows12)
                .Count(),

            "1.3" => query.Include(x => x.Rows13)
                .SelectMany(x => x.Rows13)
                .Count(),

            "1.4" => query.Include(x => x.Rows14)
                .SelectMany(x => x.Rows14)
                .Count(),

            "1.5" => query.Include(x => x.Rows15)
                .SelectMany(x => x.Rows15)
                .Count(),

            "1.6" => query.Include(x => x.Rows16)
                .SelectMany(x => x.Rows16)
                .Count(),

            "1.7" => query.Include(x => x.Rows17)
                .SelectMany(x => x.Rows17)
                .Count(),

            "1.8" => query.Include(x => x.Rows18)
                .SelectMany(x => x.Rows18)
                .Count(),

            "1.9" => query.Include(x => x.Rows19)
                .SelectMany(x => x.Rows19)
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