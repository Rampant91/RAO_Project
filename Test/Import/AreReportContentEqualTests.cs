using Client_App.Commands.AsyncCommands.Import;
using Models.Collections;
using Models.Forms.Form1;
using Xunit;

namespace Test.Import;

/// <summary>
/// Проверки сравнения содержимого отчётов при импорте (причина «Полная копия»).
/// </summary>
public class AreReportContentEqualTests
{
    [Fact]
    public void AreReportContentEqual_ReturnsFalse_WhenQuantityDiffers()
    {
        var baseRep = CreateForm11Report(quantity: 2);
        var impRep = CreateForm11Report(quantity: 1);

        Assert.False(ImportBaseAsyncCommand.AreReportContentEqual(baseRep, impRep));
    }

    [Fact]
    public void AreReportContentEqual_ReturnsFalse_WhenRadionuclidsDiffers()
    {
        var baseRep = CreateForm11Report(quantity: 1, radionuclids: "цезий-137");
        var impRep = CreateForm11Report(quantity: 1, radionuclids: "кобальт-60");

        Assert.False(ImportBaseAsyncCommand.AreReportContentEqual(baseRep, impRep));
    }

    [Fact]
    public void AreReportContentEqual_ReturnsTrue_WhenRowsMatch()
    {
        var baseRep = CreateForm11Report(quantity: 1, radionuclids: "цезий-137");
        var impRep = CreateForm11Report(quantity: 1, radionuclids: "цезий-137");

        Assert.True(ImportBaseAsyncCommand.AreReportContentEqual(baseRep, impRep));
    }

    [Fact]
    public void AreReportContentEqual_ReturnsFalse_WhenOnlyOneSideHasRows()
    {
        var baseRep = CreateForm11Report(quantity: 2);
        var impRep = new Report { FormNum_DB = "1.1" };

        Assert.False(ImportBaseAsyncCommand.AreReportContentEqual(baseRep, impRep));
    }

    [Fact]
    public void AreReportContentEqual_ReturnsTrue_WhenBothHaveNoRows()
    {
        // Легитимный случай (напр. пустая 2.6) — оба без строк считаются одинаковыми.
        // Ложная «Полная копия» из-за незагруженных строк чинится загрузкой до сравнения.
        var baseRep = new Report { FormNum_DB = "1.1" };
        var impRep = new Report { FormNum_DB = "1.1" };

        Assert.True(ImportBaseAsyncCommand.AreReportContentEqual(baseRep, impRep));
    }

    private static Report CreateForm11Report(int quantity, string radionuclids = "цезий-137")
    {
        var report = new Report { FormNum_DB = "1.1" };
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
