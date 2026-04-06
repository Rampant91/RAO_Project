using Client_App.Commands.AsyncCommands;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.ViewModels.MainWindowTabs;

public abstract class FormsTabControlBaseVM : INotifyPropertyChanged
{
    #region Constructor

    protected FormsTabControlBaseVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }

    protected FormsTabControlBaseVM(MainWindowVM mainWindowVM)
    {
        MainWindowVM = mainWindowVM;
    }

    #endregion

    #region Properties

    private CancellationTokenSource? _debounceCts;

    public MainWindowVM MainWindowVM { get; }

    private protected abstract byte DefaultOrgsPerPage { get; }

    private protected abstract byte DefaultFormsPerPage { get; }

    private protected abstract char FormNum { get; }

    private protected abstract ObservableCollection<Report>? ReportCollection { get; }

    private protected abstract ObservableCollection<Reports>? ReportsCollection { get; }

    private protected abstract Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors { get; }

    private protected abstract int TotalPagesForms { get; }

    private protected abstract int TotalPagesOrgs { get; }

    private protected abstract int TotalRowsForms { get; }

    #region CurrentPageForms

    private int _currentPageForms = 1;
    private protected int CurrentPageForms
    {
        get
        {
            if (_currentPageForms > TotalPagesForms && TotalPagesForms > 0)
                _currentPageForms = TotalPagesForms;

            return _currentPageForms;
        }
        set
        {
            _currentPageForms = value;
            OnPropertyChanged();
            UpdateReportCollection();
            OnPropertyChanged(nameof(TotalReportCount));
        }
    }

    #endregion

    #region CurrentPageOrgs

    private int _currentPageOrgs = 1;
    private protected int CurrentPageOrgs
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
            OnPropertyChanged(nameof(TotalReportCount));
        }
    }

    #endregion

    #region InSelectedReportFormsCount
    
    private int _inSelectedReportFormsCount;
    public int InSelectedReportFormsCount
    {
        get => _inSelectedReportFormsCount;
        private set
        {
            if (_inSelectedReportFormsCount != value)
            {
                _inSelectedReportFormsCount = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region RowsCountForms

    private int _rowsCountForms;
    private protected int RowsCountForms
    {
        get
        {
            if (_rowsCountForms == 0) // If not loaded yet
            {
                var (_, forms) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form" + FormNum, DefaultOrgsPerPage, DefaultFormsPerPage);
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

    #region RowsCountOrgs

    private int _rowsCountOrgs;
    private protected int RowsCountOrgs
    {
        get
        {
            if (_rowsCountOrgs == 0) // If not loaded yet
            {
                var (orgs, _) = Properties.RowCountSettings.RowCountSettingsManager.LoadSettings(
                    "form" + FormNum, DefaultOrgsPerPage, DefaultFormsPerPage);
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

    #region SearchText

    private string _searchText;
    private protected string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;

            _searchText = value;
            OnPropertyChanged();

            // Отменяем предыдущий таймер
            _debounceCts?.Cancel();

            // Создаем новый таймер
            _debounceCts = new CancellationTokenSource();

            // Задержка 300мс перед фильтрацией
            Task.Delay(300, _debounceCts.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (CurrentPageOrgs != 1)
                                CurrentPageOrgs = 1;

                            NotifySearchTextChanged();
                        });
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
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
            if (_selectedReport == value) return;

            _selectedReport = value;
            OnPropertyChanged();

            _ = UpdateInSelectedReportFormsCountAsync();
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

            _ = UpdateInSelectedReportFormsCountAsync();

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

    public int TotalReportCount
    {
        get
        {
            var allOrgs = StaticConfiguration.DBModel.ReportsCollectionDbSet
                .AsEnumerable()
                .Where(x => x.DBObservable != null)
                .Where(reps => reps.Master_DB.FormNum_DB == FormNum + ".0");

            if (!string.IsNullOrEmpty(SearchText))
            {
                var search = SearchText.ToLower().Trim();
                allOrgs = allOrgs.Where(reps => 
                    reps.Master_DB.RegNoRep.Value.ToLower().Contains(search)
                    || reps.Master_DB.OkpoRep.Value.ToLower().Contains(search)
                    || GetAdditionalSearchConditions(reps, search));
            }

            return allOrgs
                .Sum(org => org.Report_Collection
                    .Count(rep => rep.FormNum_DB.StartsWith($"{MainWindowVM.SelectedReportType}")
                                  && !rep.FormNum_DB.EndsWith(".0")));
        }
    }

    /// <summary>
    /// Всего организаций с учётом фильтра.
    /// </summary>
    private protected abstract int FilteredRowsOrgs { get; }

    /// <summary>
    /// Дополнительные условия поиска для переопределения в дочерних классах.
    /// </summary>
    protected virtual bool GetAdditionalSearchConditions(Reports reps, string search) => false;

    /// <summary>
    /// Всего организаций.
    /// </summary>
    public int TotalRowsOrgs => StaticConfiguration.DBModel.ReportsCollectionDbSet
        .Where(x => x.DBObservable != null)
        .Count(reps => reps.Master_DB.FormNum_DB == FormNum + ".0");

    #endregion

    #region Methods

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
    /// <param name="rep">Отчёт, у которого нужно посчитать количество строчек форм.</param>
    /// <returns>Количество строчек форм.</returns>
    private async Task<int> GetReportRowsCount(Report? rep)
    {
        if (rep == null || rep.FormNum == null) return 0;

        while (StaticConfiguration.IsFileLocked(null))
            await Task.Delay(50);

        await using var db = new DBModel(StaticConfiguration.DBPath);

        var baseQuery = db.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
            .Where(report => report.Reports != null && report.Reports.DBObservable != null && report.Id == rep.Id);

        if (RowSelectors.TryGetValue(rep.FormNum_DB, out var selector))
        {
            return await selector(baseQuery).CountAsync();
        }

        return 0;
    }

    private protected abstract void NotifySearchTextChanged();

    private void NotifyRowsChanged()
    {
        OnPropertyChanged(nameof(RowsCountOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    private void SaveRowCountSettings()
    {
        Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form" + FormNum,
            _rowsCountOrgs,
            _rowsCountForms);
    }

    public void UpdateFormsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));
    }

    private async Task UpdateInSelectedReportFormsCountAsync()
    {
        if (SelectedReport == null)
        {
            InSelectedReportFormsCount = 0;
            return;
        }

        // Асинхронно получаем данные
        var count = await GetReportRowsCount(SelectedReport);

        // Записываем в свойство - UI автоматически обновится через OnPropertyChanged
        InSelectedReportFormsCount = count;
    }

    public void UpdateReportCollection()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }

    public void UpdateReportsCollection()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }

    public void UpdateTotalReportCount()
    {
        OnPropertyChanged(nameof(TotalReportCount));
    }

    public void UpdateTotalReportsCount()
    {
        OnPropertyChanged(nameof(FilteredRowsOrgs));
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}