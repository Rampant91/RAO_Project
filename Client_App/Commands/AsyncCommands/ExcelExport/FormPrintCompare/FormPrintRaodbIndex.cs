using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal static partial class FormPrintRaodbIndex
{
    public sealed class LoadResult
    {
        public required List<CompareReportDto> Reports { get; init; }
        public required string RegNo { get; init; }
        public required string Okpo { get; init; }
    }

    /// <summary>
    /// Лимит Firebird для IN (...); запас ниже 1500.
    /// </summary>
    private const int FirebirdInListMaxCount = 1000;

    /// <param name="orgFilter">
    /// Если задан — грузим только организации с такими (RegNo, Okpo).
    /// Пустой список — сразу пустой результат. null — все организации с формами 1/2.
    /// </param>
    /// <param name="periodHintsByOrg">
    /// Подсказки периодов из файлов сверки (ключ «RegNo|Okpo»).
    /// Если заданы — грузим только отчёты с тем же PeriodKey или пересекающимся периодом той же формы.
    /// </param>
    public static async Task<LoadResult> LoadAsync(
        string dbPath,
        CancellationToken cancellationToken,
        string? sourceLabel = null,
        Action<int, string>? progress = null,
        int progressStartPercent = 0,
        int progressSpanPercent = 100,
        IReadOnlyCollection<(string RegNo, string Okpo)>? orgFilter = null,
        IReadOnlyDictionary<string, IReadOnlyList<ComparePeriodHint>>? periodHintsByOrg = null)
    {
        await using var db = new DBModel(dbPath);
        await db.MigrateDatabaseAsync(cancellationToken);

        const int orgChunkSize = 50;

        var supportedForms = FormPrintCompareSchema.SupportedForms.ToList();

        var supportedReportsQuery = db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => supportedForms.Contains(r.FormNum_DB));

        List<int> orgIds;
        if (orgFilter is not null)
        {
            if (orgFilter.Count == 0)
            {
                progress?.Invoke(progressStartPercent, $"{sourceLabel ?? "БД"}: фильтр организаций пуст — загрузка не нужна");
                return new LoadResult { Reports = [], RegNo = "", Okpo = "" };
            }

            var resolveSpan = Math.Max(1, progressSpanPercent / 3);
            orgIds = await ResolveOrgIdsByFilterAsync(
                db,
                orgFilter,
                progress,
                progressStartPercent,
                resolveSpan,
                cancellationToken);
        }
        else
        {
            orgIds = await supportedReportsQuery
                .Where(r => r.Reports != null)
                .Select(r => r.Reports.Id)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        var totalOrgs = orgIds.Count;
        var processedOrgs = 0;

        var reports = new List<CompareReportDto>();
        var regNo = "";
        var okpo = "";

        progress?.Invoke(
            progressStartPercent,
            orgFilter is null
                ? $"{sourceLabel ?? "БД"}: найдено организаций для формы 1/2 — {totalOrgs}"
                : $"{sourceLabel ?? "БД"}: к загрузке {totalOrgs} орг. (из {orgFilter.Count} ключей файлов)");

        if (totalOrgs == 0)
        {
            return new LoadResult { Reports = [], RegNo = "", Okpo = "" };
        }

        var orgProgressStart = orgFilter is null
            ? progressStartPercent
            : progressStartPercent + Math.Max(1, progressSpanPercent / 3);
        var orgProgressSpan = orgFilter is null
            ? progressSpanPercent
            : Math.Max(1, progressSpanPercent - Math.Max(1, progressSpanPercent / 3));

        foreach (var orgChunk in ChunkIds(orgIds, orgChunkSize))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var orgMasters = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(o => orgChunk.Contains(o.Id))
                .Include(o => o.Master_DB)
                .ThenInclude(m => m!.Rows10)
                .Include(o => o.Master_DB)
                .ThenInclude(m => m!.Rows20)
                .ToListAsync(cancellationToken);

            var identityByOrgId = new Dictionary<int, (string RegNo, string Okpo, string ShortName)>(orgMasters.Count);
            foreach (var org in orgMasters)
            {
                if (org.Master_DB is null)
                {
                    continue;
                }

                identityByOrgId[org.Id] = ReadOrgIdentity(org.Master_DB);
            }

            // OrgId берём в Select до материализации: после ToListAsync навигация Reports не заполнена.
            var reportsChunk = await supportedReportsQuery
                .Where(r => r.Reports != null && orgChunk.Contains(r.Reports.Id))
                .Select(r => new { OrgId = r.Reports.Id, Report = r })
                .ToListAsync(cancellationToken);

            var reportsByOrgId = reportsChunk
                .GroupBy(x => x.OrgId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Report).ToList());

            foreach (var orgId in orgChunk)
            {
                cancellationToken.ThrowIfCancellationRequested();

                processedOrgs++;
                var orgPercent = orgProgressStart
                                  + (orgProgressSpan * processedOrgs) / Math.Max(1, totalOrgs);
                orgPercent = Math.Min(orgProgressStart + orgProgressSpan, orgPercent);

                if (!identityByOrgId.TryGetValue(orgId, out var identity))
                {
                    progress?.Invoke(orgPercent, $"{sourceLabel ?? "БД"}: организация {processedOrgs}/{totalOrgs} (нет master-identity)");
                    continue;
                }

                var (orgReg, orgOkpo, orgShort) = identity;
                if (string.IsNullOrEmpty(regNo) && (!string.IsNullOrEmpty(orgReg) || !string.IsNullOrEmpty(orgOkpo)))
                {
                    regNo = orgReg;
                    okpo = orgOkpo;
                }

                if (!reportsByOrgId.TryGetValue(orgId, out var orgReports))
                {
                    progress?.Invoke(orgPercent, $"{sourceLabel ?? "БД"}: организация {processedOrgs}/{totalOrgs} (нет отчётов)");
                    continue;
                }

                progress?.Invoke(
                    orgPercent,
                    $"{sourceLabel ?? "БД"}: организация {processedOrgs}/{totalOrgs} | {orgReg}/{orgOkpo} | отчётов: {orgReports.Count}");

                var filteredReports = orgReports.OfType<Report>().ToList();
                if (periodHintsByOrg is not null
                    && periodHintsByOrg.TryGetValue(BuildOrgKey(orgReg, orgOkpo), out var hints)
                    && hints.Count > 0)
                {
                    filteredReports = filteredReports
                        .Where(r => IsNeededForCompareHints(r, hints))
                        .ToList();
                }

                foreach (var report in filteredReports)
                {
                    var dto = await LoadReportAsync(
                        db, report, orgReg, orgOkpo, orgShort, sourceLabel, cancellationToken);
                    if (dto is not null)
                    {
                        reports.Add(dto);
                    }
                }
            }
        }

        return new LoadResult
        {
            Reports = DeduplicateByKeyKeepMaxCorrection(reports),
            RegNo = regNo,
            Okpo = okpo
        };
    }

    /// <summary>
    /// Находит Id карточек организаций (ReportsCollection) по парам RegNo/Okpo из файлов сверки.
    /// </summary>
    private static async Task<List<int>> ResolveOrgIdsByFilterAsync(
        DBModel db,
        IReadOnlyCollection<(string RegNo, string Okpo)> orgFilter,
        Action<int, string>? progress,
        int progressStartPercent,
        int progressSpanPercent,
        CancellationToken cancellationToken)
    {
        var wanted = orgFilter
            .Select(o => (
                RegNo: ReportMatchKey.NormalizeOrg(o.RegNo),
                Okpo: ReportMatchKey.NormalizeOrg(o.Okpo)))
            .Where(o => o.RegNo.Length > 0 || o.Okpo.Length > 0)
            .ToHashSet();

        if (wanted.Count == 0)
        {
            return [];
        }

        progress?.Invoke(progressStartPercent, $"поиск в БД: {wanted.Count} орг. из файлов сверки…");

        var okpoVariants = BuildLookupVariants(orgFilter.Select(o => o.Okpo));
        var regVariants = BuildLookupVariants(orgFilter.Select(o => o.RegNo));

        var masterIds = new HashSet<int>();
        var okpoChunks = ChunkStrings(okpoVariants).ToList();
        for (var i = 0; i < okpoChunks.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var chunk = okpoChunks[i];
            var from10 = await db.form_10.AsNoTracking()
                .Where(f => f.ReportId != null && f.Okpo_DB != null && chunk.Contains(f.Okpo_DB))
                .Select(f => f.ReportId!.Value)
                .ToListAsync(cancellationToken);
            masterIds.UnionWith(from10);

            var from20 = await db.form_20.AsNoTracking()
                .Where(f => f.ReportId != null && f.Okpo_DB != null && chunk.Contains(f.Okpo_DB))
                .Select(f => f.ReportId!.Value)
                .ToListAsync(cancellationToken);
            masterIds.UnionWith(from20);

            var percent = progressStartPercent + (Math.Max(1, progressSpanPercent / 3) * (i + 1)) / Math.Max(1, okpoChunks.Count);
            progress?.Invoke(percent, $"поиск в БД: ОКПО, пакет {i + 1}/{okpoChunks.Count}");
        }

        var regChunks = ChunkStrings(regVariants).ToList();
        for (var i = 0; i < regChunks.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var chunk = regChunks[i];
            var from10 = await db.form_10.AsNoTracking()
                .Where(f => f.ReportId != null && f.RegNo_DB != null && chunk.Contains(f.RegNo_DB))
                .Select(f => f.ReportId!.Value)
                .ToListAsync(cancellationToken);
            masterIds.UnionWith(from10);

            var from20 = await db.form_20.AsNoTracking()
                .Where(f => f.ReportId != null && f.RegNo_DB != null && chunk.Contains(f.RegNo_DB))
                .Select(f => f.ReportId!.Value)
                .ToListAsync(cancellationToken);
            masterIds.UnionWith(from20);

            var basePercent = progressStartPercent + Math.Max(1, progressSpanPercent / 3);
            var percent = basePercent + (Math.Max(1, progressSpanPercent / 3) * (i + 1)) / Math.Max(1, regChunks.Count);
            progress?.Invoke(percent, $"поиск в БД: рег.№, пакет {i + 1}/{regChunks.Count}");
        }

        if (masterIds.Count == 0)
        {
            return [];
        }

        progress?.Invoke(
            progressStartPercent + (2 * Math.Max(1, progressSpanPercent / 3)),
            $"поиск в БД: проверка {masterIds.Count} master-карточек…");

        var matchedOrgIds = new HashSet<int>();
        var masterList = masterIds.ToList();
        var masterChunks = ChunkIds(masterList, FirebirdInListMaxCount).ToList();
        for (var i = 0; i < masterChunks.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var idChunk = masterChunks[i];
            var orgs = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(o => o.Master_DBId != null && idChunk.Contains(o.Master_DBId.Value))
                .Include(o => o.Master_DB)
                .ThenInclude(m => m!.Rows10)
                .Include(o => o.Master_DB)
                .ThenInclude(m => m!.Rows20)
                .ToListAsync(cancellationToken);

            foreach (var org in orgs)
            {
                if (org.Master_DB is null)
                {
                    continue;
                }

                var (reg, okpo, _) = ReadOrgIdentity(org.Master_DB);
                var key = (ReportMatchKey.NormalizeOrg(reg), ReportMatchKey.NormalizeOrg(okpo));
                if (wanted.Contains(key))
                {
                    matchedOrgIds.Add(org.Id);
                }
            }

            var basePercent = progressStartPercent + (2 * Math.Max(1, progressSpanPercent / 3));
            var tailSpan = Math.Max(1, progressSpanPercent - (2 * Math.Max(1, progressSpanPercent / 3)));
            var percent = basePercent + (tailSpan * (i + 1)) / Math.Max(1, masterChunks.Count);
            progress?.Invoke(percent, $"поиск в БД: master-карточки {i + 1}/{masterChunks.Count}");
        }

        return matchedOrgIds.ToList();
    }

    private static List<string> BuildLookupVariants(IEnumerable<string?> values)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            var trimmed = value.Trim();
            set.Add(trimmed);
            set.Add(trimmed.ToUpperInvariant());
            set.Add(trimmed.ToLowerInvariant());
        }

        return set.ToList();
    }

    private static string BuildOrgKey(string? regNo, string? okpo) =>
        $"{ReportMatchKey.NormalizeOrg(regNo)}|{ReportMatchKey.NormalizeOrg(okpo)}";

    /// <summary>
    /// Нужен ли отчёт БД для сверки: точный период из файла или пересечение с ним (та же форма).
    /// </summary>
    internal static bool IsNeededForCompareHints(Report report, IReadOnlyList<ComparePeriodHint> hints) =>
        IsNeededForCompareHints(
            report.FormNum_DB ?? "",
            report.StartPeriod_DB,
            report.EndPeriod_DB,
            report.Year_DB,
            hints);

    internal static bool IsNeededForCompareHints(
        string formNum,
        string? startPeriod,
        string? endPeriod,
        string? year,
        IReadOnlyList<ComparePeriodHint> hints)
    {
        var periodKey = FormPrintCompareNormalize.BuildPeriodKey(formNum, startPeriod, endPeriod, year);

        foreach (var hint in hints)
        {
            if (!string.Equals(formNum, hint.FormNum, StringComparison.Ordinal))
            {
                continue;
            }

            if (string.Equals(periodKey, hint.PeriodKey, StringComparison.Ordinal))
            {
                return true;
            }

            if (FormPrintCompareNormalize.PeriodsOverlap(
                    formNum,
                    startPeriod,
                    endPeriod,
                    year,
                    hint.StartPeriod,
                    hint.EndPeriod,
                    hint.Year))
            {
                return true;
            }
        }

        return false;
    }
    private static IEnumerable<List<string>> ChunkStrings(IReadOnlyList<string> values)
    {
        for (var offset = 0; offset < values.Count; offset += FirebirdInListMaxCount)
        {
            yield return values.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids, int chunkSize)
    {
        if (chunkSize <= 0)
        {
            chunkSize = FirebirdInListMaxCount;
        }

        for (var offset = 0; offset < ids.Count; offset += chunkSize)
        {
            yield return ids
                .Skip(offset)
                .Take(chunkSize)
                .ToList();
        }
    }

    public static Dictionary<ReportMatchKey, CompareReportDto> ToIndex(IEnumerable<CompareReportDto> reports)
    {
        var index = new Dictionary<ReportMatchKey, CompareReportDto>();
        foreach (var report in reports)
        {
            var key = ReportMatchKey.From(report);
            if (!index.TryGetValue(key, out var existing)
                || report.CorrectionNumber > existing.CorrectionNumber)
            {
                index[key] = report;
            }
        }

        return index;
    }

    internal static List<CompareReportDto> DeduplicateByKeyKeepMaxCorrection(List<CompareReportDto> reports) =>
        reports
            .GroupBy(r => ReportMatchKey.From(r))
            .Select(g => g.OrderByDescending(x => x.CorrectionNumber).First())
            .OrderBy(r => r.FormNum)
            .ThenBy(r => r.PeriodKey)
            .ToList();

    internal static (string RegNo, string Okpo, string ShortName) ReadOrgIdentity(Report master)
    {
        if (master.FormNum_DB == "1.0" && master.Rows10 is { Count: > 0 })
        {
            var row = PickTitleRow10(master.Rows10);
            return (row.RegNo_DB ?? "", row.Okpo_DB ?? "", row.ShortJurLico_DB ?? "");
        }

        if (master.FormNum_DB == "2.0" && master.Rows20 is { Count: > 0 })
        {
            var row = PickTitleRow20(master.Rows20);
            return (row.RegNo_DB ?? "", row.Okpo_DB ?? "", row.ShortJurLico_DB ?? "");
        }

        return ("", "", "");
    }

    private static Form10 PickTitleRow10(IList<Form10> rows)
    {
        if (rows.Count > 1
            && ((!string.IsNullOrEmpty(rows[1].RegNo_DB) || rows[1].Okpo_DB == "-")
                && !string.IsNullOrEmpty(rows[1].Okpo_DB)))
        {
            return rows[1];
        }

        return rows[0];
    }

    private static Form20 PickTitleRow20(IList<Form20> rows)
    {
        if (rows.Count > 1
            && ((!string.IsNullOrEmpty(rows[1].RegNo_DB) || rows[1].Okpo_DB == "-")
                && !string.IsNullOrEmpty(rows[1].Okpo_DB)))
        {
            return rows[1];
        }

        return rows[0];
    }

    private static async Task<CompareReportDto?> LoadReportAsync(
        DBModel db,
        Report report,
        string regNo,
        string okpo,
        string orgShortName,
        string? sourceLabel,
        CancellationToken cancellationToken)
    {
        var columns = FormPrintCompareSchema.ColumnsFor(report.FormNum_DB);
        if (columns.Length == 0)
        {
            return null;
        }

        var rows = await LoadRowsAsync(db, report.FormNum_DB, report.Id, columns, cancellationToken);
        if (rows is null)
        {
            return null;
        }

        return new CompareReportDto
        {
            ReportId = report.Id,
            FormNum = report.FormNum_DB,
            PeriodKey = FormPrintCompareNormalize.BuildPeriodKey(
                report.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, report.Year_DB),
            PeriodDisplay = FormPrintCompareNormalize.BuildPeriodDisplay(
                report.FormNum_DB, report.StartPeriod_DB, report.EndPeriod_DB, report.Year_DB),
            StartPeriod = report.StartPeriod_DB ?? "",
            EndPeriod = report.EndPeriod_DB ?? "",
            Year = report.Year_DB ?? "",
            CorrectionNumber = report.CorrectionNumber_DB,
            RegNo = regNo,
            Okpo = okpo,
            OrgShortName = orgShortName,
            SourceLabel = sourceLabel ?? "",
            Rows = rows,
            Columns = columns
        };
    }

    private static async Task<List<CompareRowDto>?> LoadRowsAsync(
        DBModel db,
        string formNum,
        int reportId,
        CompareColumn[] columns,
        CancellationToken ct) =>
        formNum switch
        {
            "1.1" => await LoadRows11Async(db, reportId, columns, ct),
            "1.2" => await LoadRows12Async(db, reportId, columns, ct),
            "1.3" => await LoadRows13Async(db, reportId, columns, ct),
            "1.4" => await LoadRows14Async(db, reportId, columns, ct),
            "1.5" => await LoadRows15Async(db, reportId, columns, ct),
            "1.6" => await LoadRows16Async(db, reportId, columns, ct),
            "1.7" => await LoadRows17Async(db, reportId, columns, ct),
            "1.8" => await LoadRows18Async(db, reportId, columns, ct),
            "1.9" => await LoadRows19Async(db, reportId, columns, ct),
            "2.1" => await LoadRows21Async(db, reportId, columns, ct),
            "2.2" => await LoadRows22Async(db, reportId, columns, ct),
            "2.3" => await LoadRows23Async(db, reportId, columns, ct),
            "2.4" => await LoadRows24Async(db, reportId, columns, ct),
            "2.5" => await LoadRows25Async(db, reportId, columns, ct),
            "2.6" => await LoadRows26Async(db, reportId, columns, ct),
            "2.7" => await LoadRows27Async(db, reportId, columns, ct),
            "2.8" => await LoadRows28Async(db, reportId, columns, ct),
            "2.9" => await LoadRows29Async(db, reportId, columns, ct),
            "2.10" => await LoadRows210Async(db, reportId, columns, ct),
            "2.11" => await LoadRows211Async(db, reportId, columns, ct),
            "2.12" => await LoadRows212Async(db, reportId, columns, ct),
            _ => null
        };

    private static async Task<List<CompareRowDto>> LoadRows11Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_11.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, i) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                f.NumberInOrder_DB.ToString(),
                f.OperationCode_DB ?? "",
                f.OperationDate_DB ?? "",
                f.PassportNumber_DB ?? "",
                f.Type_DB ?? "",
                f.Radionuclids_DB ?? "",
                f.FactoryNumber_DB ?? "",
                f.Quantity_DB?.ToString() ?? "",
                f.Activity_DB ?? "",
                f.CreatorOKPO_DB ?? "",
                f.CreationDate_DB ?? "",
                f.Category_DB?.ToString() ?? "",
                f.SignedServicePeriod_DB?.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? "",
                f.PropertyCode_DB?.ToString() ?? "",
                f.Owner_DB ?? "",
                f.DocumentVid_DB?.ToString() ?? "",
                f.DocumentNumber_DB ?? "",
                f.DocumentDate_DB ?? "",
                f.ProviderOrRecieverOKPO_DB ?? "",
                f.TransporterOKPO_DB ?? "",
                f.PackName_DB ?? "",
                f.PackType_DB ?? "",
                f.PackNumber_DB ?? ""
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows13Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_13.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                f.NumberInOrder_DB.ToString(),
                f.OperationCode_DB ?? "",
                f.OperationDate_DB ?? "",
                f.PassportNumber_DB ?? "",
                f.Type_DB ?? "",
                f.Radionuclids_DB ?? "",
                f.FactoryNumber_DB ?? "",
                f.Activity_DB ?? "",
                f.CreatorOKPO_DB ?? "",
                f.CreationDate_DB ?? "",
                f.AggregateState_DB?.ToString() ?? "",
                f.PropertyCode_DB?.ToString() ?? "",
                f.Owner_DB ?? "",
                f.DocumentVid_DB?.ToString() ?? "",
                f.DocumentNumber_DB ?? "",
                f.DocumentDate_DB ?? "",
                f.ProviderOrRecieverOKPO_DB ?? "",
                f.TransporterOKPO_DB ?? "",
                f.PackName_DB ?? "",
                f.PackType_DB ?? "",
                f.PackNumber_DB ?? ""
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows14Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_14.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            f.OperationDate_DB,
            new[]
            {
                f.NumberInOrder_DB.ToString(),
                f.OperationCode_DB ?? "",
                f.OperationDate_DB ?? "",
                f.PassportNumber_DB ?? "",
                f.Name_DB ?? "",
                f.Sort_DB?.ToString() ?? "",
                f.Radionuclids_DB ?? "",
                f.Activity_DB ?? "",
                f.ActivityMeasurementDate_DB ?? "",
                f.Volume_DB ?? "",
                f.Mass_DB ?? "",
                f.AggregateState_DB?.ToString() ?? "",
                f.PropertyCode_DB?.ToString() ?? "",
                f.Owner_DB ?? "",
                f.DocumentVid_DB?.ToString() ?? "",
                f.DocumentNumber_DB ?? "",
                f.DocumentDate_DB ?? "",
                f.ProviderOrRecieverOKPO_DB ?? "",
                f.TransporterOKPO_DB ?? "",
                f.PackName_DB ?? "",
                f.PackType_DB ?? "",
                f.PackNumber_DB ?? ""
            }
        ));
    }

    private static async Task<List<CompareRowDto>> LoadRows212Async(
        DBModel db, int reportId, CompareColumn[] columns, CancellationToken ct)
    {
        var forms = await db.form_212.AsNoTracking()
            .Where(f => f.ReportId == reportId)
            .OrderBy(f => f.NumberInOrder_DB)
            .ThenBy(f => f.Id)
            .ToListAsync(ct);

        return MapRows(forms, columns, (f, _) =>
        (
            f.Id,
            f.NumberInOrder_DB,
            (string?)null,
            new[]
            {
                f.NumberInOrder_DB.ToString(),
                f.OperationCode_DB?.ToString() ?? "",
                f.ObjectTypeCode_DB?.ToString() ?? "",
                f.Radionuclids_DB ?? "",
                f.Activity_DB ?? "",
                f.ProviderOrRecieverOKPO_DB ?? ""
            }
        ));
    }

    private static string Cell(object? value) =>
        value switch
        {
            null => "",
            string s => s,
            IFormattable f => f.ToString(null, System.Globalization.CultureInfo.InvariantCulture) ?? "",
            _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? ""
        };

    private static List<CompareRowDto> MapRows<T>(
        List<T> forms,
        CompareColumn[] columns,
        Func<T, int, (int Id, int NumberInOrder, string? OpDate, string[] Values)> selector)
    {
        var result = new List<CompareRowDto>(forms.Count);
        for (var i = 0; i < forms.Count; i++)
        {
            var (id, numberInOrder, opDate, values) = selector(forms[i], i);
            result.Add(new CompareRowDto
            {
                Id = id,
                SourceIndex = i,
                NumberInOrder = numberInOrder,
                OpDate = opDate,
                Values = values,
                Fingerprint = FormPrintCompareNormalize.BuildFingerprint(columns, values)
            });
        }

        return result;
    }
}
