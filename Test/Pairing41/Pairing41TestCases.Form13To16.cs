using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;

namespace Test.Pairing41;

/// <summary>Группа C — сопоставление 1.3 ↔ 1.6 (ОРИ-изделия).</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> Form13To16Cases()
    {
        yield return C01_BetaNuclide_Paired();
        yield return C02_AlphaTritiumTransuranium_Paired();
        yield return C03_MixedNuclideTypes_DashesVsFilledBeta_Unpaired();
        yield return C04_MainRadionuclidsOrderAndLookalike_Paired();
        yield return C05_TypeIgnored_StillPaired();
        yield return C06_ActivityMeasurementDateFromCreation_Paired();
    }

    /// <summary>C01. Бета-нуклид, одинаковые активности → пара.</summary>
    private static Pairing41TestCase C01_BetaNuclide_Paired() => new()
    {
        Name = "C01. Бета-нуклид 1.3↔1.6 — парные.",
        Form13 = [Row13(3, mainRads: "кобальт-60", beta: "1.0e+06")],
        Form16 = [Row16From13(303, mainRads: "кобальт-60", beta: "1.0e+06")],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>C02. Три пары: альфа / тритий / трансурановые.</summary>
    private static Pairing41TestCase C02_AlphaTritiumTransuranium_Paired() => new()
    {
        Name = "C02. Альфа, тритий и трансурановые — три пары.",
        Form13 =
        [
            Row13(31, mainRads: "плутоний-239", beta: "-", alpha: "2.0e+05", documentNumber: "DOC-A", packNumber: "УКТ-A"),
            Row13(32, mainRads: "тритий", beta: "-", tritium: "3.0e+04", documentNumber: "DOC-T", packNumber: "УКТ-T"),
            Row13(33, mainRads: "америций-241", beta: "-", transuranium: "4.0e+03", documentNumber: "DOC-U", packNumber: "УКТ-U")
        ],
        Form16 =
        [
            Row16From13(331, mainRads: "плутоний-239", beta: "-", alpha: "2.0e+05", documentNumber: "DOC-A", packNumber: "УКТ-A"),
            Row16From13(332, mainRads: "тритий", beta: "-", tritium: "3.0e+04", documentNumber: "DOC-T", packNumber: "УКТ-T"),
            Row16From13(333, mainRads: "америций-241", beta: "-", transuranium: "4.0e+03", documentNumber: "DOC-U", packNumber: "УКТ-U")
        ],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>C03. Смешанные типы → на РВ «-»; на 1.6 заполнена бета → непарные.</summary>
    private static Pairing41TestCase C03_MixedNuclideTypes_DashesVsFilledBeta_Unpaired() => new()
    {
        Name = "C03. Смесь типов нуклидов: «-» на 1.3 vs бета на 1.6 — непарные.",
        Form13 =
        [
            Row13(3, mainRads: "кобальт-60; плутоний-239", beta: "-", alpha: "-", tritium: "-", transuranium: "-")
        ],
        Form16 =
        [
            Row16From13(303, mainRads: "кобальт-60; плутоний-239", beta: "1.0e+06")
        ],
        ExpectedUnpaired13 = [3],
        ExpectedUnpaired16 = [303]
    };

    /// <summary>C04. Порядок нуклидов в списке не важен.</summary>
    private static Pairing41TestCase C04_MainRadionuclidsOrderAndLookalike_Paired() => new()
    {
        Name = "C04. Порядок радионуклидов разный — всё равно парные.",
        Form13 = [Row13(3, mainRads: "кобальт-60; цезий-137")],
        Form16 = [Row16From13(303, mainRads: "цезий-137; кобальт-60")],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>C05. Type есть только у 1.3 и не входит в Matches13To16.</summary>
    private static Pairing41TestCase C05_TypeIgnored_StillPaired() => new()
    {
        Name = "C05. Поле Type на 1.3 игнорируется — парные.",
        Form13 = [Row13(3, type: "Совсем-другой-тип")],
        Form16 = [Row16From13(303)],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>C06. AMD на 1.3 берётся из даты выпуска и совпадает с 1.6.</summary>
    private static Pairing41TestCase C06_ActivityMeasurementDateFromCreation_Paired() => new()
    {
        Name = "C06. AMD = дата выпуска на 1.3 — парные.",
        Form13 = [Row13(3, creationDate: "2022-11-20")],
        Form16 = [Row16From13(303, activityMeasurementDate: "2022-11-20")],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired16 = []
    };
}
