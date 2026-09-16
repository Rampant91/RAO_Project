using System;
using System.Linq;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using Models.Forms.Form1;
using OfficeOpenXml;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelRowMapperTests
{
    public FormsExcelRowMapperTests()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    [Fact]
    public void Map11_Dash_To_Null_For_Nullable_Fields()
    {
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add(FormsExcelExportHeadersForm11.ReportsSheetName);
        FormsExcelTestHelpers.WriteHeaders(ws, FormsExcelExportHeadersForm11.ReportColumns);
        FormsExcelTestHelpers.WriteMinimalForm1Row(ws, 2, "12345", "11111111", 0, new DateTime(2024, 1, 1), new DateTime(2024, 3, 31));
        ws.Cells[2, 15].Value = "-";
        ws.Cells[2, 19].Value = "-";
        ws.Cells[2, 20].Value = "-";
        ws.Cells[2, 21].Value = "-";
        ws.Cells[2, 23].Value = "-";
        ws.Cells[2, 16].Value = 1.5e10;

        var form = FormsExcelExportParserForm11.MapForm11Row(ws, 2);
        Assert.Null(form.Quantity_DB);
        Assert.Null(form.Category_DB);
        Assert.Null(form.SignedServicePeriod_DB);
        Assert.Null(form.PropertyCode_DB);
        Assert.Null(form.DocumentVid_DB);
        Assert.Contains("e+", form.Activity_DB, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Map12_Reads_Mass_And_NameIOU()
    {
        Assert.True(FormsExcelFormCatalog.TryGet("1.2", out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Отчеты 1.2");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());
        ws.Cells[2, 2].Value = "11111111";
        ws.Cells[2, 4].Value = "12345";
        ws.Cells[2, 5].Value = 0;
        ws.Cells[2, 6].Value = new DateTime(2024, 1, 1);
        ws.Cells[2, 7].Value = new DateTime(2024, 3, 31);
        ws.Cells[2, 8].Value = 1;
        ws.Cells[2, 9].Value = "11";
        ws.Cells[2, 12].Value = "ИОУ";
        ws.Cells[2, 14].Value = 12.5;

        var form = (Form12)FormsExcelRowMapperForm1x.MapRow("1.2", ws, 2);
        Assert.Equal("ИОУ", form.NameIOU_DB);
        Assert.Contains("e+", form.Mass_DB, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Map19_Reads_CodeTypeAccObject()
    {
        Assert.True(FormsExcelFormCatalog.TryGet("1.9", out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Отчеты 1.9");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());
        ws.Cells[2, 8].Value = 1;
        ws.Cells[2, 9].Value = "11";
        ws.Cells[2, 14].Value = 3;
        ws.Cells[2, 16].Value = 1.0;

        var form = (Form19)FormsExcelRowMapperForm1x.MapRow("1.9", ws, 2);
        Assert.Equal((short)3, form.CodeTypeAccObject_DB);
    }

    [Fact]
    public void Map212_Reads_Codes()
    {
        Assert.True(FormsExcelFormCatalog.TryGet("2.12", out var spec));
        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Отчеты 2.12");
        FormsExcelTestHelpers.WriteHeaders(ws, spec!.ReportColumns.ToArray());
        ws.Cells[2, 7].Value = 1;
        ws.Cells[2, 8].Value = 11;
        ws.Cells[2, 9].Value = 2;
        ws.Cells[2, 11].Value = 1.5;

        var form = (Models.Forms.Form2.Form212)FormsExcelRowMapperForm2x.MapRow("2.12", ws, 2);
        Assert.Equal((short)11, form.OperationCode_DB);
        Assert.Equal((short)2, form.ObjectTypeCode_DB);
    }
}
