using AvaloniaDataGrid = Avalonia.Controls.DataGrid;
namespace Client_App.Behaviors.TableHeader;

/// <summary>
/// Преобразование ширин колонок AvaloniaDataGrid в ширины ячеек кастомной шапки.
/// </summary>
internal static class TableHeaderColumnWidth
{
    public static double FromDataGridDisplayWidth(
        double displayValue,
        TableHeaderLayoutMetrics metrics,
        int columnIndex)
    {
        if (!double.IsFinite(displayValue) || displayValue <= 0) return displayValue;
        if (metrics.HasHorizontalScroll) return displayValue;
        if (!ShouldApplyBorderCompensation(metrics, columnIndex)) return displayValue;
        return displayValue - metrics.BorderCompensation;
    }

    /// <summary>
    /// Шапка рисует границы через Border (1px). Без компенсации col 0 чуть шире AvaloniaDataGrid.
    /// При frozen≥2 скроллируемая шапка начинается с col 1 — компенсация только у col 0,
    /// иначе на стыке frozen/scroll накапливается рассинхрон.
    /// </summary>
    private static bool ShouldApplyBorderCompensation(TableHeaderLayoutMetrics metrics, int columnIndex)
    {
        if (columnIndex == 0) return true;
        if (metrics.FrozenColumnCount >= 2) return false;
        return true;
    }
}
