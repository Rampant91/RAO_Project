using Client_App.Commands.AsyncCommands.ExcelExport.ParingOfCode41.Testing;
using Xunit;

namespace Test.Pairing41;

/// <summary>
/// Сценарные тесты парности 41 (без БД/UI/Excel).
/// Эталоны задаются вручную в <see cref="Pairing41TestCases"/>.
/// </summary>
public class Pairing41ScenarioTests
{
    [Theory]
    [MemberData(nameof(Pairing41TestCases.All), MemberType = typeof(Pairing41TestCases))]
    public void TestCase_IsInternallyConsistent(string name, Pairing41TestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        Pairing41Assertions.AssertCaseIsInternallyConsistent(testCase);
    }

    [Theory]
    [MemberData(nameof(Pairing41TestCases.All), MemberType = typeof(Pairing41TestCases))]
    public void Scenario_MatchesExpectedUnpairedIds(string name, Pairing41TestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var actual = Pairing41ScenarioRunner.Run(testCase);
        Pairing41Assertions.EqualScenario(testCase, actual);
    }

    [Theory]
    [MemberData(nameof(Pairing41TestCases.ClosestMatchOnly), MemberType = typeof(Pairing41TestCases))]
    public void ClosestMatch_MatchesExpectedFieldMaps(string name, Pairing41TestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var actual = Pairing41ScenarioRunner.RunClosestMatches(testCase);
        Pairing41Assertions.EqualClosestMatches(testCase, actual);
    }
}
