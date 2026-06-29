using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа X — крупные смешанные сценарии: длинные цепочки, много операций и дат инвентаризации,
/// несколько единиц с разной судьбой и несколько пар приём/передача. Нужны, чтобы оценить
/// корректность алгоритмов СНК и проверки инвентаризаций «в масштабе», а не только на коротких кейсах.
/// Все переходы — в разные дни (без сложных «всё в один день» цепочек, которые вынесены в V05–V08).
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> MixedCases()
    {
        yield return Mixed01_LongSingleUnitLifecycle();
        yield return Mixed02_MultiUnit_MultiInventoryDate();
        yield return Mixed03_RepeatedReceiveTransferCycles();
    }

    /// <summary>
    /// X01. Полный жизненный цикл одной единицы через много дат инвентаризации:
    /// в первой инвентаризации (52) → перезарядка+инвентаризация (52-1) → ещё инвентаризация →
    /// вторая перезарядка+инвентаризация (52-2) → передача. Каждый раз указано полное наличие.
    /// В конце в наличии только якорь.
    /// </summary>
    private static SnkTestCase Mixed01_LongSingleUnitLifecycle()
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
                Anchor(FirstInventoryDate),
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),

                Anchor(d2),
                Inv(d2, "510", "083", "ГИК-5-3", "кобальт-60", "52"),

                Recharge(d3, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Anchor(d3),
                Inv(d3, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),

                Anchor(d4),
                Inv(d4, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),

                Recharge(d5, "510", "083", "ГИК-5-3", "кобальт-60", "52-2", opCode: "54"),
                Anchor(d5),
                Inv(d5, "510", "083", "ГИК-5-3", "кобальт-60", "52-2"),

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

    /// <summary>
    /// X02. Несколько единиц с разной судьбой через несколько дат инвентаризации:
    /// 510 (в первой инв., передана позже), 700 (получена после первой инв. и инвентаризирована),
    /// 800 (в первой инв., перезаряжена и переинвентаризирована). Полное наличие на каждую дату.
    /// </summary>
    private static SnkTestCase Mixed02_MultiUnit_MultiInventoryDate()
    {
        var receiveB = new DateOnly(2022, 9, 1);
        var transferA = new DateOnly(2023, 6, 1);

        return new SnkTestCase
        {
            Name = "X02. Несколько единиц, несколько дат инвентаризации, разные судьбы.",
            EndDate = FinalDate,
            Operations =
            [
                Anchor(FirstInventoryDate),
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Inv(FirstInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80"),

                Receive(receiveB, "700", "070", "ГИК-5-3", "кобальт-60", "70"),

                Recharge(SecondInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80-1"),
                Anchor(SecondInventoryDate),
                Inv(SecondInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Inv(SecondInventoryDate, "700", "070", "ГИК-5-3", "кобальт-60", "70"),
                Inv(SecondInventoryDate, "800", "088", "ГИК-5-3", "кобальт-60", "80-1"),

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

    /// <summary>
    /// X03. Несколько пар приём/передача одной серийной единицы в разные дни (ушла и вернулась):
    /// получена → передана → снова получена → инвентаризирована → передана → снова получена.
    /// В конце в наличии (последняя операция — получение). Проверяет длинную +/- цепочку.
    /// </summary>
    private static SnkTestCase Mixed03_RepeatedReceiveTransferCycles()
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

                Anchor(inventoryB),
                Inv(inventoryB, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),

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
