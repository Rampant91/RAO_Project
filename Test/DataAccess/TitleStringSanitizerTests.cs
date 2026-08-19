using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

/// <summary>
/// Логика CustomTrim для этапа «Очистка» при инициализации (через InternalsVisibleTo).
/// </summary>
public class TitleStringSanitizerTests
{
    [Fact]
    public void CustomTrim_NullBecomesEmpty()
    {
        Assert.Equal(string.Empty, TitleRowSanitizer.CustomTrimForTest(null));
    }

    [Fact]
    public void CustomTrim_RemovesEmbeddedNewlinesAndTrims()
    {
        Assert.Equal("ab", TitleRowSanitizer.CustomTrimForTest("  a\r\nb  "));
    }

    [Fact]
    public void CustomTrim_UnchangedValue_ReturnsEqualString()
    {
        Assert.Equal("abc", TitleRowSanitizer.CustomTrimForTest("abc"));
    }

    [Fact]
    public void FieldPotentiallyDirty_TrailingSpace()
    {
        Assert.True(TitleRowSanitizer.FieldPotentiallyDirtyForTest("abc "));
        Assert.False(TitleRowSanitizer.FieldPotentiallyDirtyForTest("abc"));
    }
}
