using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Группа F16 — форма 1.6: код РАО, статус, объём/масса (т), 4 активности;
/// построчное сопоставление без qty-drain.
/// </summary>
internal static partial class TransferReceiveTestCases
{
    private const string DefaultCodeRao = "12345678901";

    private static IEnumerable<TransferReceiveTestCase> Form16Cases()
    {
        yield return F16_V01_IdealPair_EmptyResult();
        yield return F16_A01_NoCounterpart_Unpaired();
        yield return F16_C01_Codes26_36_Paired();
        yield return F16_C02_WrongCodePair_Unpaired();
        yield return F16_Act01_TritiumExponentialEqualsDecimal_Paired();
        yield return F16_Act02_BetaGammaOutsideTolerance_Unpaired();
        yield return F16_Act03_ZeroTritiumEqualsDash_Paired();
        yield return F16_Vol01_VolumeWithinTolerance_Paired();
        yield return F16_Mass01_MassWithinTolerance_Paired();
        yield return F16_Q01_DifferentQuantity_Unpaired();
        yield return F16_Q02_AntiQtyDrain_TwoRowsVsOneEight_Unpaired();
        yield return F16_Code01_DifferentCodeRao_Unpaired();
        yield return F16_St01_StatusMismatch21_31_Unpaired();
        yield return F16_St02_Status28_38_DifferentStatus_Paired();
        yield return F16_Amd01_ActivityMeasurementDateNear_Unpaired();
        yield return F16_Sub01_SubsidyWithin10_Paired();
        yield return F16_Sub02_SubsidyOutside10_Unpaired();
        yield return F16_Sub03_ZeroSubsidyEqualsDash_Paired();
        yield return F16_Fcp01_EmptyFcpSynonyms_Paired();
    }

    private static IEnumerable<TransferReceiveTestCase> Form16ClosestMatchCases()
    {
        yield return F16_C02_WrongCodePair_Unpaired();
        yield return F16_H01_RadionuclidsMismatch_ClosestLevels();
        yield return F16_Code01_DifferentCodeRao_Unpaired();
        yield return F16_Pack01_PackNumberSoftNear();
        yield return F16_S03_StatusRao28_38_NoStatusHighlightInClosest();
    }

    private static TransferReceiveRow Form16Transfer(
        int id,
        string opCode = "21",
        string? opDate = null,
        string? codeRao = null,
        string? statusRao = "1",
        string? volume = "1.0",
        string? mass = "2.0",
        string rads = "Cs-137",
        string? packType = "T-1",
        string pack = "U-1",
        string? subsidy = "0",
        string? fcpNumber = "-",
        string? providerOkpo = null,
        string tritium = "1000",
        string beta = "2000",
        string alpha = "3000",
        string trans = "4000",
        string? activityMeasurementDate = null,
        int? quantity = 1) =>
        new()
        {
            Id = id,
            RepsId = OurRepsId,
            OrgOkpo = DefaultOurOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            CodeRao = codeRao ?? DefaultCodeRao,
            StatusRao = statusRao ?? "1",
            Volume = volume ?? "1.0",
            Mass = mass ?? "2.0",
            Radionuclids = rads,
            PackType = packType ?? "T-1",
            PackNumber = pack,
            Subsidy = subsidy ?? "0",
            FcpNumber = fcpNumber ?? "-",
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultCounterpartOkpo,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = trans,
            ActivityMeasurementDate = activityMeasurementDate ?? DefaultOpDate,
            Quantity = quantity,
            IsTransfer = true
        };

