using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers;
using Client_App.Services.DataAccess;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing;

public partial class ExcelExportCheckTransferReceiveAsyncCommand
{
    #region Mode / export model

    private const string WholeDbParameter = "All";

    private static bool IsWholeDbMode(object? parameter) =>
        parameter is string s && string.Equals(s, WholeDbParameter, StringComparison.OrdinalIgnoreCase);

    private sealed class OrganizationTransferReceiveExport
    {
        public Dictionary<TransferReceiveFormId, List<TransferReceiveDto>> UnpairedByForm { get; init; } = new();

        public List<TransferReceiveDto> GetUnpaired(TransferReceiveFormId id) =>
            UnpairedByForm.TryGetValue(id, out var list) ? list : [];

        public bool HasAnyUnpaired => UnpairedByForm.Values.Any(list => list.Count > 0);
    }

    private sealed class TransferReceiveBulkLoad
    {
        public Dictionary<TransferReceiveFormId, List<TransferReceiveDto>> AllOpsByForm { get; init; } = new();
        public Dictionary<TransferReceiveFormId, Dictionary<int, List<TransferReceiveDto>>> OpsByRepsIdByForm { get; init; } = new();
        public Dictionary<string, List<int>> RepsIdsByNormOkpo { get; init; } = new(StringComparer.Ordinal);

        public List<TransferReceiveDto> GetAllOps(TransferReceiveFormId id) =>
            AllOpsByForm.TryGetValue(id, out var list) ? list : [];

        public Dictionary<int, List<TransferReceiveDto>> GetOpsByRepsId(TransferReceiveFormId id) =>
            OpsByRepsIdByForm.TryGetValue(id, out var map) ? map : new Dictionary<int, List<TransferReceiveDto>>();
    }

    /// <summary>Кандидат whole-DB: Id + титул без тяжёлого Include(Master→Rows10).</summary>
    private sealed class TransferReceiveOrgCandidate
    {
        public required int Id { get; init; }
        public required OrgTitleInfo Title { get; init; }
    }

    #endregion

    #region Execute selected org

    private async Task ExecuteForSelectedOrganizationAsync(
        Reports selectedReports,
        TransferReceiveParamsSet pairingParams,
        AnyTaskProgressBar progressBar,
        CancellationTokenSource cts)
    {
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        OrgMatchQuery.EnsureTitleRowsLoaded(selectedReports);
        var regNum = RemoveForbiddenChars(selectedReports.Master_DB.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports.Master_DB.OkpoRep.Value);
        var fileName = $"{regNum}_{okpo}_проверка_приёма_передачи";
        var exportName = $"{regNum}_{okpo}";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", exportName, "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);
        if (string.IsNullOrEmpty(fullPath))
        {
            return;
        }

