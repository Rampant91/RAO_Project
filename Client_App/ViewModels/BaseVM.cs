using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

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