using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms1TabControlVM : INotifyPropertyChanged
{
    #region Constructor
    public Forms1TabControlVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }

    public Forms1TabControlVM(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }
    
    private void SaveRowCountSettings()
    {
        Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form1", 
            _rowsCountOrgs, 
            _rowsCountForms);
    }
    #endregion

    #region Properties

    #region MainWindowVM
    private MainWindowVM _mainWindowVM;
    public MainWindowVM MainWindowVM => _mainWindowVM;

    #endregion

    #region SearchText
    private string _searchText = "";

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (CurrentPageOrgs != 1)
                CurrentPageOrgs = 1;
            _searchText = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ReportsCollection));
            OnPropertyChanged(nameof(FilteredRowsOrgs));
            OnPropertyChanged(nameof(TotalPagesOrgs));
        }
    }
    #endregion

    #region ReportsCollection
    public ObservableCollection<Reports> ReportsCollection
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
    #endregion

    #region SelectedReports

    private Reports? _selectedReports;
    public Reports? SelectedReports
    {
        get => _selectedReports;
        set
        {
            _selectedReports = value;
            OnPropertyChanged();

            // UpdateReportCollection выполняется в CurrentPageForms
            // Чтобы не вызывать метод дважды используется if else
            if (CurrentPageForms != 1)
                CurrentPageForms = 1;
            else
                UpdateReportCollection();

            UpdateFormsPageInfo();
        }
    }
    #endregion

    #region PaginationOrgs

    public int TotalPagesOrgs
    {
        get
        {
            var result = FilteredRowsOrgs / RowsCountOrgs;
            if (FilteredRowsOrgs % RowsCountOrgs > 0)
                result++;
            return result;
        }
    }
    public static int TotalRowsOrgs => StaticConfiguration.DBModel.ReportsCollectionDbSet
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

    private int _rowsCountOrgs;
    public int RowsCountOrgs
    {
        get 
        {
            if (_rowsCountOrgs == 0) // If not loaded yet
            {
                var (orgs, _) = Client_App.Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(ReportsCollection));
                OnPropertyChanged(nameof(TotalPagesOrgs));
                SaveRowCountSettings();
            }
        }
    }


    private int _currentPageOrgs = 1;
    public int CurrentPageOrgs
    {
        get
        {
            if (_currentPageOrgs > TotalPagesOrgs)
                _currentPageOrgs = TotalPagesOrgs;
            return _currentPageOrgs;
        }
        set
        {
            _currentPageOrgs = value;
            OnPropertyChanged(nameof(ReportsCollection));
            OnPropertyChanged();
        }
    }
    #endregion

    #region ReportCollection

    public ObservableCollection<Report> ReportCollection
    {
        get
        {
            if (SelectedReports is null) return null;


            return new ObservableCollection<Report>(
                SelectedReports
                    .Report_Collection
                    .AsEnumerable()
                    .OrderBy(x => x.Order)
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
                    .Take(RowsCountForms));
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
    public int TotalPagesForms
    {
        get
        {
            var result = TotalRowsForms / RowsCountForms;
            if (TotalRowsForms % RowsCountForms > 0)
                result++;
            return result;
        }
    }
    public int TotalRowsForms
    {
        get
        {
            if (SelectedReports != null)
                return SelectedReports.Report_Collection.Count;
            return 0;
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


    private int _currentPageForms = 1;
    public int CurrentPageForms
    {
        get
        {
            if ((_currentPageForms > TotalPagesForms) && (TotalPagesForms > 0))
                _currentPageForms = TotalPagesForms;

            return _currentPageForms;
        }
        set
        {
            _currentPageForms = value;
            OnPropertyChanged();
            UpdateReportCollection();
        }
    }
    #endregion

    #region TotalReportCount
    public int TotalReportCount
    {
        get
        {
            return StaticConfiguration.DBModel.ReportCollectionDbSet
                .Where(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                              && !rep.FormNum_DB.EndsWith(".0"))
                .CountAsync().Result;
        }
    }
    #endregion

    #region InSelectedReportFormsCount

    public int InSelectedReportFormsCount => GetReportRowsCount(SelectedReport);

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
    /// <param name="rep">Отчёт, у которого нужно посчитать количество строчек форм.</param>
    /// <returns>Количество строчек форм.</returns>
    public static int GetReportRowsCount(Report? rep)
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

    #endregion

    #endregion

    #region Functions

    #region UpdateOrgsPageInfo
    public void UpdateOrgsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }
    #endregion

    #region UpdateFormsPageInfo
    public void UpdateFormsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));
    }
    #endregion

    #region TotalReportCount
    public void UpdateTotalReportCount()
    {
        OnPropertyChanged(nameof(TotalReportCount));
    }
    #endregion

    #region UpdateReportCollection
    public void UpdateReportCollection()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }
    #endregion

    #region UpdateReportsCollection
    public void UpdateReportsCollection()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }
    #endregion

    #region GoToFormNum
    public void GoToFormNum (string formNum)
    {
        var report = SelectedReports?.Report_Collection.FirstOrDefault(rep => rep.FormNum_DB == formNum);
        if (report == null) return;

        var index = SelectedReports.Report_Collection.IndexOf(report);
        CurrentPageForms = (index / RowsCountForms) + 1;
    }
    #endregion

    #endregion

    #region INotifyPropertyChanged

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}