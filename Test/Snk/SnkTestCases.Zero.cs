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
                Inv(FirstInventoryDate, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            Zero(RechargeDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedSnkStock =
        [
            AnchorStock(),
            Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11"),
        ],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")),
            On(FinalDate, AnchorStock(), Stock("P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11")))
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
            Zero(RechargeDay, "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11", opCode: "99"),
        ],
        ExpectedSnkStock = [AnchorStock()],
        ExpectedInventoryStockByDate = ByDate(
            On(FirstInventoryDate, AnchorStock()),
            On(FinalDate, AnchorStock())),
        ExpectedInventoryErrorsByDate = ErrorsByDate(
            ErrOn(FinalDate,
                Err(SnkInventoryErrorType.ZeroOperationWithUnInventoriedUnit,
                    "P-101", "F-083", "Тип-М1", "кобальт-60", "УКТ-11",
                    opCode: "99", opDate: RechargeDay)))
    };
}
