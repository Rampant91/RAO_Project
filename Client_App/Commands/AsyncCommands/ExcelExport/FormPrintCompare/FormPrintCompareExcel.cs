using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;

internal static class FormPrintCompareExcel
{
    internal static readonly Color ExactFill = Color.FromArgb(198, 239, 206);
    internal static readonly Color NearFill = Color.FromArgb(255, 249, 196);
    internal static readonly Color MismatchFill = Color.FromArgb(255, 213, 79);
    internal static readonly Color DeletedFill = Color.FromArgb(255, 205, 210);
    internal static readonly Color AddedFill = Color.FromArgb(187, 222, 251);
    internal static readonly Color MovedFill = Color.FromArgb(232, 234, 246);
    /// <summary>Дублирующийся № п/п внутри одной стороны отчёта.</summary>
    internal static readonly Color DuplicateNppFill = Color.FromArgb(255, 183, 77);
    private static readonly Color SourceHeaderFill = Color.FromArgb(217, 226, 243);
    private static readonly Color CompareHeaderFill = Color.FromArgb(255, 242, 204);
    private static readonly Color SeparatorFill = Color.FromArgb(89, 89, 89);
    private static readonly Color LegendTitleFill = Color.FromArgb(33, 78, 128);

    /// <summary>
    /// Строка начала данных: 1–3 мета, 4 пояснение, 5 счётчики, 6 блоки, 7 заголовки колонок.
    /// </summary>
    internal const int ReportDataStartRow = 8;

    private const int SummaryColCount = 12;

    public static void FillWorkbook(
        ExcelPackage package,
        string regNo,
        string okpo,
        string sourceFileName,
        string compareFileName,
        IReadOnlyList<ReportCompareResult> results)
    {
        while (package.Workbook.Worksheets.Count > 0)
        {
            package.Workbook.Worksheets.Delete(0);
        }

        WriteSummary(package, regNo, okpo, sourceFileName, compareFileName, results);
        WriteLegendSheet(package);

        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Сводка",
            "Легенда"
        };

