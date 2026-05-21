using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Resources.CustomComparers.SnkComparers;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41;

public partial class ExcelExportCheckPairingOfCode41AsyncCommand
{
    private static List<Operation41PairingDto> GetUnpairedOperations(
        List<Operation41PairingDto> source,
        List<Operation41PairingDto> reference,
        Operation41PairingProfile profile,
        Func<Operation41PairingDto, Operation41PairingKey> toSourceKey,
        Func<Operation41PairingDto, Operation41PairingKey> toReferenceKey) =>
        Operation41PairingMatcher.FindUnpaired(source, reference, profile, toSourceKey, toReferenceKey);

    private static Operation41PairingKey ToPairingKey11(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm11(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.Type,
            dto.Radionuclids,
            dto.CreationDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackNumber,
            dto.Quantity);

    private static Operation41PairingKey ToPairingKey15(Operation41PairingDto dto) =>
        ToPairingKey11(dto);

    private static Operation41PairingKey ToPairingKey12(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm12(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey13(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm13(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.FacNum,
            dto.Type,
            dto.Radionuclids,
            dto.CreationDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey14(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm14(
            dto.OpCode,
            dto.OpDate,
            dto.PasNum,
            dto.Radionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey16For12(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For12(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber,
            dto.ActivityMeasurementDate);

    private static Operation41PairingKey ToPairingKey16For13(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For13(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static Operation41PairingKey ToPairingKey16For14(Operation41PairingDto dto) =>
        Operation41PairingKeyMapping.FromForm16For14(
            dto.OpCode,
            dto.OpDate,
            dto.MainRadionuclids,
            dto.ActivityMeasurementDate,
            dto.DocumentVid,
            dto.DocumentNumber,
            dto.DocumentDate,
            dto.PackName,
            dto.PackType,
            dto.PackNumber);

    private static async Task<List<Operation41PairingDto>> LoadOperation41ListAsync(
        DBModel db, int repsId, string formNum, CancellationToken cancellationToken)
    {
        var operations = formNum switch
        {
            "1.1" => await LoadForm11OperationsAsync(db, repsId, cancellationToken),
            "1.2" => await LoadForm12OperationsAsync(db, repsId, cancellationToken),
            "1.3" => await LoadForm13OperationsAsync(db, repsId, cancellationToken),
            "1.4" => await LoadForm14OperationsAsync(db, repsId, cancellationToken),
            "1.5" => await LoadForm15OperationsAsync(db, repsId, cancellationToken),
            "1.6" => await LoadForm16OperationsAsync(db, repsId, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return operations
            .Where(form => string.Equals(form.OpCode.Trim(), OperationCode, StringComparison.Ordinal))
            .ToList();
    }

    private static Task<List<Operation41PairingDto>> LoadForm11OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.1")
                .SelectMany(rep => rep.Rows11))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                CreationDate = form.CreationDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackNumber = form.PackNumber_DB,
                Quantity = form.Quantity_DB
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm12OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.2")
                .SelectMany(rep => rep.Rows12))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm13OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.3")
                .SelectMany(rep => rep.Rows13))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                CreationDate = form.CreationDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm14OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.4")
                .SelectMany(rep => rep.Rows14))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                Radionuclids = form.Radionuclids_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm15OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.5")
                .SelectMany(rep => rep.Rows15))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                PasNum = form.PassportNumber_DB,
                FacNum = form.FactoryNumber_DB,
                Type = form.Type_DB,
                Radionuclids = form.Radionuclids_DB,
                CreationDate = form.CreationDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackNumber = form.PackNumber_DB,
                Quantity = form.Quantity_DB
            })
            .ToListAsync(cancellationToken);

    private static Task<List<Operation41PairingDto>> LoadForm16OperationsAsync(
        DBModel db, int repsId, CancellationToken cancellationToken) =>
        db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == repsId)
            .SelectMany(reps => reps.Report_Collection
                .Where(rep => rep.FormNum_DB == "1.6")
                .SelectMany(rep => rep.Rows16))
            .Where(form => form.OperationCode_DB == OperationCode)
            .Select(form => new Operation41PairingDto
            {
                Id = form.Id,
                ReportId = form.ReportId ?? 0,
                OpCode = form.OperationCode_DB,
                OpDate = form.OperationDate_DB,
                MainRadionuclids = form.MainRadionuclids_DB,
                Volume = form.Volume_DB,
                Mass = form.Mass_DB,
                ActivityMeasurementDate = form.ActivityMeasurementDate_DB,
                DocumentVid = form.DocumentVid_DB,
                DocumentNumber = form.DocumentNumber_DB,
                DocumentDate = form.DocumentDate_DB,
                PackName = form.PackName_DB,
                PackType = form.PackType_DB,
                PackNumber = form.PackNumber_DB
            })
            .ToListAsync(cancellationToken);

    private static async Task<Reports> BuildReportsForExportAsync(
        DBModel db,
        Reports masterReports,
        List<Operation41PairingDto> unpairedOperations,
        string formNum,
        CancellationToken cancellationToken)
    {
        var result = new Reports { Master = masterReports.Master };

        if (unpairedOperations.Count == 0)
        {
            return result;
        }

        var formIds = unpairedOperations.Select(operation => operation.Id).Distinct().ToList();
        var reportIds = unpairedOperations
            .Where(operation => operation.ReportId != 0)
            .Select(operation => operation.ReportId)
            .Distinct()
            .ToList();

        if (reportIds.Count == 0)
        {
            return result;
        }

        var reports = await LoadReportsByIdsAsync(db, reportIds, cancellationToken);

        switch (formNum)
        {
            case "1.1":
            {
                var forms = await LoadForm11ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows11, (report, form) => report.Rows11.Add(form));
                break;
            }
            case "1.2":
            {
                var forms = await LoadForm12ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows12, (report, form) => report.Rows12.Add(form));
                break;
            }
            case "1.3":
            {
                var forms = await LoadForm13ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows13, (report, form) => report.Rows13.Add(form));
                break;
            }
            case "1.4":
            {
                var forms = await LoadForm14ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows14, (report, form) => report.Rows14.Add(form));
                break;
            }
            case "1.5":
            {
                var forms = await LoadForm15ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows15, (report, form) => report.Rows15.Add(form));
                break;
            }
            case "1.6":
            {
                var forms = await LoadForm16ByIdsAsync(db, formIds, cancellationToken);
                AttachFormsToReports(reports, forms, report => report.Rows16, (report, form) => report.Rows16.Add(form));
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null);
        }

