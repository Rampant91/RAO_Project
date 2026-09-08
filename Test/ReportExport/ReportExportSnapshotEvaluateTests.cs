using System.Threading.Tasks;
using Client_App.Services;
using Models.Collections;
using Models.Forms.Form1;
using Xunit;

namespace Test.ReportExport;

/// <summary>
/// Матрица <see cref="ReportExportSnapshotService.Evaluate"/> и ApplyFromImport / Record.
/// </summary>
public class ReportExportSnapshotEvaluateTests
{
    [Fact]
    public void Evaluate_False_WhenNoLastExportedCorrectionNumber()
    {
        var report = CreateReport(correction: 1, lastExported: null, fingerprint: "abc", mutateQuantity: true);
        Assert.False(ReportExportSnapshotService.Evaluate(report));
    }

    [Fact]
    public void Evaluate_False_WhenCorrectionNumberRaised()
    {
        var report = CreateReport(correction: 2, lastExported: 1, fingerprint: null, mutateQuantity: false);
        var fp = ReportContentFingerprint.Compute(report);
        report.LastExportedFingerprint_DB = fp;
        report.Rows11[0].Quantity_DB = 99;

        Assert.False(ReportExportSnapshotService.Evaluate(report));
    }

    [Fact]
    public void Evaluate_False_WhenFingerprintMissing()
    {
        var report = CreateReport(correction: 1, lastExported: 1, fingerprint: null, mutateQuantity: true);
        Assert.False(ReportExportSnapshotService.Evaluate(report));
    }

    [Fact]
    public void Evaluate_False_WhenFingerprintMatches()
    {
        var report = CreateReport(correction: 1, lastExported: 1, fingerprint: null, mutateQuantity: false);
        report.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(report);

        Assert.False(ReportExportSnapshotService.Evaluate(report));
    }

    [Fact]
    public void Evaluate_True_WhenSameNAndContentChanged()
    {
        var report = CreateReport(correction: 1, lastExported: 1, fingerprint: null, mutateQuantity: false);
        report.LastExportedFingerprint_DB = ReportContentFingerprint.Compute(report);
        report.Rows11[0].Quantity_DB = 42;

        Assert.True(ReportExportSnapshotService.Evaluate(report));
    }

    [Fact]
    public void ApplyFromImport_Computes_WhenFingerprintEmpty()
    {
        var report = CreateReport(correction: 2, lastExported: null, fingerprint: null, mutateQuantity: false);
        ReportExportSnapshotService.ApplyFromImport(report);

        Assert.Equal((byte)2, report.LastExportedCorrectionNumber_DB);
        Assert.False(string.IsNullOrEmpty(report.LastExportedFingerprint_DB));
        Assert.Equal(ReportContentFingerprint.Compute(report), report.LastExportedFingerprint_DB);
    }

    [Fact]
    public void ApplyFromImport_KeepsFields_WhenFingerprintPresent()
    {
        var report = CreateReport(correction: 3, lastExported: 1, fingerprint: "deadbeef", mutateQuantity: false);
        ReportExportSnapshotService.ApplyFromImport(report);

        Assert.Equal((byte)1, report.LastExportedCorrectionNumber_DB);
        Assert.Equal("deadbeef", report.LastExportedFingerprint_DB);
    }

    [Fact]
    public void RecordSuccessfulExport_WritesCurrentNAndFingerprint()
    {
        var report = CreateReport(correction: 4, lastExported: 1, fingerprint: "old", mutateQuantity: false);
        ReportExportSnapshotService.RecordSuccessfulExport(report);

        Assert.Equal((byte)4, report.LastExportedCorrectionNumber_DB);
        Assert.Equal(ReportContentFingerprint.Compute(report), report.LastExportedFingerprint_DB);
    }

    [Fact]
    public async Task EnsureSnapshotOnOpen_Skips_WhenFingerprintPresent()
    {
        var report = CreateReport(correction: 1, lastExported: 1, fingerprint: "keep-me", mutateQuantity: false);
        await ReportExportSnapshotService.EnsureSnapshotOnOpenAsync(report);
        Assert.Equal("keep-me", report.LastExportedFingerprint_DB);
    }

    [Fact]
    public async Task EnsureSnapshotOnOpen_Skips_DraftWithoutExportDateOrLastExported()
    {
        var report = CreateReport(correction: 0, lastExported: null, fingerprint: null, mutateQuantity: false);
        report.ExportDate_DB = "";
        await ReportExportSnapshotService.EnsureSnapshotOnOpenAsync(report);
        Assert.Null(report.LastExportedFingerprint_DB);
        Assert.Null(report.LastExportedCorrectionNumber_DB);
    }

    private static Report CreateReport(byte correction, byte? lastExported, string? fingerprint, bool mutateQuantity)
    {
        var report = new Report
        {
            FormNum_DB = "1.1",
            CorrectionNumber_DB = correction,
            LastExportedCorrectionNumber_DB = lastExported,
            LastExportedFingerprint_DB = fingerprint,
            StartPeriod_DB = "01.01.2024",
            EndPeriod_DB = "31.03.2024",
            ExportDate_DB = "01.02.2024"
        };
        report.Rows11.Add(new Form11
        {
            NumberInOrder_DB = 1,
            Quantity_DB = mutateQuantity ? 7 : 1,
            Radionuclids_DB = "цезий-137",
            OperationCode_DB = "01",
            OperationDate_DB = "01.01.2024",
            PassportNumber_DB = "P-1",
            Type_DB = "T",
            FactoryNumber_DB = "F-1"
        });
        return report;
    }
}
