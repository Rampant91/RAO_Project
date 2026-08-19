using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Add;
using Client_App.Commands.AsyncCommands.Delete;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.RaodbExport;
using Client_App.Services.DataAccess;
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
using System.Windows.Input;
using Avalonia.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

/// <summary>
/// Базовая VM вкладок форм главного окна (1/2/4/5).
/// Org-грид: cache-first через <see cref="Forms1WarmCache"/>; счётчики — async в фоне.
/// </summary>
public abstract class FormsTabControlBaseVM : INotifyPropertyChanged
{
    #region Commands

    /// <summary>
    /// Создать и открыть новое окно с отчётом по форме 1.x, 4.1 и 5.x для выбранной организации.
    /// Для 2.x используется старая команда, пока не обновим там интерфейс.
    /// </summary>
    public ICommand AddReport { get; private set; }

    /// <summary>
    /// Редактировать выбранный отчёт
    /// </summary>
    public ICommand NewChangeReport => new NewChangeReportAsyncCommand(this);

    /// <summary>
    /// Редактировать выбранный отчёт
    /// </summary>
    public ICommand NewChangeReports => new NewChangeReportsAsyncCommand(this);

    /// <summary>
    /// Удалить выбранный отчёт
    /// </summary>
    public ICommand DeleteReport => new NewDeleteFormAsyncCommand(this);

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для печати
    /// </summary>
    public ICommand ExcelExportFormPrint => new ExcelExportFormPrintAsyncCommand(this);

    /// <summary>
    /// Экспорт отчёта в файл .RAODB
    /// </summary>
    public ICommand ExportReport => new ExportReportAsyncCommand(this); 
    
    #endregion

    #region Constructor

    protected FormsTabControlBaseVM()
    {
        // Конструктор пуст - настройки загружаются лениво при первом доступе
    }

    protected FormsTabControlBaseVM(MainWindowVM mainWindowVM)
    {
        AddReport = new AddReportAsyncCommand();

        MainWindowVM = mainWindowVM;
    }

    #endregion

    #region Fields

    private CancellationTokenSource? _debounceCts;

    protected readonly ObservableCollection<Reports> _orgsCollection = new();
    protected readonly Forms1WarmCache _cache = Forms1WarmCache.Instance;
    private int _orgLoadGeneration;
    private int _countsLoadGeneration;

    private int _filteredRowsOrgs;
    private int _totalReportCount;
    private int _totalRowsOrgs;

    #endregion

    #region Properties

    public MainWindowVM MainWindowVM { get; }

    private protected abstract byte DefaultOrgsPerPage { get; }

    private protected abstract byte DefaultFormsPerPage { get; }

    private protected abstract char FormNum { get; }

    private protected abstract ObservableCollection<Report>? ReportCollection { get; }

    private protected virtual ObservableCollection<Reports> ReportsCollection => _orgsCollection;

    private protected abstract Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors { get; }

    private protected abstract int TotalRowsForms { get; }

    protected virtual string OrgPageMasterFormNum => $"{FormNum}.0";

    /// <summary>Фильтр формы для report-грида (1/2/5 — whitelist; 4 — null).</summary>
    protected virtual string? GetReportFormFilter() => null;

    /// <summary>Обновлять SelectedReports после загрузки report-страницы (кнопки фильтра Forms 1/2).</summary>
    protected virtual bool NotifySelectedReportsOnReportPageUpdate => false;

    /// <summary>Организаций с учётом фильтра (кэш, обновляется в фоне).</summary>
    public int FilteredRowsOrgs => _filteredRowsOrgs;

    /// <summary>Страниц организаций; зависит только от <see cref="FilteredRowsOrgs"/> и <see cref="RowsCountOrgs"/>.</summary>
    public int TotalPagesOrgs => ComputeTotalPages(FilteredRowsOrgs, RowsCountOrgs);

