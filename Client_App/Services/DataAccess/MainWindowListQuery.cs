using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Resources.CustomComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Лёгкий stub отчёта для сортировки/фильтрации без загрузки полной сущности.
/// </summary>
public sealed class ReportListStub
{
    public int Id { get; init; }
    public string FormNum { get; init; } = "";
    public string? StartPeriod { get; init; }
    public string? EndPeriod { get; init; }
    public int? Year { get; init; }
    public byte CorrectionNumber { get; init; }
}

/// <summary>
/// Выборка страниц организаций и отчётов для главного окна (без полной загрузки Local).
/// <para>
/// Два уровня кэша org: (1) in-memory <see cref="OrgKey"/>-индекс на сессию — быстрый paging/поиск;
/// (2) warm-cache страниц в <see cref="Forms1WarmCache"/> с LRU-вытеснением.
/// </para>
/// </summary>
public static class MainWindowListQuery
{
    private static readonly object OrgKeysForm10Lock = new();
    private static List<OrgKey>? _cachedOrgKeysForm10;

    private static readonly object OrgKeysForm20Lock = new();
    private static List<OrgKey>? _cachedOrgKeysForm20;

    private static readonly object OrgKeysForm40Lock = new();
    private static List<OrgKey>? _cachedOrgKeysForm40;

    private static readonly object OrgKeysForm50Lock = new();
    private static List<OrgKey>? _cachedOrgKeysForm50;

    internal sealed class OrgKey
    {
        public int Id { get; init; }
        public string RegNo { get; init; } = "";
        public string Okpo { get; init; } = "";
        public string ShortJurLico { get; init; } = "";
        public short? CodeSubjectRf { get; init; }
        public string SubjectRf { get; init; } = "";
        public string Name50 { get; init; } = "";
    }

    /// <summary>
    /// Сбрасывает кэш ключей организаций формы 1.0 (после импорта/удаления/добавления).
    /// </summary>
    public static void InvalidateOrgKeysCacheForm10()
    {
        lock (OrgKeysForm10Lock)
            _cachedOrgKeysForm10 = null;
    }

    /// <summary>
    /// Сбрасывает кэш ключей организаций формы 2.0 (после импорта/удаления/добавления).
    /// </summary>
    public static void InvalidateOrgKeysCacheForm20()
    {
        lock (OrgKeysForm20Lock)
            _cachedOrgKeysForm20 = null;
    }

    /// <summary>
    /// Сбрасывает in-memory ключи org для вкладок 1.0, 2.0, 4.0 и 5.0.
    /// </summary>
    public static void InvalidateAllOrgKeysCaches()
    {
        InvalidateOrgKeysCacheForm10();
        InvalidateOrgKeysCacheForm20();
        InvalidateOrgKeysCacheForm40();
        InvalidateOrgKeysCacheForm50();
    }

    /// <summary>
    /// Устаревшее имя — используйте <see cref="InvalidateAllOrgKeysCaches"/>.
    /// </summary>
    [Obsolete("Use InvalidateAllOrgKeysCaches")]
    public static void InvalidateOrgKeysCachesForm12() => InvalidateAllOrgKeysCaches();

    /// <summary>
    /// Сбрасывает кэш ключей организаций формы 4.0 (после импорта/удаления/добавления).
    /// </summary>
    public static void InvalidateOrgKeysCacheForm40()
    {
        lock (OrgKeysForm40Lock)
            _cachedOrgKeysForm40 = null;
    }

    /// <summary>
    /// Сбрасывает кэш ключей организаций формы 5.0 (после импорта/удаления/добавления).
    /// </summary>
    public static void InvalidateOrgKeysCacheForm50()
    {
        lock (OrgKeysForm50Lock)
            _cachedOrgKeysForm50 = null;
    }

    /// <summary>
    /// Точечно обновляет ключ одной org в кэше (после правки титула 1.0).
    /// Если кэш ещё не загружен — no-op: следующий LoadOrgKeysForm12 возьмёт данные из БД.
    /// Не делает полный Select по всем form_10.
    /// </summary>
    public static void UpsertOrgKeyForm10FromMaster(int reportsId, Report master)
    {
        if (master?.FormNum_DB is not "1.0")
            return;

        var rows = master.Rows10
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => (r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB))
            .ToList();

