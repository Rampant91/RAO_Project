using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.Testing;
using Xunit;

namespace Test.TransferReceive;

/// <summary>
/// Сценарные тесты приёма-передачи формы 1.1 (без БД/UI/Excel).
/// Эталоны задаются вручную в <see cref="TransferReceiveTestCases"/>.
/// </summary>
public class TransferReceiveScenarioTests
{
    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.All), MemberType = typeof(TransferReceiveTestCases))]
    public void TestCase_IsInternallyConsistent(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        TransferReceiveAssertions.AssertCaseIsInternallyConsistent(testCase);
    }

    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.All), MemberType = typeof(TransferReceiveTestCases))]
    public void Scenario_MatchesExpectedUnpairedIds(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var actual = TransferReceiveScenarioRunner.Run(testCase);
        TransferReceiveAssertions.EqualScenario(testCase, actual);
    }

    [Theory]
    [MemberData(nameof(TransferReceiveTestCases.ClosestMatchOnly), MemberType = typeof(TransferReceiveTestCases))]
    public void ClosestMatch_MatchesExpectedFieldMaps(string name, TransferReceiveTestCase testCase)
    {
        Assert.Equal(name, testCase.Name);
        var actual = TransferReceiveScenarioRunner.RunClosestMatches(testCase);
        TransferReceiveAssertions.EqualClosestMatches(testCase, actual);
    }
}
