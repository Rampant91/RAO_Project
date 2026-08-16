using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Реестр выгрузок фазы 0: фиксирует ожидаемый источник данных после модернизации.
/// High = нельзя опираться на SelectedReports.Report_Collection / LocalReports для payload.
/// </summary>
public class ExportDataSourceAuditTests
{
    public enum ExportRisk
    {
        Low,
        Medium,
        High
    }

    public record ExportAuditEntry(
        string Command,
        string UiSurface,
        ExportRisk Risk,
        string ExpectedSource);

    public static IEnumerable<object[]> Registry()
    {
        foreach (var e in Entries)
            yield return [e];
    }

    private static readonly ExportAuditEntry[] Entries =
    [
        new("ExcelExportListOfOrgsAsyncCommand", "Analytics", ExportRisk.Low, "TempDbByOrgId"),
        new("ExcelExportExecutorsAsyncCommand", "Analytics", ExportRisk.Low, "TempDbByOrgId"),
        new("ExcelExportListOfForms*AsyncCommand", "Analytics", ExportRisk.Low, "TempDbByOrgId"),
        new("ExcelExportAllAsyncCommand", "Analytics+OrgContext", ExportRisk.High, "TempDbLoadOrgWithReportShells"),
        new("ExcelExportFormsAsyncCommand", "Analytics+OrgContext", ExportRisk.High, "TempDbLoadOrgWithReportShellsForForm"),
        new("ExcelExportSnkAsyncCommand", "Analytics+OrgContext", ExportRisk.High, "DbHasFormNumByOrgId"),
        new("ExcelExportCheckInventoriesAsyncCommand", "Analytics+OrgContext", ExportRisk.High, "DbHasFormNumByOrgId"),
        new("ExcelExportCheckPairingOfCode41AsyncCommand", "Analytics+OrgContext", ExportRisk.Low, "TempDbByOrgId"),
        new("ExcelExportCheckTransferReceiveAsyncCommand", "Analytics+OrgContext", ExportRisk.Low, "TempDbByOrgId"),
        new("ExcelExportIntersectionsAsyncCommand", "Analytics", ExportRisk.Low, "TempDb"),
        new("ExcelExportRepWithoutPasAsyncCommand", "Analytics/Passports", ExportRisk.Low, "TempDb"),
        new("ExcelExportPasWithoutRepAsyncCommand", "Analytics/Passports", ExportRisk.Low, "TempDb"),
        new("ExcelExportCheckLastInventoryDateAsyncCommand", "Analytics", ExportRisk.Low, "TempDb"),
        new("ExcelExportLostAndExtraUnitsByRegionAsyncCommand", "Analytics", ExportRisk.Low, "TempDb"),
        new("ExcelExportPackagePassportPrikaz", "PassportsMenu", ExportRisk.Medium, "LiveDbPassport"),
        new("StoragePointsMenuWindowVM", "StoragePoints", ExportRisk.Low, "LiveDbStoragePointOnly"),
        new("GroupBulkExportReportsAsyncCommand", "Service", ExportRisk.Low, "TempDbByOrgId"),
        new("ExportReportsAsyncCommand", "OrgContext", ExportRisk.High, "OrgReportsQuery.GetReportIdsAsync"),
        new("ExportReportsWithDateRangeAsyncCommand", "OrgContext", ExportRisk.Medium, "TempDbByOrgId"),
        new("ExportAllReportsOneFileAsyncCommand", "OrgContext", ExportRisk.Medium, "TempDbCountNotLocalReports"),
        new("ExportAllReportsFromSubjectRFOneFileAsyncCommand", "OrgContext", ExportRisk.Medium, "TempDbCountNotLocalReports"),
        new("ExportAllReportsAsyncCommand", "OrgContext", ExportRisk.Low, "TempDbByOrgId"),
        new("ExportAllReportAsyncCommand", "OrgContext", ExportRisk.Low, "TempDbByReportId"),
        new("ExcelExportAllFormsByFormNumberAsyncCommand", "OrgContext", ExportRisk.Medium, "LiveDbBySelectedReportsId"),
        new("ExcelExportCheckAllFormsAsyncCommand", "OrgContext", ExportRisk.High, "TempDbLoadOrgWithReportShells"),
        new("ExcelExportFormPrintAsyncCommand", "ReportContext", ExportRisk.Low, "TempDbByReportId"),
        new("ExcelExportFormAnalysisAsyncCommand", "ReportContext", ExportRisk.Low, "TempDbByReportId"),
        new("ExportReportAsyncCommand", "ReportContext", ExportRisk.Low, "TempDbByReportId"),
        new("ExcelExportSourceMovementHistoryAsyncCommand", "FormRowsContext", ExportRisk.Low, "TempDb+SelectedPassportFields"),
    ];

    [Theory]
    [MemberData(nameof(Registry))]
    public void Registry_Entry_HasUiSurfaceAndExpectedSource(ExportAuditEntry entry)
    {
        Assert.False(string.IsNullOrWhiteSpace(entry.Command));
        Assert.False(string.IsNullOrWhiteSpace(entry.UiSurface));
        Assert.False(string.IsNullOrWhiteSpace(entry.ExpectedSource));
    }

    [Fact]
    public void HighRiskExports_MustUseOrgReportsQueryPatterns()
    {
        var high = Entries.Where(e => e.Risk == ExportRisk.High).ToList();
        Assert.Contains(high, e => e.Command == "ExcelExportAllAsyncCommand");
        Assert.Contains(high, e => e.Command == "ExcelExportFormsAsyncCommand");
        Assert.Contains(high, e => e.Command == "ExcelExportCheckAllFormsAsyncCommand");
        Assert.Contains(high, e => e.Command == "ExportReportsAsyncCommand");
        Assert.Contains(high, e => e.Command == "ExcelExportSnkAsyncCommand");
        Assert.All(high, e => Assert.Contains("Org", e.ExpectedSource + e.Command));
    }
}
