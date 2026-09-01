using System;
using System.Collections.Generic;

namespace Client_App.Behaviors.DataGrid;

/// <summary>
/// Safe min/max for AvaloniaDataGrid column widths. Default MaxWidth is Infinity and must not reach DataGridLength.
/// </summary>
internal static class DataGridColumnWidthClamp
{
    public const double FallbackMin = 20;
    public const double FallbackMax = 500;

    public static double Clamp(
        double width,
        double columnMin,
        double columnMax,
        double gridMin,
        double gridMax)
    {
        var min = PreferFinitePositive(columnMin, PreferFinitePositive(gridMin, FallbackMin));
        var max = PreferFinitePositive(columnMax, PreferFinitePositive(gridMax, FallbackMax));
        if (max < min)
        {
            max = min;
        }

        if (!double.IsFinite(width) || width <= 0)
        {
            return min;
        }

        return Math.Clamp(width, min, max);
    }

    public static List<double> SanitizeSavedWidths(IEnumerable<double> widths)
    {
        var result = new List<double>();
        foreach (var width in widths)
        {
            result.Add(double.IsFinite(width) && width > 0 ? width : 0);
        }

        return result;
    }

    private static double PreferFinitePositive(double value, double fallback) =>
        double.IsFinite(value) && value > 0 ? value : fallback;
}
