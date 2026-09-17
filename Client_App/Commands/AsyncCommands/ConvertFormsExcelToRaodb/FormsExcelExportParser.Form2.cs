using System;
using System.Collections.Generic;
using System.Linq;
using Models.Forms;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Парсер аналитической выгрузки форм 2.x: ключ по отчётному году, notes, маппинг строк.
/// </summary>
public static class FormsExcelExportParserForm2
{
    private readonly record struct GroupId(string FormNum, FormsExcelForm2ReportKey Key);

    public static FormsExcelForm2ParseResult ParseDetectedSheet(FormsExcelDetectedSheet sheet)
    {
        if (sheet.Spec.Family != FormsExcelFormFamily.Form2)
        {
            throw new InvalidOperationException(
                $"ParseDetectedSheet Form2 ожидает семейство 2.x, получено {sheet.Spec.FormNum}.");
        }

        var result = new FormsExcelForm2ParseResult();
        var groups = new Dictionary<GroupId, FormsExcelForm2ReportGroup>();
        ParseReportsSheet(sheet, groups, result);
        AttachNotes(sheet, groups, result);
        result.Groups.AddRange(OrderGroups(groups.Values));
        return result;
    }

    private static IEnumerable<FormsExcelForm2ReportGroup> OrderGroups(
        IEnumerable<FormsExcelForm2ReportGroup> groups) =>
        groups.OrderBy(g => g.FormNum)
            .ThenBy(g => g.Key.RegNo)
            .ThenBy(g => g.Key.Okpo)
            .ThenBy(g => g.Key.Year)
            .ThenBy(g => g.Key.CorrectionNumber);

    private static void ParseReportsSheet(
        FormsExcelDetectedSheet sheet,
        Dictionary<GroupId, FormsExcelForm2ReportGroup> groups,
        FormsExcelForm2ParseResult result)
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
                group = new FormsExcelForm2ReportGroup { FormNum = formNum, Key = key };
                groups[id] = group;
            }

            group.Rows.Add(FormsExcelRowMapperForm2x.MapRow(formNum, reportsSheet, row));
        }
    }

    public static bool TryReadReportKey(
        ExcelWorksheet worksheet,
        int row,
        out FormsExcelForm2ReportKey key,
        out string? error)
    {
        key = default;
        error = null;

        var okpo = FormsExcelCellConverters.CellString(worksheet.Cells[row, 2].Value);
        var regNo = FormsExcelCellConverters.CellString(worksheet.Cells[row, 4].Value);
        var year = FormsExcelCellConverters.ParseNullableInt(worksheet.Cells[row, 6].Value);
        var correction = FormsExcelCellConverters.ParseCorrectionNumber(worksheet.Cells[row, 5].Value);

        if (string.IsNullOrEmpty(regNo) && string.IsNullOrEmpty(okpo)
            && year is null && correction is null)
        {
            error = "пустая строка";
            return false;
        }

        if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo))
        {
            error = "нет рег.№ или ОКПО";
            return false;
        }

        if (year is null or <= 0)
        {
            error = "нет отчётного года";
            return false;
        }

        if (correction is null)
        {
            error = "некорректный номер корректировки";
            return false;
        }

        key = new FormsExcelForm2ReportKey(regNo, okpo, year.Value, correction.Value);
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
        Dictionary<GroupId, FormsExcelForm2ReportGroup> groups,
        FormsExcelForm2ParseResult result)
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
            var year = FormsExcelCellConverters.ParseNullableInt(notesSheet.Cells[row, 5].Value);

            if (string.IsNullOrEmpty(regNo) && string.IsNullOrEmpty(okpo)
                && FormsExcelCellConverters.IsDashOrEmpty(notesSheet.Cells[row, 6].Value)
                && FormsExcelCellConverters.IsDashOrEmpty(notesSheet.Cells[row, 8].Value))
            {
                continue;
            }

            if (string.IsNullOrEmpty(regNo) || string.IsNullOrEmpty(okpo)
                || year is null or <= 0 || correction is null)
            {
                result.Warnings.Add($"Примечание, строка {row}: не удалось сопоставить с отчётом.");
                continue;
            }

            var key = new FormsExcelForm2ReportKey(regNo, okpo, year.Value, correction.Value);
            var id = new GroupId(sheet.Spec.FormNum, key);
            if (!groups.TryGetValue(id, out var group))
            {
                result.Warnings.Add($"Примечание, строка {row}: нет группы отчёта {key}.");
                continue;
            }

            group.Notes.Add(new Note
            {
                RowNumber_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 6].Value),
                GraphNumber_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 7].Value),
                Comment_DB = FormsExcelCellConverters.CellString(notesSheet.Cells[row, 8].Value),
                Order = ++order
            });
        }
    }
}
