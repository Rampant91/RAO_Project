using System.Collections.Generic;
using System.Linq;
using Client_App.Helpers.MasterTitleRows;
using Xunit;

namespace Test.DatabaseCleanup;

public sealed class MasterTitleRowsCleanupPlannerTests
{
    [Fact]
    public void HasExcessRows_False_When_Exactly_Two_Per_Report()
    {
        var rows = new[]
        {
            new MasterTitleRowRef(1, 10, 1, 5),
            new MasterTitleRowRef(2, 10, 2, 0)
        };
        Assert.False(MasterTitleRowsCleanupPlanner.HasExcessRows(rows));
    }

    [Fact]
    public void HasExcessRows_True_When_Four_On_One_Report()
    {
        var rows = TypicalFour(reportId: 10, filledId: 1);
        Assert.True(MasterTitleRowsCleanupPlanner.HasExcessRows(rows));
    }

    [Fact]
    public void Plan_Deletes_Empty_Duplicates_Keeps_Filled()
    {
        var rows = TypicalFour(reportId: 10, filledId: 1);
        var plan = MasterTitleRowsCleanupPlanner.Plan(rows);

        Assert.Equal(new[] { 2, 4 }, plan.IdsToDelete.OrderBy(x => x).ToArray());
        Assert.Empty(plan.Conflicts);
    }

    [Fact]
    public void Plan_Healthy_Two_Rows_Deletes_Nothing()
    {
        var rows = new[]
        {
            new MasterTitleRowRef(1, 10, 1, 3),
            new MasterTitleRowRef(2, 10, 2, 0)
        };
        var plan = MasterTitleRowsCleanupPlanner.Plan(rows);
        Assert.Empty(plan.IdsToDelete);
        Assert.Empty(plan.Conflicts);
    }

    [Fact]
    public void Plan_Two_Filled_Same_Order_Skips_Delete_And_Reports_Conflict()
    {
        var rows = new[]
        {
            new MasterTitleRowRef(1, 10, 1, 3),
            new MasterTitleRowRef(2, 10, 1, 2),
            new MasterTitleRowRef(3, 10, 2, 0),
            new MasterTitleRowRef(4, 10, 2, 0)
        };
        var plan = MasterTitleRowsCleanupPlanner.Plan(rows);

        // Order 1 conflict → no deletes there; order 2 all empty → keep min Id=3, delete 4
        Assert.Equal(new[] { 4 }, plan.IdsToDelete.ToArray());
        Assert.Single(plan.Conflicts);
        Assert.Equal(10, plan.Conflicts[0].ReportId);
        Assert.Equal(1, plan.Conflicts[0].NumberInOrder);
        Assert.Equal(new[] { 1, 2 }, plan.Conflicts[0].FilledIds.ToArray());
    }

    [Fact]
    public void Plan_All_Empty_Keeps_Min_Id_Per_Order()
    {
        var rows = new[]
        {
            new MasterTitleRowRef(10, 5, 1, 0),
            new MasterTitleRowRef(11, 5, 1, 0),
            new MasterTitleRowRef(20, 5, 2, 0),
            new MasterTitleRowRef(21, 5, 2, 0)
        };
        var plan = MasterTitleRowsCleanupPlanner.Plan(rows);

        Assert.Equal(new[] { 11, 21 }, plan.IdsToDelete.OrderBy(x => x).ToArray());
        Assert.Empty(plan.Conflicts);
    }

    [Fact]
    public void Plan_Ignores_Null_ReportId_And_Other_Orders()
    {
        var rows = new[]
        {
            new MasterTitleRowRef(1, null, 1, 0),
            new MasterTitleRowRef(2, null, 1, 0),
            new MasterTitleRowRef(3, 10, 1, 2),
            new MasterTitleRowRef(4, 10, 2, 0),
            new MasterTitleRowRef(5, 10, 3, 0),
            new MasterTitleRowRef(6, 10, 3, 0)
        };
        var plan = MasterTitleRowsCleanupPlanner.Plan(rows);
        Assert.Empty(plan.IdsToDelete);
        Assert.False(MasterTitleRowsCleanupPlanner.HasExcessRows(rows.Where(r => r.ReportId is null)));
        // Report 10 has 4 rows (incl order 3) → excess true
        Assert.True(MasterTitleRowsCleanupPlanner.HasExcessRows(rows));
    }

    [Fact]
    public void ScoreFields_Dash_Is_Empty()
    {
        Assert.Equal(0, MasterTitleRowFullness.ScoreFields("", " ", "-", null));
        Assert.Equal(2, MasterTitleRowFullness.ScoreFields("A", "-", "B"));
    }

    private static List<MasterTitleRowRef> TypicalFour(int reportId, int filledId) =>
    [
        new MasterTitleRowRef(filledId, reportId, 1, 5),
        new MasterTitleRowRef(2, reportId, 1, 0),
        new MasterTitleRowRef(3, reportId, 2, 0),
        new MasterTitleRowRef(4, reportId, 2, 0)
    ];
}
