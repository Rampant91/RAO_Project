namespace Models.Comparers.FormContent;

public static class FormTextEquality
{
    public static bool Equals(string? a, string? b) =>
        Normalize(a) == Normalize(b);

    public static string Normalize(string? value)
    {
        if (FormStringHelper.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var tmp = FormStringHelper.TrimEdges(value);
        tmp = FormContentRegex.UnicodeDashes().Replace(tmp, "-");
        tmp = FormContentRegex.SpecialSymbols().Replace(tmp, string.Empty);
        return LookalikeCharMapper.ReplaceRuEnLookalikes(tmp);
    }
}
