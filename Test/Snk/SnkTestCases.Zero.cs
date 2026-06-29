using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>
/// Группа Z — нулевые операции (коды, не относящиеся к инвентаризации, приёму/передаче
/// и перезарядке). На наличие (СНК) они влиять не должны. Проверяем оба случая:
/// нулевая операция у стоящего на учёте ЗРИ (логично) и у отсутствующего (ошибочно).
/// Сейчас сверяем только списки СНК/инвентаризаций; сами коды ошибок будут проверяться отдельно.
/// </summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ZeroOperationCases()
    {
        yield return Zero01_OnUnitInStock_NoEffect();
        yield return Zero02_OnNeverOwnedUnit_NotInStock();
    }

    /// <summary>
    /// Z01. Нулевая операция (код 64) над стоящим на учёте ЗРИ. Наличие не меняется:
    /// 510 как стоял на учёте с первой инвентаризации, так и остаётся до конца периода.
    /// </summary>
    private static SnkTestCase Zero01_OnUnitInStock_NoEffect() => new()
    {
        Name = "Z01. Нулевая операция над ЗРИ в наличии (без эффекта).",
        EndDate = FinalDate,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
            Zero(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
            On(FinalDate, AnchorStock(), Stock("510", "083", "ГИК-5-3", "кобальт-60", "52-1")))
    };

    /// <summary>
    /// Z02. Нулевая операция (код 99) над ЗРИ, которого нет в наличии (не инвентаризировался,
    /// не получался). В СНК он не появляется. Логически это ошибка (нулевая операция с
    /// отсутствующим ЗРИ), поэтому помечаем кейс флагом намеренной ошибки.
    /// </summary>
    private static SnkTestCase Zero02_OnNeverOwnedUnit_NotInStock() => new()
    {
        Name = "Z02. Нулевая операция над отсутствующим ЗРИ.",
        EndDate = FinalDate,
        HasIntentionalInventoryErrors = true,
        Operations =
        [
            Anchor(FirstInventoryDate),
            Zero(RechargeDay, "510", "083", "ГИК-5-3", "кобальт-60", "52-1", opCode: "99"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock()))
    };
}
