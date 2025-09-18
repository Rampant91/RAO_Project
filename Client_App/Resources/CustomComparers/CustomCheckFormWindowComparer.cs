using System;
using System.Collections;
using System.Linq;
using Models.CheckForm;

namespace Client_App.Resources.CustomComparers;

public class CustomCheckFormWindowComparer(string propertyName, bool isAscending) : IComparer
{
    public int Compare(object? x, object? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        // Special handling for CheckError properties
        if (x is CheckError checkErrorX && y is CheckError checkErrorY)
        {
            // For "стр." column (Row property) and "Графа" column (Column property)
            if (propertyName is "Row" or "Column")
            {
                string xStr = propertyName == "Row" ? checkErrorX.Row : checkErrorX.Column;
                string yStr = propertyName == "Row" ? checkErrorY.Row : checkErrorY.Column;

                var xFirstNum = ExtractFirstNumber(xStr);
                var yFirstNum = ExtractFirstNumber(yStr);

                if (int.TryParse(xFirstNum, out var xNum) &&
                    int.TryParse(yFirstNum, out var yNum))
                {
                    return isAscending 
                        ? xNum.CompareTo(yNum) 
                        : yNum.CompareTo(xNum);
                }
                return int.TryParse(xFirstNum, out _) ? 1 : -1;
            }
        }

        // Default comparison for other properties
        try
        {
            var property = x.GetType().GetProperty(propertyName);
            if (property != null)
            {
                var xValue = property.GetValue(x);
                var yValue = property.GetValue(y);

                if (xValue == null && yValue == null) return 0;
                if (xValue == null) return isAscending ? -1 : 1;
                if (yValue == null) return isAscending ? 1 : -1;

                if (xValue is IComparable comparableX && yValue is IComparable comparableY)
                {
                    return isAscending 
                        ? comparableX.CompareTo(comparableY) 
                        : comparableY.CompareTo(comparableX);
                }
            }
        }
        catch
        {
            // If any error occurs during comparison, fall back to string comparison
        }

        // Fallback to string comparison
        var xString = x.ToString() ?? string.Empty;
        var yString = y.ToString() ?? string.Empty;
        return isAscending 
            ? string.Compare(xString, yString, StringComparison.OrdinalIgnoreCase) 
            : string.Compare(yString, xString, StringComparison.OrdinalIgnoreCase);
    }

    #region ExtractFirstNumber
    
    private static string ExtractFirstNumber(string input)
    {
        if (string.IsNullOrEmpty(input))
            return "0";

        // Get the part before any comma or dash
        var firstPart = input.Split([',', '-', ' '], StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? "0";

        // Extract only digits
        return new string(firstPart.TakeWhile(char.IsDigit).ToArray());
    }

    #endregion
}