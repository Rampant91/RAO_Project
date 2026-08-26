using Models.Forms;
using System;

namespace Models.Comparers.FormContent;

internal static class FormStringHelper
{
    public static string TrimEdges(string? value) => (value ?? string.Empty).Trim();

    public static bool IsNullOrWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value);

    #region ConvertStringToByte

    public static byte? ConvertStringToByte(string str)
    {
        if (byte.TryParse(str, out var result))
        {
            return result;
        }
        else
            return null;
    }
    #endregion

    #region ConvertStringToInt
    public static int? ConvertStringToInt(string str)
    {
        str = Form.RemoveExcelFormulaPrefix(str);
        if (int.TryParse(str, out var result))
        {
            return result;
        }
        else
            return null;
    }
    #endregion

    #region ConvertStringToShort
    public static short? ConvertStringToShort(string str)
    {
        if (short.TryParse(str, out var result))
        {
            return result;
        }
        else
            return null;
    }
    #endregion

    #region ConvertStringToFloat
    public static float? ConvertStringToFloat(string str)
    {
        str = Form.RemoveExcelFormulaPrefix(str);
        if (float.TryParse(str, out var result))
        {
            return result;
        }
        else
            return null;
    }
    #endregion

    #region ConvertStringToDateOnly
    public static DateOnly? ConvertStringToDateOnly(string str)
    {
        str = Form.RemoveExcelFormulaPrefix(str);
        if (DateOnly.TryParse(str, out var result))
        {
            return result;
        }
        else
            return null;
    }
    #endregion
}
