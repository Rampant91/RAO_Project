using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Client_App.Resources;

namespace Test.Pairing41;

/// <summary>Группа B — сопоставление 1.2 ↔ 1.6 (ИОУ).</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> Form12To16Cases()
    {
        yield return B01_IdealPair_Empty();
        yield return B02_MassFromKgConverted_Paired();
        yield return B03_MassOutsideTolerance_BothSidesUnpaired();
        yield return B04_ActivityMeasurementDateMismatch_Unpaired();
        yield return B05_DocumentAndPackLookalike_Paired();
        yield return B06_CodeRaoMatches_Paired();
        yield return B07_CodeRaoMismatch_BothSidesUnpaired();
    }

    /// <summary>B01. Идеальная пара 1.2↔1.6 → непарных нет.</summary>
    private static Pairing41TestCase B01_IdealPair_Empty() => new()
    {
        Name = "B01. Идеальная пара 1.2↔1.6.",
        Form12 = [Row12(2)],
        Form16 = [Row16From12(202)],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>B02. 1000 кг → 1 т через ToMassTon; на 1.6 масса уже в тоннах.</summary>
    private static Pairing41TestCase B02_MassFromKgConverted_Paired()
    {
        // Как при загрузке 1.2: кг → тонны
        var massTon = Pairing41ScenarioRunner.ToMassTon("1000");
        return new Pairing41TestCase
        {
            Name = "B02. Масса 1000 кг приведена к 1 т — парные.",
            Form12 = [Row12(2, massTon: massTon)],
            Form16 = [Row16From12(202, massTon: "1")],
            ExpectedUnpaired12 = [],
            ExpectedUnpaired16 = []
        };
    }

    /// <summary>B03. Масса 1 т vs 1.2 т (вне ±10%) → оба непарные.</summary>
    private static Pairing41TestCase B03_MassOutsideTolerance_BothSidesUnpaired() => new()
    {
        Name = "B03. Масса вне допуска ±10% — непарные с обеих сторон.",
        Form12 = [Row12(2, massTon: "1")],
        Form16 = [Row16From12(202, massTon: "1.2")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202]
    };

    /// <summary>B04. У 1.2 AMD = дата операции; у 1.6 другая дата измерения → непарные.</summary>
    private static Pairing41TestCase B04_ActivityMeasurementDateMismatch_Unpaired() => new()
    {
        Name = "B04. Дата измерения активности не совпадает — непарные.",
        // opDate совпадает, AMD у 1.6 сдвинута
        Form12 = [Row12(2, opDate: Form12OpDate)],
        Form16 = [Row16From12(202, opDate: Form12OpDate, activityMeasurementDate: "2024-02-10")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202]
    };

    /// <summary>B05. Lookalike RU/EN в номере документа и упаковки → пара.</summary>
    private static Pairing41TestCase B05_DocumentAndPackLookalike_Paired() => new()
    {
        Name = "B05. Lookalike документа и упаковки (СОС/COC, О/O) — парные.",
        Form12 = [Row12(2, documentNumber: "СОС-1", packNumber: "УКТ-О1")],
        Form16 = [Row16From12(202, documentNumber: "COC-1", packNumber: "УКТ-O1")],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>B06. Код РАО совпадает (изделия из ОУ) — парные.</summary>
    private static Pairing41TestCase B06_CodeRaoMatches_Paired() => new()
    {
        Name = "B06. Код РАО совпадает на 1.2 и 1.6 — парные.",
        Form12 = [Row12(2, codeRao: RaoCodeHelper.Form12CodeRao)],
        Form16 = [Row16From12(202, codeRao: RaoCodeHelper.Form12CodeRao)],
        ExpectedUnpaired12 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>B07. Остальные поля совпадают, но код РАО разный — непарные с обеих сторон.</summary>
    private static Pairing41TestCase B07_CodeRaoMismatch_BothSidesUnpaired() => new()
    {
        Name = "B07. Расхождение кода РАО — непарные с обеих сторон (1.2↔1.6).",
        Form12 = [Row12(2, codeRao: RaoCodeHelper.Form12CodeRao)],
        Form16 = [Row16From12(202, codeRao: "00000000000")],
        ExpectedUnpaired12 = [2],
        ExpectedUnpaired16 = [202]
    };
}
