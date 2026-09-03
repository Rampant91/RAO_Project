using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

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

        // 1.5: статус РАО после даты вып.; УКТ (наим./тип/номер); субсидия и ФЦП.
        Assert.Equal(22, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form15));
        Assert.Equal(12, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Quantity, TransferReceiveSheetLayout.Form15));
        Assert.Equal(13, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Activity, TransferReceiveSheetLayout.Form15));
        Assert.Equal(14, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.CreationDate, TransferReceiveSheetLayout.Form15));
        Assert.Equal(15, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.StatusRao, TransferReceiveSheetLayout.Form15));
        Assert.Equal(16, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.ProviderOrRecieverOkpo, TransferReceiveSheetLayout.Form15));
        Assert.Equal(17, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackName, TransferReceiveSheetLayout.Form15));
        Assert.Equal(18, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackType, TransferReceiveSheetLayout.Form15));
        Assert.Equal(19, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form15));
        Assert.Equal(20, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Subsidy, TransferReceiveSheetLayout.Form15));
        Assert.Equal(21, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.FcpNumber, TransferReceiveSheetLayout.Form15));
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.CreatorOkpo, TransferReceiveSheetLayout.Form15));
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
        Assert.Contains("диапазон", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ведущ", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("1.6", text, StringComparison.Ordinal);
        Assert.Contains("26", text, StringComparison.Ordinal);
        Assert.Contains("1.4/1.6", text, StringComparison.Ordinal);
        Assert.Contains("1.5 и 1.6", text, StringComparison.Ordinal);
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
    public void Form15Sheet_HasHeadersLikeForm11_WithoutCreatorOkpo()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateForm15SheetForTests(package);

        var sheet = package.Workbook.Worksheets["Форма 1.5"];
        Assert.NotNull(sheet);

        Assert.Equal(22, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form15));
        Assert.Equal("Количество, шт", sheet.Cells[2, 13].Text);
        Assert.Equal("Суммарная активность", sheet.Cells[2, 14].Text);
        Assert.Equal("Дата выпуска", sheet.Cells[2, 15].Text);
        Assert.Equal("Статус РАО", sheet.Cells[2, 16].Text);
        Assert.Equal("ОКПО поставщика или получателя", sheet.Cells[2, 17].Text);
        Assert.Equal("Наименование УКТ", sheet.Cells[2, 18].Text);
        Assert.Equal("Тип УКТ", sheet.Cells[2, 19].Text);
        Assert.Equal("Номер УКТ", sheet.Cells[2, 20].Text);
        Assert.Equal("Субсидия, %", sheet.Cells[2, 21].Text);
        Assert.Equal("Номер мероприятия ФЦП", sheet.Cells[2, 22].Text);

        var headerRow = string.Join('|', Enumerable.Range(1, 22).Select(c => sheet.Cells[2, c].Text));
        Assert.DoesNotContain("изготовителя", headerRow, StringComparison.OrdinalIgnoreCase);

        var closest = TransferReceiveTestAccess.ClosestStartColForLayoutForTests(TransferReceiveSheetLayout.Form15);
        Assert.Equal("Статус РАО", sheet.Cells[2, closest + 15].Text);
        Assert.Equal("Номер мероприятия ФЦП", sheet.Cells[2, closest + 21].Text);
        Assert.Equal("Форма 1.5", sheet.Name);
    }

    [Fact]
    public void DualBlock_LayoutConstants_AreConsistent_ForForm16()
    {
        Assert.Equal(24, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form16));
        Assert.Equal(8, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.CodeRao, TransferReceiveSheetLayout.Form16));
        Assert.Equal(9, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.StatusRao, TransferReceiveSheetLayout.Form16));
        Assert.Equal(18, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.ActivityMeasurementDate, TransferReceiveSheetLayout.Form16));
        Assert.Equal(21, TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, TransferReceiveSheetLayout.Form16));
        Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PassportNumber, TransferReceiveSheetLayout.Form16));
    }

    [Fact]
    public void Form16Sheet_HasFormOrderHeaders()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        TransferReceiveTestAccess.CreateForm16SheetForTests(package);

        var sheet = package.Workbook.Worksheets["Форма 1.6"];
        Assert.NotNull(sheet);

        Assert.Equal(24, TransferReceiveTestAccess.SourceColCountForLayoutForTests(TransferReceiveSheetLayout.Form16));
        Assert.Equal("Код РАО", sheet.Cells[2, 9].Text);
        Assert.Equal("Статус РАО", sheet.Cells[2, 10].Text);
        Assert.Equal("Объём, куб. м", sheet.Cells[2, 11].Text);
        Assert.Equal("Масса, т", sheet.Cells[2, 12].Text);
        Assert.Equal("Количество ОЗИИИ, шт", sheet.Cells[2, 13].Text);
        Assert.Equal("Тритий, Бк", sheet.Cells[2, 15].Text);
        Assert.Equal("Дата измерения активности", sheet.Cells[2, 19].Text);
        Assert.Equal("Тип УКТ", sheet.Cells[2, 21].Text);
        Assert.Equal("Номер мероприятия ФЦП", sheet.Cells[2, 24].Text);

        var closest = TransferReceiveTestAccess.ClosestStartColForLayoutForTests(TransferReceiveSheetLayout.Form16);
        Assert.Equal("Код РАО", sheet.Cells[2, closest + 8].Text);
        Assert.Equal("Форма 1.6", sheet.Name);
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

    public static IEnumerable<object[]> MarkerRowLayouts()
    {
        yield return [TransferReceiveSheetLayout.Form11];
        yield return [TransferReceiveSheetLayout.Form12];
        yield return [TransferReceiveSheetLayout.Form13];
        yield return [TransferReceiveSheetLayout.Form14];
        yield return [TransferReceiveSheetLayout.Form15];
        yield return [TransferReceiveSheetLayout.Form16];
    }

    [Theory]
    [MemberData(nameof(MarkerRowLayouts))]
    public void WriteOperationBlock_PutsUniqueMarkersIntoHeaderColumns_OnBothSides(
        TransferReceiveSheetLayout layout)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        CreateSheet(package, layout);
        var sheet = package.Workbook.Worksheets[SheetName(layout)];
        const int dataRow = 3;
        var source = MarkerRow(1, "SRC");
        var closest = MarkerRow(2, "CLS");

        TransferReceiveTestAccess.WriteOperationRowPairForTests(
            sheet, layout, dataRow, source, closest);

        var closestStart = TransferReceiveTestAccess.ClosestStartColForLayoutForTests(layout);
        AssertMarkers(sheet, dataRow, startCol: 1, layout, "SRC");
        AssertMarkers(sheet, dataRow, startCol: closestStart, layout, "CLS");
    }

    [Theory]
    [MemberData(nameof(MarkerRowLayouts))]
    public void WriteOperationBlock_HighlightLandsOnSameColumnsAsMarkerValues(
        TransferReceiveSheetLayout layout)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        CreateSheet(package, layout);
        var sheet = package.Workbook.Worksheets[SheetName(layout)];
        const int dataRow = 3;

        var levels = new Dictionary<TransferReceiveField, FieldMatchLevel>
        {
            [TransferReceiveField.PackNumber] = FieldMatchLevel.Mismatch
        };
        if (layout == TransferReceiveSheetLayout.Form16)
        {
            levels[TransferReceiveField.CodeRao] = FieldMatchLevel.Exact;
        }
        else
        {
            levels[TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact;
            levels[TransferReceiveField.Type] = FieldMatchLevel.Mismatch;
        }

        TransferReceiveTestAccess.WriteOperationRowPairForTests(
            sheet, layout, dataRow, MarkerRow(1, "SRC"), MarkerRow(2, "CLS"), levels);

        var packOffset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PackNumber, layout)!.Value;
        Assert.Equal("PACK-SRC", sheet.Cells[dataRow, 1 + packOffset].Text);
        Assert.Equal(
            TransferReceiveTestAccess.PairingMismatchFillRgbForTests,
            sheet.Cells[dataRow, 1 + packOffset].Style.Fill.BackgroundColor.Rgb);

        if (layout == TransferReceiveSheetLayout.Form16)
        {
            var codeOffset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.CodeRao, layout)!.Value;
            Assert.Equal("CODE-SRC", sheet.Cells[dataRow, 1 + codeOffset].Text);
            Assert.Equal(
                TransferReceiveTestAccess.PairingExactFillRgbForTests,
                sheet.Cells[dataRow, 1 + codeOffset].Style.Fill.BackgroundColor.Rgb);
            return;
        }

        var pasOffset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.PassportNumber, layout)!.Value;
        var typeOffset = TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
            TransferReceiveField.Type, layout)!.Value;
        Assert.Equal("PAS-SRC", sheet.Cells[dataRow, 1 + pasOffset].Text);
        Assert.Equal("TYPE-SRC", sheet.Cells[dataRow, 1 + typeOffset].Text);
        Assert.NotEqual("TYPE-SRC", sheet.Cells[dataRow, 1 + pasOffset].Text);
        Assert.Equal(
            TransferReceiveTestAccess.PairingExactFillRgbForTests,
            sheet.Cells[dataRow, 1 + pasOffset].Style.Fill.BackgroundColor.Rgb);
        Assert.Equal(
            TransferReceiveTestAccess.PairingMismatchFillRgbForTests,
            sheet.Cells[dataRow, 1 + typeOffset].Style.Fill.BackgroundColor.Rgb);
    }

    private static TransferReceiveRow MarkerRow(int id, string tag) =>
        new()
        {
            Id = id,
            OpCode = $"OP-{tag}",
            OpDate = "2024-01-15",
            PasNum = $"PAS-{tag}",
            Type = $"TYPE-{tag}",
            Radionuclids = $"RADS-{tag}",
            FacNum = $"FAC-{tag}",
            PackNumber = $"PACK-{tag}",
            PackType = $"PTYPE-{tag}",
            PackName = $"PNAME-{tag}",
            FcpNumber = $"FCP-{tag}",
            Subsidy = $"SUB-{tag}",
            StatusRao = $"ST-{tag}",
            CodeRao = $"CODE-{tag}",
            ProviderOrRecieverOkpo = $"OKPO-{tag}",
            CreatorOkpo = $"CRE-{tag}",
            Activity = "1.5e3",
            Mass = "4.25",
            Volume = "0.75",
            Quantity = 7,
            CreationDate = "2023-12-01",
            ActivityMeasurementDate = "2023-11-20",
            AggregateState = 2,
            Sort = 3
        };

    private static void AssertMarkers(
        ExcelWorksheet sheet,
        int row,
        int startCol,
        TransferReceiveSheetLayout layout,
        string tag)
    {
        string Text(TransferReceiveField field) =>
            sheet.Cells[row, startCol + TransferReceiveTestAccess.GetComparableColumnOffsetForTests(field, layout)!.Value].Text;

        Assert.Equal($"OP-{tag}", Text(TransferReceiveField.OperationCode));
        Assert.Equal($"PACK-{tag}", Text(TransferReceiveField.PackNumber));

        if (layout == TransferReceiveSheetLayout.Form16)
        {
            Assert.Equal($"CODE-{tag}", Text(TransferReceiveField.CodeRao));
            Assert.Equal($"ST-{tag}", Text(TransferReceiveField.StatusRao));
            Assert.Equal($"RADS-{tag}", Text(TransferReceiveField.Radionuclids));
            Assert.Equal($"PTYPE-{tag}", Text(TransferReceiveField.PackType));
            Assert.Equal($"FCP-{tag}", Text(TransferReceiveField.FcpNumber));
            Assert.Null(TransferReceiveTestAccess.GetComparableColumnOffsetForTests(
                TransferReceiveField.PassportNumber, layout));
            return;
        }

        Assert.Equal($"PAS-{tag}", Text(TransferReceiveField.PassportNumber));
        Assert.Equal($"TYPE-{tag}", Text(TransferReceiveField.Type));
        Assert.NotEqual($"TYPE-{tag}", Text(TransferReceiveField.PassportNumber));
        Assert.NotEqual($"RADS-{tag}", Text(TransferReceiveField.Type));
        Assert.NotEqual($"PACK-{tag}", Text(TransferReceiveField.PassportNumber));

        if (layout is TransferReceiveSheetLayout.Form11 or TransferReceiveSheetLayout.Form13
            or TransferReceiveSheetLayout.Form14 or TransferReceiveSheetLayout.Form15)
        {
            Assert.Equal($"RADS-{tag}", Text(TransferReceiveField.Radionuclids));
        }

        if (layout == TransferReceiveSheetLayout.Form15)
        {
            Assert.Equal($"FCP-{tag}", Text(TransferReceiveField.FcpNumber));
            Assert.Equal($"PTYPE-{tag}", Text(TransferReceiveField.PackType));
            Assert.Equal($"ST-{tag}", Text(TransferReceiveField.StatusRao));
        }
    }

    private static void CreateSheet(ExcelPackage package, TransferReceiveSheetLayout layout)
    {
        switch (layout)
        {
            case TransferReceiveSheetLayout.Form12:
                TransferReceiveTestAccess.CreateForm12SheetForTests(package);
                break;
            case TransferReceiveSheetLayout.Form13:
                TransferReceiveTestAccess.CreateForm13SheetForTests(package);
                break;
            case TransferReceiveSheetLayout.Form14:
                TransferReceiveTestAccess.CreateForm14SheetForTests(package);
                break;
            case TransferReceiveSheetLayout.Form15:
                TransferReceiveTestAccess.CreateForm15SheetForTests(package);
                break;
            case TransferReceiveSheetLayout.Form16:
                TransferReceiveTestAccess.CreateForm16SheetForTests(package);
                break;
            default:
                TransferReceiveTestAccess.CreateForm11SheetForTests(package);
                break;
        }
    }

    private static string SheetName(TransferReceiveSheetLayout layout) =>
        layout switch
        {
            TransferReceiveSheetLayout.Form12 => "Форма 1.2",
            TransferReceiveSheetLayout.Form13 => "Форма 1.3",
            TransferReceiveSheetLayout.Form14 => "Форма 1.4",
            TransferReceiveSheetLayout.Form15 => "Форма 1.5",
            TransferReceiveSheetLayout.Form16 => "Форма 1.6",
            _ => "Форма 1.1"
        };
}
