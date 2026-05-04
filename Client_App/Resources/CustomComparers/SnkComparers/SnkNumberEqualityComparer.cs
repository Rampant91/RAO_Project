using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client_App.Resources.CustomComparers.SnkComparers;

public partial class SnkNumberEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (x is null || y is null) return false;

        var stringsIsEmpty = CheckForEmptyStrings(x, y);
        if (stringsIsEmpty) return true;

        x = SpecialSymbolsRegex()
            .Replace(x.TrimStart(' ', '0'), "")
            .ToLower()
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

        y = SpecialSymbolsRegex()
            .Replace(y.TrimStart(' ', '0'), "")
            .ToLower()
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

        return x.Equals(y);
    }

    private static bool CheckForEmptyStrings(string? str1, string? str2)
    {
        str1 = (str1 ?? string.Empty).ToLower();
        str1 = DashesRegex().Replace(str1, "");
        str1 = SpecialSymbolsRegex().Replace(str1, "");

        str2 = (str2 ?? string.Empty).ToLower();
        str2 = DashesRegex().Replace(str2, "");
        str2 = SpecialSymbolsRegex().Replace(str2, "");

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
        return validStrings.Contains(str1) && validStrings.Contains(str2);
    }

    public int GetHashCode(string obj) => obj.GetHashCode();

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex DashesRegex();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    private static partial Regex SpecialSymbolsRegex();
}