    /// <summary>Страниц отчётов; зависит только от <see cref="TotalRowsForms"/> и <see cref="RowsCountForms"/>.</summary>
    public int TotalPagesForms => ComputeTotalPages(TotalRowsForms, RowsCountForms);

    /// <summary>Текущая страница org без clamp и без побочных эффектов (для sync/загрузки).</summary>
    private protected int CurrentPageOrgsValue => _currentPageOrgs;

    /// <summary>Текущая страница отчётов без clamp и без побочных эффектов.</summary>
    private protected int CurrentPageFormsValue => _currentPageForms;

    private static int ComputeTotalPages(int totalRows, int pageSize)
    {
        if (totalRows <= 0 || pageSize <= 0)
            return 0;
        return (totalRows + pageSize - 1) / pageSize;
    }

    private static int NormalizePage(int page, int totalPages)
    {
        if (page <= 0)
            return 1;
        if (totalPages > 0 && page > totalPages)
            return totalPages;
        return page;
    }

    private void ClampCurrentPageOrgs()
    {
        var normalized = NormalizePage(_currentPageOrgs, TotalPagesOrgs);
        if (_currentPageOrgs == normalized)
            return;
        _currentPageOrgs = normalized;
        OnPropertyChanged(nameof(CurrentPageOrgs));
    }

    private void ClampCurrentPageForms()
    {
        var normalized = NormalizePage(_currentPageForms, TotalPagesForms);
        if (_currentPageForms == normalized)
            return;
        _currentPageForms = normalized;
        OnPropertyChanged(nameof(CurrentPageForms));
    }

    #region CurrentPageForms

    private int _currentPageForms = 1;
    private protected int CurrentPageForms
    {
        get => _currentPageForms;
        set
        {
            var normalized = NormalizePage(value, TotalPagesForms);
            if (_currentPageForms == normalized)
                return;

            _currentPageForms = normalized;
            OnPropertyChanged();
            UpdateReportCollection();
        }
    }

    #endregion

    #region CurrentPageOrgs

    private int _currentPageOrgs = 1;
    public int CurrentPageOrgs
    {
        get => _currentPageOrgs;
        set
        {
            var normalized = NormalizePage(value, TotalPagesOrgs);
            if (_currentPageOrgs == normalized)
                return;

            _currentPageOrgs = normalized;
            OnPropertyChanged();
            SyncOrgsCollection();
            OnPropertyChanged(nameof(ReportsCollection));
            // TotalReportCount не зависит от страницы org — не пересчитываем (дорого на Firebird).
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
                ClampCurrentPageForms();
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

            // Проверяем и сбрасываем фильтр если нет отчётов для текущего фильтра
            CheckAndResetFilterIfNeeded();

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

    public int TotalReportCount => _totalReportCount;

    /// <summary>
    /// Дополнительные условия поиска для переопределения в дочерних классах.
    /// </summary>
    protected virtual bool GetAdditionalSearchConditions(Reports reps, string search) => false;

    /// <summary>
    /// Проверяет и сбрасывает фильтр если при переключении организации нет отчётов для текущего фильтра.
    /// Переопределяется в дочерних классах.
    /// </summary>
    protected virtual void CheckAndResetFilterIfNeeded() { }

    /// <summary>
    /// Всего организаций (без фильтра поиска).
    /// </summary>
    public int TotalRowsOrgs => _totalRowsOrgs;

    #endregion

    #region Org paging

    /// <summary>
    /// Смена вкладки: показать org из warm-cache; miss и счётчики — в фоне.
    /// </summary>
    public virtual void ActivateTab() => ReloadOrgAndReportGrids();

    /// <summary>
    /// После мутации данных (import/delete/add): перезагрузить org/report без блокировки UI.
    /// </summary>
    public virtual void UpdateOrgsPageInfo() => ReloadOrgAndReportGrids();

    private void ReloadOrgAndReportGrids()
    {
        SyncOrgsCollection();
        RefreshCountsAsync();
        OnPropertyChanged(nameof(ReportsCollection));
        if (SelectedReports != null)
            UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    protected void SyncOrgsCollection()
    {
        var search = SearchText;
        var pageNum = CurrentPageOrgsValue;
        var pageSize = RowsCountOrgs;
        var master = OrgPageMasterFormNum;

        if (_cache.TryGetOrgPage(search, pageNum, pageSize, out var cached, master))
        {
            if (ReplaceCollection(_orgsCollection, cached.Items, r => r.Id))
                OnPropertyChanged(nameof(ReportsCollection));
            SchedulePrefetchAdjacentOrgPages(search, pageNum, pageSize);
            return;
        }

        var generation = Interlocked.Increment(ref _orgLoadGeneration);
        var dbPath = StaticConfiguration.DBPath;

        _ = Task.Run(() =>
        {
            try
            {
                using var db = new DBModel(dbPath);
                var page = _cache.GetOrgPage(db, search, pageNum, pageSize, master);
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _orgLoadGeneration)
                        return;
                    if (ReplaceCollection(_orgsCollection, page.Items, r => r.Id))
                        OnPropertyChanged(nameof(ReportsCollection));
                    SchedulePrefetchAdjacentOrgPages(search, pageNum, pageSize);
                });
            }
            catch
            {
                // keep previous page visible
            }
        });
    }

