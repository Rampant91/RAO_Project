using System;
using System.Globalization;
using System.Linq;

namespace Models.Comparers.FormContent;

public static class FormExponentialEquality
{
    private const NumberStyles ParseStyles =
        NumberStyles.AllowDecimalPoint
        | NumberStyles.AllowThousands
        | NumberStyles.AllowExponent
        | NumberStyles.AllowLeadingSign;

    /// <summary>
    /// Пусто, «-», «прим.» или числовой ноль (в т.ч. 0, 0.0, 0e+0).
    /// </summary>
    public static bool IsAbsentOrZero(string? value)
    {
        if (FormStringHelper.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        var tmp = FormStringHelper.TrimEdges(value)
            .ToLowerInvariant()
            .Replace('е', 'e');
        tmp = FormContentRegex.UnicodeDashes().Replace(tmp, "-");

        if (tmp == "-")
        {
            return true;
        }

        return TryParse(value, out var parsed) && Math.Abs(parsed) <= double.Epsilon;
    }

    public static bool Equals(string? a, string? b)
    {
        if (IsAbsentOrZero(a) && IsAbsentOrZero(b))
        {
            return true;
        }

        var hasA = TryParse(a, out var valueA);
        var hasB = TryParse(b, out var valueB);

        if (hasA && hasB)
        {
            return FormDoubleEquality.Equals(valueA, valueB);
        }

        return FormTextEquality.Equals(a, b);
    }

    public static bool TryParse(string? value, out double result)
    {
        result = default;

        if (FormStringHelper.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var tmp = FormStringHelper.TrimEdges(value)
            .ToLowerInvariant()
            .Replace('е', 'e');
        tmp = FormContentRegex.UnicodeDashes().Replace(tmp, "-");

        if (tmp is "прим." or "-")
        {
            return false;
        }

        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            tmp = tmp.TrimStart('(').TrimEnd(')');
        }

        tmp = tmp.Replace('.', ',');

        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-') ? "-" : string.Empty;

        if (!tmp.Contains('e', StringComparison.Ordinal)
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }

        return double.TryParse(tmp, ParseStyles, FormComparisonCulture.Russian, out result);
    }
}
