using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа O — один и тот же набор операций, разный порядок строк <b>в один день</b> (29.11.2023).
/// Эталон наличия один для всех вариантов подгруппы. Даты — см. <see cref="SnkTestCases"/>.
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

  #region O01 — приём + перезарядка + инв. (как V04/V05 без передачи)

    // До 29.11.2023: только якорь на первой инв. (510 ещё нет).
    // На 29.11.2023: op.38 приём 510 УКТ=52 → op.53 перезарядка УКТ=52-1 → полная инв. (якорь + 510).
    // Эталон: якорь + 510 (52-1).

    private static IEnumerable<SnkTestCase> Order01_ReceiveRechargeInventory_SameDay()
    {
        var template = O01Template();

        // Операции 29.11.2023 (имена для читаемости вариантов):
        var recv510 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");       // op.38
        var rech510 = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");  // op.53
        var invAnchor = Anchor(RechargeDay);                                                     // op.10 якорь
        var inv510 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");         // op.10 510

        yield return OrderVariant("O01a", "29.11: приём→перезарядка→якорь→инв.510",
        [
            Anchor(FirstInventoryDate),
            recv510, rech510, invAnchor, inv510,
        ], template);

        yield return OrderVariant("O01b", "29.11: инв.510→приём→перезарядка→якорь",
        [
            Anchor(FirstInventoryDate),
            inv510, recv510, rech510, invAnchor,
        ], template);

        yield return OrderVariant("O01c", "29.11: перезарядка→приём→инв.510→якорь",
        [
            Anchor(FirstInventoryDate),
            rech510, recv510, inv510, invAnchor,
        ], template);

        yield return OrderVariant("O01d", "29.11: якорь→приём→перезарядка→инв.510",
        [
            Anchor(FirstInventoryDate),
            invAnchor, recv510, rech510, inv510,
        ], template);

        yield return OrderVariant("O01e", "29.11: приём→инв.510→перезарядка→якорь",
        [
            Anchor(FirstInventoryDate),
            recv510, inv510, rech510, invAnchor,
        ], template);

        yield return OrderVariant("O01f", "29.11: перезарядка→якорь→приём→инв.510",
        [
            Anchor(FirstInventoryDate),
            rech510, invAnchor, recv510, inv510,
        ], template);
    }

    private static SnkTestCase O01Template() => new()
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

  #endregion

  #region O02 — цепочка + передача в один день (как V06)

    // На 29.11.2023: приём → перезарядка → полная инв. → передача 510.
    // Эталон СНК: только якорь; 510 в формах инв. — допустимое расхождение.

    private static IEnumerable<SnkTestCase> Order02_ChainAndTransfer_SameDay()
    {
        var template = O02Template();

        var recv510 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var rech510 = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var invAnchor = Anchor(RechargeDay);
        var inv510 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var xfer510 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");

        yield return OrderVariant("O02a", "29.11: приём→перезарядка→якорь→инв→передача",
        [
            Anchor(FirstInventoryDate),
            recv510, rech510, invAnchor, inv510, xfer510,
        ], template);

        yield return OrderVariant("O02b", "29.11: передача→инв→якорь→перезарядка→приём",
        [
            Anchor(FirstInventoryDate),
            xfer510, inv510, invAnchor, rech510, recv510,
        ], template);

        yield return OrderVariant("O02c", "29.11: якорь→инв→передача→приём→перезарядка",
        [
            Anchor(FirstInventoryDate),
            invAnchor, inv510, xfer510, recv510, rech510,
        ], template);

        yield return OrderVariant("O02d", "29.11: приём→передача→перезарядка→инв→якорь",
        [
            Anchor(FirstInventoryDate),
            recv510, xfer510, rech510, inv510, invAnchor,
        ], template);

        yield return OrderVariant("O02e", "29.11: инв→приём→перезарядка→передача→якорь",
        [
            Anchor(FirstInventoryDate),
            inv510, recv510, rech510, xfer510, invAnchor,
        ], template);
    }

    private static SnkTestCase O02Template() => new()
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

  #endregion

  #region O03 — две пары приём/передача (как V10)

    // 510 уже в первой инв. На 29.11.2023: два цикла ± (нет строк op.10 в этот день).

    private static IEnumerable<SnkTestCase> Order03_ReceiveTransferPairs_SameDay()
    {
        var template = O03Template();

        var t1 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r1 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var t2 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r2 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");

        yield return OrderVariant("O03a", "29.11: передача→приём→передача→приём",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            t1, r1, t2, r2,
        ], template);

        yield return OrderVariant("O03b", "29.11: приём→передача→приём→передача",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            r1, t1, r2, t2,
        ], template);

        yield return OrderVariant("O03c", "29.11: передача→передача→приём→приём",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            t1, t2, r1, r2,
        ], template);

        yield return OrderVariant("O03d", "29.11: приём→приём→передача→передача",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            r1, r2, t1, t2,
        ], template);

        yield return OrderVariant("O03e", "29.11: приём→передача→передача→приём",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            r1, t1, t2, r2,
        ], template);
    }

    private static SnkTestCase O03Template() => new()
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

  #endregion

  #region O04 — пустые зав.№: приём+передача в один день (2+3−1=4)

    // ОСГИ-3: первая инв. qty=2. На 29.11.2023: повторная инв. qty=2, приём 3, передача 1.

    private static IEnumerable<SnkTestCase> Order04_EmptySerial_QuantitySameDay()
    {
        var template = O04Template();

        var invOsgi = Inv(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2);
        var recvOsgi = Receive(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 3);
        var xferOsgi = Transfer(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1);
        var invAnchor = Anchor(RechargeDay);

        yield return OrderVariant("O04a", "29.11: инв→приём→передача→якорь (2+3−1=4)",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            invOsgi, recvOsgi, xferOsgi, invAnchor,
        ], template);

        yield return OrderVariant("O04b", "29.11: приём→передача→инв→якорь",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            recvOsgi, xferOsgi, invOsgi, invAnchor,
        ], template);

        yield return OrderVariant("O04c", "29.11: передача→приём→якорь→инв",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            xferOsgi, recvOsgi, invAnchor, invOsgi,
        ], template);

        yield return OrderVariant("O04d", "29.11: якорь→приём→инв→передача",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            invAnchor, recvOsgi, invOsgi, xferOsgi,
        ], template);
    }

    private static SnkTestCase O04Template() => new()
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

  #endregion

  #region O05 — пустые зав.№: передача сверх наличия (как E02)

    // ОСГИ-3 qty=2; на 29.11.2023 передача qty=5 → эталон: только якорь.

    private static IEnumerable<SnkTestCase> Order05_EmptySerial_OverTransferSameDay()
    {
        var template = O05Template();

        var invOsgi = Inv(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2);
        var xferOsgi = Transfer(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 5);
        var invAnchor = Anchor(RechargeDay);

        yield return OrderVariant("O05a", "29.11: инв→передача→якорь",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            invOsgi, xferOsgi, invAnchor,
        ], template);

        yield return OrderVariant("O05b", "29.11: передача→инв→якорь",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            xferOsgi, invOsgi, invAnchor,
        ], template);

        yield return OrderVariant("O05c", "29.11: якорь→передача→инв",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            invAnchor, xferOsgi, invOsgi,
        ], template);

        yield return OrderVariant("O05d", "29.11: передача→якорь→инв",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            xferOsgi, invAnchor, invOsgi,
        ], template);
    }

    private static SnkTestCase O05Template() => new()
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

  #endregion

  #region O06 — две единицы, много операций в один день

    // На 29.11.2023: цепочки для 510 и 700; в конце полная инв. (якорь + обе единицы).

    private static IEnumerable<SnkTestCase> Order06_MultiUnit_ManyOpsSameDay()
    {
        var template = O06Template();

        var r510 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var z510 = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var i510 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r700 = Receive(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70");
        var i700 = Inv(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70");
        var invAnchor = Anchor(RechargeDay);

        yield return OrderVariant("O06a", "29.11: цепочка 510, затем 700, якорь в конце",
        [
            Anchor(FirstInventoryDate),
            r510, z510, i510, r700, i700, invAnchor,
        ], template);

        yield return OrderVariant("O06b", "29.11: цепочка 700, затем 510, якорь в конце",
        [
            Anchor(FirstInventoryDate),
            r700, i700, r510, z510, i510, invAnchor,
        ], template);

        yield return OrderVariant("O06c", "29.11: чередование 510/700, якорь в конце",
        [
            Anchor(FirstInventoryDate),
            r510, r700, z510, i700, i510, invAnchor,
        ], template);

        yield return OrderVariant("O06d", "29.11: якорь в начале дня, затем все операции",
        [
            Anchor(FirstInventoryDate),
            invAnchor, r510, z510, i510, r700, i700,
        ], template);

        yield return OrderVariant("O06e", "29.11: обратный порядок внутри единиц, якорь в конце",
        [
            Anchor(FirstInventoryDate),
            i700, r700, i510, z510, r510, invAnchor,
        ], template);
    }

    private static SnkTestCase O06Template() => new()
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

  #endregion

  #region O07 — длинная цепочка в один день + дубль инв.

    // 510 в первой инв. На 29.11.2023: пары ±, перезарядка, два op.10 для 510 (дубль), якорь.
    // HasIntentionalInventoryErrors: дубль инв. — отдельная проверка ошибок позже.

    private static IEnumerable<SnkTestCase> Order07_LongChain_ManyOpsSameDay()
    {
        var template = O07Template();

        var t1 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r1 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52");
        var rech = Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var inv1 = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var invDup = Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var t2 = Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var r2 = Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1");
        var invAnchor = Anchor(RechargeDay);

        yield return OrderVariant("O07a", "29.11: −,+ ,перезарядка,инв×2,−,+ ,якорь",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            t1, r1, rech, inv1, invDup, t2, r2, invAnchor,
        ], template);

        yield return OrderVariant("O07b", "29.11: обратный порядок всех операций дня",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            invAnchor, r2, t2, invDup, inv1, rech, r1, t1,
        ], template);

        yield return OrderVariant("O07c", "29.11: сначала инв×2+якорь, затем движения",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            inv1, invDup, invAnchor, r1, rech, t1, r2, t2,
        ], template);

        yield return OrderVariant("O07d", "29.11: −,+ ,−,+ ,перезарядка,инв,инв,якорь",
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            t1, r1, t2, r2, rech, inv1, invDup, invAnchor,
        ], template);
    }

    private static SnkTestCase O07Template() => new()
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

  #endregion
}
