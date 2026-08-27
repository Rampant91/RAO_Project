using System.Collections.Generic;
using System.Text.RegularExpressions;
using Models.Comparers.FormContent;

namespace Client_App.Resources.CustomComparers.SnkComparers;

public partial class SnkEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return true;

        if (x is null || y is null) return false;

        x = SnkRegex()
            .Replace(x, "")
            .ToLower();
        x = LookalikeCharMapper.ReplaceRuEnLookalikes(x, includeExtendedSnkSet: true);

        y = SnkRegex()
            .Replace(y, "")
            .ToLower();
        y = LookalikeCharMapper.ReplaceRuEnLookalikes(y, includeExtendedSnkSet: true);

        return x.Equals(y);
    }

    public int GetHashCode(string obj) => obj.GetHashCode();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    public static partial Regex SnkRegex();
}