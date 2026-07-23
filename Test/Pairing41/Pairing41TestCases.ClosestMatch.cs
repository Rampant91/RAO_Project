using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа H — карты closest-match для непарных строк.</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> ClosestMatchCases()
    {
        yield return H01_Form11_DocumentNumberMismatch_Highlighted();
        yield return H02_Form12_MassMismatch_Highlighted();
        yield return H03_Form16_Prefers12_WhenScoresEqual();
        yield return H04_Form16_Chooses13_WhenBetterScore();
        yield return H05_NoReference_NoClosestMap();
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
        ExpectedClosest16 = new Dictionary<int, Pairing41Form16ClosestExpectation>
        {
            [202] = new()
            {
                Profile = Form16MatchProfile.Form12,
                Matches12 = new Dictionary<Pairing12To16Field, bool>
                {
                    [Pairing12To16Field.Mass] = false,
                    [Pairing12To16Field.DocumentNumber] = true
                }
            }
        }
    };

    /// <summary>
    /// H03. Равный score 12 и 13 → приоритет профиля Form12.
    /// Score12: только Mass не совпадает (10 из 11).
    /// Score13: не совпадают MainRads, AMD, DocumentDate (10 из 13).
    /// </summary>
    private static Pairing41TestCase H03_Form16_Prefers12_WhenScoresEqual()
    {
        const string opDate = "2024-07-01";
        const string doc = "DOC-H";
        const string pack = "УКТ-H";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "H03. Closest 1.6: при равном score выбирается профиль 1.2.",
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
                [902] = new() { Profile = Form16MatchProfile.Form12 }
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
}
