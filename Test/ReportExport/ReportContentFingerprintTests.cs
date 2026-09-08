using Models.Collections;
using Models.Forms.Form1;
using Xunit;

namespace Test.ReportExport;

/// <summary>
/// Стабильность и чувствительность <see cref="ReportContentFingerprint.Compute"/>.
/// </summary>
public class ReportContentFingerprintTests
{
    [Fact]
    public void Compute_IsStable_ForSameContent()
    {
        var a = CreateForm11Report(quantity: 1, radionuclids: "цезий-137", correctionNumber: 2);
        var b = CreateForm11Report(quantity: 1, radionuclids: "цезий-137", correctionNumber: 2);

        Assert.Equal(ReportContentFingerprint.Compute(a), ReportContentFingerprint.Compute(b));
        Assert.Equal(64, ReportContentFingerprint.Compute(a).Length);
    }

    [Fact]
    public void Compute_Changes_WhenRowContentChanges()
    {
        var a = CreateForm11Report(quantity: 1, radionuclids: "цезий-137");
        var b = CreateForm11Report(quantity: 2, radionuclids: "цезий-137");

        Assert.NotEqual(ReportContentFingerprint.Compute(a), ReportContentFingerprint.Compute(b));
    }

    [Fact]
    public void Compute_Ignores_CorrectionNumberOnly()
    {
        var a = CreateForm11Report(quantity: 1, radionuclids: "цезий-137", correctionNumber: 0);
        var b = CreateForm11Report(quantity: 1, radionuclids: "цезий-137", correctionNumber: 3);

        Assert.Equal(ReportContentFingerprint.Compute(a), ReportContentFingerprint.Compute(b));
    }

    private static Report CreateForm11Report(int quantity, string radionuclids, byte correctionNumber = 0)
    {
        var report = new Report
        {
            FormNum_DB = "1.1",
            CorrectionNumber_DB = correctionNumber,
            StartPeriod_DB = "01.01.2024",
            EndPeriod_DB = "31.03.2024"
        };
        report.Rows11.Add(new Form11
        {
            NumberInOrder_DB = 1,
            Quantity_DB = quantity,
            Radionuclids_DB = radionuclids,
            OperationCode_DB = "01",
            OperationDate_DB = "01.01.2024",
            PassportNumber_DB = "P-1",
            Type_DB = "T",
            FactoryNumber_DB = "F-1"
        });
        return report;
    }
}
