using System;
using System.Linq;
using OfficeOpenXml;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>
/// Smoke Excel-разметки Pairing41: число колонок, смещения подсветки ↔ заголовки, легенда.
/// Без БД и UI; ловит фантомные/съехавшие колонки и расхождение профилей 1.2/1.3/1.4 на листе 1.6.
/// </summary>
public sealed class Pairing41ExcelLayoutTests
{
    [Fact]
    public void SheetLayouts_SourceColCount_MatchesInfoPlusDataHeaders()
    {
        Assert.Equal(
            Pairing41TestAccess.InfoColCountForTests + Pairing41TestAccess.Form1115DataHeadersForTests.Count,
            Pairing41TestAccess.Layout1115SourceColCountForTests);
        Assert.Equal(
            Pairing41TestAccess.InfoColCountForTests + Pairing41TestAccess.Form12DataHeadersForTests.Count,
            Pairing41TestAccess.Layout12SourceColCountForTests);
        Assert.Equal(
            Pairing41TestAccess.InfoColCountForTests + Pairing41TestAccess.Form13DataHeadersForTests.Count,
            Pairing41TestAccess.Layout13SourceColCountForTests);
        Assert.Equal(
            Pairing41TestAccess.InfoColCountForTests + Pairing41TestAccess.Form14DataHeadersForTests.Count,
            Pairing41TestAccess.Layout14SourceColCountForTests);
        Assert.Equal(
            Pairing41TestAccess.InfoColCountForTests + Pairing41TestAccess.Form16DataHeadersForTests.Count,
            Pairing41TestAccess.Layout16SourceColCountForTests);
    }

    [Fact]
    public void Form16_Form12Profile_MapsMassAndCodeRao_NotVolumeLikeColumns()
    {
        var info = Pairing41TestAccess.InfoColCountForTests;
        var headers = Pairing41TestAccess.Form16DataHeadersForTests;

        Assert.Equal("Код РАО", headers[1]);
        Assert.Equal("Объём, м³", headers[2]);
        Assert.Equal("Масса, т", headers[3]);
        Assert.Equal("Основные радионуклиды", headers[4]);
        Assert.Equal("Активность трития, Бк", headers[5]);
        Assert.Equal("Активность трансурановых, Бк", headers[8]);

        Assert.Equal(info + 1, Pairing41TestAccess.GetForm16Form12FieldOffsetForTests(Pairing12To16Field.CodeRao));
        Assert.Equal(info + 3, Pairing41TestAccess.GetForm16Form12FieldOffsetForTests(Pairing12To16Field.Mass));

        // Volume / MainRads / Tritium / Transuranium отсутствуют в Pairing12To16Field —
        // на листе 1.6 при профиле 1.2 эти колонки остаются белыми.
        Assert.DoesNotContain("Volume", Enum.GetNames<Pairing12To16Field>());
        Assert.DoesNotContain("MainRadionuclids", Enum.GetNames<Pairing12To16Field>());
        Assert.DoesNotContain("TritiumActivity", Enum.GetNames<Pairing12To16Field>());
        Assert.DoesNotContain("TransuraniumActivity", Enum.GetNames<Pairing12To16Field>());
    }

    [Fact]
    public void Form16_Form13Profile_DoesNotMapVolumeOrMass_ButMapsMainRadsAndActivities()
    {
        var info = Pairing41TestAccess.InfoColCountForTests;
        var headers = Pairing41TestAccess.Form16DataHeadersForTests;

        Assert.Equal(info + 4, Pairing41TestAccess.GetForm16Form13FieldOffsetForTests(Pairing13To16Field.MainRadionuclids));
        Assert.Equal(info + 5, Pairing41TestAccess.GetForm16Form13FieldOffsetForTests(Pairing13To16Field.TritiumActivity));
        Assert.Equal(info + 8, Pairing41TestAccess.GetForm16Form13FieldOffsetForTests(Pairing13To16Field.TransuraniumActivity));
        Assert.Equal("Основные радионуклиды", headers[4]);
        Assert.Equal("Активность трития, Бк", headers[5]);
        Assert.Equal("Активность трансурановых, Бк", headers[8]);

        // Volume/Mass отсутствуют в Pairing13To16Field — белые на листе 1.6 при профиле 1.3.
        Assert.DoesNotContain("Volume", Enum.GetNames<Pairing13To16Field>());
        Assert.DoesNotContain("Mass", Enum.GetNames<Pairing13To16Field>());
    }

