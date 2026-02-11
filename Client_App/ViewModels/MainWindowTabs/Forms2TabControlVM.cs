using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms2TabControlVM : INotifyPropertyChanged
{
    #region Constructor
    public Forms2TabControlVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }
    public Forms2TabControlVM (MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }
    
    private void SaveRowCountSettings()
    {
        Client_App.Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form2", 
            _rowsCountOrgs, 
            _rowsCountForms);
    }
    #endregion

    #region Properties

    #region MainWindowVM
    private MainWindowVM _mainWindowVM;
    public MainWindowVM MainWindowVM
    {
        get
        {
            return _mainWindowVM;
        }
    }
    #endregion

    #region SearchText
    private string _searchText = "";

    public string SearchText
    {
        get
        {
            return _searchText;
        }
        set
        {
            if(CurrentPageOrgs != 1)
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
    #endregion

    #region SelectedReports

    private Reports? _selectedReports;
    public Reports? SelectedReports
    {
        get
        {
            return _selectedReports;
        }
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
    public int TotalRowsOrgs
    {
        get
        {
            return StaticConfiguration.DBModel.ReportsCollectionDbSet
                .CountAsync(reps => reps.Master_DB.FormNum_DB == "2.0")
                .Result;
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
                    .Where(reps => reps.Master_DB.FormNum_DB == "2.0")
                    .Count(reps => reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[0].ShortJurLico_DB.ToLower().Contains(search)
                                   || reps.Master_DB.Rows20[1].ShortJurLico_DB.ToLower().Contains(search));
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
                var (orgs, _) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form2", 6, 8);
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


            var result = SelectedReports
                    .Report_Collection
                    .AsEnumerable();

            if (!string.IsNullOrEmpty(FormNumWhiteList))
            {
                result = result.Where(rep => rep.FormNum_DB == FormNumWhiteList);
            }


            result.OrderBy(x => 
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
            if (SelectedReports is null) return 0;

            if (!string.IsNullOrEmpty(FormNumWhiteList))
                return SelectedReports.Report_Collection.Where(rep => rep.FormNum_DB == FormNumWhiteList).Count();

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
                    "form2", 6, 8);
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
                .CountAsync(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                                   && !rep.FormNum_DB.EndsWith(".0")).Result;
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

    #endregion

    #endregion

    #region Functions

    #region GoToFormNum
    public void GoToFormNum(string formNum)
    {
        if (FormNumWhiteList != formNum)
            FormNumWhiteList = formNum;
        else
            FormNumWhiteList = "";

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }
    #endregion

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

    #endregion

    #region INotifyPropertyChanged

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}