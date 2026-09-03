using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа F12 — форма 1.2: наименование, масса, тип УКТ, qty=1.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> Form12Cases()
    {
        yield return F12_V01_IdealPair_EmptyResult();
        yield return F12_A01_NoCounterpart_Unpaired();
        yield return F12_M01_MassOutsideTolerance_Unpaired();
        yield return F12_M02_CheckMassOff_DifferentMass_Paired();
        yield return F12_T01_DifferentPackType_Unpaired();
        yield return F12_T02_CheckPackTypeOff_DifferentType_Paired();
        yield return F12_D01_OneDayDiff_InReport_ClosestNearDate();
        yield return F12_Q01_EmptySerial_AlwaysOneToOne_Paired();
    }

    private static IEnumerable<TransferReceiveTestCase> Form12ClosestMatchCases()
    {
        yield return F12_D01_OneDayDiff_InReport_ClosestNearDate();
        yield return F12_M03_MassKgVsTon_ClosestNearMass();
    }

    private static TransferReceiveRow Form12Transfer(
        int id,
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string name = "Изделие-А",
        string mass = "1.5",
        string packType = "УКТ-1",
        string pack = "N-1") =>
        new()
        {
            Id = id,
            RepsId = OurRepsId,
            OrgOkpo = DefaultOurOkpo,
            OpCode = "21",
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = name,
            PackType = packType,
            PackNumber = pack,
            ProviderOrRecieverOkpo = DefaultCounterpartOkpo,
            Mass = mass,
            CreatorOkpo = "30000003",
            CreationDate = DefaultOpDate,
            Quantity = 1,
            IsTransfer = true
        };

    private static TransferReceiveRow Form12Receive(
        int id,
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string name = "Изделие-А",
        string mass = "1.5",
        string packType = "УКТ-1",
        string pack = "N-1") =>
        new()
        {
            Id = id,
            RepsId = CounterpartRepsId,
            OrgOkpo = DefaultCounterpartOkpo,
            OpCode = "31",
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = name,
            PackType = packType,
            PackNumber = pack,
            ProviderOrRecieverOkpo = DefaultOurOkpo,
            Mass = mass,
            CreatorOkpo = "30000003",
            CreationDate = DefaultOpDate,
            Quantity = 1,
            IsTransfer = false
        };

    private static TransferReceiveTestCase F12_V01_IdealPair_EmptyResult() => new()
    {
        Name = "F12_V01. Идеальная пара 21↔31 — непарных нет.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1)],
        CounterpartOps = [Form12Receive(101)],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F12_A01_NoCounterpart_Unpaired() => new()
    {
        Name = "F12_A01. Нет контрагента — непарная.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F12_M01_MassOutsideTolerance_Unpaired() => new()
    {
        Name = "F12_M01. Масса вне ±10% — непарная.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, mass: "1.5")],
        CounterpartOps = [Form12Receive(101, mass: "2.0")],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F12_M02_CheckMassOff_DifferentMass_Paired() => new()
    {
        Name = "F12_M02. CheckMass=false при разной массе — пара.",
        FormNum = "1.2",
        Params = new TransferReceiveFormParams(
            CheckQuantity: false,
            CheckRadionuclids: false,
            CheckActivity: false,
            CheckAggregateState: false,
            CheckMass: false,
            CheckPackType: true),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, mass: "1.5")],
        CounterpartOps = [Form12Receive(101, mass: "999")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F12_T01_DifferentPackType_Unpaired() => new()
    {
        Name = "F12_T01. Разный тип УКТ — непарная.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, packType: "УКТ-1")],
        CounterpartOps = [Form12Receive(101, packType: "УКТ-2")],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F12_T02_CheckPackTypeOff_DifferentType_Paired() => new()
    {
        Name = "F12_T02. CheckPackType=false при разном типе УКТ — пара.",
        FormNum = "1.2",
        Params = new TransferReceiveFormParams(
            CheckQuantity: false,
            CheckRadionuclids: false,
            CheckActivity: false,
            CheckAggregateState: false,
            CheckMass: true,
            CheckPackType: false),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, packType: "УКТ-1")],
        CounterpartOps = [Form12Receive(101, packType: "УКТ-2")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F12_D01_OneDayDiff_InReport_ClosestNearDate() => new()
    {
        Name = "F12_D01. Дата +1 день — в отчёте; дата Near.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, opDate: "2024-06-15")],
        CounterpartOps = [Form12Receive(101, opDate: "2024-06-16")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Near,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact,
                [TransferReceiveField.Mass] = FieldMatchLevel.Exact,
                [TransferReceiveField.PackType] = FieldMatchLevel.Exact
            }
        }
    };

    private static TransferReceiveTestCase F12_Q01_EmptySerial_AlwaysOneToOne_Paired() => new()
    {
        Name = "F12_Q01. Пустые серии — всегда 1:1, пара.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, pasNum: "-", facNum: "-")],
        CounterpartOps = [Form12Receive(101, pasNum: "-", facNum: "-")],
        ExpectedUnpairedIds = []
    };

    /// <summary>Масса ×1000 (кг↔т) — непарная, но Mass = Near.</summary>
    private static TransferReceiveTestCase F12_M03_MassKgVsTon_ClosestNearMass() => new()
    {
        Name = "F12_M03. Масса ×1000 (кг↔т) — непарная; Mass Near.",
        FormNum = "1.2",
        Params = DefaultForm12Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form12Transfer(1, mass: "1.5")],
        CounterpartOps = [Form12Receive(101, mass: "1500")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.Mass] = FieldMatchLevel.Near,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact,
                [TransferReceiveField.PackType] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };
}
