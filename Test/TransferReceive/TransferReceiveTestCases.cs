using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;

namespace Test.TransferReceive;

/// <summary>
/// Наборы тестовых сценариев проверки операций приёма-передачи (формы 1.1–1.6).
/// <para>
/// Группы:
/// <list type="bullet">
/// <item><b>V</b> — smoke, идеальная пара;</item>
/// <item><b>A</b> — попадание / непопадание в непарные;</item>
/// <item><b>C</b> — таблица пар кодов;</item>
/// <item><b>D</b> — точная дата vs окно поиска ±15 дней;</item>
/// <item><b>Q</b> — пустые серии / суммирование количества;</item>
/// <item><b>N</b> — нормализация написания (сценарии);</item>
/// <item><b>P</b> — опции диалога (Check* = false);</item>
/// <item><b>O</b> — ОКПО кол. 19 / self-pair / нет контрагента;</item>
/// <item><b>H</b> — карты closest-match;</item>
/// <item><b>F12</b> — форма 1.2 (масса, тип УКТ, qty=1);</item>
/// <item><b>F13</b> — форма 1.3 (агрегатное состояние, qty=1);</item>
/// <item><b>F14</b> — форма 1.4 (вид, объём, дата изм. активности);</item>
/// <item><b>F15</b> — форма 1.5 (как 1.1 без изготовителя; 26↔36).</item>
/// <item><b>F16</b> — форма 1.6 (код РАО, 4 активности; без qty-drain).</item>
/// </list>
/// </para>
/// <para>
/// Строки уже в единицах сравнения (как после Load DTO). Даты-якоря:
/// <see cref="DefaultOpDate"/>. Id: our 1,2,3…; counterpart 101,202…
/// OurOkpo по умолчанию <see cref="DefaultOurOkpo"/>, контрагент — <see cref="DefaultCounterpartOkpo"/>.
/// </para>
/// </summary>
internal static partial class TransferReceiveTestCases
{
    private const string DefaultOpDate = "2024-06-15";
    private const string DefaultOurOkpo = "10000001";
    private const string DefaultCounterpartOkpo = "20000002";
    private const int OurRepsId = 1;
    private const int CounterpartRepsId = 2;

    public static IEnumerable<object[]> All() =>
        ValidCases()
            .Concat(PairingCases())
            .Concat(CodeCases())
            .Concat(DateCases())
            .Concat(QuantityCases())
            .Concat(NormalizationCases())
            .Concat(ParamsCases())
            .Concat(OkpoCases())
            .Concat(OkpoClosestCases())
            .Concat(ClosestMatchCases())
            .Concat(SoftClosestCases())
            .Concat(Form12Cases())
            .Concat(Form13Cases())
            .Concat(Form14Cases())
            .Concat(Form15Cases())
            .Concat(Form16Cases())
            .Select(testCase => new object[] { testCase.Name, testCase });

    public static IEnumerable<object[]> ClosestMatchOnly() =>
        ClosestMatchCases()
            .Concat(SoftClosestCases())
            .Concat(Form12ClosestMatchCases())
            .Concat(Form13ClosestMatchCases())
            .Concat(Form14ClosestMatchCases())
            .Concat(Form15ClosestMatchCases())
            .Concat(Form16ClosestMatchCases())
            .Where(testCase => testCase.ExpectedClosest is not null
                               || testCase.ExpectedClosestLevels is not null
                               || testCase.ExpectedClosestCandidateIds is not null
                               || testCase.ExpectedConfidenceMinPercent is not null)
            .Select(testCase => new object[] { testCase.Name, testCase });

    /// <summary>Передача 1.1 (код 21 по умолчанию) от нашей org к контрагенту.</summary>
    private static TransferReceiveRow RowTransfer(
        int id,
        string opCode = "21",
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string type = "ИИИ",
        string rads = "Cs-137",
        string pack = "U-1",
        string? providerOkpo = null,
        string activity = "1.0e+6",
        string creatorOkpo = "30000003",
        string? creationDate = null,
        int? quantity = 1,
        byte? aggregateState = null,
        string? orgOkpo = null,
        int? repsId = null) =>
        new()
        {
            Id = id,
            RepsId = repsId ?? OurRepsId,
            OrgOkpo = orgOkpo ?? DefaultOurOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = type,
            Radionuclids = rads,
            PackNumber = pack,
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultCounterpartOkpo,
            Activity = activity,
            CreatorOkpo = creatorOkpo,
            CreationDate = creationDate ?? DefaultOpDate,
            Quantity = quantity,
            AggregateState = aggregateState,
            IsTransfer = true
        };

    /// <summary>Приём 1.1 (код 31 по умолчанию) у контрагента от нашей org.</summary>
    private static TransferReceiveRow RowReceive(
        int id,
        string opCode = "31",
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string type = "ИИИ",
        string rads = "Cs-137",
        string pack = "U-1",
        string? providerOkpo = null,
        string activity = "1.0e+6",
        string creatorOkpo = "30000003",
        string? creationDate = null,
        int? quantity = 1,
        byte? aggregateState = null,
        string? orgOkpo = null,
        int? repsId = null) =>
        new()
        {
            Id = id,
            RepsId = repsId ?? CounterpartRepsId,
            OrgOkpo = orgOkpo ?? DefaultCounterpartOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = type,
            Radionuclids = rads,
            PackNumber = pack,
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultOurOkpo,
            Activity = activity,
            CreatorOkpo = creatorOkpo,
            CreationDate = creationDate ?? DefaultOpDate,
            Quantity = quantity,
            AggregateState = aggregateState,
            IsTransfer = false
        };

    /// <summary>Строка передачи для SCALE-тестов (уникальный паспорт/зав.№).</summary>
    internal static TransferReceiveRow CreateTransferForScale(int id, string? activity = null) =>
        RowTransfer(
            id,
            pasNum: $"P-{id}",
            facNum: $"F-{id}",
            activity: activity ?? "1.0e+6",
            quantity: 1);

    /// <summary>Строка приёма для SCALE-тестов, парная к <see cref="CreateTransferForScale"/>.</summary>
    internal static TransferReceiveRow CreateReceiveForScale(int id, int pairTransferId, string? activity = null) =>
        RowReceive(
            id,
            pasNum: $"P-{pairTransferId}",
            facNum: $"F-{pairTransferId}",
            activity: activity ?? "1.0e+6",
            quantity: 1);

    /// <summary>Безсерийная передача для SCALE qty-drain.</summary>
    internal static TransferReceiveRow CreateEmptySerialTransferForScale(int id, int quantity) =>
        RowTransfer(id, pasNum: "-", facNum: "-", quantity: quantity);

    /// <summary>Безсерийный приём для SCALE qty-drain.</summary>
    internal static TransferReceiveRow CreateEmptySerialReceiveForScale(int id, int quantity) =>
        RowReceive(id, pasNum: "-", facNum: "-", quantity: quantity);
}
