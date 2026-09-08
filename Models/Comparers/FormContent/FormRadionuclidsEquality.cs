using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Comparers.FormContent;

public static class FormRadionuclidsEquality
{
    public static bool Equals(string? a, string? b) =>
        Parse(a).SequenceEqual(Parse(b));

    /// <summary>
    /// Каноническая строка радионуклидов для fingerprint (порядок как в Parse).
    /// </summary>
    public static string NormalizeForFingerprint(string? value) =>
        string.Join(';', Parse(value));

    private static List<string> Parse(string? value)
    {
        if (FormStringHelper.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value!
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(FormTextEquality.Normalize)
            .Where(x => x.Length > 0)
            .ToList();
    }
}