        foreach (var report in OrderReportsForExport(reports))
        {
            if (ReportHasUnpairedRows(report, formNum))
            {
                result.Report_Collection.Add(report);
            }
        }

        return result;
    }

    private static void AttachFormsToReports<TForm>(
        List<Report> reports,
        List<TForm> forms,
        Func<Report, ICollection<TForm>> getRows,
        Action<Report, TForm> addRow)
        where TForm : Form1
    {
        var formsByReportId = forms
            .GroupBy(form => form.ReportId ?? 0)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(form => form.NumberInOrder_DB).ToList());

        foreach (var report in OrderReportsForExport(reports))
        {
            if (!formsByReportId.TryGetValue(report.Id, out var rowList) || rowList.Count == 0)
            {
                continue;
            }

            getRows(report).Clear();
            foreach (var form in rowList)
            {
                addRow(report, form);
            }
        }
    }

    private static async Task<List<Report>> LoadReportsByIdsAsync(
        DBModel db,
        IReadOnlyList<int> reportIds,
        CancellationToken cancellationToken)
    {
        var reports = new List<Report>();
        foreach (var idChunk in ChunkIds(reportIds))
        {
            var batch = await db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(rep => idChunk.Contains(rep.Id))
                .ToListAsync(cancellationToken);
            reports.AddRange(batch);
        }

        return reports;
    }

    private static async Task<List<Form11>> LoadForm11ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_11, formIds, cancellationToken);

    private static async Task<List<Form12>> LoadForm12ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_12, formIds, cancellationToken);

    private static async Task<List<Form13>> LoadForm13ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_13, formIds, cancellationToken);

    private static async Task<List<Form14>> LoadForm14ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_14, formIds, cancellationToken);

    private static async Task<List<Form15>> LoadForm15ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_15, formIds, cancellationToken);

    private static async Task<List<Form16>> LoadForm16ByIdsAsync(
        DBModel db, IReadOnlyList<int> formIds, CancellationToken cancellationToken) =>
        await LoadFormsByIdsAsync(db.form_16, formIds, cancellationToken);

    private static async Task<List<TForm>> LoadFormsByIdsAsync<TForm>(
        DbSet<TForm> dbSet,
        IReadOnlyList<int> formIds,
        CancellationToken cancellationToken)
        where TForm : Form1
    {
        var forms = new List<TForm>();
        foreach (var idChunk in ChunkIds(formIds))
        {
            var batch = await dbSet
                .AsNoTracking()
                .Where(form => idChunk.Contains(form.Id))
                .ToListAsync(cancellationToken);
            forms.AddRange(batch);
        }

        return forms;
    }

    private static IEnumerable<List<int>> ChunkIds(IReadOnlyList<int> ids)
    {
        for (var offset = 0; offset < ids.Count; offset += FirebirdInListMaxCount)
        {
            yield return ids.Skip(offset).Take(FirebirdInListMaxCount).ToList();
        }
    }

    private static bool ReportHasUnpairedRows(Report report, string formNum) =>
        formNum switch
        {
            "1.1" => report.Rows11.Count > 0,
            "1.2" => report.Rows12.Count > 0,
            "1.3" => report.Rows13.Count > 0,
            "1.4" => report.Rows14.Count > 0,
            "1.5" => report.Rows15.Count > 0,
            "1.6" => report.Rows16.Count > 0,
            _ => false
        };

    private static IEnumerable<Report> OrderReportsForExport(List<Report> reports) =>
        reports
            .OrderBy(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var startDate) ? startDate : DateOnly.MaxValue)
            .ThenBy(rep => DateOnly.TryParse(rep.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue);

    private sealed class Operation41PairingDto
    {
        public int Id { get; init; }
        public int ReportId { get; init; }
        public string OpCode { get; init; } = string.Empty;
        public string OpDate { get; init; } = string.Empty;
        public string PasNum { get; init; } = string.Empty;
        public string FacNum { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Radionuclids { get; init; } = string.Empty;
        public string CreationDate { get; init; } = string.Empty;
        public byte? DocumentVid { get; init; }
        public string DocumentNumber { get; init; } = string.Empty;
        public string DocumentDate { get; init; } = string.Empty;
        public string PackNumber { get; init; } = string.Empty;
        public string PackName { get; init; } = string.Empty;
        public string PackType { get; init; } = string.Empty;
        public string MainRadionuclids { get; init; } = string.Empty;
        public string Mass { get; init; } = string.Empty;
        public string Volume { get; init; } = string.Empty;
        public string ActivityMeasurementDate { get; init; } = string.Empty;
        public int? Quantity { get; init; }
    }
}
