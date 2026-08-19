using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Import;
using Client_App.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms1TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '1';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private readonly ObservableCollection<Reports> _orgsCollection = new();
    private readonly Forms1WarmCache _cache = Forms1WarmCache.Instance;
    private int _reportLoadGeneration;
    private int _orgLoadGeneration;

    #endregion

    #region Commands

    /// <summary>
    /// Проверить выбранный отчёт из главного окна
    /// </summary>
    public ICommand CheckReportFromMain { get; private set; }

    /// <summary>
    /// Выбранная форма -> Выгрузка Excel -> Для анализа
    /// </summary>
    public ICommand ExcelExportFormAnalysis { get; private set; }

    /// <summary>
    /// Выбранная организация -> Импортировать отчёт в организацию -> Из Excel
    /// </summary>
    public ICommand ImportExcel { get; private set; }

    /// <summary>
    /// Выбранная организация -> Импортировать отчёт в организацию -> Из Raodb
    /// </summary>
    public ICommand ImportRaodb { get; private set; }
    
    #endregion

    #region Constructor

    public Forms1TabControlVM()
    {
        InitializeCommands();
    }

    public Forms1TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM)
    {
        InitializeCommands();
    }

    private void InitializeCommands()
    {
        CheckReportFromMain = new CheckReportFromMainAsyncCommand(this);
        ExcelExportFormAnalysis = new ExcelExportFormAnalysisAsyncCommand(this);
        ImportExcel = new ImportExcelAsyncCommand(this);
        ImportRaodb = new ImportRaodbAsyncCommand(this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Всего организаций с учётом фильтра.
    /// </summary>
    private protected override int FilteredRowsOrgs
    {
        get
        {
            return MainWindowListQuery.CountOrgsForm12(
                StaticConfiguration.DBModel, "1.0", SearchText);
        }
    }

    protected override bool GetAdditionalSearchConditions(Reports reps, string search)
    {
        return reps.Master_DB.Rows10[0].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase)
               || reps.Master_DB.Rows10[1].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase);
    }

    protected override void CheckAndResetFilterIfNeeded()
    {
        if (string.IsNullOrEmpty(FormNumWhiteList) || SelectedReports == null)
            return;

        // Не блокируем UI загрузкой stubs: сбрасываем только если stubs уже есть и формы нет.
        var has = _cache.TryHasFormNum(SelectedReports.Id, FormNumWhiteList);
        if (has == false)
            FormNumWhiteList = string.Empty;
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

    private protected override ObservableCollection<Report> ReportCollection
    {
        get
        {
            // Коллекция обновляется явно (UpdateReportCollection / async); getter без Sync.
            return _reportCollection;
        }
    }

    private protected override ObservableCollection<Reports> ReportsCollection
    {
        get
        {
            SyncOrgsCollection();
            return _orgsCollection;
        }
    }

    private protected override Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>> RowSelectors
    {
        get
        {
            return new Dictionary<string, Func<IQueryable<Report>, IQueryable<object>>>
            {
                ["1.1"] = q => q.Include(x => x.Rows11).SelectMany(x => x.Rows11),
                ["1.2"] = q => q.Include(x => x.Rows12).SelectMany(x => x.Rows12),
                ["1.3"] = q => q.Include(x => x.Rows13).SelectMany(x => x.Rows13),
                ["1.4"] = q => q.Include(x => x.Rows14).SelectMany(x => x.Rows14),
                ["1.5"] = q => q.Include(x => x.Rows15).SelectMany(x => x.Rows15),
                ["1.6"] = q => q.Include(x => x.Rows16).SelectMany(x => x.Rows16),
                ["1.7"] = q => q.Include(x => x.Rows17).SelectMany(x => x.Rows17),
                ["1.8"] = q => q.Include(x => x.Rows18).SelectMany(x => x.Rows18),
                ["1.9"] = q => q.Include(x => x.Rows19).SelectMany(x => x.Rows19),
            };
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

            var filter = string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;
            if (_cache.TryCountReports(SelectedReports.Id, filter, out var count))
                return count;

            // Пока stubs грузятся фоном — не блокируем UI.
            return _reportCollection.Count;
        }
    }

    #endregion

    #region Functions

    public override void UpdateReportCollection()
    {
        if (SelectedReports is null)
        {
            if (_reportCollection.Count > 0)
                _reportCollection.Clear();
            OnPropertyChanged(nameof(ReportCollection));
            return;
        }

        var orgId = SelectedReports.Id;
        var filter = string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;
        var page = CurrentPageForms;
        var pageSize = RowsCountForms;

        // Cache hit — мгновенно; иначе не блокируем UI, грузим фоном.
        if (_cache.TryGetReportPage(orgId, filter, page, pageSize, out var cached))
        {
            ReplaceCollection(_reportCollection, cached, r => r.Id);
            OnPropertyChanged(nameof(ReportCollection));
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));
            OnPropertyChanged(nameof(SelectedReports)); // обновить IsEnabled кнопок фильтра
            WarmSelectedOrgAndPrefetchReports();
            return;
        }

        var generation = Interlocked.Increment(ref _reportLoadGeneration);
        if (_reportCollection.Count > 0)
            _reportCollection.Clear();
        OnPropertyChanged(nameof(ReportCollection));

        var dbPath = StaticConfiguration.DBPath;
        _ = Task.Run(() =>
        {
            try
            {
                using var db = new DBModel(dbPath);
                var items = _cache.GetReportPage(db, orgId, filter, page, pageSize);
                var total = _cache.CountReports(db, orgId, filter);
                var hasFilter = string.IsNullOrEmpty(FormNumWhiteList)
                    || _cache.HasFormNum(db, orgId, FormNumWhiteList);

                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _reportLoadGeneration || SelectedReports?.Id != orgId)
                        return;

                    if (!hasFilter && !string.IsNullOrEmpty(FormNumWhiteList))
                        FormNumWhiteList = string.Empty;

                    ReplaceCollection(_reportCollection, items, r => r.Id);
                    OnPropertyChanged(nameof(ReportCollection));
                    OnPropertyChanged(nameof(TotalRowsForms));
                    OnPropertyChanged(nameof(TotalPagesForms));
                    OnPropertyChanged(nameof(SelectedReports));
                    WarmSelectedOrgAndPrefetchReports();
                });
            }
            catch
            {
                // UI уже очищен; следующий выбор повторит загрузку.
            }
        });
    }

    public void SetWhiteList(string formNum)
    {
        FormNumWhiteList = FormNumWhiteList != formNum 
            ? formNum 
            : string.Empty;

        if (CurrentPageForms != 1)
            CurrentPageForms = 1;
        else
            UpdateReportCollection();

        UpdateFormsPageInfo();
    }

    private protected override void NotifySearchTextChanged()
    {
        SyncOrgsCollection();
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        OnPropertyChanged(nameof(TotalReportCount));
    }

    public void UpdateOrgsPageInfo()
    {
        // Не InvalidateAll: при переключении вкладок кэш должен остаться.
        SyncOrgsCollection();
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        UpdateTotalReportsCount();
        if (SelectedReports != null)
            UpdateReportCollection();
    }

    /// <summary>
    /// После правки титула 1.0: пересобрать текущую страницу org из уже пропатченных ключей
    /// (без полного сброса OrgKeys / report warm-cache). Сохраняет SelectedReports.
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

        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
    }

    public override void UpdateReportsCollectionWithoutReCreation()
    {
        var selectedId = SelectedReports?.Master_DB?.Id;
        _cache.InvalidateAll();
        SyncOrgsCollection();

        if (selectedId.HasValue)
        {
            var restored = _orgsCollection.FirstOrDefault(r => r.Master_DB?.Id == selectedId.Value);
            if (restored != null)
                SetSelectedReportsWithoutReload(restored);
        }

        OnPropertyChanged(nameof(ReportsCollection));
    }

    private void SyncOrgsCollection()
    {
        var search = SearchText;
        var pageNum = CurrentPageOrgs;
        var pageSize = RowsCountOrgs;

        if (_cache.TryGetOrgPage(search, pageNum, pageSize, out var cached))
        {
            ReplaceCollection(_orgsCollection, cached.Items, r => r.Id);
            _cache.PrefetchAdjacentOrgPages(
                StaticConfiguration.DBPath, search, pageNum, pageSize, TotalPagesOrgs);
            return;
        }

        var generation = Interlocked.Increment(ref _orgLoadGeneration);
        var dbPath = StaticConfiguration.DBPath;

        // Быстрый sync-путь, если UI уже на странице и кэша нет — всё же грузим sync,
        // но параллельно не дёргаем TotalReportCount (убрано с CurrentPageOrgs).
        // Для смены страницы предпочтительнее показать кэш; при miss — короткий sync.
        try
        {
            var page = _cache.GetOrgPage(StaticConfiguration.DBModel, search, pageNum, pageSize);
            if (generation != _orgLoadGeneration) return;
            ReplaceCollection(_orgsCollection, page.Items, r => r.Id);
            _cache.PrefetchAdjacentOrgPages(dbPath, search, pageNum, pageSize, TotalPagesOrgs);
        }
        catch
        {
            // leave previous page visible
        }
    }

    private void WarmSelectedOrgAndPrefetchReports()
    {
        if (SelectedReports is null) return;

        var filter = string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;
        _cache.OnOrgSelected(
            StaticConfiguration.DBPath,
            SelectedReports.Id,
            filter,
            CurrentPageForms,
            RowsCountForms);

        _cache.PrefetchAdjacentReportPages(
            StaticConfiguration.DBPath,
            SelectedReports.Id,
            filter,
            CurrentPageForms,
            RowsCountForms,
            TotalPagesForms);
    }

    private static void ReplaceCollection<T>(
        ObservableCollection<T> target,
        IReadOnlyList<T> source,
        Func<T, int> idSelector)
    {
        if (target.Count == source.Count)
        {
            var same = true;
            for (var i = 0; i < source.Count; i++)
            {
                if (idSelector(target[i]) != idSelector(source[i]))
                {
                    same = false;
                    break;
                }
            }

            if (same)
                return;
        }

        target.Clear();
        foreach (var item in source)
            target.Add(item);
    }

    #endregion
}
