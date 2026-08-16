using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа S — soft closest: уровни Near, выбор кандидата, tie-break 1.6.</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> SoftClosestCases()
    {
        yield return S01_Form16_TieBreak_PrefersForm12_WhenScoresEqual();
        yield return S02_OperationCode_41vs14_IsNear();
        yield return S03_TwoCharTypoTruePair_BeatsOneCharLookalike();
        yield return S04_FactoryManufacturerPrefix_NearExact();
        yield return S05_PackNumberVariants_Near();
        yield return S06_PassportYearDigitMissing_Near();
        yield return S07_PassportSameYearDifferentCore_Mismatch();
        yield return S08_Form12_OneDayDiff_ClosestNearDate();
        yield return S09_Form12_MassNear_OutsidePairingTolerance();
        yield return S10_Form13_OneDayDiff_ClosestNearDate();
        yield return S11_Form13_MainRadsTypo_IsNear();
        yield return S12_Form14_OneDayDiff_ClosestNearDate();
        yield return S13_Form14_MassNear_OutsidePairingTolerance();
        yield return S14_Form14_VolumeNear_IsNear();
    }

    /// <summary>
    /// S01. При равном weighted-score у 1.2 и 1.3 побеждает профиль Form12 (приоритет 12→13→14).
    /// Form12: −3 (масса 1↔1.12); Form13: −10 (нуклиды + даты + документ) → оба 34.5.
    /// </summary>
    private static Pairing41TestCase S01_Form16_TieBreak_PrefersForm12_WhenScoresEqual()
    {
        const string opDate = "2024-09-01";
        const string doc = "DOC-TIE";
        const string pack = "УКТ-TIE";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "S01. Closest 1.6: при равном weighted-score — профиль Form12.",
            Form12 =
            [
                Row12(2, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    massTon: "1", beta: beta, alpha: "-", activityMeasurementDate: opDate)
            ],
            Form13 =
            [
                new Pairing41Row
                {
                    Id = 3,
                    OpDate = opDate,
                    Type = "Тип-ОРИ",
                    MainRadionuclids = "йод-131",
                    Radionuclids = "йод-131",
                    TritiumActivity = "-",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TransuraniumActivity = "-",
                    ActivityMeasurementDate = opDate,
                    CreationDate = opDate,
                    DocumentVid = 1,
                    DocumentNumber = "OTHER",
                    DocumentDate = "2020-01-01",
                    PackName = "Другая упаковка",
                    PackType = "ТипУКТ",
                    PackNumber = pack
                }
            ],
            Form16 =
            [
                new Pairing41Row
                {
                    Id = 900,
                    OpDate = opDate,
                    Mass = "1.12",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TritiumActivity = "-",
                    TransuraniumActivity = "-",
                    MainRadionuclids = "кобальт-60",
                    ActivityMeasurementDate = opDate,
                    DocumentVid = 1,
                    DocumentNumber = doc,
                    DocumentDate = opDate,
                    PackName = "Упаковка",
                    PackType = "ТипУКТ",
                    PackNumber = pack
                }
            ],
            ExpectedUnpaired12 = [2],
            ExpectedUnpaired13 = [3],
            ExpectedUnpaired16 = [900],
            ExpectedClosest16 = new Dictionary<int, Pairing41Form16ClosestExpectation>
            {
                [900] = new()
                {
                    Profile = Form16MatchProfile.Form12,
                    Levels12 = new Dictionary<Pairing12To16Field, FieldMatchLevel>
                    {
                        [Pairing12To16Field.Mass] = FieldMatchLevel.Near,
                        [Pairing12To16Field.DocumentNumber] = FieldMatchLevel.Exact
                    }
                }
            },
            ExpectedClosestCandidate16 = new Dictionary<int, int> { [900] = 2 },
            ExpectedConfidenceMinPercent16 = new Dictionary<int, int> { [900] = 90 }
        };
    }

    /// <summary>S02. Код операции 41 ↔ 14 (опечатка приёма на 1.5) — Near; 1.1 непарная, 14 не в непарных 1.5.</summary>
    private static Pairing41TestCase S02_OperationCode_41vs14_IsNear() => new()
    {
        Name = "S02. Soft 1.1↔1.5: код 41 ↔ 14 — Near.",
        Form11 = [Row11(1, opCode: "41")],
        Form15 = [Row11(101, opCode: "14")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [],
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.OperationCode] = FieldMatchLevel.Near,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 },
        ExpectedConfidenceMinPercent11 = new Dictionary<int, int> { [1] = 95 }
    };

    /// <summary>
    /// S03. «Чужая» ближе по паспорту (1 символ), но другой зав.№;
    /// настоящая пара с 2 опечатками в паспорте и верным зав.№ побеждает за счёт веса идентификаторов.
    /// </summary>
    private static Pairing41TestCase S03_TwoCharTypoTruePair_BeatsOneCharLookalike() => new()
    {
        Name = "S03. Soft 1.1↔1.5: верный зав.№ + 2 опечатки паспорта побеждают 1 опечатку «чужой».",
        Form11 = [Row11(1, pasNum: "ABC12345", facNum: "F-REAL", type: "T1")],
        Form15 =
        [
            Row11(101, pasNum: "ABC12346", facNum: "F-OTHER", type: "T1"),
            Row11(202, pasNum: "ABC12X4Y", facNum: "F-REAL", type: "T1")
        ],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101, 202],
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 202 }
    };

    /// <summary>S04. Зав.№ с префиксом рег.№/года ↔ короткое ядро — Near (1.1↔1.5).</summary>
    private static Pairing41TestCase S04_FactoryManufacturerPrefix_NearExact() => new()
    {
        Name = "S04. Soft 1.1↔1.5: зав.№ 74041/22/0436 ↔ 0436 — Near.",
        Form11 = [Row11(1, facNum: "74041/22/0436", pasNum: "P-S04")],
        Form15 = [Row11(101, facNum: "0436", pasNum: "P-S04")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.FactoryNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 },
        ExpectedConfidenceMinPercent11 = new Dictionary<int, int> { [1] = 80 }
    };

    /// <summary>S05. УКТ «39 (38)» ↔ «38 (39)» — Exact (тот же набор номеров).</summary>
    private static Pairing41TestCase S05_PackNumberVariants_Near() => new()
    {
        Name = "S05. Soft 1.1↔1.5: УКТ 39(38) ↔ 38(39) — Exact.",
        Form11 = [Row11(1, packNumber: "39 (38)", pasNum: "P-S05")],
        Form15 = [Row11(101, packNumber: "38 (39)", pasNum: "P-S05")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PackNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>S06. Паспорт «3197/25» ↔ «3197/2» — Near (1.1↔1.5).</summary>
    private static Pairing41TestCase S06_PassportYearDigitMissing_Near() => new()
    {
        Name = "S06. Soft 1.1↔1.5: паспорт 3197/25 ↔ 3197/2 — Near.",
        Form11 = [Row11(1, pasNum: "3197/25", facNum: "F-S06")],
        Form15 = [Row11(101, pasNum: "3197/2", facNum: "F-S06")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Near
            }
        }
    };

    /// <summary>S07. Общий суффикс года при разных ядрах — Passport Mismatch (не Near).</summary>
    private static Pairing41TestCase S07_PassportSameYearDifferentCore_Mismatch() => new()
    {
        Name = "S07. Soft 1.1↔1.5: паспорт 829/22 ↔ 721/22 — Mismatch ядра.",
        Form11 = [Row11(1, pasNum: "829/22", facNum: "F-S07")],
        Form15 = [Row11(101, pasNum: "721/22", facNum: "F-S07")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.FactoryNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 }
    };

    /// <summary>S08. 1.2↔1.6: дата +1 день — непарные, OperationDate Near в closest.</summary>
    private static Pairing41TestCase S08_Form12_OneDayDiff_ClosestNearDate()
    {
        const string opDate12 = "2024-06-15";
        const string opDate16 = "2024-06-16";

        return new Pairing41TestCase
        {
            Name = "S08. Soft 1.2↔1.6: дата +1 день — OperationDate Near.",
            Form12 = [Row12(2, opDate: opDate12, documentNumber: "DOC-S08")],
            Form16 = [Row16From12(202, opDate: opDate16, documentNumber: "DOC-S08")],
            ExpectedUnpaired12 = [2],
            ExpectedUnpaired16 = [202],
            ExpectedClosest12Levels = new Dictionary<int, IReadOnlyDictionary<Pairing12To16Field, FieldMatchLevel>>
            {
                [2] = new Dictionary<Pairing12To16Field, FieldMatchLevel>
                {
                    [Pairing12To16Field.OperationDate] = FieldMatchLevel.Near,
                    [Pairing12To16Field.DocumentNumber] = FieldMatchLevel.Exact,
                    [Pairing12To16Field.Mass] = FieldMatchLevel.Exact
                }
            },
            ExpectedClosestCandidate12 = new Dictionary<int, int> { [2] = 202 }
        };
    }

    /// <summary>S09. 1.2↔1.6: масса 1↔1.12 вне 10% парности — непарные, Mass Near в closest.</summary>
    private static Pairing41TestCase S09_Form12_MassNear_OutsidePairingTolerance() => new()
    {
        Name = "S09. Soft 1.2↔1.6: масса 1↔1.12 — Mass Near, не парные.",
        Form12 = [Row12(2, massTon: "1", documentNumber: "DOC-S09")],
        Form16 = [Row16From12(202, massTon: "1.12", documentNumber: "DOC-S09")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202],
        ExpectedClosest12Levels = new Dictionary<int, IReadOnlyDictionary<Pairing12To16Field, FieldMatchLevel>>
        {
            [2] = new Dictionary<Pairing12To16Field, FieldMatchLevel>
            {
                [Pairing12To16Field.Mass] = FieldMatchLevel.Near,
                [Pairing12To16Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate12 = new Dictionary<int, int> { [2] = 202 },
        ExpectedConfidenceMinPercent12 = new Dictionary<int, int> { [2] = 90 }
    };

    /// <summary>S10. 1.3↔1.6: дата +1 день — OperationDate Near.</summary>
    private static Pairing41TestCase S10_Form13_OneDayDiff_ClosestNearDate()
    {
        const string opDate13 = "2024-03-01";
        const string opDate16 = "2024-03-02";

        return new Pairing41TestCase
        {
            Name = "S10. Soft 1.3↔1.6: дата +1 день — OperationDate Near.",
            Form13 = [Row13(3, opDate: opDate13, documentNumber: "DOC-S10")],
            Form16 = [Row16From13(303, opDate: opDate16, documentNumber: "DOC-S10")],
            ExpectedUnpaired13 = [3],
            ExpectedUnpaired16 = [303],
            ExpectedClosest13Levels = new Dictionary<int, IReadOnlyDictionary<Pairing13To16Field, FieldMatchLevel>>
            {
                [3] = new Dictionary<Pairing13To16Field, FieldMatchLevel>
                {
                    [Pairing13To16Field.OperationDate] = FieldMatchLevel.Near,
                    [Pairing13To16Field.DocumentNumber] = FieldMatchLevel.Exact,
                    [Pairing13To16Field.MainRadionuclids] = FieldMatchLevel.Exact
                }
            },
            ExpectedClosestCandidate13 = new Dictionary<int, int> { [3] = 303 }
        };
    }

    /// <summary>S11. 1.3↔1.6: опечатка в нуклиде — MainRadionuclids Near.</summary>
    private static Pairing41TestCase S11_Form13_MainRadsTypo_IsNear() => new()
    {
        Name = "S11. Soft 1.3↔1.6: кобальт-60 ↔ кобальт-61 — MainRadionuclids Near.",
        Form13 = [Row13(3, mainRads: "кобальт-60", documentNumber: "DOC-S11")],
        Form16 = [Row16From13(303, mainRads: "кобальт-61", documentNumber: "DOC-S11")],
        ExpectedUnpaired13 = [3],
        ExpectedUnpaired16 = [303],
        ExpectedClosest13Levels = new Dictionary<int, IReadOnlyDictionary<Pairing13To16Field, FieldMatchLevel>>
        {
            [3] = new Dictionary<Pairing13To16Field, FieldMatchLevel>
            {
                [Pairing13To16Field.MainRadionuclids] = FieldMatchLevel.Near,
                [Pairing13To16Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate13 = new Dictionary<int, int> { [3] = 303 }
    };

    /// <summary>S12. 1.4↔1.6: дата +1 день — OperationDate Near.</summary>
    private static Pairing41TestCase S12_Form14_OneDayDiff_ClosestNearDate()
    {
        const string opDate14 = "2024-10-01";
        const string opDate16 = "2024-10-02";

        return new Pairing41TestCase
        {
            Name = "S12. Soft 1.4↔1.6: дата +1 день — OperationDate Near.",
            Form14 = [Row14(4, opDate: opDate14, documentNumber: "DOC-S12")],
            Form16 = [Row16From14(404, opDate: opDate16, documentNumber: "DOC-S12")],
            ExpectedUnpaired14 = [4],
            ExpectedUnpaired16 = [404],
            ExpectedClosest14Levels = new Dictionary<int, IReadOnlyDictionary<Pairing14To16Field, FieldMatchLevel>>
            {
                [4] = new Dictionary<Pairing14To16Field, FieldMatchLevel>
                {
                    [Pairing14To16Field.OperationDate] = FieldMatchLevel.Near,
                    [Pairing14To16Field.DocumentNumber] = FieldMatchLevel.Exact
                }
            },
            ExpectedClosestCandidate14 = new Dictionary<int, int> { [4] = 404 }
        };
    }

    /// <summary>S13. 1.4↔1.6: масса 0.5↔0.56 вне 10% парности — Mass Near.</summary>
    private static Pairing41TestCase S13_Form14_MassNear_OutsidePairingTolerance() => new()
    {
        Name = "S13. Soft 1.4↔1.6: масса 0.5↔0.56 — Mass Near, не парные.",
        Form14 = [Row14(4, massTon: "0.5", documentNumber: "DOC-S13")],
        Form16 = [Row16From14(404, massTon: "0.56", documentNumber: "DOC-S13")],
        ExpectedUnpaired14 = [4],
        ExpectedUnpaired16 = [404],
        ExpectedClosest14Levels = new Dictionary<int, IReadOnlyDictionary<Pairing14To16Field, FieldMatchLevel>>
        {
            [4] = new Dictionary<Pairing14To16Field, FieldMatchLevel>
            {
                [Pairing14To16Field.Mass] = FieldMatchLevel.Near,
                [Pairing14To16Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate14 = new Dictionary<int, int> { [4] = 404 }
    };

    /// <summary>S14. 1.4↔1.6: объём 2↔2.25 вне 10% парности — Volume Near.</summary>
    private static Pairing41TestCase S14_Form14_VolumeNear_IsNear() => new()
    {
        Name = "S14. Soft 1.4↔1.6: объём 2↔2.25 — Volume Near, не парные.",
        Form14 = [Row14(4, volume: "2", documentNumber: "DOC-S14")],
        Form16 = [Row16From14(404, volume: "2.25", documentNumber: "DOC-S14")],
        ExpectedUnpaired14 = [4],
        ExpectedUnpaired16 = [404],
        ExpectedClosest14Levels = new Dictionary<int, IReadOnlyDictionary<Pairing14To16Field, FieldMatchLevel>>
        {
            [4] = new Dictionary<Pairing14To16Field, FieldMatchLevel>
            {
                [Pairing14To16Field.Volume] = FieldMatchLevel.Near,
                [Pairing14To16Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosestCandidate14 = new Dictionary<int, int> { [4] = 404 }
    };
}
