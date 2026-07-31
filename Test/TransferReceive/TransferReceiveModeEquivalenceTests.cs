using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;

namespace Test.TransferReceive;

/// <summary>
/// Equivalence org ↔ All: то же ядро <c>AnalyzeForm11ForOrganization</c>;
/// org передаёт пул контрагентов, All — полный merged-пул (свои + чужие, дедуп по Id).
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
    /// Org-пул контрагентов ≡ All-стиль (наши + контрагенты в одном пуле; ядро дедупит по Id).
    /// </summary>
    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.All), MemberType = typeof(TransferReceiveTestCases))]
    public void OrgCounterpartPool_EqualsMergedFullPool(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var orgStyle = TransferReceiveScenarioRunner.Run(testCase);

        var allStyleCase = CloneWithCounterpartPool(
            testCase,
            testCase.OurOps.Concat(testCase.CounterpartOps).ToList());
        var allStyle = TransferReceiveScenarioRunner.Run(allStyleCase);

        TransferReceiveAssertions.EqualIds(
            orgStyle.UnpairedIds,
            allStyle.UnpairedIds,
            $"{name}: org ↔ All (merged pool)");
    }

    /// <summary>
    /// Shared pool (whole-DB once) ≡ обычный Analyze с counterpart-пулом.
    /// </summary>
    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.All), MemberType = typeof(TransferReceiveTestCases))]
    public void SharedFullPool_EqualsOrgAnalyze(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var orgStyle = TransferReceiveScenarioRunner.Run(testCase);
        var shared = TransferReceiveScenarioRunner.RunWithSharedFullPool(testCase);
        TransferReceiveAssertions.EqualIds(
            orgStyle.UnpairedIds,
            shared.UnpairedIds,
            $"{name}: shared pool ↔ org Analyze");
    }

    /// <summary>
    /// Closest: org-пул ≡ merged full-пул (как при bulk All).
    /// </summary>
    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.ClosestMatchOnly), MemberType = typeof(TransferReceiveTestCases))]
    public void OrgCounterpartPool_EqualsMergedFullPool_Closest(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var orgStyle = TransferReceiveScenarioRunner.RunClosestMatches(testCase);

        var allStyleCase = CloneWithCounterpartPool(
            testCase,
            testCase.OurOps.Concat(testCase.CounterpartOps).ToList());
        var allStyle = TransferReceiveScenarioRunner.RunClosestMatches(allStyleCase);

        Assert.Equal(
            orgStyle.Closest.Keys.OrderBy(id => id),
            allStyle.Closest.Keys.OrderBy(id => id));
        foreach (var id in orgStyle.Closest.Keys)
        {
            Assert.Equal(
                orgStyle.Closest[id].OrderBy(kv => kv.Key).Select(kv => (kv.Key, kv.Value)),
                allStyle.Closest[id].OrderBy(kv => kv.Key).Select(kv => (kv.Key, kv.Value)));
        }
    }

    /// <summary>
    /// Лишние ops чужой org (другой ОКПО) в full-пуле не меняют результат выбранной org.
    /// </summary>
    [Fact]
    public void FullPoolWithUnrelatedNoise_SameAsOrgPool()
    {
        var baseCase = new TransferReceiveTestCase
        {
            Name = "EQ. Noise org в full-пуле — результат как у org-пула.",
            OurOkpo = "10000001",
            OurOps =
            [
                TransferReceiveTestCases.CreateTransferForScale(1)
            ],
            CounterpartOps =
            [
                TransferReceiveTestCases.CreateReceiveForScale(101, pairTransferId: 1)
            ],
            ExpectedUnpairedIds = []
        };

        var noise = new List<TransferReceiveRow>
        {
            new()
            {
                Id = 9001,
                RepsId = 99,
                OrgOkpo = "99999999",
                OpCode = "21",
                OpDate = "2024-06-15",
                PasNum = "P-9001",
                FacNum = "F-9001",
                Type = "ИИИ",
                Radionuclids = "Cs-137",
                ProviderOrRecieverOkpo = "88888888",
                Activity = "1.0e+6",
                CreatorOkpo = "99999999",
                CreationDate = "2020-01-01",
                Quantity = 1,
                IsTransfer = true
            },
            new()
            {
                Id = 9002,
                RepsId = 98,
                OrgOkpo = "88888888",
                OpCode = "31",
                OpDate = "2024-06-15",
                PasNum = "P-9001",
                FacNum = "F-9001",
                Type = "ИИИ",
                Radionuclids = "Cs-137",
                ProviderOrRecieverOkpo = "99999999",
                Activity = "1.0e+6",
                CreatorOkpo = "88888888",
                CreationDate = "2020-01-01",
                Quantity = 1,
                IsTransfer = false
            }
        };

        var orgStyle = TransferReceiveScenarioRunner.Run(baseCase);
        var allStyle = TransferReceiveScenarioRunner.Run(CloneWithCounterpartPool(
            baseCase,
            baseCase.OurOps.Concat(baseCase.CounterpartOps).Concat(noise).ToList()));

        TransferReceiveAssertions.EqualIds(orgStyle.UnpairedIds, allStyle.UnpairedIds, "noise pool");
        TransferReceiveAssertions.EqualScenario(baseCase, allStyle);
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

    private static TransferReceiveTestCase CloneWithCounterpartPool(
        TransferReceiveTestCase source,
        IReadOnlyList<TransferReceiveRow> counterpartOps) =>
        new()
        {
            Name = source.Name,
            FormNum = source.FormNum,
            Params = source.Params,
            OurOkpo = source.OurOkpo,
            OurOps = source.OurOps,
            CounterpartOps = counterpartOps,
            OkpoAliases = source.OkpoAliases,
            ExpectedUnpairedIds = source.ExpectedUnpairedIds,
            ExpectedClosest = source.ExpectedClosest,
            ExpectedClosestLevels = source.ExpectedClosestLevels,
            ExpectedClosestCandidateIds = source.ExpectedClosestCandidateIds,
            ExpectedConfidenceMinPercent = source.ExpectedConfidenceMinPercent
        };
}
