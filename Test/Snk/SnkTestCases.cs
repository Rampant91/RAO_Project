using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Наборы тестовых сценариев СНК и проверки инвентаризаций.
/// <para>Группы: V — корректные; E — ошибки пользователя; N — нормализация полей;
/// M — несколько единиц; Z — нулевые операции; X — длинные цепочки; O — порядок строк в один день.</para>
/// <para>
/// Общие даты (см. также имена полей ниже):
/// <list type="bullet">
/// <item><b>19.01.2022</b> — <see cref="FirstInventoryDate"/>, первая инвентаризация</item>
/// <item><b>01.01.2023</b> — <see cref="SecondInventoryDate"/></item>
/// <item><b>10.11.2023</b> — <see cref="ReceiveDay"/>, приём «между» инвентаризациями</item>
/// <item><b>29.11.2023</b> — <see cref="RechargeDay"/>, типичный «горячий» день (цепочки, пары ±)</item>
/// <item><b>29.01.2024</b> — <see cref="LaterTransferDay"/>, передача после цепочки</item>
/// <item><b>01.06.2024</b> — <see cref="ThirdRechargeDay"/>, вторая перезарядка</item>
/// <item><b>01.01.2026</b> — <see cref="FinalDate"/>, конец периода (EndDate), не день инвентаризации</item>
/// </list>
/// </para>
/// <para>
/// <b>Якорь 999/001</b> — учётная единица, стоящая на учёте с первой инвентаризации и не двигающаяся.
/// Нужен, чтобы задать дату первой инвентаризации и имитировать «полное наличие» в отчёте.
/// Если в какую-то дату инвентаризируется хотя бы одна единица, в этот день инвентаризируется
/// <b>всё</b> наличие — см. <see cref="FullInventoryOn"/>.
/// </para>
/// </summary>
internal static partial class SnkTestCases
{
    private static readonly DateOnly FirstInventoryDate = new(2022, 1, 19);
    private static readonly DateOnly SecondInventoryDate = new(2023, 1, 1);
    private static readonly DateOnly ReceiveDay = new(2023, 11, 10);
    private static readonly DateOnly RechargeDay = new(2023, 11, 29);
    private static readonly DateOnly ThirdRechargeDay = new(2024, 6, 1);
    private static readonly DateOnly LaterTransferDay = new(2024, 1, 29);

    /// <summary>Конец периода (EndDate). Фиксированная дата, позже всех операций.</summary>
    private static readonly DateOnly FinalDate = new(2026, 1, 1);

    public static IEnumerable<object[]> All() =>
        ValidCases()
            .Concat(ErrorCases())
            .Concat(NormalizationCases())
            .Concat(MultiUnitCases())
            .Concat(ZeroOperationCases())
            .Concat(MixedCases())
            .Concat(OrderCases())
            .Select(testCase => new object[] { testCase.Name, testCase });

    /// <summary>op.10 — инвентаризация якоря 999/001 (всегда в наличии с первой инв.).</summary>
    private static SnkTestOperationSpec Anchor(DateOnly date) =>
        Operation("10", date, "999", "001", "Тип-A", "кобальт-60", "1");

    /// <summary>Ожидаемая строка СНК для якоря 999/001.</summary>
    private static SnkStockSnapshot AnchorStock() =>
        Stock("999", "001", "Тип-A", "кобальт-60", "1");

    /// <summary>
    /// Полная инвентаризация на дату: якорь + перечисленные единицы (каждая — op.10).
    /// Правило отчёта: если инвентаризируем день, в форме op.10 должны быть все единицы в наличии.
    /// </summary>
    private static SnkTestOperationSpec[] FullInventoryOn(
        DateOnly date, params SnkTestOperationSpec[] unitInventoryLines) =>
        [Anchor(date), ..unitInventoryLines];

    private static KeyValuePair<DateOnly, IReadOnlyList<SnkStockSnapshot>> On(
        DateOnly date, params SnkStockSnapshot[] stock) =>
        new(date, stock);

    private static Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>> ByDate(
        params KeyValuePair<DateOnly, IReadOnlyList<SnkStockSnapshot>>[] entries) =>
        entries.ToDictionary(entry => entry.Key, entry => entry.Value);

    private static SnkTestOperationSpec Operation(
        string opCode,
        DateOnly opDate,
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string packNumber,
        int quantity = 1) =>
        new(
            OpCode: opCode,
            OpDate: opDate,
            PasNum: pasNum,
            FacNum: facNum,
            Type: type,
            Radionuclids: radionuclids,
            PackNumber: packNumber,
            Quantity: quantity);

    /// <summary>op.10 — инвентаризация учётной единицы.</summary>
    private static SnkTestOperationSpec Inv(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1) =>
        Operation("10", date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>op.38 (или другой плюсовой код) — приём.</summary>
    private static SnkTestOperationSpec Receive(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "38") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>op.28 (или другой минусовой код) — передача.</summary>
    private static SnkTestOperationSpec Transfer(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "28") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>op.53/54 — перезарядка.</summary>
    private static SnkTestOperationSpec Recharge(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "53") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>Нулевая операция (по умолчанию op.64, на СНК не влияет).</summary>
    private static SnkTestOperationSpec Zero(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "64") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>Вариант O-группы: тот же эталон, другой порядок строк в <see cref="SnkTestCase.Operations"/>.</summary>
    private static SnkTestCase OrderVariant(
        string id,
        string orderLabel,
        IReadOnlyList<SnkTestOperationSpec> operations,
        SnkTestCase template) => new()
    {
        Name = $"{id}. {orderLabel}",
        EndDate = template.EndDate,
        Operations = operations,
        ExpectedSnkStock = template.ExpectedSnkStock,
        ExpectedInventoryStockByDate = template.ExpectedInventoryStockByDate,
        AllowedInventoryVsSnkDifferenceByDate = template.AllowedInventoryVsSnkDifferenceByDate,
        HasIntentionalInventoryErrors = template.HasIntentionalInventoryErrors,
    };

    private static SnkStockSnapshot Stock(
        string pasNum,
        string facNum,
        string type,
        string radionuclids,
        string packNumber,
        int quantity = 1) =>
        new(
            PasNum: pasNum,
            FacNum: facNum,
            Type: type,
            Radionuclids: radionuclids,
            PackNumber: packNumber,
            OpCode: "",
            OpDate: default,
            Quantity: quantity);
}
