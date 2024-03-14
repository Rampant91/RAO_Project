using System.Collections.Generic;

namespace Client_App.Resources;

internal class CustomStringTitleFormComparer : IComparer<string>
{
    public int Compare(string? str1, string? str2)
    {
        if (str1 is null)
            return 1;
        if (str2 is null)
            return -1;
        str1 = string.Concat(str1.ToLower().Split(' ', '.', ',', '(', ')', '-', '"'));
        str2 = string.Concat(str2.ToLower().Split(' ', '.', ',', '(', ')', '-', '"'));
        return string.CompareOrdinal(str1, str2);
    }
}