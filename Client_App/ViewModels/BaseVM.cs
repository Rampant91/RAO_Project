using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.ViewModels;

public class BaseVM
{
    private protected static string PasFolderPath = @"Y:\Исайчева\Паспорта\Тестовые паспорта";

    private protected static bool ComparePasParam(string? nameDb, string? namePas)
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

    private protected static string ConvertPrimToDash(string? num)
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

    private protected static string ConvertDateToYear(string? date)
    {
        Regex r = new(@"(\d{1,2}[.\/]){1,2}\d{4}");
        if (date is null || !r.IsMatch(date))
            return "0000";
        var matches = r.Matches(date);
        return matches.FirstOrDefault()!.Value[^4..];
    }

    private protected static string TranslateToEng(string pasName)
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

    private protected static string TranslateToRus(string pasName)
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
}