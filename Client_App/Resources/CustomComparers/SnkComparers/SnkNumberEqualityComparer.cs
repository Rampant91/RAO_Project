using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        return ReplaceSimilarSymbols(
            SpecialSymbolsRegex().Replace(value.TrimStart(' ', '0'), "").ToLower());
    }

    private static string ReplaceSimilarSymbols(string value)
    {
        return value
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('з', '3')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('н', 'h')
            .Replace('о', 'o')
            .Replace('0', 'o')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');
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
