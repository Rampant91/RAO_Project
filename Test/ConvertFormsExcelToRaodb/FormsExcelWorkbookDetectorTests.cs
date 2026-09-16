using System;
using System.Linq;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using OfficeOpenXml;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelWorkbookDetectorTests
{
    public FormsExcelWorkbookDetectorTests()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public void Detect_By_Sheet_Name_And_Headers()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Отчеты 1.1");
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);

        var result = FormsExcelWorkbookDetector.Detect(package.Workbook);
        Assert.Single(result.Sheets);
        Assert.Equal("1.1", result.Sheets[0].Spec.FormNum);
        Assert.Equal(DetectionSource.SheetName, result.Sheets[0].Source);
    }

    [Fact]
    public void Detect_By_Headers_When_Sheet_Renamed()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Данные");
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);

        var result = FormsExcelWorkbookDetector.Detect(package.Workbook);
        Assert.Single(result.Sheets);
        Assert.Equal("1.1", result.Sheets[0].Spec.FormNum);
        Assert.Equal(DetectionSource.Headers, result.Sheets[0].Source);
    }

    [Fact]
    public void Detect_Unsupported_Form_As_Note_Not_Sheet()
    {
        using var package = new ExcelPackage();
        package.Workbook.Worksheets.Add("Отчеты 3.1");

        var result = FormsExcelWorkbookDetector.Detect(package.Workbook);
        Assert.Empty(result.Sheets);
        Assert.Contains(result.Notes, n => n.Contains("3.1", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData("1.2")]
    [InlineData("1.3")]
    [InlineData("1.4")]
    [InlineData("1.5")]
    [InlineData("1.6")]
    [InlineData("1.7")]
    [InlineData("1.8")]
    [InlineData("1.9")]
    public void Detect_Form1x_By_Catalog_Headers(string formNum)
    {
        Assert.True(FormsExcelFormCatalog.TryGet(formNum, out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add($"Отчеты {formNum}");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());

        var result = FormsExcelWorkbookDetector.Detect(package.Workbook);
        Assert.Single(result.Sheets);
        Assert.Equal(formNum, result.Sheets[0].Spec.FormNum);
    }

    [Theory]
    [InlineData("2.1")]
    [InlineData("2.5")]
    [InlineData("2.12")]
    public void Detect_Form2x_By_Catalog_Headers(string formNum)
    {
        Assert.True(FormsExcelFormCatalog.TryGet(formNum, out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add($"Отчеты {formNum}");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());

        var result = FormsExcelWorkbookDetector.Detect(package.Workbook);
        Assert.Single(result.Sheets);
        Assert.Equal(formNum, result.Sheets[0].Spec.FormNum);
        Assert.Equal(FormsExcelFormFamily.Form2, result.Sheets[0].Spec.Family);
    }

    [Theory]
    [InlineData("Отчеты 1.1", "1.1")]
    [InlineData("Отчёты 2.10", "2.10")]
    [InlineData("Отчеты 1.1_22-24", "1.1")]
    public void TryParseReportsSheetName_Extracts_FormNum(string sheetName, string expected)
    {
        Assert.True(FormsExcelWorkbookDetector.TryParseReportsSheetName(sheetName, out var formNum));
        Assert.Equal(expected, formNum);
    }
}
