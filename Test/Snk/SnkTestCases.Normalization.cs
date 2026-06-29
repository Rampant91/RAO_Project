using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа N — распознавание одной и той же единицы при разном написании полей.
/// Во всех кейсах единица инвентаризируется в первой инвентаризации с написанием «A»,
/// а затем передаётся с написанием «B». Если алгоритм считает написания эквивалентными,
/// передача снимает единицу с учёта и в СНК остаётся только якорная 999/001.
/// Если распознавание сломано — единица «застрянет» в наличии, и тест это покажет.
/// </summary>
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

    /// <summary>
    /// N01. Зав.№ с ведущим нулём: «083» при инвентаризации и «83» при передаче — одна и та же единица.
    /// </summary>
    private static SnkTestCase Norm01_FactoryNumber_LeadingZero() => new()
    {
        Name = "N01. Зав.№: ведущий ноль (083 = 83).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "83", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// N02. УКТ со спецсимволами: «52-1» при инвентаризации и «521» при передаче — один и тот же УКТ.
    /// </summary>
    private static SnkTestCase Norm02_PackNumber_SpecialCharacters() => new()
    {
        Name = "N02. УКТ: спецсимволы (52-1 = 521).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "521"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// N03. Тип: разный регистр и кириллица/латиница («ГИК-5-3» и «гик-5-3») — один и тот же тип.
    /// </summary>
    private static SnkTestCase Norm03_Type_CaseAndCyrillicLatin() => new()
    {
        Name = "N03. Тип: регистр и кириллица/латиница (ГИК-5-3 = гик-5-3).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "510", "083", "гик-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// N04. Радионуклиды: порядок перечисления не важен («кобальт-60, цезий-137» = «цезий-137, кобальт-60»).
    /// </summary>
    private static SnkTestCase Norm04_Radionuclides_OrderInsensitive() => new()
    {
        Name = "N04. Радионуклиды: порядок не важен.",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60, цезий-137", "52-1"),
            Operation("28", RechargeDay, "510", "083", "ГИК-5-3", "цезий-137, кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60, цезий-137", "52-1")),
            On(FinalDate, AnchorStock()))
    };

    /// <summary>
    /// N05. № паспорта с пробелами: «510» при инвентаризации и «5 1 0» при передаче — один и тот же номер.
    /// </summary>
    private static SnkTestCase Norm05_PassportNumber_Spaces() => new()
    {
        Name = "N05. № паспорта: пробелы (510 = 5 1 0).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Operation("10", FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Operation("28", RechargeDay, "5 1 0", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock()))
    };
}
