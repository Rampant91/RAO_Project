using System.Collections.Generic;
using Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

namespace Test.Snk;

/// <summary>Группа Z — нулевые операции.</summary>
internal static partial class SnkTestCases
{
    private static IEnumerable<SnkTestCase> ZeroOperationCases()
    {
        yield return Zero01_OnUnitInStock_NoEffect();
        yield return Zero02_OnNeverOwnedUnit_NotInStock();
    }

    /// <summary>Z01. op.64 над 510 в наличии — без эффекта.</summary>
    private static SnkTestCase Zero01_OnUnitInStock_NoEffect() => new()
    {
        Name = "Z01. Нулевая операция над ЗРИ в наличии (без эффекта).",
        EndDate = FinalDate,
        Operations =
        [
            ..FullInventoryOn(FirstInventoryDate,
                Inv(FirstInventoryDate, "510", "083", "ГИК-5-3", "кобальт-60", "52-1")),
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
    /// Z02. op.99 над отсутствующим 510 — без эффекта на СНК; ошибка типа 8 (см. также E09).
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
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.ZeroOperationWithUnInventoriedUnit,
                    "510", "083", "ГИК-5-3", "кобальт-60", "52-1",
                    opCode: "99", opDate: RechargeDay)))
    };
}
