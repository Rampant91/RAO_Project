using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа A — сопоставление 1.1 ↔ 1.5 (ЗРИ).</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> Form11To15Cases()
    {
        yield return A01_FullMatch_WithSerial_Empty();
        yield return A02_OneFieldMismatch_BothSidesUnpaired();
        yield return A03_ExtraRowOnEachSide();
        yield return A04_EmptySerial_Quantity8vs8_Paired();
        yield return A05_EmptySerial_Quantity8vs5_SourceUnpaired();
        yield return A06_ActivityWithin10Percent_Paired();
        yield return A07_ActivityOutside10Percent_Unpaired();
        yield return A08_EmptySerialPlaceholders_QuantityPaired();
        yield return A09_SerialPlaceholderPassport_MatchesEmptyPassport();
        yield return A10_Form15Code14_UnpairedOn11_NotOn15();
        yield return A11_Form15Code14_OpCodeCheckOff_Paired();
        yield return A12_Form15Code14_Alone_NotExportedAsUnpaired15();
        yield return A13_EmptySerial_DifferentPackNumber_Unpaired();
        yield return A14_EmptySerial_EightOnesVsFourPlusFour_Paired();
    }

    /// <summary>A01. Две пары с заполненными паспорт/зав.№ → непарных нет.</summary>
    private static Pairing41TestCase A01_FullMatch_WithSerial_Empty() => new()
    {
        Name = "A01. Полное совпадение 1.1↔1.5 с серийными номерами.",
        Form11 = [Row11(1), Row11(2, pasNum: "P-200", facNum: "F-200")],
        Form15 = [Row11(101), Row11(102, pasNum: "P-200", facNum: "F-200")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>A02. Отличается только номер документа → оба непарные.</summary>
    private static Pairing41TestCase A02_OneFieldMismatch_BothSidesUnpaired() => new()
    {
        Name = "A02. Расхождение номера документа — непарные с обеих сторон.",
        // 1.1 DOC-A vs 1.5 DOC-B
        Form11 = [Row11(1, documentNumber: "DOC-A")],
        Form15 = [Row11(101, documentNumber: "DOC-B")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101]
    };

    /// <summary>A03. По одной «лишней» строке на каждой стороне.</summary>
    private static Pairing41TestCase A03_ExtraRowOnEachSide() => new()
    {
        Name = "A03. Лишняя строка на 1.1 и лишняя на 1.5.",
        Form11 =
        [
            Row11(1), // пара с 101
            Row11(2, pasNum: "P-ONLY-11", facNum: "F-ONLY-11")
        ],
        Form15 =
        [
            Row11(101),
            Row11(102, pasNum: "P-ONLY-15", facNum: "F-ONLY-15")
        ],
        ExpectedUnpaired11 = [2],
        ExpectedUnpaired15 = [102]
    };

    /// <summary>A04. Пустые паспорт+зав.№: 8×qty=1 ↔ 1×qty=8 → партия сошлась.</summary>
    private static Pairing41TestCase A04_EmptySerial_Quantity8vs8_Paired() => new()
    {
        Name = "A04. Пустые серийные: 8 строк по 1 ↔ одна строка qty=8.",
        Form11 =
        [
            Row11(1, pasNum: "", facNum: "", quantity: 1),
            Row11(2, pasNum: "", facNum: "", quantity: 1),
            Row11(3, pasNum: "", facNum: "", quantity: 1),
            Row11(4, pasNum: "", facNum: "", quantity: 1),
            Row11(5, pasNum: "", facNum: "", quantity: 1),
            Row11(6, pasNum: "", facNum: "", quantity: 1),
            Row11(7, pasNum: "", facNum: "", quantity: 1),
            Row11(8, pasNum: "", facNum: "", quantity: 1)
        ],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 8)],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>A05. Пустые серийные: qty 8 на 1.1 vs 5 на 1.5 → остаток на 1.1.</summary>
    private static Pairing41TestCase A05_EmptySerial_Quantity8vs5_SourceUnpaired() => new()
    {
        Name = "A05. Пустые серийные: qty 8 vs 5 — остаток на 1.1.",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 8)],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 5)],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = []
    };

    /// <summary>A06. Активность 100 vs 109 (внутри ±10%) → пара.</summary>
    private static Pairing41TestCase A06_ActivityWithin10Percent_Paired() => new()
    {
        Name = "A06. Активность внутри допуска ±10% — парные.",
        Form11 = [Row11(1, activity: "100")],
        Form15 = [Row11(101, activity: "109")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>A07. Активность 100 vs 112 (вне ±10%) → оба непарные.</summary>
    private static Pairing41TestCase A07_ActivityOutside10Percent_Unpaired() => new()
    {
        Name = "A07. Активность вне допуска ±10% — непарные.",
        Form11 = [Row11(1, activity: "100")],
        Form15 = [Row11(101, activity: "112")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101]
    };

    /// <summary>A08. «б.н.» / «без номера» / «-» как пустые серийные → qty-сводка.</summary>
    private static Pairing41TestCase A08_EmptySerialPlaceholders_QuantityPaired() => new()
    {
        Name = "A08. Заглушки серийных (б.н., без номера, -) — как пустые, qty сходится.",
        Form11 =
        [
            Row11(1, pasNum: "б.н.", facNum: "без номера", quantity: 1),
            Row11(2, pasNum: "-", facNum: "бн", quantity: 1)
        ],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 2)],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>A09. Паспорт «б.н.» ↔ пустой паспорт при одном зав.№ → пара по ключу с серийными.</summary>
    private static Pairing41TestCase A09_SerialPlaceholderPassport_MatchesEmptyPassport() => new()
    {
        Name = "A09. Паспорт «б.н.» эквивалентен пустому при том же зав.№.",
        Form11 = [Row11(1, pasNum: "б.н.", facNum: "F-100")],
        Form15 = [Row11(101, pasNum: "", facNum: "F-100")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>
    /// A10. 1.1 код 41 ↔ 1.5 код 14 при прочих равных: кандидат находится (closest),
    /// 1.1 непарная, строка с кодом 14 не попадает в непарные 1.5.
    /// </summary>
    private static Pairing41TestCase A10_Form15Code14_UnpairedOn11_NotOn15() => new()
    {
        Name = "A10. 1.5 с кодом 14: непарная на 1.1, код 14 не в непарных 1.5.",
        Form11 = [Row11(1)],
        Form15 = [Row11(101, opCode: "14")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [],
        ExpectedClosest11 = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, bool>>
        {
            [1] = new Dictionary<Pairing11To15Field, bool>
            {
                [Pairing11To15Field.OperationCode] = false,
                [Pairing11To15Field.PassportNumber] = true,
                [Pairing11To15Field.DocumentNumber] = true,
                [Pairing11To15Field.Type] = true
            }
        },
        ExpectedClosest11Levels = new Dictionary<int, IReadOnlyDictionary<Pairing11To15Field, FieldMatchLevel>>
        {
            [1] = new Dictionary<Pairing11To15Field, FieldMatchLevel>
            {
                [Pairing11To15Field.OperationCode] = FieldMatchLevel.Near,
                [Pairing11To15Field.PassportNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.DocumentNumber] = FieldMatchLevel.Exact,
                [Pairing11To15Field.Type] = FieldMatchLevel.Exact
            }
        }
    };

    /// <summary>A11. Та же пара 41↔14, но галка «код операции» снята → считаются парными.</summary>
    private static Pairing41TestCase A11_Form15Code14_OpCodeCheckOff_Paired() => new()
    {
        Name = "A11. 1.5 код 14 при выключенном CheckOperationCode — парные.",
        Params11To15 = new Pairing11To15Params(CheckOperationCode: false),
        Form11 = [Row11(1)],
        Form15 = [Row11(101, opCode: "14")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>A12. Только 1.5 с кодом 14 (без 1.1) — в непарные 1.5 не попадает.</summary>
    private static Pairing41TestCase A12_Form15Code14_Alone_NotExportedAsUnpaired15() => new()
    {
        Name = "A12. Одиночная 1.5 с кодом 14 — не в непарных 1.5.",
        Form15 = [Row11(101, opCode: "14")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>
    /// A13. Пустые серийные, одинаковый qty, разные номера УКТ — разные партии, не суммируются.
    /// </summary>
    private static Pairing41TestCase A13_EmptySerial_DifferentPackNumber_Unpaired() => new()
    {
        Name = "A13. Пустые серийные: разный номер УКТ — не суммируются, оба непарные.",
        Form11 = [Row11(1, pasNum: "", facNum: "", quantity: 1, packNumber: "УКТ-A")],
        Form15 = [Row11(101, pasNum: "", facNum: "", quantity: 1, packNumber: "УКТ-B")],
        ExpectedUnpaired11 = [1],
        ExpectedUnpaired15 = [101]
    };

    /// <summary>
    /// A14. Пустые серийные: 8×qty=1 ↔ qty=4 + qty=4 — разная нарезка одной партии, парные.
    /// </summary>
    private static Pairing41TestCase A14_EmptySerial_EightOnesVsFourPlusFour_Paired() => new()
    {
        Name = "A14. Пустые серийные: 8×1 ↔ 4+4 — парные.",
        Form11 =
        [
            Row11(1, pasNum: "", facNum: "", quantity: 1),
            Row11(2, pasNum: "", facNum: "", quantity: 1),
            Row11(3, pasNum: "", facNum: "", quantity: 1),
            Row11(4, pasNum: "", facNum: "", quantity: 1),
            Row11(5, pasNum: "", facNum: "", quantity: 1),
            Row11(6, pasNum: "", facNum: "", quantity: 1),
            Row11(7, pasNum: "", facNum: "", quantity: 1),
            Row11(8, pasNum: "", facNum: "", quantity: 1)
        ],
        Form15 =
        [
            Row11(101, pasNum: "", facNum: "", quantity: 4),
            Row11(102, pasNum: "", facNum: "", quantity: 4)
        ],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };
}
