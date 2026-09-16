using System;
using System.Globalization;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Разбор ячеек аналитического Excel (даты / «-» / числа) без привязки к печатному ExcelGetRow.
/// </summary>
public static class FormsExcelCellConverters
{
    private static readonly CultureInfo Ru = new("ru-RU", useUserOverride: false);

    /// <summary>
    /// Нормализует дату к <c>dd.MM.yyyy</c> для ключей группировки и полей отчёта.
    /// </summary>
    public static string NormalizeDate(object? value, string? cellText = null)
    {
        switch (value)
        {
            case DateTime dt:
                return dt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            case DateOnly d:
                return d.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }

        foreach (var candidate in new[] { cellText, Convert.ToString(value) })
        {
            var str = candidate?.Trim();
            if (string.IsNullOrEmpty(str) || str == "-")
            {
                continue;
            }

            if (DateTime.TryParse(str, Ru, DateTimeStyles.None, out var parsed)
                || DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed)
                || DateTime.TryParse(str, out parsed))
            {
                return parsed.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }

            return str;
        }

        return "-";
    }

    public static string CellString(object? value)
    {
        var str = Convert.ToString(value)?.Trim();
        return string.IsNullOrEmpty(str) ? "" : str;
    }

    public static string CellStringOrDash(object? value)
    {
        var str = CellString(value);
        return str == "" ? "-" : str;
    }

    public static bool IsDashOrEmpty(object? value)
    {
        var str = CellString(value);
        return str is "" or "-";
    }

    public static int? ParseNullableInt(object? value)
    {
        if (IsDashOrEmpty(value)) return null;
        if (value is int i) return i;
        if (value is long l) return (int)l;
        if (value is double d) return (int)d;
        var str = RemoveFormulaPrefix(CellString(value));
        return int.TryParse(str, NumberStyles.Any, Ru, out var parsed)
               || int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
            ? parsed
            : null;
    }

    public static short? ParseNullableShort(object? value)
    {
        if (IsDashOrEmpty(value)) return null;
        if (value is short s) return s;
        if (value is int i) return (short)i;
        if (value is double d) return (short)d;
        var str = RemoveFormulaPrefix(CellString(value));
        return short.TryParse(str, NumberStyles.Any, Ru, out var parsed)
               || short.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
            ? parsed
            : null;
    }

    public static byte? ParseNullableByte(object? value)
    {
        if (IsDashOrEmpty(value)) return null;
        if (value is byte b) return b;
        if (value is int i) return (byte)i;
        if (value is double d) return (byte)d;
        var str = RemoveFormulaPrefix(CellString(value));
        return byte.TryParse(str, NumberStyles.Any, Ru, out var parsed)
               || byte.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
            ? parsed
            : null;
    }

    public static float? ParseNullableFloat(object? value)
    {
        if (IsDashOrEmpty(value)) return null;
        if (value is float f) return f;
        if (value is double d) return (float)d;
        if (value is decimal m) return (float)m;
        var str = RemoveFormulaPrefix(CellString(value)).Replace('е', 'E').Replace('Е', 'E');
        return float.TryParse(str, NumberStyles.Any, Ru, out var parsed)
               || float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
            ? parsed
            : null;
    }

    public static byte? ParseCorrectionNumber(object? value)
    {
        if (IsDashOrEmpty(value)) return null;
        if (value is byte b) return b;
        if (value is int i && i is >= 0 and <= byte.MaxValue) return (byte)i;
        if (value is double d && d is >= 0 and <= byte.MaxValue) return (byte)d;
        var str = RemoveFormulaPrefix(CellString(value));
        return byte.TryParse(str, NumberStyles.Any, Ru, out var parsed)
               || byte.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
            ? parsed
            : null;
    }

    /// <summary>
    /// Активность как в <c>Form.ConvertFromExcelDouble</c> (научная запись, ru-RU).
    /// </summary>
    public static string ParseActivity(object? value)
    {
        if (IsDashOrEmpty(value)) return "-";

        double doubleValue;
        switch (value)
        {
            case double d:
                doubleValue = d;
                break;
            case float f:
                doubleValue = f;
                break;
            case decimal m:
                doubleValue = (double)m;
                break;
            case int i:
                doubleValue = i;
                break;
            default:
            {
                var str = RemoveFormulaPrefix(CellString(value))
                    .Replace('е', 'E').Replace('Е', 'E').Replace('e', 'E');
                if (!(double.TryParse(str, NumberStyles.Any, Ru, out doubleValue)
                      || double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleValue)))
                {
                    return CellStringOrDash(value);
                }

                break;
            }
        }

        return doubleValue.ToString("0.00######################################################e+00", Ru);
    }

    private static string RemoveFormulaPrefix(string value) =>
        value.StartsWith('=') ? value[1..] : value;
}
