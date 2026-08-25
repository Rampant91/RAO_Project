using System.Drawing;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare;
using Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using OfficeOpenXml;
using Xunit;

namespace Test.FormPrintCompare;

/// <summary>
/// Проверка содержимого и подсветки ячеек в выгрузке Excel.
/// </summary>
public class FormPrintCompareExcelLayoutTests
{
    private static string ToExcelRgb(Color color) =>
        $"FF{color.R:X2}{color.G:X2}{color.B:X2}";

    private static CompareReportDto Left(params CompareRowDto[] rows) =>
        FormPrintCompareTestAccess.CreateReport11("01.01.2024", "31.12.2024", 0, rows);

    private static CompareReportDto Right(params CompareRowDto[] rows) =>
        FormPrintCompareTestAccess.CreateReport11("01.01.2024", "31.12.2024", 1, rows);

    private static CompareRowDto R(
        int id,
        int index,
        int npp,
        string passport,
        string factory = "F1",
        string activity = "1.0e+3") =>
        FormPrintCompareTestAccess.Row11(
            id, index, npp, "11", "15.03.2024", passport, "ИИИ", factory, activity);

    [Fact]
    public void Identical_OnlySummary_NoReportSheet()
    {
        var left = Left(R(1, 0, 1, "PAS1"));
        var right = Right(R(2, 0, 1, "PAS1"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        Assert.Equal(2, package.Workbook.Worksheets.Count); // Сводка + Легенда
        Assert.Equal("Сводка", package.Workbook.Worksheets[0].Name);
        Assert.Equal("Легенда", package.Workbook.Worksheets[1].Name);
        var summary = package.Workbook.Worksheets[0];
        Assert.Equal("Совпадает (лист не создан)", summary.Cells[14, 11].Text);
        Assert.Equal("Начало отчёта", summary.Cells[13, 6].Text);
        Assert.Equal("Конец отчёта", summary.Cells[13, 7].Text);
        Assert.Equal("01.01.2024", summary.Cells[14, 6].Text);
        Assert.Equal("31.12.2024", summary.Cells[14, 7].Text);
        Assert.Equal("различаются", summary.Cells[14, 10].Text);
        Assert.Equal("Исходник: source.RAODB", summary.Cells[2, 1].Text);
        Assert.Equal("Файлы для сверки: compare.RAODB", summary.Cells[3, 1].Text);
        Assert.Equal("Отчётов в файлах сверки", summary.Cells[6, 1].Text);
        Assert.Equal(1, summary.Cells[6, 2].GetValue<int>());
        Assert.True(summary.Cells[1, 1, 1, 16].Merge);
        Assert.False(summary.Cells[6, 1, 6, 15].Merge);
        Assert.True(summary.Column(5).Width <= 10.5);
        Assert.True(summary.Column(6).Width <= 14.5);
    }

    [Fact]
    public void MissingRight_DoesNotCreateDetailSheet()
    {
        var left = Left(R(1, 0, 1, "PAS1"));
        var result = FormPrintCompareTestAccess.Compare(left, null);

        Assert.Equal(ReportCompareKind.MissingInCompareFiles, result.Kind);
        Assert.False(result.CreatesDetailSheet);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        Assert.Equal(2, package.Workbook.Worksheets.Count); // Сводка + Легенда
    }

    [Fact]
    public void LegendSheet_Exists_AndRightHeaderIsForCompare()
    {
        var left = Left(R(1, 0, 1, "GONE"));
        var right = Right(R(2, 0, 1, "NEW"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        Assert.Equal("Легенда", package.Workbook.Worksheets[1].Name);
        var legend = package.Workbook.Worksheets[1];
        Assert.Equal("Как читать сравнение", legend.Cells[30, 1].Text);
        Assert.Contains("Сводка", legend.Cells[34, 1].Text);
        Assert.True(legend.Column(1).Width <= 75.5);
        var report = package.Workbook.Worksheets[2];
        var leftStart = 2;
        var rightStart = 2 + result.Left.Columns.Length + 2;
        var headerRow = FormPrintCompareTestAccess.ReportDataStartRow - 2;
        Assert.Equal("Отчёт из текущей БД (source.RAODB)", report.Cells[headerRow, leftStart].Text);
        Assert.Equal("Отчёт на сравнение из файла (compare.RAODB)", report.Cells[headerRow, rightStart].Text);
        Assert.Equal("12345_1.1_01.01.24-31.12.24", report.Name);
    }

    [Fact]
    public void SheetName_UsesRegFormPeriod_TwoDigitYear_IndexOnlyOnCollision()
    {
        var a = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "98765", "okpo1", 1,
            R(1, 0, 1, "A"));
        var b = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 1, "98765", "okpo1", 2,
            R(2, 0, 1, "B"));
        // Одинаковый ключ периода/формы/рег — разные корректировки; оба с отличиями → два листа.
        var leftA = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "98765", "okpo1", 10,
            R(1, 0, 1, "LEFT1"));
        var rightA = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 0, "98765", "okpo1", 11,
            R(2, 0, 1, "RIGHT1"));
        var leftB = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 2, "98765", "okpo1", 20,
            R(3, 0, 1, "LEFT2"));
        var rightB = FormPrintCompareTestAccess.CreateReport11(
            "01.01.2024", "31.03.2024", 2, "98765", "okpo1", 21,
            R(4, 0, 1, "RIGHT2"));

