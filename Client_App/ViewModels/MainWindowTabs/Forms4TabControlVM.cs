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

public class Forms4TabControlVM : FormsTabControlBaseVM
{
    #region Fields

    private protected override byte DefaultFormsPerPage => 10;

    private protected override byte DefaultOrgsPerPage => 8;

    private protected override char FormNum => '4';

    private readonly ObservableCollection<Report> _reportCollection = new();
    private readonly ObservableCollection<Reports> _orgsCollection = new();
    private readonly Forms1WarmCache _cache = Forms1WarmCache.Instance;
    private int _reportLoadGeneration;
    private int _orgLoadGeneration;
    private const string MasterFormNum = "4.0";

    #endregion

    #region Constructor

    public Forms4TabControlVM() { }

    public Forms4TabControlVM(MainWindowVM mainWindowVM) : base(mainWindowVM) { }

    #endregion

    #region Properties

    private protected override int FilteredRowsOrgs
    {
        get
        {
            if (_cache.TryGetOrgPage(SearchText, CurrentPageOrgs, RowsCountOrgs, out var cached, MasterFormNum))
                return cached.TotalCount;
            var page = MainWindowListQuery.GetOrgPageForm40(
                StaticConfiguration.DBModel, SearchText, page: 1, pageSize: 1);
            return page.TotalCount;
        }
    }

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
                ["4.1"] = q => q.Include(x => x.Rows41).SelectMany(x => x.Rows41)
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
            if (_cache.TryCountReports(SelectedReports.Id, null, out var count))
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
        var page = CurrentPageForms;
        var pageSize = RowsCountForms;

        if (_cache.TryGetReportPage(orgId, null, page, pageSize, out var cached))
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
                var items = _cache.GetReportPage(db, orgId, null, page, pageSize);
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
        _cache.OnOrgSelected(
            StaticConfiguration.DBPath, SelectedReports.Id, null, CurrentPageForms, RowsCountForms);
        _cache.PrefetchAdjacentReportPages(
            StaticConfiguration.DBPath, SelectedReports.Id, null,
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