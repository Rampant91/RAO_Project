using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Warm-cache главного окна для форм 1.x: stubs организации, страницы org/report ±2,
/// LRU вытеснение недавно покинутых организаций.
/// Prefetch всегда в отдельном <see cref="DBModel"/> (DbContext не потокобезопасен).
/// </summary>
public sealed class Forms1WarmCache
{
    public static Forms1WarmCache Instance { get; } = new();

    private const int MaxOrgsWithStubs = 6;
    private const int MaxReportPages = 15;
    private const int MaxOrgPages = 9;

    private readonly object _gate = new();
    private readonly Dictionary<int, OrgWarmState> _orgStubs = new();
    private readonly LinkedList<int> _orgLru = new();
    private readonly Dictionary<string, List<Report>> _reportPages = new();
    private readonly LinkedList<string> _reportPageLru = new();
    private readonly Dictionary<string, CachedOrgPage> _orgPages = new();
    private readonly LinkedList<string> _orgPageLru = new();

    private CancellationTokenSource? _prefetchCts;
    private CancellationTokenSource? _orgPrefetchCts;
    private int? _activeOrgId;

    private sealed class OrgWarmState
    {
        public required List<ReportListStub> Stubs { get; init; }
    }

    private sealed class CachedOrgPage
    {
        public required List<Reports> Items { get; init; }
        public required int TotalCount { get; init; }
    }

    public void InvalidateAll()
    {
        lock (_gate)
        {
            _orgStubs.Clear();
            _orgLru.Clear();
            _reportPages.Clear();
            _reportPageLru.Clear();
            _orgPages.Clear();
            _orgPageLru.Clear();
            _activeOrgId = null;
        }

        MainWindowListQuery.InvalidateOrgKeysCacheForm10();
        CancelPrefetch();
        CancelPrefetchOrgsOnly();
    }

    public void InvalidateOrg(int orgId)
    {
        lock (_gate)
        {
            _orgStubs.Remove(orgId);
            _orgLru.Remove(orgId);
            RemoveReportPagesForOrg_NoLock(orgId);
        }
    }

    /// <summary>
    /// Сбрасывает только кэш страниц org-грида (после смены RegNo/OKPO/Short у одной org).
    /// Ключи OrgKeys и stubs отчётов не трогает.
    /// </summary>
    public void InvalidateOrgPages()
    {
        lock (_gate)
        {
            _orgPages.Clear();
            _orgPageLru.Clear();
        }

        CancelPrefetchOrgsOnly();
    }

    /// <summary>null — stubs ещё не в кэше.</summary>
    public bool TryGetReportStubs(int orgId, out List<ReportListStub> stubs)
    {
        lock (_gate)
        {
            if (!_orgStubs.TryGetValue(orgId, out var state))
            {
                stubs = [];
                return false;
            }

            stubs = state.Stubs;
            return true;
        }
    }

    /// <summary>null — stubs ещё не загружены; иначе есть ли форма.</summary>
    public bool? TryHasFormNum(int orgId, string formNum)
    {
        lock (_gate)
        {
            if (!_orgStubs.TryGetValue(orgId, out var state))
                return null;
            return state.Stubs.Any(s => s.FormNum == formNum);
        }
    }

    public bool TryGetReportPage(
        int orgId, string? formNumWhiteList, int page, int pageSize, out List<Report> items)
    {
        var (safePage, safePageSize, _) = PagingHelper.Normalize(page, pageSize);
        var key = ReportPageKey(orgId, formNumWhiteList, safePage, safePageSize);
        lock (_gate)
        {
            if (_reportPages.TryGetValue(key, out var cached))
            {
                TouchLru(_reportPageLru, key);
                items = cached;
                return true;
            }
        }

        items = [];
        return false;
    }

    public bool TryGetOrgPage(
        string? searchText, int page, int pageSize, out PagedResult<Reports> result)
    {
        var (safePage, safePageSize, _) = PagingHelper.Normalize(page, pageSize);
        var key = OrgPageKey(searchText, safePage, safePageSize);
        lock (_gate)
        {
            if (_orgPages.TryGetValue(key, out var cached))
            {
                TouchLru(_orgPageLru, key);
                result = new PagedResult<Reports>
                {
                    Items = cached.Items,
                    TotalCount = cached.TotalCount,
                    Page = safePage,
                    PageSize = safePageSize
                };
                return true;
            }
        }

        result = new PagedResult<Reports>
        {
            Items = [],
            TotalCount = 0,
            Page = safePage,
            PageSize = safePageSize
        };
        return false;
    }

