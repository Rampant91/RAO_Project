using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Comparers.FormContent;

namespace Client_App.Resources.CustomComparers.SnkComparers;

public partial class SnkRadionuclidsEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (x is null || y is null) return false;

        return ParseToNormalizedSet(x).SetEquals(ParseToNormalizedSet(y));
    }

    public int GetHashCode(string obj)
    {
        var canonicalString = string.Join("|", ParseToNormalizedSet(obj).OrderBy(x => x));
        return canonicalString.GetHashCode();
    }

    private static HashSet<string> ParseToNormalizedSet(string value)
    {
        return value.Split([',', ';'])
            .Select(NormalizeRadionuclide)
            .Where(rad => !string.IsNullOrWhiteSpace(rad))
            .ToHashSet();
    }

    private static string NormalizeRadionuclide(string value)
    {
        return SnkRegex()
            .Replace(value, "")
            .ToLower() is var cleaned
            ? LookalikeCharMapper.ReplaceRuEnLookalikes(cleaned, includeExtendedSnkSet: true)
            : string.Empty;
    }

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    public static partial Regex SnkRegex();
}