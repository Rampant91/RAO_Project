using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;

namespace Test.TransferReceive;

/// <summary>Группа A — попадание / непопадание строки в отчёт непарных.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> PairingCases()
    {
        yield return A01_NoCounterpart_InReport();
        yield return A02_PassportMismatch_InReport();
        yield return A03_ExtraCounterpartReceive_IdealPairStillEmpty();
        yield return A04_SelfPair_NotInReport();
    }

    /// <summary>A01. Нет операций контрагента в пуле → наша передача в отчёте.</summary>
    private static TransferReceiveTestCase A01_NoCounterpart_InReport() => new()
    {
        Name = "A01. Нет контрагента в пуле — передача в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>A02. Контрагент есть, но другой паспорт → наша передача в отчёте.</summary>
    private static TransferReceiveTestCase A02_PassportMismatch_InReport() => new()
    {
        Name = "A02. Другой паспорт у контрагента — передача в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "P-001")],
        CounterpartOps = [RowReceive(101, pasNum: "P-OTHER")],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>A03. Лишний receive у контрагента не мешает идеальной паре.</summary>
    private static TransferReceiveTestCase A03_ExtraCounterpartReceive_IdealPairStillEmpty() => new()
    {
        Name = "A03. Лишний receive у контрагента — идеальная пара всё равно сходится.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps =
        [
            RowReceive(101),
            RowReceive(202, pasNum: "P-EXTRA", facNum: "F-EXTRA")
        ],
        ExpectedUnpairedIds = []
    };

    /// <summary>A04. Self-pair внутри одной OrgOkpo (передача себе) → не в отчёте.</summary>
    private static TransferReceiveTestCase A04_SelfPair_NotInReport() => new()
    {
        Name = "A04. Self-pair внутри org — не в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps =
        [
            RowTransfer(1, providerOkpo: DefaultOurOkpo, orgOkpo: DefaultOurOkpo, repsId: OurRepsId),
            RowReceive(2, providerOkpo: DefaultOurOkpo, orgOkpo: DefaultOurOkpo, repsId: OurRepsId)
        ],
        CounterpartOps = [],
        ExpectedUnpairedIds = []
    };
}
