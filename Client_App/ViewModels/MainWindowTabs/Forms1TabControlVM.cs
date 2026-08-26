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

    private protected override byte DefaultFormsPerPage => MainWindowPagingDefaults.DefaultFormsPerPage;

    private protected override byte DefaultOrgsPerPage => MainWindowPagingDefaults.DefaultOrgsPerPage;

    private protected override char FormNum => '1';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private int _reportLoadGeneration;

    #endregion

    #region Commands

    public ICommand CheckReportFromMain { get; private set; }
    public ICommand ExcelExportFormAnalysis { get; private set; }
    public ICommand ImportExcel { get; private set; }
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

    protected override bool GetAdditionalSearchConditions(Reports reps, string search)
    {
        return reps.Master_DB.Rows10[0].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase)
               || reps.Master_DB.Rows10[1].ShortJurLico_DB.Contains(search, StringComparison.CurrentCultureIgnoreCase);
    }

    protected override void CheckAndResetFilterIfNeeded()
    {
        if (string.IsNullOrEmpty(FormNumWhiteList) || SelectedReports == null)
            return;

        var has = _cache.TryHasFormNum(SelectedReports.Id, FormNumWhiteList);
        if (has == false)
            FormNumWhiteList = string.Empty;
    }

    protected override string? GetReportFormFilter() =>
        string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;

    protected override bool NotifySelectedReportsOnReportPageUpdate => true;

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

    private protected override ObservableCollection<Report> ReportCollection => _reportCollection;

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

    private protected override int TotalRowsForms
    {
        get
        {
            if (SelectedReports is null) return 0;

            var filter = GetReportFormFilter();
            if (_cache.TryCountReports(SelectedReports.Id, filter, out var count))
                return count;

            return _reportCollection.Count;
        }
    }

    #endregion

    #region Functions

    public override void UpdateReportCollection()
    {
        if (SelectedReports is null)
        {
            Interlocked.Increment(ref _reportLoadGeneration);
            SetReportsLoading(false);
            if (_reportCollection.Count > 0)
                _reportCollection.Clear();
            OnPropertyChanged(nameof(ReportCollection));
            return;
        }

        var orgId = SelectedReports.Id;
        var filter = GetReportFormFilter();
        var page = CurrentPageFormsValue;
        var pageSize = RowsCountForms;

        if (_cache.TryGetReportPage(orgId, filter, page, pageSize, out var cached))
        {
            Interlocked.Increment(ref _reportLoadGeneration);
            SetReportsLoading(false);
            ReplaceCollection(_reportCollection, cached, r => r.Id);
            OnPropertyChanged(nameof(ReportCollection));
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));
            if (NotifySelectedReportsOnReportPageUpdate)
                OnPropertyChanged(nameof(SelectedReports));
            WarmSelectedOrgAndPrefetchReports();
            return;
        }

        var generation = Interlocked.Increment(ref _reportLoadGeneration);
        ScheduleReportsLoadingIndicator(generation, () => _reportLoadGeneration);
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
                var hasFilter = string.IsNullOrEmpty(FormNumWhiteList)
                    || _cache.HasFormNum(db, orgId, FormNumWhiteList);

                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _reportLoadGeneration || SelectedReports?.Id != orgId)
                        return;

                    SetReportsLoading(false);
                    if (!hasFilter && !string.IsNullOrEmpty(FormNumWhiteList))
                        FormNumWhiteList = string.Empty;

                    ReplaceCollection(_reportCollection, items, r => r.Id);
                    OnPropertyChanged(nameof(ReportCollection));
                    OnPropertyChanged(nameof(TotalRowsForms));
                    OnPropertyChanged(nameof(TotalPagesForms));
                    if (NotifySelectedReportsOnReportPageUpdate)
                        OnPropertyChanged(nameof(SelectedReports));
                    WarmSelectedOrgAndPrefetchReports();
                });
            }
            catch
            {
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation == _reportLoadGeneration)
                        SetReportsLoading(false);
                });
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

    /// <summary>
    /// Полный сброс warm-cache и OrgKeys (тяжелее базового <see cref="FormsTabControlBaseVM.UpdateReportsCollectionWithoutReCreation"/>).
    /// </summary>
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
        RefreshCountsAsync();
    }

    #endregion
}
