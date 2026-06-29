using System.Collections.Generic;
using System.Linq;
using static Client_App.Commands.AsyncCommands.ExcelExport.Snk.ExcelExportSnkBaseAsyncCommand;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk.Testing;

internal static class SnkTestOperationSplitter
{
    public static (
        List<ShortFormDTO> Inventory,
        List<ShortFormDTO> PlusMinus,
        List<ShortFormDTO> Recharge)
        Split(string formNum, IReadOnlyList<SnkTestOperationSpec> operations)
    {
        List<ShortFormDTO> inventory = [];
        List<ShortFormDTO> plusMinus = [];
        List<ShortFormDTO> recharge = [];

        var id = 1;
        foreach (var spec in operations.OrderBy(x => x.OpDate).ThenBy(x => x.OpCode))
        {
            var dto = ToShortFormDto(spec, id++);

            if (ExcelExportSnkTestHarness.IsInventoryOperation(spec.OpCode))
            {
                inventory.Add(dto);
            }
            else if (ExcelExportSnkTestHarness.IsRechargeOperation(spec.OpCode))
            {
                recharge.Add(dto);
            }
            else if (ExcelExportSnkTestHarness.IsPlusMinusOperation(formNum, spec.OpCode))
            {
                plusMinus.Add(dto);
            }
        }

        return (inventory, plusMinus, recharge);
    }

    public static SnkStockSnapshot ToSnapshot(ShortFormDTO dto) =>
        new(
            dto.PasNum,
            dto.FacNum,
            dto.Type,
            dto.Radionuclids,
            dto.PackNumber,
            dto.OpCode,
            dto.OpDate,
            dto.Quantity);

    private static ShortFormDTO ToShortFormDto(SnkTestOperationSpec spec, int id) =>
        new()
        {
            Id = id,
            NumberInOrder = id,
            OpCode = spec.OpCode,
            OpDate = spec.OpDate,
            PasNum = spec.PasNum,
            FacNum = spec.FacNum,
            Type = spec.Type,
            Radionuclids = spec.Radionuclids,
            PackNumber = spec.PackNumber,
            Quantity = spec.Quantity,
            RepDto = new ShortReportDTO(1, spec.OpDate, spec.OpDate)
        };
}