        var key = ToOrgKeyFromTitleRows(reportsId, rows);

        lock (OrgKeysForm10Lock)
        {
            if (_cachedOrgKeysForm10 == null)
                return;

            var idx = _cachedOrgKeysForm10.FindIndex(k => k.Id == reportsId);
            if (idx >= 0)
                _cachedOrgKeysForm10[idx] = key;
            else
                _cachedOrgKeysForm10.Add(key);
        }
    }

    /// <summary>
    /// Точечно обновляет ключ одной org в кэше (после правки титула 2.0).
    /// </summary>
    public static void UpsertOrgKeyForm20FromMaster(int reportsId, Report master)
    {
        if (master?.FormNum_DB is not "2.0")
            return;

        var rows = master.Rows20
            .OrderBy(r => r.NumberInOrder_DB)
            .Select(r => (r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB))
            .ToList();

        var key = ToOrgKeyFromTitleRows(reportsId, rows);

        lock (OrgKeysForm20Lock)
        {
            if (_cachedOrgKeysForm20 == null)
                return;

            var idx = _cachedOrgKeysForm20.FindIndex(k => k.Id == reportsId);
            if (idx >= 0)
                _cachedOrgKeysForm20[idx] = key;
            else
                _cachedOrgKeysForm20.Add(key);
        }
    }

    /// <summary>
    /// Страница организаций форм 1.0 / 2.0 с RegNo/Okpo/ShortJurLico.
    /// </summary>
    public static PagedResult<Reports> GetOrgPageForm12(
        DBModel db,
        string masterFormNum,
        string? searchText,
        int page,
        int pageSize)
    {
        var (safePage, safePageSize, skip) = PagingHelper.Normalize(page, pageSize);
        var keys = LoadOrgKeysForm12(db, masterFormNum);
        var filtered = FilterOrgKeys(keys, searchText);
        var comparator = new CustomReportsComparer();
        var orderedIds = filtered
            .OrderBy(k => k.RegNo, comparator)
            .ThenBy(k => k.Okpo, comparator)
            .Select(k => k.Id)
            .ToList();

        var total = orderedIds.Count;
        var pageIds = orderedIds.Skip(skip).Take(safePageSize).ToList();
        var items = LoadOrgsByIds(db, pageIds, masterFormNum);
        return new PagedResult<Reports>
        {
            Items = items,
            TotalCount = total,
            Page = safePage,
            PageSize = safePageSize
        };
    }

    public static int CountOrgsForm12(DBModel db, string masterFormNum, string? searchText)
    {
        var keys = LoadOrgKeysForm12(db, masterFormNum);
        return FilterOrgKeys(keys, searchText).Count();
    }

    public static int CountOrgsForm40(DBModel db, string? searchText) =>
        FilterOrgKeysForm40(LoadOrgKeysForm40(db), searchText).Count();

    public static int CountOrgsForm50(DBModel db, string? searchText) =>
        FilterOrgKeysForm50(LoadOrgKeysForm50(db), searchText).Count();

    public static PagedResult<Reports> GetOrgPageForm40(
        DBModel db, string? searchText, int page, int pageSize)
    {
        var (safePage, safePageSize, skip) = PagingHelper.Normalize(page, pageSize);
        var orderedIds = GetFilteredOrgIdsForm40(db, searchText);
        var pageIds = orderedIds.Skip(skip).Take(safePageSize).ToList();
        var items = LoadOrgsByIds(db, pageIds, "4.0");
        return new PagedResult<Reports>
        {
            Items = items,
            TotalCount = orderedIds.Count,
            Page = safePage,
            PageSize = safePageSize
        };
    }

    public static PagedResult<Reports> GetOrgPageForm50(
        DBModel db, string? searchText, int page, int pageSize)
    {
        var (safePage, safePageSize, skip) = PagingHelper.Normalize(page, pageSize);
        var orderedIds = GetFilteredOrgIdsForm50(db, searchText);
        var pageIds = orderedIds.Skip(skip).Take(safePageSize).ToList();
        var items = LoadOrgsByIds(db, pageIds, "5.0");
        return new PagedResult<Reports>
        {
            Items = items,
            TotalCount = orderedIds.Count,
            Page = safePage,
            PageSize = safePageSize
        };
    }

    public static int CountReports(
        DBModel db, int reportsId, string? formNumWhiteList = null)
    {
        var q = db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => r.Reports != null && r.Reports.Id == reportsId);
        if (!string.IsNullOrWhiteSpace(formNumWhiteList))
            q = q.Where(r => r.FormNum_DB == formNumWhiteList);
        return q.Count();
    }

    public static int CountAllReportsForFormType(
        DBModel db, char formTypeDigit, string? orgSearchText, string masterFormNum)
    {
        var prefix = formTypeDigit + ".";

        // Без поиска — JOIN по master-форме, без IN (...).
        if (string.IsNullOrWhiteSpace(orgSearchText))
        {
            return db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && r.Reports.DBObservableId != null
                            && r.Reports.Master_DB.FormNum_DB == masterFormNum
                            && r.FormNum_DB.StartsWith(prefix)
                            && !r.FormNum_DB.EndsWith(".0"));
        }

        var filteredOrgIds = FilterOrgKeys(LoadOrgKeysForm12(db, masterFormNum), orgSearchText)
            .Select(k => k.Id)
            .ToList();
        if (filteredOrgIds.Count == 0) return 0;

        return FirebirdInClause.SumCount(filteredOrgIds, batch =>
            db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && batch.Contains(r.Reports.Id)
                            && r.FormNum_DB.StartsWith(prefix)
                            && !r.FormNum_DB.EndsWith(".0")));
    }

    public static int CountAllReportsForForm40(DBModel db, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && r.Reports.DBObservableId != null
                            && r.Reports.Master_DB.FormNum_DB == "4.0"
                            && r.FormNum_DB.StartsWith("4.")
                            && !r.FormNum_DB.EndsWith(".0"));
        }

        var orgIds = GetFilteredOrgIdsForm40(db, searchText);
        if (orgIds.Count == 0) return 0;
        return FirebirdInClause.SumCount(orgIds, batch =>
            db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && batch.Contains(r.Reports.Id)
                            && r.FormNum_DB.StartsWith("4.")
                            && !r.FormNum_DB.EndsWith(".0")));
    }

    public static int CountAllReportsForForm50(DBModel db, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && r.Reports.DBObservableId != null
                            && r.Reports.Master_DB.FormNum_DB == "5.0"
                            && r.FormNum_DB.StartsWith("5.")
                            && !r.FormNum_DB.EndsWith(".0"));
        }

        var orgIds = GetFilteredOrgIdsForm50(db, searchText);
        if (orgIds.Count == 0) return 0;
        return FirebirdInClause.SumCount(orgIds, batch =>
            db.ReportCollectionDbSet
                .AsNoTracking()
                .Count(r => r.Reports != null
                            && batch.Contains(r.Reports.Id)
                            && r.FormNum_DB.StartsWith("5.")
                            && !r.FormNum_DB.EndsWith(".0")));
    }

    /// <summary>
    /// Страница организаций для вкладки формы (1/2/4/5).
    /// </summary>
    public static PagedResult<Reports> GetOrgPage(
        DBModel db, char formDigit, string? searchText, int page, int pageSize) =>
        formDigit switch
        {
            '1' => GetOrgPageForm12(db, "1.0", searchText, page, pageSize),
            '2' => GetOrgPageForm12(db, "2.0", searchText, page, pageSize),
            '4' => GetOrgPageForm40(db, searchText, page, pageSize),
            '5' => GetOrgPageForm50(db, searchText, page, pageSize),
            _ => new PagedResult<Reports>
            {
                Items = [],
                TotalCount = 0,
                Page = 1,
                PageSize = Math.Max(1, pageSize)
            }
        };

    public static List<ReportListStub> LoadReportStubs(DBModel db, int reportsId)
    {
        return db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => r.Reports != null && r.Reports.Id == reportsId)
            .Select(r => new ReportListStub
            {
                Id = r.Id,
                FormNum = r.FormNum_DB,
                StartPeriod = r.StartPeriod_DB,
                EndPeriod = r.EndPeriod_DB,
                Year = r.Year_DB,
                CorrectionNumber = r.CorrectionNumber_DB
            })
            .ToList();
    }

    /// <summary>
    /// Лёгкие stubs только одной формы (для popup выбора отчёта — без загрузки всех форм org).
    /// </summary>
    public static List<ReportListStub> LoadReportStubsForForm(DBModel db, int reportsId, string formNum)
    {
        return db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => r.Reports != null && r.Reports.Id == reportsId && r.FormNum_DB == formNum)
            .Select(r => new ReportListStub
            {
                Id = r.Id,
                FormNum = r.FormNum_DB,
                StartPeriod = r.StartPeriod_DB,
                EndPeriod = r.EndPeriod_DB,
                Year = r.Year_DB,
                CorrectionNumber = r.CorrectionNumber_DB
            })
            .ToList();
    }

    /// <summary>
    /// Оболочки Report только с полями списка (периоды/год) — без обращения к БД за полными сущностями.
    /// </summary>
    public static List<Report> CreateReportShellsFromStubs(
        IReadOnlyList<ReportListStub> stubs,
        string? formNumWhiteList,
        bool orderByYear,
        Report? preferSameInstance = null)
    {
        var orderedIds = GetOrderedReportIds(stubs, formNumWhiteList, orderByYear);
        var byId = stubs.ToDictionary(s => s.Id);
        var shells = new List<Report>(orderedIds.Count);
        foreach (var id in orderedIds)
        {
            if (preferSameInstance != null && preferSameInstance.Id == id)
            {
                shells.Add(preferSameInstance);
                continue;
            }

            if (!byId.TryGetValue(id, out var stub))
                continue;

            shells.Add(new Report
            {
                Id = stub.Id,
                FormNum_DB = stub.FormNum,
                StartPeriod_DB = stub.StartPeriod,
                EndPeriod_DB = stub.EndPeriod,
                Year_DB = stub.Year,
                CorrectionNumber_DB = stub.CorrectionNumber
            });
        }

        return shells;
    }

    public static List<int> GetOrderedReportIds(
        IReadOnlyList<ReportListStub> stubs,
        string? formNumWhiteList,
        bool orderByYear = false)
    {
        IEnumerable<ReportListStub> filtered = stubs;
        if (!string.IsNullOrWhiteSpace(formNumWhiteList))
            filtered = stubs.Where(r => r.FormNum == formNumWhiteList);

        IOrderedEnumerable<ReportListStub> ordered = filtered
            .OrderBy(r =>
            {
                var parts = r.FormNum.Split('.');
                return parts.Length > 1 && int.TryParse(parts[1], out var n) ? n : int.MinValue;
            });

        ordered = orderByYear
            ? ordered
                .ThenByDescending(r => r.Year ?? int.MaxValue)
                .ThenBy(r => r.CorrectionNumber)
            : ordered
                .ThenByDescending(r => ParseDateOrMax(r.StartPeriod))
                .ThenByDescending(r => ParseDateOrMax(r.EndPeriod))
                .ThenBy(r => r.CorrectionNumber);

        return ordered.Select(r => r.Id).ToList();
    }

    public static List<Report> LoadReportsByIds(DBModel db, IReadOnlyList<int> orderedIds)
    {
        if (orderedIds.Count == 0) return [];

        var idList = orderedIds as List<int> ?? orderedIds.ToList();
        var loaded = FirebirdInClause.QueryAll(idList, batch =>
            db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(r => batch.Contains(r.Id))
                .ToList());

        var byId = loaded.ToDictionary(r => r.Id);
        return idList
            .Where(id => byId.ContainsKey(id))
            .Select(id => byId[id])
            .ToList();
    }

    public static List<Report> GetReportPage(
        DBModel db,
        int reportsId,
        string? formNumWhiteList,
        int page,
        int pageSize,
        bool orderByYear = false)
    {
        var (safePage, safePageSize, skip) = PagingHelper.Normalize(page, pageSize);
        var stubs = LoadReportStubs(db, reportsId);
        var orderedIds = GetOrderedReportIds(stubs, formNumWhiteList, orderByYear)
            .Skip(skip)
            .Take(safePageSize)
            .ToList();
        return LoadReportsByIds(db, orderedIds);
    }

    private static List<int> GetFilteredOrgIdsForm40(DBModel db, string? searchText) =>
        FilterOrgKeysForm40(LoadOrgKeysForm40(db), searchText)
            .OrderBy(k => k.SubjectRf)
            .Select(k => k.Id)
            .ToList();

    private static List<int> GetFilteredOrgIdsForm50(DBModel db, string? searchText) =>
        FilterOrgKeysForm50(LoadOrgKeysForm50(db), searchText)
            .Select(k => k.Id)
            .ToList();

    private static List<OrgKey> LoadOrgKeysForm40(DBModel db)
    {
        lock (OrgKeysForm40Lock)
        {
            if (_cachedOrgKeysForm40 != null)
                return _cachedOrgKeysForm40;
        }

        var loaded = db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "4.0")
            .Select(x => new OrgKey
            {
                Id = x.Id,
                SubjectRf = x.Master_DB.Rows40
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => r.CodeSubjectRF_DB)
                    .FirstOrDefault() ?? "",
                RegNo = x.Master_DB.Rows40
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => r.SubjectRF_DB)
                    .FirstOrDefault() ?? "",
                ShortJurLico = x.Master_DB.Rows40
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => r.ShortNameOrganUprav_DB)
                    .FirstOrDefault() ?? ""
            })
            .ToList();

        lock (OrgKeysForm40Lock)
            _cachedOrgKeysForm40 = loaded;

        return loaded;
    }

    private static List<OrgKey> LoadOrgKeysForm50(DBModel db)
    {
        lock (OrgKeysForm50Lock)
        {
            if (_cachedOrgKeysForm50 != null)
                return _cachedOrgKeysForm50;
        }

        var loaded = db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "5.0")
            .Select(x => new OrgKey
            {
                Id = x.Id,
                ShortJurLico = x.Master_DB.Rows50
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => r.ShortName_DB)
                    .FirstOrDefault() ?? "",
                Name50 = x.Master_DB.Rows50
                    .OrderBy(r => r.NumberInOrder_DB)
                    .Select(r => r.Name_DB)
                    .FirstOrDefault() ?? ""
            })
            .ToList();

        lock (OrgKeysForm50Lock)
            _cachedOrgKeysForm50 = loaded;

        return loaded;
    }

    internal static IEnumerable<OrgKey> FilterOrgKeysForm40ForTest(IEnumerable<OrgKey> keys, string? searchText) =>
        FilterOrgKeysForm40(keys, searchText);

    internal static IEnumerable<OrgKey> FilterOrgKeysForm50ForTest(IEnumerable<OrgKey> keys, string? searchText) =>
        FilterOrgKeysForm50(keys, searchText);

    private static IEnumerable<OrgKey> FilterOrgKeysForm40(IEnumerable<OrgKey> keys, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return keys;

        var s = searchText.Trim();
        return keys.Where(k =>
            k.SubjectRf.Contains(s, StringComparison.CurrentCultureIgnoreCase)
            || k.RegNo.Contains(s, StringComparison.CurrentCultureIgnoreCase)
            || k.ShortJurLico.Contains(s, StringComparison.CurrentCultureIgnoreCase));
    }

    private static IEnumerable<OrgKey> FilterOrgKeysForm50(IEnumerable<OrgKey> keys, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return keys;

        var s = searchText.Trim();
        return keys.Where(k =>
            (!string.IsNullOrEmpty(k.ShortJurLico) && k.ShortJurLico.Contains(s, StringComparison.CurrentCultureIgnoreCase))
            || (string.IsNullOrEmpty(k.ShortJurLico) && k.Name50.Contains(s, StringComparison.CurrentCultureIgnoreCase)));
    }

    /// <summary>
    /// Лёгкие поля титула 1.0 (RegNo/Okpo/Short) для всех org — без полного preload form_10.
    /// </summary>
    public static IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> GetForm10DisplayKeys(DBModel db)
    {
        return LoadOrgKeysForm12(db, "1.0")
            .ToDictionary(
                k => k.Id,
                k => new Form10TitleSelector.TitleFields(k.RegNo, k.Okpo, k.ShortJurLico));
    }

    /// <summary>
    /// Лёгкие поля титула 2.0 (RegNo/Okpo/Short) для всех org — без полного preload form_20.
    /// </summary>
    public static IReadOnlyDictionary<int, Form10TitleSelector.TitleFields> GetForm20DisplayKeys(DBModel db)
    {
        return LoadOrgKeysForm12(db, "2.0")
            .ToDictionary(
                k => k.Id,
                k => new Form10TitleSelector.TitleFields(k.RegNo, k.Okpo, k.ShortJurLico));
    }

    private static List<OrgKey> LoadOrgKeysForm12(DBModel db, string masterFormNum)
    {
        if (masterFormNum == "1.0")
        {
            lock (OrgKeysForm10Lock)
            {
                if (_cachedOrgKeysForm10 != null)
                    return _cachedOrgKeysForm10;
            }

            var loaded = db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "1.0")
                .Select(x => new
                {
                    x.Id,
                    Rows = x.Master_DB.Rows10
                        .OrderBy(r => r.NumberInOrder_DB)
                        .Select(r => new { r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB })
                        .ToList()
                })
                .AsEnumerable()
                .Select(x => ToOrgKeyFromTitleRows(x.Id, x.Rows.Select(r => (r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB)).ToList()))
                .ToList();

            lock (OrgKeysForm10Lock)
                _cachedOrgKeysForm10 = loaded;

            return loaded;
        }

        if (masterFormNum == "2.0")
        {
            lock (OrgKeysForm20Lock)
            {
                if (_cachedOrgKeysForm20 != null)
                    return _cachedOrgKeysForm20;
            }

            var loaded20 = db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == "2.0")
                .Select(x => new
                {
                    x.Id,
                    Rows = x.Master_DB.Rows20
                        .OrderBy(r => r.NumberInOrder_DB)
                        .Select(r => new { r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB })
                        .ToList()
                })
                .AsEnumerable()
                .Select(x => ToOrgKeyFromTitleRows(x.Id, x.Rows.Select(r => (r.RegNo_DB, r.Okpo_DB, r.ShortJurLico_DB)).ToList()))
                .ToList();

            lock (OrgKeysForm20Lock)
                _cachedOrgKeysForm20 = loaded20;

            return loaded20;
        }

        return [];
    }

    private static OrgKey ToOrgKeyFromTitleRows(
        int id, List<(string RegNo, string Okpo, string Short)> rows)
    {
        var row0 = rows.ElementAtOrDefault(0);
        var row1 = rows.ElementAtOrDefault(1);
        var title = Form10TitleSelector.PickFromOrderedRows(
            (row0.RegNo, row0.Okpo, row0.Short),
            (row1.RegNo, row1.Okpo, row1.Short));
        return new OrgKey
        {
            Id = id,
            RegNo = title.RegNo,
            Okpo = title.Okpo,
            ShortJurLico = title.ShortJurLico
        };
    }

    private static IEnumerable<OrgKey> FilterOrgKeys(IEnumerable<OrgKey> keys, string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return keys;
        var s = searchText.Trim();
        return keys.Where(k =>
            k.RegNo.Contains(s, StringComparison.CurrentCultureIgnoreCase)
            || k.Okpo.Contains(s, StringComparison.CurrentCultureIgnoreCase)
            || k.ShortJurLico.Contains(s, StringComparison.CurrentCultureIgnoreCase));
    }

    private static List<Reports> LoadOrgsByIds(DBModel db, List<int> pageIds, string masterFormNum)
    {
        if (pageIds.Count == 0) return [];

        var loaded = FirebirdInClause.QueryAll(pageIds, batch =>
        {
            IQueryable<Reports> q = db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(x => batch.Contains(x.Id));

            q = masterFormNum switch
            {
                "1.0" => q.Include(x => x.Master_DB).ThenInclude(m => m.Rows10),
                "2.0" => q.Include(x => x.Master_DB).ThenInclude(m => m.Rows20),
                "4.0" => q.Include(x => x.Master_DB).ThenInclude(m => m.Rows40),
                "5.0" => q.Include(x => x.Master_DB).ThenInclude(m => m.Rows50),
                _ => q.Include(x => x.Master_DB)
            };

            return q.ToList();
        });

        var byId = loaded.ToDictionary(r => r.Id);
        return pageIds
            .Where(id => byId.ContainsKey(id))
            .Select(id => byId[id])
            .ToList();
    }

    private static DateOnly ParseDateOrMax(string? value) =>
        value != null && DateOnly.TryParse(value, out var d) ? d : DateOnly.MaxValue;
}
