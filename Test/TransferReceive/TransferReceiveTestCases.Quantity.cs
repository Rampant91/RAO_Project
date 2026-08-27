using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа Q — пустые серии / суммирование количества.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> QuantityCases()
    {
        yield return Q01_EmptySerial_QtyEqual_Paired();
        yield return Q02_EmptySerial_QtyPartial_InReport();
        yield return Q03_EmptySerial_QtyDrainAcrossTwoCandidates();
        yield return Q04_CheckQuantityOff_PartialQty_Paired();
        yield return Q05_EmptySerial_DifferentPack_Unpaired();
    }

    private static TransferReceiveRow EmptySerialTransfer(
        int id, int qty, string? opDate = null) =>
        RowTransfer(id, pasNum: "-", facNum: "-", quantity: qty, opDate: opDate);

    private static TransferReceiveRow EmptySerialReceive(
        int id, int qty, string? opDate = null) =>
        RowReceive(id, pasNum: "-", facNum: "-", quantity: qty, opDate: opDate);

    /// <summary>Q01. Пустые паспорт+зав.№, qty 8↔8 → пара.</summary>
    private static TransferReceiveTestCase Q01_EmptySerial_QtyEqual_Paired() => new()
    {
        Name = "Q01. Пустые серии, qty 8↔8 — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [EmptySerialTransfer(1, 8)],
        CounterpartOps = [EmptySerialReceive(101, 8)],
        ExpectedUnpairedIds = []
    };

    /// <summary>Q02. qty 8↔5 → source unpaired.</summary>
    private static TransferReceiveTestCase Q02_EmptySerial_QtyPartial_InReport() => new()
    {
        Name = "Q02. Пустые серии, qty 8↔5 — остаток в отчёте.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [EmptySerialTransfer(1, 8)],
        CounterpartOps = [EmptySerialReceive(101, 5)],
        ExpectedUnpairedIds = [1]
    };

    /// <summary>Q03. Source 10, candidates 6+4 → пара (drain).</summary>
    private static TransferReceiveTestCase Q03_EmptySerial_QtyDrainAcrossTwoCandidates() => new()
    {
        Name = "Q03. Пустые серии, qty 10 закрывается 6+4 — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [EmptySerialTransfer(1, 10)],
        CounterpartOps =
        [
            EmptySerialReceive(101, 6),
            EmptySerialReceive(202, 4)
        ],
        ExpectedUnpairedIds = []
    };

    /// <summary>Q04. CheckQuantity=false при 8↔5 → пара.</summary>
    private static TransferReceiveTestCase Q04_CheckQuantityOff_PartialQty_Paired() => new()
    {
        Name = "Q04. CheckQuantity=false при qty 8↔5 — пара.",
        OurOkpo = DefaultOurOkpo,
        Params = new TransferReceiveFormParams(CheckQuantity: false),
        OurOps = [EmptySerialTransfer(1, 8)],
        CounterpartOps = [EmptySerialReceive(101, 5)],
        ExpectedUnpairedIds = []
    };

    /// <summary>
    /// Q05. Пустые серии, одинаковый qty, разный номер упаковки — не пара
    /// (УКТ входит в ключ при CheckPackNumber).
    /// </summary>
    private static TransferReceiveTestCase Q05_EmptySerial_DifferentPack_Unpaired() => new()
    {
        Name = "Q05. Пустые серии, разный УКТ — непарные.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "-", facNum: "-", quantity: 8, pack: "УКТ-A")],
        CounterpartOps = [RowReceive(101, pasNum: "-", facNum: "-", quantity: 8, pack: "УКТ-B")],
        ExpectedUnpairedIds = [1]
    };
}
