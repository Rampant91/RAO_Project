using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа E — сценарии с логическими ошибками пользователя.
/// Сейчас проверяются только итоговые списки СНК и инвентаризаций (наличие/количество).
/// Сами ошибки (их типы) будут отлавливаться отдельно, поэтому для кейсов с расхождением
/// «есть в инвентаризации, но нет в СНК» выставлен флаг <see cref="SnkTestCase.HasIntentionalInventoryErrors"/>.
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ErrorCases()
    {
        yield return Error01_TransferOfNeverOwnedUnit();
        yield return Error02_EmptySerial_TransferMoreThanAvailable();
        yield return Error03_DoubleTransfer_Serial();
        yield return Error04_InventoryOfAlreadyTransferredUnit();
    }

    /// <summary>
    /// E01. Передача ЗРИ, которого не было в наличии (нет инвентаризации и получения). В СНК он не появляется.
    /// </summary>
    private static SnkTestCase Error01_TransferOfNeverOwnedUnit() => new()
    {
        Name = "E01. Передача отсутствующего ЗРИ.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// E02. Пустые зав.№/паспорт: передано больше, чем было (инв. 2, передача 5).
    /// <para>
    /// Семантика СНК (намеренная): количество не уходит в минус — <c>Math.Max(0, qty)</c>,
    /// единица снимается с учёта (в СНК только якорь). Игнорировать передачу нельзя:
    /// иначе в наличии останется 2, хотя пользователь указал списание 5.
    /// Отдельно при проверке инвентаризаций должна фиксироваться ошибка типа 9
    /// (QuantityGivenExceedsAvailable) — это будет в тестах ошибок позже.
    /// </para>
    /// Группа O05 проверяет, что при перестановке строк тот же эталон (обнуление) сохраняется.
    /// </summary>
    private static SnkTestCase Error02_EmptySerial_TransferMoreThanAvailable() => new()
    {
        Name = "E02. Пустые зав.№/паспорт: передано больше, чем было.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2),
            Operation("28", RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 5),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// E03. Повторная передача уже снятого с учёта ЗРИ. После первой передачи его нет, вторая — ошибка.
    /// </summary>
    private static SnkTestCase Error03_DoubleTransfer_Serial() => new()
    {
        Name = "E03. Повторная передача уже снятого ЗРИ.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", LaterTransferDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// E04. Инвентаризация ранее снятого с учёта ЗРИ: единица получена и передана, а позже ошибочно
    /// внесена в инвентаризацию. В СНК её нет, но в формах инвентаризации она есть — это и есть ошибка.
    /// </summary>
    private static SnkTestCase Error04_InventoryOfAlreadyTransferredUnit() => new()
    {
        Name = "E04. Инвентаризация ранее снятого с учёта ЗРИ.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("38", ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", new DateOnly(2023, 11, 20), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Operation("10", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };
}
