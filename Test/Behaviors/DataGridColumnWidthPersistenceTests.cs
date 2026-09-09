using Client_App.Behaviors.DataGrid;
using Xunit;

namespace Test.Behaviors;

public class DataGridColumnWidthPersistenceTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(-1, true)]
    [InlineData(double.NaN, true)]
    [InlineData(160, false)]
    [InlineData(1, false)]
    public void ShouldKeepXamlWidth_TreatsNonPositiveAsKeepStar(double saved, bool expected) =>
        Assert.Equal(expected, DataGridColumnWidthPersistence.ShouldKeepXamlWidth(saved));

    [Fact]
    public void GetPersistableWidth_PrefersActualWidth_OverStaleAbsoluteValue()
    {
        var result = DataGridColumnWidthPersistence.GetPersistableWidth(
            actualWidth: 280,
            widthIsAbsolute: true,
            widthIsStar: false,
            widthValue: 160,
            preserveStarLayout: true);

        Assert.Equal(280, result);
    }

    [Fact]
    public void GetPersistableWidth_StarWithoutActual_ReturnsZero()
    {
        var result = DataGridColumnWidthPersistence.GetPersistableWidth(
            actualWidth: 0,
            widthIsAbsolute: false,
            widthIsStar: true,
            widthValue: 1,
            preserveStarLayout: true);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetPersistableWidth_StarWithActual_PersistsPixels()
    {
        var result = DataGridColumnWidthPersistence.GetPersistableWidth(
            actualWidth: 420,
            widthIsAbsolute: false,
            widthIsStar: true,
            widthValue: 1,
            preserveStarLayout: true);

        Assert.Equal(420, result);
    }

    [Theory]
    [InlineData("MainWindow.Orgs.1.v7", true)]
    [InlineData("MainWindow.Orgs.2.v7", true)]
    [InlineData("MainWindow.Orgs.4", true)]
    [InlineData("MainWindow.Reports.1", false)]
    [InlineData("MainWindow.Reports.2", false)]
    [InlineData("1.1", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void IsMainWindowOrgsFormNum_OnlyOrgsKeys(string? formNum, bool expected) =>
        Assert.Equal(expected, DataGridColumnWidthPersistence.IsMainWindowOrgsFormNum(formNum));

    [Theory]
    [InlineData(false, "MainWindow.Orgs.1.v7", true)]
    [InlineData(false, "MainWindow.Reports.2", false)]
    [InlineData(true, "MainWindow.Reports.2", true)]
    [InlineData(true, "1.1", true)]
    public void ShouldPreserveStarLayout_OrgsOrExplicitFlag(
        bool preserveProperty,
        string? formNum,
        bool expected) =>
        Assert.Equal(
            expected,
            DataGridColumnWidthPersistence.ShouldPreserveStarLayout(preserveProperty, formNum));
}
