using Client_App.Behaviors.DataGrid;
using Xunit;

namespace Test.Behaviors;

public class DataGridMaxColumnWidthGuardTests
{
    [Fact]
    public void WouldThrow_WhenColumnsExist_AndCurrentMaxIsInfinity()
    {
        Assert.True(DataGridMaxColumnWidthGuard.WouldThrowOnAssign(1, double.PositiveInfinity));
        Assert.True(DataGridMaxColumnWidthGuard.WouldThrowOnAssign(25, double.NegativeInfinity));
    }

    [Fact]
    public void WouldNotThrow_WhenNoColumnsYet()
    {
        Assert.False(DataGridMaxColumnWidthGuard.WouldThrowOnAssign(0, double.PositiveInfinity));
    }

    [Fact]
    public void WouldNotThrow_WhenCurrentMaxIsAlreadyFinite()
    {
        Assert.False(DataGridMaxColumnWidthGuard.WouldThrowOnAssign(25, 500));
    }
}
