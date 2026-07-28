using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;

namespace Test.TransferReceive;

/// <summary>Группа N — сценарная нормализация: разное написание, пара всё равно находится.</summary>
internal static partial class TransferReceiveTestCases
{
    private static IEnumerable<TransferReceiveTestCase> NormalizationCases()
    {
        yield return N01_FactoryLeadingZeros_Paired();
        yield return N02_RadOrderDifferent_Paired();
        yield return N03_PassportSpaces_Paired();
        yield return N04_TypeLookalike_Paired();
    }

    /// <summary>N01. Заводской номер с ведущими нулями.</summary>
    private static TransferReceiveTestCase N01_FactoryLeadingZeros_Paired() => new()
    {
        Name = "N01. Зав.№ 007 vs 7 — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, facNum: "007")],
        CounterpartOps = [RowReceive(101, facNum: "7")],
        ExpectedUnpairedIds = []
    };

    /// <summary>N02. Радионуклиды в другом порядке.</summary>
    private static TransferReceiveTestCase N02_RadOrderDifferent_Paired() => new()
    {
        Name = "N02. Радионуклиды Cs-137,Co-60 vs Co-60,Cs-137 — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, rads: "Cs-137,Co-60")],
        CounterpartOps = [RowReceive(101, rads: "Co-60,Cs-137")],
        ExpectedUnpairedIds = []
    };

    /// <summary>N03. Пробелы в паспорте.</summary>
    private static TransferReceiveTestCase N03_PassportSpaces_Paired() => new()
    {
        Name = "N03. Паспорт «P-001» vs « P-001 » — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, pasNum: "P-001")],
        CounterpartOps = [RowReceive(101, pasNum: " P-001 ")],
        ExpectedUnpairedIds = []
    };

    /// <summary>N04. Lookalike в типе (если применимо к NormalizeNumber).</summary>
    private static TransferReceiveTestCase N04_TypeLookalike_Paired() => new()
    {
        Name = "N04. Тип в разном регистре — пара.",
        OurOkpo = DefaultOurOkpo,
        OurOps = [RowTransfer(1, type: "ИИИ")],
        CounterpartOps = [RowReceive(101, type: "иии")],
        ExpectedUnpairedIds = []
    };
}
