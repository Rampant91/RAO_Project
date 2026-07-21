using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Минимальный пакет bridge-кейсов для формы 1.3.
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> Form13Cases()
    {
        yield return Form13_01_AnchorOnly_NoMovement();
        yield return Form13_02_EmptySerial_MultiRows_QuantityAggregation();
    }

    /// <summary>
    /// F13-01. Только инвентаризация якоря, без движений.
    /// </summary>
    private static SnkTestCase Form13_01_AnchorOnly_NoMovement() => new()
    {
        Name = "F13-01. Форма 1.3: только якорь, без движения.",
        FormNum = "1.3",
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
    /// F13-02. Пустые зав.№/паспорт в 1.3 представлены множеством строк qty=1.
    /// Ожидаем тот же принцип суммирования, что и в 1.1 для пустых номеров.
    /// </summary>
    private static SnkTestCase Form13_02_EmptySerial_MultiRows_QuantityAggregation() => new()
    {
        Name = "F13-02. Форма 1.3: пустые зав/паспорт, суммирование множества строк.",
        FormNum = "1.3",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "", "", "Тип-Кол", "кобальт-60", "", quantity: 1),
            Inv(FirstInventoryDate, "", "", "Тип-Кол", "кобальт-60", "", quantity: 1),
            Receive(new DateOnly(2023, 3, 1), "", "", "Тип-Кол", "кобальт-60", "", quantity: 1),
            Transfer(new DateOnly(2023, 4, 1), "", "", "Тип-Кол", "кобальт-60", "", quantity: 1),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 2),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 2)),
            On(FinalDate, AnchorStock(), Stock("", "", "Тип-Кол", "кобальт-60", "", quantity: 2)))
    };
}
