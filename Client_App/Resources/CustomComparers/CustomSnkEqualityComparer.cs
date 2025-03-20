using AvaloniaEdit;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client_App.Resources.CustomComparers;

public partial class CustomSnkEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        _ = SnkRegex()
            .Replace(x, "")
            .ToLower()
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
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

        _ = SnkRegex()
            .Replace(y, "")
            .ToLower()
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
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

    public int GetHashCode(string obj) => obj.GetHashCode();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    public static partial Regex SnkRegex();
}