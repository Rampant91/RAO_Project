using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing;

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

        progressBarVM.SetProgressBar(12, "Поиск организаций с операциями приёма/передачи", "Вся БД", "Выгрузка в .xlsx");
        var discoverProgress = new ProgressReporter(
            (percent, text) => progressBarVM.SetProgressBar(percent, text, "Вся БД"),
            percentMin: 12,
            percentMax: 28);
        var enabledIds = pairingParams.EnabledFormIds;
        var candidates = await LoadOrganizationsWithTransferReceiveAsync(
            db, cts.Token, discoverProgress, enabledIds);
        if (candidates.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar, wholeDatabase: true);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        var bulkProgress = new ProgressReporter(
            (percent, text) => progressBarVM.SetProgressBar(percent, text, "Вся БД"),
            percentMin: 28,
            percentMax: 52);
        var orgTitles = candidates.ToDictionary(c => c.Id, c => c.Title);
        var bulk = await LoadAllTransferReceiveBulkAsync(
            db, orgTitles, cts.Token, bulkProgress, enabledIds);

        progressBarVM.SetProgressBar(52, "Построение общих пулов сопоставления", "Вся БД", "Выгрузка в .xlsx");
        var sharedPools = new Dictionary<TransferReceiveFormId, Dictionary<string, List<TransferReceiveDto>>>();
        foreach (var formId in enabledIds)
        {
            sharedPools[formId] = BuildOpsPoolByOrgOkpo([], bulk.GetAllOps(formId), bulk.RepsIdsByNormOkpo);
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
                (p, text) => Status(Math.Min(percentSpan - 1, Math.Max(0, p / 5)), text));

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

        FinalizeWorkbookTables(excelPackage);
        progressBarVM.SetProgressBar(95, "Сохранение", "Вся БД", "Выгрузка в .xlsx");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
        await CleanupAndClose(progressBar, tmpDbPath);
    }

    #endregion

    #region Discover orgs with transfer/receive

    /// <summary>
    /// Организации с хотя бы одной операцией приёма/передачи на 1.1 или 1.3.
    /// Id — Distinct по таблицам форм; титулы — точечно из form_10 (без Include Master→Rows10).
    /// </summary>
    private static async Task<List<TransferReceiveOrgCandidate>> LoadOrganizationsWithTransferReceiveAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress = null,
        IReadOnlySet<TransferReceiveFormId>? enabledFormIds = null)
    {
        enabledFormIds ??= ImplementedFormDescriptors.Select(d => d.Id).ToHashSet();
        var repsIds = new HashSet<int>();
        var stages = Math.Max(1, enabledFormIds.Count);
        var stage = 0;
        foreach (var descriptor in ImplementedFormDescriptors.Where(d => enabledFormIds.Contains(d.Id)))
        {
            progress?.ReportNow(stage, stages,
                $"поиск организаций: сканирование формы {descriptor.FormNum} ({stage + 1} из {stages})");
            await AddRepsIdsForFormAsync(db, descriptor.Id, repsIds, cancellationToken);
            stage++;
            progress?.ReportNow(stage, stages,
                $"поиск организаций: форма {descriptor.FormNum} — найдено организаций: {repsIds.Count}");
        }

        if (repsIds.Count == 0)
        {
            return [];
        }

        // Только org из активной коллекции (как раньше Where DBObservable != null).
        var orderedIds = repsIds.OrderBy(id => id).ToList();
        var activeIds = new List<int>(orderedIds.Count);
        var idChunks = ChunkIds(orderedIds).ToList();
        for (var i = 0; i < idChunks.Count; i++)
        {
            progress?.ReportNow(i, idChunks.Count,
                $"поиск организаций: фильтр активных карточек {i + 1} из {idChunks.Count}");
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.DBObservable != null && idChunks[i].Contains(reps.Id))
                .Select(reps => reps.Id)
                .ToListAsync(cancellationToken);
            activeIds.AddRange(batch);
        }

        if (activeIds.Count == 0)
        {
            return [];
        }

        progress?.Status($"поиск организаций: загрузка титулов ({activeIds.Count} орг.)…");
        var orgTitles = new Dictionary<int, OrgTitleInfo>();
        await LoadOrgTitlesForRepsIdsAsync(db, activeIds, orgTitles, cancellationToken);
        progress?.ReportNow(1, 1, $"поиск организаций: готово — {orgTitles.Count}");

        var regNoComparer = new CustomReportsComparer();
        return orgTitles
            .Select(kv => new TransferReceiveOrgCandidate { Id = kv.Key, Title = kv.Value })
            .OrderBy(c => c.Title.RegNo, regNoComparer)
            .ThenBy(c => c.Title.Okpo, regNoComparer)
            .ToList();
    }

    private static async Task AddRepsIdsForFormAsync(
        DBModel db,
        TransferReceiveFormId formId,
        HashSet<int> repsIds,
        CancellationToken cancellationToken)
    {
        switch (formId)
        {
            case TransferReceiveFormId.Form11:
                await AddRepsIdsWithTransferReceiveCodesAsync(db.form_11, repsIds, cancellationToken);
                break;
            case TransferReceiveFormId.Form13:
                await AddRepsIdsWithTransferReceiveCodesAsync(db.form_13, repsIds, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(formId), formId, "Нет discover-загрузчика для формы.");
        }
    }

    private static async Task AddRepsIdsWithTransferReceiveCodesAsync<TForm>(
        DbSet<TForm> forms,
        HashSet<int> repsIds,
        CancellationToken cancellationToken)
        where TForm : Form1
    {
        var codes = TransferReceiveCodesForm11And13;
        var ids = await forms
            .AsNoTracking()
            .Where(form => form.OperationCode_DB != null
                           && codes.Contains(form.OperationCode_DB)
                           && form.Report != null
                           && form.Report.Reports != null)
            .Select(form => form.Report!.Reports.Id)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var id in ids)
        {
            repsIds.Add(id);
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
            counterpartOpsByForm[descriptor.Id] = await LoadFormOpsForRepsIdsAsync(
                descriptor.Id, db, counterpartRepsIds, orgTitles, cancellationToken, loadProgress);
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
        var ourOkpoNorm = NormalizeNumber(ourOkpo);
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
                ourOps, counterpartOps, ourOkpoNorm, formOptions, repsIdsByOkpo, matchProgress);
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
    /// Whole-DB: пул ОКПО уже построен один раз; per-org только unpaired + closest.
    /// </summary>
    private OrganizationTransferReceiveExport? BuildOrganizationExportFromSharedPools(
        string ourOkpo,
        IReadOnlyDictionary<TransferReceiveFormId, List<TransferReceiveDto>> ourOpsByForm,
        IReadOnlyDictionary<TransferReceiveFormId, Dictionary<string, List<TransferReceiveDto>>> sharedPools,
        TransferReceiveParamsSet pairingParams,
        Action<int, string>? reportProgress = null)
    {
        _currentParams = pairingParams;
        var ourOkpoNorm = NormalizeNumber(ourOkpo);
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
                ourOps, sharedPool, ourOkpoNorm, formOptions, matchProgress);
            unpairedByForm[descriptor.Id] = unpaired;

            reportProgress?.Invoke(matchMax,
                $"непарных операций {descriptor.FormNum}: {unpaired.Count} из {ourOps.Count}");

            var closestProgress = new ProgressReporter(reportProgress, matchMax, closestMax);
            closestProgress.Status(
                $"поиск ближайших совпадений {descriptor.FormNum}: 0 из {unpaired.Count}");
            _closestByForm[descriptor.Id] = BuildClosestMatchResults(
                unpaired, sharedPool, formOptions, closestProgress);
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
    /// Единое ядро: пул кандидатов + непарные. Без обращений к БД.
    /// Org-режим и whole-DB отличаются только тем, откуда взяты <paramref name="ourOps"/> /
    /// <paramref name="counterpartOps"/> (точечная загрузка vs bulk).
    /// </summary>
    private static (List<TransferReceiveDto> Unpaired, Dictionary<string, List<TransferReceiveDto>> OpsByOrgOkpo)
        AnalyzeFormForOrganization(
            List<TransferReceiveDto> ourOps,
            List<TransferReceiveDto> counterpartOps,
            string ourOkpoNorm,
            TransferReceiveFormParams options,
            IReadOnlyDictionary<string, List<int>>? repsIdsByNormOkpo = null,
            ProgressReporter? progress = null)
    {
        var opsByOrgOkpo = BuildOpsPoolByOrgOkpo(ourOps, counterpartOps, repsIdsByNormOkpo);
        var unpaired = ComputeUnpairedOperations(ourOps, opsByOrgOkpo, ourOkpoNorm, options, progress);
        return (unpaired, opsByOrgOkpo);
    }

    /// <summary>Алиас для тестов / старых вызовов.</summary>
    private static (List<TransferReceiveDto> Unpaired, Dictionary<string, List<TransferReceiveDto>> OpsByOrgOkpo)
        AnalyzeForm11ForOrganization(
            List<TransferReceiveDto> ourOps,
            List<TransferReceiveDto> counterpartOps,
            string ourOkpoNorm,
            TransferReceiveFormParams options,
            IReadOnlyDictionary<string, List<int>>? repsIdsByNormOkpo = null,
            ProgressReporter? progress = null) =>
        AnalyzeFormForOrganization(ourOps, counterpartOps, ourOkpoNorm, options, repsIdsByNormOkpo, progress);

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

        foreach (var group in allOps.GroupBy(op => NormalizeNumber(op.OrgOkpo), StringComparer.Ordinal))
        {
            IndexUnder(group.Key, group.OrderBy(op => op.Id).ToList());
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

        foreach (var list in result.Values)
        {
            list.Sort((left, right) => left.Id.CompareTo(right.Id));
        }

        return result;
    }

    private static List<TransferReceiveDto> ComputeUnpairedOperations(
        List<TransferReceiveDto> ourOps,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null)
    {
        var usedCandidateIds = new HashSet<int>();
        var pairedOurIds = new HashSet<int>();
        var total = ourOps.Count;
        var processed = 0;

        void OnSourceDone()
        {
            processed++;
            progress?.Report(processed, total, $"сопоставление: {processed} из {total} операций");
        }

        var ourTransfers = ourOps.Where(op => op.IsTransfer).OrderBy(op => op.Id).ToList();
        var ourReceives = ourOps.Where(op => !op.IsTransfer).OrderBy(op => op.Id).ToList();

        MatchSide(ourTransfers, isSourceTransfer: true, opsByOrgOkpo, usedCandidateIds, pairedOurIds, ourOkpoNorm, options, OnSourceDone);
        MatchSide(
            ourReceives.Where(op => !pairedOurIds.Contains(op.Id)).ToList(),
            isSourceTransfer: false,
            opsByOrgOkpo,
            usedCandidateIds,
            pairedOurIds,
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
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        ProgressReporter? progress = null) =>
        ComputeUnpairedOperations(ourOps, opsByOrgOkpo, ourOkpoNorm, options, progress);

    private static void MatchSide(
        List<TransferReceiveDto> sources,
        bool isSourceTransfer,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
        string ourOkpoNorm,
        TransferReceiveFormParams options,
        Action? onSourceDone = null)
    {
        var withSerial = sources.Where(op => !SerialNumbersAreEmpty(op)).ToList();
        var withoutSerial = sources.Where(SerialNumbersAreEmpty).ToList();

        MatchWithSerial(withSerial, isSourceTransfer, opsByOrgOkpo, usedCandidateIds, pairedOurIds, ourOkpoNorm, options, onSourceDone);
        MatchWithoutSerial(withoutSerial, isSourceTransfer, opsByOrgOkpo, usedCandidateIds, pairedOurIds, ourOkpoNorm, options, onSourceDone);
    }

    private static void MatchWithSerial(
        List<TransferReceiveDto> sources,
        bool isSourceTransfer,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
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

            var candidates = GetCounterpartCandidates(source, isSourceTransfer, opsByOrgOkpo);
            TransferReceiveDto? claimed = null;
            foreach (var candidate in candidates)
            {
                if (usedCandidateIds.Contains(candidate.Id) || candidate.Id == source.Id)
                {
                    continue;
                }

                // ±N дней — только сужение поиска; пара требует точной даты (IsPairMatch).
                if (options.CheckOperationDate
                    && !DateWithinTolerance(source.OpDate, candidate.OpDate))
                {
                    continue;
                }

                if (!IsPairMatch(source, candidate, options, ourOkpoNorm, includeSerial: true))
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
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo,
        HashSet<int> usedCandidateIds,
        HashSet<int> pairedOurIds,
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
            var candidates = GetCounterpartCandidates(source, isSourceTransfer, opsByOrgOkpo)
                .Where(c => !usedCandidateIds.Contains(c.Id) && c.Id != source.Id)
                .Where(SerialNumbersAreEmpty)
                .Where(c => !options.CheckOperationDate
                            || DateWithinTolerance(source.OpDate, c.OpDate))
                .OrderBy(c => c.Id)
                .ToList();

            foreach (var candidate in candidates)
            {
                if (remainingSourceQty <= 0)
                {
                    break;
                }

                var state = GetOrCreateState(candidate);
                if (state.RemainingQuantity <= 0)
                {
                    continue;
                }

                // Для безсерийных qty в ключе не участвует — проверяем остальные поля + активность/коды/даты.
                if (!IsPairMatch(source, state.Row, options, ourOkpoNorm, includeSerial: false))
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

    private static List<TransferReceiveDto> GetCounterpartCandidates(
        TransferReceiveDto source,
        bool isSourceTransfer,
        IReadOnlyDictionary<string, List<TransferReceiveDto>> opsByOrgOkpo)
    {
        var counterpartOkpo = NormalizeNumber(source.ProviderOrRecieverOkpo);
        if (counterpartOkpo.Length == 0
            || !opsByOrgOkpo.TryGetValue(counterpartOkpo, out var ops))
        {
            return [];
        }

        return ops
            .Where(op => isSourceTransfer ? !op.IsTransfer : op.IsTransfer)
            .ToList();
    }

    private sealed class RemainingRowState(TransferReceiveDto row, int remainingQuantity)
    {
        public TransferReceiveDto Row { get; } = row;
        public int RemainingQuantity { get; set; } = remainingQuantity;
    }

    #endregion
}