    private static TransferReceiveRow Form16Receive(
        int id,
        string opCode = "31",
        string? opDate = null,
        string? codeRao = null,
        string? statusRao = "1",
        string? volume = "1.0",
        string? mass = "2.0",
        string rads = "Cs-137",
        string? packType = "T-1",
        string pack = "U-1",
        string? subsidy = "0",
        string? fcpNumber = "-",
        string? providerOkpo = null,
        string tritium = "1000",
        string beta = "2000",
        string alpha = "3000",
        string trans = "4000",
        string? activityMeasurementDate = null,
        int? quantity = 1) =>
        new()
        {
            Id = id,
            RepsId = CounterpartRepsId,
            OrgOkpo = DefaultCounterpartOkpo,
            OpCode = opCode,
            OpDate = opDate ?? DefaultOpDate,
            CodeRao = codeRao ?? DefaultCodeRao,
            StatusRao = statusRao ?? "1",
            Volume = volume ?? "1.0",
            Mass = mass ?? "2.0",
            Radionuclids = rads,
            PackType = packType ?? "T-1",
            PackNumber = pack,
            Subsidy = subsidy ?? "0",
            FcpNumber = fcpNumber ?? "-",
            ProviderOrRecieverOkpo = providerOkpo ?? DefaultOurOkpo,
            TritiumActivity = tritium,
            BetaGammaActivity = beta,
            AlphaActivity = alpha,
            TransuraniumActivity = trans,
            ActivityMeasurementDate = activityMeasurementDate ?? DefaultOpDate,
            Quantity = quantity,
            IsTransfer = false
        };

