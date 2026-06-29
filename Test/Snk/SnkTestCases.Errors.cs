using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа E — ошибки пользователя (проверяется итоговое наличие, не типы ошибок).</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ErrorCases()
    {
        yield return Error01_TransferOfNeverOwnedUnit();
        yield return Error02_EmptySerial_TransferMoreThanAvailable();
        yield return Error03_DoubleTransfer_Serial();
        yield return Error04_InventoryOfAlreadyTransferredUnit();
    }

    /// <summary>E01. Передача 510, которой не было в наличии.</summary>
    private static SnkTestCase Error01_TransferOfNeverOwnedUnit() => new()
    {
        Name = "E01. Передача отсутствующего ЗРИ.",
        EndDate = FinalDate,
        Operations =
        [
            // 19.01.2022 | только якорь
            Anchor(FirstInventoryDate),
            // 29.11.2023 | op.28 | передача 510 (не получали)
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// E02. ОСГИ-3 (пустые зав.№): инв. qty=2, передача qty=5 → СНК 0 (Math.Max). См. O05.
    /// </summary>
    private static SnkTestCase Error02_EmptySerial_TransferMoreThanAvailable() => new()
    {
        Name = "E02. Пустые зав.№/паспорт: передано больше, чем было.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            // 29.11.2023 | op.28 | передача qty=5 при наличии 2
            Transfer(RechargeDay, "", "", "ОСГИ-3", "кобальт-60", "", quantity: 5),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("", "", "ОСГИ-3", "кобальт-60", "", quantity: 2)),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>E03. Две передачи 510 — вторая ошибочна.</summary>
    private static SnkTestCase Error03_DoubleTransfer_Serial() => new()
    {
        Name = "E03. Повторная передача уже снятого ЗРИ.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Transfer(LaterTransferDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// E04. 510 передана; на 29.11.2023 ошибочно в op.10 (в формах есть, в СНК нет).
    /// </summary>
    private static SnkTestCase Error04_InventoryOfAlreadyTransferredUnit() => new()
    {
        Name = "E04. Инвентаризация ранее снятого с учёта ЗРИ.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Receive(ReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Transfer(new DateOnly(2023, 11, 20), "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            // 29.11.2023 | якорь корректно; 510 — ошибочная строка инв.
            Anchor(RechargeDay),
            Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(RechargeDay, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };
}
