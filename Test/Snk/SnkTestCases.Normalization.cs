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

    /// <summary>N01. Зав.№ 083 при инв., 83 при передаче.</summary>
    private static SnkTestCase Norm01_FactoryNumber_LeadingZero() => new()
    {
        Name = "N01. Зав.№: ведущий ноль (083 = 83).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "510", "83", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N02. УКТ 52-1 при инв., 521 при передаче.</summary>
    private static SnkTestCase Norm02_PackNumber_SpecialCharacters() => new()
    {
        Name = "N02. УКТ: спецсимволы (52-1 = 521).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "521"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N03. Тип ГИК-5-3 / гик-5-3.</summary>
    private static SnkTestCase Norm03_Type_CaseAndCyrillicLatin() => new()
    {
        Name = "N03. Тип: регистр и кириллица/латиница (ГИК-5-3 = гик-5-3).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "510", "083", "гик-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
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
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60, цезий-137", "52-1")),
            Transfer(RechargeDay, "510", "083", "ГИК-5-3", "цезий-137, кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60, цезий-137", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>N05. № паспорта 510 / «5 1 0».</summary>
    private static SnkTestCase Norm05_PassportNumber_Spaces() => new()
    {
        Name = "N05. № паспорта: пробелы (510 = 5 1 0).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            Transfer(RechargeDay, "5 1 0", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };
}
