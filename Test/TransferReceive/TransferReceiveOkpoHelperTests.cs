using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Pairing.Shared;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.Pairing.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Хелперы ОКПО без БД: SQL-variants, seed титулов, 8 ↔ 8_5.
/// </summary>
public sealed class TransferReceiveOkpoHelperTests
{
    [Fact]
    public void BuildOurOkpoSqlMatchVariants_IncludesRawNormAndLeadingZeroPads()
    {
        var variants = TransferReceiveTestAccess.BuildOurOkpoSqlMatchVariantsForTests("0012345678");
        Assert.Contains("0012345678", variants);
        var norm = TransferReceiveTestAccess.NormalizeNumberForTests("0012345678");
        Assert.Contains(norm, variants);
        Assert.Contains("0" + norm, variants);
    }

    [Fact]
    public void BuildOurOkpoSqlMatchVariants_IncludesEightDigitHeadOfExtended()
    {
        var variants = TransferReceiveTestAccess.BuildOurOkpoSqlMatchVariantsForTests("08624243_40044");
        Assert.Contains("08624243_40044", variants);
        Assert.Contains("08624243", variants);
    }

    [Fact]
    public void CounterpartProviderPointsToUs_AcceptsFullMatchAndEightPrefix()
    {
        Assert.True(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests(
            "010000001", "10000001"));
        Assert.True(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests(
            "08624243", "08624243_40044"));
        Assert.True(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests(
            "08624243_40044", "08624243"));
        Assert.False(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests(
            "20000002", "10000001"));
        Assert.False(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests(
            "-", "10000001"));
    }

