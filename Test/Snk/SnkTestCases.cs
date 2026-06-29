using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Наборы тестовых сценариев СНК и проверки инвентаризаций.
/// Сгруппированы по смыслу (каждая группа — в отдельном файле-партиале):
/// <list type="bullet">
/// <item><b>V##</b> — корректные сценарии без ошибок пользователя (<see cref="ValidCases"/>).</item>
/// <item><b>E##</b> — сценарии с логическими ошибками пользователя (<see cref="ErrorCases"/>).</item>
/// <item><b>N##</b> — распознавание единиц при разном написании: кириллица/латиница,
/// регистр, спецсимволы, ведущие нули, порядок радионуклидов (<see cref="NormalizationCases"/>).</item>
/// <item><b>M##</b> — несколько единиц одновременно в списках (<see cref="MultiUnitCases"/>).</item>
/// <item><b>Z##</b> — нулевые операции (<see cref="ZeroOperationCases"/>).</item>
/// <item><b>X##</b> — длинные смешанные сценарии (<see cref="MixedCases"/>).</item>
/// <item><b>O##</b> — те же логические сценарии с разным порядком строк операций
/// в один день; эталон наличия должен совпадать (<see cref="OrderCases"/>).</item>
/// </list>
/// Префикс и номер в имени сохранены, чтобы быстро находить кейс по упавшему тесту.
/// Во всех сценариях присутствует «якорная» единица 999/001, которая стоит на учёте с первой
/// инвентаризации и никогда не двигается — она фиксирует дату первой инвентаризации и служит
/// постоянным элементом полного наличия на каждую дату инвентаризации.
/// </summary>
internal static partial class SnkTestCases
{
    private static readonly DateOnly FirstInventoryDate = new(2022, 1, 19);
    private static readonly DateOnly SecondInventoryDate = new(2023, 1, 1);
    private static readonly DateOnly ReceiveDay = new(2023, 11, 10);
    private static readonly DateOnly RechargeDay = new(2023, 11, 29);
    private static readonly DateOnly ThirdRechargeDay = new(2024, 6, 1);
    private static readonly DateOnly LaterTransferDay = new(2024, 1, 29);

    /// <summary>
    /// Дата окончания периода (EndDate) для всех кейсов. Фиксированная (а не DateTime.Today),
    /// чтобы прогон тестов был детерминированным и не зависел от системной даты.
    /// Заведомо позже всех операций в кейсах и не совпадает ни с одной датой инвентаризации.
    /// </summary>
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

    /// <summary>Операция инвентаризации (код 10) якорной единицы 999/001 на заданную дату.</summary>
    private static SnkTestOperationSpec Anchor(DateOnly date) =>
        Operation("10", date, "999", "001", "Тип-A", "кобальт-60", "1");

    /// <summary>Строка СНК для якорной единицы 999/001.</summary>
    private static SnkStockSnapshot AnchorStock() =>
        Stock("999", "001", "Тип-A", "кобальт-60", "1");

    /// <summary>Пара «дата → ожидаемое наличие» для словарей результатов.</summary>
    private static KeyValuePair<DateOnly, IReadOnlyList<SnkStockSnapshot>> On(
        DateOnly date, params SnkStockSnapshot[] stock) =>
        new(date, stock);

    /// <summary>Собирает словарь «дата → наличие» из пар <see cref="On"/>.</summary>
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

    /// <summary>Операция инвентаризации (код 10) произвольной единицы.</summary>
    private static SnkTestOperationSpec Inv(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1) =>
        Operation("10", date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>Операция получения (плюсовая). Код по умолчанию 38, можно задать другой плюсовой код формы.</summary>
    private static SnkTestOperationSpec Receive(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "38") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>Операция передачи (минусовая). Код по умолчанию 28, можно задать другой минусовой код формы.</summary>
    private static SnkTestOperationSpec Transfer(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "28") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>Операция перезарядки. Код по умолчанию 53, допустимо 54.</summary>
    private static SnkTestOperationSpec Recharge(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "53") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>
    /// Нулевая операция (не влияет на наличие). Код по умолчанию 64 — нейтральный,
    /// не помечается ошибкой 8 даже для отсутствующего ЗРИ. Для «ошибочной» нулевой
    /// операции у несуществующего ЗРИ задайте иной код (например, 99).
    /// </summary>
    private static SnkTestOperationSpec Zero(
        DateOnly date, string pasNum, string facNum, string type, string radionuclids,
        string packNumber, int quantity = 1, string opCode = "64") =>
        Operation(opCode, date, pasNum, facNum, type, radionuclids, packNumber, quantity);

    /// <summary>
    /// Вариант сценария с тем же эталоном, но другим порядком операций в массиве
    /// (порядок строк в отчёте в один день). Имя: «O##. … — порядок: …».
    /// </summary>
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

    private static List<SnkTestOperationSpec> Ops(params SnkTestOperationSpec[] operations) =>
        [.. operations];

    /// <summary>Ожидаемая строка СНК: только ключевые поля (форма 1.1).</summary>
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
