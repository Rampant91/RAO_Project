using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Layout guards for report <c>DataGrid.table</c> (Avalonia 11.2.6).
/// <para>
/// Main regression: <see cref="TableStyle_DoesNotSetMaxColumnWidth"/> and
/// <see cref="ReportTableDataGrids_SetMaxColumnWidthLocally"/> — prevent reintroducing the
/// "value cannot be set to infinity" crash when opening report forms (MaxColumnWidth in style vs on element).
/// </para>
/// </summary>
public class DataGridReportTableLayoutGuardTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "RAO_Project.sln"))
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
            {
                dir = dir.Parent;
            }

            Assert.NotNull(dir);
            return dir!.FullName;
        }
    }

    [Fact]
    public void TableScrollViewer_DisablesHorizontalScroll_SoDataGridGetsFiniteWidth()
    {
        var path = Path.Combine(RepoRoot, "Client_App", "App.axaml");
        var src = File.ReadAllText(path);
        var marker = "Selector=\"ScrollViewer.tableScrollViewer\"";
        var index = src.IndexOf(marker, StringComparison.Ordinal);
        Assert.True(index >= 0, "Missing tableScrollViewer style");

        var slice = src.Substring(index, Math.Min(900, src.Length - index));
        Assert.Contains("HorizontalScrollBarVisibility\" Value=\"Disabled\"", slice);
        Assert.DoesNotContain("HorizontalScrollBarVisibility\" Value=\"Auto\"", slice);
    }

    [Fact]
    public void ReportFormHeaders_DoNotBindWidthToDataGridBounds()
    {
        var root = Path.Combine(RepoRoot, "Client_App", "Views", "Forms");
        var offenders = Directory.EnumerateFiles(root, "Form_*.axaml", SearchOption.AllDirectories)
            .Where(path => File.ReadAllText(path).Contains("Bounds.Width, ElementName=dataGrid", StringComparison.Ordinal))
            .Select(path => Path.GetRelativePath(RepoRoot, path))
            .ToList();

        Assert.True(offenders.Count == 0, "Header Width must not bind to dataGrid Bounds.Width:\n" + string.Join("\n", offenders));
    }

    [Fact]
    public void TableStyle_DoesNotSetMaxColumnWidth()
    {
        var path = Path.Combine(RepoRoot, "Client_App", "App.axaml");
        var src = File.ReadAllText(path);
        var marker = "Selector=\"DataGrid.table\"";
        var index = src.IndexOf(marker, StringComparison.Ordinal);
        Assert.True(index >= 0, "Missing DataGrid.table style");

        var nextStyle = src.IndexOf("Selector=\"", index + marker.Length, StringComparison.Ordinal);
        var slice = nextStyle > index
            ? src.Substring(index, nextStyle - index)
            : src.Substring(index, Math.Min(900, src.Length - index));

        Assert.DoesNotContain("Property=\"MaxColumnWidth\"", slice);
    }

    [Fact]
    public void ReportTableDataGrids_SetMaxColumnWidthLocally()
    {
        var root = Path.Combine(RepoRoot, "Client_App", "Views", "Forms");
        var offenders = Directory.EnumerateFiles(root, "Form_*.axaml", SearchOption.AllDirectories)
            .Where(path =>
            {
                var src = File.ReadAllText(path);
                return src.Contains("Classes=\"table\"", StringComparison.Ordinal)
                       && !src.Contains("MaxColumnWidth=\"500\"", StringComparison.Ordinal);
            })
            .Select(path => Path.GetRelativePath(RepoRoot, path))
            .ToList();

        Assert.True(offenders.Count == 0, "Report DataGrid.table must set MaxColumnWidth on the element:\n" + string.Join("\n", offenders));
    }
}
