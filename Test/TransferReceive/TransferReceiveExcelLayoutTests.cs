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
    public void DualBlock_LayoutConstants_AreConsistent_ForForm11()
    {
        // Форма 1.1: 18 колонок в блоке + разделитель + схожесть + блок closest.
        Assert.Equal(18, TransferReceiveTestAccess.SourceColCountForTests);
        Assert.Equal(19, TransferReceiveTestAccess.SeparatorColForTests);
        Assert.Equal(20, TransferReceiveTestAccess.ConfidenceColForTests);
        Assert.Equal(21, TransferReceiveTestAccess.ClosestStartColForTests);
        Assert.Equal(38, TransferReceiveTestAccess.TotalColCountForTests);
        Assert.Equal(
            TransferReceiveTestAccess.SourceColCountForTests + 1,
            TransferReceiveTestAccess.SeparatorColForTests);
        Assert.Equal(
            TransferReceiveTestAccess.SeparatorColForTests + 1,
            TransferReceiveTestAccess.ConfidenceColForTests);
        Assert.Equal(
            TransferReceiveTestAccess.ConfidenceColForTests + 1,
            TransferReceiveTestAccess.ClosestStartColForTests);
        Assert.Equal(
            TransferReceiveTestAccess.ClosestStartColForTests + TransferReceiveTestAccess.SourceColCountForTests - 1,
            TransferReceiveTestAccess.TotalColCountForTests);
    }

    [Fact]
    public void DualBlock_LayoutConstants_AreConsistent_ForForm12()
    {
        Assert.Equal(17, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form12));
        Assert.Equal(20, TransferReceiveTestAccess.ClosestStartColForLayoutForTests(TransferReceiveSheetLayout.Form12));
        Assert.Equal(36, TransferReceiveTestAccess.TotalColCountForLayoutForTests(TransferReceiveSheetLayout.Form12));
    }

    [Fact]
    public void ComparableFieldOffsets_PointInsideSourceBlock()
    {
        foreach (TransferReceiveSheetLayout layout in Enum.GetValues<TransferReceiveSheetLayout>())
        {
            var sourceCols = TransferReceiveTestAccess.SourceColCountForLayoutForTests(layout);
            foreach (TransferReceiveField field in Enum.GetValues<TransferReceiveField>())
            {
                var offset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(field, layout);
                if (offset is null)
                {
                    continue;
                }

                Assert.InRange(offset.Value, 0, sourceCols - 1);
            }
        }

        Assert.Equal(15, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackType, TransferReceiveSheetLayout.Form12));
        Assert.Equal(11, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Mass, TransferReceiveSheetLayout.Form12));
        // Тип УКТ на 1.1/1.3 не выгружается (не участвует в сверке).
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackType, TransferReceiveSheetLayout.Form11));
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackType, TransferReceiveSheetLayout.Form13));
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Mass, TransferReceiveSheetLayout.Form11));
    }

    [Fact]
    public void FormColumnOrder_MatchesFormFieldSequence()
    {
        // 1.1: количество перед активностью; номер УКТ в конце (без типа УКТ).
        Assert.Equal(12, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Quantity, TransferReceiveSheetLayout.Form11));
        Assert.Equal(13, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Activity, TransferReceiveSheetLayout.Form11));
        Assert.Equal(17, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form11));

        // 1.2: наименование → зав.№ → масса; тип УКТ сразу перед номером УКТ.
        Assert.Equal(9, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Type, TransferReceiveSheetLayout.Form12));
        Assert.Equal(10, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.FactoryNumber, TransferReceiveSheetLayout.Form12));
        Assert.Equal(11, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Mass, TransferReceiveSheetLayout.Form12));
        Assert.Equal(15, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackType, TransferReceiveSheetLayout.Form12));
        Assert.Equal(16, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form12));

        // 1.3: как в форме — акт. → изг. → дата вып. → агр. → пост/пол. → номер УКТ.
        Assert.Equal(11, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.FactoryNumber, TransferReceiveSheetLayout.Form13));
        Assert.Equal(12, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Activity, TransferReceiveSheetLayout.Form13));
        Assert.Equal(14, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.CreationDate, TransferReceiveSheetLayout.Form13));
        Assert.Equal(15, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.AggregateState, TransferReceiveSheetLayout.Form13));
        Assert.Equal(16, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.ProviderOrRecieverOkpo, TransferReceiveSheetLayout.Form13));
        Assert.Equal(17, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form13));
        Assert.True(
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.CreationDate, TransferReceiveSheetLayout.Form13)! <
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.AggregateState, TransferReceiveSheetLayout.Form13)!);
        Assert.True(
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.FactoryNumber, TransferReceiveSheetLayout.Form13)! <
            TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.Activity, TransferReceiveSheetLayout.Form13)!);

        // 1.4: наименование → вид → … → объём → масса → агр. → ОКПО → УКТ.
        Assert.Equal(19, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form14));
        Assert.Equal(9, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Type, TransferReceiveSheetLayout.Form14));
        Assert.Equal(10, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Sort, TransferReceiveSheetLayout.Form14));
        Assert.Equal(13, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.ActivityMeasurementDate, TransferReceiveSheetLayout.Form14));
        Assert.Equal(14, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Volume, TransferReceiveSheetLayout.Form14));
        Assert.Equal(15, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Mass, TransferReceiveSheetLayout.Form14));
        Assert.Equal(16, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.AggregateState, TransferReceiveSheetLayout.Form14));
        Assert.Equal(18, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form14));
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.FactoryNumber, TransferReceiveSheetLayout.Form14));
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
        Assert.Contains("Схожесть", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("жёлт", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("зелён", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("важн", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("±15", text, StringComparison.Ordinal);
        Assert.Contains("±10%", text, StringComparison.Ordinal);
        Assert.DoesNotContain("closest", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("lookalike", text, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Form12Sheet_HasFormOrderHeaders_WithoutEmptyColumn()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateForm12SheetForTests(package);

        var sheet = package.Workbook.Worksheets["Форма 1.2"];
        Assert.NotNull(sheet);

        Assert.Equal("Наименование", sheet.Cells[2, 10].Text);
        Assert.Equal("Заводской номер", sheet.Cells[2, 11].Text);
        Assert.Equal("Масса обеднённого урана, кг", sheet.Cells[2, 12].Text);
        Assert.Equal("Тип УКТ", sheet.Cells[2, 16].Text);
        Assert.Equal("Номер УКТ", sheet.Cells[2, 17].Text);
        Assert.False(string.IsNullOrWhiteSpace(sheet.Cells[2, 12].Text));

        var closest = TransferReceiveTestAccess.ClosestStartColForLayoutForTests(TransferReceiveSheetLayout.Form12);
        Assert.Equal("Наименование", sheet.Cells[2, closest + 9].Text);
        Assert.Equal("Заводской номер", sheet.Cells[2, closest + 10].Text);
        Assert.Equal("Масса обеднённого урана, кг", sheet.Cells[2, closest + 11].Text);
        Assert.Equal("Тип УКТ", sheet.Cells[2, closest + 15].Text);
        Assert.Equal("Номер УКТ", sheet.Cells[2, closest + 16].Text);
        Assert.Equal("Форма 1.2", sheet.Name);
    }

    [Fact]
    public void Form11AndForm13Sheets_HaveConfidenceHeaderBold()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateForm11SheetForTests(package);
        TransferReceiveTestAccess.CreateForm13SheetForTests(package);

        var conf11 = TransferReceiveTestAccess.ConfidenceColForTests;
        var sheet11 = package.Workbook.Worksheets["Форма 1.1"];
        Assert.Equal("Схожесть, %", sheet11.Cells[2, conf11].Text);
        Assert.True(sheet11.Cells[2, conf11].Style.Font.Bold);

        var sheet13 = package.Workbook.Worksheets["Форма 1.3"];
        Assert.Equal("Схожесть, %", sheet13.Cells[2, conf11].Text);
        Assert.True(sheet13.Cells[2, conf11].Style.Font.Bold);
    }

    [Fact]
    public void WriteUnpairedSmokeRow_WritesOpCodesAndBoldConfidence()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateForm11SheetForTests(package);
        var sheet = package.Workbook.Worksheets["Форма 1.1"];
        var layout = TransferReceiveSheetLayout.Form11;
        const int dataRow = 3;

        TransferReceiveTestAccess.WriteUnpairedSmokeRowForTests(
            sheet, layout, dataRow, sourceOpCode: "21", closestOpCode: "31", confidencePercent: 85);

        var opOffset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.OperationCode, layout)!.Value;
        var confCol = TransferReceiveTestAccess.ConfidenceColForTests;
        var closestStart = TransferReceiveTestAccess.ClosestStartColForTests;

        Assert.Equal("21", sheet.Cells[dataRow, 1 + opOffset].Text);
        Assert.Equal("31", sheet.Cells[dataRow, closestStart + opOffset].Text);
        Assert.Equal("85", sheet.Cells[dataRow, confCol].Text);
        Assert.True(sheet.Cells[dataRow, confCol].Style.Font.Bold);
    }
}
