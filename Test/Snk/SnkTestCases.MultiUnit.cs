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
        yield return Multi04_SameSerialDifferentPack_TransferHitsOnlyOne();
        yield return Multi05_SameSerialDifferentPack_ReversedOrder();
    }

    /// <summary>M01. 510 и 700 в первой инв.; 700 передана на 29.11.2023.</summary>
    private static SnkTestCase Multi01_TwoUnitsInFirstInventory_OneTransferred() => new()
    {
        Name = "M01. Две единицы в первой инвентаризации, одна передана.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Inv(FirstInventoryDate, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20")),
            Transfer(RechargeDay, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Stock("P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20")),
            On(FinalDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")))
    };

    /// <summary>M02. 510 (серийная) + Тип-Кол (пустые зав.№, qty).</summary>
    private static SnkTestCase Multi02_SerialAndEmptySerial_BothStay() => new()
    {
        Name = "M02. Серийная и пустые зав.№ единицы вместе.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Inv(FirstInventoryDate, "", "", "Тип-Кол", "кобальт-60", "", quantity: 2)),
            Receive(new DateOnly(2023, 3, 1), "", "", "Тип-Кол", "кобальт-60", "", quantity: 1),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
            Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 3),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 2)),
            On(FinalDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
                Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 3)))
    };

    /// <summary>M03. 700 в первой инв. (передана); 510 получена позже.</summary>
    private static SnkTestCase Multi03_MixedFirstInventoryAndReceivedLater() => new()
    {
        Name = "M03. Одна в первой инвентаризации (передана), другая получена позже (остаётся).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20")),
            Receive(ReceiveDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
            Transfer(RechargeDay, "P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-201", "F-301", "Тип-М1", "кобальт-60", "УКТ-20")),
            On(FinalDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10")))
    };

    /// <summary>
    /// M04. Две РАЗНЫЕ учётные единицы с одинаковыми паспортом/зав.№/типом/РН, но разным УКТ
    /// (52 и УКТ-11), обе в наличии с первой инвентаризации. Перезарядки между ними НЕТ — это
    /// два параллельных физических источника в разных контейнерах. Передача с УКТ=52 должна
    /// затронуть только единицу 52; единица УКТ-11 остаётся в наличии.
    /// </summary>
    private static SnkTestCase Multi04_SameSerialDifferentPack_TransferHitsOnlyOne() => new()
    {
        Name = "M04. Две единицы один серийник/разный УКТ; передача 52 затрагивает только её.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            Transfer(RechargeDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")))
    };

    /// <summary>
    /// M05. То же, что M04, но строки инвентаризации идут в обратном порядке (УКТ-11 перед 52).
    /// Результат не должен зависеть от порядка строк: обе единицы различаются по УКТ,
    /// передача 52 затрагивает только её.
    /// </summary>
    private static SnkTestCase Multi05_SameSerialDifferentPack_ReversedOrder() => new()
    {
        Name = "M05. Две единицы один серийник/разный УКТ; обратный порядок строк.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
            Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
            Transfer(RechargeDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-10"),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate,
                AnchorStock(),
                Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")))
    };
}
