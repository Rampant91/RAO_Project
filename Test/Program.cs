using System;
using System.Text.RegularExpressions;

namespace Test;

class Program
{
    static void Main(string[] args)
    {
        var fullPath = "55555_22222222_2.0_1.2.2.8#1.raodb";
        MatchCollection matches = Regex.Matches(fullPath, @"(.+)#(\d+)(?=\.raodb)");
        if (matches.Count > 0)
        {
            foreach (Match match in matches)
            {
                if (!int.TryParse(match.Groups[2].Value, out var index)) return;
                fullPath = match.Groups[1].Value + $"#{index + 1}.raodb";
            }
        }
        Console.ReadKey();

    }
}