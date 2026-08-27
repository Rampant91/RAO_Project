using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.Testing;
using Client_App.Resources;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

/// <summary>Группа F — сторона 1.6: приоритет 12→13→14 и симметрия.</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> Form16Cases()
    {
        yield return F01_PairedOnlyWith12();
        yield return F02_PairedOnlyWith13And14();
        yield return F03_Prefer12Over13_WhenBothMatch();
        yield return F04_NoRvMatch_Unpaired16();
        yield return F05_MassMismatch_SymmetricUnpaired12And16();
        yield return F06_TwoForm16_OneForm12_SecondUnpaired();
        yield return F07_OneForm16_CannotCloseBoth12And13();
        yield return F08_OneForm16_CannotCloseBoth12And14();
        yield return F09_OneForm16_CannotCloseBoth13And14();
    }

    /// <summary>F01. Одна 1.6 находит пару только в 1.2.</summary>
    private static Pairing41TestCase F01_PairedOnlyWith12() => new()
    {
        Name = "F01. Строка 1.6 парная только с 1.2.",
        Form12 = [Row12(2)],
        Form16 = [Row16From12(202)],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>F02. Две 1.6: одна с 1.3, другая с 1.4.</summary>
    private static Pairing41TestCase F02_PairedOnlyWith13And14() => new()
    {
        Name = "F02. Две строки 1.6 парные с 1.3 и 1.4.",
        Form13 = [Row13(3)],
        Form14 = [Row14(4)],
        Form16 = [Row16From13(303), Row16From14(404)],
        ExpectedUnpaired13 = [],
        ExpectedUnpaired14 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>
    /// F03. Приоритет 12→13: первая 1.6 подходит и под 1.2, и под 1.3 — съедает 1.2;
    /// вторая подходит только под 1.3. При ошибочном 13→12 вторая 1.6 осталась бы непарной.
    /// </summary>
    private static Pairing41TestCase F03_Prefer12Over13_WhenBothMatch()
    {
        const string opDate = "2024-05-01";
        const string docDate = "2024-05-01";
        const string doc = "DOC-X";
        const string pack = "УКТ-X";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "F03. Приоритет 12→13: первая 1.6 берёт 1.2, вторая — оставшуюся 1.3.",
            Form12 =
            [
                Row12(2, opDate: opDate, documentNumber: doc, documentDate: docDate, packNumber: pack,
                    massTon: "1", beta: beta, alpha: "-", activityMeasurementDate: opDate)
            ],
            Form13 =
            [
                Row13(3, opDate: opDate, documentNumber: doc, documentDate: docDate, packNumber: pack,
                    mainRads: "кобальт-60", beta: beta, alpha: "-", creationDate: opDate)
            ],
            Form16 =
            [
                // Id=901: совпадает и с 1.2, и с 1.3 → при приоритете 12 съедает 1.2
                new Pairing41Row
                {
                    Id = 901,
                    OpDate = opDate,
                    Mass = "1",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TritiumActivity = "-",
                    TransuraniumActivity = "-",
                    MainRadionuclids = "кобальт-60",
                    ActivityMeasurementDate = opDate,
                    DocumentVid = 1,
                    DocumentNumber = doc,
                    DocumentDate = docDate,
                    PackName = "Упаковка",
                    PackType = "ТипУКТ",
                    PackNumber = pack,
                    CodeRao = RaoCodeHelper.Form12CodeRao
                },
                // Id=902: масса не подходит под 1.2, подходит под 1.3
                new Pairing41Row
                {
                    Id = 902,
                    OpDate = opDate,
                    Mass = "999",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TritiumActivity = "-",
                    TransuraniumActivity = "-",
                    MainRadionuclids = "кобальт-60",
                    ActivityMeasurementDate = opDate,
                    DocumentVid = 1,
                    DocumentNumber = doc,
                    DocumentDate = docDate,
                    PackName = "Упаковка",
                    PackType = "ТипУКТ",
                    PackNumber = pack,
                    CodeRao = Pairing41TestCases.ExpandedCodeRaoFrom13()
                }
            ],
            ExpectedUnpaired12 = [],
            ExpectedUnpaired13 = [],
            ExpectedUnpaired16 = []
        };
    }

    /// <summary>F04. Номер документа другой — ни 1.2, ни 1.6 не находят пару.</summary>
    private static Pairing41TestCase F04_NoRvMatch_Unpaired16() => new()
    {
        Name = "F04. Нет совпадения с РВ — 1.6 и 1.2 непарные.",
        Form12 = [Row12(2)],
        Form16 = [Row16From12(202, documentNumber: "DOC-OTHER")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202]
    };

    /// <summary>F05. Масса вне допуска — симметрия 1.2 и 1.6.</summary>
    private static Pairing41TestCase F05_MassMismatch_SymmetricUnpaired12And16() => new()
    {
        Name = "F05. Расхождение массы — симметрично непарные 1.2 и 1.6.",
        Form12 = [Row12(2, massTon: "1")],
        Form16 = [Row16From12(202, massTon: "1.2")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202]
    };

    /// <summary>F06. Две одинаковые 1.6 и одна 1.2 — вторая 1.6 без пары.</summary>
    private static Pairing41TestCase F06_TwoForm16_OneForm12_SecondUnpaired() => new()
    {
        Name = "F06. Две строки 1.6 на одну 1.2 — вторая 1.6 непарная.",
        // 201 съедает единственную 1.2; 202 остаётся
        Form12 = [Row12(2)],
        Form16 = [Row16From12(201), Row16From12(202)],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = [202]
    };

    /// <summary>
    /// F07. Одна 1.6 подходит и под 1.2, и под 1.3 — закрывает только 1.2 (приоритет 12→13).
    /// 1.3 остаётся непарной; 1.6 парная. Регрессия раздельных used[] по пулу 1.6.
    /// </summary>
    private static Pairing41TestCase F07_OneForm16_CannotCloseBoth12And13()
    {
        const string opDate = "2024-05-01";
        const string docDate = "2024-05-01";
        const string doc = "DOC-SHARE";
        const string pack = "УКТ-SHARE";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "F07. Одна 1.6 не закрывает одновременно 1.2 и 1.3 — 1.3 непарная.",
            Form12 =
            [
                Row12(2, opDate: opDate, documentNumber: doc, documentDate: docDate, packNumber: pack,
                    massTon: "1", beta: beta, alpha: "-", activityMeasurementDate: opDate)
            ],
            Form13 =
            [
                Row13(3, opDate: opDate, documentNumber: doc, documentDate: docDate, packNumber: pack,
                    mainRads: "кобальт-60", beta: beta, alpha: "-", creationDate: opDate)
            ],
            Form16 =
            [
                new Pairing41Row
                {
                    Id = 901,
                    OpDate = opDate,
                    Mass = "1",
                    BetaGammaActivity = beta,
                    AlphaActivity = "-",
                    TritiumActivity = "-",
                    TransuraniumActivity = "-",
                    MainRadionuclids = "кобальт-60",
                    ActivityMeasurementDate = opDate,
                    DocumentVid = 1,
                    DocumentNumber = doc,
                    DocumentDate = docDate,
                    PackName = "Упаковка",
                    PackType = "ТипУКТ",
                    PackNumber = pack,
                    CodeRao = RaoCodeHelper.Form12CodeRao
                }
            ],
            ExpectedUnpaired12 = [],
            ExpectedUnpaired13 = [3],
            ExpectedUnpaired16 = []
        };
    }

    /// <summary>
    /// F08. Одна 1.6 подходит под 1.2 и 1.4 — закрывает только 1.2; 1.4 непарная.
    /// </summary>
    private static Pairing41TestCase F08_OneForm16_CannotCloseBoth12And14()
    {
        const string opDate = "2024-06-01";
        const string doc = "DOC-F08";
        const string pack = "УКТ-F08";
        const string beta = "1.0e+06";

        return new Pairing41TestCase
        {
            Name = "F08. Одна 1.6 не закрывает одновременно 1.2 и 1.4 — 1.4 непарная.",
            Form12 =
            [
                Row12(2, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    massTon: "1", beta: beta, alpha: "-", activityMeasurementDate: opDate)
            ],
            Form14 =
            [
                Row14(4, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    massTon: "1", volume: "0.5", mainRads: "кобальт-60", beta: beta, alpha: "-",
                    activityMeasurementDate: opDate)
            ],
            Form16 =
            [
                new Pairing41Row
                {
                    Id = 904,
                    OpDate = opDate,
                    Volume = "0.5",
                    Mass = "1",
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
                    PackNumber = pack,
                    CodeRao = RaoCodeHelper.Form12CodeRao
                }
            ],
            ExpectedUnpaired12 = [],
            ExpectedUnpaired14 = [4],
            ExpectedUnpaired16 = []
        };
    }

    /// <summary>
    /// F09. Нет 1.2: одна 1.6 подходит под 1.3 и 1.4 — закрывает только 1.3; 1.4 непарная.
    /// </summary>
    private static Pairing41TestCase F09_OneForm16_CannotCloseBoth13And14()
    {
        const string opDate = "2024-07-01";
        const string doc = "DOC-F09";
        const string pack = "УКТ-F09";
        const string beta = "2.0e+06";

        return new Pairing41TestCase
        {
            Name = "F09. Одна 1.6 не закрывает одновременно 1.3 и 1.4 — 1.4 непарная.",
            Form13 =
            [
                Row13(3, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    mainRads: "кобальт-60", beta: beta, alpha: "-", creationDate: opDate)
            ],
            Form14 =
            [
                Row14(4, opDate: opDate, documentNumber: doc, documentDate: opDate, packNumber: pack,
                    massTon: "1", volume: "0.4", mainRads: "кобальт-60", beta: beta, alpha: "-",
                    activityMeasurementDate: opDate)
            ],
            Form16 =
            [
                new Pairing41Row
                {
                    Id = 905,
                    OpDate = opDate,
                    Volume = "0.4",
                    Mass = "1",
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
                    PackNumber = pack,
                    CodeRao = Pairing41TestCases.ExpandedCodeRaoFrom13()
                }
            ],
            ExpectedUnpaired13 = [],
            ExpectedUnpaired14 = [4],
            ExpectedUnpaired16 = []
        };
    }
}
