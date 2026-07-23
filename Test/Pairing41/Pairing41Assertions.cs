using System;
using System.Collections.Generic;
using System.Linq;
using Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.Testing;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.PairingOfCode41.ExcelExportCheckPairingOfCode41AsyncCommand;

namespace Test.Pairing41;

internal static class Pairing41Assertions
{
    public static void EqualIds(IReadOnlyList<int> expected, IReadOnlyList<int> actual, string context)
    {
        var expectedOrdered = expected.OrderBy(id => id).ToList();
        var actualOrdered = actual.OrderBy(id => id).ToList();
        Assert.True(
            expectedOrdered.SequenceEqual(actualOrdered),
            $"{context}: expected [{string.Join(", ", expectedOrdered)}], actual [{string.Join(", ", actualOrdered)}]");
    }

    public static void EqualScenario(Pairing41TestCase testCase, Pairing41ScenarioResult actual)
    {
        EqualIds(testCase.ExpectedUnpaired11, actual.Unpaired11, $"{testCase.Name}: Form11");
        EqualIds(testCase.ExpectedUnpaired12, actual.Unpaired12, $"{testCase.Name}: Form12");
        EqualIds(testCase.ExpectedUnpaired13, actual.Unpaired13, $"{testCase.Name}: Form13");
        EqualIds(testCase.ExpectedUnpaired14, actual.Unpaired14, $"{testCase.Name}: Form14");
        EqualIds(testCase.ExpectedUnpaired15, actual.Unpaired15, $"{testCase.Name}: Form15");
        EqualIds(testCase.ExpectedUnpaired16, actual.Unpaired16, $"{testCase.Name}: Form16");
    }

    public static void EqualClosestMatches(Pairing41TestCase testCase, Pairing41ClosestMatchResult actual)
    {
        AssertPartialFieldMap(testCase.Name, "Form11", testCase.ExpectedClosest11, actual.Closest11);
        AssertPartialFieldMap(testCase.Name, "Form15", testCase.ExpectedClosest15, actual.Closest15);
        AssertPartialFieldMap(testCase.Name, "Form12", testCase.ExpectedClosest12, actual.Closest12);
        AssertPartialFieldMap(testCase.Name, "Form13", testCase.ExpectedClosest13, actual.Closest13);
        AssertPartialFieldMap(testCase.Name, "Form14", testCase.ExpectedClosest14, actual.Closest14);
        AssertForm16Closest(testCase.Name, testCase.ExpectedClosest16, actual.Closest16);
    }

    public static void AssertCaseIsInternallyConsistent(Pairing41TestCase testCase)
    {
        AssertKnownIds(testCase.Name, "Form11", testCase.Form11, testCase.ExpectedUnpaired11);
        AssertKnownIds(testCase.Name, "Form12", testCase.Form12, testCase.ExpectedUnpaired12);
        AssertKnownIds(testCase.Name, "Form13", testCase.Form13, testCase.ExpectedUnpaired13);
        AssertKnownIds(testCase.Name, "Form14", testCase.Form14, testCase.ExpectedUnpaired14);
        AssertKnownIds(testCase.Name, "Form15", testCase.Form15, testCase.ExpectedUnpaired15);
        AssertKnownIds(testCase.Name, "Form16", testCase.Form16, testCase.ExpectedUnpaired16);

        AssertNoDuplicateIds($"{testCase.Name}: Form11", testCase.Form11);
        AssertNoDuplicateIds($"{testCase.Name}: Form12", testCase.Form12);
        AssertNoDuplicateIds($"{testCase.Name}: Form13", testCase.Form13);
        AssertNoDuplicateIds($"{testCase.Name}: Form14", testCase.Form14);
        AssertNoDuplicateIds($"{testCase.Name}: Form15", testCase.Form15);
        AssertNoDuplicateIds($"{testCase.Name}: Form16", testCase.Form16);

        AssertClosestExpectationIds(testCase.Name, "Form11", testCase.ExpectedClosest11?.Keys, testCase.ExpectedUnpaired11);
        AssertClosestExpectationIds(testCase.Name, "Form15", testCase.ExpectedClosest15?.Keys, testCase.ExpectedUnpaired15);
        AssertClosestExpectationIds(testCase.Name, "Form12", testCase.ExpectedClosest12?.Keys, testCase.ExpectedUnpaired12);
        AssertClosestExpectationIds(testCase.Name, "Form13", testCase.ExpectedClosest13?.Keys, testCase.ExpectedUnpaired13);
        AssertClosestExpectationIds(testCase.Name, "Form14", testCase.ExpectedClosest14?.Keys, testCase.ExpectedUnpaired14);
        AssertClosestExpectationIds(testCase.Name, "Form16", testCase.ExpectedClosest16?.Keys, testCase.ExpectedUnpaired16);
    }

