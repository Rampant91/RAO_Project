using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;

namespace Test.TransferReceive;

/// <summary>Группа V — smoke: идеальная пара, пустой результат.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> ValidCases()
    {
        yield return V01_IdealPair_EmptyResult();
    }

    /// <summary>V01. Идеальная пара 21↔31 → непарных нет.</summary>
    private static TransferReceiveTestCase V01_IdealPair_EmptyResult() => new()
    {
        Name = "V01. Идеальная пара 21↔31, непарных нет.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps = [RowReceive(101)],
        ExpectedUnpairedIds = []
    };
}