    [Fact]
    public void OkpoReferencesMatch_EightVsExtended_IsMatch_DifferentTail_IsNot()
    {
        Assert.True(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "08624243", "08624243_40044"));
        Assert.True(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "08624243_40044", "08624243"));
        Assert.True(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "08624243_40044", "08624243_40044"));
        Assert.False(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "08624243_40044", "08624243_99999"));
        Assert.False(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "08624243", "08624244_40044"));
    }

    [Fact]
    public void SimilarityProviderOkpo_EightPrefix_IsNearAlmostExact()
    {
        Assert.Equal(
            FieldMatchLevel.Near,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "08624243", "08624243_40044", "08624243_40044"));
        Assert.True(
            TransferReceiveTestAccess.SimilarityProviderOkpoScoreForTests(
                "08624243", "08624243_40044", "08624243_40044") >= 0.99);
        Assert.Equal(
            FieldMatchLevel.Exact,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "08624243_40044", "08624243_40044", "08624243_40044"));
        Assert.Equal(
            FieldMatchLevel.Mismatch,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "99999999", "08624243_40044", "08624243_40044"));
    }

    [Fact]
    public void SimilarityProviderOkpo_LongNearTypo_IsNear_ShortTypo_IsMismatch()
    {
        // Длинный ОКПО: несколько перепутанных цифр при той же длине — Near (подсветка).
        Assert.Equal(
            FieldMatchLevel.Near,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "84111730330002", "84117173030002", "84117173030002"));

        // Короткий 8-значный: одна опечатка — по-прежнему Mismatch (не смягчаем).
        Assert.Equal(
            FieldMatchLevel.Mismatch,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "08624244", "08624243", "08624243"));

        // Совсем другой длинный номер — Mismatch.
        Assert.Equal(
            FieldMatchLevel.Mismatch,
            TransferReceiveTestAccess.SimilarityProviderOkpoLevelForTests(
                "99999999999999", "84117173030002", "84117173030002"));
    }

    [Fact]
    public void SeedOkpoAliasMapFromTitles_IndexesNormalizedTitleOkpo()
    {
        var map = TransferReceiveTestAccess.SeedOkpoAliasMapFromTitlesForTests(
            new Dictionary<int, string>
            {
                [1] = "00100",
                [2] = "200"
            });

        var key100 = TransferReceiveTestAccess.NormalizeNumberForTests("00100");
        Assert.Contains(1, map[key100]);
        Assert.True(map.ContainsKey(TransferReceiveTestAccess.NormalizeNumberForTests("200")));
    }

    [Fact]
    public void SeedOkpoAliasMapFromTitles_IndexesEightDigitHeadOfExtended()
    {
        var map = TransferReceiveTestAccess.SeedOkpoAliasMapFromTitlesForTests(
            new Dictionary<int, string> { [7] = "08624243_40044" });

        Assert.Contains(7, map["08624243"]);
        Assert.Contains(7, map[TransferReceiveTestAccess.NormalizeNumberForTests("08624243")]);
        Assert.Contains(7, map[TransferReceiveTestAccess.NormalizeNumberForTests("08624243_40044")]);
    }

    [Fact]
    public void BuildOurOkpoSqlMatchVariants_IncludesEightDigitHeadOfFourteen()
    {
        var variants = TransferReceiveTestAccess.BuildOurOkpoSqlMatchVariantsForTests("12345678901234");
        Assert.Contains("12345678901234", variants);
        Assert.Contains("12345678", variants);
    }

    [Fact]
    public void OkpoIndexKeys_FourteenDigit_IncludesHead8()
    {
        var keys = TransferReceiveTestAccess.OkpoIndexKeysForTests("12345678901234");
        Assert.Contains("12345678", keys);
        Assert.Contains(TransferReceiveTestAccess.NormalizeNumberForTests("12345678901234"), keys);
    }

    [Fact]
    public void OkpoReferencesMatch_EightVsFourteen_IsMatch()
    {
        Assert.True(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "12345678", "12345678901234"));
        Assert.True(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "12345678901234", "12345678"));
        Assert.False(TransferReceiveTestAccess.OkpoReferencesMatchForTests(
            "12345678901234", "12345678909999"));
    }

    [Fact]
    public void OkpoClaimMatchesTitle_LegalOrBranch_FullOrHead8()
    {
        Assert.True(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "11111111", legalOkpo: "11111111", branchOkpo: "22222222"));
        Assert.True(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "22222222", legalOkpo: "11111111", branchOkpo: "22222222"));
        Assert.True(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "11111111", legalOkpo: "11111111_40044", branchOkpo: ""));
        Assert.True(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "12345678", legalOkpo: "12345678901234", branchOkpo: "-"));
        Assert.False(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "99999999", legalOkpo: "11111111", branchOkpo: "22222222"));
        // Одна голова 8_5 — для отбора org (не для reverse-ссылки пары).
        Assert.True(TransferReceiveTestAccess.OkpoClaimMatchesTitleForTests(
            "11111111_99999", legalOkpo: "11111111_40044", branchOkpo: ""));
    }

    [Fact]
    public void SeedOkpoAliasMapFromTitles_IndexesLegalAndBranch()
    {
        var map = TransferReceiveTestAccess.SeedOkpoAliasMapFromTitlesForTests(
            new Dictionary<int, (string Legal, string Branch)>
            {
                [5] = ("11111111", "22222222_12345")
            });

        Assert.Contains(5, map[TransferReceiveTestAccess.NormalizeNumberForTests("11111111")]);
        Assert.Contains(5, map["22222222"]);
        Assert.Contains(5, map[TransferReceiveTestAccess.NormalizeNumberForTests("22222222_12345")]);
    }

    [Fact]
    public void TryGetOkpoHead8_SupportsAllThreeFormats()
    {
        Assert.True(TransferReceiveTestAccess.TryGetOkpoHead8ForTests("12345678", out var h1));
        Assert.Equal("12345678", h1);
        Assert.True(TransferReceiveTestAccess.TryGetOkpoHead8ForTests("12345678_99999", out var h2));
        Assert.Equal("12345678", h2);
        Assert.True(TransferReceiveTestAccess.TryGetOkpoHead8ForTests("12345678901234", out var h3));
        Assert.Equal("12345678", h3);
        Assert.False(TransferReceiveTestAccess.TryGetOkpoHead8ForTests("-", out _));
        Assert.False(TransferReceiveTestAccess.TryGetOkpoHead8ForTests("123", out _));
    }
}
