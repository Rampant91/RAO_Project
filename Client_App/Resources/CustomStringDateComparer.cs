using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client_App.Resources;

internal partial class CustomStringDateComparer : IComparer<string>
{
    public CustomStringDateComparer(IComparer<string> baseComparer) { }
    public int Compare(string? date1, string? date2)
    {
        if (string.IsNullOrEmpty(date1) || date1 is "-")
            return 1;
        if (string.IsNullOrEmpty(date2) || date2 is "-")
            return -1;
        var r = DateStringRegex();

        if (DateOnly.TryParse(date1, out var dateOnly1) && DateOnly.TryParse(date2, out var dateOnly2))
        {
            return string.CompareOrdinal(dateOnly1.ToShortDateString(), dateOnly2.ToShortDateString());
        }
        if (!r.IsMatch(date1) && !r.IsMatch(date2))
            return string.CompareOrdinal(date1, date2);
        if (!r.IsMatch(date2))
            return 1;
        return -1;
    }

    [GeneratedRegex(@"^(\d{1,2})\.(\d{1,2})\.(\d{2,4})$")]
    private static partial Regex DateStringRegex();
}