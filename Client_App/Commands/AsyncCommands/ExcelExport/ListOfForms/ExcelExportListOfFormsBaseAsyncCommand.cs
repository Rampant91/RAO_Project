using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Client_App.Commands.AsyncCommands.ExcelExport.ListOfForms;

public abstract class ExcelExportListOfFormsBaseAsyncCommand : ExcelBaseAsyncCommand
{
    /// <summary>
    /// Firebird ограничивает список значений в IN (...) ~1500 элементами.
    /// </summary>
    private const int FirebirdInClauseBatchSize = 1000;

    private const double MaxTextColumnWidth = 35;
    private const double RegNoColumnWidth = 14;
    private const double OkpoColumnWidth = 12;
    private const double FormNumColumnWidth = 8;
    private const double DateColumnWidth = 13;
    private const double YearColumnWidth = 8;
    private const double CorrectionColumnWidth = 14;
    private const double RowCountColumnWidth = 14;

    /// <summary>
    /// Высота строки заголовков для списка форм 1 (в пунктах).
    /// </summary>
    private protected const double HeaderRowHeightForm1 = 55;

    /// <summary>
    /// Высота строки заголовков для списка форм 2 (в пунктах).
    /// </summary>
    private protected const double HeaderRowHeightForm2 = 45;

    /// <summary>
    /// Суффикс переноса строки в заголовке узкой колонки — стрелка AutoFilter остаётся на пустой строке.
    /// </summary>
    private protected const string HeaderFilterLineBreak = "\n";

    /// <summary>
    /// Цвет фона чередующихся строк данных.
    /// </summary>
    private protected static readonly Color AlternatingRowFill = Color.FromArgb(221, 235, 247); // #DDEBF7

    private protected static readonly string[] Form1ChildFormNumbers =
        ["1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9"];

    private protected static readonly string[] Form2ChildFormNumbers =
    [
        "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"
    ];

    /// <summary>
    /// Интервал обновления прогрессбара при записи строк в Excel (в строках).
    /// </summary>
    private protected const int ProgressUpdateRowInterval = 50;

    /// <summary>
    /// Прогрессбар в начале загрузки данных из БД.
    /// </summary>
    private protected const int ProgressDbLoadStart = 20;

    /// <summary>
    /// Прогрессбар после загрузки титульных данных организаций.
    /// </summary>
    private protected const int ProgressTitlesLoaded = 21;

    /// <summary>
    /// Прогрессбар после загрузки списка организаций и дочерних отчётов.
    /// </summary>
    private protected const int ProgressReportsListEnd = 22;

    /// <summary>
    /// Прогрессбар в начале подсчёта строк форм в БД.
    /// </summary>
    private protected const int ProgressRowCountsStart = 23;

    /// <summary>
    /// Прогрессбар после подсчёта строк форм 1 в БД.
    /// </summary>
    private protected const int ProgressRowCountsEndForm1 = 74;

    /// <summary>
    /// Прогрессбар после подсчёта строк форм 2 в БД.
    /// </summary>
    private protected const int ProgressRowCountsEndForm2 = 83;

    /// <summary>
    /// Прогрессбар после записи данных в Excel.
    /// </summary>
    private protected const int ProgressExcelFillEnd = 92;

    /// <summary>
    /// Прогрессбар перед сохранением файла.
    /// </summary>
    private protected const int ProgressSaveStart = 95;

    /// <summary>
    /// Прогрессбар перед очисткой временных данных.
    /// </summary>
    private protected const int ProgressCleanup = 98;

    #region ExportData

    /// <summary>
    /// Организация с полями титула и дочерними отчётами для выгрузки списка форм.
    /// </summary>
    private protected sealed class FormListOrgExportData(
        string regNo,
        string okpo,
        string shortJurLico,
        IReadOnlyList<FormReportListInfo> reports)
    {
        public string RegNo { get; } = regNo;
        public string Okpo { get; } = okpo;
        public string ShortJurLico { get; } = shortJurLico;
        public IReadOnlyList<FormReportListInfo> Reports { get; } = reports;
    }

    private protected sealed record FormListExportPreparedData(
        List<FormListOrgExportData> Orgs,
        Dictionary<string, List<int>> ReportIdsByForm);

