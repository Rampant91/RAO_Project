using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace Client_App.Resources;

public static partial class StaticStringMethods
{
    #region ComparePasParam
    
    internal static bool ComparePasParam(string? nameDb, string? namePas)
    {
        if (nameDb == null || namePas == null)
        {
            return nameDb == null && namePas == null;
        }

        nameDb = RestrictedSymbolsRegex()
            .Replace(nameDb, string.Empty)
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('о', 'o')
            .Replace('о', '0')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');

        namePas = RestrictedSymbolsRegex()
            .Replace(namePas, string.Empty)
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('о', 'o')
            .Replace('о', '0')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');

        return nameDb.Equals(namePas, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region ConvertDateToYear

    internal static string ConvertDateToYear(string? date)
    {
        return DateTime.TryParse(date, out var dateTime)
            ? dateTime.Year.ToString()
            : "0000";
    }

    #endregion

    #region ConvertPrimToDash

    internal static string ConvertPrimToDash(string? num)
    {
        if (num == null)
        {
            return "";
        }
        if (num.Contains("прим", StringComparison.OrdinalIgnoreCase)
            || num.Equals("бн", StringComparison.OrdinalIgnoreCase)
            || num.Equals("бп", StringComparison.OrdinalIgnoreCase)
            || num.Contains("без", StringComparison.OrdinalIgnoreCase)
            || num.Contains("нет", StringComparison.OrdinalIgnoreCase)
            || num.Contains("отсут", StringComparison.OrdinalIgnoreCase))
        {
            return "-";
        }
        return num;
    }

    #endregion

    #region ConvertToExcel
    
    public static object ConvertToExcelDate(string value, ExcelWorksheet worksheet, int row, int column)
    {
        if (DateTime.TryParse(value, out var dateTime))
        {
            worksheet.Cells[row, column].Style.Numberformat.Format = "dd.mm.yyyy";
        }
        return value is null or "" or "-"
            ? "-"
            : DateTime.TryParse(value, out _)
                ? dateTime.Date
                : value;
    }

    public static object ConvertToExcelDouble(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : double.TryParse(ReplaceE(value), out var doubleValue)
                ? doubleValue
                : value;
    }

    public static object ConvertToExcelInt(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : int.TryParse(ReplaceE(value), out var intValue)
                ? intValue
                : value;
    }

    public static object ConvertToExcelString(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : value;
    }

    private static string ReplaceE(string numberE)
    {
        return numberE.Replace("е", "E").Replace("Е", "E").Replace("e", "E")
            .Replace("(", "").Replace(")", "").Replace(".", ",");
    }

    #endregion

    #region ConvertStringtoExponential

    public static string ConvertStringToExponential(string str) =>
        str.ToLower()
            .Replace(".", ",")
            .Replace("(", "")
            .Replace(")", "")
            .Replace('е', 'e')
            .Trim();

    #endregion

    #region RemoveForbiddenChars

    internal static string RemoveForbiddenChars(string str)
    {
        str = str.Replace(" ", "").Replace(Environment.NewLine, "");
        str = RestrictedSymbolsRegex().Replace(str, "");
        return str;
    }

    #endregion

    #region StringDateReverse

    internal static string StringDateReverse(string date)
    {
        var charArray = date.Replace("_", "0").Replace("/", ".").Split(".");
        if (charArray[0].Length == 1)
            charArray[0] = $"0{charArray[0]}";
        if (charArray[1].Length == 1)
            charArray[1] = $"0{charArray[0]}";
        if (charArray[2].Length == 2)
            charArray[2] = $"20{charArray[0]}";
        Array.Reverse(charArray);
        return string.Join("", charArray);
    }

    #endregion

    #region StringReverse

    internal static string? StringReverse(string? str)
    {
        if (str is null) return null;
        var charArray = str.Replace("_", "0").Replace("/", ".").Split(".");
        Array.Reverse(charArray);
        return string.Join("", charArray);
    }

    #endregion

    #region TranslateToEng

    internal static string TranslateToEng(string pasName)
    {
        Dictionary<string, string> dictRusToEng = new()
        {
            {"а", "a"},
            {"е", "e"},
            {"к", "k"},
            {"м", "m"},
            {"о", "o"},
            {"р", "p"},
            {"с", "c"},
            {"т", "t"},
            {"у", "y"},
            {"х", "x"}
        };
        return pasName.Aggregate("", (current, ch) => 
            current + (dictRusToEng.TryGetValue(ch.ToString(), out var ss)
                ? ss
                : ch));
    }
    
    #endregion

    #region TranslateToRus

    internal static string TranslateToRus(string pasName)
    {
        Dictionary<string, string> dictEngToRus = new()
        {
            {"a", "а"},
            {"e", "е"},
            {"k", "к"},
            {"m", "м"},
            {"o", "о"},
            {"p", "р"},
            {"c", "с"},
            {"t", "т"},
            {"y", "у"},
            {"x", "х"}
        };
        return pasName.Aggregate("", (current, ch) => 
            current + (dictEngToRus.TryGetValue(ch.ToString(), out var ss) 
                ? ss 
                : ch));
    }

    [GeneratedRegex("[\\\\/:*?\"<>|]\\s")]
    private static partial Regex RestrictedSymbolsRegex();

    #endregion
}