    /// <summary>
    /// После правки титула 1.0/2.0: пересобрать org-страницу без полного сброса OrgKeys.
    /// </summary>
    public void RefreshOrgListAfterTitleChange()
    {
        var keep = SelectedReports;
        _cache.InvalidateOrgPages();
        SyncOrgsCollection();

        if (keep != null)
        {
            for (var i = 0; i < _orgsCollection.Count; i++)
            {
                if (_orgsCollection[i].Id == keep.Id)
                {
                    _orgsCollection[i] = keep;
                    break;
                }
            }

            SetSelectedReportsWithoutReload(keep);
        }

        NotifyOrgFilterChanged();
    }

    private void SchedulePrefetchAdjacentOrgPages(string? search, int pageNum, int pageSize)
    {
        var totalPages = ComputeTotalPages(_filteredRowsOrgs, pageSize);
        _cache.PrefetchAdjacentOrgPages(
            StaticConfiguration.DBPath, search, pageNum, pageSize, totalPages, OrgPageMasterFormNum);
    }

    protected void WarmSelectedOrgAndPrefetchReports()
    {
        if (SelectedReports is null) return;

        var filter = GetReportFormFilter();
        _cache.OnOrgSelected(
            StaticConfiguration.DBPath,
            SelectedReports.Id,
            filter,
            CurrentPageFormsValue,
            RowsCountForms);

        _cache.PrefetchAdjacentReportPages(
            StaticConfiguration.DBPath,
            SelectedReports.Id,
            filter,
            CurrentPageFormsValue,
            RowsCountForms,
            TotalPagesForms);
    }

    #endregion

    #region Async counts

