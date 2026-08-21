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
        Assert.Equal("Совпадает (лист не создан)", summary.Cells[15, 7].Text);
        Assert.Equal("Начало отчёта", summary.Cells[14, 2].Text);
        Assert.Equal("Конец отчёта", summary.Cells[14, 3].Text);
        Assert.Equal("01.01.2024", summary.Cells[15, 2].Text);
        Assert.Equal("31.12.2024", summary.Cells[15, 3].Text);
        Assert.Equal("различаются", summary.Cells[15, 6].Text);
        Assert.Equal("source.RAODB", summary.Cells[4, 2].Text);
        Assert.Equal("compare.RAODB", summary.Cells[5, 2].Text);
    }

    [Fact]
    public void MissingRight_SheetHasMissingMessage()
    {
        var left = Left(R(1, 0, 1, "PAS1"));
        var result = FormPrintCompareTestAccess.Compare(left, null);

        using var package = FormPrintCompareTestAccess.BuildWorkbookForTests(
            "12345", "12345678", "compare.RAODB", [result]);

        Assert.Equal(3, package.Workbook.Worksheets.Count);
        var sheet = package.Workbook.Worksheets[2];
        Assert.Contains("отсутствует отчёт для сверки", sheet.Cells[FormPrintCompareTestAccess.ReportDataStartRow, 1].Text);
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
        var report = package.Workbook.Worksheets[2];
        var leftStart = 2;
        var rightStart = 2 + result.Left.Columns.Length + 2;
        var headerRow = FormPrintCompareTestAccess.ReportDataStartRow - 2;
        Assert.Equal("Исходник (source.RAODB)", report.Cells[headerRow, leftStart].Text);
        Assert.Equal("Для сравнения (compare.RAODB)", report.Cells[headerRow, rightStart].Text);
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

        Assert.Equal("Изменена", sheet.Cells[row, 1].Text);
        Assert.Contains("ZWSP", rightPassport);
        Assert.NotEqual(leftPassport, rightPassport);
    }
}