        progressBarVM.SetProgressBar(10, "Создание временной БД", exportName, "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        void Status(int percent, string text) =>
            progressBarVM.SetProgressBar(percent, text, exportName);

        var export = await BuildOrganizationExportAsync(db, selectedReports, pairingParams, cts.Token, Status);
        if (export is null)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        Status(70, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        InitializeWorkbook(excelPackage);
        AppendOrganizationToWorkbook(excelPackage, export, progressBarVM, percentBase: 75, percentSpan: 15);
        Status(93, "Оформление таблиц Excel (фильтры, сетка)…");
        FinalizeWorkbookTables(excelPackage);

        Status(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
        await CleanupAndClose(progressBar, tmpDbPath);
    }

    #endregion

    #region Execute whole DB

    private async Task ExecuteForWholeDatabaseAsync(
        TransferReceiveParamsSet pairingParams,
        AnyTaskProgressBar progressBar,
        CancellationTokenSource cts)
    {
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        const string fileName = "проверка_приёма_передачи_вся_БД";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", "Вся БД", "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);
        if (string.IsNullOrEmpty(fullPath))
        {
            return;
        }

        progressBarVM.SetProgressBar(10, "Создание временной БД", "Вся БД", "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Загрузка операций приёма/передачи (вся БД)", "Вся БД", "Выгрузка в .xlsx");
        var enabledIds = pairingParams.EnabledFormIds;
        var (candidates, bulk) = await LoadWholeDatabaseTransferReceiveAsync(
            db,
            cts.Token,
            BindProgressToUi(progressBarVM, "Вся БД"),
            enabledIds);
        if (candidates.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar, wholeDatabase: true);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(52, "Построение общих пулов и индексов сопоставления", "Вся БД", "Выгрузка в .xlsx");
        var sharedPools = new Dictionary<TransferReceiveFormId, Dictionary<string, List<TransferReceiveDto>>>();
        var sharedIndexes = new Dictionary<TransferReceiveFormId, SharedFormSearchIndexes>();
        foreach (var formId in enabledIds)
        {
            var pool = BuildOpsPoolByOrgOkpo([], bulk.GetAllOps(formId), bulk.RepsIdsByNormOkpo);
            sharedPools[formId] = pool;
            sharedIndexes[formId] = SharedFormSearchIndexes.Build(pool, pairingParams.GetParams(formId));
        }

        progressBarVM.SetProgressBar(55, "Инициализация Excel пакета", "Вся БД", "Выгрузка в .xlsx");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        InitializeWorkbook(excelPackage);

        var anyUnpairedWritten = false;
        var total = candidates.Count;
        for (var i = 0; i < total; i++)
        {
            cts.Token.ThrowIfCancellationRequested();
            var org = candidates[i];
            var orgIndex = i + 1;
            var regNum = RemoveForbiddenChars(org.Title.RegNo);
            var okpo = RemoveForbiddenChars(org.Title.Okpo);
            var exportName = $"{regNum}_{okpo}";
            var percentBase = 55 + (int)(38.0 * i / total);
            var percentSpan = Math.Max(1, (int)(38.0 / total));

            void Status(int offsetWithinOrg, string stage) =>
                progressBarVM.SetProgressBar(
                    Math.Min(94, percentBase + offsetWithinOrg),
                    $"Организация {orgIndex} из {total}: {stage}",
                    exportName);

            Status(0, "сопоставление");
            var ourOpsByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
            foreach (var formId in enabledIds)
            {
                bulk.GetOpsByRepsId(formId).TryGetValue(org.Id, out var ourOps);
                ourOpsByForm[formId] = ourOps ?? [];
            }

            var export = BuildOrganizationExportFromSharedPools(
                org.Title.Okpo,
                ourOpsByForm,
                sharedPools,
                pairingParams,
                (p, text) => Status(Math.Min(percentSpan - 1, Math.Max(0, p / 5)), text),
                sharedIndexes);

            if (export is null)
            {
                continue;
            }

            Status(percentSpan - 1, "запись в Excel");
            AppendOrganizationToWorkbook(
                excelPackage,
                export,
                progressBarVM,
                percentBase: percentBase,
                percentSpan: percentSpan,
                orgIndex: orgIndex,
                orgCount: total,
                orgLabel: exportName);
            anyUnpairedWritten = true;
        }

        if (!anyUnpairedWritten)
        {
            await ShowNoUnpairedOperationsMessage(progressBar, wholeDatabase: true);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(93, "Оформление таблиц Excel (фильтры, сетка)…", "Вся БД", "Выгрузка в .xlsx");
        FinalizeWorkbookTables(excelPackage);
        progressBarVM.SetProgressBar(95, "Сохранение", "Вся БД", "Выгрузка в .xlsx");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
        await CleanupAndClose(progressBar, tmpDbPath);
    }

    #endregion

    #region Whole-DB load (ops → titles → candidates)

    /// <summary>
    /// Whole-DB: Distinct org по форме → пакетная загрузка ops → титулы → кандидаты.
    /// </summary>
    private static async Task<(List<TransferReceiveOrgCandidate> Candidates, TransferReceiveBulkLoad Bulk)>
        LoadWholeDatabaseTransferReceiveAsync(
            DBModel db,
            CancellationToken cancellationToken,
            Action<int, string>? reportProgress = null,
            IReadOnlySet<TransferReceiveFormId>? enabledFormIds = null)
    {
        enabledFormIds ??= ImplementedFormDescriptors.Select(d => d.Id).ToHashSet();
        var enabledDescriptors = ImplementedFormDescriptors
            .Where(d => enabledFormIds.Contains(d.Id))
            .ToList();
        if (enabledDescriptors.Count == 0)
        {
            return ([], new TransferReceiveBulkLoad());
        }

        var allOpsByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
        var formCount = enabledDescriptors.Count;
        // 12–45%: постраничная загрузка форм; 45–52%: активные / титулы / ОКПО.
        const int formsBandMin = 12;
        const int formsBandMax = 45;
        const int loadBandMax = 52;

        for (var i = 0; i < formCount; i++)
        {
            var descriptor = enabledDescriptors[i];
            var nestMin = formsBandMin + (formsBandMax - formsBandMin) * i / formCount;
            var nestMax = formsBandMin + (formsBandMax - formsBandMin) * (i + 1) / formCount;
            var formProgress = new ProgressReporter(reportProgress, nestMin, nestMax);

            // Один проход по строкам формы (keyset по Id) — без долгого Distinct «в тишине».
            var allOps = await LoadFormOpsPagedWholeDbAsync(
                descriptor.Id,
                db,
                cancellationToken,
                formProgress,
                descriptor.FormNum);
            allOpsByForm[descriptor.Id] = allOps;
            formProgress.ReportNow(
                1, 1, $"форма {descriptor.FormNum}: готово — {allOps.Count} строк");
        }

        var repsIdsFromOps = allOpsByForm.Values
            .SelectMany(ops => ops)
            .Select(op => op.RepsId)
            .Distinct()
            .OrderBy(id => id)
            .ToList();

        if (repsIdsFromOps.Count == 0)
        {
            return ([], new TransferReceiveBulkLoad());
        }

        var postProgress = new ProgressReporter(reportProgress, formsBandMax, loadBandMax);
        postProgress.Status($"фильтр активных карточек ({repsIdsFromOps.Count} орг.)…");
        var activeIds = new List<int>(repsIdsFromOps.Count);
        var idChunks = ChunkIds(repsIdsFromOps).ToList();
        for (var i = 0; i < idChunks.Count; i++)
        {
            postProgress.ReportNow(i, idChunks.Count,
                $"фильтр активных карточек {i + 1} из {idChunks.Count}");
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservable != null && idChunks[i].Contains(reps.Id))
                .Select(reps => reps.Id)
                .ToListAsync(cancellationToken);
            activeIds.AddRange(batch);
        }

        postProgress.ReportNow(1, 3, $"активных организаций: {activeIds.Count}");

        if (activeIds.Count == 0)
        {
            return ([], new TransferReceiveBulkLoad());
        }

        postProgress.Status($"загрузка титулов ({repsIdsFromOps.Count} орг.)…");
        var orgTitles = new Dictionary<int, OrgTitleInfo>();
        await LoadOrgTitlesForRepsIdsAsync(db, repsIdsFromOps, orgTitles, cancellationToken);
        postProgress.ReportNow(2, 3, $"титулы: {orgTitles.Count}");

        foreach (var ops in allOpsByForm.Values)
        {
            ApplyOrgTitlesToOps(ops, orgTitles);
        }

        var opsByRepsIdByForm = new Dictionary<TransferReceiveFormId, Dictionary<int, List<TransferReceiveDto>>>();
        foreach (var (formId, allOps) in allOpsByForm)
        {
            opsByRepsIdByForm[formId] = allOps
                .GroupBy(op => op.RepsId)
                .ToDictionary(g => g.Key, g => g.OrderBy(op => op.Id).ToList());
        }

        postProgress.Status("карта ОКПО→орг.…");
        var allOpsConcat = allOpsByForm.Values.SelectMany(ops => ops).ToList();
        var repsIdsByNormOkpo = await BuildOkpoAliasMapForWholeDbAsync(
            db, orgTitles, allOpsConcat, cancellationToken, postProgress);

        var activeSet = activeIds.ToHashSet();
        var regNoComparer = new CustomReportsComparer();
        var candidates = orgTitles
            .Where(kv => activeSet.Contains(kv.Key))
            .Select(kv => new TransferReceiveOrgCandidate { Id = kv.Key, Title = kv.Value })
            .OrderBy(c => c.Title.RegNo, regNoComparer)
            .ThenBy(c => c.Title.Okpo, regNoComparer)
            .ToList();

        var counts = string.Join(", ",
            enabledDescriptors.Select(d => $"{d.FormNum}={allOpsByForm[d.Id].Count}"));
        postProgress.ReportNow(3, 3, $"готово — {counts}, кандидатов={candidates.Count}");

        return (candidates, new TransferReceiveBulkLoad
        {
            AllOpsByForm = allOpsByForm,
            OpsByRepsIdByForm = opsByRepsIdByForm,
            RepsIdsByNormOkpo = repsIdsByNormOkpo
        });
    }

    private static void ApplyOrgTitlesToOps(
        IEnumerable<TransferReceiveDto> ops,
        IReadOnlyDictionary<int, OrgTitleInfo> orgTitles)
    {
        foreach (var op in ops)
        {
            if (!orgTitles.TryGetValue(op.RepsId, out var title))
            {
                continue;
            }

            op.OrgOkpo = title.Okpo;
            op.OrgRegNo = title.RegNo;
            op.OrgShortName = title.ShortName;
        }
    }

    #endregion

    #region Build organization export

    /// <summary>
    /// Режим выбранной организации: загрузка данных + общее ядро анализа.
    /// Режим «вся БД» вызывает <see cref="BuildOrganizationExportFromLoaded"/> после bulk-load.
    /// </summary>
    private async Task<OrganizationTransferReceiveExport?> BuildOrganizationExportAsync(
        DBModel db,
        Reports selectedReports,
        TransferReceiveParamsSet pairingParams,
        CancellationToken cancellationToken,
        Action<int, string>? reportProgress = null)
    {
        _currentParams = pairingParams;
        var ourOkpo = selectedReports.Master_DB.OkpoRep.Value?.Trim() ?? string.Empty;
        var ourRegNo = selectedReports.Master_DB.RegNoRep.Value?.Trim() ?? string.Empty;
        var ourShortName = selectedReports.Master_DB.ShortJurLicoRep.Value?.Trim() ?? string.Empty;

        var enabledForms = pairingParams.EnabledForms;
        if (enabledForms.Count == 0)
        {
            return null;
        }

        var ourOpsByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
        var formIndex = 0;
        foreach (var descriptor in enabledForms)
        {
            var pct = 15 + formIndex * 2;
            reportProgress?.Invoke(pct, $"загрузка операций {descriptor.FormNum} выбранной организации…");
            var ops = await LoadFormOpsForRepsAsync(
                descriptor.Id, db, selectedReports.Id, ourOkpo, cancellationToken);
            ourOpsByForm[descriptor.Id] = ops;
            reportProgress?.Invoke(pct + 1, $"загружено операций {descriptor.FormNum}: {ops.Count}");
            formIndex++;
        }

        if (ourOpsByForm.Values.All(ops => ops.Count == 0))
        {
            return null;
        }

        foreach (var ops in ourOpsByForm.Values)
        {
            foreach (var op in ops)
            {
                op.OrgRegNo = ourRegNo;
                op.OrgShortName = ourShortName;
                op.OrgOkpo = ourOkpo;
            }
        }

        var counterpartRawOkpos = ourOpsByForm.Values
            .SelectMany(ops => ops)
            .Select(op => op.ProviderOrRecieverOkpo?.Trim() ?? string.Empty)
            .Where(okpo => okpo.Length > 0 && okpo != "-")
            .Distinct(StringComparer.Ordinal)
            .ToList();

        var okpoProgress = new ProgressReporter(reportProgress, percentMin: 20, percentMax: 32);
        okpoProgress.Status($"поиск контрагентов по ОКПО: 0 из {counterpartRawOkpos.Count}");
        var (repsIdsByOkpo, orgTitles) =
            await LoadRepsIdsByOkpoAsync(db, counterpartRawOkpos, cancellationToken, okpoProgress);

        var counterpartRepsIds = repsIdsByOkpo.Values
            .SelectMany(ids => ids)
            .Where(id => id != selectedReports.Id)
            .Distinct()
            .ToList();

        reportProgress?.Invoke(
            33,
            $"найдено контрагентов: {counterpartRepsIds.Count} орг. (уникальных ОКПО: {counterpartRawOkpos.Count})");

        var counterpartOpsByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
        var loadSpan = Math.Max(1, 10 / Math.Max(1, enabledForms.Count));
        for (var i = 0; i < enabledForms.Count; i++)
        {
            var descriptor = enabledForms[i];
            var percentMin = 34 + i * loadSpan;
            var percentMax = Math.Min(44, percentMin + loadSpan);
            var loadProgress = new ProgressReporter(reportProgress, percentMin, percentMax);

            // Сужение SQL по кол.19 только если ОКПО участвует в сверке — иначе сломаем пары/closest.
            IReadOnlyList<string>? providerOkpoVariants = null;
            string? ourOkpoFilter = null;
            if (pairingParams.GetParams(descriptor.Id).CheckProviderOrRecieverOkpo)
            {
                providerOkpoVariants = BuildOurOkpoSqlMatchVariants(ourOkpo);
                ourOkpoFilter = ourOkpo;
            }

            counterpartOpsByForm[descriptor.Id] = await LoadFormOpsForRepsIdsAsync(
                descriptor.Id,
                db,
                counterpartRepsIds,
                orgTitles,
                cancellationToken,
                loadProgress,
                providerOkpoVariants,
                ourOkpoFilter);
        }

        var counterpartSummary = string.Join(", ",
            enabledForms.Select(d =>
                $"{d.FormNum}={counterpartOpsByForm.GetValueOrDefault(d.Id)?.Count ?? 0}"));
        reportProgress?.Invoke(
            44,
            $"загружено операций контрагентов: {counterpartSummary}, {counterpartRepsIds.Count} орг.");

        return BuildOrganizationExportFromLoaded(
            ourOkpo,
            ourOpsByForm,
            counterpartOpsByForm,
            repsIdsByOkpo,
            pairingParams,
            reportProgress);
    }

    /// <summary>
    /// Сопоставление + closest из уже загруженных DTO (org после точечной загрузки).
    /// <paramref name="counterpartOrAllOpsByForm"/> — операции контрагентов либо полный пул
    /// (включая «свои»; дубли по Id отфильтрует ядро).
    /// </summary>
    private OrganizationTransferReceiveExport? BuildOrganizationExportFromLoaded(
        string ourOkpo,
        IReadOnlyDictionary<TransferReceiveFormId, List<TransferReceiveDto>> ourOpsByForm,
        IReadOnlyDictionary<TransferReceiveFormId, List<TransferReceiveDto>> counterpartOrAllOpsByForm,
        IReadOnlyDictionary<string, List<int>> repsIdsByOkpo,
        TransferReceiveParamsSet pairingParams,
        Action<int, string>? reportProgress = null)
    {
        _currentParams = pairingParams;
        var enabledForms = pairingParams.EnabledForms;
        var unpairedByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
        _closestByForm = new Dictionary<TransferReceiveFormId, Dictionary<int, ClosestMatchResult>>();

        var formCount = Math.Max(1, enabledForms.Count);
        for (var i = 0; i < enabledForms.Count; i++)
        {
            var descriptor = enabledForms[i];
            var formOptions = pairingParams.GetParams(descriptor.Id);
            ourOpsByForm.TryGetValue(descriptor.Id, out var ourOps);
            ourOps ??= [];
            counterpartOrAllOpsByForm.TryGetValue(descriptor.Id, out var counterpartOps);
            counterpartOps ??= [];

            var matchMin = 45 + (int)(20.0 * i / formCount);
            var matchMax = 45 + (int)(20.0 * (i + 0.5) / formCount);
            var closestMax = 45 + (int)(20.0 * (i + 1) / formCount);

            if (ourOps.Count == 0)
            {
                unpairedByForm[descriptor.Id] = [];
                _closestByForm[descriptor.Id] = new Dictionary<int, ClosestMatchResult>();
                continue;
            }

            var matchProgress = new ProgressReporter(reportProgress, matchMin, matchMax);
            matchProgress.Status($"сопоставление формы {descriptor.FormNum}: 0 из {ourOps.Count} операций");
            var (unpaired, opsByOrgOkpo) = AnalyzeFormForOrganization(
                ourOps, counterpartOps, ourOkpo, formOptions, repsIdsByOkpo, matchProgress);
            unpairedByForm[descriptor.Id] = unpaired;

            reportProgress?.Invoke(matchMax,
                $"непарных операций {descriptor.FormNum}: {unpaired.Count} из {ourOps.Count}");

            var closestProgress = new ProgressReporter(reportProgress, matchMax, closestMax);
            closestProgress.Status(
                $"поиск ближайших совпадений {descriptor.FormNum}: 0 из {unpaired.Count}");
            _closestByForm[descriptor.Id] = BuildClosestMatchResults(
                unpaired, opsByOrgOkpo, formOptions, closestProgress);
        }

        if (unpairedByForm.Values.All(list => list.Count == 0))
        {
            return null;
        }

        return new OrganizationTransferReceiveExport { UnpairedByForm = unpairedByForm };
    }

    /// <summary>
    /// Whole-DB: пул ОКПО и поисковые индексы уже построены один раз; per-org только unpaired + closest.
    /// </summary>
    private OrganizationTransferReceiveExport? BuildOrganizationExportFromSharedPools(
        string ourOkpo,
        IReadOnlyDictionary<TransferReceiveFormId, List<TransferReceiveDto>> ourOpsByForm,
        IReadOnlyDictionary<TransferReceiveFormId, Dictionary<string, List<TransferReceiveDto>>> sharedPools,
        TransferReceiveParamsSet pairingParams,
        Action<int, string>? reportProgress = null,
        IReadOnlyDictionary<TransferReceiveFormId, SharedFormSearchIndexes>? sharedIndexes = null)
    {
        _currentParams = pairingParams;
        var enabledForms = pairingParams.EnabledForms;
        var unpairedByForm = new Dictionary<TransferReceiveFormId, List<TransferReceiveDto>>();
        _closestByForm = new Dictionary<TransferReceiveFormId, Dictionary<int, ClosestMatchResult>>();

        var formCount = Math.Max(1, enabledForms.Count);
        for (var i = 0; i < enabledForms.Count; i++)
        {
            var descriptor = enabledForms[i];
            var formOptions = pairingParams.GetParams(descriptor.Id);
            ourOpsByForm.TryGetValue(descriptor.Id, out var ourOps);
            ourOps ??= [];
            sharedPools.TryGetValue(descriptor.Id, out var sharedPool);
            sharedPool ??= new Dictionary<string, List<TransferReceiveDto>>(StringComparer.Ordinal);
            SharedFormSearchIndexes? formIndexes = null;
            if (sharedIndexes is not null)
            {
                sharedIndexes.TryGetValue(descriptor.Id, out formIndexes);
            }

            var matchMin = 45 + (int)(20.0 * i / formCount);
            var matchMax = 45 + (int)(20.0 * (i + 0.5) / formCount);
            var closestMax = 45 + (int)(20.0 * (i + 1) / formCount);

            if (ourOps.Count == 0)
            {
                unpairedByForm[descriptor.Id] = [];
                _closestByForm[descriptor.Id] = new Dictionary<int, ClosestMatchResult>();
                continue;
            }

            var matchProgress = new ProgressReporter(reportProgress, matchMin, matchMax);
            matchProgress.Status($"сопоставление формы {descriptor.FormNum}: 0 из {ourOps.Count} операций");
            var unpaired = ComputeUnpairedOperations(
                ourOps, sharedPool, ourOkpo, formOptions, matchProgress, formIndexes?.Pairing);
            unpairedByForm[descriptor.Id] = unpaired;

            reportProgress?.Invoke(matchMax,
                $"непарных операций {descriptor.FormNum}: {unpaired.Count} из {ourOps.Count}");

            var closestProgress = new ProgressReporter(reportProgress, matchMax, closestMax);
            closestProgress.Status(
                $"поиск ближайших совпадений {descriptor.FormNum}: 0 из {unpaired.Count}");
            _closestByForm[descriptor.Id] = BuildClosestMatchResults(
                unpaired,
                sharedPool,
                formOptions,
                closestProgress,
                formIndexes?.Closest,
                formIndexes?.Norms);
        }

        if (unpairedByForm.Values.All(list => list.Count == 0))
        {
            return null;
        }

        return new OrganizationTransferReceiveExport { UnpairedByForm = unpairedByForm };
    }

    #endregion

    #region Shared analysis core (org + whole-DB)

    /// <summary>
    /// Индексы поиска, общие для всех org в режиме «вся БД» (immutable после Build).
    /// </summary>
    private sealed class SharedFormSearchIndexes
    {
        public required PairingCandidateIndex Pairing { get; init; }
        public required ClosestCandidateIndex Closest { get; init; }
        public required System.Collections.Concurrent.ConcurrentDictionary<int, TransferReceiveNorm> Norms { get; init; }

        public static SharedFormSearchIndexes Build(
            IReadOnlyDictionary<string, List<TransferReceiveDto>> pool,
            TransferReceiveFormParams options) =>
            new()
            {
                Pairing = PairingCandidateIndex.Build(pool, options),
                Closest = ClosestCandidateIndex.Build(pool, options.CheckOperationDate),
                Norms = BuildClosestNormCache([], pool)
            };
    }

    /// <summary>
    /// Единое ядро: пул кандидатов + непарные. Без обращений к БД.
    /// Org-режим и whole-DB отличаются только тем, откуда взяты <paramref name="ourOps"/> /
    /// <paramref name="counterpartOps"/> (точечная загрузка vs bulk).
    /// </summary>
    private static (List<TransferReceiveDto> Unpaired, Dictionary<string, List<TransferReceiveDto>> OpsByOrgOkpo)
        AnalyzeFormForOrganization(
            List<TransferReceiveDto> ourOps,
            List<TransferReceiveDto> counterpartOps,
            string ourOkpoRaw,
            TransferReceiveFormParams options,
            IReadOnlyDictionary<string, List<int>>? repsIdsByNormOkpo = null,
            ProgressReporter? progress = null)
    {
        var opsByOrgOkpo = BuildOpsPoolByOrgOkpo(ourOps, counterpartOps, repsIdsByNormOkpo);
        var unpaired = ComputeUnpairedOperations(ourOps, opsByOrgOkpo, ourOkpoRaw, options, progress);
        return (unpaired, opsByOrgOkpo);
    }

    /// <summary>Устаревший алиас — то же, что <see cref="AnalyzeFormForOrganization"/>.</summary>
    private static (List<TransferReceiveDto> Unpaired, Dictionary<string, List<TransferReceiveDto>> OpsByOrgOkpo)
        AnalyzeForm11ForOrganization(
            List<TransferReceiveDto> ourOps,
            List<TransferReceiveDto> counterpartOps,
            string ourOkpoRaw,
            TransferReceiveFormParams options,
            IReadOnlyDictionary<string, List<int>>? repsIdsByNormOkpo = null,
            ProgressReporter? progress = null) =>
        AnalyzeFormForOrganization(ourOps, counterpartOps, ourOkpoRaw, options, repsIdsByNormOkpo, progress);

    /// <summary>
    /// Пул операций по нормализованному ОКПО.
    /// Свои ops мержатся с контрагентами без дублей по Id (self-pair внутри org).
    /// Дополнительно индексируем по ОКПО, которыми org нашлась в кол. 19 (алиасы),
    /// чтобы closest/pair находили контрагента даже если титульный ОКПО отличается от сырого в операции.
    /// </summary>
    private static Dictionary<string, List<TransferReceiveDto>> BuildOpsPoolByOrgOkpo(
        List<TransferReceiveDto> ourOps,
        List<TransferReceiveDto> counterpartOps,
        IReadOnlyDictionary<string, List<int>>? repsIdsByNormOkpo = null)
    {
        var ourIds = ourOps.Select(op => op.Id).ToHashSet();
        var allOps = ourOps
            .Concat(counterpartOps.Where(op => !ourIds.Contains(op.Id)))
            .ToList();

        var opsByRepsId = allOps
            .GroupBy(op => op.RepsId)
            .ToDictionary(g => g.Key, g => g.OrderBy(op => op.Id).ToList());

        var result = new Dictionary<string, List<TransferReceiveDto>>(StringComparer.Ordinal);

        void IndexUnder(string okpoNorm, IReadOnlyList<TransferReceiveDto> ops)
        {
            if (okpoNorm.Length == 0 || ops.Count == 0)
            {
                return;
            }

            if (!result.TryGetValue(okpoNorm, out var list))
            {
                list = [];
                result[okpoNorm] = list;
            }

            var existingIds = list.Count == 0 ? null : list.Select(op => op.Id).ToHashSet();
            foreach (var op in ops)
            {
                if (existingIds is null || existingIds.Add(op.Id))
                {
                    list.Add(op);
                }
            }
        }

        foreach (var op in allOps)
        {
            foreach (var key in OkpoIndexKeys(op.OrgOkpo))
            {
                if (!result.TryGetValue(key, out var list))
                {
                    list = [];
                    result[key] = list;
                }

                list.Add(op);
            }
        }

        if (repsIdsByNormOkpo is not null)
        {
            foreach (var (normOkpo, repsIds) in repsIdsByNormOkpo)
            {
                foreach (var repsId in repsIds)
                {
                    if (opsByRepsId.TryGetValue(repsId, out var ops))
                    {
                        IndexUnder(normOkpo, ops);
                    }
                }
            }
        }

        foreach (var key in result.Keys.ToList())
        {
            var list = result[key];
            list.Sort((left, right) => left.Id.CompareTo(right.Id));
            // Один op может попасть под ключ и через титул, и через алиас.
            var deduped = new List<TransferReceiveDto>(list.Count);
            var seen = new HashSet<int>();
            foreach (var op in list)
            {
                if (seen.Add(op.Id))
                {
                    deduped.Add(op);
                }
            }

            result[key] = deduped;
        }

        return result;
    }

    private static List<TransferReceiveDto> ComputeUnpairedOperations(
        List<TransferReceiveDto> ourOps,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        string ourOkpoRaw,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null,
        PairingCandidateIndex? prebuiltIndex = null)
    {
        var usedCandidateIds = new HashSet<int>();
        var pairedOurIds = new HashSet<int>();
        var total = ourOps.Count;
        var processed = 0;
        var ourOkpoNorm = NormalizeNumber(ourOkpoRaw);

        void OnSourceDone()
        {
            processed++;
            progress?.Report(processed, total, $"сопоставление: {processed} из {total} операций");
        }

        var ourTransfers = ourOps.Where(op => op.IsTransfer).OrderBy(op => op.Id).ToList();
        var ourReceives = ourOps.Where(op => !op.IsTransfer).OrderBy(op => op.Id).ToList();
        var index = prebuiltIndex ?? PairingCandidateIndex.Build(opsByOrgOkpo, options);

        MatchSide(ourTransfers, isSourceTransfer: true, index, usedCandidateIds, pairedOurIds, ourOkpoRaw, ourOkpoNorm, options, OnSourceDone);
        MatchSide(
            ourReceives.Where(op => !pairedOurIds.Contains(op.Id)).ToList(),
            isSourceTransfer: false,
            index,
            usedCandidateIds,
            pairedOurIds,
            ourOkpoRaw,
            ourOkpoNorm,
            options,
            OnSourceDone);

        // Операции, уже спаренные как кандидат на стороне передачи, в MatchSide receives не попадали.
        processed = total;
        progress?.Report(processed, total, $"сопоставление: {processed} из {total} операций");

        return ourOps
            .Where(op => !pairedOurIds.Contains(op.Id))
            .OrderBy(op => op.Id)
            .ToList();
    }

    /// <summary>Алиас для тестов / старых вызовов.</summary>
    private static List<TransferReceiveDto> ComputeUnpairedForm11(
        List<TransferReceiveDto> ourOps,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        string ourOkpoRaw,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null) =>
        ComputeUnpairedOperations(ourOps, opsByOrgOkpo, ourOkpoRaw, options, progress);

    private static void MatchSide(
        List<TransferReceiveDto> sources,
        bool isSourceTransfer,
        PairingCandidateIndex index,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
        string ourOkpoRaw,
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        Action? onSourceDone = null)
    {
        var withSerial = sources.Where(op => !SerialNumbersAreEmpty(op)).ToList();
        var withoutSerial = sources.Where(SerialNumbersAreEmpty).ToList();

        MatchWithSerial(withSerial, isSourceTransfer, index, usedCandidateIds, pairedOurIds, ourOkpoRaw, ourOkpoNorm, options, onSourceDone);
        MatchWithoutSerial(withoutSerial, isSourceTransfer, index, usedCandidateIds, pairedOurIds, ourOkpoRaw, ourOkpoNorm, options, onSourceDone);
    }

    private static void MatchWithSerial(
        List<TransferReceiveDto> sources,
        bool isSourceTransfer,
        PairingCandidateIndex index,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
        string ourOkpoRaw,
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        Action? onSourceDone = null)
    {
        foreach (var source in sources)
        {
            if (pairedOurIds.Contains(source.Id))
            {
                onSourceDone?.Invoke();
                continue;
            }

            var sourceKey = BuildPairingKey(
                source, options, includeSerial: true, includeQuantity: options.CheckQuantity);
            var candidates = index.LookupWithSerial(source, isSourceTransfer, sourceKey, options);
            TransferReceiveDto? claimed = null;
            foreach (var (candidate, candidateKey) in candidates)
            {
                if (usedCandidateIds.Contains(candidate.Id) || candidate.Id == source.Id)
                {
                    continue;
                }

                if (!IsPairMatch(
                        source,
                        candidate,
                        options,
                        ourOkpoRaw,
                        includeSerial: true,
                        precomputedSourceKey: sourceKey,
                        precomputedCandidateKey: candidateKey))
                {
                    continue;
                }

                claimed = candidate;
                break;
            }

            if (claimed is not null)
            {
                usedCandidateIds.Add(claimed.Id);
                pairedOurIds.Add(source.Id);
                if (NormalizeNumber(claimed.OrgOkpo) == ourOkpoNorm)
                {
                    pairedOurIds.Add(claimed.Id);
                }
            }

            onSourceDone?.Invoke();
        }
    }

    private static void MatchWithoutSerial(
        List<TransferReceiveDto> sources,
        bool isSourceTransfer,
        PairingCandidateIndex index,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
        string ourOkpoRaw,
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        Action? onSourceDone = null)
    {
        var remainingByCandidateId = new Dictionary<int, RemainingRowState>();

        RemainingRowState GetOrCreateState(TransferReceiveDto candidate)
        {
            if (!remainingByCandidateId.TryGetValue(candidate.Id, out var state))
            {
                state = new RemainingRowState(candidate, GetQuantityForComparison(candidate));
                remainingByCandidateId[candidate.Id] = state;
            }

            return state;
        }

        foreach (var source in sources)
        {
            if (pairedOurIds.Contains(source.Id))
            {
                onSourceDone?.Invoke();
                continue;
            }

            var remainingSourceQty = options.CheckQuantity ? GetQuantityForComparison(source) : 1;
            var sourceKey = BuildPairingKey(
                source, options, includeSerial: false, includeQuantity: false);
            var candidates = index.LookupWithoutSerial(source, isSourceTransfer, sourceKey, options);

            foreach (var (candidate, candidateKey) in candidates)
            {
                if (remainingSourceQty <= 0)
                {
                    break;
                }

                if (usedCandidateIds.Contains(candidate.Id) || candidate.Id == source.Id)
                {
                    continue;
                }

                var state = GetOrCreateState(candidate);
                if (state.RemainingQuantity <= 0)
                {
                    continue;
                }

                // Для безсерийных qty в ключе не участвует — проверяем остальные поля + активность/коды/даты.
                if (!IsPairMatch(
                        source,
                        state.Row,
                        options,
                        ourOkpoRaw,
                        includeSerial: false,
                        precomputedSourceKey: sourceKey,
                        precomputedCandidateKey: candidateKey))
                {
                    continue;
                }

                if (options.CheckQuantity)
                {
                    var matchedQty = Math.Min(remainingSourceQty, state.RemainingQuantity);
                    remainingSourceQty -= matchedQty;
                    state.RemainingQuantity -= matchedQty;
                }
                else
                {
                    remainingSourceQty = 0;
                    state.RemainingQuantity = 0;
                }

                if (state.RemainingQuantity <= 0)
                {
                    usedCandidateIds.Add(state.Row.Id);
                    if (NormalizeNumber(state.Row.OrgOkpo) == ourOkpoNorm)
                    {
                        pairedOurIds.Add(state.Row.Id);
                    }
                }
            }

            if (remainingSourceQty <= 0)
            {
                pairedOurIds.Add(source.Id);
            }

            onSourceDone?.Invoke();
        }
    }

    /// <summary>
    /// Индекс кандидатов для точного pairing: по ОКПО контрагента, направлению и ключу пары.
    /// При включённой дате — дополнительно по дню (пара требует точного совпадения даты).
    /// </summary>
    private sealed class PairingCandidateIndex
    {
        private readonly Dictionary<string, OkpoDirectionBuckets> _byOkpo;

        private PairingCandidateIndex(Dictionary<string, OkpoDirectionBuckets> byOkpo) =>
            _byOkpo = byOkpo;

        public static PairingCandidateIndex Build(
            IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
            TransferReceiveFormParams options)
        {
            var byOkpo = new Dictionary<string, OkpoDirectionBuckets>(opsByOrgOkpo.Count, StringComparer.Ordinal);
            foreach (var (okpo, ops) in opsByOrgOkpo)
            {
                var transfers = new DirectionBucket();
                var receives = new DirectionBucket();
                foreach (var op in ops)
                {
                    var bucket = op.IsTransfer ? transfers : receives;
                    var keyWithSerial = BuildPairingKey(
                        op, options, includeSerial: true, includeQuantity: options.CheckQuantity);
                    bucket.AddWithSerial(op, keyWithSerial, options.CheckOperationDate);

                    if (SerialNumbersAreEmpty(op))
                    {
                        var keyWithoutSerial = BuildPairingKey(
                            op, options, includeSerial: false, includeQuantity: false);
                        bucket.AddWithoutSerial(op, keyWithoutSerial, options.CheckOperationDate);
                    }
                }

                transfers.SortAll();
                receives.SortAll();
                byOkpo[okpo] = new OkpoDirectionBuckets(transfers, receives);
            }

            return new PairingCandidateIndex(byOkpo);
        }

        public IEnumerable<(TransferReceiveDto Row, string PairingKey)> LookupWithSerial(
            TransferReceiveDto source,
            bool isSourceTransfer,
            string sourcePairingKey,
            TransferReceiveFormParams options)
        {
            if (!TryGetDirection(source, isSourceTransfer, out var bucket))
            {
                return [];
            }

            return bucket.LookupWithSerial(source, sourcePairingKey, options.CheckOperationDate);
        }

        public IEnumerable<(TransferReceiveDto Row, string PairingKey)> LookupWithoutSerial(
            TransferReceiveDto source,
            bool isSourceTransfer,
            string sourcePairingKey,
            TransferReceiveFormParams options)
        {
            if (!TryGetDirection(source, isSourceTransfer, out var bucket))
            {
                return [];
            }

            return bucket.LookupWithoutSerial(source, sourcePairingKey, options.CheckOperationDate);
        }

        private bool TryGetDirection(
            TransferReceiveDto source,
            bool isSourceTransfer,
            out DirectionBucket bucket)
        {
            foreach (var key in OkpoIndexKeys(source.ProviderOrRecieverOkpo))
            {
                if (!_byOkpo.TryGetValue(key, out var pools))
                {
                    continue;
                }

                // Передача ищет приём у контрагента и наоборот.
                bucket = isSourceTransfer ? pools.Receives : pools.Transfers;
                return true;
            }

            bucket = DirectionBucket.Empty;
            return false;
        }

        private sealed class OkpoDirectionBuckets(DirectionBucket transfers, DirectionBucket receives)
        {
            public DirectionBucket Transfers { get; } = transfers;
            public DirectionBucket Receives { get; } = receives;
        }

        private sealed class DirectionBucket
        {
            public static DirectionBucket Empty { get; } = new();

            private readonly Dictionary<string, List<(TransferReceiveDto Row, string PairingKey)>> _withSerial =
                new(StringComparer.Ordinal);

            private readonly Dictionary<string, List<(TransferReceiveDto Row, string PairingKey)>> _withoutSerial =
                new(StringComparer.Ordinal);

            public void AddWithSerial(TransferReceiveDto op, string pairingKey, bool indexByDate) =>
                Add(_withSerial, op, pairingKey, indexByDate);

            public void AddWithoutSerial(TransferReceiveDto op, string pairingKey, bool indexByDate) =>
                Add(_withoutSerial, op, pairingKey, indexByDate);

            public void SortAll()
            {
                foreach (var list in _withSerial.Values)
                {
                    list.Sort(static (a, b) => a.Row.Id.CompareTo(b.Row.Id));
                }

                foreach (var list in _withoutSerial.Values)
                {
                    list.Sort(static (a, b) => a.Row.Id.CompareTo(b.Row.Id));
                }
            }

            public IEnumerable<(TransferReceiveDto Row, string PairingKey)> LookupWithSerial(
                TransferReceiveDto source,
                string sourcePairingKey,
                bool filterByDate) =>
                Lookup(_withSerial, source, sourcePairingKey, filterByDate);

            public IEnumerable<(TransferReceiveDto Row, string PairingKey)> LookupWithoutSerial(
                TransferReceiveDto source,
                string sourcePairingKey,
                bool filterByDate) =>
                Lookup(_withoutSerial, source, sourcePairingKey, filterByDate);

            private static void Add(
                Dictionary<string, List<(TransferReceiveDto Row, string PairingKey)>> map,
                TransferReceiveDto op,
                string pairingKey,
                bool indexByDate)
            {
                var lookupKey = MakeLookupKey(pairingKey, op.OpDate, indexByDate);
                if (!map.TryGetValue(lookupKey, out var list))
                {
                    list = [];
                    map[lookupKey] = list;
                }

                list.Add((op, pairingKey));
            }

            private static IEnumerable<(TransferReceiveDto Row, string PairingKey)> Lookup(
                Dictionary<string, List<(TransferReceiveDto Row, string PairingKey)>> map,
                TransferReceiveDto source,
                string sourcePairingKey,
                bool filterByDate)
            {
                var lookupKey = MakeLookupKey(sourcePairingKey, source.OpDate, filterByDate);
                return map.TryGetValue(lookupKey, out var list) ? list : [];
            }

            /// <summary>
            /// Ключ индекса: pairingKey, либо pairingKey + точный день / нормализованная непарсибельная дата.
            /// </summary>
            private static string MakeLookupKey(string pairingKey, string? opDate, bool indexByDate)
            {
                if (!indexByDate)
                {
                    return pairingKey;
                }

                if (DateOnly.TryParse(opDate, out var date))
                {
                    return string.Concat(pairingKey, "\0d:", date.DayNumber.ToString(CultureInfo.InvariantCulture));
                }

                return string.Concat(pairingKey, "\0u:", NormalizeDate(opDate));
            }
        }
    }

    private sealed class RemainingRowState(TransferReceiveDto row, int remainingQuantity)
    {
        public TransferReceiveDto Row { get; } = row;
        public int RemainingQuantity { get; set; } = remainingQuantity;
    }

    #endregion
}
