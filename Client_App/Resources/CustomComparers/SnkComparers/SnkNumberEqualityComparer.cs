using System.Collections.Generic;
using System.Text.RegularExpressions;
using Models.Comparers.FormContent;

namespace Client_App.Resources.CustomComparers.SnkComparers;

public partial class SnkNumberEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (x is null || y is null) return false;

        return Normalize(x).Equals(Normalize(y));
    }

    public int GetHashCode(string obj) => Normalize(obj).GetHashCode();

    private static string Normalize(string value)
    {
        if (CheckForEmptyString(value))
        {
            return string.Empty;
        }

        var cleaned = SpecialSymbolsRegex().Replace(value.TrimStart(' ', '0'), "").ToLower();
        return LookalikeCharMapper.ReplaceRuEnLookalikes(cleaned, includeExtendedSnkSet: true);
    }

    private static bool CheckForEmptyString(string? value)
    {
        var normalized = (value ?? string.Empty).ToLower();
        normalized = DashesRegex().Replace(normalized, "");
        normalized = SpecialSymbolsRegex().Replace(normalized, "");

        List<string> validStrings =
        [
            "",
            "-",
            "бн",
            "безномера",
            "нет",
            "нетданных",
            "нд",
            "отсутствует",
            "прим",
            "примечание"
        ];
        return validStrings.Contains(normalized);
    }

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex DashesRegex();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    private static partial Regex SpecialSymbolsRegex();
}
