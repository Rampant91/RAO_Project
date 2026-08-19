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
using Avalonia.Threading;

namespace Client_App.ViewModels.MainWindowTabs;

public class Forms5TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '5';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private readonly ObservableCollection<Reports> _orgsCollection = new();
    private readonly Forms1WarmCache _cache = Forms1WarmCache.Instance;
    private int _reportLoadGeneration;
    private int _orgLoadGeneration;
    private const string MasterFormNum = "5.0";

    #endregion

    #region Constructor

    public Forms5TabControlVM() { }

    public Forms5TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override int FilteredRowsOrgs
    {
        get
        {
            if (_cache.TryGetOrgPage(SearchText, CurrentPageOrgs, RowsCountOrgs, out var cached, MasterFormNum))
                return cached.TotalCount;
            return MainWindowListQuery.GetOrgPageForm50(
                StaticConfiguration.DBModel, SearchText, page: 1, pageSize: 1).TotalCount;
        }
    }

    protected override void CheckAndResetFilterIfNeeded()
    {
        if (string.IsNullOrEmpty(FormNumWhiteList) || SelectedReports == null)
            return;

        var has = _cache.TryHasFormNum(SelectedReports.Id, FormNumWhiteList);
        if (has == false)
            FormNumWhiteList = string.Empty;
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

    private protected override ObservableCollection<Report> ReportCollection => _reportCollection;

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
                ["5.1"] = q => q.Include(x => x.Rows51).SelectMany(x => x.Rows51),
                ["5.2"] = q => q.Include(x => x.Rows52).SelectMany(x => x.Rows52),
                ["5.3"] = q => q.Include(x => x.Rows53).SelectMany(x => x.Rows53),
                ["5.4"] = q => q.Include(x => x.Rows54).SelectMany(x => x.Rows54),
                ["5.5"] = q => q.Include(x => x.Rows55).SelectMany(x => x.Rows55),
                ["5.6"] = q => q.Include(x => x.Rows56).SelectMany(x => x.Rows56),
                ["5.7"] = q => q.Include(x => x.Rows57).SelectMany(x => x.Rows57),
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

        if (_cache.TryGetReportPage(orgId, filter, page, pageSize, out var cached))
        {
            ReplaceCollection(_reportCollection, cached, r => r.Id);
            OnPropertyChanged(nameof(ReportCollection));
            OnPropertyChanged(nameof(TotalRowsForms));
            OnPropertyChanged(nameof(TotalPagesForms));
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
                Dispatcher.UIThread.Post(() =>
                {
                    if (generation != _reportLoadGeneration || SelectedReports?.Id != orgId)
                        return;
                    ReplaceCollection(_reportCollection, items, r => r.Id);
                    OnPropertyChanged(nameof(ReportCollection));
                    OnPropertyChanged(nameof(TotalRowsForms));
                    OnPropertyChanged(nameof(TotalPagesForms));
                    WarmSelectedOrgAndPrefetchReports();
                });
            }
            catch { }
        });
    }

    private void SyncOrgsCollection()
    {
        var search = SearchText;
        var pageNum = CurrentPageOrgs;
        var pageSize = RowsCountOrgs;

        if (_cache.TryGetOrgPage(search, pageNum, pageSize, out var cached, MasterFormNum))
        {
            ReplaceCollection(_orgsCollection, cached.Items, r => r.Id);
            _cache.PrefetchAdjacentOrgPages(
                StaticConfiguration.DBPath, search, pageNum, pageSize, TotalPagesOrgs, MasterFormNum);
            return;
        }

        var generation = Interlocked.Increment(ref _orgLoadGeneration);
        try
        {
            var orgPage = _cache.GetOrgPage(StaticConfiguration.DBModel, search, pageNum, pageSize, MasterFormNum);
            if (generation != _orgLoadGeneration) return;
            ReplaceCollection(_orgsCollection, orgPage.Items, r => r.Id);
            _cache.PrefetchAdjacentOrgPages(
                StaticConfiguration.DBPath, search, pageNum, pageSize, TotalPagesOrgs, MasterFormNum);
        }
        catch { }
    }

    private void WarmSelectedOrgAndPrefetchReports()
    {
        if (SelectedReports is null) return;
        var filter = string.IsNullOrEmpty(FormNumWhiteList) ? null : FormNumWhiteList;
        _cache.OnOrgSelected(
            StaticConfiguration.DBPath, SelectedReports.Id, filter, CurrentPageForms, RowsCountForms);
        _cache.PrefetchAdjacentReportPages(
            StaticConfiguration.DBPath, SelectedReports.Id, filter,
            CurrentPageForms, RowsCountForms, TotalPagesForms);
    }

    private static void ReplaceCollection<T>(
        ObservableCollection<T> target, IReadOnlyList<T> source, Func<T, int> idSelector)
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
            if (same) return;
        }

        target.Clear();
        foreach (var item in source)
            target.Add(item);
    }

    public void SetWhiteList(string formNum)
    {
        if (FormNumWhiteList != formNum)
            FormNumWhiteList = formNum;
        else
            FormNumWhiteList = "";

        UpdateReportCollection();
        UpdateFormsPageInfo();
    }

    private protected override void NotifySearchTextChanged()
    {
        SyncOrgsCollection();
        OnPropertyChanged(nameof(ReportsCollection));
        OnPropertyChanged(nameof(FilteredRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
    }

    public void UpdateOrgsPageInfo()
    {
        SyncOrgsCollection();
        OnPropertyChanged(nameof(TotalRowsOrgs));
        OnPropertyChanged(nameof(TotalPagesOrgs));
        OnPropertyChanged(nameof(ReportsCollection));
        if (SelectedReports != null)
            UpdateReportCollection();
    }

    #endregion
}