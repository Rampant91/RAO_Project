using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа M — несколько учётных единиц одновременно в списках.
/// Проверяют, что единицы не «склеиваются» и не теряются, когда у них разные судьбы
/// (часть остаётся, часть передаётся, часть с пустыми зав.№ и учётом по количеству).
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> MultiUnitCases()
    {
        yield return Multi01_TwoUnitsInFirstInventory_OneTransferred();
        yield return Multi02_SerialAndEmptySerial_BothStay();
        yield return Multi03_MixedFirstInventoryAndReceivedLater();
    }

    /// <summary>
    /// M01. Две единицы в первой инвентаризации, одна позже передана. В СНК остаётся вторая.
    /// </summary>
    private static SnkTestCase Multi01_TwoUnitsInFirstInventory_OneTransferred() => new()
    {
        Name = "M01. Две единицы в первой инвентаризации, одна передана.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("10", FirstInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
            Operation("28", RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
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

    /// <summary>
    /// M02. Серийная единица и единица с пустыми зав.№ (учёт по количеству) одновременно. Обе остаются.
    /// </summary>
    private static SnkTestCase Multi02_SerialAndEmptySerial_BothStay() => new()
    {
        Name = "M02. Серийная и пустые зав.№ единицы вместе.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("10", FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2),
            Operation("38", new DateOnly(2023, 3, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1),
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

    /// <summary>
    /// M03. Смешанные судьбы: одна единица в первой инвентаризации (позже передана),
    /// другая получена после первой инвентаризации (остаётся).
    /// </summary>
    private static SnkTestCase Multi03_MixedFirstInventoryAndReceivedLater() => new()
    {
        Name = "M03. Одна в первой инвентаризации (передана), другая получена позже (остаётся).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
            Operation("38", ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("28", RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
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
