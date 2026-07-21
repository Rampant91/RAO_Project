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

    /// <summary>X01. 510: 52 → УКТ-11 → УКТ-12 → передача. Полная инв. на каждую дату op.10.</summary>
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
                    Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10")),
                ..FullInventoryOn(d2,
                    Inv(d2, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10")),
                Recharge(d3, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                ..FullInventoryOn(d3,
                    Inv(d3, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                ..FullInventoryOn(d4,
                    Inv(d4, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                Recharge(d5, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-12", opCode: "54"),
                ..FullInventoryOn(d5,
                    Inv(d5, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-12")),
                Transfer(transferDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-12"),
            ],
            ExpectedSnkStock = [AnchorStock()],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10")),
                On(d2, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10")),
                On(d3, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                On(d4, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                On(d5, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-12")),
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
                    Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                    Inv(FirstInventoryDate, "P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-30")),
                Receive(receiveB, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
                Recharge(SecondInventoryDate, "P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-31"),
                ..FullInventoryOn(SecondInventoryDate,
                    Inv(SecondInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                    Inv(SecondInventoryDate, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
                    Inv(SecondInventoryDate, "P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-31")),
                Transfer(transferA, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
            ],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
                Stock("P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-31"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate,
                    AnchorStock(),
                    Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                    Stock("P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-30")),
                On(SecondInventoryDate,
                    AnchorStock(),
                    Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                    Stock("P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
                    Stock("P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-31")),
                On(FinalDate,
                    AnchorStock(),
                    Stock("P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
                    Stock("P-301", "F-401", "Тип-М1", "кобальт-60", "УКТ-31")))
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
                Receive(new DateOnly(2022, 6, 1), "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Transfer(new DateOnly(2022, 9, 1), "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Receive(new DateOnly(2023, 1, 1), "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                ..FullInventoryOn(inventoryB,
                    Inv(inventoryB, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                Transfer(new DateOnly(2023, 9, 1), "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Receive(new DateOnly(2024, 1, 1), "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
            ],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock()),
                On(inventoryB, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
                On(FinalDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")))
        };
    }
}
