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
    /// В будущем рядом с этим словарём будет добавлен ожидаемый список ошибок по тем же датам.

    /// </summary>

    public required IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>> ExpectedInventoryStockByDate { get; init; }



    /// <summary>

    /// Необязательно. Единицы, для которых на указанную дату допустимо расхождение между

    /// таблицей инвентаризации (строки op.10) и расчётным СНК. Типичный пример —

    /// ЗРИ проинвентаризировали и в тот же день передали (op.28): он есть в инвентаризации,

    /// но его уже нет в наличии на конец дня. Такое расхождение не считается ошибкой.

    /// По умолчанию пусто: любое расхождение «есть в инвентаризации, нет в СНК» считается ошибкой.

    /// </summary>

    public IReadOnlyDictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>> AllowedInventoryVsSnkDifferenceByDate { get; init; }

        = new Dictionary<DateOnly, IReadOnlyList<SnkStockSnapshot>>();



    /// <summary>

    /// Сценарий содержит намеренные логические ошибки пользователя (передача отсутствующего ЗРИ,

    /// инвентаризация ранее снятого с учёта и т.п.). Списки СНК и инвентаризации всё равно проверяются,

    /// но проверка «инвентаризация vs СНК» (допустимость расхождения) пропускается, т.к. сами ошибки

    /// будут отлавливаться отдельно. По умолчанию false.

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

/// Результат прогона тестового сценария.

/// </summary>

public sealed class SnkScenarioResult

{

    public required IReadOnlyList<SnkUnitSnapshot> GroupedUnits { get; init; }



    public required IReadOnlyList<SnkStockSnapshot> UnitsInStock { get; init; }



    public DateOnly FirstInventoryDate { get; init; }

}


