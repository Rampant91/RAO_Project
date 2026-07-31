using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа F13 — форма 1.3: агрегатное состояние, qty всегда 1.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> Form13Cases()
    {
        yield return F13_V01_IdealPair_EmptyResult();
        yield return F13_A01_NoCounterpart_Unpaired();
        yield return F13_Agg01_DifferentAggregateState_Unpaired();
        yield return F13_Agg02_CheckAggregateStateOff_DifferentState_Paired();
        yield return F13_D01_OneDayDiff_InReport_ClosestRedDate();
        yield return F13_Q01_EmptySerial_AlwaysOneToOne_Paired();
    }

    private static IEnumerable<TransferReceiveTestCase> Form13ClosestMatchCases()
    {
        yield return F13_D01_OneDayDiff_InReport_ClosestRedDate();
    }

    private static TransferReceiveRow Form13Transfer(
        int id,
        byte? aggregateState = 1,
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001") =>
        RowTransfer(id, opDate: opDate, pasNum: pasNum, facNum: facNum, quantity: 1, aggregateState: aggregateState);

    private static TransferReceiveRow Form13Receive(
        int id,
        byte? aggregateState = 1,
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001") =>
        RowReceive(id, opDate: opDate, pasNum: pasNum, facNum: facNum, quantity: 1, aggregateState: aggregateState);

    /// <summary>F13_V01. Идеальная пара 21↔31, AggregateState=1 → непарных нет.</summary>
    private static TransferReceiveTestCase F13_V01_IdealPair_EmptyResult() => new()
    {
        Name = "F13_V01. Идеальная пара 21↔31, AggregateState=1 — непарных нет.",
        FormNum = "1.3",
        Params = DefaultForm13Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1, aggregateState: 1)],
        CounterpartOps = [Form13Receive(101, aggregateState: 1)],
        ExpectedUnpairedIds = []
    };

    /// <summary>F13_A01. Нет контрагента → непарная.</summary>
    private static TransferReceiveTestCase F13_A01_NoCounterpart_Unpaired() => new()
    {
        Name = "F13_A01. Нет контрагента — непарная.",
        FormNum = "1.3",
        Params = DefaultForm13Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F13_Agg01. Разное AggregateState → непарная.</summary>
    private static TransferReceiveTestCase F13_Agg01_DifferentAggregateState_Unpaired() => new()
    {
        Name = "F13_Agg01. Разное AggregateState — непарная.",
        FormNum = "1.3",
        Params = DefaultForm13Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1, aggregateState: 1)],
        CounterpartOps = [Form13Receive(101, aggregateState: 2)],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F13_Agg02. CheckAggregateState=false при разном состоянии → пара.</summary>
    private static TransferReceiveTestCase F13_Agg02_CheckAggregateStateOff_DifferentState_Paired() => new()
    {
        Name = "F13_Agg02. CheckAggregateState=false при разном состоянии — пара.",
        FormNum = "1.3",
        Params = new TransferReceiveFormParams(CheckQuantity: false, CheckAggregateState: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1, aggregateState: 1)],
        CounterpartOps = [Form13Receive(101, aggregateState: 2)],
        ExpectedUnpairedIds = []
    };

    /// <summary>
    /// F13_D01. Дата +1 день → непарная; closest есть, дата красная.
    /// </summary>
    private static TransferReceiveTestCase F13_D01_OneDayDiff_InReport_ClosestRedDate() => new()
    {
        Name = "F13_D01. Дата +1 день — в отчёте; closest с красной датой.",
        FormNum = "1.3",
        Params = DefaultForm13Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1, aggregateState: 1, opDate: "2024-06-15")],
        CounterpartOps = [Form13Receive(101, aggregateState: 1, opDate: "2024-06-16")],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>
        {
            [1] = new Dictionary<TransferReceiveField, bool>
            {
                [TransferReceiveField.OperationCode] = true,
                [TransferReceiveField.OperationDate] = false,
                [TransferReceiveField.PassportNumber] = true,
                [TransferReceiveField.Type] = true,
                [TransferReceiveField.Radionuclids] = true,
                [TransferReceiveField.FactoryNumber] = true,
                [TransferReceiveField.AggregateState] = true,
                [TransferReceiveField.Activity] = true,
                [TransferReceiveField.CreatorOkpo] = true,
                [TransferReceiveField.CreationDate] = true,
                [TransferReceiveField.ProviderOrRecieverOkpo] = true,
                [TransferReceiveField.PackNumber] = true
            }
        }
    };

    /// <summary>F13_Q01. Пустые серии: qty всегда 1 → 1:1 пара.</summary>
    private static TransferReceiveTestCase F13_Q01_EmptySerial_AlwaysOneToOne_Paired() => new()
    {
        Name = "F13_Q01. Пустые серии, qty всегда 1 — пара 1:1.",
        FormNum = "1.3",
        Params = DefaultForm13Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form13Transfer(1, aggregateState: 1, pasNum: "-", facNum: "-")],
        CounterpartOps = [Form13Receive(101, aggregateState: 1, pasNum: "-", facNum: "-")],
        ExpectedUnpairedIds = []
    };
}
