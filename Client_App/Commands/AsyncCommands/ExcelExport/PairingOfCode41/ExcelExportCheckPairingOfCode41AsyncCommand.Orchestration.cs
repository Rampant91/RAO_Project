using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using OfficeOpenXml;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Mode / export model

    private const string WholeDbParameter = "All";

    private static bool IsWholeDbMode(object? parameter) =>
        parameter is string s && string.Equals(s, WholeDbParameter, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Результат проверки одной организации: данные для дописывания в Excel (null = непарных нет).
    /// </summary>
    private sealed class OrganizationPairingExport
    {
        public required Reports Form11 { get; init; }
        public required Reports Form12 { get; init; }
        public required Reports Form13 { get; init; }
        public required Reports Form14 { get; init; }
        public required Reports Form15 { get; init; }
        public required Reports Form16 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm13 { get; init; }
        public required List<Operation41PairingDto> UnpairedForm14 { get; init; }
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
            db, selectedReports.Id, pairingParams, cts.Token, Status);

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
        var candidates = await LoadOrganizationsWithOperation41Async(db, cts.Token);
        if (candidates.Count == 0)
        {
            await ShowNoUnpairedOperationsMessage(progressBar);
            await CleanupAndClose(progressBar, tmpDbPath);
            return;
        }

        progressBarVM.SetProgressBar(13, "Загрузка операций 41…", "Вся БД", "Выгрузка в .xlsx");
        var bulkByOrg = await LoadAllOperation41GroupedByRepsIdAsync(
            db,
            pairingParams,
            cts.Token,
            text => progressBarVM.SetProgressBar(13, text, "Вся БД"));

        progressBarVM.SetProgressBar(15, "Инициализация Excel пакета", "Вся БД", "Выгрузка в .xlsx");
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
            var percentBase = 15 + (int)(75.0 * i / total);
            var percentSpan = Math.Max(1, (int)(75.0 / total));

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
                org.Id,
                loaded,
                pairingParams,
                cts.Token,
                (p, text) => Status(Math.Min(percentSpan - 1, p / 5), text),
                masterReportsHint: org);

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
        DBModel db, CancellationToken cancellationToken)
    {
        var repsIds = new HashSet<int>();
        await AddRepsIdsWithCode41Async(db.form_11, repsIds, cancellationToken);
        await AddRepsIdsWithCode41Async(db.form_12, repsIds, cancellationToken);
        await AddRepsIdsWithCode41Async(db.form_13, repsIds, cancellationToken);
        await AddRepsIdsWithCode41Async(db.form_14, repsIds, cancellationToken);
        await AddRepsIdsWithCode41Async(db.form_15, repsIds, cancellationToken);
        await AddRepsIdsWithCode41Async(db.form_16, repsIds, cancellationToken);

        if (repsIds.Count == 0)
        {
            return [];
        }

        var orderedIds = repsIds.OrderBy(id => id).ToList();
        var list = new List<Reports>(orderedIds.Count);
        foreach (var idChunk in ChunkIds(orderedIds))
        {
            var batch = await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(reps => reps.Master_DB)
                .ThenInclude(master => master.Rows10)
                .Where(reps => reps.DBObservable != null && idChunk.Contains(reps.Id))
                .ToListAsync(cancellationToken);
            list.AddRange(batch);
        }

        return list
            .OrderBy(reps => reps.Master_DB.RegNoRep.Value)
            .ThenBy(reps => reps.Master_DB.OkpoRep.Value)
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
    /// Пайплайн одной org: загрузка DTO → сопоставление → closest → строки Excel.
    /// </summary>
    private async Task<OrganizationPairingExport?> BuildOrganizationPairingExportAsync(
        DBModel db,
        int repsId,
        PairingParamsSet pairingParams,
        CancellationToken cancellationToken,
        Action<int, string>? reportProgress = null,
        Reports? masterReportsHint = null)
    {
        var p11 = pairingParams.Pairing11To15;

        reportProgress?.Invoke(18, "загрузка 1.1");
        var form11 = await LoadOperation41ListAsync(db, repsId, "1.1", cancellationToken, p11);
        reportProgress?.Invoke(22, "загрузка 1.2");
        var form12 = await LoadOperation41ListAsync(db, repsId, "1.2", cancellationToken);
        reportProgress?.Invoke(26, "загрузка 1.3");
        var form13 = await LoadOperation41ListAsync(db, repsId, "1.3", cancellationToken);
        reportProgress?.Invoke(30, "загрузка 1.4");
        var form14 = await LoadOperation41ListAsync(db, repsId, "1.4", cancellationToken);
        reportProgress?.Invoke(34, "загрузка 1.5");
        var form15 = await LoadOperation41ListAsync(db, repsId, "1.5", cancellationToken, p11);
        reportProgress?.Invoke(38, "загрузка 1.6");
        var form16 = await LoadOperation41ListAsync(db, repsId, "1.6", cancellationToken);

        return await BuildOrganizationPairingExportFromLoadedAsync(
            db,
            repsId,
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
            reportProgress,
            masterReportsHint);
    }

    /// <summary>
    /// Сопоставление + Excel-данные из уже загруженных DTO (org-режим и whole-DB после bulk).
    /// </summary>
    private async Task<OrganizationPairingExport?> BuildOrganizationPairingExportFromLoadedAsync(
        DBModel db,
        int repsId,
        OrgOperation41Lists loaded,
        PairingParamsSet pairingParams,
        CancellationToken cancellationToken,
        Action<int, string>? reportProgress = null,
        Reports? masterReportsHint = null)
    {
        var p11 = pairingParams.Pairing11To15;
        var p12 = pairingParams.Pairing12To16;
        var p13 = pairingParams.Pairing13To16;
        var p14 = pairingParams.Pairing14To16;

        reportProgress?.Invoke(42, "сопоставление");
        var unpaired = ComputeOrganizationUnpaired(
            loaded.Form11, loaded.Form12, loaded.Form13, loaded.Form14, loaded.Form15, loaded.Form16,
            pairingParams);

        if (unpaired.Form11.Count == 0 && unpaired.Form15.Count == 0
            && unpaired.Form12.Count == 0 && unpaired.Form13.Count == 0
            && unpaired.Form14.Count == 0 && unpaired.Form16.Count == 0)
        {
            return null;
        }

        reportProgress?.Invoke(55, "closest-match");
        _form11ClosestMatchHighlights = BuildClosestMatchHighlights(unpaired.Form11, loaded.Form15, p11);
        _form15ClosestMatchHighlights = BuildClosestMatchHighlights(unpaired.Form15, loaded.Form11, p11);
        _form12ClosestMatchHighlights = BuildClosestMatchHighlights12To16(unpaired.Form12, loaded.Form16, p12);
        _form13ClosestMatchHighlights = BuildClosestMatchHighlights13To16(unpaired.Form13, loaded.Form16, p13);
        _form14ClosestMatchHighlights = BuildClosestMatchHighlights14To16(unpaired.Form14, loaded.Form16, p14);
        _form16ClosestMatchHighlights = BuildClosestMatchHighlights16(
            unpaired.Form16, loaded.Form12, loaded.Form13, loaded.Form14, p12, p13, p14);

        reportProgress?.Invoke(65, "загрузка организации");
        var masterReports = masterReportsHint is { Id: var hintId } && hintId == repsId
            ? masterReportsHint
            : await db.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(reps => reps.Master_DB)
                .ThenInclude(master => master.Rows10)
                .FirstAsync(reps => reps.Id == repsId, cancellationToken);

        reportProgress?.Invoke(70, "загрузка непарных строк");
        var reports11 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form11, "1.1", cancellationToken);
        var reports12 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form12, "1.2", cancellationToken);
        var reports13 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form13, "1.3", cancellationToken);
        var reports14 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form14, "1.4", cancellationToken);
        var reports15 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form15, "1.5", cancellationToken);
        var reports16 = await BuildReportsForExportAsync(db, masterReports, unpaired.Form16, "1.6", cancellationToken);

        return new OrganizationPairingExport
        {
            Form11 = reports11,
            Form12 = reports12,
            Form13 = reports13,
            Form14 = reports14,
            Form15 = reports15,
            Form16 = reports16,
            UnpairedForm13 = unpaired.Form13,
            UnpairedForm14 = unpaired.Form14
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

        return new OrganizationUnpairedSets(
            GetUnpairedOperations11To15(form11, form15, p11),
            GetUnpairedOperations12To16(form12, form16, p12),
            GetUnpairedOperations13To16(form13, form16, p13),
            GetUnpairedOperations14To16(form14, form16, p14),
            GetUnpairedOperations11To15(form15Code41Only, form11, p11),
            GetUnpairedForm16(form16, form12, form13, form14, p12, p13, p14));
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
}
