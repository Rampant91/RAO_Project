using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
using System;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Report DataGrid MaxColumnWidth safety (Avalonia 11.2.6).
/// <para>
/// <b>Fixed crash:</b> opening report forms threw <c>ArgumentException: value cannot be set to infinity</c>
/// with the debugger on the last <c>DataGridTemplateColumn</c>. Root cause was
/// <c>MaxColumnWidth="500"</c> in the <c>DataGrid.table</c> style in App.axaml: styles apply after
/// <c>Columns</c> are parsed, so changing the default Infinity to 500 runs
/// <c>OnColumnMaxWidthChanged</c> → <c>new DataGridLength(Infinity, …)</c> which is illegal.
/// </para>
/// <para>
/// <b>Fix in repo:</b> remove MaxColumnWidth from the style; set <c>MaxColumnWidth="500"</c> on each
/// report <c>DataGrid</c> element (attribute before Columns). This guard is a fallback only when
/// <c>Columns</c> is still empty — never assign a finite max after columns exist while current max is Infinity.
/// </para>
/// <para>See also: <c>.cursor/rules/datagrid-report-table-width.mdc</c>,
/// <c>DataGridReportTableLayoutGuardTests</c>.</para>
/// </summary>
internal static class DataGridMaxColumnWidthGuard
{
    public static bool WouldThrowOnAssign(int columnCount, double currentMax) =>
        columnCount > 0 && double.IsInfinity(currentMax);

    public static bool TryApplyReportTableCap(AvaloniaDataGrid grid)
    {
        if (!grid.Classes.Contains("table"))
        {
            return false;
        }

        return TrySetFiniteMax(grid, DataGridColumnWidthClamp.FallbackMax);
    }

    public static bool TrySetFiniteMax(AvaloniaDataGrid grid, double max)
    {
        if (!double.IsFinite(max) || max <= 0)
        {
            return false;
        }

        var current = grid.MaxColumnWidth;
        if (WouldThrowOnAssign(grid.Columns.Count, current))
        {
            return false;
        }

        if (double.IsFinite(current) && Math.Abs(current - max) < 0.5)
        {
            return false;
        }

        grid.MaxColumnWidth = max;
        return true;
    }
}
