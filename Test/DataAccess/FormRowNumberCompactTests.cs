using System.Collections.Generic;
using System.Linq;
using Client_App.Services.DataAccess;
using Models.Forms.Form1;
using Xunit;

namespace Test.DataAccess;

public class FormRowNumberCompactTests
{
    [Fact]
    public void DisplayNumber_Page2Size20_StartsAt21()
    {
        Assert.Equal(21, FormRowNumberCompact.DisplayNumber(currentPage: 2, rowCount: 20, indexOnPage: 0));
        Assert.Equal(40, FormRowNumberCompact.DisplayNumber(2, 20, 19));
    }

    [Fact]
    public void NeedsCompact_AlreadyOneToN_IsFalse()
    {
        var current = new List<(int Id, int Number)> { (10, 1), (11, 2), (12, 3) };
        Assert.False(FormRowNumberCompact.NeedsCompact(current));
    }

    [Fact]
    public void NeedsCompact_Gaps_IsTrue()
    {
        var current = new List<(int Id, int Number)> { (1, 1), (2, 2), (3, 5), (4, 8) };
        Assert.True(FormRowNumberCompact.NeedsCompact(current));
        var ids = current.Select(x => x.Id).ToList();
        var assignments = FormRowNumberCompact.Assignments(ids);
        Assert.Equal(new[] { (1, 1), (2, 2), (3, 3), (4, 4) }, assignments);
    }

    [Fact]
    public void Assignments_DeleteFirstOfFive_CompactsRemaining()
    {
        var remaining = new[] { 2, 3, 4, 5 };
        Assert.Equal(new[] { (2, 1), (3, 2), (4, 3), (5, 4) }, FormRowNumberCompact.Assignments(remaining));
    }

    [Fact]
    public void BuildCompactIdOrder_InsertsNewRowBeforeTarget()
    {
        var inserted = new Form11 { Id = 99, InsertBeforeId = 30 };
        var order = FormRowNumberCompact.BuildCompactIdOrder([10, 20, 30, 99], [inserted]);
        Assert.Equal([10, 20, 99, 30], order);
    }

    [Fact]
    public void BuildCompactIdOrder_DuplicateNppBrokenById_KeepsDbOrderWhenNoInsert()
    {
        var order = FormRowNumberCompact.BuildCompactIdOrder([5, 7, 8], []);
        Assert.Equal([5, 7, 8], order);
        Assert.Equal(new[] { (5, 1), (7, 2), (8, 3) }, FormRowNumberCompact.Assignments(order));
    }

    [Fact]
    public void BuildUpdateCaseSql_QuotesTableAndLimitsBatch()
    {
        var assignments = FormRowNumberCompact.Assignments([10, 11, 12]);
        var sql = FormRowNumberCompact.BuildUpdateCaseSql("form_11", reportId: 4, assignments, 0, 3);
        Assert.Contains("UPDATE \"form_11\" SET \"NumberInOrder_DB\" = CASE \"Id\"", sql);
        Assert.Contains("WHEN 10 THEN 1", sql);
        Assert.Contains("WHEN 12 THEN 3", sql);
        Assert.Contains("WHERE \"ReportId\" = 4 AND \"Id\" IN (10,11,12)", sql);
    }

    [Fact]
    public void TableName_OnlyForm1x()
    {
        Assert.Equal("form_11", FormRowNumberCompact.TableName("1.1"));
        Assert.Equal("", FormRowNumberCompact.TableName("2.1"));
    }
}
