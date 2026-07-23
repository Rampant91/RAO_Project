using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Xunit;

namespace Test.Pairing41;

/// <summary>
/// Регрессии масштаба и детерминизма общего ядра <c>ComputeOrganizationUnpaired</c>
/// (то же, что используют org- и whole-DB режимы выгрузки).
/// Полное покрытие эталонов — в <see cref="Pairing41ScenarioTests"/>.
/// </summary>
public class Pairing41ModeEquivalenceTests
{
    [Theory]
    [MemberData(nameof(Pairing41TestCases.All), MemberType = typeof(Pairing41TestCases))]
    public void SharedUnpairedPipeline_IsDeterministic(string name, Pairing41TestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var first = Pairing41ScenarioRunner.Run(testCase);
        var second = Pairing41ScenarioRunner.Run(testCase);
        Pairing41Assertions.EqualIds(first.Unpaired11, second.Unpaired11, $"{name}: Form11");
        Pairing41Assertions.EqualIds(first.Unpaired12, second.Unpaired12, $"{name}: Form12");
        Pairing41Assertions.EqualIds(first.Unpaired13, second.Unpaired13, $"{name}: Form13");
        Pairing41Assertions.EqualIds(first.Unpaired14, second.Unpaired14, $"{name}: Form14");
        Pairing41Assertions.EqualIds(first.Unpaired15, second.Unpaired15, $"{name}: Form15");
        Pairing41Assertions.EqualIds(first.Unpaired16, second.Unpaired16, $"{name}: Form16");
    }

    /// <summary>
    /// Регрессия бакетов по дате: много строк с одной OpDate должны склеиваться жадно.
    /// </summary>
    [Fact]
    public void LargeSameOpDatePool_12To16_AllPair()
    {
        const int n = 400;
        var form12 = new List<Pairing41Row>(n);
        var form16 = new List<Pairing41Row>(n);
        for (var i = 0; i < n; i++)
        {
            var mass = (1 + i % 7).ToString(System.Globalization.CultureInfo.InvariantCulture);
            form12.Add(Pairing41TestCases.CreateRow12ForScale(id: i + 1, massTon: mass));
            form16.Add(Pairing41TestCases.CreateRow16From12ForScale(id: 10_000 + i, massTon: mass));
        }

        var testCase = new Pairing41TestCase
        {
            Name = "SCALE. 400 пар 1.2↔1.6 с одной датой — все парные.",
            Form12 = form12,
            Form16 = form16,
            ExpectedUnpaired12 = [],
            ExpectedUnpaired16 = []
        };

        var actual = Pairing41ScenarioRunner.Run(testCase);
        Pairing41Assertions.EqualScenario(testCase, actual);
    }

    [Fact]
    public void LargeSameOpDatePool_12To16_OneExtraOnEachSide()
    {
        const int n = 200;
        var form12 = new List<Pairing41Row>(n + 1);
        var form16 = new List<Pairing41Row>(n + 1);
        for (var i = 0; i < n; i++)
        {
            form12.Add(Pairing41TestCases.CreateRow12ForScale(id: i + 1, massTon: "1"));
            form16.Add(Pairing41TestCases.CreateRow16From12ForScale(id: 10_000 + i, massTon: "1"));
        }

        form12.Add(Pairing41TestCases.CreateRow12ForScale(id: 9_001, massTon: "2"));
        form16.Add(Pairing41TestCases.CreateRow16From12ForScale(id: 19_001, massTon: "3"));

        var testCase = new Pairing41TestCase
        {
            Name = "SCALE. 200 пар + по одной лишней стороне.",
            Form12 = form12,
            Form16 = form16,
            ExpectedUnpaired12 = [9_001],
            ExpectedUnpaired16 = [19_001]
        };

        var actual = Pairing41ScenarioRunner.Run(testCase);
        Pairing41Assertions.EqualScenario(testCase, actual);
    }
}
