using System;
using System.Globalization;

namespace Models.Comparers.FormContent;

public static class FormDateEquality
{
    private const DateTimeStyles ParseStyles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;

    public static bool Equals(string? a, string? b)
    {
        if (TryParse(a, out var dateA) && TryParse(b, out var dateB))
        {
            return dateA == dateB;
        }

        return FormTextEquality.Equals(a, b);
    }

    public static bool TryParse(string? value, out DateOnly date)
    {
        date = default;

        if (FormStringHelper.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = FormContentRegex.Whitespace().Replace(FormStringHelper.TrimEdges(value), string.Empty);

        if (DateOnly.TryParse(normalized, FormComparisonCulture.Russian, ParseStyles, out date))
        {
            return true;
        }

        if (DateTime.TryParse(normalized, FormComparisonCulture.Russian, ParseStyles, out var dateTime))
        {
            date = DateOnly.FromDateTime(dateTime);
            return true;
        }

        return false;
    }
}
