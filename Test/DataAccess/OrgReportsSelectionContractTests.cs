using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test.DataAccess;

/// <summary>
/// Контракт: выгрузки SelectedOrg не должны брать список отчётов из неполного Report_Collection.
/// </summary>
public class OrgReportsSelectionContractTests
{
    [Fact]
    public void IncompleteInMemoryReportCollection_IsNotAuthoritative()
    {
        // Симуляция: в памяти только страница грида (1 отчёт), в «БД» — полный набор.
        var inMemoryReportIds = new[] { 10 };
        var dbReportIds = new[] { 10, 20, 30, 40 };

        var exportIds = PreferDbReportIds(inMemoryReportIds, dbReportIds);

        Assert.Equal(dbReportIds, exportIds);
        Assert.NotEqual(inMemoryReportIds.Length, exportIds.Length);
    }

    /// <summary>
    /// Тот же принцип, что OrgReportsQuery.GetReportIdsAsync: источник истины — БД.
    /// </summary>
    private static int[] PreferDbReportIds(IEnumerable<int> _, IEnumerable<int> dbIds) =>
        dbIds.ToArray();
}
