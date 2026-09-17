using System;
using System.Collections.Generic;
using System.Linq;
using Models.Forms;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Общий парсер аналитической выгрузки форм 1.x: ключ периода, группировка, notes, маппинг строк.
/// </summary>
public static class FormsExcelExportParserForm1
{
    private readonly record struct GroupId(string FormNum, FormsExcelReportKey Key);

    public static FormsExcelForm1ParseResult ParseDetectedSheet(FormsExcelDetectedSheet sheet)
    {
        if (sheet.Spec.Family != FormsExcelFormFamily.Form1)
        {
            throw new InvalidOperationException(
                $"ParseDetectedSheet Form1 ожидает семейство 1.x, получено {sheet.Spec.FormNum}.");
        }

        var result = new FormsExcelForm1ParseResult();
        var groups = new Dictionary<GroupId, FormsExcelForm1ReportGroup>();
        ParseReportsSheet(sheet, groups, result);
        AttachNotes(sheet, groups, result);
        result.Groups.AddRange(OrderGroups(groups.Values));
        return result;
    }

    /// <summary>
    /// Разбор всех листов 1.x в книге (для тестов и многолистовых файлов).
    /// </summary>
    public static FormsExcelForm1ParseResult ParseWorkbook(ExcelPackage package)
    {
        var detection = FormsExcelWorkbookDetector.Detect(package.Workbook);
        var form1Sheets = detection.Sheets
            .Where(s => s.Spec.Family == FormsExcelFormFamily.Form1)
            .ToList();

        if (form1Sheets.Count == 0)
        {
            var hint = detection.Notes.Count > 0
                ? string.Join(" ", detection.Notes.Take(3))
                : "Ожидается лист «Отчеты X.Y» с эталонными заголовками формы 1.x.";
            throw new InvalidOperationException($"Не найден лист выгрузки формы 1.x. {hint}");
        }

        var result = new FormsExcelForm1ParseResult();
        result.Warnings.AddRange(detection.Notes);
        var groups = new Dictionary<GroupId, FormsExcelForm1ReportGroup>();

        foreach (var sheet in form1Sheets)
        {
            ParseReportsSheet(sheet, groups, result);
            AttachNotes(sheet, groups, result);
        }

        result.Groups.AddRange(OrderGroups(groups.Values));
        return result;
    }

    private static IEnumerable<FormsExcelForm1ReportGroup> OrderGroups(
        IEnumerable<FormsExcelForm1ReportGroup> groups) =>
        groups.OrderBy(g => g.FormNum)
            .ThenBy(g => g.Key.RegNo)
            .ThenBy(g => g.Key.Okpo)
            .ThenBy(g => g.Key.StartPeriod)
            .ThenBy(g => g.Key.EndPeriod)
            .ThenBy(g => g.Key.CorrectionNumber);

    private static void ParseReportsSheet(
        FormsExcelDetectedSheet sheet,
        Dictionary<GroupId, FormsExcelForm1ReportGroup> groups,
        FormsExcelForm1ParseResult result)
    {
        var reportsSheet = sheet.ReportsSheet;
        var formNum = sheet.Spec.FormNum;
        var columnCount = sheet.Spec.ReportColumns.Count;
        var endRow = reportsSheet.Dimension?.End.Row ?? 1;

        for (var row = 2; row <= endRow; row++)
        {
            if (!TryReadReportKey(reportsSheet, row, out var key, out var keyError))
            {
                if (IsEmptyDataRow(reportsSheet, row, columnCount))
                {
                    continue;
                }

                result.Warnings.Add($"Лист «{reportsSheet.Name}», строка {row}: пропущена ({keyError}).");
                continue;
            }

            var id = new GroupId(formNum, key);
            if (!groups.TryGetValue(id, out var group))
            {
                group = new FormsExcelForm1ReportGroup { FormNum = formNum, Key = key };
                groups[id] = group;
            }

            group.Rows.Add(FormsExcelRowMapperForm1x.MapRow(formNum, reportsSheet, row));
        }
    }

