using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа X — длинные цепочки в разные дни (см. V05–V08 и O## для «всё в один день»).</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> XCases()
    {
        yield return X01_LongSingleUnitLifecycle();
        yield return X02_MultiUnit_MultiInventoryDate();
        yield return X03_RepeatedReceiveTransferCycles();
    }

    /// <summary>X01. 510: 52 → 52-1 → 52-2 → передача. Полная инв. на каждую дату op.10.</summary>
    private static SnkTestCase X01_LongSingleUnitLifecycle()
    {
        var d2 = new DateOnly(2022, 7, 1);
        var d3 = new DateOnly(2023, 1, 1);
        var d4 = new DateOnly(2023, 7, 1);
        var d5 = new DateOnly(2024, 1, 1);
        var transferDay = new DateOnly(2024, 6, 1);

        return new SnkTestCase
        {
            Name = "X01. Длинный жизненный цикл одной единицы (много инвентаризаций).",
            EndDate = FinalDate,
            Operations =
            [
                ..FullInventoryOn(FirstInventoryDate,
                    Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52")),
                ..FullInventoryOn(d2,
                    Inv(d2, "510", "083", "ГИК-5-3", "кобальт-60", "52")),
                Recharge(d3, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                ..FullInventoryOn(d3,
                    Inv(d3, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                ..FullInventoryOn(d4,
                    Inv(d4, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                Recharge(d5, "510", "083", "ГИК-5-3", "кобальт-60", "52-2", opCode: "54"),
                ..FullInventoryOn(d5,
                    Inv(d5, "510", "083", "ГИК-5-3", "кобальт-60", "52-2")),
                Transfer(transferDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-2"),
            ],
            ExpectedSnkStock = [AnchorStock()],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
                On(d2, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
                On(d3, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(d4, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(d5, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-2")),
                On(FinalDate, AnchorStock()))
        };
    }

    /// <summary>X02. Три единицы, полная инв. на 19.01.2022 и 01.01.2023.</summary>
    private static SnkTestCase X02_MultiUnit_MultiInventoryDate()
    {
        var receiveB = new DateOnly(2022, 9, 1);
        var transferA = new DateOnly(2023, 6, 1);

        return new SnkTestCase
        {
            Name = "X02. Несколько единиц, несколько дат инвентаризации, разные судьбы.",
            EndDate = FinalDate,
            Operations =
            [
                ..FullInventoryOn(FirstInventoryDate,
                    Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Inv(FirstInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80")),
                Receive(receiveB, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
                Recharge(SecondInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80-1"),
                ..FullInventoryOn(SecondInventoryDate,
                    Inv(SecondInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Inv(SecondInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
                    Inv(SecondInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80-1")),
                Transfer(transferA, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("700", "070", "ГИК-5-3", "кобальт-60", "70"),
                Stock("800", "088", "ГИК-5-3", "кобальт-60", "80-1"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate,
                    AnchorStock(),
                    Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Stock("800", "088", "ГИК-5-3", "кобальт-60", "80")),
                On(SecondInventoryDate,
                    AnchorStock(),
                    Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Stock("700", "070", "ГИК-5-3", "кобальт-60", "70"),
                    Stock("800", "088", "ГИК-5-3", "кобальт-60", "80-1")),
                On(FinalDate,
                    AnchorStock(),
                    Stock("700", "070", "ГИК-5-3", "кобальт-60", "70"),
                    Stock("800", "088", "ГИК-5-3", "кобальт-60", "80-1")))
        };
    }

    /// <summary>X03. 510: циклы приём/передача в разные дни; в конце в наличии.</summary>
    private static SnkTestCase X03_RepeatedReceiveTransferCycles()
    {
        var inventoryB = new DateOnly(2023, 6, 1);

        return new SnkTestCase
        {
            Name = "X03. Повторные циклы приём/передача одной единицы (разные дни).",
            EndDate = FinalDate,
            Operations =
            [
                Anchor(FirstInventoryDate),
                Receive(new DateOnly(2022, 6, 1), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Transfer(new DateOnly(2022, 9, 1), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Receive(new DateOnly(2023, 1, 1), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                ..FullInventoryOn(inventoryB,
                    Inv(inventoryB, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                Transfer(new DateOnly(2023, 9, 1), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Receive(new DateOnly(2024, 1, 1), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock()),
                On(inventoryB, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
        };
    }
}
