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

    private const int SummaryColCount = 16;

    private const int SummaryColRegNo = 1;
    private const int SummaryColShortName = 2;
    private const int SummaryColOkpo = 3;
    private const int SummaryColFile = 4;
    private const int SummaryColForm = 5;
    private const int SummaryColStart = 6;
    private const int SummaryColEnd = 7;
    private const int SummaryColCorrSource = 8;
    private const int SummaryColCorrCompare = 9;
    private const int SummaryColCorrMatch = 10;
    private const int SummaryColResult = 11;
    private const int SummaryColAdded = 12;
    private const int SummaryColDeleted = 13;
    private const int SummaryColChanged = 14;
    private const int SummaryColMoved = 15;
    private const int SummaryColUnchanged = 16;

    public static void FillWorkbook(
        ExcelPackage package,
        string sourceFileName,
        string compareFileName,
        IReadOnlyList<ReportCompareResult> results,
        Action<int, string>? progress = null,
        int progressStartPercent = 0,
        int progressSpanPercent = 100)
    {
        var safeSpan = Math.Max(1, progressSpanPercent);
        var detailStart = progressStartPercent + Math.Max(1, safeSpan / 4);
        var detailSpan = Math.Max(1, safeSpan - (detailStart - progressStartPercent));

        progress?.Invoke(progressStartPercent, "excel: подготовка листов");
        while (package.Workbook.Worksheets.Count > 0)
        {
            package.Workbook.Worksheets.Delete(0);
        }

        WriteSummary(package, sourceFileName, compareFileName, results);
        progress?.Invoke(progressStartPercent + Math.Max(1, safeSpan / 8), "excel: заполнение сводки");
        WriteLegendSheet(package);
        progress?.Invoke(progressStartPercent + Math.Max(1, safeSpan / 6), "excel: заполнение легенды");

        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Сводка",
            "Легенда"
        };

        var detailResults = OrderForSheets(results).ToList();
        var detailTotal = Math.Max(1, detailResults.Count);
        for (var i = 0; i < detailResults.Count; i++)
        {
            var result = detailResults[i];
            var sheetName = UniqueSheetName(BuildSheetName(result.Left), usedNames);
            var sheet = package.Workbook.Worksheets.Add(sheetName);
            WriteReportSheet(sheet, result, sourceFileName, compareFileName);
            var percent = detailStart + (detailSpan * (i + 1)) / detailTotal;
            progress?.Invoke(percent, $"excel: лист {i + 1}/{detailTotal} ({sheetName})");
        }

        progress?.Invoke(progressStartPercent + safeSpan, "excel: готово");
    }

    /// <summary>Листы только при совпавшем ключе и отличиях.</summary>
    internal static IEnumerable<ReportCompareResult> OrderForSheets(IReadOnlyList<ReportCompareResult> results) =>
        results
            .Where(r => r.CreatesDetailSheet)
            .OrderBy(r => r.OrderOnlyChanged ? 1 : 0)
            .ThenBy(r => r.Left.RegNo)
            .ThenBy(r => r.Left.FormNum)
            .ThenBy(r => r.Left.PeriodKey);

    internal static IEnumerable<ReportCompareResult> OrderForSummary(IReadOnlyList<ReportCompareResult> results) =>
        results
            .OrderBy(r => r.Kind switch
            {
                ReportCompareKind.PeriodOverlap => 0,
                ReportCompareKind.Compared when !r.IsIdentical && !r.OrderOnlyChanged => 1,
                ReportCompareKind.Compared when r.OrderOnlyChanged => 2,
                ReportCompareKind.Compared => 3,
                ReportCompareKind.MissingInSourceDb => 4,
                _ => 5
            })
            .ThenBy(r => SummaryFileSide(r).RegNo)
            .ThenBy(r => SummaryFileSide(r).FormNum)
            .ThenBy(r => SummaryFileSide(r).PeriodKey);

    /// <summary>Сторона из файла сверки — основа строки сводки.</summary>
    private static CompareReportDto SummaryFileSide(ReportCompareResult result) =>
        result.Right ?? result.Left;

    private static void WriteSummary(
        ExcelPackage package,
        string sourceFileName,
        string compareFileName,
        IReadOnlyList<ReportCompareResult> results)
    {
        var sheet = package.Workbook.Worksheets.Add("Сводка");

        // Шапка на всю ширину таблицы — иначе AutoFit раздувает колонки «Форма» / даты.
        // Рег.№ / ОКПО только в таблице: организаций может быть много.
        WriteMergedHeaderRow(
            sheet,
            1,
            SummaryColCount,
            "Сверка отчётов с БД (исходная БД МПЗФ ↔ файл(ы) для сверки; из БД — только организации из файлов)",
            bold: true,
            wrap: true,
            minHeight: 30);
        WriteMergedHeaderRow(sheet, 2, SummaryColCount, $"Исходник: {sourceFileName}");
        WriteMergedHeaderRow(sheet, 3, SummaryColCount, $"Файлы для сверки: {compareFileName}");
        WriteMergedHeaderRow(
            sheet,
            4,
            SummaryColCount,
            "Легенда подсветки и расшифровка счётчиков — на листе «Легенда». Листы сравнения создаются только при отличиях.",
            wrap: true,
            minHeight: 28);

        var compared = results.Where(r => r.Kind == ReportCompareKind.Compared).ToList();
        var identical = compared.Count(r => r.IsIdentical);
        var orderOnly = compared.Count(r => r.OrderOnlyChanged);
        var changed = compared.Count(r => !r.IsIdentical && !r.OrderOnlyChanged);
        var missingInSource = results.Count(r => r.Kind == ReportCompareKind.MissingInSourceDb);
        var overlap = results.Count(r => r.Kind == ReportCompareKind.PeriodOverlap);

        WriteSummaryCounterRow(sheet, 6, "Отчётов в файлах сверки", results.Count);
        WriteSummaryCounterRow(sheet, 7, "Полностью совпадают (без отдельных листов)", identical);
        WriteSummaryCounterRow(sheet, 8, "Изменён только порядок строк", orderOnly);
        WriteSummaryCounterRow(sheet, 9, "Есть изменения данных", changed);
        WriteSummaryCounterRow(sheet, 10, "Отсутствует в текущей БД (без отдельных листов)", missingInSource);
        WriteSummaryCounterRow(sheet, 11, "Период пересекается, но не совпадает (без отдельных листов)", overlap);

        const int headerRow = 13;
        sheet.Cells[headerRow, SummaryColRegNo].Value = "Рег.№";
        sheet.Cells[headerRow, SummaryColShortName].Value = "Сокр. наименование";
        sheet.Cells[headerRow, SummaryColOkpo].Value = "ОКПО";
        sheet.Cells[headerRow, SummaryColFile].Value = "Файл сверки";
        sheet.Cells[headerRow, SummaryColForm].Value = "Форма";
        sheet.Cells[headerRow, SummaryColStart].Value = "Начало отчёта";
        sheet.Cells[headerRow, SummaryColEnd].Value = "Конец отчёта";
        sheet.Cells[headerRow, SummaryColCorrSource].Value = "Корр. исходник";
        sheet.Cells[headerRow, SummaryColCorrCompare].Value = "Корр. сравнение";
        sheet.Cells[headerRow, SummaryColCorrMatch].Value = "Корректировки";
        sheet.Cells[headerRow, SummaryColResult].Value = "Результат";
        sheet.Cells[headerRow, SummaryColAdded].Value = "Добавлено (+)";
        sheet.Cells[headerRow, SummaryColDeleted].Value = "Удалено (−)";
        sheet.Cells[headerRow, SummaryColChanged].Value = "Изменено (~)";
        sheet.Cells[headerRow, SummaryColMoved].Value = "Перемещено (↔)";
        sheet.Cells[headerRow, SummaryColUnchanged].Value = "Без изменений (=)";
        using (var range = sheet.Cells[headerRow, 1, headerRow, SummaryColCount])
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.SetBackground(SourceHeaderFill, ExcelFillStyle.Solid);
            range.Style.WrapText = true;
        }

        var row = headerRow + 1;
        foreach (var result in OrderForSummary(results))
        {
            var fromFile = SummaryFileSide(result);
            sheet.Cells[row, SummaryColRegNo].Value = fromFile.RegNo;
            sheet.Cells[row, SummaryColShortName].Value = fromFile.OrgShortName;
            sheet.Cells[row, SummaryColOkpo].Value = fromFile.Okpo;
            sheet.Cells[row, SummaryColFile].Value = fromFile.SourceLabel;

            WriteSummaryPeriod(sheet, row, fromFile);

            if (result.Kind == ReportCompareKind.MissingInSourceDb)
            {
                sheet.Cells[row, SummaryColCorrSource].Value = "—";
                sheet.Cells[row, SummaryColCorrCompare].Value = fromFile.CorrectionNumber;
            }
            else
            {
                sheet.Cells[row, SummaryColCorrSource].Value = result.Left.CorrectionNumber;
                sheet.Cells[row, SummaryColCorrCompare].Value = result.Right?.CorrectionNumber;
            }

            WriteCorrectionMatch(sheet, row, result);

            sheet.Cells[row, SummaryColResult].Value = result.Kind switch
            {
                ReportCompareKind.MissingInSourceDb => "Отсутствует в текущей БД (лист не создан)",
                ReportCompareKind.PeriodOverlap =>
                    $"Период пересекается, но не совпадает (в БД: {result.Left.PeriodDisplay}; лист не создан)",
                _ when result.IsIdentical => "Совпадает (лист не создан)",
                _ when result.OrderOnlyChanged => "Только порядок строк",
                _ => "Есть изменения"
            };
            sheet.Cells[row, SummaryColAdded].Value = result.AddedCount;
            sheet.Cells[row, SummaryColDeleted].Value = result.DeletedCount;
            sheet.Cells[row, SummaryColChanged].Value = result.ChangedCount;
            sheet.Cells[row, SummaryColMoved].Value = result.MovedCount;
            sheet.Cells[row, SummaryColUnchanged].Value = result.UnchangedCount;
            if (result.IsIdentical)
            {
                sheet.Cells[row, SummaryColResult].Style.Fill.SetBackground(ExactFill, ExcelFillStyle.Solid);
            }

            row++;
        }

        var lastDataRow = Math.Max(row - 1, headerRow);
        if (lastDataRow > headerRow)
        {
            sheet.Cells[headerRow, 1, lastDataRow, SummaryColCount].AutoFilter = true;
        }

        // Только таблица: шапка уже в merge и не должна раздувать узкие колонки.
        sheet.Cells[headerRow, 1, lastDataRow, SummaryColCount].AutoFitColumns();
        // A/B заняты счётчиками (подпись + число) — A шире подписи, не сжимать до «Рег.№».
        sheet.Column(1).Width = Math.Max(sheet.Column(1).Width, 48);
        sheet.Column(2).Width = Math.Max(sheet.Column(2).Width, 8);
        sheet.Column(SummaryColForm).Width = Math.Min(Math.Max(sheet.Column(SummaryColForm).Width, 8), 10);
        sheet.Column(SummaryColStart).Width = Math.Min(Math.Max(sheet.Column(SummaryColStart).Width, 12), 14);
        sheet.Column(SummaryColEnd).Width = Math.Min(Math.Max(sheet.Column(SummaryColEnd).Width, 12), 14);
        sheet.Column(SummaryColResult).Width = Math.Max(sheet.Column(SummaryColResult).Width, 28);
        sheet.Column(SummaryColShortName).Width = Math.Max(sheet.Column(SummaryColShortName).Width, 18);
        sheet.Column(SummaryColFile).Width = Math.Max(sheet.Column(SummaryColFile).Width, 18);

        WidenSummaryColumn(sheet, SummaryColCorrSource, min: 15, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColCorrCompare, min: 16, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColCorrMatch, min: 14, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColAdded, min: 14, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColDeleted, min: 13, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColChanged, min: 13, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColMoved, min: 15, extra: 1.5);
        WidenSummaryColumn(sheet, SummaryColUnchanged, min: 17, extra: 1.5);
    }

    private static void WidenSummaryColumn(ExcelWorksheet sheet, int column, double min, double extra)
    {
        sheet.Column(column).Width = Math.Max(sheet.Column(column).Width + extra, min);
    }

    /// <summary>Подпись счётчика в A, число сразу в B — видно без горизонтального скролла.</summary>
    private static void WriteSummaryCounterRow(ExcelWorksheet sheet, int row, string label, int value)
    {
        sheet.Cells[row, 1].Value = label;
        sheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        sheet.Cells[row, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        sheet.Cells[row, 2].Value = value;
        sheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        sheet.Cells[row, 2].Style.Font.Bold = true;
    }

    private static void WriteSummaryPeriod(ExcelWorksheet sheet, int row, CompareReportDto report)
    {
        sheet.Cells[row, SummaryColForm].Value = report.FormNum;
        if (FormPrintCompareNormalize.IsYearOnlyForm(report.FormNum))
        {
            var year = string.IsNullOrWhiteSpace(report.Year)
                ? report.PeriodDisplay
                : string.Concat(report.Year.Where(char.IsDigit));
            if (string.IsNullOrEmpty(year))
            {
                year = report.PeriodDisplay;
            }

            sheet.Cells[row, SummaryColStart].Value = year;
            sheet.Cells[row, SummaryColStart, row, SummaryColEnd].Merge = true;
            sheet.Cells[row, SummaryColStart].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            return;
        }

        sheet.Cells[row, SummaryColStart].Value = FormPrintCompareNormalize.NormalizePeriodPart(report.StartPeriod);
        sheet.Cells[row, SummaryColEnd].Value = FormPrintCompareNormalize.NormalizePeriodPart(report.EndPeriod);
    }

    private static void WriteCorrectionMatch(ExcelWorksheet sheet, int row, ReportCompareResult result)
    {
        if (result.Kind is ReportCompareKind.MissingInCompareFiles or ReportCompareKind.MissingInSourceDb)
        {
            sheet.Cells[row, SummaryColCorrMatch].Value = "—";
            return;
        }

        if (!result.HasRight)
        {
            sheet.Cells[row, SummaryColCorrMatch].Value = "—";
            return;
        }

        var match = result.Left.CorrectionNumber == result.Right!.CorrectionNumber;
        sheet.Cells[row, SummaryColCorrMatch].Value = match ? "совпадают" : "различаются";
        var fill = match ? ExactFill : MismatchFill;
        sheet.Cells[row, SummaryColCorrSource].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
        sheet.Cells[row, SummaryColCorrCompare].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
        sheet.Cells[row, SummaryColCorrMatch].Style.Fill.SetBackground(fill, ExcelFillStyle.Solid);
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
        WriteLegendItem(sheet, 4, "Совпадение — значения одинаковые (в том числе при разной записи одного числа)", ExactFill);
        WriteLegendItem(sheet, 5, "Мелкое отличие — почти то же, но не совпадает; скрытая разница поясняется в ячейке", NearFill);
        WriteLegendItem(sheet, 6, "Заметное отличие — существенная разница значений", MismatchFill);

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
        sheet.Cells[27, 1].Value =
            "Отсутствует в текущей БД — есть в файле сверки, в открытой БД нет; лист не создаётся";
        sheet.Cells[28, 1].Value =
            "Период пересекается — даты не те же, интервалы пересекаются; лист не создаётся";

        sheet.Cells[30, 1].Value = "Как читать сравнение";
        sheet.Cells[30, 1].Style.Font.Bold = true;
        sheet.Cells[31, 1].Value =
            "Сравнение ячеек — по всем колонкам формы, не только по полям, по которым строки сопоставляются.";
        sheet.Cells[32, 1].Value =
            "Скрытое отличие (невидимый символ, похожие буквы разной раскладки) — пояснение в ячейке.";
        sheet.Cells[33, 1].Value = "Слева — исходник (имя файла БД), справа — файл(ы) для сравнения.";
        sheet.Cells[34, 1].Value =
            "На листе «Сводка» — по одной строке на отчёт из файла сверки. Отдельный лист — только если найден такой же отчёт в БД и есть отличия.";
        sheet.Cells[35, 1].Value =
            "Формы 1.7/1.8: строки с пустыми кодом и датой операции — раскладка к заглавной строке выше; сопоставляются внутри этой группы по радионуклиду.";

        sheet.Cells[1, 1, 35, 1].AutoFitColumns();
        // Легенда читается без горизонтального скролла: длинные пояснения — отдельными строками.
        sheet.Column(1).Width = Math.Min(Math.Max(sheet.Column(1).Width, 55), 75);
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
            $"Отчёт из текущей БД: {sourceFileName}  |  Отчёт на сравнение из файла: {compareFileName}");

        var countsText =
            $"Строки: без изменений (={result.UnchangedCount}), изменено (~{result.ChangedCount}), " +
            $"сбит № п/п (↔{result.MovedCount}), удалено (−{result.DeletedCount}), добавлено (+{result.AddedCount}).";

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
                result.Message ?? "Отчёт из файла сверки отсутствует в текущей БД.",
                fill: DeletedFill, bold: true, wrap: true, minHeight: 30);
            sheet.Column(statusCol).Width = 36;
            return;
        }

        sheet.Cells[headerRow, leftStart].Value = $"Отчёт из текущей БД ({sourceFileName})";
        sheet.Cells[headerRow, leftStart, headerRow, leftStart + colCount - 1].Merge = true;
        sheet.Cells[headerRow, leftStart].Style.Fill.SetBackground(SourceHeaderFill, ExcelFillStyle.Solid);
        sheet.Cells[headerRow, leftStart].Style.Font.Bold = true;

        sheet.Cells[headerRow, rightStart].Value = $"Отчёт на сравнение из файла ({compareFileName})";
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

    /// <summary>
    /// Имя листа: «5цифрРег_форма_период», год в датах — 2 цифры.
    /// Индекс (_2, _3…) добавляет <see cref="UniqueSheetName"/> только при коллизии.
    /// </summary>
    internal static string BuildSheetName(CompareReportDto report)
    {
        var reg = FormatRegNoFiveDigits(report.RegNo);
        var period = FormatPeriodForSheetName(report);
        return SanitizeSheetName($"{reg}_{report.FormNum}_{period}");
    }

    private static string FormatRegNoFiveDigits(string? regNo)
    {
        var digits = string.Concat((regNo ?? string.Empty).Where(char.IsDigit));
        if (digits.Length == 0)
        {
            return "00000";
        }

        if (digits.Length >= 5)
        {
            return digits[..5];
        }

        return digits.PadLeft(5, '0');
    }

    private static string FormatPeriodForSheetName(CompareReportDto report)
    {
        if (FormPrintCompareNormalize.IsYearOnlyForm(report.FormNum))
        {
            var yearDigits = string.Concat((report.Year ?? report.PeriodKey ?? string.Empty).Where(char.IsDigit));
            if (yearDigits.Length >= 2)
            {
                return yearDigits[^2..];
            }

            return string.IsNullOrEmpty(yearDigits) ? "бг" : yearDigits;
        }

        var start = FormatDateTwoDigitYear(report.StartPeriod);
        var end = FormatDateTwoDigitYear(report.EndPeriod);
        if (string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end))
        {
            return "бп";
        }

        return $"{start}-{end}";
    }

    private static string FormatDateTwoDigitYear(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var trimmed = value.Trim().Replace('/', '.');
        var ru = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
        if (DateOnly.TryParse(trimmed, ru, System.Globalization.DateTimeStyles.None, out var date)
            || DateOnly.TryParse(trimmed, out date))
        {
            return date.ToString("dd.MM.yy");
        }

        return trimmed;
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

    /// <summary>Добавляет _2, _3… только если базовое имя уже занято.</summary>
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