    [Fact]
    public void Form16_Form14Profile_MapsAllDataColumns()
    {
        var info = Pairing41TestAccess.InfoColCountForTests;
        var headers = Pairing41TestAccess.Form16DataHeadersForTests;

        Assert.Equal(info + 2, Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(Pairing14To16Field.Volume));
        Assert.Equal(info + 3, Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(Pairing14To16Field.Mass));
        Assert.Equal(info + 4, Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(Pairing14To16Field.MainRadionuclids));
        Assert.Equal(info + 5, Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(Pairing14To16Field.TritiumActivity));
        Assert.Equal(info + 8, Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(Pairing14To16Field.TransuraniumActivity));
        Assert.Equal("Объём, м³", headers[2]);
        Assert.Equal("Масса, т", headers[3]);
    }

    [Fact]
    public void FormSheets_FieldOffsets_PointInsideDataHeaderRange()
    {
        var info = Pairing41TestAccess.InfoColCountForTests;

        foreach (Pairing11To15Field field in Enum.GetValues<Pairing11To15Field>())
        {
            var offset = Pairing41TestAccess.GetForm1115FieldOffsetForTests(field);
            Assert.NotNull(offset);
            Assert.InRange(offset.Value, info, Pairing41TestAccess.Layout1115SourceColCountForTests - 1);
        }

        foreach (Pairing12To16Field field in Enum.GetValues<Pairing12To16Field>())
        {
            var offset = Pairing41TestAccess.GetForm12FieldOffsetForTests(field);
            Assert.NotNull(offset);
            Assert.InRange(offset.Value, info, Pairing41TestAccess.Layout12SourceColCountForTests - 1);
            var form16Offset = Pairing41TestAccess.GetForm16Form12FieldOffsetForTests(field);
            Assert.NotNull(form16Offset);
            Assert.InRange(form16Offset.Value, info, Pairing41TestAccess.Layout16SourceColCountForTests - 1);
        }

        foreach (Pairing13To16Field field in Enum.GetValues<Pairing13To16Field>())
        {
            var offset = Pairing41TestAccess.GetForm13FieldOffsetForTests(field);
            Assert.NotNull(offset);
            Assert.InRange(offset.Value, info, Pairing41TestAccess.Layout13SourceColCountForTests - 1);
            var form16Offset = Pairing41TestAccess.GetForm16Form13FieldOffsetForTests(field);
            Assert.NotNull(form16Offset);
            Assert.InRange(form16Offset.Value, info, Pairing41TestAccess.Layout16SourceColCountForTests - 1);
        }

        foreach (Pairing14To16Field field in Enum.GetValues<Pairing14To16Field>())
        {
            var offset = Pairing41TestAccess.GetForm14FieldOffsetForTests(field);
            Assert.NotNull(offset);
            Assert.InRange(offset.Value, info, Pairing41TestAccess.Layout14SourceColCountForTests - 1);
            var form16Offset = Pairing41TestAccess.GetForm16Form14FieldOffsetForTests(field);
            Assert.NotNull(form16Offset);
            Assert.InRange(form16Offset.Value, info, Pairing41TestAccess.Layout16SourceColCountForTests - 1);
        }
    }

    [Fact]
    public void Form13AndForm14_LeaveGapForAggregateStateColumn()
    {
        var info = Pairing41TestAccess.InfoColCountForTests;
        // На листах 1.3/1.4 колонка «Агрегатное состояние» между AMD и документом — не в enum сравнения.
        Assert.Equal(info + 6, Pairing41TestAccess.GetForm13FieldOffsetForTests(Pairing13To16Field.ActivityMeasurementDate));
        Assert.Equal(info + 8, Pairing41TestAccess.GetForm13FieldOffsetForTests(Pairing13To16Field.DocumentVid));
        Assert.Equal(info + 8, Pairing41TestAccess.GetForm14FieldOffsetForTests(Pairing14To16Field.ActivityMeasurementDate));
        Assert.Equal(info + 10, Pairing41TestAccess.GetForm14FieldOffsetForTests(Pairing14To16Field.DocumentVid));
    }

    [Fact]
    public void LegendSheet_ExplainsClosestMatchAndForm16WhiteCells()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage();
        Pairing41TestAccess.CreatePairingLegendSheetForTests(package);

        var sheet = package.Workbook.Worksheets["Легенда"];
        Assert.NotNull(sheet);

        var text = string.Join('\n', Enumerable.Range(1, sheet.Dimension?.End.Row ?? 0)
            .Select(r => sheet.Cells[r, 1].Text + " " + sheet.Cells[r, 2].Text));

        Assert.Contains("ближайшее совпадение", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("предположение", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("похожая", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Форма 1.6", text, StringComparison.Ordinal);
        Assert.Contains("белыми", text, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("№ формы", text, StringComparison.Ordinal);
    }
}
