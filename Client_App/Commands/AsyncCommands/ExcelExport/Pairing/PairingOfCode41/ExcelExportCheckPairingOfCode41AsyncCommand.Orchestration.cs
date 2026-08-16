using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Mode / export model

    private const string WholeDbParameter = "All";

    private static bool IsWholeDbMode(object? parameter) =>
        parameter is string s && string.Equals(s, WholeDbParameter, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Результат проверки одной организации: непарные строки для дописывания в Excel (null = непарных нет).
    /// </summary>
    private sealed class OrganizationPairingExport
    {
        public required List<Operation41PairingDto> UnpairedForm11 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm12 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm13 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm14 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm15 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm16 { get; init; }
    }

    #endregion

    #region Execute selected org / whole DB

    private async Task ExecuteForSelectedOrganizationAsync(
        Reports selectedReports,
        PairingParamsSet pairingParams,
        AnyTaskProgressBar progressBar,
        CancellationTokenSource cts)
    {
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        OrgMatchQuery.EnsureTitleRowsLoaded(selectedReports);
        var regNum = RemoveForbiddenChars(selectedReports.Master_DB.RegNoRep.Value);
        var okpo = RemoveForbiddenChars(selectedReports.Master_DB.OkpoRep.Value);
        var fileName = $"{regNum}_{okpo}_непарные_операции_41";
        var exportName = $"{regNum}_{okpo}";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", exportName, "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);
        if (string.IsNullOrEmpty(fullPath))
        {
            return;
        }

        progressBarVM.SetProgressBar(8, "Загрузка справочника радионуклидов", exportName, "Выгрузка в .xlsx");
        if (!TryLoadRDictionary(out var rDictionaryError))
        {
            await ShowRDictionaryLoadErrorMessage(progressBar, rDictionaryError);
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        progressBarVM.SetProgressBar(10, "Создание временной БД", exportName, "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        void Status(int percent, string text) =>
            progressBarVM.SetProgressBar(percent, text, exportName);

        var export = await BuildOrganizationPairingExportAsync(
            db, selectedReports, pairingParams, cts.Token, Status);

        if (export is null)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        Status(70, "Инициализация Excel пакета");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        InitializePairingWorkbook(excelPackage);
        AppendOrganizationToPairingWorkbook(excelPackage, export, progressBarVM, percentBase: 72, percentSpan: 20);
        progressBarVM.SetProgressBar(93, "Оформление таблиц Excel (фильтры, сетка)…", exportName, "Выгрузка в .xlsx");
        FinalizePairingWorkbookTables(excelPackage);

        Status(95, "Сохранение");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
        await CleanupAndClose(progressBar, tmpDbPath);
    }

    private async Task ExecuteForWholeDatabaseAsync(
        PairingParamsSet pairingParams,
        AnyTaskProgressBar progressBar,
        CancellationTokenSource cts)
    {
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        const string fileName = "непарные_операции_41_вся_БД";

        progressBarVM.SetProgressBar(5, "Запрос пути сохранения", "Вся БД", "Выгрузка в .xlsx");
        var (fullPath, openTemp) = await ExcelGetFullPathWithUniqueIndex(fileName, cts, progressBar);
        if (string.IsNullOrEmpty(fullPath))
        {
            return;
        }

        progressBarVM.SetProgressBar(8, "Загрузка справочника радионуклидов", "Вся БД", "Выгрузка в .xlsx");
        if (!TryLoadRDictionary(out var rDictionaryError))
        {
            await ShowRDictionaryLoadErrorMessage(progressBar, rDictionaryError);
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            return;
        }

        progressBarVM.SetProgressBar(10, "Создание временной БД", "Вся БД", "Выгрузка в .xlsx");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(12, "Поиск организаций с операциями 41", "Вся БД", "Выгрузка в .xlsx");
        var wholeDbLastPercent = 12;
        void ReportWholeDb(int percent, string text)
        {
            if (percent < wholeDbLastPercent)
            {
                percent = wholeDbLastPercent;
            }
            else
            {
                wholeDbLastPercent = percent;
            }

            progressBarVM.SetProgressBar(percent, text, "Вся БД");
        }

        var scanProgress = new ProgressReporter(ReportWholeDb, percentMin: 12, percentMax: 20);
        var cardProgress = new ProgressReporter(ReportWholeDb, percentMin: 20, percentMax: 28);
        var candidates = await LoadOrganizationsWithOperation41Async(
            db, cts.Token, scanProgress, cardProgress);
        if (candidates.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        var bulkProgress = new ProgressReporter(ReportWholeDb, percentMin: 28, percentMax: 55);
        var bulkByOrg = await LoadAllOperation41GroupedByRepsIdAsync(
            db, pairingParams, cts.Token, bulkProgress);

        progressBarVM.SetProgressBar(55, "Инициализация Excel пакета", "Вся БД", "Выгрузка в .xlsx");
        using var excelPackage = await InitializeExcelPackage(fullPath);
        InitializePairingWorkbook(excelPackage);

        var anyUnpairedWritten = false;
        var total = candidates.Count;
        for (var i = 0; i < total; i++)
        {
            cts.Token.ThrowIfCancellationRequested();
            var org = candidates[i];
            var orgIndex = i + 1;
            var regNum = RemoveForbiddenChars(org.Master_DB.RegNoRep.Value);
            var okpo = RemoveForbiddenChars(org.Master_DB.OkpoRep.Value);
            var exportName = $"{regNum}_{okpo}";
            var percentBase = 55 + (int)(38.0 * i / total);
            var percentSpan = Math.Max(1, (int)(38.0 / total));

            void Status(int offsetWithinOrg, string stage) =>
                progressBarVM.SetProgressBar(
                    Math.Min(94, percentBase + offsetWithinOrg),
                    $"Организация {orgIndex} из {total}: {stage}",
                    exportName);

            Status(0, "сопоставление");
            bulkByOrg.TryGetValue(org.Id, out var loaded);
            loaded ??= new OrgOperation41Lists();
            var export = await BuildOrganizationPairingExportFromLoadedAsync(
                db,
                org,
                loaded,
                pairingParams,
                cts.Token,
                (p, text) => Status(Math.Min(percentSpan - 1, p / 5), text));

            if (export is null)
            {
                continue;
            }

            Status(percentSpan - 1, "запись в Excel");
            AppendOrganizationToPairingWorkbook(excelPackage, export, progressBarVM: null);
            anyUnpairedWritten = true;
        }

        if (!anyUnpairedWritten)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(93, "Оформление таблиц Excel (фильтры, сетка)…", "Вся БД", "Выгрузка в .xlsx");
        FinalizePairingWorkbookTables(excelPackage);
        progressBarVM.SetProgressBar(95, "Сохранение", "Вся БД", "Выгрузка в .xlsx");
        await ExcelSaveAndOpen(excelPackage, fullPath, openTemp, cts, progressBar);
        await CleanupAndClose(progressBar, tmpDbPath);
    }

    #endregion

    #region Discover orgs with code 41

    /// <summary>
    /// Организации (с Master+Rows10), у которых есть хотя бы одна строка форм 1.1–1.6 с кодом 41.
    /// Id собираются отдельными Distinct по таблицам форм (без тяжёлого OR Any), затем Reports грузятся пакетами по 1000 (лимит Firebird IN).
    /// </summary>
    private static async Task<List<Reports>> LoadOrganizationsWithOperation41Async(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? scanProgress = null,
        ProgressReporter? cardProgress = null)
    {
        var repsIds = new HashSet<int>();
        const int formCount = 6;
        var done = 0;

        async Task ScanFormAsync<TForm>(DbSet<TForm> forms, string formLabel)
            where TForm : Models.Forms.Form1.Form1
        {
            scanProgress?.ReportNow(done, formCount,
                $"поиск организаций: сканирование формы {formLabel} ({done + 1} из {formCount})");
            await AddRepsIdsWithCode41Async(forms, repsIds, cancellationToken);
            done++;
            scanProgress?.ReportNow(done, formCount,
                $"поиск организаций: форма {formLabel} — найдено организаций: {repsIds.Count}");
        }

        await ScanFormAsync(db.form_11, "1.1");
        await ScanFormAsync(db.form_12, "1.2");
        await ScanFormAsync(db.form_13, "1.3");
        await ScanFormAsync(db.form_14, "1.4");
        await ScanFormAsync(db.form_15, "1.5");
        await ScanFormAsync(db.form_16, "1.6");

        if (repsIds.Count == 0)
        {
            return [];
        }

        var orderedIds = repsIds.OrderBy(id => id).ToList();
        var list = new List<Reports>(orderedIds.Count);
        var chunks = ChunkIds(orderedIds).ToList();
        for (var i = 0; i < chunks.Count; i++)
        {
            cardProgress?.ReportNow(i, chunks.Count,
                $"поиск организаций: загрузка карточек {i + 1} из {chunks.Count} (всего {orderedIds.Count})");
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(reps => reps.Master_DB)
                .ThenInclude(master => master.Rows10)
                .Where(reps => reps.DBObservable != null && chunks[i].Contains(reps.Id))
                .ToListAsync(cancellationToken);
            list.AddRange(batch);
        }

        cardProgress?.ReportNow(1, 1, $"поиск организаций: готово — {list.Count}");

        var regNoComparer = new CustomReportsComparer();
        return list
            .OrderBy(reps => reps.Master_DB.RegNoRep.Value, regNoComparer)
            .ThenBy(reps => reps.Master_DB.OkpoRep.Value, regNoComparer)
            .ToList();
    }

    private static async Task AddRepsIdsWithCode41Async<TForm>(
        DbSet<TForm> forms,
        HashSet<int> repsIds,
        CancellationToken cancellationToken)
        where TForm : Models.Forms.Form1.Form1
    {
        var ids = await forms
            .AsNoTracking()
            .Where(form => form.OperationCode_DB == OperationCode
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
    /// Пайплайн одной org: загрузка DTO → реквизиты org → сопоставление → closest → строки Excel.
    /// </summary>
    private async Task<OrganizationPairingExport?> BuildOrganizationPairingExportAsync(
        DBModel db,
        Reports selectedReports,
        PairingParamsSet pairingParams,
        CancellationToken cancellationToken,
        Action<int, string>? reportProgress = null)
    {
        var repsId = selectedReports.Id;
        var p11 = pairingParams.Pairing11To15;

        var loadProgress = new ProgressReporter(reportProgress, percentMin: 15, percentMax: 38);

        var form11 = await LoadOperation41ListAsync(db, repsId, "1.1", cancellationToken, p11, loadProgress);
        var form12 = await LoadOperation41ListAsync(db, repsId, "1.2", cancellationToken, progress: loadProgress);
        var form13 = await LoadOperation41ListAsync(db, repsId, "1.3", cancellationToken, progress: loadProgress);
        var form14 = await LoadOperation41ListAsync(db, repsId, "1.4", cancellationToken, progress: loadProgress);
        var form15 = await LoadOperation41ListAsync(db, repsId, "1.5", cancellationToken, p11, loadProgress);
        var form16 = await LoadOperation41ListAsync(db, repsId, "1.6", cancellationToken, progress: loadProgress);

        return await BuildOrganizationPairingExportFromLoadedAsync(
            db,
            selectedReports,
            new OrgOperation41Lists
            {
                Form11 = form11,
                Form12 = form12,
                Form13 = form13,
                Form14 = form14,
                Form15 = form15,
                Form16 = form16
            },
            pairingParams,
            cancellationToken,
            reportProgress);
    }

    /// <summary>
    /// Реквизиты org + сопоставление + closest-match из уже загруженных DTO (org-режим и whole-DB после bulk).
    /// </summary>
    private async Task<OrganizationPairingExport?> BuildOrganizationPairingExportFromLoadedAsync(
        DBModel db,
        Reports org,
        OrgOperation41Lists loaded,
        PairingParamsSet pairingParams,
        CancellationToken cancellationToken,
        Action<int, string>? reportProgress = null)
    {
        var p11 = pairingParams.Pairing11To15;
        var p12 = pairingParams.Pairing12To16;
        var p13 = pairingParams.Pairing13To16;
        var p14 = pairingParams.Pairing14To16;

        StampOrganizationInfo(loaded.Form11, org);
        StampOrganizationInfo(loaded.Form12, org);
        StampOrganizationInfo(loaded.Form13, org);
        StampOrganizationInfo(loaded.Form14, org);
        StampOrganizationInfo(loaded.Form15, org);
        StampOrganizationInfo(loaded.Form16, org);

        var matchProgress = new ProgressReporter(reportProgress, percentMin: 40, percentMax: 55);
        matchProgress.Status(
            $"сопоставление: 1.1↔1.5 ({loaded.Form11.Count}), 1.2/1.3/1.4↔1.6 ({loaded.Form12.Count + loaded.Form13.Count + loaded.Form14.Count})…");
        var unpaired = ComputeOrganizationUnpaired(
            loaded.Form11, loaded.Form12, loaded.Form13, loaded.Form14, loaded.Form15, loaded.Form16,
            pairingParams);
        matchProgress.Report(1, 1,
            $"непарных: 1.1={unpaired.Form11.Count}, 1.5={unpaired.Form15.Count}, " +
            $"1.2={unpaired.Form12.Count}, 1.3={unpaired.Form13.Count}, 1.4={unpaired.Form14.Count}, 1.6={unpaired.Form16.Count}");

        if (unpaired.Form11.Count == 0 && unpaired.Form15.Count == 0
            && unpaired.Form12.Count == 0 && unpaired.Form13.Count == 0
            && unpaired.Form14.Count == 0 && unpaired.Form16.Count == 0)
        {
            return null;
        }

        var closestProgress11 = new ProgressReporter(reportProgress, 55, 57);
        closestProgress11.Status($"поиск ближайших совпадений 1.1: 0 из {unpaired.Form11.Count}");
        _form11ClosestMatchHighlights = BuildClosestMatchHighlights(unpaired.Form11, loaded.Form15, p11, closestProgress11);

        var closestProgress15 = new ProgressReporter(reportProgress, 57, 59);
        closestProgress15.Status($"поиск ближайших совпадений 1.5: 0 из {unpaired.Form15.Count}");
        _form15ClosestMatchHighlights = BuildClosestMatchHighlights(unpaired.Form15, loaded.Form11, p11, closestProgress15);

        var closestProgress12 = new ProgressReporter(reportProgress, 59, 62);
        closestProgress12.Status($"поиск ближайших совпадений 1.2: 0 из {unpaired.Form12.Count}");
        _form12ClosestMatchHighlights = BuildClosestMatchHighlights12To16(unpaired.Form12, loaded.Form16, p12, closestProgress12);

        var closestProgress13 = new ProgressReporter(reportProgress, 62, 65);
        closestProgress13.Status($"поиск ближайших совпадений 1.3: 0 из {unpaired.Form13.Count}");
        _form13ClosestMatchHighlights = BuildClosestMatchHighlights13To16(unpaired.Form13, loaded.Form16, p13, closestProgress13);

        var closestProgress14 = new ProgressReporter(reportProgress, 65, 68);
        closestProgress14.Status($"поиск ближайших совпадений 1.4: 0 из {unpaired.Form14.Count}");
        _form14ClosestMatchHighlights = BuildClosestMatchHighlights14To16(unpaired.Form14, loaded.Form16, p14, closestProgress14);

        var closestProgress16 = new ProgressReporter(reportProgress, 68, 70);
        closestProgress16.Status($"поиск ближайших совпадений 1.6: 0 из {unpaired.Form16.Count}");
        _form16ClosestMatchHighlights = BuildClosestMatchHighlights16(
            unpaired.Form16, loaded.Form12, loaded.Form13, loaded.Form14, p12, p13, p14, closestProgress16);

        return new OrganizationPairingExport
        {
            UnpairedForm11 = unpaired.Form11,
            UnpairedForm12 = unpaired.Form12,
            UnpairedForm13 = unpaired.Form13,
            UnpairedForm14 = unpaired.Form14,
            UnpairedForm15 = unpaired.Form15,
            UnpairedForm16 = unpaired.Form16
        };
    }

    #endregion

    #region Shared unpaired core

    /// <summary>
    /// Единое ядро сопоставления для режима одной org и всей БД (и для unit-тестов).
    /// 1.1 ищет пару среди 1.5 с кодами 41 и 14; обратная сверка 1.5→1.1 — только по коду 41
    /// (ошибочный код 14 на 1.5 не ищет пару и не попадает в непарные 1.5).
    /// </summary>
    private static OrganizationUnpairedSets ComputeOrganizationUnpaired(
        List<Operation41PairingDto> form11,
        List<Operation41PairingDto> form12,
        List<Operation41PairingDto> form13,
        List<Operation41PairingDto> form14,
        List<Operation41PairingDto> form15,
        List<Operation41PairingDto> form16,
        PairingParamsSet pairingParams)
    {
        var p11 = pairingParams.Pairing11To15;
        var p12 = pairingParams.Pairing12To16;
        var p13 = pairingParams.Pairing13To16;
        var p14 = pairingParams.Pairing14To16;

        // Код 14 на 1.5 — только кандидат для 1.1, не источник обратной сверки.
        var form15Code41Only = form15.Where(IsForm15Code41).ToList();

        // 1.2 / 1.3 / 1.4 / 1.6 — один пул 1.6: строка 1.6 закрывает не больше одной строки РВ (12→13→14).
        var (unpaired12, unpaired13, unpaired14, unpaired16) =
            GetUnpairedForms12To16Shared(form12, form13, form14, form16, p12, p13, p14);

        return new OrganizationUnpairedSets(
            GetUnpairedOperations11To15(form11, form15, p11),
            unpaired12,
            unpaired13,
            unpaired14,
            GetUnpairedOperations11To15(form15Code41Only, form11, p11),
            unpaired16);
    }

    private static bool IsForm15Code41(Operation41PairingDto row) =>
        string.Equals(row.OpCode.Trim(), OperationCode, StringComparison.Ordinal);

    private static bool IsForm15PairingCandidateOpCode(string? opCode)
    {
        var code = opCode?.Trim() ?? string.Empty;
        return code == OperationCode || code == Form15ReceiveMistypeOpCode;
    }

    private readonly record struct OrganizationUnpairedSets(
        List<Operation41PairingDto> Form11,
        List<Operation41PairingDto> Form12,
        List<Operation41PairingDto> Form13,
        List<Operation41PairingDto> Form14,
        List<Operation41PairingDto> Form15,
        List<Operation41PairingDto> Form16);

    #endregion

    #region Progress

    /// <summary>
    /// Редкие обновления прогрессбара (не чаще чем раз в <see cref="MinIntervalMs"/> мс),
    /// плюс всегда границы этапа — без заметной потери производительности.
    /// </summary>
    private sealed class ProgressReporter(Action<int, string>? report, int percentMin, int percentMax)
    {
        private const int MinIntervalMs = 300;
        private long _lastReportTicks = long.MinValue / 2;

        public ProgressReporter Nest(int nestMin, int nestMax) =>
            new(report, nestMin, nestMax);

        public void Status(string text) =>
            report?.Invoke(percentMin, text);

        public void Report(int done, int total, string text) =>
            ReportCore(done, total, text, force: false);

        /// <summary>Обновить сразу (перед/после SQL-пакета), без троттлинга.</summary>
        public void ReportNow(int done, int total, string text) =>
            ReportCore(done, total, text, force: true);

        private void ReportCore(int done, int total, string text, bool force)
        {
            if (report is null)
            {
                return;
            }

            var now = Environment.TickCount64;
            var isBoundary = done <= 0 || total <= 0 || done >= total;
            if (!force && !isBoundary && now - _lastReportTicks < MinIntervalMs)
            {
                return;
            }

            _lastReportTicks = now;
            var percent = percentMin;
            if (total > 0)
            {
                var t = Math.Clamp(done / (double)total, 0, 1);
                percent = percentMin + (int)((percentMax - percentMin) * t);
            }

            report(percent, text);
        }
    }

    #endregion
}
