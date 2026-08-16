using Client_App.Services.DataAccess;
using Xunit;

namespace Test.DataAccess;

public class Form10TitleSelectorTests
{
    [Fact]
    public void Pick_UsesSecondRow_WhenSeparateOkpoFilled()
    {
        var title = Form10TitleSelector.Pick(
            regNo0: "R0", okpo0: "111", short0: "Head",
            regNo1: "R1", okpo1: "222", short1: "Branch");

        Assert.Equal("R1", title.RegNo);
        Assert.Equal("222", title.Okpo);
        Assert.Equal("Branch", title.ShortJurLico);
    }

    [Fact]
    public void Pick_UsesFirstRow_WhenSecondOkpoEmpty()
    {
        var title = Form10TitleSelector.Pick(
            regNo0: "R0", okpo0: "111", short0: "Head",
            regNo1: "", okpo1: "", short1: "");

        Assert.Equal("R0", title.RegNo);
        Assert.Equal("111", title.Okpo);
        Assert.Equal("Head", title.ShortJurLico);
    }

    [Fact]
    public void Pick_OkpoDash_UsesFirstForOkpo_SecondForRegNoWhenPresent()
    {
        // OkpoRep: "-" → строка 0; RegNoRep: Okpo "-" и непустой → может взять RegNo1
        var title = Form10TitleSelector.Pick(
            regNo0: "R0", okpo0: "111", short0: "Head",
            regNo1: "R1", okpo1: "-", short1: "Branch");

        Assert.Equal("R1", title.RegNo);
        Assert.Equal("111", title.Okpo);
        Assert.Equal("Head", title.ShortJurLico);
    }
}
