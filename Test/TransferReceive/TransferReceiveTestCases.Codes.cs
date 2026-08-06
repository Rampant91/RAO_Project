using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;

namespace Test.TransferReceive;

/// <summary>Группа C — таблица пар кодов передачи ↔ приёма.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> CodeCases()
    {
        yield return C01_Pair_21_31();
        yield return C02_Pair_22_32();
        yield return C03_Pair_25_37();
        yield return C04_Pair_27_35();
        yield return C05_Pair_28_38();
        yield return C06_Pair_29_39();
        yield return C07_WrongPair_21_32_InReport();
    }

    private static TransferReceiveTestCase IdealCodePair(string name, string transferCode, string receiveCode) => new()
    {
        Name = name,
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opCode: transferCode)],
        CounterpartOps = [RowReceive(101, opCode: receiveCode)],
        ExpectedUnpairedIds = []
    };

    private static TransferReceiveTestCase C01_Pair_21_31() =>
        IdealCodePair("C01. Пара кодов 21↔31.", "21", "31");

    private static TransferReceiveTestCase C02_Pair_22_32() =>
        IdealCodePair("C02. Пара кодов 22↔32.", "22", "32");

    private static TransferReceiveTestCase C03_Pair_25_37() =>
        IdealCodePair("C03. Пара кодов 25↔37.", "25", "37");

    private static TransferReceiveTestCase C04_Pair_27_35() =>
        IdealCodePair("C04. Пара кодов 27↔35.", "27", "35");

    private static TransferReceiveTestCase C05_Pair_28_38() =>
        IdealCodePair("C05. Пара кодов 28↔38.", "28", "38");

    private static TransferReceiveTestCase C06_Pair_29_39() =>
        IdealCodePair("C06. Пара кодов 29↔39.", "29", "39");

    /// <summary>C07. 21 рядом с 32 — непарный код → в отчёте.</summary>
    private static TransferReceiveTestCase C07_WrongPair_21_32_InReport() => new()
    {
        Name = "C07. Коды 21 и 32 не парные — передача в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opCode: "21")],
        CounterpartOps = [RowReceive(101, opCode: "32")],
        ExpectedUnpairedIds = [1]
    };
}
