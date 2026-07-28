using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.Testing;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

internal static class TransferReceiveAssertions
{
    public static void EqualIds(IReadOnlyList<int> expected, IReadOnlyList<int> actual, string context)
    {
        var expectedOrdered = expected.OrderBy(id => id).ToList();
        var actualOrdered = actual.OrderBy(id => id).ToList();
        Assert.True(
            expectedOrdered.SequenceEqual(actualOrdered),
            $"{context}: expected [{string.Join(", ", expectedOrdered)}], actual [{string.Join(", ", actualOrdered)}]");
    }

    public static void EqualScenario(TransferReceiveTestCase testCase, TransferReceiveScenarioResult actual) =>
        EqualIds(testCase.ExpectedUnpairedIds, actual.UnpairedIds, $"{testCase.Name}: unpaired");

    public static void EqualClosestMatches(TransferReceiveTestCase testCase, TransferReceiveClosestMatchResult actual) =>
        AssertPartialFieldMap(testCase.Name, testCase.ExpectedClosest, actual.Closest);

    public static void AssertCaseIsInternallyConsistent(TransferReceiveTestCase testCase)
    {
        var knownIds = testCase.OurOps.Select(row => row.Id).ToHashSet();
        foreach (var id in testCase.ExpectedUnpairedIds)
        {
            Assert.True(
                knownIds.Contains(id),
                $"{testCase.Name}: ExpectedUnpaired Id={id} отсутствует в OurOps");
        }

        Assert.Equal(
            testCase.OurOps.Select(row => row.Id).Distinct().Count(),
            testCase.OurOps.Count);

        Assert.Equal(
            testCase.CounterpartOps.Select(row => row.Id).Distinct().Count(),
            testCase.CounterpartOps.Count);

        if (testCase.ExpectedClosest is null)
        {
            return;
        }

        foreach (var id in testCase.ExpectedClosest.Keys)
        {
            Assert.True(
                testCase.ExpectedUnpairedIds.Contains(id),
                $"{testCase.Name}: ExpectedClosest Id={id} должен быть среди ExpectedUnpairedIds");
        }
    }

    private static void AssertPartialFieldMap(
        string caseName,
        IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>>? expected,
        IReadOnlyDictionary<int, IReadOnlyDictionary<TransferReceiveField, bool>> actual)
    {
        if (expected is null)
        {
            return;
        }

        if (expected.Count == 0)
        {
            Assert.True(
                actual.Count == 0,
                $"{caseName}: ожидалась пустая closest-карта, actual Ids=[{string.Join(", ", actual.Keys)}]");
            return;
        }

        foreach (var (id, expectedFields) in expected)
        {
            Assert.True(
                actual.TryGetValue(id, out var actualFields),
                $"{caseName}: Id={id} — нет closest-match карты");

            foreach (var (field, expectedMatch) in expectedFields)
            {
                Assert.True(
                    actualFields.TryGetValue(field, out var actualMatch),
                    $"{caseName}: Id={id} — нет поля {field}");
                Assert.True(
                    actualMatch == expectedMatch,
                    $"{caseName}: Id={id} поле {field}: expected={expectedMatch}, actual={actualMatch}");
            }
        }
    }
}
