using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;

using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

/// <summary>
/// Строка операции 41 для unit-тестов сопоставления (без БД).
/// </summary>
public sealed class Pairing41Row
{
    public int Id { get; init; }
    public int RepsId { get; init; }
    public int ReportId { get; init; }
    public string OpCode { get; init; } = "41";
    public string OpDate { get; init; } = string.Empty;
    public string PasNum { get; init; } = string.Empty;
    public string FacNum { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Radionuclids { get; init; } = string.Empty;
    public string CreationDate { get; init; } = string.Empty;
    public byte? DocumentVid { get; init; }
    public string DocumentNumber { get; init; } = string.Empty;
    public string DocumentDate { get; init; } = string.Empty;
    public string ProviderOrRecieverOkpo { get; init; } = string.Empty;
    public string TransporterOkpo { get; init; } = string.Empty;
    public string PackName { get; init; } = string.Empty;
    public string PackType { get; init; } = string.Empty;
    public string PackNumber { get; init; } = string.Empty;
    public string Activity { get; init; } = string.Empty;
    public string MainRadionuclids { get; init; } = string.Empty;
    public string TritiumActivity { get; init; } = string.Empty;
    public string BetaGammaActivity { get; init; } = string.Empty;
    public string AlphaActivity { get; init; } = string.Empty;
    public string TransuraniumActivity { get; init; } = string.Empty;
    public string Mass { get; init; } = string.Empty;
    public string Volume { get; init; } = string.Empty;
    public string ActivityMeasurementDate { get; init; } = string.Empty;
    public int? Quantity { get; init; }
    public string FormNum { get; init; } = string.Empty;
    public string CodeRao { get; init; } = string.Empty;
    public byte? AggregateState { get; init; }
    public string OrgRegNo { get; init; } = string.Empty;
}

/// <summary>
/// Ожидание closest-match для непарной строки 1.6 (профиль РВ + частичная карта полей).
/// </summary>
public sealed class Pairing41Form16ClosestExpectation
{
    public required Form16MatchProfile Profile { get; init; }
    public IReadOnlyDictionary<Pairing12To16Field, bool>? Matches12 { get; init; }
    public IReadOnlyDictionary<Pairing13To16Field, bool>? Matches13 { get; init; }
    public IReadOnlyDictionary<Pairing14To16Field, bool>? Matches14 { get; init; }
}

/// <summary>
/// Сценарий сопоставления парности 41 и ожидаемые непарные Id по формам.
/// Ожидания closest-match опциональны: null = не проверять; заданный словарь = частичное сравнение.
/// </summary>
public sealed class Pairing41TestCase
{
    public required string Name { get; init; }

    public Pairing11To15Params Params11To15 { get; init; } = new();
    public Pairing12To16Params Params12To16 { get; init; } = new();
    public Pairing13To16Params Params13To16 { get; init; } = new();
    public Pairing14To16Params Params14To16 { get; init; } = new();

    public IReadOnlyList<Pairing41Row> Form11 { get; init; } = [];
    public IReadOnlyList<Pairing41Row> Form12 { get; init; } = [];
    public IReadOnlyList<Pairing41Row> Form13 { get; init; } = [];
    public IReadOnlyList<Pairing41Row> Form14 { get; init; } = [];
    public IReadOnlyList<Pairing41Row> Form15 { get; init; } = [];
    public IReadOnlyList<Pairing41Row> Form16 { get; init; } = [];

    public IReadOnlyList<int> ExpectedUnpaired11 { get; init; } = [];
    public IReadOnlyList<int> ExpectedUnpaired12 { get; init; } = [];
    public IReadOnlyList<int> ExpectedUnpaired13 { get; init; } = [];
    public IReadOnlyList<int> ExpectedUnpaired14 { get; init; } = [];
    public IReadOnlyList<int> ExpectedUnpaired15 { get; init; } = [];
    public IReadOnlyList<int> ExpectedUnpaired16 { get; init; } = [];

    /// <summary>Частичная карта: Id → (поле → совпало?). Null = не проверять closest для 1.1.</summary>
    public IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>? ExpectedClosest11 { get; init; }

    public IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>? ExpectedClosest15 { get; init; }
    public IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing12To16Field, bool>>? ExpectedClosest12 { get; init; }
    public IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing13To16Field, bool>>? ExpectedClosest13 { get; init; }
    public IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing14To16Field, bool>>? ExpectedClosest14 { get; init; }
    public IReadOnlyDictionary<int, Pairing41Form16ClosestExpectation>? ExpectedClosest16 { get; init; }

    /// <summary>Id непарной → Id ближайшего кандидата (форма 1.5). Null = не проверять.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate11 { get; init; }

    /// <summary>Id непарной 1.5 → Id ближайшего кандидата (форма 1.1).</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate15 { get; init; }

    /// <summary>Id непарной 1.2 → Id ближайшего кандидата 1.6.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate12 { get; init; }

    /// <summary>Id непарной 1.3 → Id ближайшего кандидата 1.6.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate13 { get; init; }

    /// <summary>Id непарной 1.4 → Id ближайшего кандидата 1.6.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate14 { get; init; }

    /// <summary>Id непарной 1.6 → Id ближайшего кандидата на стороне РВ.</summary>
    public IReadOnlyDictionary<int, int>? ExpectedClosestCandidate16 { get; init; }

    /// <summary>Id → ожидаемое AggregateStateMatchesCodeRao closest-кандидата на 1.6 (для непарной 1.3).</summary>
    public IReadOnlyDictionary<int, bool>? ExpectedAggregateStateMatch13 { get; init; }

    /// <summary>Id → ожидаемое AggregateStateMatchesCodeRao closest-кандидата на 1.6 (для непарной 1.4).</summary>
    public IReadOnlyDictionary<int, bool>? ExpectedAggregateStateMatch14 { get; init; }
}

public sealed record Pairing41ScenarioResult(
    IReadOnlyList<int> Unpaired11,
    IReadOnlyList<int> Unpaired12,
    IReadOnlyList<int> Unpaired13,
    IReadOnlyList<int> Unpaired14,
    IReadOnlyList<int> Unpaired15,
    IReadOnlyList<int> Unpaired16);

public sealed record Pairing41ClosestMatchResult(
    IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>> Closest11,
    IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>> Closest15,
    IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing12To16Field, bool>> Closest12,
    IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing13To16Field, bool>> Closest13,
    IReadOnlyDictionary<int, IReadOnlyDictionary<Pairing14To16Field, bool>> Closest14,
    IReadOnlyDictionary<int, Form16ClosestMatchHighlight> Closest16,
    IReadOnlyDictionary<int, bool?> Closest13AggregateStateMatch,
    IReadOnlyDictionary<int, bool?> Closest14AggregateStateMatch,
    IReadOnlyDictionary<int, int> Closest11CandidateIds,
    IReadOnlyDictionary<int, int> Closest15CandidateIds,
    IReadOnlyDictionary<int, int> Closest12CandidateIds,
    IReadOnlyDictionary<int, int> Closest13CandidateIds,
    IReadOnlyDictionary<int, int> Closest14CandidateIds,
    IReadOnlyDictionary<int, int> Closest16CandidateIds);
