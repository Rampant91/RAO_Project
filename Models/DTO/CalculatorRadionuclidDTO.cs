using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Models.DTO;

public partial class CalculatorRadionuclidDTO
{
    public required string Name { get; set; }

    public required string Abbreviation { get; set; }

    public required double Halflife { get; set; }

    public required string Unit { get; set; }

    public required string D { get; set; }

    public required string Mza { get; set; }

    private string _activity = "";
    public string Activity
    {
        get => _activity;
        set => _activity = ExponentialString(value);
    }

    #region ExponentialString
    
    private protected static string ExponentialString(string value)
    {
        var tmp = (value ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace('е', 'e');
        tmp = ReplaceDashes(tmp);
        if (tmp != "прим.")
        {
            tmp = tmp.Replace('.', ',');
        }
        if (tmp is "прим." or "-")
        {
            return tmp;
        }
        var doubleStartsWithBrackets = false;
        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            doubleStartsWithBrackets = true;
            tmp = tmp
                .TrimStart('(')
                .TrimEnd(')');
        }
        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-')
            ? "-"
            : string.Empty;
        if (!tmp.Contains('e')
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }
        if (double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                new CultureInfo("ru-RU", useUserOverride: false),
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        return doubleStartsWithBrackets
            ? $"({tmp})"
            : tmp;
    }

    private protected static string ReplaceDashes(string value) =>
        value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };

    #endregion

    #region Regex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();

    #endregion
}