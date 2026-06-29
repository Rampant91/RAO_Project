using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа M — несколько учётных единиц в одних списках.</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> MultiUnitCases()
    {
        yield return Multi01_TwoUnitsInFirstInventory_OneTransferred();
        yield return Multi02_SerialAndEmptySerial_BothStay();
        yield return Multi03_MixedFirstInventoryAndReceivedLater();
    }

    /// <summary>M01. 510 и 700 в первой инв.; 700 передана на 29.11.2023.</summary>
    private static SnkTestCase Multi01_TwoUnitsInFirstInventory_OneTransferred() => new()
    {
        Name = "M01. Две единицы в первой инвентаризации, одна передана.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Inv(FirstInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70")),
            Transfer(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("700", "070", "ГИК-5-3", "кобальт-60", "70")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>M02. 510 (серийная) + ОСГИ-3 (пустые зав.№, qty).</summary>
    private static SnkTestCase Multi02_SerialAndEmptySerial_BothStay() => new()
    {
        Name = "M02. Серийная и пустые зав.№ единицы вместе.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            Receive(new DateOnly(2023, 3, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 3),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            On(FinalDate,
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 3)))
    };

    /// <summary>M03. 700 в первой инв. (передана); 510 получена позже.</summary>
    private static SnkTestCase Multi03_MixedFirstInventoryAndReceivedLater() => new()
    {
        Name = "M03. Одна в первой инвентаризации (передана), другая получена позже (остаётся).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70")),
            Receive(ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Transfer(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("700", "070", "ГИК-5-3", "кобальт-60", "70")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")))
    };
}
