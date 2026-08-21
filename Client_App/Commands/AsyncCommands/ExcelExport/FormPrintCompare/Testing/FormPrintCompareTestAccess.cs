using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ExcelExport.FormPrintCompare.Testing;

/// <summary>
/// Публичный фасад для unit-тестов сравнения form-print (доступ к internal API).
/// </summary>
public static class FormPrintCompareTestAccess
{
    public static string NormalizeId(string? value) => FormPrintCompareNormalize.NormalizeId(value);

    public static string NormalizeRads(string? value) => FormPrintCompareNormalize.NormalizeRads(value);

    public static string BuildPeriodKey(string formNum, string? start, string? end, string? year) =>
        FormPrintCompareNormalize.BuildPeriodKey(formNum, start, end, year);

    public static FieldMatchLevel CompareCellLevel(
        string formNum,
        int columnIndex,
        string? left,
        string? right)
    {
        var columns = FormPrintCompareSchema.ColumnsFor(formNum);
        return FormPrintCompareNormalize.CompareCell(columns[columnIndex], left, right).Level;
    }

    public static string FormatCellForExport(string? value, string? other, FieldMatchLevel level) =>
        FormPrintCompareNormalize.FormatCellForExport(value, other, level);

    public static CompareReportDto CreateReport11(
        string periodStart,
        string periodEnd,
        byte correction,
        params CompareRowDto[] rows) =>
        new()
        {
            ReportId = 1,
            FormNum = "1.1",
            PeriodKey = FormPrintCompareNormalize.BuildPeriodKey("1.1", periodStart, periodEnd, null),
            PeriodDisplay = FormPrintCompareNormalize.BuildPeriodDisplay("1.1", periodStart, periodEnd, null),
            StartPeriod = periodStart,
            EndPeriod = periodEnd,
            CorrectionNumber = correction,
            RegNo = "12345",
            Okpo = "12345678",
            Columns = FormPrintCompareSchema.ColumnsFor("1.1"),
            Rows = rows.ToList()
        };

    public static CompareRowDto Row11(
        int id,
        int index,
        int numberInOrder,
        string opCode,
        string opDate,
        string passport,
        string type,
        string factory,
        string activity = "1.0e+3",
        string quantity = "1",
        string rads = "Co-60")
    {
        var columns = FormPrintCompareSchema.ColumnsFor("1.1");
        var values = new string[columns.Length];
        for (var i = 0; i < values.Length; i++)
        {
            values[i] = "";
        }

        values[0] = numberInOrder.ToString();
        values[1] = opCode;
        values[2] = opDate;
        values[3] = passport;
        values[4] = type;
        values[5] = rads;
        values[6] = factory;
        values[7] = quantity;
        values[8] = activity;

        return new CompareRowDto
        {
            Id = id,
            SourceIndex = index,
            NumberInOrder = numberInOrder,
            OpDate = opDate,
            Values = values,
            Fingerprint = FormPrintCompareNormalize.BuildFingerprint(columns, values)
        };
    }

    public static ReportCompareResult Compare(CompareReportDto left, CompareReportDto? right) =>
        FormPrintCompareMatcher.Compare(left, right);

    public static ExcelPackage BuildWorkbookForTests(
        string regNo,
        string okpo,
        string etalonName,
        IReadOnlyList<ReportCompareResult> results,
        string sourceName = "source.RAODB")
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var package = new ExcelPackage();
        FormPrintCompareExcel.FillWorkbook(package, regNo, okpo, sourceName, etalonName, results);
        return package;
    }

    public static Color? FillForDifference(FieldMatchLevel level) =>
        FormPrintCompareExcel.FillForDifference(level);

    public static Color NearFill => FormPrintCompareExcel.NearFill;
    public static Color MismatchFill => FormPrintCompareExcel.MismatchFill;
    public static Color DeletedFill => FormPrintCompareExcel.DeletedFill;
    public static Color AddedFill => FormPrintCompareExcel.AddedFill;
    public static Color ExactFill => FormPrintCompareExcel.ExactFill;
    public static Color DuplicateNppFill => FormPrintCompareExcel.DuplicateNppFill;
    public static int ReportDataStartRow => FormPrintCompareExcel.ReportDataStartRow;

    public static Color? FillForLevel(FieldMatchLevel level) =>
        FormPrintCompareExcel.FillForLevel(level);

    public static Color StatusColumnFill(DiffRowStatus status) =>
        FormPrintCompareExcel.StatusColumnFill(status);

    public static Color ConfidenceFill(int percent) =>
        FormPrintCompareExcel.ConfidenceFill(percent);

    public static int PassportColumnIndex => 3;
    public static int FactoryColumnIndex => 6;
    public static int ActivityColumnIndex => 8;
}
