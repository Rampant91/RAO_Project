using System;
using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

/// <summary>
/// Режим формирования union операций (как в выгрузке СНК или в проверке инвентаризаций).
/// </summary>
public enum SnkInventoryUnionMode
{
    /// <summary>Инвентаризации только с первой даты (как ExcelExportSnkAsyncCommand).</summary>
    SnkExport,

    /// <summary>Все инвентаризации (как ExcelExportCheckInventoriesAsyncCommand).</summary>
    CheckInventories
}

/// <summary>
/// Типы ошибок проверки инвентаризаций (соответствуют номерам 0–9 в отчёте).
/// </summary>
public enum SnkInventoryErrorType
{
    InventoryDuplicate = 0,
    RegisteredAndNotInventoriedUnit = 1,
    MissingFromInventoryUnit = 2,
    GivenUnitIsInventoried = 3,
    UnInventoriedUnitGivenAway = 4,
    ReDeRegistration = 5,
    InventoriedUnitReceived = 6,
    ReRegistration = 7,
    ZeroOperationWithUnInventoriedUnit = 8,
    QuantityGivenExceedsAvailable = 9
}

/// <summary>
/// Ожидаемая ошибка проверки инвентаризаций на дату.
/// </summary>
public sealed record SnkExpectedInventoryError(
    SnkInventoryErrorType ErrorType,
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string PackNumber,
    string? OpCode = null,
    DateOnly? OpDate = null);

/// <summary>
/// Минимальное описание одной операции для тестового сценария.
/// </summary>
public sealed record SnkTestOperationSpec(
    string OpCode,
    DateOnly OpDate,
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string PackNumber,
    int Quantity = 1);

/// <summary>
/// Тестовый кейс: входные операции и ожидаемые результаты для выгрузки СНК и проверки инвентаризаций.
/// </summary>
public sealed class SnkTestCase
{
    public required string Name { get; init; }

    public string FormNum { get; init; } = "1.1";

    /// <summary>
    /// Конец периода (дата формирования СНК в выгрузке и верхняя граница для проверки инвентаризаций).
    /// </summary>
    public required DateOnly EndDate { get; init; }

    /// <summary>Все операции сценария.</summary>
    public required IReadOnlyList<SnkTestOperationSpec> Operations { get; init; }

    /// <summary>
    /// Ожидаемый СНК для ExcelExportSnkAsyncCommand на дату <see cref="EndDate"/>.
    /// </summary>
    public required IReadOnlyList<SnkStockSnapshot> ExpectedSnkStock { get; init; }

    /// <summary>
    /// Ожидаемый СНК для ExcelExportCheckInventoriesAsyncCommand на каждую уникальную дату op.10
    /// в пределах <see cref="EndDate"/> и на саму дату <see cref="EndDate"/>.
    /// </summary>
    public required IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>> ExpectedInventoryStockByDate { get; init; }

    /// <summary>
    /// Ожидаемые ошибки проверки инвентаризаций по датам проверки (op.10 + EndDate).
    /// </summary>
    public IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkExpectedInventoryError>> ExpectedInventoryErrorsByDate { get; init; }
        = new Dictionary<DateOnly, IReadOnlyList<SnkExpectedInventoryError>>();

    /// <summary>
    /// Необязательно. Единицы, для которых на указанную дату допустимо расхождение между
    /// таблицей инвентаризации (строки op.10) и расчётным СНК.
    /// </summary>
    public IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>> AllowedInventoryVsSnkDifferenceByDate { get; init; }
        = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>();

    /// <summary>
    /// Сценарий содержит намеренные логические ошибки пользователя.
    /// </summary>
    public bool HasIntentionalInventoryErrors { get; init; }
}

/// <summary>
/// Тестовый сценарий: цепочка операций без чтения из БД (низкоуровневый запуск).
/// </summary>
public sealed class SnkTestScenario
{
    public string FormNum { get; init; } = "1.1";

    public SnkInventoryUnionMode UnionMode { get; init; } = SnkInventoryUnionMode.CheckInventories;

    public DateOnly EndDate { get; init; }

    public required IReadOnlyList<SnkTestOperationSpec> Operations { get; init; }
}

/// <summary>
/// Снимок одной учётной единицы после группировки операций.
/// </summary>
public sealed record SnkUnitSnapshot(
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string PackNumber,
    IReadOnlyList<string> OpCodesInOrder);

/// <summary>
/// Снимок единицы в СНК (наличие на дату).
/// </summary>
public sealed record SnkStockSnapshot(
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string PackNumber,
    string OpCode,
    DateOnly OpDate,
    int Quantity);

/// <summary>
/// Фактическая ошибка проверки инвентаризаций (результат harness).
/// </summary>
public sealed record SnkActualInventoryError(
    SnkInventoryErrorType ErrorType,
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string PackNumber,
    string OpCode,
    DateOnly OpDate);

/// <summary>
/// Результат прогона тестового сценария.
/// </summary>
public sealed class SnkScenarioResult
{
    public required IReadOnlyList<SnkUnitSnapshot> GroupedUnits { get; init; }

    public required IReadOnlyList<SnkStockSnapshot> UnitsInStock { get; init; }

    public DateOnly FirstInventoryDate { get; init; }
}
