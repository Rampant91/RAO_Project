using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Client_App.Resources;
using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа P — опции диалога (Check* = false снимает поле из ключа).</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> ParamsCases()
    {
        yield return P01_IgnoreType_MismatchBecomesPaired();
        yield return P02_IgnoreMass12_MismatchBecomesPaired();
        yield return P03_IgnoreActivityMeasurementDate13_Paired();
        yield return P04_IgnoreVolume14_MismatchBecomesPaired();
        yield return P05_OnlyDocumentNumber12_OtherDiffsIgnored();
        yield return P06_IgnoreMass_SymmetricBothSidesPaired();
        yield return P07_IgnoreCodeRao12_MismatchBecomesPaired();
        yield return P08_IgnoreCodeRao13_MismatchBecomesPaired();
        yield return P09_IgnoreCodeRao14_MismatchBecomesPaired();
    }

    /// <summary>P01. Как A02 по типу, но CheckType выключен → пара находится.</summary>
    private static Pairing41TestCase P01_IgnoreType_MismatchBecomesPaired() => new()
    {
        Name = "P01. Выключен Type — расхождение типа не мешает паре 1.1↔1.5.",
        Params11To15 = new Pairing11To15Params(CheckType: false),
        Form11 = [Row11(1, type: "Тип-А")],
        Form15 = [Row11(101, type: "Тип-Б")],
        ExpectedUnpaired11 = [],
        ExpectedUnpaired15 = []
    };

    /// <summary>P02. Как B03, но CheckMass выключен → пара находится.</summary>
    private static Pairing41TestCase P02_IgnoreMass12_MismatchBecomesPaired() => new()
    {
        Name = "P02. Выключена масса — 1 т vs 1.2 т всё равно парные (1.2↔1.6).",
        Params12To16 = new Pairing12To16Params(CheckMass: false),
        Form12 = [Row12(2, massTon: "1")],
        Form16 = [Row16From12(202, massTon: "1.2")],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P03. AMD разная, но CheckActivityMeasurementDate выключен → пара.</summary>
    private static Pairing41TestCase P03_IgnoreActivityMeasurementDate13_Paired() => new()
    {
        Name = "P03. Выключена AMD — разные даты измерения 1.3↔1.6 парные.",
        Params13To16 = new Pairing13To16Params(CheckActivityMeasurementDate: false),
        Form13 = [Row13(3, creationDate: "2022-01-01")],
        Form16 = [Row16From13(303, activityMeasurementDate: "2023-06-01")],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P04. Объём разный, CheckVolume выключен → пара.</summary>
    private static Pairing41TestCase P04_IgnoreVolume14_MismatchBecomesPaired() => new()
    {
        Name = "P04. Выключен объём — 2 vs 2.5 парные (1.4↔1.6).",
        Params14To16 = new Pairing14To16Params(CheckVolume: false),
        Form14 = [Row14(4, volume: "2")],
        Form16 = [Row16From14(404, volume: "2.5")],
        ExpectedUnpaired14 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P05. Включён только номер документа — масса/упаковка не мешают.</summary>
    private static Pairing41TestCase P05_OnlyDocumentNumber12_OtherDiffsIgnored() => new()
    {
        Name = "P05. Только номер документа в ключе — остальные отличия игнорируются.",
        Params12To16 = new Pairing12To16Params(
            CheckOperationDate: false,
            CheckMass: false,
            CheckBetaGammaActivity: false,
            CheckAlphaActivity: false,
            CheckActivityMeasurementDate: false,
            CheckDocumentVid: false,
            CheckDocumentNumber: true,
            CheckDocumentDate: false,
            CheckPackName: false,
            CheckPackType: false,
            CheckPackNumber: false),
        // масса и упаковка разные, DOC-SAME общий
        Form12 = [Row12(2, massTon: "1", packNumber: "УКТ-A", documentNumber: "DOC-SAME")],
        Form16 = [Row16From12(202, massTon: "9", packNumber: "УКТ-B", documentNumber: "DOC-SAME")],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P06. Выключенная масса — симметрия обеих сторон 1.2↔1.6.</summary>
    private static Pairing41TestCase P06_IgnoreMass_SymmetricBothSidesPaired() => new()
    {
        Name = "P06. Выключена масса — симметрично парные 1.2 и 1.6.",
        Params12To16 = new Pairing12To16Params(CheckMass: false),
        Form12 = [Row12(2, massTon: "1")],
        Form16 = [Row16From12(202, massTon: "1.5")],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P07. Разный код РАО при выключенном CheckCodeRao — всё равно парные.</summary>
    private static Pairing41TestCase P07_IgnoreCodeRao12_MismatchBecomesPaired() => new()
    {
        Name = "P07. Выключен Код РАО — расхождение кода не мешает паре 1.2↔1.6.",
        Params12To16 = new Pairing12To16Params(CheckCodeRao: false),
        Form12 = [Row12(2, codeRao: RaoCodeHelper.Form12CodeRao)],
        Form16 = [Row16From12(202, codeRao: "99999999999")],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P08. Разный код РАО 1.3↔1.6 при выключенном CheckCodeRao13.</summary>
    private static Pairing41TestCase P08_IgnoreCodeRao13_MismatchBecomesPaired() => new()
    {
        Name = "P08. Выключен Код РАО — расхождение кода не мешает паре 1.3↔1.6.",
        Params13To16 = new Pairing13To16Params(CheckCodeRao: false),
        Form13 = [Row13(3, codeRao: "11111111111")],
        Form16 = [Row16From13(303, codeRao: "99999999999")],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>P09. Разный код РАО 1.4↔1.6 при выключенном CheckCodeRao14.</summary>
    private static Pairing41TestCase P09_IgnoreCodeRao14_MismatchBecomesPaired() => new()
    {
        Name = "P09. Выключен Код РАО — расхождение кода не мешает паре 1.4↔1.6.",
        Params14To16 = new Pairing14To16Params(CheckCodeRao: false),
        Form14 = [Row14(4, codeRao: "11111111111")],
        Form16 = [Row16From14(404, codeRao: "99999999999")],
        ExpectedUnpaired14 = [],
        ExpectedUnpaired16 = []
    };
}
