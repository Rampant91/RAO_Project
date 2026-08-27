using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models.Helpers;

public static partial class DashStringHelper
{
    // Explicit set (FormContentRegex.UnicodeDashes) + \p{Pd} + lookalikes outside Pd:
    // U+00AD soft hyphen, U+2212 minus sign, U+02D7 modifier letter minus, U+2796 heavy minus sign.
    [GeneratedRegex(@"[-᠆‐‑‒–—―⸺⸻－﹘﹣－\p{Pd}\u00AD\u2212\u02D7\u2796]+")]
    private static partial Regex DashChars();

    public static string NormalizeDashes(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }

        return DashChars().Replace(value, "-");
    }

    public static string RemoveDashes(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }

        return DashChars().Replace(value, string.Empty);
    }

    public static bool IsDash(string? value) =>
        value is not null && NormalizeDashes(value).Trim() == "-";

    public static bool IsNullOrEmptyOrDash(string? value) =>
        string.IsNullOrEmpty(value) || IsDash(value);

    public static bool IsNullOrWhiteSpaceOrDash(string? value) =>
        string.IsNullOrWhiteSpace(value) || IsDash(value);

    // When an allowed entry is "-", any dash lookalike matches via IsDash.
    public static bool IsOneOf(string? value, params string[] allowed) =>
        IsOneOf(value, (IEnumerable<string>)allowed);

    public static bool IsOneOf(string? value, IEnumerable<string> allowed)
    {
        foreach (var item in allowed)
        {
            if (item == "-")
            {
                if (IsDash(value))
                {
                    return true;
                }
            }
            else if (value == item)
            {
                return true;
            }
        }

        return false;
    }
}
