using System.Linq;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using OfficeOpenXml;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelForm2ParserTests
{
    public FormsExcelForm2ParserTests()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public void Group_By_Year_And_Correction()
    {
        Assert.True(FormsExcelFormCatalog.TryGet("2.12", out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Отчеты 2.12");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());
        FormsExcelTestHelpers.WriteMinimalForm2Row(ws, 2, "12345", "11111111", 0, 2024);
        FormsExcelTestHelpers.WriteMinimalForm2Row(ws, 3, "12345", "11111111", 1, 2024);
        FormsExcelTestHelpers.WriteMinimalForm2Row(ws, 4, "12345", "11111111", 0, 2025);

        var detection = FormsExcelWorkbookDetector.Detect(package.Workbook);
        var parsed = FormsExcelExportParserForm2.ParseDetectedSheet(detection.Sheets[0]);
        Assert.Equal(3, parsed.Groups.Count);
    }
}
