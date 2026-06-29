using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа O — устойчивость к порядку строк операций в один день.
/// Для каждого логического сценария задаётся один эталон (ожидаемое наличие),
/// а в <see cref="SnkTestCase.Operations"/> те же операции переставляются.
/// Алгоритм СНК должен нормализовать порядок и давать одинаковый результат.
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> OrderCases()
    {
        foreach (var variant in Order01_ReceiveRechargeInventory_SameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order02_ChainAndTransfer_SameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order03_ReceiveTransferPairs_SameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order04_EmptySerial_QuantitySameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order05_EmptySerial_OverTransferSameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order06_MultiUnit_ManyOpsSameDay())
        {
            yield return variant;
        }

        foreach (var variant in Order07_LongChain_ManyOpsSameDay())
        {
            yield return variant;
        }
    }

  #region O01 — приём + перезарядка + инвентаризация в один день

    private static IEnumerable<SnkTestCase> Order01_ReceiveRechargeInventory_SameDay()
    {
        var template = new SnkTestCase
        {
            Name = "O01-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock()),
                On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
        };

        var recv = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var rech = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var anchDay = Anchor(RechargeDay);
        var inv = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var prefix = Ops(Anchor(FirstInventoryDate));

        yield return OrderVariant("O01a", "приём→перезарядка→якорь→инв.510",
            [.. prefix, .. Ops(recv, rech, anchDay, inv)], template);
        yield return OrderVariant("O01b", "инв.510→приём→перезарядка→якорь",
            [.. prefix, .. Ops(inv, recv, rech, anchDay)], template);
        yield return OrderVariant("O01c", "перезарядка→приём→инв.510→якорь",
            [.. prefix, .. Ops(rech, recv, inv, anchDay)], template);
        yield return OrderVariant("O01d", "якорь→приём→перезарядка→инв.510",
            [.. prefix, .. Ops(anchDay, recv, rech, inv)], template);
        yield return OrderVariant("O01e", "приём→инв.510→перезарядка→якорь",
            [.. prefix, .. Ops(recv, inv, rech, anchDay)], template);
        yield return OrderVariant("O01f", "перезарядка→якорь→приём→инв.510",
            [.. prefix, .. Ops(rech, anchDay, recv, inv)], template);
    }

  #endregion

  #region O02 — цепочка + передача в один день (как V06)

    private static IEnumerable<SnkTestCase> Order02_ChainAndTransfer_SameDay()
    {
        var template = new SnkTestCase
        {
            Name = "O02-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock = [AnchorStock()],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock()),
                On(RechargeDay, AnchorStock()),
                On(FinalDate, AnchorStock())),
            AllowedInventoryVsSnkDifferenceByDate = ByDate(
                On(RechargeDay, Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
        };

        var recv = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var rech = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var inv = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var xfer = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var anchDay = Anchor(RechargeDay);
        var prefix = Ops(Anchor(FirstInventoryDate));

        yield return OrderVariant("O02a", "приём→перезарядка→якорь→инв→передача",
            [.. prefix, .. Ops(recv, rech, anchDay, inv, xfer)], template);
        yield return OrderVariant("O02b", "передача→инв→якорь→перезарядка→приём",
            [.. prefix, .. Ops(xfer, inv, anchDay, rech, recv)], template);
        yield return OrderVariant("O02c", "якорь→инв→передача→приём→перезарядка",
            [.. prefix, .. Ops(anchDay, inv, xfer, recv, rech)], template);
        yield return OrderVariant("O02d", "приём→передача→перезарядка→инв→якорь",
            [.. prefix, .. Ops(recv, xfer, rech, inv, anchDay)], template);
        yield return OrderVariant("O02e", "инв→приём→перезарядка→передача→якорь",
            [.. prefix, .. Ops(inv, recv, rech, xfer, anchDay)], template);
    }

  #endregion

  #region O03 — несколько пар приём/передача в один день (как V10)

    private static IEnumerable<SnkTestCase> Order03_ReceiveTransferPairs_SameDay()
    {
        var template = new SnkTestCase
        {
            Name = "O03-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
        };

        var t1 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r1 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var t2 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r2 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var prefix = Ops(
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"));

        yield return OrderVariant("O03a", "передача→приём→передача→приём",
            [.. prefix, .. Ops(t1, r1, t2, r2)], template);
        yield return OrderVariant("O03b", "приём→передача→приём→передача",
            [.. prefix, .. Ops(r1, t1, r2, t2)], template);
        yield return OrderVariant("O03c", "передача→передача→приём→приём",
            [.. prefix, .. Ops(t1, t2, r1, r2)], template);
        yield return OrderVariant("O03d", "приём→приём→передача→передача",
            [.. prefix, .. Ops(r1, r2, t1, t2)], template);
        yield return OrderVariant("O03e", "приём→передача→передача→приём",
            [.. prefix, .. Ops(r1, t1, t2, r2)], template);
    }

  #endregion

  #region O04 — пустые зав.№: приём и передача в один день (количество)

    private static IEnumerable<SnkTestCase> Order04_EmptySerial_QuantitySameDay()
    {
        var template = new SnkTestCase
        {
            Name = "O04-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 4),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
                On(RechargeDay, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 4)),
                On(FinalDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 4)))
        };

        var inv = Inv(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2);
        var recv = Receive(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 3);
        var xfer = Transfer(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1);
        var anchDay = Anchor(RechargeDay);
        var prefix = Ops(
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2));

        yield return OrderVariant("O04a", "инв→приём→передача→якорь (2+3−1=4)",
            [.. prefix, .. Ops(inv, recv, xfer, anchDay)], template);
        yield return OrderVariant("O04b", "приём→передача→инв→якорь",
            [.. prefix, .. Ops(recv, xfer, inv, anchDay)], template);
        yield return OrderVariant("O04c", "передача→приём→якорь→инв",
            [.. prefix, .. Ops(xfer, recv, anchDay, inv)], template);
        yield return OrderVariant("O04d", "якорь→приём→инв→передача",
            [.. prefix, .. Ops(anchDay, recv, inv, xfer)], template);
    }

  #endregion

  #region O05 — пустые зав.№: передача больше наличия (как E02, эталон — обнуление)

    private static IEnumerable<SnkTestCase> Order05_EmptySerial_OverTransferSameDay()
    {
        // Эталон: после передачи 5 при наличии 2 единица снимается с учёта (qty→0, не в СНК).
        // См. комментарий к E02 — это намеренная семантика Math.Max(0, …), а не игнор операции.
        var template = new SnkTestCase
        {
            Name = "O05-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock = [AnchorStock()],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
                On(RechargeDay, AnchorStock()),
                On(FinalDate, AnchorStock()))
        };

        var invDay = Inv(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2);
        var xfer = Transfer(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 5);
        var anchDay = Anchor(RechargeDay);
        var prefix = Ops(
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2));

        yield return OrderVariant("O05a", "инв→передача→якорь",
            [.. prefix, .. Ops(invDay, xfer, anchDay)], template);
        yield return OrderVariant("O05b", "передача→инв→якорь",
            [.. prefix, .. Ops(xfer, invDay, anchDay)], template);
        yield return OrderVariant("O05c", "якорь→передача→инв",
            [.. prefix, .. Ops(anchDay, xfer, invDay)], template);
        yield return OrderVariant("O05d", "передача→якорь→инв",
            [.. prefix, .. Ops(xfer, anchDay, invDay)], template);
    }

  #endregion

  #region O06 — две единицы, много операций в один день (перемешанный порядок)

    private static IEnumerable<SnkTestCase> Order06_MultiUnit_ManyOpsSameDay()
    {
        var template = new SnkTestCase
        {
            Name = "O06-ref",
            EndDate = FinalDate,
            Operations = [],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("700", "070", "ГИК-5-3", "кобальт-60", "70"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock()),
                On(RechargeDay,
                    AnchorStock(),
                    Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Stock("700", "070", "ГИК-5-3", "кобальт-60", "70")),
                On(FinalDate,
                    AnchorStock(),
                    Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                    Stock("700", "070", "ГИК-5-3", "кобальт-60", "70")))
        };

        var r510 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var z510 = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var i510 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r700 = Receive(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70");
        var i700 = Inv(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70");
        var anchDay = Anchor(RechargeDay);
        var prefix = Ops(Anchor(FirstInventoryDate));

        yield return OrderVariant("O06a", "по блокам: 510 цепочка, затем 700",
            [.. prefix, .. Ops(r510, z510, i510, r700, i700, anchDay)], template);
        yield return OrderVariant("O06b", "по блокам: 700, затем 510",
            [.. prefix, .. Ops(r700, i700, r510, z510, i510, anchDay)], template);
        yield return OrderVariant("O06c", "чередование: 510→700→510→700",
            [.. prefix, .. Ops(r510, r700, z510, i700, i510, anchDay)], template);
        yield return OrderVariant("O06d", "якорь в начале дня, затем все операции",
            [.. prefix, .. Ops(anchDay, r510, z510, i510, r700, i700)], template);
        yield return OrderVariant("O06e", "якорь в конце, обратный порядок внутри единиц",
            [.. prefix, .. Ops(i700, r700, i510, z510, r510, anchDay)], template);
    }

  #endregion

  #region O07 — длинная цепочка в один день (пары +/-, перезарядка, дубль инв.)

    private static IEnumerable<SnkTestCase> Order07_LongChain_ManyOpsSameDay()
    {
        // 510: в первой инв.; в RechargeDay — передача, приём, перезарядка, повторная инв. (ошибочный дубль),
        // ещё одна передача и приём. Итог: 52-1 в наличии. Дубль op.10 не меняет СНК, но есть в формах.
        var template = new SnkTestCase
        {
            Name = "O07-ref",
            EndDate = FinalDate,
            HasIntentionalInventoryErrors = true,
            Operations = [],
            ExpectedSnkStock =
            [
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ],
            ExpectedInventoryStockByDate = ByDate(
                On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
                On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
        };

        var t1 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r1 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var rech = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var inv1 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var invDup = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var t2 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r2 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var anchDay = Anchor(RechargeDay);
        var prefix = Ops(
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"));

        yield return OrderVariant("O07a", "линейный: −,+ ,перезарядка,инв×2,−,+ ,якорь",
            [.. prefix, .. Ops(t1, r1, rech, inv1, invDup, t2, r2, anchDay)], template);
        yield return OrderVariant("O07b", "обратный порядок всех операций дня",
            [.. prefix, .. Ops(anchDay, r2, t2, invDup, inv1, rech, r1, t1)], template);
        yield return OrderVariant("O07c", "сначала все инв.+якорь, затем движения",
            [.. prefix, .. Ops(inv1, invDup, anchDay, r1, rech, t1, r2, t2)], template);
        yield return OrderVariant("O07d", "чередование: −,+ ,−,+ ,перезарядка,инв,инв,якорь",
            [.. prefix, .. Ops(t1, r1, t2, r2, rech, inv1, invDup, anchDay)], template);
    }

  #endregion
}
