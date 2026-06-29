using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа V — корректные сценарии без ошибок пользователя.
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ValidCases()
    {
        yield return Valid01_InFirstInventory_SingleUnit_NoMovement();
        yield return Valid02_NotInFirstInventory_ReceivedLater_Stays();
        yield return Valid03_InFirstInventory_RechargeAndReinventory_SameDay();
        yield return Valid04_NotInFirstInventory_ReceiveThenRechargeInventory_Stays();
        yield return Valid05_NotInFirstInventory_ChainSameDay_TransferLater();
        yield return Valid06_NotInFirstInventory_ChainAndTransfer_SameDay();
        yield return Valid07_InFirstInventoryDay_Chain_TransferLater();
        yield return Valid08_InFirstInventoryDay_ChainAndTransfer_SameDay();
        yield return Valid09_TwoRecharges_OnSeparateDays();
        yield return Valid10_TwoReceiveTransferPairs_SameDay_NetInStock();
        yield return Valid11_EmptySerial_QuantityAccumulation();
        yield return Valid12_AlternativePlusMinusCodes_Equivalent();
    }

    /// <summary>V01. Только якорь с первой инвентаризации, без движений.</summary>
    private static SnkTestCase Valid01_InFirstInventory_SingleUnit_NoMovement() => new()
    {
        Name = "V01. В первой инвентаризации, без движения.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022 | op.10 | якорь 999/001
            Anchor(FirstInventoryDate),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>V02. 510 получена после первой инв., остаётся (510 не была в первой инв.).</summary>
    private static SnkTestCase Valid02_NotInFirstInventory_ReceivedLater_Stays() => new()
    {
        Name = "V02. Не в первой инвентаризации, получена позже, остаётся.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022 | op.10 | якорь (510 ещё нет на учёте)
            Anchor(FirstInventoryDate),
            // 10.11.2023 | op.38 | приём 510/083 УКТ=52
            Receive(ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")))
    };

    /// <summary>V03. 510 в первой инв.; позже перезарядка 52→52-1 и полная инв. в один день.</summary>
    private static SnkTestCase Valid03_InFirstInventory_RechargeAndReinventory_SameDay() => new()
    {
        Name = "V03. В первой инвентаризации, перезарядка и инвентаризация в один день.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022 | полная инв.: якорь + 510 УКТ=52
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52")),
            // 29.11.2023 | op.53 | перезарядка 510 → УКТ=52-1
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            // 29.11.2023 | полная инв.: якорь + 510 УКТ=52-1
            ..FullInventoryOn(RechargeDay,
                Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>V04. 510 не в первой инв.: приём → перезарядка → полная инв. в один день.</summary>
    private static SnkTestCase Valid04_NotInFirstInventory_ReceiveThenRechargeInventory_Stays() => new()
    {
        Name = "V04. Не в первой инвентаризации, получение, затем перезарядка и инвентаризация. Остаётся.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022 | op.10 | только якорь
            Anchor(FirstInventoryDate),
            // 10.11.2023 | op.38 | приём 510 УКТ=52
            Receive(ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            // 29.11.2023 | op.53 | перезарядка → УКТ=52-1
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            // 29.11.2023 | полная инв.: якорь + 510 УКТ=52-1
            ..FullInventoryOn(RechargeDay,
                Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
        ],
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

    /// <summary>V05. Цепочка приём→перезарядка→инв. в один день; передача позже.</summary>
    private static SnkTestCase Valid05_NotInFirstInventory_ChainSameDay_TransferLater() => new()
    {
        Name = "V05. Не в первой инвентаризации, цепочка в один день, передача позже.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            // 29.11.2023 | приём → перезарядка → полная инв.
            Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ..FullInventoryOn(RechargeDay,
                Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            // 29.01.2024 | op.28 | передача 510 УКТ=52-1
            Transfer(LaterTransferDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// V06. Как V05, но передача в тот же день, что цепочка.
    /// 510 есть в формах инв., но на конец дня уже снят — допустимое расхождение.
    /// </summary>
    private static SnkTestCase Valid06_NotInFirstInventory_ChainAndTransfer_SameDay() => new()
    {
        Name = "V06. Не в первой инвентаризации, цепочка и передача в один день.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            // 29.11.2023
            Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ..FullInventoryOn(RechargeDay,
                Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock()),
            On(FinalDate, AnchorStock())),
        AllowedInventoryVsSnkDifferenceByDate = ByDate(
            On(RechargeDay, Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>V07. 510 появляется в день первой инв. (приём→перезарядка→инв.); передача позже.</summary>
    private static SnkTestCase Valid07_InFirstInventoryDay_Chain_TransferLater() => new()
    {
        Name = "V07. Цепочка в день первой инвентаризации, передача позже.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022
            Anchor(FirstInventoryDate),
            Receive(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Recharge(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1", opCode: "54"),
            Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            // 01.01.2023 | полная инв.
            ..FullInventoryOn(SecondInventoryDate,
                Inv(SecondInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            // 29.11.2023 | передача
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>V08. Как V07, но передача в день первой инв. Допустимое расхождение инв. vs СНК.</summary>
    private static SnkTestCase Valid08_InFirstInventoryDay_ChainAndTransfer_SameDay() => new()
    {
        Name = "V08. Цепочка и передача в день первой инвентаризации.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022
            Anchor(FirstInventoryDate),
            Receive(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Recharge(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1", opCode: "54"),
            Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Transfer(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            // 01.01.2023 | только якорь (510 уже передана)
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(SecondInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock())),
        AllowedInventoryVsSnkDifferenceByDate = ByDate(
            On(FirstInventoryDate, Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>V09. Две перезарядки в разные дни: 52 → 52-1 → 52-2.</summary>
    private static SnkTestCase Valid09_TwoRecharges_OnSeparateDays() => new()
    {
        Name = "V09. Две перезарядки в разные дни.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52")),
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ..FullInventoryOn(RechargeDay,
                Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Recharge(ThirdRechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-2", opCode: "54"),
            ..FullInventoryOn(ThirdRechargeDay,
                Inv(ThirdRechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-2")),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-2"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(ThirdRechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-2")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-2")))
    };

    /// <summary>V10. Две пары приём/передача 510 в один день — взаимокомпенсация, в наличии остаётся.</summary>
    private static SnkTestCase Valid10_TwoReceiveTransferPairs_SameDay_NetInStock() => new()
    {
        Name = "V10. Две пары приём/передача в один день, наличие не меняется.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            // 29.11.2023 | две пары ± (инв. в этот день нет — только движения)
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>V11. Пустые зав.№/паспорт: инв.2 + приём3 − передача1 = 4 (в разные дни).</summary>
    private static SnkTestCase Valid11_EmptySerial_QuantityAccumulation() => new()
    {
        Name = "V11. Пустые зав.№/паспорт: накопление количества (2 + 3 − 1 = 4).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            // 01.03.2023 | op.38 | приём ОСГИ-3 qty=3
            Receive(new DateOnly(2023, 3, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 3),
            // 01.04.2023 | op.28 | передача ОСГИ-3 qty=1
            Transfer(new DateOnly(2023, 4, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 4),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            On(FinalDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 4)))
    };

    /// <summary>V12. Как V02, но коды приёма/передачи 11/21 вместо 38/28.</summary>
    private static SnkTestCase Valid12_AlternativePlusMinusCodes_Equivalent() => new()
    {
        Name = "V12. Альтернативные коды приёма/передачи (11/21).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Receive(ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52", opCode: "11"),
            Receive(ReceiveDay, "700", "070", "ГИК-5-3", "кобальт-60", "70", opCode: "11"),
            Transfer(RechargeDay, "700", "070", "ГИК-5-3", "кобальт-60", "70", opCode: "21"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")))
    };
}
