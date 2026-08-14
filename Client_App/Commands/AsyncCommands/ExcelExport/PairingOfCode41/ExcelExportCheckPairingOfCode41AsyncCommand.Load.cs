using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    #region Load DTO

    /// <summary>
    /// Загрузка операций 41 для формы. <paramref name="repsId"/> = null — whole-DB (keyset-пагинация).
    /// </summary>
    private static Task<List<Operation41PairingDto>> LoadOperation41ListAsync(
        DBModel db,
        int? repsId,
        string formNum,
        CancellationToken cancellationToken,
        Pairing11To15Params? pairing11To15Params = null,
        ProgressReporter? progress = null) =>
        repsId is int orgId
            ? LoadOperation41ForRepsAsync(db, orgId, formNum, cancellationToken, pairing11To15Params, progress)
            : LoadOperation41PagedWholeDbAsync(db, formNum, cancellationToken, pairing11To15Params, progress);

    private static async Task<List<Operation41PairingDto>> LoadOperation41ForRepsAsync(
        DBModel db,
        int repsId,
        string formNum,
        CancellationToken cancellationToken,
        Pairing11To15Params? pairing11To15Params,
        ProgressReporter? progress)
    {
        progress?.ReportNow(0, 1, $"форма {formNum}: загрузка операций…");
        var operations = formNum switch
        {
            "1.1" => await LoadForm11ForRepsAsync(db, repsId, cancellationToken, pairing11To15Params),
            "1.2" => await LoadForm12ForRepsAsync(db, repsId, cancellationToken),
            "1.3" => await LoadForm13ForRepsAsync(db, repsId, cancellationToken),
            "1.4" => await LoadForm14ForRepsAsync(db, repsId, cancellationToken),
            "1.5" => await LoadForm15ForRepsAsync(db, repsId, cancellationToken, pairing11To15Params),
            "1.6" => await LoadForm16ForRepsAsync(db, repsId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        progress?.ReportNow(1, 1, $"форма {formNum}: загружено {operations.Count} строк");
        return OrderOperation41List(formNum, operations);
    }

    private static async Task<List<Operation41PairingDto>> LoadOperation41PagedWholeDbAsync(
        DBModel db,
        string formNum,
        CancellationToken cancellationToken,
        Pairing11To15Params? pairing11To15Params,
        ProgressReporter? progress)
    {
        var operations = formNum switch
        {
            "1.1" => await LoadForm11PagedWholeDbAsync(db, cancellationToken, progress, pairing11To15Params),
            "1.2" => await LoadForm12PagedWholeDbAsync(db, cancellationToken, progress),
            "1.3" => await LoadForm13PagedWholeDbAsync(db, cancellationToken, progress),
            "1.4" => await LoadForm14PagedWholeDbAsync(db, cancellationToken, progress),
            "1.5" => await LoadForm15PagedWholeDbAsync(db, cancellationToken, progress, pairing11To15Params),
            "1.6" => await LoadForm16PagedWholeDbAsync(db, cancellationToken, progress),
            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return OrderOperation41List(formNum, operations);
    }

    private static List<Operation41PairingDto> OrderOperation41List(string formNum, List<Operation41PairingDto> operations) =>
        operations
            .Where(form => formNum == "1.5"
                ? IsForm15PairingCandidateOpCode(form.OpCode)
                : string.Equals(form.OpCode.Trim(), OperationCode, StringComparison.Ordinal))
            .OrderBy(form => form.Id)
            .ToList();

    private static int WholeDbPageSize() =>
        WholeDbOpsPageSize > 0 ? WholeDbOpsPageSize : 2000;

    #region Form 1.1

    private static async Task<List<Operation41PairingDto>> LoadForm11ForRepsAsync(
        DBModel db,
        int repsId,
        CancellationToken cancellationToken,
        Pairing11To15Params? options)
    {
        var rows = await db.form_11
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB == OperationCode)
            .Select(form => new Form11Row(
                form.Id,
                form.Report!.Reports.Id,
                form.ReportId ?? 0,
                form.OperationCode_DB,
                form.OperationDate_DB,
                form.PassportNumber_DB,
                form.FactoryNumber_DB,
                form.Type_DB,
                form.Radionuclids_DB,
                form.CreationDate_DB,
                form.DocumentVid_DB,
                form.DocumentNumber_DB,
                form.DocumentDate_DB,
                form.ProviderOrRecieverOKPO_DB,
                form.TransporterOKPO_DB,
                form.PackName_DB,
                form.PackType_DB,
                form.PackNumber_DB,
                form.Activity_DB,
                form.Quantity_DB,
                form.NumberInOrder_DB,
                form.Report.StartPeriod_DB,
                form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

        return rows.Select(row => MapForm11Row(row, options)).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm11PagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        Pairing11To15Params? options)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма 1.1: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма 1.1: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_11
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB == OperationCode)
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form11Row(
                    form.Id,
                    form.Report!.Reports.Id,
                    form.ReportId ?? 0,
                    form.OperationCode_DB,
                    form.OperationDate_DB,
                    form.PassportNumber_DB,
                    form.FactoryNumber_DB,
                    form.Type_DB,
                    form.Radionuclids_DB,
                    form.CreationDate_DB,
                    form.DocumentVid_DB,
                    form.DocumentNumber_DB,
                    form.DocumentDate_DB,
                    form.ProviderOrRecieverOKPO_DB,
                    form.TransporterOKPO_DB,
                    form.PackName_DB,
                    form.PackType_DB,
                    form.PackNumber_DB,
                    form.Activity_DB,
                    form.Quantity_DB,
                    form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB,
                    form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                result.Add(MapForm11Row(row, options));
            }

            ReportPageLoaded(progress, "1.1", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private sealed record Form11Row(
        int Id,
        int RepsId,
        int ReportId,
        string OpCode,
        string OpDate,
        string PasNum,
        string FacNum,
        string Type,
        string Radionuclids,
        string CreationDate,
        byte? DocumentVid,
        string DocumentNumber,
        string DocumentDate,
        string ProviderOrRecieverOkpo,
        string TransporterOkpo,
        string PackName,
        string PackType,
        string PackNumber,
        string Activity,
        int? Quantity,
        int? NumberInOrder,
        string? StartPeriod,
        string? EndPeriod);

    private static Operation41PairingDto MapForm11Row(Form11Row row, Pairing11To15Params? options) =>
        new()
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = options == null || options.CheckOperationDate ? row.OpDate : string.Empty,
            PasNum = options == null || options.CheckPassportNumber ? row.PasNum : string.Empty,
            FacNum = options == null || options.CheckFactoryNumber ? row.FacNum : string.Empty,
            Type = options == null || options.CheckType ? row.Type : string.Empty,
            Radionuclids = options == null || options.CheckRadionuclids ? row.Radionuclids : string.Empty,
            CreationDate = options == null || options.CheckCreationDate ? row.CreationDate : string.Empty,
            DocumentVid = options == null || options.CheckDocumentVid ? row.DocumentVid : null,
            DocumentNumber = options == null || options.CheckDocumentNumber ? row.DocumentNumber : string.Empty,
            DocumentDate = options == null || options.CheckDocumentDate ? row.DocumentDate : string.Empty,
            ProviderOrRecieverOkpo = options == null || options.CheckProviderOrRecieverOkpo
                ? row.ProviderOrRecieverOkpo
                : string.Empty,
            TransporterOkpo = options == null || options.CheckTransporterOkpo ? row.TransporterOkpo : string.Empty,
            PackName = options == null || options.CheckPackName ? row.PackName : string.Empty,
            PackType = options == null || options.CheckPackType ? row.PackType : string.Empty,
            PackNumber = options == null || options.CheckPackNumber ? row.PackNumber : string.Empty,
            Activity = options == null || options.CheckActivity ? row.Activity : string.Empty,
            Quantity = options == null || options.CheckQuantity ? row.Quantity : null,
            FormNum = "1.1",
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };

    #endregion

    #region Form 1.2

    private sealed record Form12Row(
        int Id,
        int RepsId,
        int ReportId,
        string OpCode,
        string OpDate,
        string Mass,
        byte? DocumentVid,
        string DocumentNumber,
        string DocumentDate,
        string PackName,
        string PackType,
        string PackNumber,
        int? NumberInOrder,
        string? StartPeriod,
        string? EndPeriod);

    private static async Task<List<Operation41PairingDto>> LoadForm12ForRepsAsync(
        DBModel db,
        int repsId,
        CancellationToken cancellationToken)
    {
        var rows = await db.form_12
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB == OperationCode)
            .Select(form => new Form12Row(
                form.Id,
                form.Report!.Reports.Id,
                form.ReportId ?? 0,
                form.OperationCode_DB,
                form.OperationDate_DB,
                form.Mass_DB,
                form.DocumentVid_DB,
                form.DocumentNumber_DB,
                form.DocumentDate_DB,
                form.PackName_DB,
                form.PackType_DB,
                form.PackNumber_DB,
                form.NumberInOrder_DB,
                form.Report.StartPeriod_DB,
                form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

        return rows.Select(MapForm12Row).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm12PagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма 1.2: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма 1.2: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_12
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB == OperationCode)
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form12Row(
                    form.Id,
                    form.Report!.Reports.Id,
                    form.ReportId ?? 0,
                    form.OperationCode_DB,
                    form.OperationDate_DB,
                    form.Mass_DB,
                    form.DocumentVid_DB,
                    form.DocumentNumber_DB,
                    form.DocumentDate_DB,
                    form.PackName_DB,
                    form.PackType_DB,
                    form.PackNumber_DB,
                    form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB,
                    form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            result.AddRange(rows.Select(MapForm12Row));
            ReportPageLoaded(progress, "1.2", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    private static Operation41PairingDto MapForm12Row(Form12Row row)
    {
        var massTon = ToMassTon(row.Mass);
        return new Operation41PairingDto
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = row.OpDate,
            Mass = massTon,
            BetaGammaActivity = ComputeFromMass(massTon, 25_000_000_000d),
            AlphaActivity = ComputeFromMass(massTon, 16_100_000_000d),
            ActivityMeasurementDate = row.OpDate,
            DocumentVid = row.DocumentVid,
            DocumentNumber = row.DocumentNumber,
            DocumentDate = row.DocumentDate,
            PackName = row.PackName,
            PackType = row.PackType,
            PackNumber = row.PackNumber,
            FormNum = "1.2",
            CodeRao = RaoCodeHelper.Form12CodeRao,
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };
    }

    #endregion

    #region Form 1.3 / 1.4 — records + mappers (abbreviated pattern)

    private sealed record Form13Row(
        int Id, int RepsId, int ReportId, string OpCode, string OpDate,
        string Radionuclids, string Activity, string CreationDate,
        byte? DocumentVid, string DocumentNumber, string DocumentDate,
        string PackName, string PackType, string PackNumber,
        byte? AggregateState, int? NumberInOrder, string? StartPeriod, string? EndPeriod);

    private static async Task<List<Operation41PairingDto>> LoadForm13ForRepsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        (await QueryForm13RowsForReps(db, repsId, cancellationToken)).Select(MapForm13Row).ToList();

    private static Task<List<Form13Row>> QueryForm13RowsForReps(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.form_13
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB == OperationCode)
            .Select(form => new Form13Row(
                form.Id, form.Report!.Reports.Id, form.ReportId ?? 0,
                form.OperationCode_DB, form.OperationDate_DB,
                form.Radionuclids_DB, form.Activity_DB, form.CreationDate_DB,
                form.DocumentVid_DB, form.DocumentNumber_DB, form.DocumentDate_DB,
                form.PackName_DB, form.PackType_DB, form.PackNumber_DB,
                form.AggregateState_DB, form.NumberInOrder_DB,
                form.Report.StartPeriod_DB, form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

    private static async Task<List<Operation41PairingDto>> LoadForm13PagedWholeDbAsync(
        DBModel db, CancellationToken cancellationToken, ProgressReporter? progress) =>
        await LoadForm13PagedCore(db, cancellationToken, progress);

    private static async Task<List<Operation41PairingDto>> LoadForm13PagedCore(
        DBModel db, CancellationToken cancellationToken, ProgressReporter? progress)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;
        progress?.ReportNow(0, 1, $"форма 1.3: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(result.Count, result.Count + pageSize,
                $"форма 1.3: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_13
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB == OperationCode)
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form13Row(
                    form.Id, form.Report!.Reports.Id, form.ReportId ?? 0,
                    form.OperationCode_DB, form.OperationDate_DB,
                    form.Radionuclids_DB, form.Activity_DB, form.CreationDate_DB,
                    form.DocumentVid_DB, form.DocumentNumber_DB, form.DocumentDate_DB,
                    form.PackName_DB, form.PackType_DB, form.PackNumber_DB,
                    form.AggregateState_DB, form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB, form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0) break;
            lastId = rows[^1].Id;
            result.AddRange(rows.Select(MapForm13Row));
            ReportPageLoaded(progress, "1.3", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize) break;
        }

        return result;
    }

    private static Operation41PairingDto MapForm13Row(Form13Row row)
    {
        var activities = GetActivitiesForExport(row.Radionuclids, row.Activity);
        return new Operation41PairingDto
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = row.OpDate,
            Radionuclids = row.Radionuclids,
            CreationDate = row.CreationDate,
            MainRadionuclids = row.Radionuclids,
            TritiumActivity = activities["tritium"],
            BetaGammaActivity = activities["beta"],
            AlphaActivity = activities["alpha"],
            TransuraniumActivity = activities["transuranium"],
            ActivityMeasurementDate = row.CreationDate,
            DocumentVid = row.DocumentVid,
            DocumentNumber = row.DocumentNumber,
            DocumentDate = row.DocumentDate,
            PackName = row.PackName,
            PackType = row.PackType,
            PackNumber = row.PackNumber,
            FormNum = "1.3",
            AggregateState = row.AggregateState,
            CodeRao = RaoCodeHelper.ComputeCodeRaoFromForm13(row.Radionuclids, row.AggregateState),
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };
    }

    private sealed record Form14Row(
        int Id, int RepsId, int ReportId, string OpCode, string OpDate,
        string Radionuclids, string Activity, string Volume, string Mass,
        string ActivityMeasurementDate,
        byte? DocumentVid, string DocumentNumber, string DocumentDate,
        string PackName, string PackType, string PackNumber,
        byte? AggregateState, int? NumberInOrder, string? StartPeriod, string? EndPeriod);

    private static async Task<List<Operation41PairingDto>> LoadForm14ForRepsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        (await QueryForm14RowsForReps(db, repsId, cancellationToken)).Select(MapForm14Row).ToList();

    private static Task<List<Form14Row>> QueryForm14RowsForReps(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.form_14
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB == OperationCode)
            .Select(form => new Form14Row(
                form.Id, form.Report!.Reports.Id, form.ReportId ?? 0,
                form.OperationCode_DB, form.OperationDate_DB,
                form.Radionuclids_DB, form.Activity_DB, form.Volume_DB, form.Mass_DB,
                form.ActivityMeasurementDate_DB,
                form.DocumentVid_DB, form.DocumentNumber_DB, form.DocumentDate_DB,
                form.PackName_DB, form.PackType_DB, form.PackNumber_DB,
                form.AggregateState_DB, form.NumberInOrder_DB,
                form.Report.StartPeriod_DB, form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm14PagedWholeDbAsync(
        DBModel db, CancellationToken cancellationToken, ProgressReporter? progress) =>
        LoadForm14PagedCore(db, cancellationToken, progress);

    private static async Task<List<Operation41PairingDto>> LoadForm14PagedCore(
        DBModel db, CancellationToken cancellationToken, ProgressReporter? progress)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;
        progress?.ReportNow(0, 1, $"форма 1.4: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(result.Count, result.Count + pageSize,
                $"форма 1.4: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_14
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB == OperationCode)
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form14Row(
                    form.Id, form.Report!.Reports.Id, form.ReportId ?? 0,
                    form.OperationCode_DB, form.OperationDate_DB,
                    form.Radionuclids_DB, form.Activity_DB, form.Volume_DB, form.Mass_DB,
                    form.ActivityMeasurementDate_DB,
                    form.DocumentVid_DB, form.DocumentNumber_DB, form.DocumentDate_DB,
                    form.PackName_DB, form.PackType_DB, form.PackNumber_DB,
                    form.AggregateState_DB, form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB, form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0) break;
            lastId = rows[^1].Id;
            result.AddRange(rows.Select(MapForm14Row));
            ReportPageLoaded(progress, "1.4", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize) break;
        }

        return result;
    }

    private static Operation41PairingDto MapForm14Row(Form14Row row)
    {
        var activities = GetActivitiesForExport(row.Radionuclids, row.Activity);
        return new Operation41PairingDto
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = row.OpDate,
            Radionuclids = row.Radionuclids,
            MainRadionuclids = row.Radionuclids,
            Volume = row.Volume,
            Mass = ToMassTon(row.Mass),
            TritiumActivity = activities["tritium"],
            BetaGammaActivity = activities["beta"],
            AlphaActivity = activities["alpha"],
            TransuraniumActivity = activities["transuranium"],
            ActivityMeasurementDate = row.ActivityMeasurementDate,
            DocumentVid = row.DocumentVid,
            DocumentNumber = row.DocumentNumber,
            DocumentDate = row.DocumentDate,
            PackName = row.PackName,
            PackType = row.PackType,
            PackNumber = row.PackNumber,
            FormNum = "1.4",
            AggregateState = row.AggregateState,
            CodeRao = RaoCodeHelper.ComputeCodeRaoFromForm14(row.Radionuclids, row.AggregateState),
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };
    }

    #endregion

    #region Form 1.5

    private sealed record Form15Row(
        int Id,
        int RepsId,
        int ReportId,
        string OpCode,
        string OpDate,
        string PasNum,
        string FacNum,
        string Type,
        string Radionuclids,
        string CreationDate,
        byte? DocumentVid,
        string DocumentNumber,
        string DocumentDate,
        string ProviderOrRecieverOkpo,
        string TransporterOkpo,
        string PackName,
        string PackType,
        string PackNumber,
        string Activity,
        int? Quantity,
        int? NumberInOrder,
        string? StartPeriod,
        string? EndPeriod);

    private static Operation41PairingDto MapForm15Row(Form15Row row, Pairing11To15Params? options) =>
        new()
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = options == null || options.CheckOperationDate ? row.OpDate : string.Empty,
            PasNum = options == null || options.CheckPassportNumber ? row.PasNum : string.Empty,
            FacNum = options == null || options.CheckFactoryNumber ? row.FacNum : string.Empty,
            Type = options == null || options.CheckType ? row.Type : string.Empty,
            Radionuclids = options == null || options.CheckRadionuclids ? row.Radionuclids : string.Empty,
            CreationDate = options == null || options.CheckCreationDate ? row.CreationDate : string.Empty,
            DocumentVid = options == null || options.CheckDocumentVid ? row.DocumentVid : null,
            DocumentNumber = options == null || options.CheckDocumentNumber ? row.DocumentNumber : string.Empty,
            DocumentDate = options == null || options.CheckDocumentDate ? row.DocumentDate : string.Empty,
            ProviderOrRecieverOkpo = options == null || options.CheckProviderOrRecieverOkpo
                ? row.ProviderOrRecieverOkpo
                : string.Empty,
            TransporterOkpo = options == null || options.CheckTransporterOkpo ? row.TransporterOkpo : string.Empty,
            PackName = options == null || options.CheckPackName ? row.PackName : string.Empty,
            PackType = options == null || options.CheckPackType ? row.PackType : string.Empty,
            PackNumber = options == null || options.CheckPackNumber ? row.PackNumber : string.Empty,
            Activity = options == null || options.CheckActivity ? row.Activity : string.Empty,
            Quantity = options == null || options.CheckQuantity ? row.Quantity : null,
            FormNum = "1.5",
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };

    private static async Task<List<Operation41PairingDto>> LoadForm15ForRepsAsync(
        DBModel db,
        int repsId,
        CancellationToken cancellationToken,
        Pairing11To15Params? options)
    {
        var rows = await db.form_15
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && (form.OperationCode_DB == OperationCode
                               || form.OperationCode_DB == Form15ReceiveMistypeOpCode))
            .Select(form => new Form15Row(
                form.Id,
                form.Report!.Reports.Id,
                form.ReportId ?? 0,
                form.OperationCode_DB,
                form.OperationDate_DB,
                form.PassportNumber_DB,
                form.FactoryNumber_DB,
                form.Type_DB,
                form.Radionuclids_DB,
                form.CreationDate_DB,
                form.DocumentVid_DB,
                form.DocumentNumber_DB,
                form.DocumentDate_DB,
                form.ProviderOrRecieverOKPO_DB,
                form.TransporterOKPO_DB,
                form.PackName_DB,
                form.PackType_DB,
                form.PackNumber_DB,
                form.Activity_DB,
                form.Quantity_DB,
                form.NumberInOrder_DB,
                form.Report.StartPeriod_DB,
                form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

        return rows.Select(row => MapForm15Row(row, options)).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm15PagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress,
        Pairing11To15Params? options)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма 1.5: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма 1.5: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_15
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && (form.OperationCode_DB == OperationCode
                                   || form.OperationCode_DB == Form15ReceiveMistypeOpCode))
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form15Row(
                    form.Id,
                    form.Report!.Reports.Id,
                    form.ReportId ?? 0,
                    form.OperationCode_DB,
                    form.OperationDate_DB,
                    form.PassportNumber_DB,
                    form.FactoryNumber_DB,
                    form.Type_DB,
                    form.Radionuclids_DB,
                    form.CreationDate_DB,
                    form.DocumentVid_DB,
                    form.DocumentNumber_DB,
                    form.DocumentDate_DB,
                    form.ProviderOrRecieverOKPO_DB,
                    form.TransporterOKPO_DB,
                    form.PackName_DB,
                    form.PackType_DB,
                    form.PackNumber_DB,
                    form.Activity_DB,
                    form.Quantity_DB,
                    form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB,
                    form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            foreach (var row in rows)
            {
                result.Add(MapForm15Row(row, options));
            }

            ReportPageLoaded(progress, "1.5", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    #endregion

    #region Form 1.6

    private sealed record Form16Row(
        int Id,
        int RepsId,
        int ReportId,
        string OpCode,
        string OpDate,
        string MainRadionuclids,
        string Volume,
        string Mass,
        string TritiumActivity,
        string BetaGammaActivity,
        string AlphaActivity,
        string TransuraniumActivity,
        string ActivityMeasurementDate,
        byte? DocumentVid,
        string DocumentNumber,
        string DocumentDate,
        string PackName,
        string PackType,
        string PackNumber,
        string CodeRao,
        int? NumberInOrder,
        string? StartPeriod,
        string? EndPeriod);

    private static Operation41PairingDto MapForm16Row(Form16Row row) =>
        new()
        {
            Id = row.Id,
            RepsId = row.RepsId,
            ReportId = row.ReportId,
            OpCode = row.OpCode,
            OpDate = row.OpDate,
            MainRadionuclids = row.MainRadionuclids,
            Volume = row.Volume,
            Mass = row.Mass,
            TritiumActivity = row.TritiumActivity,
            BetaGammaActivity = row.BetaGammaActivity,
            AlphaActivity = row.AlphaActivity,
            TransuraniumActivity = row.TransuraniumActivity,
            ActivityMeasurementDate = row.ActivityMeasurementDate,
            DocumentVid = row.DocumentVid,
            DocumentNumber = row.DocumentNumber,
            DocumentDate = row.DocumentDate,
            PackName = row.PackName,
            PackType = row.PackType,
            PackNumber = row.PackNumber,
            CodeRao = row.CodeRao,
            FormNum = "1.6",
            NumberInOrder = row.NumberInOrder,
            StartPeriod = row.StartPeriod ?? string.Empty,
            EndPeriod = row.EndPeriod ?? string.Empty
        };

    private static async Task<List<Operation41PairingDto>> LoadForm16ForRepsAsync(
        DBModel db,
        int repsId,
        CancellationToken cancellationToken)
    {
        var rows = await db.form_16
            .AsNoTracking()
            .Where(form => form.Report != null
                           && form.Report.Reports != null
                           && form.Report.Reports.Id == repsId
                           && form.OperationCode_DB == OperationCode)
            .Select(form => new Form16Row(
                form.Id,
                form.Report!.Reports.Id,
                form.ReportId ?? 0,
                form.OperationCode_DB,
                form.OperationDate_DB,
                form.MainRadionuclids_DB,
                form.Volume_DB,
                form.Mass_DB,
                form.TritiumActivity_DB,
                form.BetaGammaActivity_DB,
                form.AlphaActivity_DB,
                form.TransuraniumActivity_DB,
                form.ActivityMeasurementDate_DB,
                form.DocumentVid_DB,
                form.DocumentNumber_DB,
                form.DocumentDate_DB,
                form.PackName_DB,
                form.PackType_DB,
                form.PackNumber_DB,
                form.CodeRAO_DB,
                form.NumberInOrder_DB,
                form.Report.StartPeriod_DB,
                form.Report.EndPeriod_DB))
            .ToListAsync(cancellationToken);

        return rows.Select(MapForm16Row).ToList();
    }

    private static async Task<List<Operation41PairingDto>> LoadForm16PagedWholeDbAsync(
        DBModel db,
        CancellationToken cancellationToken,
        ProgressReporter? progress)
    {
        var pageSize = WholeDbPageSize();
        var result = new List<Operation41PairingDto>();
        var lastId = 0;
        var page = 0;

        progress?.ReportNow(0, 1, $"форма 1.6: загрузка операций (страницами по {pageSize})…");

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            page++;
            progress?.ReportNow(
                result.Count,
                result.Count + pageSize,
                $"форма 1.6: загружено {result.Count} строк, чтение пакета {page}…");

            var rows = await db.form_16
                .AsNoTracking()
                .Where(form => form.Id > lastId
                               && form.Report != null
                               && form.Report.Reports != null
                               && form.OperationCode_DB == OperationCode)
                .OrderBy(form => form.Id)
                .Take(pageSize)
                .Select(form => new Form16Row(
                    form.Id,
                    form.Report!.Reports.Id,
                    form.ReportId ?? 0,
                    form.OperationCode_DB,
                    form.OperationDate_DB,
                    form.MainRadionuclids_DB,
                    form.Volume_DB,
                    form.Mass_DB,
                    form.TritiumActivity_DB,
                    form.BetaGammaActivity_DB,
                    form.AlphaActivity_DB,
                    form.TransuraniumActivity_DB,
                    form.ActivityMeasurementDate_DB,
                    form.DocumentVid_DB,
                    form.DocumentNumber_DB,
                    form.DocumentDate_DB,
                    form.PackName_DB,
                    form.PackType_DB,
                    form.PackNumber_DB,
                    form.CodeRAO_DB,
                    form.NumberInOrder_DB,
                    form.Report.StartPeriod_DB,
                    form.Report.EndPeriod_DB))
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                break;
            }

            lastId = rows[^1].Id;
            result.AddRange(rows.Select(MapForm16Row));

            ReportPageLoaded(progress, "1.6", result.Count, page, rows.Count, pageSize);
            if (rows.Count < pageSize)
            {
                break;
            }
        }

        return result;
    }

    #endregion

    private static void ReportPageLoaded(
        ProgressReporter? progress,
        string formNum,
        int loaded,
        int page,
        int rowsInPage,
        int pageSize) =>
        progress?.ReportNow(
            loaded,
            rowsInPage < pageSize ? loaded : loaded + pageSize,
            $"форма {formNum}: загружено {loaded} строк (пакет {page})");

    #endregion
}