    private static TransferReceiveTestCase F16_V01_IdealPair_EmptyResult() => new()
    {
        Name = "F16_V01. Идеальная пара 21↔31 — непарных нет.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1)],
        CounterpartOps = [Form16Receive(101)],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_A01_NoCounterpart_Unpaired() => new()
    {
        Name = "F16_A01. Нет контрагента — операция непарная.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F16_C01_Codes26_36_Paired() => new()
    {
        Name = "F16_C01. Коды 26↔36 — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, opCode: "26")],
        CounterpartOps = [Form16Receive(101, opCode: "36")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_C02_WrongCodePair_Unpaired() => new()
    {
        Name = "F16_C02. Коды 21↔32 — непарная; OperationCode Near.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, opCode: "21")],
        CounterpartOps = [Form16Receive(101, opCode: "32")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationCode] = FieldMatchLevel.Near,
                [TransferReceiveField.CodeRao] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    private static TransferReceiveTestCase F16_Act01_TritiumExponentialEqualsDecimal_Paired() => new()
    {
        Name = "F16_Act01. Тритий 1000 ↔ 1e+3 — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, tritium: "1000")],
        CounterpartOps = [Form16Receive(101, tritium: "1e+3")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Act02_BetaGammaOutsideTolerance_Unpaired() => new()
    {
        Name = "F16_Act02. Бета-гамма вне ±10% — непарная.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, beta: "2000")],
        CounterpartOps = [Form16Receive(101, beta: "5000")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F16_Act03. Тритий 0 ↔ «-» — пара.</summary>
    private static TransferReceiveTestCase F16_Act03_ZeroTritiumEqualsDash_Paired() => new()
    {
        Name = "F16_Act03. Тритий 0 ↔ «-» — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, tritium: "0")],
        CounterpartOps = [Form16Receive(101, tritium: "-")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Vol01_VolumeWithinTolerance_Paired() => new()
    {
        Name = "F16_Vol01. Объём в ±10% — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, volume: "10")],
        CounterpartOps = [Form16Receive(101, volume: "10.5")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Mass01_MassWithinTolerance_Paired() => new()
    {
        Name = "F16_Mass01. Масса в ±10% — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, mass: "2.0")],
        CounterpartOps = [Form16Receive(101, mass: "2.1")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Q01_DifferentQuantity_Unpaired() => new()
    {
        Name = "F16_Q01. Разное количество — непарная.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, quantity: 4)],
        CounterpartOps = [Form16Receive(101, quantity: 8)],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F16_Q02_AntiQtyDrain_TwoRowsVsOneEight_Unpaired() => new()
    {
        Name = "F16_Q02. Две строки 4+4 против одной 8 — не склеиваются (анти-drain).",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps =
        [
            Form16Transfer(1, quantity: 4),
            Form16Transfer(2, quantity: 4)
        ],
        CounterpartOps = [Form16Receive(101, quantity: 8)],
        ExpectedUnpairedIds = [1, 2]
    };

    private static TransferReceiveTestCase F16_Code01_DifferentCodeRao_Unpaired() => new()
    {
        Name = "F16_Code01. Разный код РАО — непарная; CodeRao Mismatch.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, codeRao: "11111111111")],
        CounterpartOps = [Form16Receive(101, codeRao: "22222222222")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.CodeRao] = FieldMatchLevel.Mismatch
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    private static TransferReceiveTestCase F16_St01_StatusMismatch21_31_Unpaired() => new()
    {
        Name = "F16_St01. Разный статус при 21↔31 — непарная.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, statusRao: "1")],
        CounterpartOps = [Form16Receive(101, statusRao: "2")],
        ExpectedUnpairedIds = [1]
    };

    private static TransferReceiveTestCase F16_St02_Status28_38_DifferentStatus_Paired() => new()
    {
        Name = "F16_St02. Коды 28↔38, разный статус — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, opCode: "28", statusRao: "1")],
        CounterpartOps = [Form16Receive(101, opCode: "38", statusRao: "2")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Amd01_ActivityMeasurementDateNear_Unpaired() => new()
    {
        Name = "F16_Amd01. Дата изм. активности +10 дней — непарная; closest Near.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, activityMeasurementDate: "2024-06-15")],
        CounterpartOps = [Form16Receive(101, activityMeasurementDate: "2024-06-25")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.ActivityMeasurementDate] = FieldMatchLevel.Near
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    private static TransferReceiveTestCase F16_Sub01_SubsidyWithin10_Paired() => new()
    {
        Name = "F16_Sub01. Субсидия |Δ|≤10 п.п. — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, subsidy: "50")],
        CounterpartOps = [Form16Receive(101, subsidy: "55")],
        ExpectedUnpairedIds = []
    };

    /// <summary>F16_Sub02. Субсидия 50↔70 — непарная.</summary>
    private static TransferReceiveTestCase F16_Sub02_SubsidyOutside10_Unpaired() => new()
    {
        Name = "F16_Sub02. Субсидия 50↔70 — непарная.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, subsidy: "50")],
        CounterpartOps = [Form16Receive(101, subsidy: "70")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>F16_Sub03. Субсидия 0 ↔ «-» — пара.</summary>
    private static TransferReceiveTestCase F16_Sub03_ZeroSubsidyEqualsDash_Paired() => new()
    {
        Name = "F16_Sub03. Субсидия 0 ↔ «-» — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, subsidy: "0")],
        CounterpartOps = [Form16Receive(101, subsidy: "-")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_Fcp01_EmptyFcpSynonyms_Paired() => new()
    {
        Name = "F16_Fcp01. ФЦП «без номера» ↔ «_» — пара.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, fcpNumber: "без номера")],
        CounterpartOps = [Form16Receive(101, fcpNumber: "_")],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase F16_H01_RadionuclidsMismatch_ClosestLevels() => new()
    {
        Name = "F16_H01. Closest: радионуклиды Mismatch, код/дата Exact.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, rads: "Cs-137")],
        CounterpartOps = [Form16Receive(101, rads: "Sr-90")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationCode] = FieldMatchLevel.Exact,
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Exact,
                [TransferReceiveField.Radionuclids] = FieldMatchLevel.Mismatch
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };

    private static TransferReceiveTestCase F16_Pack01_PackNumberSoftNear() => new()
    {
        Name = "F16_Pack01. Номер УКТ soft-Near — кандидат выбран.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        // Перестановка соседних символов — Near в closest, но ключ пары разный (не lookalike-Exact).
        OurOps = [Form16Transfer(1, pack: "4510")],
        CounterpartOps = [Form16Receive(101, pack: "4501")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.PackNumber] = FieldMatchLevel.Near
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 },
        ExpectedConfidenceMinPercent = new Dictionary<int, int> { [1] = 50 }
    };

    /// <summary>F16_S03. Closest 28↔38: статус РАО не попадает в уровни подсветки.</summary>
    private static TransferReceiveTestCase F16_S03_StatusRao28_38_NoStatusHighlightInClosest() => new()
    {
        Name = "F16_S03. Closest 28↔38: StatusRao не в ExpectedClosestLevels.",
        FormNum = "1.6",
        Params = DefaultForm16Params(),
        OurOkpo = DefaultOurOkpo,
        OurOps = [Form16Transfer(1, opCode: "28", statusRao: "1", pack: "U-A")],
        CounterpartOps = [Form16Receive(101, opCode: "38", statusRao: "9", pack: "U-B")],
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
