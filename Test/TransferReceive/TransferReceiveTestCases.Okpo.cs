using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing.TransferReceiveScenarioRunner;

namespace Test.TransferReceive;

/// <summary>Группа O — ОКПО кол. 19, self-pair, отсутствие контрагента.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> OkpoCases()
    {
        yield return O01_CandidateNotPointingToUs_InReport();
        yield return O02_OkpoAlias_Paired();
        yield return O03_EmptyProviderOkpo_InReport_NoClosest();
        yield return O04_EightDigitPrefixOfExtended_Paired();
        yield return O05_ExtendedVsEightDigitProvider_Paired();
    }

    /// <summary>O01. Candidate.ProviderOkpo не указывает на нас → unpaired.</summary>
    private static TransferReceiveTestCase O01_CandidateNotPointingToUs_InReport() => new()
    {
        Name = "O01. Контрагент в кол.19 указывает не на нас — в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps = [RowReceive(101, providerOkpo: "99999999")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>
    /// O02. Титульный ОКПО ≠ ОКПО из кол.19; пул находится только через OkpoAliases.
    /// </summary>
    private static TransferReceiveTestCase O02_OkpoAlias_Paired()
    {
        const string searchOkpo = "20000002";
        const string titleOkpo = "BRANCH99";
        return new TransferReceiveTestCase
        {
            Name = "O02. ОКПО-алиас из кол.19 — пара через OkpoAliases.",
            OurOkpo = DefaultOurOkpo,
            OurOps = [RowTransfer(1, providerOkpo: searchOkpo)],
            CounterpartOps =
            [
                RowReceive(101, orgOkpo: titleOkpo, providerOkpo: DefaultOurOkpo)
            ],
            OkpoAliases = new Dictionary<string, IReadOnlyList<int>>
            {
                [NormalizeNumber(searchOkpo)] = [CounterpartRepsId]
            },
            ExpectedUnpairedIds = []
        };
    }

    /// <summary>O03. Пустой/«-» в кол.19 → unpaired, closest нет.</summary>
    private static TransferReceiveTestCase O03_EmptyProviderOkpo_InReport_NoClosest() => new()
    {
        Name = "O03. Пустой ОКПО в кол.19 — в отчёте; closest нет.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, providerOkpo: "-")],
        CounterpartOps = [RowReceive(101)],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>()
    };

    /// <summary>
    /// O04. Наш ОКПО формата 8_5; контрагент в кол.19 указал только первые 8 цифр → пара.
    /// </summary>
    private static TransferReceiveTestCase O04_EightDigitPrefixOfExtended_Paired()
    {
        const string ourExtended = "08624243_40044";
        const string shortHead = "08624243";
        return new TransferReceiveTestCase
        {
            Name = "O04. Кол.19 = первые 8 цифр нашего 8_5 — пара.",
            OurOkpo = ourExtended,
            OurOps = [RowTransfer(1, orgOkpo: ourExtended, providerOkpo: DefaultCounterpartOkpo)],
            CounterpartOps =
            [
                RowReceive(101, providerOkpo: shortHead)
            ],
            ExpectedUnpairedIds = []
        };
    }

    /// <summary>
    /// O05. Мы указали полный 8_5 контрагента; у него титул — 8 цифр (голова) → пара через индекс.
    /// </summary>
    private static TransferReceiveTestCase O05_ExtendedVsEightDigitProvider_Paired()
    {
        const string counterpartExtended = "20000002_12345";
        const string counterpartHead = "20000002";
        return new TransferReceiveTestCase
        {
            Name = "O05. Мы пишем 8_5 контрагента, титул у него — 8 цифр — пара.",
            OurOkpo = DefaultOurOkpo,
            OurOps = [RowTransfer(1, providerOkpo: counterpartExtended)],
            CounterpartOps =
            [
                RowReceive(101, orgOkpo: counterpartHead, providerOkpo: DefaultOurOkpo)
            ],
            ExpectedUnpairedIds = []
        };
    }
}
