using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Группа F15 — форма 1.5: как 1.1 без ОКПО изготовителя; коды включают 26↔36; qty-drain при пустых сериях.
/// Общие правила (даты, нормализация, soft-match) покрыты сценариями 1.1 — здесь дельты и минимальный smoke.
/// </summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> Form15Cases()
    {
        yield return F15_V01_IdealPair_EmptyResult();
        yield return F15_A01_NoCounterpart_Unpaired();
        yield return F15_C01_Codes26_36_Paired();
        yield return F15_C02_WrongCodePair_Unpaired();
        yield return F15_Q01_EmptySerialMarkers_QtyEqual_Paired();
        yield return F15_Act01_ExponentialEqualsDecimal_Paired();
        yield return F15_Act02_ZeroActivityEqualsDash_Paired();
        yield return F15_H01_PassportMismatch_ClosestLevels();
        yield return F15_S01_StatusRao28_38_DifferentStatus_Paired();
        yield return F15_S02_StatusRaoMismatch21_31_Unpaired();
        yield return F15_Sub01_SubsidyWithin10_Paired();
        yield return F15_Sub02_SubsidyOutside10_Unpaired();
        yield return F15_Sub03_ZeroSubsidyEqualsDash_Paired();
        yield return F15_Fcp01_EmptyFcpSynonyms_Paired();
    }

    private static IEnumerable<TransferReceiveTestCase> Form15ClosestMatchCases()
    {
        yield return F15_H01_PassportMismatch_ClosestLevels();
        yield return F15_C02_WrongCodePair_Unpaired();
        yield return F15_S03_StatusRao28_38_NoStatusHighlightInClosest();
    }

    private static TransferReceiveRow Form15Transfer(
        int id,
        string opCode = "21",
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string type = "ИИИ",
        string rads = "Cs-137",
        string pack = "U-1",
        string? packName = null,
        string? packType = null,
        string? statusRao = "1",
        string? subsidy = "0",
        string? fcpNumber = "-",
        string? providerOkpo = null,
        string activity = "1.0e+6",
        string? creationDate = null,
        int? quantity = 1) =>
        new()
        {
            Id = id,
            RepsId = OurRepsId,
            OrgOkpo = DefaultOurOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = type,
            Radionuclids = rads,
            StatusRao = statusRao ?? "1",
            PackName = packName ?? "УКТ-1",
            PackType = packType ?? "Т-1",
            PackNumber = pack,
            Subsidy = subsidy ?? "0",
            FcpNumber = fcpNumber ?? "-",
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultCounterpartOkpo,
            Activity = activity,
            CreationDate = creationDate ?? DefaultOpDate,
            Quantity = quantity,
            IsTransfer = true
        };

    private static TransferReceiveRow Form15Receive(
        int id,
        string opCode = "31",
        string? opDate = null,
        string pasNum = "P-001",
        string facNum = "F-001",
        string type = "ИИИ",
        string rads = "Cs-137",
        string pack = "U-1",
        string? packName = null,
        string? packType = null,
        string? statusRao = "1",
        string? subsidy = "0",
        string? fcpNumber = "-",
        string? providerOkpo = null,
        string activity = "1.0e+6",
        string? creationDate = null,
        int? quantity = 1) =>
        new()
        {
            Id = id,
            RepsId = CounterpartRepsId,
            OrgOkpo = DefaultCounterpartOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            PasNum = pasNum,
            FacNum = facNum,
            Type = type,
            Radionuclids = rads,
            StatusRao = statusRao ?? "1",
            PackName = packName ?? "УКТ-1",
            PackType = packType ?? "Т-1",
            PackNumber = pack,
            Subsidy = subsidy ?? "0",
            FcpNumber = fcpNumber ?? "-",
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultOurOkpo,
            Activity = activity,
            CreationDate = creationDate ?? DefaultOpDate,
            Quantity = quantity,
            IsTransfer = false
        };

    /// <summary>F15_V01. Идеальная пара 21↔31 → непарных нет (smoke конвейера 1.5).</summary>
    private static TransferReceiveTestCase F15_V01_IdealPair_EmptyResult() => new()
    {
        Name = "F15_V01. Идеальная пара 21↔31 — непарных нет.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1)],
        CounterpartOps = [Form15Receive(101)],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_A01. Нет контрагента → непарная.</summary>
    private static TransferReceiveTestCase F15_A01_NoCounterpart_Unpaired() => new()
    {
        Name = "F15_A01. Нет контрагента — операция непарная.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F15_C01. Пара кодов 26↔36 → парные (дельта 1.5).</summary>
    private static TransferReceiveTestCase F15_C01_Codes26_36_Paired() => new()
    {
        Name = "F15_C01. Коды 26↔36 — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, opCode: "26")],
        CounterpartOps = [Form15Receive(101, opCode: "36")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_C02. Непарный код 21↔32 → непарная; closest: код Near.</summary>
    private static TransferReceiveTestCase F15_C02_WrongCodePair_Unpaired() => new()
    {
        Name = "F15_C02. Коды 21↔32 — непарная; OperationCode Near.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, opCode: "21")],
        CounterpartOps = [Form15Receive(101, opCode: "32")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationCode] = FieldMatchLevel.Near,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact,
                [TransferReceiveField.FactoryNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    /// <summary>
    /// F15_Q01. Заглушки «н.д.» / «нет данных» как пустые серии, qty 8↔8 → пара
    /// (общий qty-drain на DefaultForm15Params; маркеры — дельта к Q на «-»).
    /// </summary>
    private static TransferReceiveTestCase F15_Q01_EmptySerialMarkers_QtyEqual_Paired() => new()
    {
        Name = "F15_Q01. Пустые серии (н.д. / нет данных), qty 8↔8 — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, pasNum: "н.д.", facNum: "нет данных", quantity: 8)],
        CounterpartOps = [Form15Receive(101, pasNum: "н/д", facNum: "н.д.", quantity: 8)],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_Act01. Активность 1000 ↔ 1e+3 → пара (FormExponentialEquality на 1.5).</summary>
    private static TransferReceiveTestCase F15_Act01_ExponentialEqualsDecimal_Paired() => new()
    {
        Name = "F15_Act01. Активность 1000 ↔ 1e+3 — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, activity: "1000")],
        CounterpartOps = [Form15Receive(101, activity: "1e+3")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_Act02. Активность 0 ↔ «-» — пара.</summary>
    private static TransferReceiveTestCase F15_Act02_ZeroActivityEqualsDash_Paired() => new()
    {
        Name = "F15_Act02. Активность 0 ↔ «-» — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, activity: "0")],
        CounterpartOps = [Form15Receive(101, activity: "-")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_H01. Разный паспорт → closest: Passport Mismatch.</summary>
    private static TransferReceiveTestCase F15_H01_PassportMismatch_ClosestLevels() => new()
    {
        Name = "F15_H01. Closest: паспорт Mismatch, код/дата Exact.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, pasNum: "P-001")],
        CounterpartOps = [Form15Receive(101, pasNum: "P-OTHER")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationCode] = FieldMatchLevel.Exact,
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Exact,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Mismatch,
                [TransferReceiveField.FactoryNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    /// <summary>F15_S01. Коды 28↔38: разный статус РАО не мешает паре.</summary>
    private static TransferReceiveTestCase F15_S01_StatusRao28_38_DifferentStatus_Paired() => new()
    {
        Name = "F15_S01. Коды 28↔38, разный статус РАО — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, opCode: "28", statusRao: "1")],
        CounterpartOps = [Form15Receive(101, opCode: "38", statusRao: "2")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_S02. Коды 21↔31: разный статус РАО — непарная.</summary>
    private static TransferReceiveTestCase F15_S02_StatusRaoMismatch21_31_Unpaired() => new()
    {
        Name = "F15_S02. Коды 21↔31, разный статус РАО — непарная.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, statusRao: "1")],
        CounterpartOps = [Form15Receive(101, statusRao: "2")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F15_Sub01. Субсидия 50↔55 (Δ=5) — пара.</summary>
    private static TransferReceiveTestCase F15_Sub01_SubsidyWithin10_Paired() => new()
    {
        Name = "F15_Sub01. Субсидия 50↔55 — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, subsidy: "50")],
        CounterpartOps = [Form15Receive(101, subsidy: "55")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_Sub02. Субсидия 50↔70 — непарная.</summary>
    private static TransferReceiveTestCase F15_Sub02_SubsidyOutside10_Unpaired() => new()
    {
        Name = "F15_Sub02. Субсидия 50↔70 — непарная.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, subsidy: "50")],
        CounterpartOps = [Form15Receive(101, subsidy: "70")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F15_Sub03. Субсидия 0 ↔ «-» — пара.</summary>
    private static TransferReceiveTestCase F15_Sub03_ZeroSubsidyEqualsDash_Paired() => new()
    {
        Name = "F15_Sub03. Субсидия 0 ↔ «-» — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, subsidy: "0")],
        CounterpartOps = [Form15Receive(101, subsidy: "-")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_Fcp01. Номер ФЦП «без номера» ↔ «_» — пара.</summary>
    private static TransferReceiveTestCase F15_Fcp01_EmptyFcpSynonyms_Paired() => new()
    {
        Name = "F15_Fcp01. ФЦП «без номера» ↔ «_» — пара.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, fcpNumber: "без номера")],
        CounterpartOps = [Form15Receive(101, fcpNumber: "_")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F15_S03. Closest 28↔38: статус РАО не попадает в уровни подсветки.</summary>
    private static TransferReceiveTestCase F15_S03_StatusRao28_38_NoStatusHighlightInClosest() => new()
    {
        Name = "F15_S03. Closest 28↔38: StatusRao не в ExpectedClosestLevels.",
        FormNum = "1.5",
        Params = DefaultForm15Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form15Transfer(1, opCode: "28", statusRao: "1", pasNum: "P-A")],
        CounterpartOps = [Form15Receive(101, opCode: "38", statusRao: "9", pasNum: "P-B")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationCode] = FieldMatchLevel.Exact,
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };
}
