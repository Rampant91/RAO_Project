using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа N — одна единица, разное написание при инв. и передаче.</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> NormalizationCases()
    {
        yield return Norm01_FactoryNumber_LeadingZero();
        yield return Norm02_PackNumber_SpecialCharacters();
        yield return Norm03_Type_CaseAndCyrillicLatin();
        yield return Norm04_Radionuclides_OrderInsensitive();
        yield return Norm05_PassportNumber_Spaces();
    }

    /// <summary>N01. Зав.№ 00083 при инв., 83 при передаче (ведущие нули).</summary>
    private static SnkTestCase Norm01_FactoryNumber_LeadingZero() => new()
    {
        Name = "N01. Зав.№: ведущий ноль (00083 = 83).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "00083", "Тип-М1", "кобальт-60", "УКТ-11")),
            Transfer(RechargeDay, "P-101", "83", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "00083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N02. УКТ УКТ-A1 при инв., УКТA1 при передаче (дефис игнорируется).</summary>
    private static SnkTestCase Norm02_PackNumber_SpecialCharacters() => new()
    {
        Name = "N02. УКТ: спецсимволы (УКТ-A1 = УКТA1).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-A1")),
            Transfer(RechargeDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТA1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-A1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N03. Тип Тип-М1 / тип-м1.</summary>
    private static SnkTestCase Norm03_Type_CaseAndCyrillicLatin() => new()
    {
        Name = "N03. Тип: регистр и кириллица/латиница (Тип-М1 = тип-м1).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            Transfer(RechargeDay, "P-101", "F-083", "тип-м1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N04. Порядок радионуклидов в строке.</summary>
    private static SnkTestCase Norm04_Radionuclides_OrderInsensitive() => new()
    {
        Name = "N04. Радионуклиды: порядок не важен.",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60, цезий-137", "УКТ-11")),
            Transfer(RechargeDay, "P-101", "F-083", "Тип-М1", "цезий-137, кобальт-60", "УКТ-11"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60, цезий-137", "УКТ-11")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N05. № паспорта P-101 / «P 1 0 1».</summary>
    private static SnkTestCase Norm05_PassportNumber_Spaces() => new()
    {
        Name = "N05. № паспорта: пробелы (P-101 = P 1 0 1).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            Transfer(RechargeDay, "P 1 0 1", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate, AnchorStock()))
    };
}
