using System.Text.RegularExpressions;

namespace Models.Comparers.FormContent;

internal static partial class FormContentRegex
{
    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    internal static partial Regex SpecialSymbols();

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    internal static partial Regex UnicodeDashes();

    [GeneratedRegex(@"\s+")]
    internal static partial Regex Whitespace();
}
