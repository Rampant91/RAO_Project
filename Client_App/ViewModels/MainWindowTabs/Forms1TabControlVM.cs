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

    public int InSelectedReportFormsCount => GetReportRowsCount(SelectedReport);

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
        .Count(reps => reps.Master_DB.FormNum_DB == "1.0");
    
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

    public override int RowsCountOrgs
    {
        get 
        {
            if (_rowsCountOrgs == 0) // If not loaded yet
            {
                var (orgs, _) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form1", 6, 8);
                _rowsCountOrgs = orgs;
            }
            return _rowsCountOrgs;
        }
        set
        {
            if (_rowsCountOrgs != value)
            {
                _rowsCountOrgs = value;
                NotifyRowsChanged();
                SaveRowCountSettings();
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
                !DateOnly.TryParse(x.StartPeriod_DB, out _) ?
                    DateOnly.MaxValue :
                    DateOnly.Parse(x.StartPeriod_DB))
                .ThenByDescending(x => x.EndPeriod_DB == null ||
                    !DateOnly.TryParse(x.EndPeriod_DB, out _) ?
                    DateOnly.MaxValue :
                    DateOnly.Parse(x.EndPeriod_DB))
                .ThenBy(rep => rep.CorrectionNumber_DB)
                .Skip((CurrentPageForms - 1) * RowsCountForms)
                .Take(RowsCountForms);

            return new ObservableCollection<Report>(result);
        }
    }

    #endregion

    #region FormNumWhiteList
    private string _formNumWhiteList = "";
    public string FormNumWhiteList
    {
        get
        {
            return _formNumWhiteList;
        }
        set
        {
            _formNumWhiteList = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region SelectedReport

    private Report? _selectedReport;
    public Report? SelectedReport
    {
        get => _selectedReport;
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
            if (SelectedReports is null) return 0;

            if (!string.IsNullOrEmpty(FormNumWhiteList))
                return SelectedReports.Report_Collection.Count(rep => rep.FormNum_DB == FormNumWhiteList);

            return SelectedReports.Report_Collection.Count;
        }
    }

    private int _rowsCountForms;
    public int RowsCountForms
    {
        get 
        {
            if (_rowsCountForms == 0) // If not loaded yet
            {
                var (_, forms) = Client_App.Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form1", 6, 8);
                _rowsCountForms = forms;
            }
            return _rowsCountForms;
        }
        set
        {
            if (_rowsCountForms != value)
            {
                _rowsCountForms = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPagesForms));
                UpdateReportCollection();
                SaveRowCountSettings();
            }
        }
    }

    #endregion

    #endregion

    #region Functions

    public void GoToFormNum(string formNum)
    {
        FormNumWhiteList = FormNumWhiteList != formNum 
            ? formNum 
            : string.Empty;

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

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

    private void SaveRowCountSettings()
    {
        Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form1",
            _rowsCountOrgs,
            _rowsCountForms);
    }

    public override void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    #endregion
}