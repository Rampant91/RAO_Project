using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа H — карты closest-match для непарных.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> ClosestMatchCases()
    {
        yield return H01_PartialFieldMap();
        yield return D02_OneDayDiff_InReport_ClosestRedDate();
        yield return D03_OutsideSearchWindow_InReport_NoClosest();
        yield return H03_NoCandidates_EmptyClosest();
        yield return H04_EmptySerial_QuantityHighlight_DifferentQty();
        yield return H05_EmptySerial_QuantityHighlight_EqualQty_PackMismatch();
        yield return O03_EmptyProviderOkpo_InReport_NoClosest();
    }

    /// <summary>H01. Частичное совпадение: паспорт другой, остальное как у пары.</summary>
    private static TransferReceiveTestCase H01_PartialFieldMap() => new()
    {
        Name = "H01. Closest: паспорт красный, код/дата/тип зелёные.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "P-001")],
        CounterpartOps = [RowReceive(101, pasNum: "P-OTHER")],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>
        {
            [1] = new Dictionary<TransferReceiveField, bool>
            {
                [TransferReceiveField.OperationCode] = true,
                [TransferReceiveField.OperationDate] = true,
                [TransferReceiveField.PassportNumber] = false,
                [TransferReceiveField.Type] = true,
                [TransferReceiveField.FactoryNumber] = true
            }
        }
    };

    /// <summary>H03. Нет кандидатов противоположной стороны → пустая closest-карта.</summary>
    private static TransferReceiveTestCase H03_NoCandidates_EmptyClosest() => new()
    {
        Name = "H03. Нет кандидатов — closest пуст.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1)],
        CounterpartOps = [],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>()
    };

    /// <summary>
    /// H04. Пустые серии, qty 8↔5: в closest Quantity сравнивается построчно → красный.
    /// </summary>
    private static TransferReceiveTestCase H04_EmptySerial_QuantityHighlight_DifferentQty() => new()
    {
        Name = "H04. Пустые серии qty 8↔5 — Quantity в closest красный (построчно).",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "-", facNum: "-", quantity: 8)],
        CounterpartOps = [RowReceive(101, pasNum: "-", facNum: "-", quantity: 5)],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>
        {
            [1] = new Dictionary<TransferReceiveField, bool>
            {
                [TransferReceiveField.Quantity] = false,
                [TransferReceiveField.OperationCode] = true,
                [TransferReceiveField.OperationDate] = true
            }
        }
    };

    /// <summary>
    /// H05. Пустые серии, одинаковый qty, разный УКТ: непарные; Quantity зелёный, PackNumber красный.
    /// </summary>
    private static TransferReceiveTestCase H05_EmptySerial_QuantityHighlight_EqualQty_PackMismatch() => new()
    {
        Name = "H05. Пустые серии qty 1↔1, разный УКТ — Quantity зелёный, PackNumber красный.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "-", facNum: "-", quantity: 1, pack: "УКТ-A")],
        CounterpartOps = [RowReceive(101, pasNum: "-", facNum: "-", quantity: 1, pack: "УКТ-B")],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>
        {
            [1] = new Dictionary<TransferReceiveField, bool>
            {
                [TransferReceiveField.Quantity] = true,
                [TransferReceiveField.PackNumber] = false,
                [TransferReceiveField.OperationCode] = true,
                [TransferReceiveField.OperationDate] = true
            }
        }
    };
}
