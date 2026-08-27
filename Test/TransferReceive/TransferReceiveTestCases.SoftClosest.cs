using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>Группа H — soft closest: уровни, выбор кандидата, типовые опечатки.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> SoftClosestCases()
    {
        yield return H10_TwoCharTypoTruePair_BeatsOneCharLookalike();
        yield return H11_FactoryManufacturerPrefix_NearExact();
        yield return H12_PackNumberVariants_Near();
        yield return H13_PassportYearDigitMissing_Near();
        yield return H14_PassportSameYearDifferentCore_Mismatch();
    }

    /// <summary>
    /// H10. «Чужая» ближе по паспорту (1 символ), но другой зав.№;
    /// настоящая пара с 2 опечатками в паспорте и верным зав.№ побеждает за счёт веса идентификаторов.
    /// </summary>
    private static TransferReceiveTestCase H10_TwoCharTypoTruePair_BeatsOneCharLookalike() => new()
    {
        Name = "H10. Soft: верный зав.№ + 2 опечатки паспорта побеждают 1 опечатку «чужой».",
        OurOkpo = DefaultOurOkpo,
        OurOps =
        [
            RowTransfer(1, pasNum: "ABC12345", facNum: "F-REAL", type: "T1")
        ],
        CounterpartOps =
        [
            RowReceive(101, pasNum: "ABC12346", facNum: "F-OTHER", type: "T1"),
            RowReceive(202, pasNum: "ABC12X4Y", facNum: "F-REAL", type: "T1")
        ],
        ExpectedUnpairedIds = [1],
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 202 }
    };

    /// <summary>H11. Зав.№ с префиксом рег.№/года ↔ короткое ядро — Near.</summary>
    private static TransferReceiveTestCase H11_FactoryManufacturerPrefix_NearExact() => new()
    {
        Name = "H11. Soft: зав.№ 74041/22/0436 ↔ 0436 — Near, closest есть.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, facNum: "74041/22/0436", pasNum: "P-H11")],
        CounterpartOps = [RowReceive(101, facNum: "0436", pasNum: "P-H11")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.FactoryNumber] = FieldMatchLevel.Near,
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 },
        ExpectedConfidenceMinPercent = new Dictionary<int, int> { [1] = 80 }
    };

    /// <summary>H12. УКТ «39 (38)» ↔ «38 (39)» — Exact (тот же набор).</summary>
    private static TransferReceiveTestCase H12_PackNumberVariants_Near() => new()
    {
        Name = "H12. Soft: УКТ 39(38) ↔ 38(39) — Exact.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pack: "39 (38)", pasNum: "P-H12")],
        CounterpartOps = [RowReceive(101, pack: "38 (39)", pasNum: "P-H12")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.PackNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>H13. Паспорт «3197/25» ↔ «3197/2» — Near.</summary>
    private static TransferReceiveTestCase H13_PassportYearDigitMissing_Near() => new()
    {
        Name = "H13. Soft: паспорт 3197/25 ↔ 3197/2 — Near.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "3197/25", facNum: "F-H13")],
        CounterpartOps = [RowReceive(101, pasNum: "3197/2", facNum: "F-H13")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Near
            }
        }
    };

    /// <summary>H14. Общий суффикс года при разных ядрах — Passport Mismatch (не Near).</summary>
    private static TransferReceiveTestCase H14_PassportSameYearDifferentCore_Mismatch() => new()
    {
        Name = "H14. Soft: паспорт 829/22 ↔ 721/22 — Mismatch ядра.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "829/22", facNum: "F-H14")],
        CounterpartOps = [RowReceive(101, pasNum: "721/22", facNum: "F-H14")],
        ExpectedUnpairedIds = [1],
        ExpectedClosestLevels = new Dictionary<int, IReadOnlyDictionary<TransferReceiveField, FieldMatchLevel>>
        {
            [1] = new Dictionary<TransferReceiveField, FieldMatchLevel>
            {
                [TransferReceiveField.PassportNumber] = FieldMatchLevel.Mismatch,
                [TransferReceiveField.FactoryNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidateIds = new Dictionary<int, int> { [1] = 101 }
    };
}