    private static void AssertPartialFieldMap<TField>(
        string caseName,
        string formLabel,
        IReadOnlyDictionary<int, IReadOnlyDictionary<TField, bool>>? expected,
        IReadOnlyDictionary<int, IReadOnlyDictionary<TField, bool>> actual)
        where TField : struct, Enum
    {
        if (expected is null)
        {
            return;
        }

        if (expected.Count == 0)
        {
            Assert.True(
                actual.Count == 0,
                $"{caseName}: {formLabel} — ожидалась пустая closest-карта, actual Ids=[{string.Join(", ", actual.Keys)}]");
            return;
        }

        foreach (var (id, expectedFields) in expected)
        {
            Assert.True(
                actual.TryGetValue(id, out var actualFields),
                $"{caseName}: {formLabel} Id={id} — нет closest-match карты");

            foreach (var (field, expectedMatch) in expectedFields)
            {
                Assert.True(
                    actualFields.TryGetValue(field, out var actualMatch),
                    $"{caseName}: {formLabel} Id={id} — нет поля {field}");
                Assert.True(
                    actualMatch == expectedMatch,
                    $"{caseName}: {formLabel} Id={id} поле {field}: expected={expectedMatch}, actual={actualMatch}");
            }
        }
    }

    private static void AssertForm16Closest(
        string caseName,
        IReadOnlyDictionary<int, Pairing41Form16ClosestExpectation>? expected,
        IReadOnlyDictionary<int, Form16ClosestMatchHighlight> actual)
    {
        if (expected is null)
        {
            return;
        }

        if (expected.Count == 0)
        {
            Assert.True(
                actual.Count == 0,
                $"{caseName}: Form16 — ожидалась пустая closest-карта, actual Ids=[{string.Join(", ", actual.Keys)}]");
            return;
        }

        foreach (var (id, expectedItem) in expected)
        {
            Assert.True(
                actual.TryGetValue(id, out var actualItem),
                $"{caseName}: Form16 Id={id} — нет closest-match");
            Assert.Equal(expectedItem.Profile, actualItem.Profile);

            if (expectedItem.Matches12 is not null)
            {
                Assert.NotNull(actualItem.Matches12);
                foreach (var (field, expectedMatch) in expectedItem.Matches12)
                {
                    Assert.True(actualItem.Matches12!.TryGetValue(field, out var actualMatch));
                    Assert.Equal(expectedMatch, actualMatch);
                }
            }

            if (expectedItem.Matches13 is not null)
            {
                Assert.NotNull(actualItem.Matches13);
                foreach (var (field, expectedMatch) in expectedItem.Matches13)
                {
                    Assert.True(actualItem.Matches13!.TryGetValue(field, out var actualMatch));
                    Assert.Equal(expectedMatch, actualMatch);
                }
            }

            if (expectedItem.Matches14 is not null)
            {
                Assert.NotNull(actualItem.Matches14);
                foreach (var (field, expectedMatch) in expectedItem.Matches14)
                {
                    Assert.True(actualItem.Matches14!.TryGetValue(field, out var actualMatch));
                    Assert.Equal(expectedMatch, actualMatch);
                }
            }
        }
    }

    private static void AssertClosestExpectationIds(
        string caseName, string formLabel, IEnumerable<int>? expectedClosestIds, IReadOnlyList<int> expectedUnpaired)
    {
        if (expectedClosestIds is null)
        {
            return;
        }

        var unpaired = expectedUnpaired.ToHashSet();
        foreach (var id in expectedClosestIds)
        {
            Assert.True(
                unpaired.Contains(id),
                $"{caseName}: ExpectedClosest {formLabel} Id={id} должен быть среди ExpectedUnpaired");
        }
    }

    private static void AssertKnownIds(
        string caseName, string formLabel, IReadOnlyList<Pairing41Row> rows, IReadOnlyList<int> expectedUnpaired)
    {
        var known = rows.Select(row => row.Id).ToHashSet();
        foreach (var id in expectedUnpaired)
        {
            Assert.True(known.Contains(id), $"{caseName}: expected unpaired {formLabel} Id={id} отсутствует во входных строках");
        }
    }

    private static void AssertNoDuplicateIds(string context, IReadOnlyList<Pairing41Row> rows)
    {
        var duplicates = rows.GroupBy(row => row.Id).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
        Assert.True(duplicates.Count == 0, $"{context}: дубликаты Id [{string.Join(", ", duplicates)}]");
    }
}
