using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;

namespace Test.TransferReceive;

/// <summary>
/// Регрессии масштаба и детерминизма общего ядра <c>AnalyzeForm11ForOrganization</c>
/// (то же, что будут использовать org- и whole-DB режимы выгрузки).
/// Полное покрытие эталонов — в <see cref="TransferReceiveScenarioTests"/>.
/// </summary>
public class TransferReceiveModeEquivalenceTests
{
    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.All), MemberType = typeof(TransferReceiveTestCases))]
    public void SharedUnpairedPipeline_IsDeterministic(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var first = TransferReceiveScenarioRunner.Run(testCase);
        var second = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualIds(first.UnpairedIds, second.UnpairedIds, $"{name}: unpaired");
    }

    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.ClosestMatchOnly), MemberType = typeof(TransferReceiveTestCases))]
    public void SharedClosestPipeline_IsDeterministic(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var first = TransferReceiveScenarioRunner.RunClosestMatches(testCase);
        var second = TransferReceiveScenarioRunner.RunClosestMatches(testCase);
        TransferReceiveAssertions.EqualClosestMatches(testCase, first);
        TransferReceiveAssertions.EqualClosestMatches(testCase, second);
        Assert.Equal(first.Closest.Keys.OrderBy(id => id), second.Closest.Keys.OrderBy(id => id));
    }

    /// <summary>
    /// Много идеальных пар с одной OpDate — все должны склеиться (жадное сопоставление).
    /// </summary>
    [Fact]
    public void LargeSameOpDatePool_AllPairs()
    {
        const int n = 300;
        var ourOps = new List<TransferReceiveRow>(n);
        var counterpartOps = new List<TransferReceiveRow>(n);
        for (var i = 0; i < n; i++)
        {
            var transferId = i + 1;
            ourOps.Add(TransferReceiveTestCases.CreateTransferForScale(transferId));
            counterpartOps.Add(TransferReceiveTestCases.CreateReceiveForScale(10_000 + i, transferId));
        }

        var testCase = new TransferReceiveTestCase
        {
            Name = "SCALE. 300 пар 21↔31 с одной датой — все парные.",
            OurOkpo = "10000001",
            OurOps = ourOps,
            CounterpartOps = counterpartOps,
            ExpectedUnpairedIds = []
        };

        var actual = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualScenario(testCase, actual);
    }

    [Fact]
    public void LargeSameOpDatePool_OneExtraTransferUnpaired()
    {
        const int n = 200;
        var ourOps = new List<TransferReceiveRow>(n + 1);
        var counterpartOps = new List<TransferReceiveRow>(n);
        for (var i = 0; i < n; i++)
        {
            var transferId = i + 1;
            ourOps.Add(TransferReceiveTestCases.CreateTransferForScale(transferId));
            counterpartOps.Add(TransferReceiveTestCases.CreateReceiveForScale(10_000 + i, transferId));
        }

        ourOps.Add(TransferReceiveTestCases.CreateTransferForScale(9_001));

        var testCase = new TransferReceiveTestCase
        {
            Name = "SCALE. 200 пар + одна лишняя передача.",
            OurOkpo = "10000001",
            OurOps = ourOps,
            CounterpartOps = counterpartOps,
            ExpectedUnpairedIds = [9_001]
        };

        var actual = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualScenario(testCase, actual);
    }

    /// <summary>
    /// Безсерийные: один source qty=N закрывается N кандидатами qty=1.
    /// </summary>
    [Fact]
    public void LargeEmptySerialQtyDrain_AllPaired()
    {
        const int n = 150;
        var counterpartOps = new List<TransferReceiveRow>(n);
        for (var i = 0; i < n; i++)
        {
            counterpartOps.Add(TransferReceiveTestCases.CreateEmptySerialReceiveForScale(10_000 + i, quantity: 1));
        }

        var testCase = new TransferReceiveTestCase
        {
            Name = "SCALE. Пустые серии: qty N закрывается N×1 — пара.",
            OurOkpo = "10000001",
            OurOps = [TransferReceiveTestCases.CreateEmptySerialTransferForScale(1, quantity: n)],
            CounterpartOps = counterpartOps,
            ExpectedUnpairedIds = []
        };

        var actual = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualScenario(testCase, actual);
    }

    /// <summary>
    /// Безсерийные: source qty=N+1 при N кандидатах qty=1 → unpaired.
    /// </summary>
    [Fact]
    public void LargeEmptySerialQtyDrain_RemainderUnpaired()
    {
        const int n = 120;
        var counterpartOps = new List<TransferReceiveRow>(n);
        for (var i = 0; i < n; i++)
        {
            counterpartOps.Add(TransferReceiveTestCases.CreateEmptySerialReceiveForScale(10_000 + i, quantity: 1));
        }

        var testCase = new TransferReceiveTestCase
        {
            Name = "SCALE. Пустые серии: qty N+1 при N кандидатах — остаток в отчёте.",
            OurOkpo = "10000001",
            OurOps = [TransferReceiveTestCases.CreateEmptySerialTransferForScale(1, quantity: n + 1)],
            CounterpartOps = counterpartOps,
            ExpectedUnpairedIds = [1]
        };

        var actual = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualScenario(testCase, actual);
    }
}
