using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа H — карты closest-match для непарных строк.</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> ClosestMatchCases()
    {
        yield return H01_Form11_DocumentNumberMismatch_Highlighted();
        yield return H02_Form12_MassMismatch_Highlighted();
        yield return H03_Form16_Chooses13_WhenHigherScore();
        yield return H04_Form16_Chooses13_WhenBetterScore();
        yield return H05_NoReference_NoClosestMap();
        yield return H06_EmptySerial_ExtraRowOn15_QuantityEqualTrue();
        yield return H07_EmptySerial_PartialQtyRemainder_QuantityByValue();
        yield return H08_EmptySerial_SourceQtyRemainder_QuantityHighlightedFalse();
        yield return H09_WithSerial_QuantityEqual_StillGreenOnOtherMismatch();
        yield return H10_Form13_AggregateStateMatchesCodeRaoDigit_True();
        yield return H11_EmptySerial_PackMismatch_QuantityTrue_PackFalse();
        yield return H12_Form13_AggregateStateMismatchCodeRaoDigit_False();
        yield return H13_PicksBestScoreCandidate_NotWeakerOne();
        yield return H14_SymmetricFieldMap_LeftAndRightShareSameMismatches();
    }

    /// <summary>H01. Единственное расхождение — номер документа; closest отмечает его false.</summary>
    private static Pairing41TestCase H01_Form11_DocumentNumberMismatch_Highlighted() => new()
    {
        Name = "H01. Closest 1.1↔1.5: номер документа не совпал, паспорт и тип — да.",
        Form11 = [Row11(1, documentNumber: "DOC-A")],
        Form15 = [Row11(101, documentNumber: "DOC-B")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.PassportNumber] = true,
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosest15 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [101] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.PassportNumber] = true,
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 },
        ExpectedClosestCandidate15 = new Dictionary<int, int> { [101] = 1 },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosest15Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [101] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>H02. Масса вне допуска; closest на 1.2 и профиль Form12 на 1.6.</summary>
    private static Pairing41TestCase H02_Form12_MassMismatch_Highlighted() => new()
    {
        Name = "H02. Closest 1.2↔1.6: масса false, документ и упаковка true.",
        Form12 = [Row12(2, massTon: "1")],
        Form16 = [Row16From12(202, massTon: "1.2")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202],
        ExpectedClosest12 = new Dictionary<int, IReadOnlyDictionary<Pairing12To16Field, bool>>
        {
            [2] = new Dictionary<Pairing12To16Field, bool>
            {
                [Pairing12To16Field.Mass] = false,
                [Pairing12To16Field.DocumentNumber] = true,
                [Pairing12To16Field.PackNumber] = true
            }
        },
        ExpectedClosestCandidate12 = new Dictionary<int, int> { [2] = 202 },
        ExpectedClosestCandidate16 = new Dictionary<int, int> { [202] = 2 },
        ExpectedClosest16 = new Dictionary<int, Pairing41Form16ClosestExpectation>
        {
            [202] = new()
            {
                Profile = Form16MatchProfile.Form12,
                Matches12 = new Dictionary<Pairing12To16Field, bool>
                {
                    [Pairing12To16Field.Mass] = false,
                    [Pairing12To16Field.DocumentNumber] = true
                },
                Levels12 = new Dictionary<Pairing12To16Field, FieldMatchLevel>
                {
                    [Pairing12To16Field.Mass] = FieldMatchLevel.Near,
                    [Pairing12To16Field.DocumentNumber] = FieldMatchLevel.Exact
                }
            }
        },
        ExpectedClosest12Levels = new Dictionary<int, IReadOnlyDictionary<Pairing12To16Field, FieldMatchLevel>>
        {
            [2] = new Dictionary<Pairing12To16Field, FieldMatchLevel>
            {
                [Pairing12To16Field.Mass] = FieldMatchLevel.Near,
                [Pairing12To16Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing12To16Field.PackNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H03. При soft-score выше у 1.3 — closest-профиль Form13 (tie-break Form12 — см. S01).
    /// </summary>
    private static Pairing41TestCase H03_Form16_Chooses13_WhenHigherScore()
    {
        const string opDate = "2024-07-01";
        const string doc = "DOC-H";
        const string pack = "УКТ-H";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "H03. Closest 1.6: при soft-score выше у 1.3 — профиль Form13.",
            Form12 =
            [
                // масса 1 — у 1.6 будет 9 → −1 к score12
                Row12(2, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    massTon: "1", beta: beta, alpha: "-", activityMeasurementDate: opDate)
            ],
            Form13 =
            [
                // documentDate/creation другие; у 1.6 другие main rads → −3 к score13
                Row13(3, opDate: opDate, documentNumber: doc, documentDate: "2020-01-01", packNumber: pack,
                    mainRads: "кобальт-60", beta: beta, alpha: "-", creationDate: "2023-01-01")
            ],
            Form16 =
            [
                new Pairing41Row
                {
                    Id = 902,
                    OpDate = opDate,
                    Mass = "9",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TritiumActivity = "-",
                    TransuraniumActivity = "-",
                    MainRadionuclids = "цезий-137",
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
            ExpectedUnpaired16 = [902],
            ExpectedClosest16 = new Dictionary<int, Pairing41Form16ClosestExpectation>
            {
                [902] = new() { Profile = Form16MatchProfile.Form13 }
            }
        };
    }

    /// <summary>
    /// H04. 1.2 почти не пересекается с 1.6; 1.3 почти совпадает (кроме номера документа)
    /// → closest-профиль Form13, DocumentNumber = false.
    /// </summary>
    private static Pairing41TestCase H04_Form16_Chooses13_WhenBetterScore()
    {
        const string opDate = "2024-08-01";
        const string doc = "DOC-H4";
        const string pack = "УКТ-H4";

        return new Pairing41TestCase
        {
            Name = "H04. Closest 1.6: score выше у 1.3 — профиль Form13.",
            Form12 =
            [
                // заведомо «чужая» строка → низкий score12
                Row12(2, opDate: "2020-01-01", documentNumber: "OTHER", packNumber: "OTHER",
                    massTon: "99", beta: "1", alpha: "1", activityMeasurementDate: "2020-01-01")
            ],
            Form13 =
            [
                Row13(3, opDate: opDate, documentNumber: doc, packNumber: pack,
                    mainRads: "кобальт-60", beta: "1.0e+06", creationDate: opDate)
            ],
            Form16 =
            [
                // почти как 1.3, но DOC-DIFF → непарная, closest всё равно Form13
                Row16From13(303, opDate: opDate, documentNumber: "DOC-DIFF", packNumber: pack,
                    mainRads: "кобальт-60", beta: "1.0e+06", activityMeasurementDate: opDate)
            ],
            ExpectedUnpaired12 = [2],
            ExpectedUnpaired13 = [3],
            ExpectedUnpaired16 = [303],
            ExpectedClosest16 = new Dictionary<int, Pairing41Form16ClosestExpectation>
            {
                [303] = new()
                {
                    Profile = Form16MatchProfile.Form13,
                    Matches13 = new Dictionary<Pairing13To16Field, bool>
                    {
                        [Pairing13To16Field.MainRadionuclids] = true,
                        [Pairing13To16Field.DocumentNumber] = false,
                        [Pairing13To16Field.PackNumber] = true
                    },
                    Levels13 = new Dictionary<Pairing13To16Field, FieldMatchLevel>
                    {
                        [Pairing13To16Field.MainRadionuclids] = FieldMatchLevel.Exact,
                        [Pairing13To16Field.DocumentNumber] = FieldMatchLevel.Mismatch,
                        [Pairing13To16Field.PackNumber] = FieldMatchLevel.Exact
                    }
                }
            }
        };
    }

    /// <summary>H05. Непарная 1.2 без кандидатов на 1.6 → карта closest пустая.</summary>
    private static Pairing41TestCase H05_NoReference_NoClosestMap() => new()
    {
        Name = "H05. Нет строк 1.6 — closest-карта для 1.2 пустая.",
        Form12 = [Row12(2)],
        Form16 = [],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [],
        ExpectedClosest12 = new Dictionary<int, IReadOnlyDictionary<Pairing12To16Field, bool>>()
    };

    /// <summary>
    /// H06. Пустые серийные: 1×qty=1 на 1.1 и 2×qty=1 на 1.5 → одна непарная на 1.5.
    /// У ближайшего кандидата qty тоже 1 → Quantity в closest true (построчно).
    /// </summary>
    private static Pairing41TestCase H06_EmptySerial_ExtraRowOn15_QuantityEqualTrue() => new()
    {
        Name = "H06. Пустые серийные: лишняя qty=1 на 1.5 — Quantity в closest true (1==1).",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 1)],
        Form15 =
        [
            Row11(101, pasNum: "", facNum: "", quantity: 1),
            Row11(102, pasNum: "", facNum: "", quantity: 1)
        ],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = [102],
        ExpectedClosest15 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [102] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Quantity] = true,
                [Pairing11To15Field.DocumentNumber] = true,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosest15Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [102] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H07. Пустые серийные: 1.1 qty=3; 1.5 qty=3,3,2 → непарные 3 и 2 на 1.5.
    /// Closest к 102 (qty=3) даёт Quantity true; к 103 (qty=2) — false vs 3.
    /// </summary>
    private static Pairing41TestCase H07_EmptySerial_PartialQtyRemainder_QuantityByValue() => new()
    {
        Name = "H07. Пустые серийные: остаток 3 и 2 на 1.5 — Quantity true/false по числам.",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 3)],
        Form15 =
        [
            Row11(101, pasNum: "", facNum: "", quantity: 3),
            Row11(102, pasNum: "", facNum: "", quantity: 3),
            Row11(103, pasNum: "", facNum: "", quantity: 2)
        ],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = [102, 103],
        ExpectedClosest15 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [102] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Quantity] = true,
                [Pairing11To15Field.DocumentNumber] = true,
                [Pairing11To15Field.Type] = true
            },
            [103] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Quantity] = false,
                [Pairing11To15Field.DocumentNumber] = true,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosest15Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [102] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            },
            [103] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H08. Пустые серийные: 1.1 qty=8 vs 1.5 qty=5 → остаток на 1.1, Quantity false (8≠5).
    /// </summary>
    private static Pairing41TestCase H08_EmptySerial_SourceQtyRemainder_QuantityHighlightedFalse() => new()
    {
        Name = "H08. Пустые серийные: остаток qty на 1.1 — Quantity false (8≠5).",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 8)],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 5)],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [],
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Quantity] = false,
                [Pairing11To15Field.DocumentNumber] = true,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H09. С серийными номерами: расхождение только документа; qty одинаковый —
    /// Quantity остаётся true (ветка агрегации не применяется).
    /// </summary>
    private static Pairing41TestCase H09_WithSerial_QuantityEqual_StillGreenOnOtherMismatch() => new()
    {
        Name = "H09. С серийными: qty совпал, документ нет — Quantity true.",
        Form11 = [Row11(1, documentNumber: "DOC-A", quantity: 3)],
        Form15 = [Row11(101, documentNumber: "DOC-B", quantity: 3)],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Quantity] = true,
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.PassportNumber] = true
            }
        },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H10. Непарная 1.3 с AggregateState=1; ближайшая 1.6 с кодом РАО, начинающимся на «1»
    /// (единственный кандидат, номер документа не совпал) → AggregateStateMatchesCodeRao = true.
    /// </summary>
    private static Pairing41TestCase H10_Form13_AggregateStateMatchesCodeRaoDigit_True() => new()
    {
        Name = "H10. Closest 1.3↔1.6: AggregateState=1 совпадает с первой цифрой кода РАО — true.",
        Form13 = [Row13(3, aggregateState: 1, documentNumber: "DOC-H10")],
        Form16 = [Row16From13(303, codeRao: "184100084_", documentNumber: "DOC-H10-DIFF")],
        ExpectedUnpaired13 = [3],
        ExpectedUnpaired16 = [303],
        ExpectedAggregateStateMatch13 = new Dictionary<int, bool> { [3] = true }
    };

    /// <summary>
    /// H11. Как у пользователя: пустые серийные, qty=1 с обеих сторон, разные УКТ.
    /// Непарность из‑за упаковки; Quantity в closest true, PackNumber false.
    /// </summary>
    private static Pairing41TestCase H11_EmptySerial_PackMismatch_QuantityTrue_PackFalse() => new()
    {
        Name = "H11. Пустые серийные: разный УКТ — PackNumber false, Quantity true.",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 1, packNumber: "1604022")],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 1, packNumber: "16041148")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.PackNumber] = false,
                [Pairing11To15Field.Quantity] = true,
                [Pairing11To15Field.Type] = true,
                [Pairing11To15Field.DocumentNumber] = true
            }
        },
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 },
        ExpectedClosestCandidate15 = new Dictionary<int, int> { [101] = 1 },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PackNumber] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosest15Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [101] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.PackNumber] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.Quantity] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H12. AggregateState=2, код РАО ближайшей 1.6 начинается на «1» → AggregateStateMatchesCodeRao = false.
    /// </summary>
    private static Pairing41TestCase H12_Form13_AggregateStateMismatchCodeRaoDigit_False() => new()
    {
        Name = "H12. Closest 1.3↔1.6: AggregateState=2 не бьётся с первой цифрой «1» кода РАО.",
        Form13 = [Row13(3, aggregateState: 2, documentNumber: "DOC-H12")],
        Form16 = [Row16From13(303, codeRao: "184100084_", documentNumber: "DOC-H12-DIFF")],
        ExpectedUnpaired13 = [3],
        ExpectedUnpaired16 = [303],
        ExpectedAggregateStateMatch13 = new Dictionary<int, bool> { [3] = false },
        ExpectedClosestCandidate13 = new Dictionary<int, int> { [3] = 303 }
    };

    /// <summary>
    /// H13. Два кандидата на 1.5: слабый (другой тип+документ) и сильный (только документ).
    /// Closest должен выбрать сильного (Id=102), а не слабого (Id=101).
    /// </summary>
    private static Pairing41TestCase H13_PicksBestScoreCandidate_NotWeakerOne() => new()
    {
        Name = "H13. Closest выбирает кандидата с лучшим score, не первого попавшегося.",
        Form11 = [Row11(1, documentNumber: "DOC-SRC", type: "Тип-А")],
        Form15 =
        [
            Row11(101, documentNumber: "DOC-WEAK", type: "Тип-Б"),
            Row11(102, documentNumber: "DOC-BEST", type: "Тип-А")
        ],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101, 102],
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 102 },
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.Type] = true,
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.PassportNumber] = true
            }
        },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Mismatch,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>
    /// H14. Одна карта FieldMatches для пары: у непарной 1.1 и у её кандидата 1.5
    /// одинаковые true/false по полям (как в Excel слева и справа одной строкой).
    /// </summary>
    private static Pairing41TestCase H14_SymmetricFieldMap_LeftAndRightShareSameMismatches() => new()
    {
        Name = "H14. Одна карта FieldMatches: слева и справа одни и те же совпадения/расхождения.",
        Form11 = [Row11(1, documentNumber: "DOC-A", type: "Тип-А")],
        Form15 = [Row11(101, documentNumber: "DOC-B", type: "Тип-А")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101],
        ExpectedClosestCandidate11 = new Dictionary<int, int> { [1] = 101 },
        ExpectedClosestCandidate15 = new Dictionary<int, int> { [101] = 1 },
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.Type] = true,
                [Pairing11To15Field.PassportNumber] = true
            }
        },
        ExpectedClosest15 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [101] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.DocumentNumber] = false,
                [Pairing11To15Field.Type] = true,
                [Pairing11To15Field.PassportNumber] = true
            }
        },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact
            }
        },
        ExpectedClosest15Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [101] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Near,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact
            }
        }
    };
}
