using System;
using System.Collections.Generic;
using OfficeOpenXml;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Сравнение эталонных заголовков аналитической выгрузки с первой строкой листа.
/// </summary>
public static class FormsExcelHeaders
{
    public static bool HeadersMatch(ExcelWorksheet worksheet, IReadOnlyList<string> expected, out string? error)
    {
        error = null;
        if (worksheet.Dimension is null)
        {
            error = "Лист пуст.";
            return false;
        }

        for (var col = 1; col <= expected.Count; col++)
        {
            var actual = Convert.ToString(worksheet.Cells[1, col].Value)?.Trim() ?? "";
            if (!string.Equals(actual, expected[col - 1], StringComparison.Ordinal))
            {
                error =
                    $"Ожидался заголовок «{expected[col - 1]}» в колонке {col}, получено «{actual}».";
                return false;
            }
        }

        return true;
    }
}
