using System.Collections.Generic;
using Xunit;
using static Client_App.Commands.AsyncCommands.ExcelExport.TransferReceivePairing.ExcelExportCheckTransferReceiveAsyncCommand;

namespace Test.TransferReceive;

/// <summary>
/// Хелперы ОКПО без БД: SQL-variants, seed титулов, NormalizeNumber-фильтр.
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
    public void CounterpartProviderPointsToUs_UsesNormalizeNumber()
    {
        var ourNorm = TransferReceiveTestAccess.NormalizeNumberForTests("10000001");
        Assert.True(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests("010000001", ourNorm));
        Assert.False(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests("20000002", ourNorm));
        Assert.False(TransferReceiveTestAccess.CounterpartProviderPointsToUsForTests("-", ourNorm));
    }

    [Fact]
    public void SeedOkpoAliasMapFromTitles_IndexesNormalizedTitleOkpo()
    {
        var map = TransferReceiveTestAccess.SeedOkpoAliasMapFromTitlesForTests(
            new Dictionary<int, string>
            {
                [10] = "00100",
                [20] = "200",
                [30] = "-"
            });

        var key100 = TransferReceiveTestAccess.NormalizeNumberForTests("00100");
        Assert.True(map.ContainsKey(key100));
        Assert.Equal([10], map[key100]);
        Assert.True(map.ContainsKey(TransferReceiveTestAccess.NormalizeNumberForTests("200")));
        Assert.DoesNotContain(map.Keys, k => k.Length == 0);
    }
}
