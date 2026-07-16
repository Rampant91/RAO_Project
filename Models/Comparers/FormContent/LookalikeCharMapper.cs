using System.Text;

namespace Models.Comparers.FormContent;

public static class LookalikeCharMapper
{
    public static string ReplaceRuEnLookalikes(string? value, bool includeExtendedSnkSet = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var source = value.ToLowerInvariant();
        var sb = new StringBuilder(source.Length);

        foreach (var ch in source)
        {
            sb.Append(ch switch
            {
                'а' => 'a',
                'б' => 'b',
                'в' => 'b',
                'г' => 'r',
                'е' => 'e',
                'ё' => 'e',
                'к' => 'k',
                'м' => 'm',
                'н' => 'h',
                'о' => 'o',
                '0' => 'o',
                'р' => 'p',
                'с' => 'c',
                'т' => 't',
                'у' => 'y',
                'х' => 'x',
                'з' when includeExtendedSnkSet => '3',
                _ => ch
            });
        }

        return sb.ToString();
    }
}
