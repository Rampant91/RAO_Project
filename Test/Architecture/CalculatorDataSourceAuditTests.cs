using System.IO;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Калькуляторы меню «Сервис» и расчёт категории по строке формы
/// не должны зависеть от LocalReports / полного preload строк.
/// </summary>
public class CalculatorDataSourceAuditTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "RAO_Project.sln"))
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
                dir = dir.Parent;
            Assert.NotNull(dir);
            return dir!.FullName;
        }
    }

    private static string ReadCalculatorSource(string fileName)
    {
        var path = Path.Combine(RepoRoot, "Client_App", "Commands", "AsyncCommands", "Calculator", fileName);
        Assert.True(File.Exists(path), $"Missing {path}");
        return File.ReadAllText(path);
    }

    [Theory]
    [InlineData("OpenCalculatorAsyncCommand.cs")]
    [InlineData("ActivityCalculationAsyncCommand.cs")]
    [InlineData("CategoryCalculationAsyncCommand.cs")]
    [InlineData("CalculatorFilterAsyncCommand.cs")]
    public void ServiceMenuCalculators_DoNotTouchLocalReportsOrDbCollections(string fileName)
    {
        var src = ReadCalculatorSource(fileName);
        Assert.DoesNotContain("ReportsStorage", src);
        Assert.DoesNotContain("LocalReports", src);
        Assert.DoesNotContain("ReportCollectionDbSet", src);
        Assert.DoesNotContain("ReportsCollectionDbSet", src);
        Assert.DoesNotContain("Report_Collection", src);
    }

    [Fact]
    public void OpenCalculator_LoadsDictionaryFromSpreadsheetOnly()
    {
        var src = ReadCalculatorSource("OpenCalculatorAsyncCommand.cs");
        Assert.Contains("R.xlsx", src);
        Assert.Contains("ActivityCalculator", src);
        Assert.Contains("CategoryCalculator", src);
        Assert.Contains("\"activity\"", src);
        Assert.Contains("\"category\"", src);
    }

    [Fact]
    public void CategoryFromReport_UsesSelectedForm11ScalarsOnly()
    {
        var src = ReadCalculatorSource("CategoryCalculationFromReportAsyncCommand.cs");
        Assert.DoesNotContain("ReportsStorage", src);
        Assert.DoesNotContain("LocalReports", src);
        Assert.DoesNotContain("ReportCollectionDbSet", src);
        Assert.Contains("Form11", src);
        Assert.Contains("Activity_DB", src);
        Assert.Contains("Quantity_DB", src);
        Assert.Contains("Radionuclids_DB", src);
        // Данные строки уже на текущей странице редактора (paging) — догрузка всей формы не нужна.
        Assert.DoesNotContain("EnsureAllRows", src);
        Assert.DoesNotContain("Include(", src);
    }
}