using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers;

/// <summary>
/// Компаратор для сравнения рег.№ и ОКПО при сортировке списка организаций.
/// </summary>
public class CustomReportsComparer : IComparer<string>
{
    public int Compare(string? str1, string? str2)
    {
        if (str1 == null && str2 == null) return 0;
        if (str1 == null) return -1;
        if (str2 == null) return 1;

        var charArray1 = str1.ToCharArray();
        var charArray2 = str2.ToCharArray();
        var maxLength = Math.Max(charArray1.Length, charArray2.Length);

        for (var i = 0; i < maxLength; i++)
        {
            if (charArray1.Length <= i) return 1;
            if (charArray2.Length <= i) return -1;
            if (!char.IsNumber(charArray1[i]) && char.IsNumber(charArray2[i]))
            {
                return -1;
            }

            if (char.IsNumber(charArray1[i]) && !char.IsNumber(charArray2[i]))
            {
                return 1;
            }
        }
        return string.CompareOrdinal(str1, str2);
    }
}