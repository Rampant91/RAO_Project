using System;
using System.Linq;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Smoke Excel-разметки TransferReceive: dual-block, смещения подсветки, легенда.
/// </summary>
public sealed class TransferReceiveExcelLayoutTests
{
    [Fact]
    public void DualBlock_LayoutConstants_AreConsistent()
    {
        Assert.Equal(18, TransferReceiveTestAccess.SourceColCountForTests);
        Assert.Equal(19, TransferReceiveTestAccess.SeparatorColForTests);
        Assert.Equal(20, TransferReceiveTestAccess.ClosestStartColForTests);
        Assert.Equal(37, TransferReceiveTestAccess.TotalColCountForTests);
        Assert.Equal(
            TransferReceiveTestAccess.SourceColCountForTests + 1,
            TransferReceiveTestAccess.SeparatorColForTests);
        Assert.Equal(
            TransferReceiveTestAccess.SeparatorColForTests + 1,
            TransferReceiveTestAccess.ClosestStartColForTests);
        Assert.Equal(
            TransferReceiveTestAccess.ClosestStartColForTests + TransferReceiveTestAccess.SourceColCountForTests - 1,
            TransferReceiveTestAccess.TotalColCountForTests);
    }

    [Fact]
    public void ComparableFieldOffsets_PointInsideSourceBlock()
    {
        var sourceCols = TransferReceiveTestAccess.SourceColCountForTests;
        foreach (TransferReceiveField field in Enum.GetValues<TransferReceiveField>())
        {
            var offset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(field);
            Assert.NotNull(offset);
            Assert.InRange(offset.Value, 0, sourceCols - 1);
        }
    }

    [Fact]
    public void QuantityAndAggregateState_ShareSameColumnOffset()
    {
        // На 1.1 — «Количество», на 1.3 — «Агрегатное состояние» в той же позиции блока.
        Assert.Equal(
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(TransferReceiveField.Quantity),
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(TransferReceiveField.AggregateState));
        Assert.Equal(12, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(TransferReceiveField.Quantity));
        Assert.Equal(17, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(TransferReceiveField.PackNumber));
    }

    [Fact]
    public void LegendSheet_ExplainsClosestMatchCaveats()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateLegendSheetForTests(package);

        var sheet = package.Workbook.Worksheets["Легенда"];
        Assert.NotNull(sheet);

        var text = string.Join('\n', Enumerable.Range(1, sheet.Dimension?.End.Row ?? 0)
            .Select(r => sheet.Cells[r, 1].Text + " " + sheet.Cells[r, 2].Text));

        Assert.Contains("ближайшее совпадение", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("предположение", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("похожая", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("построчно", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("1.5", text, StringComparison.Ordinal);
    }
}
