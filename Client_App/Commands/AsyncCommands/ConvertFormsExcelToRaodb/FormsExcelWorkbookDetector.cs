using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Автодетект листов аналитической выгрузки по имени («Отчеты X.Y») и/или эталону заголовков.
/// </summary>
public static partial class FormsExcelWorkbookDetector
{
    public static FormsExcelWorkbookDetectResult Detect(ExcelWorkbook workbook)
    {
        var result = new FormsExcelWorkbookDetectResult();
        var used = new HashSet<ExcelWorksheet>();

        foreach (var ws in workbook.Worksheets)
        {
            if (!TryParseReportsSheetName(ws.Name, out var formNumFromName))
            {
                continue;
            }

            if (!FormsExcelFormCatalog.TryGet(formNumFromName, out var spec) || spec is null)
            {
                result.Notes.Add(
                    $"Лист «{ws.Name}»: форма {formNumFromName} пока не поддерживается конвертером.");
                continue;
            }

            if (!FormsExcelHeaders.HeadersMatch(ws, spec.ReportColumns, out var headerError))
            {
                result.Notes.Add(
                    $"Лист «{ws.Name}»: заголовки не совпадают с выгрузкой формы {spec.FormNum}. {headerError}");
                continue;
            }

            result.Sheets.Add(new FormsExcelDetectedSheet(
                spec,
                ws,
                FindNotesSheet(workbook, spec),
                DetectionSource.SheetName));
            used.Add(ws);
        }

        foreach (var ws in workbook.Worksheets)
        {
            if (used.Contains(ws) || IsNotesSheetName(ws.Name))
            {
                continue;
            }

            FormsExcelFormSpec? matched = null;
            foreach (var spec in FormsExcelFormCatalog.All)
            {
                if (!FormsExcelHeaders.HeadersMatch(ws, spec.ReportColumns, out _))
                {
                    continue;
                }

                if (matched is not null)
                {
                    result.Notes.Add(
                        $"Лист «{ws.Name}»: заголовки совпали с несколькими формами " +
                        $"({matched.FormNum} и {spec.FormNum}) — лист пропущен.");
                    matched = null;
                    break;
                }

                matched = spec;
            }

            if (matched is null)
            {
                continue;
            }

            result.Sheets.Add(new FormsExcelDetectedSheet(
                matched,
                ws,
                FindNotesSheet(workbook, matched),
                DetectionSource.Headers));
            used.Add(ws);
        }

        result.Sheets.Sort((a, b) =>
            string.Compare(a.Spec.FormNum, b.Spec.FormNum, StringComparison.Ordinal));
        return result;
    }

    public static bool TryParseReportsSheetName(string sheetName, out string formNum)
    {
        formNum = "";
        var match = ReportsSheetNameRegex().Match(sheetName.Trim());
        if (!match.Success)
        {
            return false;
        }

        formNum = match.Groups[1].Value;
        return true;
    }

    private static bool IsNotesSheetName(string sheetName) =>
        NotesSheetNameRegex().IsMatch(sheetName.Trim());

    private static ExcelWorksheet? FindNotesSheet(ExcelWorkbook workbook, FormsExcelFormSpec spec)
    {
        var exact = workbook.Worksheets.FirstOrDefault(ws =>
            string.Equals(ws.Name, spec.NotesSheetPrefix, StringComparison.Ordinal));
        if (exact is not null)
        {
            return exact;
        }

        return workbook.Worksheets.FirstOrDefault(ws =>
            ws.Name.StartsWith(spec.NotesSheetPrefix, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// «Отчеты 1.1», «Отчёты 2.10», «Отчеты 1.1_22-24».
    /// </summary>
    [GeneratedRegex(@"^Отч[её]ты\s+(\d+\.\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex ReportsSheetNameRegex();

    [GeneratedRegex(@"^Примечания\s+(\d+\.\d+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex NotesSheetNameRegex();
}

public enum DetectionSource
{
    SheetName,
    Headers
}

/// <summary>
/// Распознанный лист отчётов + опциональный лист примечаний.
/// </summary>
public sealed class FormsExcelDetectedSheet
{
    public FormsExcelDetectedSheet(
        FormsExcelFormSpec spec,
        ExcelWorksheet reportsSheet,
        ExcelWorksheet? notesSheet,
        DetectionSource source)
    {
        Spec = spec;
        ReportsSheet = reportsSheet;
        NotesSheet = notesSheet;
        Source = source;
    }

    public FormsExcelFormSpec Spec { get; }
    public ExcelWorksheet ReportsSheet { get; }
    public ExcelWorksheet? NotesSheet { get; }
    public DetectionSource Source { get; }
}

/// <summary>
/// Результат автодетекта книги.
/// </summary>
public sealed class FormsExcelWorkbookDetectResult
{
    public List<FormsExcelDetectedSheet> Sheets { get; } = [];
    public List<string> Notes { get; } = [];
}
