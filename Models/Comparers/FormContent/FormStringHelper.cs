namespace Models.Comparers.FormContent;

internal static class FormStringHelper
{
    public static string TrimEdges(string? value) => (value ?? string.Empty).Trim();

    public static bool IsNullOrWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value);
}
