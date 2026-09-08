using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа D — точная дата для пары; ±15 дней только окно поиска closest.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> DateCases()
    {
        yield return D01_SameDate_Paired();
        yield return D02_OneDayDiff_InReport_ClosestNearDate();
        yield return D03_OutsideSearchWindow_InReport_NoClosest();
        yield return D04_CustomTolerance_FindsClosestAt20Days();
    }

    /// <summary>D01. Даты равны → пара.</summary>
    private static TransferReceiveTestCase D01_SameDate_Paired() => new()
    {
        Name = "D01. Одинаковая дата операции — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opDate: DefaultOpDate)],
        CounterpartOps = [RowReceive(101, opDate: DefaultOpDate)],
        ExpectedUnpairedIds = []
    };

    /// <summary>
    /// D02. Отличие 1 день (внутри ±15) → не пара, в отчёте;
    /// closest есть, дата Near (жёлтая); в bool-карте Exact=false.
    /// </summary>
    private static TransferReceiveTestCase D02_OneDayDiff_InReport_ClosestNearDate() => new()
    {
        Name = "D02. Дата +1 день — в отчёте; closest с жёлтой датой (Near, не Exact).",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opDate: "2024-06-15")],
        CounterpartOps = [RowReceive(101, opDate: "2024-06-16")],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>
        {
            [1] = new Dictionary<TransferReceiveField, bool>
            {
                [TransferReceiveField.OperationCode] = true,
                [TransferReceiveField.OperationDate] = false,
                [TransferReceiveField.PassportNumber] = true,
                [TransferReceiveField.Type] = true,
                [TransferReceiveField.Radionuclids] = true,
                [TransferReceiveField.FactoryNumber] = true,
                [TransferReceiveField.Quantity] = true,
                [TransferReceiveField.Activity] = true,
                [TransferReceiveField.CreatorOkpo] = true,
                [TransferReceiveField.CreationDate] = true,
                [TransferReceiveField.ProviderOrRecieverOkpo] = true,
                [TransferReceiveField.PackNumber] = true
            }
        },
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Near
            }
        }
    };

    /// <summary>D03. Отличие 20 дней (>15) → в отчёте; closest нет.</summary>
    private static TransferReceiveTestCase D03_OutsideSearchWindow_InReport_NoClosest() => new()
    {
        Name = "D03. Дата +20 дней — в отчёте; closest нет (вне окна поиска).",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opDate: "2024-06-15")],
        CounterpartOps = [RowReceive(101, opDate: "2024-07-05")],
        ExpectedUnpairedIds = [1],
        ExpectedClosest = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>()
    };

    /// <summary>
    /// D04. При окне ±30 дней находится closest на +20 дней (при ±15 — нет, см. D03).
    /// </summary>
    private static TransferReceiveTestCase D04_CustomTolerance_FindsClosestAt20Days() => new()
    {
        Name = "D04. Окно поиска ±30 дней — closest на +20 дней.",
        OperationDateSearchToleranceDays = 30,
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, opDate: "2024-06-15")],
        CounterpartOps = [RowReceive(101, opDate: "2024-07-05")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 },
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.OperationDate] = FieldMatchLevel.Near
            }
        }
    };
}