    public static bool TryReadReportKey(
        ExcelWorksheet worksheet,
        int row,
        out FormsExcelReportKey key,
        out string? error)
    {
        key = default;
        error = null;

        var okpo = FormsExcelCellConverters.CellString(worksheet.Cells[row, 2].Value);
        var regNo = FormsExcelCellConverters.CellString(worksheet.Cells[row, 4].Value);
        var start = FormsExcelCellConverters.NormalizeDate(
            worksheet.Cells[row, 6].Value,
            worksheet.Cells[row, 6].Text);
        var end = FormsExcelCellConverters.NormalizeDate(
            worksheet.Cells[row, 7].Value,
            worksheet.Cells[row, 7].Text);
        var correction = FormsExcelCellConverters.ParseCorrectionNumber(worksheet.Cells[row, 5].Value);

        if (string.IsNullOrEmpty(regNo) && string.IsNullOrEmpty(okpo)
            && (start is "-" or "") && (end is "-" or "") && correction is null)
        {
            error = "пустая строка";
            return false;
        }

        if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo))
        {
            error = "нет рег.№ или ОКПО";
            return false;
        }

        if (correction is null)
        {
            error = "некорректный номер корректировки";
            return false;
        }

        key = new FormsExcelReportKey(regNo, okpo, start, end, correction.Value);
        return true;
    }

    private static bool IsEmptyDataRow(ExcelWorksheet worksheet, int row, int columnCount)
    {
        for (var col = 1; col <= columnCount; col++)
        {
            if (!FormsExcelCellConverters.IsDashOrEmpty(worksheet.Cells[row, col].Value)
                && !string.IsNullOrWhiteSpace(Convert.ToString(worksheet.Cells[row, col].Value)))
            {
                return false;
            }
        }

        return true;
    }

    private static void AttachNotes(
        FormsExcelDetectedSheet sheet,
        Dictionary<GroupId, FormsExcelForm1ReportGroup> groups,
        FormsExcelForm1ParseResult result)
    {
        var notesSheet = sheet.NotesSheet;
        if (notesSheet is null)
        {
            return;
        }

        if (!FormsExcelHeaders.HeadersMatch(notesSheet, sheet.Spec.NotesColumns, out var notesHeaderError))
        {
            result.Warnings.Add(
                $"Лист «{notesSheet.Name}» пропущен: заголовки не совпадают ({notesHeaderError}).");
            return;
        }

        var endRow = notesSheet.Dimension?.End.Row ?? 1;
        var order = 0L;
        for (var row = 2; row <= endRow; row++)
        {
            var okpo = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 1].Value);
            var regNo = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 3].Value);
            var correction = FormsExcelCellConverters.ParseCorrectionNumber(notesSheet.Cells[row, 4].Value);
            var start = FormsExcelCellConverters.NormalizeDate(
                notesSheet.Cells[row, 5].Value,
                notesSheet.Cells[row, 5].Text);
            var end = FormsExcelCellConverters.NormalizeDate(
                notesSheet.Cells[row, 6].Value,
                notesSheet.Cells[row, 6].Text);

            if (string.IsNullOrEmpty(regNo) && string.IsNullOrEmpty(okpo)
                && FormsExcelCellConverters.IsDashOrEmpty(notesSheet.Cells[row, 7].Value)
                && FormsExcelCellConverters.IsDashOrEmpty(notesSheet.Cells[row, 9].Value))
            {
                continue;
            }

            if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo) || correction is null)
            {
                result.Warnings.Add($"Примечание, строка {row}: не удалось сопоставить с отчётом.");
                continue;
            }

            var key = new FormsExcelReportKey(regNo, okpo, start, end, correction.Value);
            var id = new GroupId(sheet.Spec.FormNum, key);
            if (!groups.TryGetValue(id, out var group))
            {
                result.Warnings.Add($"Примечание, строка {row}: нет группы отчёта {key}.");
                continue;
            }

            group.Notes.Add(new Note
            {
                RowNumber_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 7].Value),
                GraphNumber_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 8].Value),
                Comment_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 9].Value),
                Order = ++order
            });
        }
    }
}
