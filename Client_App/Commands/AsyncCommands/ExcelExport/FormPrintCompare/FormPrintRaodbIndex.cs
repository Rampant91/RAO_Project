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

internal static class FormPrintRaodbIndex
{
    public sealed class LoadResult
    {
        public required List<CompareReportDto> Reports { get; init; }
        public required string RegNo { get; init; }
        public required string Okpo { get; init; }
    }

    public static async Task<LoadResult> LoadAsync(string dbPath, CancellationToken cancellationToken)
    {
        await using var db = new DBModel(dbPath);
        await db.MigrateDatabaseAsync(cancellationToken);

        var orgs = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Include(x => x.Master_DB)
            .ThenInclude(m => m!.Rows10)
            .Include(x => x.Master_DB)
            .ThenInclude(m => m!.Rows20)
            .Include(x => x.Report_Collection)
            .ToListAsync(cancellationToken);

        var reports = new List<CompareReportDto>();
        var regNo = "";
        var okpo = "";

        foreach (var org in orgs)
        {
            if (org.Master_DB is null)
            {
                continue;
            }

            var (orgReg, orgOkpo) = ReadOrgIdentity(org.Master_DB);
            if (string.IsNullOrEmpty(regNo) && (!string.IsNullOrEmpty(orgReg) || !string.IsNullOrEmpty(orgOkpo)))
            {
                regNo = orgReg;
                okpo = orgOkpo;
            }

            foreach (var report in org.Report_Collection.OfType<Report>())
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!FormPrintCompareSchema.SupportedForms.Contains(report.FormNum_DB))
                {
                    continue;
                }

                var dto = await LoadReportAsync(db, report, orgReg, orgOkpo, cancellationToken);
                if (dto is not null)
                {
                    reports.Add(dto);
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

    public static Dictionary<ReportMatchKey, CompareReportDto> ToIndex(IEnumerable<CompareReportDto> reports)
    {
        var index = new Dictionary<ReportMatchKey, CompareReportDto>();
        foreach (var report in reports)
        {
            var key = new ReportMatchKey(report.FormNum, report.PeriodKey);
            if (!index.TryGetValue(key, out var existing)
                || report.CorrectionNumber > existing.CorrectionNumber)
            {
                index[key] = report;
            }
        }

        return index;
    }

    private static List<CompareReportDto> DeduplicateByKeyKeepMaxCorrection(List<CompareReportDto> reports) =>
        reports
            .GroupBy(r => new ReportMatchKey(r.FormNum, r.PeriodKey))
            .Select(g => g.OrderByDescending(x => x.CorrectionNumber).First())
            .OrderBy(r => r.FormNum)
            .ThenBy(r => r.PeriodKey)
            .ToList();

    private static (string RegNo, string Okpo) ReadOrgIdentity(Report master)
    {
        if (master.FormNum_DB == "1.0" && master.Rows10 is { Count: > 0 })
        {
            var row = PickTitleRow10(master.Rows10);
            return (row.RegNo_DB ?? "", row.Okpo_DB ?? "");
        }

        if (master.FormNum_DB == "2.0" && master.Rows20 is { Count: > 0 })
        {
            var row = PickTitleRow20(master.Rows20);
            return (row.RegNo_DB ?? "", row.Okpo_DB ?? "");
        }

        return ("", "");
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
        CancellationToken cancellationToken)
    {
        var columns = FormPrintCompareSchema.ColumnsFor(report.FormNum_DB);
        if (columns.Length == 0)
        {
            return null;
        }

        var rows = report.FormNum_DB switch
        {
            "1.1" => await LoadRows11Async(db, report.Id, columns, cancellationToken),
            "1.3" => await LoadRows13Async(db, report.Id, columns, cancellationToken),
            "1.4" => await LoadRows14Async(db, report.Id, columns, cancellationToken),
            "2.12" => await LoadRows212Async(db, report.Id, columns, cancellationToken),
            _ => []
        };

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
            Rows = rows,
            Columns = columns
        };
    }

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