        Assert.Equal("98765_1.1_01.01.24-31.03.24", FormPrintCompareTestAccess.BuildSheetName(a));
        Assert.Equal("98765_1.1_01.01.24-31.03.24", FormPrintCompareTestAccess.BuildSheetName(b));

        var results = new[]
        {
            FormPrintCompareTestAccess.Compare(leftA, rightA),
            FormPrintCompareTestAccess.Compare(leftB, rightB)
        };
        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "98765", "okpo1", "compare.RAODB", results);

        Assert.Equal("98765_1.1_01.01.24-31.03.24", package.Workbook.Worksheets[2].Name);
        Assert.Equal("98765_1.1_01.01.24-31.03.24_2", package.Workbook.Worksheets[3].Name);
    }

    [Fact]
    public void DeletedAndAdded_ValuesAndFills_AreCorrect()
    {
        var left = Left(R(1, 0, 1, "GONE"));
        var right = Right(R(2, 0, 1, "NEW"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var dataRow = FormPrintCompareTestAccess.ReportDataStartRow;
        var columns = result.Left.Columns.Length;
        var leftStart = 2;
        var rightStart = leftStart + columns + 2; // sep + confidence

        var statuses = new[]
        {
            sheet.Cells[dataRow, 1].Text,
            sheet.Cells[dataRow + 1, 1].Text
        };
        Assert.Contains(statuses, s => s.Contains("Удалена"));
        Assert.Contains(statuses, s => s.Contains("Добавлена"));

        var deletedRow = statuses[0].Contains("Удалена") ? dataRow : dataRow + 1;
        var addedRow = statuses[0].Contains("Добавлена") ? dataRow : dataRow + 1;

        Assert.Equal("GONE", sheet.Cells[deletedRow, leftStart + FormPrintCompareTestAccess.PassportColumnIndex].Text);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.DeletedFill),
            sheet.Cells[deletedRow, leftStart].Style.Fill.BackgroundColor.Rgb);

        Assert.Equal("NEW", sheet.Cells[addedRow, rightStart + FormPrintCompareTestAccess.PassportColumnIndex].Text);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.AddedFill),
            sheet.Cells[addedRow, rightStart].Style.Fill.BackgroundColor.Rgb);

        Assert.Equal(0, sheet.Cells[deletedRow, leftStart + columns + 1].GetValue<int>());
    }

    [Fact]
    public void ChangedActivity_HighlightsExactGreen_AndMismatchAmber()
    {
        var left = Left(R(1, 0, 1, "PAS1", activity: "1.0e+3"));
        var right = Right(R(2, 0, 1, "PAS1", activity: "2.0e+3"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var row = FormPrintCompareTestAccess.ReportDataStartRow;
        var leftStart = 2;
        var activityCol = leftStart + FormPrintCompareTestAccess.ActivityColumnIndex;
        var passportCol = leftStart + FormPrintCompareTestAccess.PassportColumnIndex;
        var confidenceCol = leftStart + result.Left.Columns.Length + 1;

        Assert.Equal("Изменена", sheet.Cells[row, 1].Text);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.MismatchFill),
            sheet.Cells[row, 1].Style.Fill.BackgroundColor.Rgb);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.MismatchFill),
            sheet.Cells[row, activityCol].Style.Fill.BackgroundColor.Rgb);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.ExactFill),
            sheet.Cells[row, passportCol].Style.Fill.BackgroundColor.Rgb);
        Assert.InRange(sheet.Cells[row, confidenceCol].GetValue<int>(), 1, 99);
    }

    [Fact]
    public void UnchangedRow_ShownWithGreenAndConfidence100()
    {
        var left = Left(
            R(1, 0, 1, "KEEP"),
            R(2, 1, 2, "GONE"));
        var right = Right(R(10, 0, 1, "KEEP"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var dataRow = FormPrintCompareTestAccess.ReportDataStartRow;
        var leftStart = 2;
        var confidenceCol = leftStart + result.Left.Columns.Length + 1;
        var passportCol = leftStart + FormPrintCompareTestAccess.PassportColumnIndex;

        Assert.Contains(result.DisplayLines, l => l.Status == DiffRowStatus.Unchanged);
        var unchangedExcelRow = dataRow;
        while (!string.IsNullOrEmpty(sheet.Cells[unchangedExcelRow, 1].Text)
               && !sheet.Cells[unchangedExcelRow, 1].Text.Contains("Без изменений"))
        {
            unchangedExcelRow++;
        }

        Assert.Contains("Без изменений", sheet.Cells[unchangedExcelRow, 1].Text);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.ExactFill),
            sheet.Cells[unchangedExcelRow, 1].Style.Fill.BackgroundColor.Rgb);
        Assert.Equal(100, sheet.Cells[unchangedExcelRow, confidenceCol].GetValue<int>());
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.ExactFill),
            sheet.Cells[unchangedExcelRow, passportCol].Style.Fill.BackgroundColor.Rgb);
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.ExactFill),
            sheet.Cells[unchangedExcelRow, confidenceCol].Style.Fill.BackgroundColor.Rgb);
    }

    [Fact]
    public void Moved_StatusShowsNumbers_Confidence100()
    {
        var left = Left(R(1, 0, 1, "PAS1"), R(2, 1, 2, "PAS2"));
        var right = Right(R(20, 0, 1, "PAS2"), R(10, 1, 2, "PAS1"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var row = FormPrintCompareTestAccess.ReportDataStartRow;
        var confidenceCol = 2 + result.Left.Columns.Length + 1;
        Assert.Contains("№ п/п:", sheet.Cells[row, 1].Text);
        Assert.Equal(100, sheet.Cells[row, confidenceCol].GetValue<int>());
        var nppCol = 2; // left № п/п
        Assert.Equal(
            ToExcelRgb(FormPrintCompareTestAccess.MismatchFill),
            sheet.Cells[row, nppCol].Style.Fill.BackgroundColor.Rgb);
    }

    [Fact]
    public void FillForLevel_ExactNearMismatch_HaveColors()
    {
        Assert.Equal(
            FormPrintCompareTestAccess.ExactFill.ToArgb(),
            FormPrintCompareTestAccess.FillForLevel(FieldMatchLevel.Exact)!.Value.ToArgb());
        Assert.Equal(
            FormPrintCompareTestAccess.NearFill.ToArgb(),
            FormPrintCompareTestAccess.FillForLevel(FieldMatchLevel.Near)!.Value.ToArgb());
        Assert.Equal(
            FormPrintCompareTestAccess.MismatchFill.ToArgb(),
            FormPrintCompareTestAccess.FillForLevel(FieldMatchLevel.Mismatch)!.Value.ToArgb());
    }

    [Fact]
    public void ConfidenceFill_Allows100Green()
    {
        Assert.Equal(
            FormPrintCompareTestAccess.ExactFill.ToArgb(),
            FormPrintCompareTestAccess.ConfidenceFill(100).ToArgb());
        Assert.Equal(
            FormPrintCompareTestAccess.ExactFill.ToArgb(),
            FormPrintCompareTestAccess.ConfidenceFill(80).ToArgb());
        Assert.Equal(
            FormPrintCompareTestAccess.NearFill.ToArgb(),
            FormPrintCompareTestAccess.ConfidenceFill(50).ToArgb());
    }

    [Fact]
    public void ReportSheet_EnablesAutoFilterOnColumnHeaderRow()
    {
        var left = Left(R(1, 0, 1, "GONE"));
        var right = Right(R(2, 0, 1, "NEW"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var sectionRow = FormPrintCompareTestAccess.ReportDataStartRow - 1;
        Assert.NotNull(sheet.AutoFilterAddress);
        Assert.StartsWith($"A{sectionRow}:", sheet.AutoFilterAddress.Address);
    }

    [Fact]
    public void HiddenCharDifference_IsVisibleInExportedCells()
    {
        var left = Left(R(1, 0, 1, "74041/24/0124"));
        var right = Right(R(2, 0, 1, "74041/24/0124\u200B"));
        var result = FormPrintCompareTestAccess.Compare(left, right);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        var sheet = package.Workbook.Worksheets[2];
        var row = FormPrintCompareTestAccess.ReportDataStartRow;
        var leftPassport = sheet.Cells[row, 2 + FormPrintCompareTestAccess.PassportColumnIndex].Text;
        var rightPassport = sheet.Cells[
            row,
            2 + result.Left.Columns.Length + 2 + FormPrintCompareTestAccess.PassportColumnIndex].Text;

        Assert.NotEmpty(sheet.Cells[row, 1].Text);
        Assert.True(
            rightPassport.Contains("ZWSP") || rightPassport.Contains("\u200B"),
            $"Expected hidden marker in '{rightPassport}'.");
        Assert.NotEqual(leftPassport, rightPassport);
    }
}