    public bool TryCountReports(int orgId, string? formNumWhiteList, out int count)
    {
        lock (_gate)
        {
            if (!_orgStubs.TryGetValue(orgId, out var state))
            {
                count = 0;
                return false;
            }

            count = string.IsNullOrWhiteSpace(formNumWhiteList)
                ? state.Stubs.Count
                : state.Stubs.Count(s => s.FormNum == formNumWhiteList);
            return true;
        }
    }

    public int CountReports(DBModel db, int orgId, string? formNumWhiteList)
    {
        var stubs = GetOrLoadStubs(db, orgId);
        if (string.IsNullOrWhiteSpace(formNumWhiteList))
            return stubs.Count;
        return stubs.Count(s => s.FormNum == formNumWhiteList);
    }

    public bool HasFormNum(DBModel db, int orgId, string formNum) =>
        GetOrLoadStubs(db, orgId).Any(s => s.FormNum == formNum);

    public List<Report> GetReportPage(
        DBModel db,
        int orgId,
        string? formNumWhiteList,
        int page,
        int pageSize)
    {
        var (safePage, safePageSize, _) = PagingHelper.Normalize(page, pageSize);
        var key = ReportPageKey(orgId, formNumWhiteList, safePage, safePageSize);

        lock (_gate)
        {
            if (_reportPages.TryGetValue(key, out var cached))
            {
                TouchLru(_reportPageLru, key);
                return cached;
            }
        }

        var stubs = GetOrLoadStubs(db, orgId);
        var (_, _, skip) = PagingHelper.Normalize(safePage, safePageSize);
        var pageIds = MainWindowListQuery.GetOrderedReportIds(stubs, formNumWhiteList)
            .Skip(skip)
            .Take(safePageSize)
            .ToList();
        var items = MainWindowListQuery.LoadReportsByIds(db, pageIds);
        PutReportPage(key, items);
        return items;
    }

    public PagedResult<Reports> GetOrgPage(
        DBModel db, string? searchText, int page, int pageSize)
    {
        var (safePage, safePageSize, _) = PagingHelper.Normalize(page, pageSize);
        var key = OrgPageKey(searchText, safePage, safePageSize);

        lock (_gate)
        {
            if (_orgPages.TryGetValue(key, out var cached))
            {
                TouchLru(_orgPageLru, key);
                return new PagedResult<Reports>
                {
                    Items = cached.Items,
                    TotalCount = cached.TotalCount,
                    Page = safePage,
                    PageSize = safePageSize
                };
            }
        }

        var result = MainWindowListQuery.GetOrgPageForm12(db, "1.0", searchText, safePage, safePageSize);
        PutOrgPage(key, result.Items.ToList(), result.TotalCount);
        return result;
    }

    public void OnOrgSelected(
        string dbPath,
        int orgId,
        string? formNumWhiteList,
        int reportPage,
        int reportPageSize)
    {
        _activeOrgId = orgId;
        CancelPrefetch();
        var cts = new CancellationTokenSource();
        _prefetchCts = cts;
        var filter = formNumWhiteList;
        var page = reportPage;
        var pageSize = reportPageSize;

        _ = Task.Run(async () =>
        {
            try
            {
                await using var db = new DBModel(dbPath);
                GetOrLoadStubs(db, orgId);
                GetReportPage(db, orgId, filter, page, pageSize);

                var total = CountReports(db, orgId, filter);
                var totalPages = total <= 0
                    ? 0
                    : (total + Math.Max(1, pageSize) - 1) / Math.Max(1, pageSize);

                foreach (var p in NeighborPages(page, radius: 2))
                {
                    if (cts.IsCancellationRequested || p < 1 || p > totalPages) continue;
                    GetReportPage(db, orgId, filter, p, pageSize);
                }
            }
            catch
            {
                // best-effort
            }
        }, cts.Token);
    }

    public void PrefetchAdjacentReportPages(
        string dbPath,
        int orgId,
        string? formNumWhiteList,
        int currentPage,
        int pageSize,
        int totalPages)
    {
        if (_activeOrgId != orgId) return;
        CancelPrefetch();
        var cts = new CancellationTokenSource();
        _prefetchCts = cts;
        var filter = formNumWhiteList;

        _ = Task.Run(async () =>
        {
            try
            {
                await using var db = new DBModel(dbPath);
                foreach (var p in NeighborPages(currentPage, radius: 2))
                {
                    if (cts.IsCancellationRequested || p < 1 || p > totalPages) continue;
                    GetReportPage(db, orgId, filter, p, pageSize);
                }
            }
            catch
            {
                // best-effort
            }
        }, cts.Token);
    }

