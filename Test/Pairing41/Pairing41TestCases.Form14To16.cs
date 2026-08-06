using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;

namespace Test.Pairing41;

/// <summary>Группа D — сопоставление 1.4 ↔ 1.6 (ОРИ-прочие).</summary>
internal static partial class Pairing41TestCases
{
    private static IEnumerable<Pairing41TestCase> Form14To16Cases()
    {
        yield return D01_IdealPair_Empty();
        yield return D02_VolumeOutsideTolerance_BothSidesUnpaired();
        yield return D03_MassFromKgConverted_Paired();
        yield return D04_MassOutsideTolerance_SymmetricUnpaired();
        yield return D05_CodeRaoMismatch_BothSidesUnpaired();
    }

    /// <summary>D01. Идеальная пара 1.4↔1.6 → непарных нет.</summary>
    private static Pairing41TestCase D01_IdealPair_Empty() => new()
    {
        Name = "D01. Идеальная пара 1.4↔1.6.",
        Form14 = [Row14(4)],
        Form16 = [Row16From14(404)],
        ExpectedUnpaired14 = [],
        ExpectedUnpaired16 = []
    };

    /// <summary>D02. Объём 2 vs 2.5 (вне ±10%) → оба непарные.</summary>
    private static Pairing41TestCase D02_VolumeOutsideTolerance_BothSidesUnpaired() => new()
    {
        Name = "D02. Объём вне допуска ±10% — непарные с обеих сторон.",
        Form14 = [Row14(4, volume: "2")],
        Form16 = [Row16From14(404, volume: "2.5")],
        ExpectedUnpaired14 = [4],
        ExpectedUnpaired16 = [404]
    };

    /// <summary>D03. 500 кг → 0.5 т; на 1.6 масса уже в тоннах.</summary>
    private static Pairing41TestCase D03_MassFromKgConverted_Paired()
    {
        var massTon = Pairing41ScenarioRunner.ToMassTon("500");
        return new Pairing41TestCase
        {
            Name = "D03. Масса 500 кг приведена к 0.5 т — парные.",
            Form14 = [Row14(4, massTon: massTon)],
            Form16 = [Row16From14(404, massTon: "0.5")],
            ExpectedUnpaired14 = [],
            ExpectedUnpaired16 = []
        };
    }

    /// <summary>D04. Масса 0.5 vs 0.7 (вне ±10%) → симметрично непарные.</summary>
    private static Pairing41TestCase D04_MassOutsideTolerance_SymmetricUnpaired() => new()
    {
        Name = "D04. Масса вне допуска — непарные на 1.4 и 1.6.",
        Form14 = [Row14(4, massTon: "0.5")],
        Form16 = [Row16From14(404, massTon: "0.7")],
        ExpectedUnpaired14 = [4],
        ExpectedUnpaired16 = [404]
    };

    /// <summary>D05. Остальные поля совпадают, но код РАО разный — непарные с обеих сторон (1.4↔1.6).</summary>
    private static Pairing41TestCase D05_CodeRaoMismatch_BothSidesUnpaired() => new()
    {
        Name = "D05. Расхождение кода РАО — непарные с обеих сторон (1.4↔1.6).",
        Form14 = [Row14(4, codeRao: "1__1__00__")],
        Form16 = [Row16From14(404, codeRao: "2__1__00__")],
        ExpectedUnpaired14 = [4],
        ExpectedUnpaired16 = [404]
    };
}