    private protected readonly record struct FormReportListInfo(
        int OrgId,
        int ReportId,
        string FormNum_DB,
        string? StartPeriod_DB,
        string? EndPeriod_DB,
        string? Year_DB,
        byte CorrectionNumber_DB);

    private readonly record struct TitleRowInfo(
        int MasterReportId,
        int NumberInOrder,
        string? RegNo_DB,
        string? Okpo_DB,
        string? ShortJurLico_DB);

    #endregion

    #region Progress

    /// <summary>
    /// Подсчитывает количество строк, которое будет записано в Excel.
    /// </summary>
    private protected static int CountExportRows(IEnumerable<FormListOrgExportData> orgs) =>
        orgs.Sum(o => o.Reports.Count);

    /// <summary>
    /// Обновляет прогрессбар при загрузке счётчиков строк по номеру формы.
    /// </summary>
    private protected static void SetRowCountsProgress(
        AnyTaskProgressBarVM progressBarVM,
        int formIndex,
        int formCount,
        int progressEnd,
        string formNum)
    {
        var percent = ProgressRowCountsStart
                      + (int)Math.Floor((progressEnd - ProgressRowCountsStart) * formIndex / (double)formCount);
        progressBarVM.SetProgressBar(percent, $"Загрузка списка форм {formNum}");
    }

    #endregion

    #region GetReportsList

    /// <summary>
    /// Получение списка организаций с титульными полями и дочерними отчётами.
    /// </summary>
    private protected static async Task<List<FormListOrgExportData>> GetReportsList(
        DBModel db,
        string masterFormNum,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var token = cts.Token;
        var childFormNumbers = masterFormNum switch
        {
            "1.0" => Form1ChildFormNumbers,
            "2.0" => Form2ChildFormNumbers,
            _ => throw new ArgumentOutOfRangeException(nameof(masterFormNum), masterFormNum, null)
        };

        progressBarVM.SetProgressBar(ProgressDbLoadStart, "Загрузка списка организаций");

        var orgRows = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.DBObservableId != null && reps.Master_DB.FormNum_DB == masterFormNum)
            .Select(reps => new { reps.Id, MasterReportId = reps.Master_DBId })
            .Where(x => x.MasterReportId != null)
            .ToListAsync(token);

        if (orgRows.Count == 0)
            return [];

        var masterReportIds = orgRows
            .Select(x => x.MasterReportId!.Value)
            .Distinct()
            .ToList();

        progressBarVM.SetProgressBar(ProgressTitlesLoaded, "Загрузка титульных данных");

        var titleByMasterId = masterFormNum switch
        {
            "1.0" => await LoadTitleRowsForm10Async(db, masterReportIds, token),
            "2.0" => await LoadTitleRowsForm20Async(db, masterReportIds, token),
            _ => throw new ArgumentOutOfRangeException(nameof(masterFormNum), masterFormNum, null)
        };

        var orgIds = orgRows.Select(o => o.Id).ToHashSet();
        var reportsByOrgId = await LoadFormReportsForList(
            db, orgIds, childFormNumbers, progressBarVM, token);

        var result = new List<FormListOrgExportData>(orgRows.Count);
        foreach (var org in orgRows)
        {
            titleByMasterId.TryGetValue(org.MasterReportId!.Value, out var titleRows);
            var (regNo, okpo, shortJurLico) = masterFormNum switch
            {
                "1.0" => BuildTitleFields(titleRows, ResolveTitleForm10, ResolveShortJurLicoForm10),
                "2.0" => BuildTitleFields(titleRows, ResolveTitleForm20, ResolveShortJurLicoForm20),
                _ => (string.Empty, string.Empty, string.Empty)
            };

            reportsByOrgId.TryGetValue(org.Id, out var reports);
            result.Add(new FormListOrgExportData(
                regNo,
                okpo,
                shortJurLico,
                reports ?? []));
        }

