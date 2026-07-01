using System;
using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа E — ошибки пользователя: итоговое наличие и типы ошибок проверки инвентаризаций.</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ErrorCases()
    {
        yield return Error00_DuplicateInventory_Serial();
        yield return Error00b_DuplicateInventory_AfterRecharge_Ukt();
        yield return Error01_TransferOfNeverOwnedUnit();
        yield return Error02_EmptySerial_TransferMoreThanAvailable();
        yield return Error03_DoubleTransfer_Serial();
        yield return Error04_InventoryOfAlreadyTransferredUnit();
        yield return Error05_RegisteredAndNotInventoried_Serial();
        yield return Error05b_RegisteredAndNotInventoried_UktOnReceive();
        yield return Error06_MissingFromInventory_Serial();
        yield return Error07_InventoriedUnitReceived_Serial();
        yield return Error07b_RegisteredAndMissing_UktOnReceive();
        yield return Error07c_InventoriedUnitReceived_Isolated();
        yield return Error08_ReRegistration_DoubleReceive();
        yield return Error08b_ReRegistration_Isolated();
        yield return Error09_ZeroOnNeverOwnedUnit();
    }

    /// <summary>Дата приёма между первой и второй инвентаризациями (до <see cref="SecondInventoryDate"/>).</summary>
    private static readonly DateOnly MidPeriodReceiveDay = new(2022, 6, 15);

    /// <summary>Второй приём в том же периоде (для ошибки 7).</summary>
    private static readonly DateOnly MidPeriodSecondReceiveDay = new(2022, 8, 1);

    /// <summary>E00. Две op.10 для одного ЗРИ в один день — дубль инвентаризации (тип 0).</summary>
    private static SnkTestCase Error00_DuplicateInventory_Serial() => new()
    {
        Name = "E00. Дубль op.10 для серийного ЗРИ в один день.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Anchor(RechargeDay),
            Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(RechargeDay,
                Err(SnkInventoryErrorType.InventoryDuplicate,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "10", opDate: RechargeDay)))
    };

    /// <summary>
    /// E00b. После перезарядки УКТ 52→52-1 — дубль op.10; ошибка привязана к строке с УКТ 52-1.
    /// </summary>
    private static SnkTestCase Error00b_DuplicateInventory_AfterRecharge_Ukt() => new()
    {
        Name = "E00b. Дубль op.10 после перезарядки: УКТ 52-1 в ошибке.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Receive(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Recharge(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(RechargeDay),
            Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Inv(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(RechargeDay, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(RechargeDay,
                Err(SnkInventoryErrorType.InventoryDuplicate,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "10", opDate: RechargeDay)))
    };
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
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.UnInventoriedUnitGivenAway,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "28", opDate: RechargeDay)))
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
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.QuantityGivenExceedsAvailable,
                    "", "", "ОСГИ-3", "кобальт-60", "",
                    opCode: "28", opDate: RechargeDay)))
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
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.ReDeRegistration,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "28", opDate: LaterTransferDay)))
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
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(RechargeDay,
                Err(SnkInventoryErrorType.GivenUnitIsInventoried,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "10", opDate: RechargeDay)))
    };

    /// <summary>E05. Приём между инвентаризациями, на промежуточной дате нет op.10 для ЗРИ (тип 1).</summary>
    private static SnkTestCase Error05_RegisteredAndNotInventoried_Serial() => new()
    {
        Name = "E05. ЗРИ принят, но не проинвентаризирован на промежуточной дате.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>E05b. Ошибка 1 привязана к приёму с УКТ 52 (до перезарядки).</summary>
    private static SnkTestCase Error05b_RegisteredAndNotInventoried_UktOnReceive() => new()
    {
        Name = "E05b. Ошибка 1: в выгрузке УКТ из операции приёма (52).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>E06. ЗРИ в СНК с первой инв., отсутствует во второй инв., передач не было (тип 2).</summary>
    private static SnkTestCase Error06_MissingFromInventory_Serial() => new()
    {
        Name = "E06. ЗРИ в наличии, но отсутствует в промежуточной инвентаризации.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.MissingFromInventoryUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "10", opDate: FirstInventoryDate)))
    };

    /// <summary>
    /// E07. Повторный приём ЗРИ, уже в наличии: на промежуточной дате без op.10 для 510
    /// срабатывают ошибки 1, 2 и 6 (все привязаны к операции приёма).
    /// </summary>
    private static SnkTestCase Error07_InventoriedUnitReceived_Serial() => new()
    {
        Name = "E07. Повторный приём ЗРИ, уже в наличии (типы 1+2+6).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay),
                Err(SnkInventoryErrorType.MissingFromInventoryUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay),
                Err(SnkInventoryErrorType.InventoriedUnitReceived,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>
    /// E07b. Ошибки 1+2: УКТ привязан к операции приёма (52), не к инв. (52-1).
    /// В ExpectedSnkStock две строки 510: приём с УКТ=52 создаёт отдельную учётную единицу
    /// (52) параллельно ранее проинвентаризированной (52-1) — так отражает расчёт СНК.
    /// </summary>
    private static SnkTestCase Error07b_RegisteredAndMissing_UktOnReceive() => new()
    {
        Name = "E07b. Ошибки 1+2: в выгрузке УКТ из операции приёма (52).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate,
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52")),
            On(FinalDate,
                AnchorStock(),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
                Stock("510", "083", "ГИК-5-3", "кобальт-60", "52"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.MissingFromInventoryUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "10", opDate: FirstInventoryDate),
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>E07c. Изолированная ошибка 6: 510 проинвентаризирован и на второй дате.</summary>
    private static SnkTestCase Error07c_InventoriedUnitReceived_Isolated() => new()
    {
        Name = "E07c. Изолированная ошибка 6: повторный приём при полной инв.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            ..FullInventoryOn(SecondInventoryDate,
                Inv(SecondInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.InventoriedUnitReceived,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>
    /// E08. Два приёма подряд без передачи: ошибки 1 и 7 на второй приём;
    /// при полной инв. на первой дате дополнительно 6 и 2.
    /// </summary>
    private static SnkTestCase Error08_ReRegistration_DoubleReceive() => new()
    {
        Name = "E08. Двойная постановка на учёт (типы 1+2+6+7).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Receive(MidPeriodSecondReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodSecondReceiveDay),
                Err(SnkInventoryErrorType.ReRegistration,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodSecondReceiveDay),
                Err(SnkInventoryErrorType.MissingFromInventoryUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodSecondReceiveDay),
                Err(SnkInventoryErrorType.InventoriedUnitReceived,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodReceiveDay)))
    };

    /// <summary>E08b. Изолированная ошибка 7: два приёма, 510 не был в первой инв.</summary>
    private static SnkTestCase Error08b_ReRegistration_Isolated() => new()
    {
        Name = "E08b. Изолированная ошибка 7: два приёма без первой инв.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Receive(MidPeriodReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Receive(MidPeriodSecondReceiveDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Anchor(SecondInventoryDate),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(SecondInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"))),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(SecondInventoryDate,
                Err(SnkInventoryErrorType.RegisteredAndNotInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodSecondReceiveDay),
                Err(SnkInventoryErrorType.ReRegistration,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "38", opDate: MidPeriodSecondReceiveDay)))
    };

    /// <summary>E09. Нулевая операция над ЗРИ, которого не было в наличии (тип 8; см. также Z02).</summary>
    private static SnkTestCase Error09_ZeroOnNeverOwnedUnit() => new()
    {
        Name = "E09. Нулевая операция над отсутствующим ЗРИ.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Zero(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1", opCode: "99"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.ZeroOperationWithUnInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "99", opDate: RechargeDay)))
    };
}
