using System.Collections.Generic;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Регрессии подготовки к whole-DB: GetNextDataRow (append), OrderForExport, progress «i из N».
/// </summary>
public sealed class TransferReceiveWorkbookAppendTests
{
    [Fact]
    public void GetNextDataRow_AfterHeaders_ReturnsDataStartRow()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "header";
        sheet.Cells[2, 1].Value = "fields";

        var next = TransferReceiveTestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(3, next);
    }

    [Fact]
    public void GetNextDataRow_AfterData_ContinuesAfterLastRow()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "h1";
        sheet.Cells[2, 1].Value = "h2";
        sheet.Cells[3, 1].Value = "org-A";
        sheet.Cells[4, 1].Value = "org-A-2";

        var next = TransferReceiveTestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(5, next);
    }

    /// <summary>
    /// Регрессия whole-DB: org B дописывается ниже org A — правый блок A не затирается.
    /// </summary>
    [Fact]
    public void AppendSimulation_SecondOrgDoesNotOverwriteFirstOrgClosestBlock()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        var sheet = package.Workbook.Worksheets.Add("Форма 1.1");
        sheet.Cells[1, 1].Value = "Непарная";
        sheet.Cells[2, 1].Value = "поля";

        // TransferReceive Form11: ClosestStartCol = 21
        const int closestStartCol = 21;
        var rowA = TransferReceiveTestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(3, rowA);
        sheet.Cells[rowA, 1].Value = "94015";
        sheet.Cells[rowA, closestStartCol].Value = "94015-closest";

        var rowB = TransferReceiveTestAccess.GetNextDataRowForTests(sheet);
        Assert.Equal(4, rowB);
        sheet.Cells[rowB, 1].Value = "93024";

        Assert.Equal("94015", sheet.Cells[3, 1].Value?.ToString());
        Assert.Equal("94015-closest", sheet.Cells[3, closestStartCol].Value?.ToString());
        Assert.Equal("93024", sheet.Cells[4, 1].Value?.ToString());
        Assert.True(string.IsNullOrEmpty(sheet.Cells[4, closestStartCol].Value?.ToString()));
    }

    [Fact]
    public void OrderForExport_SortsByRegNoThenPeriodsThenNumberInOrderThenId()
    {
        // CustomReportsComparer: числовой порядок рег.№ — 9 перед 10, 93024 перед 94015.
        var ordered = TransferReceiveTestAccess.OrderForExportIdsForTests(
        [
            (Id: 3, OrgRegNo: "94015", StartPeriod: "2024-01-01", EndPeriod: "2024-03-31", NumberInOrder: 1),
            (Id: 1, OrgRegNo: "93024", StartPeriod: "2024-06-01", EndPeriod: "2024-06-30", NumberInOrder: 2),
            (Id: 2, OrgRegNo: "93024", StartPeriod: "2024-01-01", EndPeriod: "2024-03-31", NumberInOrder: 1),
            (Id: 4, OrgRegNo: "93024", StartPeriod: "2024-01-01", EndPeriod: "2024-03-31", NumberInOrder: 1)
        ]);

        Assert.Equal([2, 4, 1, 3], ordered);
    }

    [Fact]
    public void FormatOrgExcelStage_MultiOrg_IncludesIndexAndLabel()
    {
        var text = TransferReceiveTestAccess.FormatMultiOrgExcelStageForTests(
            orgIndex: 2,
            orgCount: 5,
            stage: "запись в Excel",
            orgLabel: "93024_10000001");

        Assert.Equal("Организация 2 из 5, 93024_10000001: запись в Excel", text);
    }

    [Fact]
    public void FormatOrgExcelStage_SingleOrgMode_ReturnsStageAsIs()
    {
        var text = TransferReceiveTestAccess.FormatMultiOrgExcelStageForTests(
            orgIndex: 0,
            orgCount: 0,
            stage: "заполнение листа «Форма 1.1»");

        Assert.Equal("заполнение листа «Форма 1.1»", text);
    }
}
