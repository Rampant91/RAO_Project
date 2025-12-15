using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace Client_App.Resources;
 
public static partial class StaticStringMethods
{
    #region ComparePasParam

    /// <summary>
    /// У обоих входных строчек удаляет запрещённые символы, заменяет похожие русские символы на английские и сравнивает строчки.
    /// </summary>
    /// <param name="nameDb">Первая строчка для сравнения.</param>
    /// <param name="namePas">Вторая строчка для сравнения.</param>
    /// <returns>Флаг, равны ли две строчки.</returns>
    public static bool ComparePasParam(string? nameDb, string? namePas)
    {
        if (nameDb == null || namePas == null)
        {
            return nameDb == null && namePas == null;
        }

        nameDb = RestrictedSymbolsRegex().Replace(nameDb, string.Empty)
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('о', '0')
            .Replace('о', '0')
            .Replace('р', 'p')
            .Replace('с', 'c')
            .Replace('т', 't')
            .Replace('у', 'y')
            .Replace('х', 'x');

        namePas = RestrictedSymbolsRegex().Replace(namePas, string.Empty)
            .Replace('а', 'a')
            .Replace('б', 'b')
            .Replace('в', 'b')
            .Replace('г', 'r')
            .Replace('е', 'e')
            .Replace('ё', 'e')
            .Replace('к', 'k')
            .Replace('м', 'm')
            .Replace('о', '0')
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

    /// <summary>
    /// Преобразует строчку с датой в строчку с годом или при отсутствии возможности возвращает строчку "0000".
    /// </summary>
    /// <param name="date">Строчка с датой для преобразования.</param>
    /// <returns>Преобразованная строчка из 4 символов или "0000".</returns>
    public static string ConvertDateToYear(string? date)
    {
        return DateOnly.TryParse(date, out var dateTime)
            ? dateTime.Year.ToString()
            : "0000";
    }

    #endregion

    #region ConvertPrimToDash

    /// <summary>
    /// Заменяет null на пустую строчку, заменяет определённые вариации на "-" и возвращает строчку.
    /// </summary>
    /// <param name="str">Строчка для преобразования.</param>
    /// <returns>Преобразованная строка.</returns>
    public static string ConvertPrimToDash(string? str)
    {
        if (str == null)
        {
            return string.Empty;
        }
        if (str.Contains("прим", StringComparison.OrdinalIgnoreCase)
            || str.Equals("бн", StringComparison.OrdinalIgnoreCase)
            || str.Equals("бп", StringComparison.OrdinalIgnoreCase)
            || str.Contains("без", StringComparison.OrdinalIgnoreCase)
            || str.Contains("нет", StringComparison.OrdinalIgnoreCase)
            || str.Contains("отсут", StringComparison.OrdinalIgnoreCase))
        {
            return "-";
        }
        return str;
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

    public static object ConvertToExcelShort(string value)
    {
        return value is null or "" or "-"
            ? "-"
            : short.TryParse(ReplaceE(value), out var shortValue)
                ? shortValue
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

    internal static string RemoveForbiddenChars(string? str)
    {
        str ??= string.Empty;
        str = str.Replace(" ", "").Replace(Environment.NewLine, "");
        str = RestrictedSymbolsRegex().Replace(str, "");
        return str;
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

    #endregion

    [GeneratedRegex("[\\\\/:*?\"<>|\\s+]")]
    private static partial Regex RestrictedSymbolsRegex();
}