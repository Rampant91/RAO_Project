using System;
using Client_App.Services.Updates;
using Xunit;

namespace Test.Updates;

public class UpdateAutoPromptPolicyTests
{
    private static readonly DateTime Now = new(2026, 8, 27, 14, 0, 0);

    [Fact]
    public void NewReleaseId_IsNotSuppressed_EvenIfCheckedToday()
    {
        var suppress = UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            availableKey: "12_fix",
            lastNotifiedKey: "11_morning",
            lastNotifiedAt: Now.AddHours(-3),
            now: Now);

        Assert.False(suppress);
    }

    [Fact]
    public void SameReleaseId_WithinDay_IsSuppressed()
    {
        var suppress = UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            availableKey: "11_morning",
            lastNotifiedKey: "11_morning",
            lastNotifiedAt: Now.AddHours(-3),
            now: Now);

        Assert.True(suppress);
    }

    [Fact]
    public void SameReleaseId_AfterDay_IsNotSuppressed()
    {
        var suppress = UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            availableKey: "11_morning",
            lastNotifiedKey: "11_morning",
            lastNotifiedAt: Now.AddHours(-25),
            now: Now);

        Assert.False(suppress);
    }

    [Fact]
    public void NeverNotified_IsNotSuppressed()
    {
        var suppress = UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            availableKey: "11_morning",
            lastNotifiedKey: null,
            lastNotifiedAt: Now.AddHours(-1),
            now: Now);

        Assert.False(suppress);
    }

    [Fact]
    public void ReleaseIdComparison_IsCaseInsensitive()
    {
        var suppress = UpdateAutoPromptPolicy.ShouldSuppressAutoPrompt(
            availableKey: "11_Morning",
            lastNotifiedKey: "11_morning",
            lastNotifiedAt: Now.AddMinutes(-10),
            now: Now);

        Assert.True(suppress);
    }
}