        foreach (var result in OrderForSheets(results))
        {
            var sheetName = UniqueSheetName(BuildSheetName(result.Left), usedNames);
            var sheet = package.Workbook.Worksheets.Add(sheetName);
            WriteReportSheet(sheet, result, sourceFileName, compareFileName);
        }
    }

    /// <summary>Листы: всё, кроме полностью совпавших (они только в сводке).</summary>
    internal static IEnumerable<ReportCompareResult> OrderForSheets(IReadOnlyList<ReportCompareResult> results) =>
        results
            .Where(r => !r.IsIdentical)
            .OrderBy(r => r.HasRight ? 1 : 0)
            .ThenBy(r => r.OrderOnlyChanged ? 1 : 0)
            .ThenBy(r => r.Left.FormNum)
            .ThenBy(r => r.Left.PeriodKey);

    internal static IEnumerable<ReportCompareResult> OrderForSummary(IReadOnlyList<ReportCompareResult> results) =>
        results
            .OrderBy(r => r.HasRight ? 1 : 0)
            .ThenBy(r => r.IsIdentical ? 2 : r.OrderOnlyChanged ? 1 : 0)
            .ThenBy(r => r.Left.FormNum)
            .ThenBy(r => r.Left.PeriodKey);

    private static void WriteSummary(
        ExcelPackage package,
        string regNo,
        string okpo,
        string sourceFileName,
        string compareFileName,
        IReadOnlyList<ReportCompareResult> results)
    {
        var sheet = package.Workbook.Worksheets.Add("Сводка");
        sheet.Cells[1, 1].Value = "Сравнение отчётов (исходная БД МПЗФ ↔ файл для сравнения .RAODB)";
        sheet.Cells[1, 1].Style.Font.Bold = true;
        sheet.Cells[2, 1].Value = "Рег.№";
        sheet.Cells[2, 2].Value = regNo;
        sheet.Cells[3, 1].Value = "ОКПО";
        sheet.Cells[3, 2].Value = okpo;
        sheet.Cells[4, 1].Value = "Исходник";
        sheet.Cells[4, 2].Value = sourceFileName;
        sheet.Cells[5, 1].Value = "Файл для сравнения";
        sheet.Cells[5, 2].Value = compareFileName;
        sheet.Cells[6, 1].Value =
            "Легенда подсветки и расшифровка счётчиков — на листе «Легенда». Листы сравнения создаются только при отличиях.";

        var identical = results.Count(r => r.IsIdentical);
        var orderOnly = results.Count(r => r.OrderOnlyChanged);
        var changed = results.Count(r => r.HasRight && !r.IsIdentical && !r.OrderOnlyChanged);
        var missing = results.Count(r => !r.HasRight);

        sheet.Cells[8, 1].Value = "Всего отчётов в исходниках";
        sheet.Cells[8, 2].Value = results.Count;
        sheet.Cells[9, 1].Value = "Полностью совпадают (без отдельных листов)";
        sheet.Cells[9, 2].Value = identical;
        sheet.Cells[10, 1].Value = "Изменён только порядок строк";
        sheet.Cells[10, 2].Value = orderOnly;
        sheet.Cells[11, 1].Value = "Есть изменения данных";
        sheet.Cells[11, 2].Value = changed;
        sheet.Cells[12, 1].Value = "Нет отчёта для сверки";
        sheet.Cells[12, 2].Value = missing;

        const int headerRow = 14;
        sheet.Cells[headerRow, 1].Value = "Форма";
        sheet.Cells[headerRow, 2].Value = "Начало отчёта";
        sheet.Cells[headerRow, 3].Value = "Конец отчёта";
        sheet.Cells[headerRow, 4].Value = "Корр. исходник";
        sheet.Cells[headerRow, 5].Value = "Корр. сравнение";
        sheet.Cells[headerRow, 6].Value = "Корректировки";
        sheet.Cells[headerRow, 7].Value = "Результат";
        sheet.Cells[headerRow, 8].Value = "Добавлено (+)";
        sheet.Cells[headerRow, 9].Value = "Удалено (−)";
        sheet.Cells[headerRow, 10].Value = "Изменено (~)";
        sheet.Cells[headerRow, 11].Value = "Перемещено (↔)";
        sheet.Cells[headerRow, 12].Value = "Без изменений (=)";
        using (var range = sheet.Cells[headerRow, 1, headerRow, SummaryColCount])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.SetBackground(SourceHeaderFill, ExcelFillStyle.Solid);
            range.Style.WrapText = true;
        }

        var row = headerRow + 1;
        foreach (var result in OrderForSummary(results))
        {
            WriteSummaryPeriod(sheet, row, result.Left);

            sheet.Cells[row, 4].Value = result.Left.CorrectionNumber;
            sheet.Cells[row, 5].Value = result.Right?.CorrectionNumber;
            WriteCorrectionMatch(sheet, row, result);

            sheet.Cells[row, 7].Value = result switch
            {
                { HasRight: false } => "Нет отчёта для сверки",
                { IsIdentical: true } => "Совпадает (лист не создан)",
                { OrderOnlyChanged: true } => "Только порядок строк",
                _ => "Есть изменения"
            };
            sheet.Cells[row, 8].Value = result.AddedCount;
            sheet.Cells[row, 9].Value = result.DeletedCount;
            sheet.Cells[row, 10].Value = result.ChangedCount;
            sheet.Cells[row, 11].Value = result.MovedCount;
            sheet.Cells[row, 12].Value = result.UnchangedCount;
            if (result.IsIdentical)
            {
                sheet.Cells[row, 7].Style.Fill.SetBackground(ExactFill, ExcelFillStyle.Solid);
            }

            row++;
        }

        var lastDataRow = Math.Max(row - 1, headerRow);
        if (lastDataRow > headerRow)
        {
            sheet.Cells[headerRow, 1, lastDataRow, SummaryColCount].AutoFilter = true;
        }

        sheet.Cells[1, 1, Math.Max(row, headerRow), SummaryColCount].AutoFitColumns();
    }

    private static void WriteSummaryPeriod(ExcelWorksheet sheet, int row, CompareReportDto report)
    {
        sheet.Cells[row, 1].Value = report.FormNum;
        if (FormPrintCompareNormalize.IsYearOnlyForm(report.FormNum))
        {
            var year = string.IsNullOrWhiteSpace(report.Year)
                ? report.PeriodDisplay
                : string.Concat(report.Year.Where(char.IsDigit));
            if (string.IsNullOrEmpty(year))
            {
                year = report.PeriodDisplay;
            }

            sheet.Cells[row, 2].Value = year;
            sheet.Cells[row, 2, row, 3].Merge = true;
            sheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return;
        }

        sheet.Cells[row, 2].Value = FormPrintCompareNormalize.NormalizePeriodPart(report.StartPeriod);
        sheet.Cells[row, 3].Value = FormPrintCompareNormalize.NormalizePeriodPart(report.EndPeriod);
    }

    private static void WriteCorrectionMatch(ExcelWorksheet sheet, int row, ReportCompareResult result)
    {
        if (!result.HasRight)
        {
            sheet.Cells[row, 6].Value = "—";
            return;
        }

        var match = result.Left.CorrectionNumber == result.Right!.CorrectionNumber;
        sheet.Cells[row, 6].Value = match ? "совпадают" : "различаются";
        var fill = match ? ExactFill : MismatchFill;
        sheet.Cells[row, 4].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
        sheet.Cells[row, 5].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
        sheet.Cells[row, 6].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
    }

    private static void WriteLegendSheet(ExcelPackage package)
    {
        var sheet = package.Workbook.Worksheets.Add("Легенда");
        sheet.Cells[1, 1].Value = "Легенда сравнения отчётов";
        sheet.Cells[1, 1].Style.Font.Bold = true;
        sheet.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
        sheet.Cells[1, 1].Style.Fill.SetBackground(LegendTitleFill, ExcelFillStyle.Solid);

        sheet.Cells[3, 1].Value = "Подсветка ячеек";
        sheet.Cells[3, 1].Style.Font.Bold = true;
        WriteLegendItem(sheet, 4, "Совпадение (Exact) — то же значение, в т.ч. 2e+14 и 2,00e+14 / lookalike", ExactFill);
        WriteLegendItem(sheet, 5, "Мелкое отличие (Near) — близко, но не то же; скрытая разница поясняется в ячейке", NearFill);
        WriteLegendItem(sheet, 6, "Заметное отличие (Mismatch) — существенная разница", MismatchFill);

        sheet.Cells[8, 1].Value = "Статус строки";
        sheet.Cells[8, 1].Style.Font.Bold = true;
        WriteLegendItem(sheet, 9, "Удалена — только в исходнике", DeletedFill);
        WriteLegendItem(sheet, 10, "Добавлена — только в файле для сравнения", AddedFill);
        WriteLegendItem(sheet, 11, "Сбит № п/п — содержимое то же, жёлтым только колонка «№ п/п»", MismatchFill);
        WriteLegendItem(sheet, 12, "Изменена — есть отличия в данных (подсветка ячеек)", MismatchFill);
        WriteLegendItem(sheet, 13, "Без изменений — строка совпала (зелёный статус)", ExactFill);
        WriteLegendItem(sheet, 14, "Оранжевая шапка — объяснение сбоя нумерации по всему отчёту", DuplicateNppFill);

        sheet.Cells[16, 1].Value = "Схожесть, %";
        sheet.Cells[16, 1].Style.Font.Bold = true;
        WriteLegendItem(sheet, 17, "100…80 — высокое совпадение", ExactFill);
        WriteLegendItem(sheet, 18, "79…50 — частичное", NearFill);
        WriteLegendItem(sheet, 19, "ниже 50 — слабое / нет пары (0)", DeletedFill);

        sheet.Cells[21, 1].Value = "Счётчики в сводке";
        sheet.Cells[21, 1].Style.Font.Bold = true;
        sheet.Cells[22, 1].Value = "Добавлено (+) — строки только в файле для сравнения";
        sheet.Cells[23, 1].Value = "Удалено (−) — строки только в исходнике";
        sheet.Cells[24, 1].Value = "Изменено (~) — пара строк с отличиями хотя бы в одной колонке";
        sheet.Cells[25, 1].Value = "Перемещено (↔) — то же содержимое, другой № п/п";
        sheet.Cells[26, 1].Value = "Без изменений (=) — полное совпадение по всем колонкам";

        sheet.Cells[28, 1].Value =
            "Сравнение ячеек — по всем колонкам формы (не только по ключам сопоставления строк). " +
            "Если отличие скрыто (невидимый символ, lookalike), в ячейке показывается пояснение. " +
            "Слева — исходник (имя файла БД). Справа — файл для сравнения (имя файла). " +
            "Полностью совпавшие отчёты перечислены только на листе «Сводка».";
        sheet.Cells[1, 1, 28, 1].AutoFitColumns();
    }

    private static void WriteReportSheet(
        ExcelWorksheet sheet,
        ReportCompareResult result,
        string sourceFileName,
        string compareFileName)
    {
        var left = result.Left;
        var columns = left.Columns;
        var colCount = columns.Length;
        var statusCol = 1;
        var leftStart = 2;
        var separatorCol = leftStart + colCount;
        var confidenceCol = separatorCol + 1;
        var rightStart = confidenceCol + 1;
        var lastCol = Math.Max(rightStart + colCount - 1, 8);
        var headerRow = ReportDataStartRow - 2;
        var sectionRow = ReportDataStartRow - 1;
        var dataStart = ReportDataStartRow;

        WriteMergedHeaderRow(
            sheet, 1, lastCol,
            $"Форма {left.FormNum}, период: {left.PeriodDisplay}",
            bold: true);

        var corrRight = result.Right is null ? "—" : result.Right.CorrectionNumber.ToString();
        var corrNote = result.HasRight
            ? (left.CorrectionNumber == result.Right!.CorrectionNumber
                ? "совпадают"
                : "различаются")
            : "—";
        WriteMergedHeaderRow(
            sheet, 2, lastCol,
            $"Корректировка: исходник {left.CorrectionNumber} → сравнение {corrRight} ({corrNote})",
            fill: result.HasRight
                ? (left.CorrectionNumber == result.Right!.CorrectionNumber ? ExactFill : MismatchFill)
                : null);

        WriteMergedHeaderRow(
            sheet, 3, lastCol,
            $"Исходник: {sourceFileName}  |  Для сравнения: {compareFileName}");

        var countsText =
            $"Строки: без изменений (={result.UnchangedCount}), изменено (~{result.ChangedCount}), " +
            $"сбит № п/п (↔{result.MovedCount}), удалено (−{result.DeletedCount}), добавлено (+{result.AddedCount}). " +
            "Легенда — лист «Легенда».";

        // Строка 4 — пояснение сбоя нумерации (или кратко «см. счётчики»); строка 5 — счётчики.
        if (!string.IsNullOrEmpty(result.Message)
            && (result.OrderOnlyChanged || result.HasNppDuplicates))
        {
            WriteMergedHeaderRow(
                sheet, 4, lastCol, result.Message!,
                fill: DuplicateNppFill, bold: true, wrap: true, minHeight: 48);
            WriteMergedHeaderRow(sheet, 5, lastCol, countsText, wrap: true, minHeight: 32);
        }
        else
        {
            WriteMergedHeaderRow(sheet, 4, lastCol, countsText, wrap: true, minHeight: 32);
            WriteMergedHeaderRow(sheet, 5, lastCol, string.Empty);
        }

        if (!result.HasRight)
        {
            WriteMergedHeaderRow(
                sheet, dataStart, lastCol,
                result.Message ?? "Для отчёта отсутствует отчёт для сверки.",
                fill: DeletedFill, bold: true, wrap: true, minHeight: 30);
            sheet.Column(statusCol).Width = 36;
            return;
        }

        sheet.Cells[headerRow, leftStart].Value = $"Исходник ({sourceFileName})";
        sheet.Cells[headerRow, leftStart, headerRow, leftStart + colCount - 1].Merge = true;
        sheet.Cells[headerRow, leftStart].Style.Fill.SetBackground(SourceHeaderFill, ExcelFillStyle.Solid);
        sheet.Cells[headerRow, leftStart].Style.Font.Bold = true;

        sheet.Cells[headerRow, rightStart].Value = $"Для сравнения ({compareFileName})";
        sheet.Cells[headerRow, rightStart, headerRow, rightStart + colCount - 1].Merge = true;
        sheet.Cells[headerRow, rightStart].Style.Fill.SetBackground(CompareHeaderFill, ExcelFillStyle.Solid);
        sheet.Cells[headerRow, rightStart].Style.Font.Bold = true;

        sheet.Cells[sectionRow, statusCol].Value = "Статус";
        sheet.Cells[sectionRow, statusCol].Style.Font.Bold = true;
        sheet.Cells[sectionRow, confidenceCol].Value = "Схожесть, %";
        sheet.Cells[sectionRow, confidenceCol].Style.Font.Bold = true;
        for (var i = 0; i < colCount; i++)
        {
            sheet.Cells[sectionRow, leftStart + i].Value = columns[i].Header;
            sheet.Cells[sectionRow, rightStart + i].Value = columns[i].Header;
            sheet.Cells[sectionRow, leftStart + i].Style.Font.Bold = true;
            sheet.Cells[sectionRow, rightStart + i].Style.Font.Bold = true;
            sheet.Cells[sectionRow, leftStart + i].Style.Fill.SetBackground(SourceHeaderFill, ExcelFillStyle.Solid);
            sheet.Cells[sectionRow, rightStart + i].Style.Fill.SetBackground(CompareHeaderFill, ExcelFillStyle.Solid);
        }

        sheet.Cells[sectionRow, separatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);

        var row = dataStart;
        foreach (var line in result.DisplayLines)
        {
            sheet.Cells[row, statusCol].Value = line.StatusText;
            sheet.Cells[row, confidenceCol].Value = line.ConfidencePercent;
            sheet.Cells[row, confidenceCol].Style.Fill.SetBackground(
                ConfidenceFill(line.ConfidencePercent),
                ExcelFillStyle.Solid);

            switch (line.Status)
            {
                case DiffRowStatus.Deleted:
                    WriteRowValues(sheet, row, leftStart, line.Left!.Values);
                    FillBlock(sheet, row, leftStart, colCount, DeletedFill);
                    sheet.Cells[row, statusCol].Style.Fill.SetBackground(DeletedFill, ExcelFillStyle.Solid);
                    break;
                case DiffRowStatus.Added:
                    WriteRowValues(sheet, row, rightStart, line.Right!.Values);
                    FillBlock(sheet, row, rightStart, colCount, AddedFill);
                    sheet.Cells[row, statusCol].Style.Fill.SetBackground(AddedFill, ExcelFillStyle.Solid);
                    break;
                default:
                    WriteComparedRowValues(sheet, row, leftStart, rightStart, line);
                    sheet.Cells[row, statusCol].Style.Fill.SetBackground(
                        StatusColumnFill(line.Status),
                        ExcelFillStyle.Solid);

                    if (line.Status == DiffRowStatus.Moved)
                    {
                        // Только колонка № п/п — жёлтым; остальное без заливки (содержимое совпало).
                        for (var i = 0; i < colCount; i++)
                        {
                            if (!columns[i].IsRowNumber)
                            {
                                continue;
                            }

                            sheet.Cells[row, leftStart + i].Style.Fill.SetBackground(
                                MismatchFill, ExcelFillStyle.Solid);
                            sheet.Cells[row, rightStart + i].Style.Fill.SetBackground(
                                MismatchFill, ExcelFillStyle.Solid);
                        }
                    }
                    else if (line.FieldLevels is not null)
                    {
                        for (var i = 0; i < colCount && i < line.FieldLevels.Length; i++)
                        {
                            var fill = FillForLevel(line.FieldLevels[i]);
                            if (fill is null)
                            {
                                continue;
                            }

                            sheet.Cells[row, leftStart + i].Style.Fill.SetBackground(fill.Value, ExcelFillStyle.Solid);
                            sheet.Cells[row, rightStart + i].Style.Fill.SetBackground(fill.Value, ExcelFillStyle.Solid);
                        }
                    }

                    break;
            }

            sheet.Cells[row, separatorCol].Style.Fill.SetBackground(SeparatorFill, ExcelFillStyle.Solid);
            row++;
        }

        var lastDataRow = Math.Max(row - 1, sectionRow);
        if (lastDataRow >= dataStart)
        {
            sheet.Cells[sectionRow, 1, lastDataRow, lastCol].AutoFilter = true;
        }

        // Статус и пояснения в шапке не сжимать в узкую колонку.
        sheet.Column(statusCol).Width = Math.Max(sheet.Column(statusCol).Width, 28);
    }

    private static void WriteMergedHeaderRow(
        ExcelWorksheet sheet,
        int row,
        int lastCol,
        string text,
        Color? fill = null,
        bool bold = false,
        bool wrap = false,
        double? minHeight = null)
    {
        var range = sheet.Cells[row, 1, row, lastCol];
        range.Merge = true;
        range.Value = text;
        range.Style.Font.Bold = bold;
        range.Style.WrapText = wrap;
        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        if (fill is not null)
        {
            range.Style.Fill.SetBackground(fill.Value, ExcelFillStyle.Solid);
        }

        if (minHeight is not null)
        {
            sheet.Row(row).Height = Math.Max(sheet.Row(row).Height, minHeight.Value);
        }
    }

    private static void WriteLegendItem(ExcelWorksheet sheet, int row, string text, Color fill)
    {
        sheet.Cells[row, 1].Value = text;
        sheet.Cells[row, 1].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
    }

    private static void WriteRowValues(ExcelWorksheet sheet, int row, int startCol, string[] values)
    {
        for (var i = 0; i < values.Length; i++)
        {
            sheet.Cells[row, startCol + i].Value = values[i];
        }
    }

    private static void WriteComparedRowValues(
        ExcelWorksheet sheet,
        int row,
        int leftStart,
        int rightStart,
        DiffLine line)
    {
        var leftValues = line.Left!.Values;
        var rightValues = line.Right!.Values;
        var levels = line.FieldLevels;
        var count = Math.Max(leftValues.Length, rightValues.Length);
        for (var i = 0; i < count; i++)
        {
            var leftVal = i < leftValues.Length ? leftValues[i] : "";
            var rightVal = i < rightValues.Length ? rightValues[i] : "";
            var level = levels is not null && i < levels.Length ? levels[i] : FieldMatchLevel.Exact;
            sheet.Cells[row, leftStart + i].Value =
                FormPrintCompareNormalize.FormatCellForExport(leftVal, rightVal, level);
            sheet.Cells[row, rightStart + i].Value =
                FormPrintCompareNormalize.FormatCellForExport(rightVal, leftVal, level);
        }
    }

    private static void FillBlock(ExcelWorksheet sheet, int row, int startCol, int count, Color fill)
    {
        for (var i = 0; i < count; i++)
        {
            sheet.Cells[row, startCol + i].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
        }
    }

    /// <summary>Заливка колонки «Статус» — сразу видно строки с отличиями.</summary>
    internal static Color StatusColumnFill(DiffRowStatus status) => status switch
    {
        DiffRowStatus.Changed => MismatchFill,
        DiffRowStatus.Moved => MismatchFill, // тот же акцент, что у колонки № п/п
        DiffRowStatus.Deleted => DeletedFill,
        DiffRowStatus.Added => AddedFill,
        DiffRowStatus.Unchanged => ExactFill,
        _ => ExactFill
    };

    /// <summary>Подсветка ячеек: Exact / Near / Mismatch.</summary>
    internal static Color? FillForLevel(FieldMatchLevel level) => level switch
    {
        FieldMatchLevel.Exact => ExactFill,
        FieldMatchLevel.Near => NearFill,
        FieldMatchLevel.Mismatch => MismatchFill,
        _ => null
    };

    /// <summary>Совместимость со старыми тестами: отличия; Exact теперь тоже красится через FillForLevel.</summary>
    internal static Color? FillForDifference(FieldMatchLevel level) =>
        level is FieldMatchLevel.Near or FieldMatchLevel.Mismatch ? FillForLevel(level) : null;

    internal static Color ConfidenceFill(int percent) =>
        percent >= 80 ? ExactFill :
        percent >= 50 ? NearFill :
        DeletedFill;

    private static string BuildSheetName(CompareReportDto report)
    {
        var raw = FormPrintCompareNormalize.IsYearOnlyForm(report.FormNum)
            ? $"{report.FormNum}_{report.PeriodDisplay}"
            : $"{report.FormNum}_{FormPrintCompareNormalize.NormalizePeriodPart(report.StartPeriod)}_{FormPrintCompareNormalize.NormalizePeriodPart(report.EndPeriod)}";
        return SanitizeSheetName(raw);
    }

    private static string SanitizeSheetName(string name)
    {
        var sb = new StringBuilder(name.Length);
        foreach (var ch in name)
        {
            sb.Append(ch is '\\' or '/' or '?' or '*' or '[' or ']' or ':' ? '_' : ch);
        }

        var cleaned = sb.ToString().Trim();
        if (string.IsNullOrEmpty(cleaned))
        {
            cleaned = "Лист";
        }

        return cleaned.Length <= 31 ? cleaned : cleaned[..31];
    }

    private static string UniqueSheetName(string baseName, HashSet<string> used)
    {
        var name = baseName;
        var i = 2;
        while (!used.Add(name))
        {
            var suffix = $"_{i++}";
            var maxBase = 31 - suffix.Length;
            name = (baseName.Length <= maxBase ? baseName : baseName[..maxBase]) + suffix;
        }

        return name;
    }
}
