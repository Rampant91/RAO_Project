using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client_App.Resources;

internal static class StaticStringMethods
{
    #region ComparePasParam
    
    internal static bool ComparePasParam(string? nameDb, string? namePas)
    {
        if (nameDb == null || namePas == null)
        {
            return nameDb == null && namePas == null;
        }
        nameDb = Regex.Replace(nameDb, "[\\\\/:*?\"<>|]", "");
        nameDb = Regex.Replace(nameDb, "\\s+", "");
        namePas = Regex.Replace(namePas, "[\\\\/:*?\"<>|]", "");
        namePas = Regex.Replace(namePas, "\\s+", "");
        return nameDb.Equals(namePas, StringComparison.OrdinalIgnoreCase)
               || TranslateToEng(nameDb).Equals(TranslateToEng(namePas), StringComparison.OrdinalIgnoreCase)
               || TranslateToRus(nameDb).Equals(TranslateToRus(namePas), StringComparison.OrdinalIgnoreCase);
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

    #region RemoveForbiddenChars

    internal static string RemoveForbiddenChars(string str)
    {
        str = str.Replace(Environment.NewLine, "").Trim();
        str = Regex.Replace(str, "[\\\\/:*?\"<>|]", "");
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
            {"А", "A"},
            {"е", "e"},
            {"Е", "E"},
            {"к", "k"},
            {"К", "K"},
            {"м", "m"},
            {"М", "M"},
            {"о", "o"},
            {"О", "O"},
            {"р", "p"},
            {"Р", "P"},
            {"с", "c"},
            {"С", "C"},
            {"Т", "T"},
            {"у", "y"},
            {"У", "Y"},
            {"х", "x"},
            {"Х", "X"},
        };
        var newPasName = "";
        foreach (var ch in pasName)
        {
            if (dictRusToEng.TryGetValue(ch.ToString(), out var ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    }
    
    #endregion

    #region TranslateToRus

    internal static string TranslateToRus(string pasName)
    {
        Dictionary<string, string> dictEngToRus = new()
        {
            {"a", "а"},
            {"A", "А"},
            {"e", "е"},
            {"E", "Е"},
            {"k", "к"},
            {"K", "К"},
            {"m", "м"},
            {"M", "М"},
            {"o", "о"},
            {"O", "О"},
            {"p", "р"},
            {"P", "Р"},
            {"c", "с"},
            {"C", "С"},
            {"T", "Т"},
            {"y", "у"},
            {"Y", "У"},
            {"x", "х"},
            {"X", "Х"},
        };
        var newPasName = "";
        foreach (var ch in pasName)
        {
            if (dictEngToRus.TryGetValue(ch.ToString(), out var ss)) newPasName += ss;
            else newPasName += ch;
        }
        return newPasName;
    } 

    #endregion
}