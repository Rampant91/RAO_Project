using System;
using System.Linq;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using OfficeOpenXml;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelParserForm11Tests
{
    public FormsExcelParserForm11Tests()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public void Headers_Match_ExactExportLayout()
    {
        using var package = CreatePackageWithHeaders(FormsExcelExportHeadersForm11.ReportColumns);
        Assert.True(FormsExcelExportParserForm11.HeadersMatch(
            package.Workbook.Worksheets[0],
            FormsExcelExportHeadersForm11.ReportColumns,
            out var error));
        Assert.Null(error);
    }

    [Fact]
    public void Headers_Reject_WrongColumn()
    {
        var headers = FormsExcelExportHeadersForm11.ReportColumns.ToArray();
        headers[3] = "Рег. №";
        using var package = CreatePackageWithHeaders(headers);
        Assert.False(FormsExcelExportParserForm11.HeadersMatch(
            package.Workbook.Worksheets[0],
            FormsExcelExportHeadersForm11.ReportColumns,
            out var error));
        Assert.Contains("Рег.№", error);
    }

    [Fact]
    public void TryReadReportKey_Rejects_Missing_RegNo()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("sheet");
        ws.Cells[2, 2].Value = "11111111";
        ws.Cells[2, 5].Value = 0;
        ws.Cells[2, 6].Value = new DateTime(2024, 1, 1);
        ws.Cells[2, 7].Value = new DateTime(2024, 3, 31);

        Assert.False(FormsExcelExportParserForm1.TryReadReportKey(ws, 2, out _, out var error));
        Assert.Contains("ОКПО", error, StringComparison.Ordinal);
    }

    [Fact]
    public void Group_Splits_By_CorrectionNumber()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 2, "12345", "11111111", 0, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 3, "12345", "11111111", 1, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));

        var result = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Equal(2, result.Groups.Count);
        Assert.Equal((byte)0, result.Groups[0].Key.CorrectionNumber);
        Assert.Equal((byte)1, result.Groups[1].Key.CorrectionNumber);
        Assert.All(result.Groups, g => Assert.Single(g.Rows));
    }

    [Fact]
    public void Group_DateTime_And_Text_Same_Key()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 2, "12345", "11111111", 0, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
        ws.Cells[3, 2].Value = "11111111";
        ws.Cells[3, 4].Value = "12345";
        ws.Cells[3, 5].Value = 0;
        ws.Cells[3, 6].Value = "01.01.2024";
        ws.Cells[3, 7].Value = "31.03.2024";
        ws.Cells[3, 8].Value = 2;
        ws.Cells[3, 9].Value = "11";

        var result = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Single(result.Groups);
        Assert.Equal(2, result.Groups[0].Rows.Count);
        Assert.Equal("01.01.2024", result.Groups[0].Key.StartPeriod);
        Assert.Equal("31.03.2024", result.Groups[0].Key.EndPeriod);
    }

    [Fact]
    public void Group_Allows_Open_End_Period()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 2, "12345", "11111111", 0, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
        ws.Cells[3, 2].Value = "22222222";
        ws.Cells[3, 4].Value = "54321";
        ws.Cells[3, 5].Value = 0;
        ws.Cells[3, 6].Value = new DateTime(2024, 4, 1);
        ws.Cells[3, 7].Value = "-";
        ws.Cells[3, 8].Value = 1;
        ws.Cells[3, 9].Value = "11";

        var result = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Equal(2, result.Groups.Count);
        Assert.Equal("-", result.Groups.Single(g => g.Key.RegNo == "54321").Key.EndPeriod);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Notes_Attach_With_Open_End_Period()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        ws.Cells[2, 2].Value = "11111111";
        ws.Cells[2, 4].Value = "12345";
        ws.Cells[2, 5].Value = 0;
        ws.Cells[2, 6].Value = new DateTime(2024, 1, 1);
        ws.Cells[2, 7].Value = "-";
        ws.Cells[2, 8].Value = 1;
        ws.Cells[2, 9].Value = "11";

        var notes = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.NotesSheetName);
        FormsExcelTestHelpers.WriteHeaders(notes, FormsExcelExportHeadersForm11.NotesColumns);
        notes.Cells[2, 1].Value = "11111111";
        notes.Cells[2, 3].Value = "12345";
        notes.Cells[2, 4].Value = 0;
        notes.Cells[2, 5].Value = new DateTime(2024, 1, 1);
        notes.Cells[2, 6].Value = "-";
        notes.Cells[2, 7].Value = "1";
        notes.Cells[2, 8].Value = "2";
        notes.Cells[2, 9].Value = "пояснение";

        var result = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Single(result.Groups);
        Assert.Single(result.Groups[0].Notes);
    }

    [Fact]
    public void Notes_Attach_By_Key_MissingSheet_Ok()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 2, "12345", "11111111", 0, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));

        var withoutNotes = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Empty(withoutNotes.Groups[0].Notes);

        var notes = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.NotesSheetName);
        FormsExcelTestHelpers.WriteHeaders(notes, FormsExcelExportHeadersForm11.NotesColumns);
        notes.Cells[2, 1].Value = "11111111";
        notes.Cells[2, 3].Value = "12345";
        notes.Cells[2, 4].Value = 0;
        notes.Cells[2, 5].Value = new DateTime(2024, 1, 1);
        notes.Cells[2, 6].Value = new DateTime(2024, 3, 31);
        notes.Cells[2, 7].Value = "1";
        notes.Cells[2, 8].Value = "2";
        notes.Cells[2, 9].Value = "пояснение";

        var withNotes = FormsExcelExportParserForm11.ParseWorkbook(package);
        Assert.Single(withNotes.Groups[0].Notes);
        Assert.Equal("пояснение", withNotes.Groups[0].Notes[0].Comment_DB);
    }

    [Fact]
    public void ReportKey_ToString_Uses_User_Friendly_Labels()
    {
        var key = new FormsExcelReportKey("12345", "11111111", "01.01.2024", "-", 2);
        Assert.Contains("№ кор. 2", key.ToString(), StringComparison.Ordinal);
        Assert.Contains("с 01.01.2024", key.ToString(), StringComparison.Ordinal);
    }

    private static ExcelPackage CreatePackageWithHeaders(string[] headers)
    {
        var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("sheet");
        FormsExcelTestHelpers.WriteHeaders(ws, headers);
        return package;
    }
}
