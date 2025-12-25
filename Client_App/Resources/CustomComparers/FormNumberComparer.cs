using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers;

/// <summary>
/// Компаратор для сортировки номеров форм в правильном порядке
/// </summary>
public class FormNumberComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        // Разбиваем строки на части (например, "2.10" -> ["2", "10"])
        var xParts = x.Split('.');
        var yParts = y.Split('.');

        // Сравниваем по частям
        for (int i = 0; i < Math.Max(xParts.Length, yParts.Length); i++)
        {
            if (i >= xParts.Length) return -1;
            if (i >= yParts.Length) return 1;

            if (int.TryParse(xParts[i], out var xNum) && int.TryParse(yParts[i], out var yNum))
            {
                var comparison = xNum.CompareTo(yNum);
                if (comparison != 0) return comparison;
            }
            else
            {
                var comparison = string.Compare(xParts[i], yParts[i], StringComparison.Ordinal);
                if (comparison != 0) return comparison;
            }
        }

        return 0;
    }
}