        progressBarVM.SetProgressBar(ProgressReportsListEnd, "Список организаций загружен");
        return result;
    }

    private static async Task<Dictionary<int, List<TitleRowInfo>>> LoadTitleRowsForm10Async(
        DBModel db,
        List<int> masterReportIds,
        CancellationToken token)
    {
        var result = new Dictionary<int, List<TitleRowInfo>>(masterReportIds.Count);
        if (masterReportIds.Count == 0)
            return result;

        var batchCount = (masterReportIds.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = masterReportIds
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var rows = await db.form_10
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .Select(f => new TitleRowInfo(
                    f.ReportId!.Value,
                    f.NumberInOrder_DB,
                    f.RegNo_DB,
                    f.Okpo_DB,
                    f.ShortJurLico_DB))
                .ToListAsync(token);

            AppendTitleRows(result, rows);
        }

        return result;
    }

    private static async Task<Dictionary<int, List<TitleRowInfo>>> LoadTitleRowsForm20Async(
        DBModel db,
        List<int> masterReportIds,
        CancellationToken token)
    {
        var result = new Dictionary<int, List<TitleRowInfo>>(masterReportIds.Count);
        if (masterReportIds.Count == 0)
            return result;

        var batchCount = (masterReportIds.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = masterReportIds
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var rows = await db.form_20
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .Select(f => new TitleRowInfo(
                    f.ReportId!.Value,
                    f.NumberInOrder_DB,
                    f.RegNo_DB,
                    f.Okpo_DB,
                    f.ShortJurLico_DB))
                .ToListAsync(token);

            AppendTitleRows(result, rows);
        }

        return result;
    }

    private static void AppendTitleRows(Dictionary<int, List<TitleRowInfo>> result, List<TitleRowInfo> rows)
    {
        foreach (var row in rows)
        {
            if (!result.TryGetValue(row.MasterReportId, out var list))
            {
                list = [];
                result[row.MasterReportId] = list;
            }

            list.Add(row);
        }
    }

    /// <summary>
    /// Загружает только поля дочерних отчётов, необходимые для выгрузки списка форм.
    /// </summary>
    private static async Task<Dictionary<int, List<FormReportListInfo>>> LoadFormReportsForList(
        DBModel db,
        HashSet<int> orgIds,
        string[] childFormNumbers,
        AnyTaskProgressBarVM progressBarVM,
        CancellationToken token)
    {
        if (orgIds.Count == 0)
            return [];

        var orgIdList = orgIds.ToList();
        var batchCount = (orgIdList.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        var result = new Dictionary<int, List<FormReportListInfo>>(orgIds.Count);
        var reportsLoadRange = ProgressReportsListEnd - ProgressTitlesLoaded;

        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = orgIdList
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var status = batchCount == 1
                ? "Загрузка списка отчётов"
                : $"Загрузка списка отчётов (пакет {batchIndex + 1}/{batchCount})";
            var percent = ProgressTitlesLoaded
                            + (batchCount == 1
                                ? reportsLoadRange
                                : (int)Math.Floor(reportsLoadRange * (batchIndex + 1) / (double)batchCount));
            progressBarVM.SetProgressBar(percent, status);

            var rows = await db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(r => r.Reports != null && batch.Contains(r.Reports.Id))
                .Where(r => childFormNumbers.Contains(r.FormNum_DB))
                .Select(r => new FormReportListInfo(
                    r.Reports!.Id,
                    r.Id,
                    r.FormNum_DB,
                    r.StartPeriod_DB,
                    r.EndPeriod_DB,
                    r.Year_DB,
                    r.CorrectionNumber_DB))
                .ToListAsync(token);

            foreach (var row in rows)
            {
                if (!result.TryGetValue(row.OrgId, out var list))
                {
                    list = [];
                    result[row.OrgId] = list;
                }

                list.Add(row);
            }
        }

        return result;
    }

    #endregion

    #region PrepareExport

    private protected static FormListExportPreparedData PrepareForm1Export(
        List<FormListOrgExportData> orgs,
        DateOnly startDate,
        DateOnly endDate)
    {
        var reportIdsByForm = Form1ChildFormNumbers.ToDictionary(f => f, _ => new List<int>());
        var preparedOrgs = new List<FormListOrgExportData>(orgs.Count);

        foreach (var org in orgs)
        {
            var filtered = new List<FormReportListInfo>();
            foreach (var rep in org.Reports)
            {
                if (!IsForm1ReportInDateRange(rep, startDate, endDate))
                    continue;

                filtered.Add(rep);
                reportIdsByForm[rep.FormNum_DB].Add(rep.ReportId);
            }

            preparedOrgs.Add(new FormListOrgExportData(org.RegNo, org.Okpo, org.ShortJurLico, filtered));
        }

        return new FormListExportPreparedData(preparedOrgs, reportIdsByForm);
    }

    private protected static FormListExportPreparedData PrepareForm2Export(
        List<FormListOrgExportData> orgs,
        int minYear,
        int maxYear)
    {
        var reportIdsByForm = Form2ChildFormNumbers.ToDictionary(f => f, _ => new List<int>());
        var preparedOrgs = new List<FormListOrgExportData>(orgs.Count);

        foreach (var org in orgs)
        {
            var filtered = new List<FormReportListInfo>();
            foreach (var rep in org.Reports)
            {
                if (!IsForm2ReportInYearRange(rep, minYear, maxYear))
                    continue;

                filtered.Add(rep);
                reportIdsByForm[rep.FormNum_DB].Add(rep.ReportId);
            }

            preparedOrgs.Add(new FormListOrgExportData(org.RegNo, org.Okpo, org.ShortJurLico, filtered));
        }

        return new FormListExportPreparedData(preparedOrgs, reportIdsByForm);
    }

    private protected static bool IsForm1ReportInDateRange(FormReportListInfo rep, DateOnly startDate, DateOnly endDate)
    {
        if (startDate == DateOnly.MinValue && endDate == DateOnly.MaxValue)
            return true;
        if (!DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDateTime))
            return false;
        return repEndDateTime >= startDate && repEndDateTime <= endDate;
    }

    private protected static bool IsForm2ReportInYearRange(FormReportListInfo rep, int minYear, int maxYear)
    {
        if (minYear == 0 && maxYear == 9999)
            return true;
        if (rep.Year_DB?.Length != 4 || !int.TryParse(rep.Year_DB, out var currentRepsYear))
            return false;
        return currentRepsYear >= minYear && currentRepsYear <= maxYear;
    }

    private protected static List<FormReportListInfo> OrderForm1Reports(IReadOnlyList<FormReportListInfo> reports) =>
        reports
            .OrderBy(x => x.FormNum_DB)
            .ThenByDescending(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDateOnly) ? stDateOnly : DateOnly.MaxValue)
            .ThenByDescending(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDateOnly) ? endDateOnly : DateOnly.MaxValue)
            .ThenByDescending(x => x.CorrectionNumber_DB)
            .ToList();

    private protected static List<FormReportListInfo> OrderForm2Reports(IReadOnlyList<FormReportListInfo> reports) =>
        reports
            .OrderBy(x => byte.TryParse(x.FormNum_DB[2..], out var formNum) ? formNum : byte.MaxValue)
            .ThenByDescending(x => int.TryParse(x.Year_DB, out var year) ? year : int.MinValue)
            .ThenByDescending(x => x.CorrectionNumber_DB)
            .ToList();

    #endregion

    #region TitleFields

    private static (string RegNo, string Okpo, string ShortJurLico) BuildTitleFields(
        List<TitleRowInfo>? rows,
        Func<List<TitleRowInfo>?, (string RegNo, string Okpo, string ShortJurLico)> resolveRegOkpo,
        Func<List<TitleRowInfo>?, string> resolveShortJurLico)
    {
        var (regNo, okpo, _) = resolveRegOkpo(rows);
        return (regNo, okpo, resolveShortJurLico(rows));
    }

    private static (string RegNo, string Okpo, string ShortJurLico) ResolveTitleForm10(List<TitleRowInfo>? rows) =>
        ResolveRegOkpo(rows, GetForm10RowRegNo, GetForm10RowOkpo);

    private static (string RegNo, string Okpo, string ShortJurLico) ResolveTitleForm20(List<TitleRowInfo>? rows) =>
        ResolveRegOkpo(rows, GetForm20RowRegNo, GetForm20RowOkpo);

    private static string ResolveShortJurLicoForm10(List<TitleRowInfo>? rows) =>
        ResolveShortJurLico(rows, GetForm10ShortJurLico);

    private static string ResolveShortJurLicoForm20(List<TitleRowInfo>? rows) =>
        ResolveShortJurLico(rows, GetForm20ShortJurLico);

    private static (string RegNo, string Okpo, string ShortJurLico) ResolveRegOkpo(
        List<TitleRowInfo>? rows,
        Func<TitleRowInfo, string> getRegNo,
        Func<TitleRowInfo, string> getOkpo)
    {
        if (rows is null || rows.Count == 0)
            return (string.Empty, string.Empty, string.Empty);

        var ordered = rows.OrderBy(r => r.NumberInOrder).ToList();
        var hasBranch = ordered.Count > 1;
        var hasHead = ordered.Count > 0;

        if (hasBranch)
        {
            var branch = ordered[1];
            if ((getRegNo(branch) != "" || branch.Okpo_DB == "-") && getOkpo(branch) != "")
                return (getRegNo(branch), getOkpo(branch), string.Empty);
        }

        return hasHead
            ? (getRegNo(ordered[0]), getOkpo(ordered[0]), string.Empty)
            : (string.Empty, string.Empty, string.Empty);
    }

    private static string ResolveShortJurLico(
        List<TitleRowInfo>? rows,
        Func<TitleRowInfo, string> getShortJurLico)
    {
        if (rows is null || rows.Count == 0)
            return string.Empty;

        var ordered = rows.OrderBy(r => r.NumberInOrder).ToList();

        if (ordered.Count > 1 && ordered[1].Okpo_DB is not ("" or "-"))
            return getShortJurLico(ordered[1]);

        return ordered.Count > 0 ? getShortJurLico(ordered[0]) : string.Empty;
    }

    private static string GetForm10RowOkpo(TitleRowInfo row) =>
        string.IsNullOrWhiteSpace(row.Okpo_DB) ? string.Empty : row.Okpo_DB.Trim();

    private static string GetForm10RowRegNo(TitleRowInfo row) =>
        string.IsNullOrWhiteSpace(row.RegNo_DB) ? string.Empty : row.RegNo_DB.Trim();

    private static string GetForm10ShortJurLico(TitleRowInfo row) =>
        string.IsNullOrWhiteSpace(row.ShortJurLico_DB) ? string.Empty : row.ShortJurLico_DB.Trim();

    private static string GetForm20RowOkpo(TitleRowInfo row) => GetForm10RowOkpo(row);
    private static string GetForm20RowRegNo(TitleRowInfo row) => GetForm10RowRegNo(row);
    private static string GetForm20ShortJurLico(TitleRowInfo row) => GetForm10ShortJurLico(row);

    #endregion

    #region RowCounts

    /// <summary>
    /// Подсчёт строк формы 1 по пакетам reportId через таблицу form_XX (без загрузки сущностей Report).
    /// </summary>
    private protected static async Task AppendForm1RowCountsAsync<TForm>(
        DbSet<TForm> formDbSet,
        IReadOnlyList<int> reportIds,
        Dictionary<int, (int RowCount, int Code10Count)> result,
        CancellationToken token) where TForm : Form1
    {
        if (reportIds.Count == 0)
            return;

        var batchCount = (reportIds.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = reportIds
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var rows = await formDbSet
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .GroupBy(f => f.ReportId!.Value)
                .Select(g => new
                {
                    ReportId = g.Key,
                    Total = g.Count(),
                    Code10 = g.Count(f => f.OperationCode_DB == "10")
                })
                .ToListAsync(token);

            foreach (var row in rows)
                result[row.ReportId] = (row.Total, row.Code10);
        }
    }

    /// <summary>
    /// Подсчёт строк формы 2 по пакетам reportId через таблицу form_XX.
    /// </summary>
    private protected static async Task AppendForm2RowCountsAsync<TForm>(
        DbSet<TForm> formDbSet,
        IReadOnlyList<int> reportIds,
        Dictionary<int, int> result,
        CancellationToken token) where TForm : Form2
    {
        if (reportIds.Count == 0)
            return;

        var batchCount = (reportIds.Count + FirebirdInClauseBatchSize - 1) / FirebirdInClauseBatchSize;
        for (var batchIndex = 0; batchIndex < batchCount; batchIndex++)
        {
            var batch = reportIds
                .Skip(batchIndex * FirebirdInClauseBatchSize)
                .Take(FirebirdInClauseBatchSize)
                .ToList();

            var rows = await formDbSet
                .AsNoTracking()
                .Where(f => f.ReportId != null && batch.Contains(f.ReportId.Value))
                .GroupBy(f => f.ReportId!.Value)
                .Select(g => new { ReportId = g.Key, Total = g.Count() })
                .ToListAsync(token);

            foreach (var row in rows)
                result[row.ReportId] = row.Total;
        }
    }

    #endregion

    #region ExcelColumnWidths

    /// <summary>
    /// Чередует белый и светло-голубой фон строк данных.
    /// </summary>
    private protected static void ApplyAlternatingRowColors(ExcelWorksheet worksheet, int firstRow, int lastRow)
    {
        if (lastRow < firstRow || worksheet.Dimension is null)
            return;

        var lastColumn = worksheet.Dimension.End.Column;

        for (var row = firstRow; row <= lastRow; row++)
        {
            if ((row - firstRow) % 2 != 0)
            {
                worksheet.Cells[row, 1, row, lastColumn].Style.Fill.SetBackground(
                    AlternatingRowFill,
                    ExcelFillStyle.Solid);
            }
        }
    }

    /// <summary>
    /// Подбирает ширину колонок и оформляет строку заголовков.
    /// </summary>
    private protected static void ApplyFormListExcelStyle(ExcelWorksheet worksheet, double headerRowHeight)
    {
        ApplyFormListColumnWidths(worksheet);
        ApplyExcelHeaderRowStyle(
            worksheet,
            worksheet.Dimension.End.Column,
            headerRowHeight: headerRowHeight);
    }

    /// <summary>
    /// Фиксированные ширины колонок без AutoFit по данным (как в выгрузках организаций/исполнителей).
    /// </summary>
    private protected static void ApplyFormListColumnWidths(ExcelWorksheet worksheet)
    {
        for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            var header = NormalizeHeaderText(worksheet.Cells[1, col].Value?.ToString());
            var column = worksheet.Column(col);

            if (TryGetFormListFixedColumnWidth(header, out var fixedWidth))
            {
                column.Width = fixedWidth;
                if (IsFormListTextColumn(header))
                    column.Style.WrapText = true;
                continue;
            }

            if (IsFormListTextColumn(header))
            {
                column.Width = MaxTextColumnWidth;
                column.Style.WrapText = true;
            }
        }
    }

    private static string NormalizeHeaderText(string? header) =>
        header?.TrimEnd('\n', '\r') ?? string.Empty;

    private static bool TryGetFormListFixedColumnWidth(string header, out double width)
    {
        switch (header)
        {
            case "Рег. №":
                width = RegNoColumnWidth;
                return true;
            case "ОКПО":
                width = OkpoColumnWidth;
                return true;
            case "Форма":
                width = FormNumColumnWidth;
                return true;
            case "Дата начала периода":
            case "Дата конца периода":
                width = DateColumnWidth;
                return true;
            case "Отчетный год":
                width = YearColumnWidth;
                return true;
            case "Номер корректировки":
                width = CorrectionColumnWidth;
                return true;
            case "Количество строк":
                width = RowCountColumnWidth;
                return true;
            case "Сокращенное наименование":
            case "Инвентаризация":
                width = MaxTextColumnWidth;
                return true;
            default:
                width = 0;
                return false;
        }
    }

    private static bool IsFormListTextColumn(string header) =>
        header is "Сокращенное наименование" or "Инвентаризация";

    #endregion

    #region ReportsCountCheck

    /// <summary>
    /// Подсчёт количества организаций. При количестве равном 0, выводится сообщение, операция завершается.
    /// </summary>
    private protected static async Task ReportsCountCheck(DBModel db, string formNum, AnyTaskProgressBar? progressBar, CancellationTokenSource cts)
    {
        var countReports = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(x => x.DBObservableId != null && x.Master_DB.FormNum_DB == formNum)
            .CountAsync(cts.Token);

        if (countReports == 0)
        {
            #region MessageRepsNotFound

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Не удалось совершить выгрузку списка всех отчетов по форме {formNum[0]} с указанием количества строк," +
                        $"{Environment.NewLine}поскольку в текущей базе отсутствует отчетность по формам {formNum[0]}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(progressBar ?? Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

    #endregion
}