    public void PrefetchAdjacentOrgPages(
        string dbPath, string? searchText, int currentPage, int pageSize, int totalPages)
    {
        CancelPrefetchOrgsOnly();
        var cts = new CancellationTokenSource();
        _orgPrefetchCts = cts;
        var search = searchText;

        _ = Task.Run(async () =>
        {
            try
            {
                await using var db = new DBModel(dbPath);
                foreach (var p in NeighborPages(currentPage, radius: 2))
                {
                    if (cts.IsCancellationRequested || p < 1 || p > totalPages) continue;
                    GetOrgPage(db, search, p, pageSize);
                }
            }
            catch
            {
                // best-effort
            }
        }, cts.Token);
    }

    private static IEnumerable<int> NeighborPages(int current, int radius)
    {
        for (var d = 1; d <= radius; d++)
        {
            yield return current - d;
            yield return current + d;
        }
    }

    private void CancelPrefetch()
    {
        try { _prefetchCts?.Cancel(); } catch { /* ignore */ }
        _prefetchCts = null;
    }

    private void CancelPrefetchOrgsOnly()
    {
        try { _orgPrefetchCts?.Cancel(); } catch { /* ignore */ }
        _orgPrefetchCts = null;
    }

    private List<ReportListStub> GetOrLoadStubs(DBModel db, int orgId)
    {
        lock (_gate)
        {
            if (_orgStubs.TryGetValue(orgId, out var existing))
            {
                TouchLru(_orgLru, orgId);
                return existing.Stubs;
            }
        }

        var stubs = MainWindowListQuery.LoadReportStubs(db, orgId);
        lock (_gate)
        {
            if (_orgStubs.TryGetValue(orgId, out var raced))
            {
                TouchLru(_orgLru, orgId);
                return raced.Stubs;
            }

            _orgStubs[orgId] = new OrgWarmState { Stubs = stubs };
            TouchLru(_orgLru, orgId);
            EvictOrgs_NoLock();
            return stubs;
        }
    }

    private void PutReportPage(string key, List<Report> items)
    {
        lock (_gate)
        {
            _reportPages[key] = items;
            TouchLru(_reportPageLru, key);
            EvictReportPages_NoLock();
        }
    }

    private void PutOrgPage(string key, List<Reports> items, int totalCount)
    {
        lock (_gate)
        {
            _orgPages[key] = new CachedOrgPage { Items = items, TotalCount = totalCount };
            TouchLru(_orgPageLru, key);
            EvictOrgPages_NoLock();
        }
    }

    private void EvictOrgs_NoLock()
    {
        while (_orgLru.Count > MaxOrgsWithStubs)
        {
            var node = _orgLru.First;
            while (node != null && _activeOrgId == node.Value)
                node = node.Next;

            if (node == null)
                break;

            var oldest = node.Value;
            _orgLru.Remove(oldest);
            _orgStubs.Remove(oldest);
            RemoveReportPagesForOrg_NoLock(oldest);
        }
    }

    private void EvictReportPages_NoLock()
    {
        while (_reportPageLru.Count > MaxReportPages)
        {
            var oldest = _reportPageLru.First!.Value;
            _reportPageLru.RemoveFirst();
            _reportPages.Remove(oldest);
        }
    }

    private void EvictOrgPages_NoLock()
    {
        while (_orgPageLru.Count > MaxOrgPages)
        {
            var oldest = _orgPageLru.First!.Value;
            _orgPageLru.RemoveFirst();
            _orgPages.Remove(oldest);
        }
    }

    private void RemoveReportPagesForOrg_NoLock(int orgId)
    {
        var prefix = orgId + "|";
        var toRemove = _reportPages.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)).ToList();
        foreach (var k in toRemove)
        {
            _reportPages.Remove(k);
            _reportPageLru.Remove(k);
        }
    }

    private static void TouchLru<T>(LinkedList<T> lru, T key) where T : notnull
    {
        lru.Remove(key);
        lru.AddLast(key);
    }

    private static string ReportPageKey(int orgId, string? filter, int page, int pageSize) =>
        $"{orgId}|{filter ?? ""}|{page}|{pageSize}";

    private static string OrgPageKey(string? search, int page, int pageSize) =>
        $"{search ?? ""}|{page}|{pageSize}";
}
