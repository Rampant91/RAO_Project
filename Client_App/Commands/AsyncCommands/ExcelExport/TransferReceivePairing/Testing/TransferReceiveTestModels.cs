using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;

using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

/// <summary>
/// Строка операции приёма/передачи для unit-тестов (без БД).
/// </summary>
public sealed class TransferReceiveRow
{
    public int Id { get; init; }
    public int RepsId { get; init; }
    public int ReportId { get; init; }
    public string OrgOkpo { get; init; } = string.Empty;
    public string OpCode { get; init; } = string.Empty;
    public string OpDate { get; init; } = string.Empty;
    public string PasNum { get; init; } = string.Empty;
    public string FacNum { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Radionuclids { get; init; } = string.Empty;
    public string PackNumber { get; init; } = string.Empty;
    public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
    public string Activity { get; init; } = string.Empty;
    public string CreatorOkpo { get; init; } = string.Empty;
    public string CreationDate { get; init; } = string.Empty;
    public int? Quantity { get; init; }
    public byte? AggregateState { get; init; }

    /// <summary>
    /// Если null — выводится из кода операции (21/22/… = transfer).
    /// </summary>
    public bool? IsTransfer { get; init; }
}

/// <summary>
/// Сценарий сверки приёма-передачи (формы 1.1 / 1.3) и ожидания.
/// Closest: null = не проверять; пустой словарь = ожидаем отсутствие карт; иначе частичное сравнение.
/// </summary>
public sealed class TransferReceiveTestCase
{
    public required string Name { get; init; }

    /// <summary>Номер формы сценария («1.1» или «1.3»).</summary>
    public string FormNum { get; init; } = "1.1";

    public TransferReceiveFormParams Params { get; init; } = new();

    /// <summary>ОКПО выбранной организации (нормализуется в runner).</summary>
    public string OurOkpo { get; init; } = "10000001";

    public IReadOnlyList<TransferReceiveRow> OurOps { get; init; } = [];
    public IReadOnlyList<TransferReceiveRow> CounterpartOps { get; init; } = [];

    /// <summary>
    /// Опциональные алиасы ОКПО → RepsId (как после LoadRepsIdsByOkpo):
    /// ключ — уже нормализованный ОКПО из кол. 19.
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyList<int>>? OkpoAliases { get; init; }

    public IReadOnlyList<int> ExpectedUnpairedIds { get; init; } = [];

    public IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>? ExpectedClosest { get; init; }

    /// <summary>Ожидаемый уровень полей (Exact/Near/Mismatch); частичное сравнение.</summary>
    public IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>? ExpectedClosestLevels { get; init; }

    /// <summary>Ожидаемый Id кандидата closest для unpaired Id.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidateIds { get; init; }

    /// <summary>Минимальная «Схожесть, %» для unpaired Id (частично).</summary>
    public IReadOnlyDictionary<int, int>? ExpectedConfidenceMinPercent { get; init; }
}

public sealed record TransferReceiveScenarioResult(IReadOnlyList<int> UnpairedIds);

public sealed record TransferReceiveClosestMatchResult(
    IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>> Closest,
    IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>> Levels,
    IReadOnlyDictionary<int, int> ConfidencePercent,
    IReadOnlyDictionary<int, int> CandidateIds);