    protected void RefreshCountsAsync()
    {
        var generation = Interlocked.Increment(ref _countsLoadGeneration);
        var search = SearchText;
        var dbPath = StaticConfiguration.DBPath;
        var formNum = FormNum;

        _ = Task.Run(() =>
        {
            try
            {
                using var db = new DBModel(dbPath);
                var filtered = QueryFilteredRowsOrgs(db, formNum, search);
                var totalReports = QueryTotalReportCount(db, formNum, search);
                var totalOrgs = QueryTotalRowsOrgs(db, formNum);

                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _countsLoadGeneration)
                        return;

                    _filteredRowsOrgs = filtered;
                    _totalReportCount = totalReports;
                    _totalRowsOrgs = totalOrgs;
                    OnPropertyChanged(nameof(FilteredRowsOrgs));
                    OnPropertyChanged(nameof(TotalPagesOrgs));
                    OnPropertyChanged(nameof(TotalReportCount));
                    OnPropertyChanged(nameof(TotalRowsOrgs));
                });
            }
            catch
            {
                // keep previous counts
            }
        });
    }

    private static int QueryFilteredRowsOrgs(DBModel db, char formNum, string? search) =>
        formNum switch
        {
            '1' => MainWindowListQuery.CountOrgsForm12(db, "1.0", search),
            '2' => MainWindowListQuery.CountOrgsForm12(db, "2.0", search),
            '4' => MainWindowListQuery.CountOrgsForm40(db, search),
            '5' => MainWindowListQuery.CountOrgsForm50(db, search),
            _ => 0
        };

    private static int QueryTotalReportCount(DBModel db, char formNum, string? search) =>
        formNum switch
        {
            '1' => MainWindowListQuery.CountAllReportsForFormType(db, '1', search, "1.0"),
            '2' => MainWindowListQuery.CountAllReportsForFormType(db, '2', search, "2.0"),
            '4' => MainWindowListQuery.CountAllReportsForForm40(db, search),
            '5' => MainWindowListQuery.CountAllReportsForForm50(db, search),
            _ => 0
        };

    private static int QueryTotalRowsOrgs(DBModel db, char formNum) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Count(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == $"{formNum}.0");

    #endregion

    #region Methods

    private protected virtual void NotifySearchTextChanged()
    {
        SyncOrgsCollection();
        RefreshCountsAsync();
        NotifyOrgFilterChanged();
    }

    private void NotifyRowsChanged()
    {
        OnPropertyChanged(nameof(RowsCountOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        ClampCurrentPageOrgs();
        SyncOrgsCollection();
    }

    private void SaveRowCountSettings()
    {
        Properties.RowCountSettings.RowCountSettingsManager.SaveSettings(
            "form" + FormNum,
            _rowsCountOrgs,
            _rowsCountForms);
    }

    public virtual void UpdateFormsPageInfo()
    {
        OnPropertyChanged(nameof(TotalRowsForms));
        OnPropertyChanged(nameof(TotalPagesForms));
        ClampCurrentPageForms();
    }

    protected static bool ReplaceCollection<T>(
        ObservableCollection<T> target, IReadOnlyList<T> source, Func<T, int> idSelector) =>
        CollectionSync.ReplaceById(target, source, idSelector);

    /// <summary>
    /// Возвращает количество строчек форм у отчёта.
    /// </summary>
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
            return await selector(baseQuery).CountAsync();

        return 0;
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

    public virtual void UpdateReportCollection()
    {
        OnPropertyChanged(nameof(ReportCollection));
    }

    public void UpdateReportsCollection()
    {
        OnPropertyChanged(nameof(ReportsCollection));
    }

    /// <summary>
    /// Обновляет содержимое коллекции организаций без пересоздания объекта.
    /// Сохраняет выбранную организацию.
    /// </summary>
    public virtual void UpdateReportsCollectionWithoutReCreation()
    {
        var selectedId = SelectedReports?.Master_DB?.Id;
        _cache.InvalidateOrgPages();
        SyncOrgsCollection();
        RefreshCountsAsync();

        if (selectedId.HasValue)
        {
            var restored = _orgsCollection.FirstOrDefault(r => r.Master_DB?.Id == selectedId.Value);
            if (restored != null)
                SetSelectedReportsWithoutReload(restored);
        }

        OnPropertyChanged(nameof(ReportsCollection));
    }

    /// <summary>
    /// Меняет SelectedReports без перезагрузки списка отчётов (восстановление после refresh org page).
    /// </summary>
    protected void SetSelectedReportsWithoutReload(Reports? reports)
    {
        _selectedReports = reports;
        OnPropertyChanged(nameof(SelectedReports));
    }

    public void UpdateTotalReportCount() => RefreshCountsAsync();

    protected void NotifyOrgFilterChanged()
    {
        ClampCurrentPageOrgs();
        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    public void UpdateTotalReportsCount()
    {
        RefreshCountsAsync();
        NotifyOrgFilterChanged();
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