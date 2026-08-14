using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа F14 — форма 1.4: вид, объём, дата изм. активности, масса, агр. состояние.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> Form14Cases()
    {
        yield return F14_V01_IdealPair_EmptyResult();
        yield return F14_A01_NoCounterpart_Unpaired();
        yield return F14_Sort01_DifferentSort_Unpaired();
        yield return F14_Sort02_CheckSortOff_DifferentSort_Paired();
        yield return F14_Vol01_VolumeOutsideTolerance_Unpaired();
        yield return F14_Vol02_VolumeWithin10Percent_Paired();
        yield return F14_Amd01_DifferentActivityMeasurementDate_Unpaired();
        yield return F14_Q01_EmptyPassport_AlwaysOneToOne_Paired();
    }

    private static IEnumerable<TransferReceiveTestCase> Form14ClosestMatchCases()
    {
        yield return F14_Amd01_DifferentActivityMeasurementDate_Unpaired();
    }

    private static TransferReceiveRow Form14Transfer(
        int id,
        byte? sort = 1,
        byte? aggregateState = 1,
        string? opDate = null,
        string pasNum = "P-001",
        string name = "Источник",
        string volume = "1.0",
        string mass = "2.0",
        string? activityMeasurementDate = null,
        string activity = "1.0e+6") =>
        new()
        {
            Id = id,
            RepsId = OurRepsId,
            OrgOkpo = DefaultOurOkpo,
            OpCode = "21",
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            Type = name,
            Sort = sort,
            Radionuclids = "Cs-137",
            Activity = activity,
            ActivityMeasurementDate = activityMeasurementDate ?? DefaultOpDate,
            Volume = volume,
            Mass = mass,
            AggregateState = aggregateState,
            PackNumber = "U-1",
            ProviderOrRecieverOkpo = DefaultCounterpartOkpo,
            Quantity = 1,
            IsTransfer = true
        };

    private static TransferReceiveRow Form14Receive(
        int id,
        byte? sort = 1,
        byte? aggregateState = 1,
        string? opDate = null,
        string pasNum = "P-001",
        string name = "Источник",
        string volume = "1.0",
        string mass = "2.0",
        string? activityMeasurementDate = null,
        string activity = "1.0e+6") =>
        new()
        {
            Id = id,
            RepsId = CounterpartRepsId,
            OrgOkpo = DefaultCounterpartOkpo,
            OpCode = "31",
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            Type = name,
            Sort = sort,
            Radionuclids = "Cs-137",
            Activity = activity,
            ActivityMeasurementDate = activityMeasurementDate ?? DefaultOpDate,
            Volume = volume,
            Mass = mass,
            AggregateState = aggregateState,
            PackNumber = "U-1",
            ProviderOrRecieverOkpo = DefaultOurOkpo,
            Quantity = 1,
            IsTransfer = false
        };

    /// <summary>F14_V01. Идеальная пара 21↔31 → непарных нет.</summary>
    private static TransferReceiveTestCase F14_V01_IdealPair_EmptyResult() => new()
    {
        Name = "F14_V01. Идеальная пара 21↔31 — непарных нет.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1)],
        CounterpartOps = [Form14Receive(101)],
        ExpectedUnpairedIds = []
    };

    /// <summary>F14_A01. Нет контрагента → непарная.</summary>
    private static TransferReceiveTestCase F14_A01_NoCounterpart_Unpaired() => new()
    {
        Name = "F14_A01. Нет контрагента — операция непарная.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F14_Sort01. Разный вид → непарная.</summary>
    private static TransferReceiveTestCase F14_Sort01_DifferentSort_Unpaired() => new()
    {
        Name = "F14_Sort01. Разный Sort — непарная.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, sort: 1)],
        CounterpartOps = [Form14Receive(101, sort: 2)],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F14_Sort02. CheckSort=false и разный вид → пара.</summary>
    private static TransferReceiveTestCase F14_Sort02_CheckSortOff_DifferentSort_Paired() => new()
    {
        Name = "F14_Sort02. CheckSort=false, разный Sort — пара.",
        FormNum = "1.4",
        Params = DefaultForm14Params() with { CheckSort = false },
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, sort: 1)],
        CounterpartOps = [Form14Receive(101, sort: 2)],
        ExpectedUnpairedIds = []
    };

    /// <summary>F14_Vol01. Объём вне ±10% → непарная.</summary>
    private static TransferReceiveTestCase F14_Vol01_VolumeOutsideTolerance_Unpaired() => new()
    {
        Name = "F14_Vol01. Объём вне ±10% — непарная.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, volume: "1.0")],
        CounterpartOps = [Form14Receive(101, volume: "1.2")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F14_Vol02. Объём в допуске ±10% → пара.</summary>
    private static TransferReceiveTestCase F14_Vol02_VolumeWithin10Percent_Paired() => new()
    {
        Name = "F14_Vol02. Объём в допуске ±10% — пара.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, volume: "1.0")],
        CounterpartOps = [Form14Receive(101, volume: "1.05")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F14_Amd01. Разная дата измерения активности → непарная.</summary>
    private static TransferReceiveTestCase F14_Amd01_DifferentActivityMeasurementDate_Unpaired() => new()
    {
        Name = "F14_Amd01. Разная дата измерения активности — непарная.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, activityMeasurementDate: "2024-06-15")],
        CounterpartOps = [Form14Receive(101, activityMeasurementDate: "2024-06-16")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.ActivityMeasurementDate] = FieldMatchLevel.Near
            }
        }
    };

    /// <summary>F14_Q01. Пустой паспорт — всегда 1:1, qty не суммируется.</summary>
    private static TransferReceiveTestCase F14_Q01_EmptyPassport_AlwaysOneToOne_Paired() => new()
    {
        Name = "F14_Q01. Пустой паспорт — 1:1, пара есть.",
        FormNum = "1.4",
        Params = DefaultForm14Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form14Transfer(1, pasNum: "-")],
        CounterpartOps = [Form14Receive(101, pasNum: "-")],
        ExpectedUnpairedIds = []
    };
}
