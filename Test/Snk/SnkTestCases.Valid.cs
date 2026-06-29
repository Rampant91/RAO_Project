using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа V — корректные сценарии без ошибок пользователя.
/// Проверяют базовое наличие, перезарядки, передачи и накопление количества,
/// а также ключевую развилку алгоритма: была единица в первой инвентаризации или нет.
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

    /// <summary>
    /// V01. Единица стоит на учёте с первой инвентаризации и не двигается. Базовый случай.
    /// </summary>
    private static SnkTestCase Valid01_InFirstInventory_SingleUnit_NoMovement() => new()
    {
        Name = "V01. В первой инвентаризации, без движения.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// V02. Единица получена после первой инвентаризации и остаётся в наличии (без передачи).
    /// </summary>
    private static SnkTestCase Valid02_NotInFirstInventory_ReceivedLater_Stays() => new()
    {
        Name = "V02. Не в первой инвентаризации, получена позже, остаётся.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
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

    /// <summary>
    /// V03. Единица в первой инвентаризации; позже перезарядка и повторная инвентаризация в один день.
    /// </summary>
    private static SnkTestCase Valid03_InFirstInventory_RechargeAndReinventory_SameDay() => new()
    {
        Name = "V03. В первой инвентаризации, перезарядка и инвентаризация в один день.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("53", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
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

    /// <summary>
    /// V04. Не в первой инвентаризации: получение раньше, затем перезарядка и инвентаризация в один день. Остаётся.
    /// </summary>
    private static SnkTestCase Valid04_NotInFirstInventory_ReceiveThenRechargeInventory_Stays() => new()
    {
        Name = "V04. Не в первой инвентаризации, получение, затем перезарядка и инвентаризация. Остаётся.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("53", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
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

    /// <summary>
    /// V05. Не в первой инвентаризации: получение, перезарядка и инвентаризация в один день, передача позже.
    /// </summary>
    private static SnkTestCase Valid05_NotInFirstInventory_ChainSameDay_TransferLater() => new()
    {
        Name = "V05. Не в первой инвентаризации, цепочка в один день, передача позже.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("53", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", LaterTransferDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// V06. Не в первой инвентаризации: получение, перезарядка, инвентаризация и передача в один день.
    /// В день инвентаризации ЗРИ есть в формах, но уже снят с учёта — допустимое расхождение.
    /// </summary>
    private static SnkTestCase Valid06_NotInFirstInventory_ChainAndTransfer_SameDay() => new()
    {
        Name = "V06. Не в первой инвентаризации, цепочка и передача в один день.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("53", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock()),
            On(FinalDate, AnchorStock())),
        AllowedInventoryVsSnkDifferenceByDate = ByDate(
            On(RechargeDay, Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>
    /// V07. Единица появилась в день первой инвентаризации (получение, перезарядка, инвентаризация); передача позже.
    /// </summary>
    private static SnkTestCase Valid07_InFirstInventoryDay_Chain_TransferLater() => new()
    {
        Name = "V07. Цепочка в день первой инвентаризации, передача позже.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("54", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(SecondInventoryDate),
            Operation("10", SecondInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// V08. Получение, перезарядка, инвентаризация и передача в день первой инвентаризации.
    /// В этот день ЗРИ есть в формах, но уже снят с учёта — допустимое расхождение.
    /// </summary>
    private static SnkTestCase Valid08_InFirstInventoryDay_ChainAndTransfer_SameDay() => new()
    {
        Name = "V08. Цепочка и передача в день первой инвентаризации.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("54", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
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

    /// <summary>
    /// V09. Две перезарядки в разные дни (52 → 52-1 → 52-2). Подряд идущие перезарядки не рассматриваются.
    /// </summary>
    private static SnkTestCase Valid09_TwoRecharges_OnSeparateDays() => new()
    {
        Name = "V09. Две перезарядки в разные дни.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Operation("53", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("54", ThirdRechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-2"),
            Anchor(ThirdRechargeDay),
            Operation("10", ThirdRechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-2"),
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

    /// <summary>
    /// V10. Две пары приём/передача с одним и тем же УКТ в один день: наличие не меняется (взаимокомпенсация).
    /// </summary>
    private static SnkTestCase Valid10_TwoReceiveTransferPairs_SameDay_NetInStock() => new()
    {
        Name = "V10. Две пары приём/передача в один день, наличие не меняется.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("38", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("38", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
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

    /// <summary>
    /// V11. Единица с пустыми зав.№/№ паспорта (учёт по количеству): инв. 2, приём 3, передача 1 → 4.
    /// </summary>
    private static SnkTestCase Valid11_EmptySerial_QuantityAccumulation() => new()
    {
        Name = "V11. Пустые зав.№/паспорт: накопление количества (2 + 3 − 1 = 4).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2),
            Operation("38", new DateOnly(2023, 3, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 3),
            Operation("28", new DateOnly(2023, 4, 1), "", "", "ОСГИ-3", "кобальт-60", "", quantity: 1),
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

    /// <summary>
    /// V12. Те же действия, что в V02 (получение и хранение), но другими кодами операций:
    /// получение «11» и (для проверки снятия) передача «21» вместо 38/28. Алгоритм ветвится
    /// по спискам кодов формы, поэтому результат должен совпадать с поведением для 38/28.
    /// 510 получена и осталась; 700 получена и передана теми же альтернативными кодами.
    /// </summary>
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
