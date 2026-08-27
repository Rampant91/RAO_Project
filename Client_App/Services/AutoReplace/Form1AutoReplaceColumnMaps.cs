using System.Collections.Generic;
using Models.Forms.Form1;

namespace Client_App.Services.AutoReplace;

/// <summary>
/// Индексы колонок DataGrid (0 = № п/п) для полей, участвующих в автозамене форм 1.x.
/// </summary>
internal static class Form1AutoReplaceColumnMaps
{
    private static readonly IReadOnlyDictionary<int, string> Form11To14 = new Dictionary<int, string>
    {
        [1] = nameof(Form1.OperationCode),
        [2] = nameof(Form1.OperationDate),
        [3] = "PassportNumber",
    };

    private static readonly IReadOnlyDictionary<int, string> OperationCodeOnly = new Dictionary<int, string>
    {
        [1] = nameof(Form1.OperationCode),
    };

    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<int, string>> ByFormNum =
        new Dictionary<string, IReadOnlyDictionary<int, string>>
        {
            ["1.1"] = Form11To14,
            ["1.2"] = Form11To14,
            ["1.3"] = Form11To14,
            ["1.4"] = Form11To14,
            ["1.5"] = OperationCodeOnly,
            ["1.6"] = OperationCodeOnly,
            ["1.7"] = OperationCodeOnly,
            ["1.8"] = OperationCodeOnly,
            ["1.9"] = OperationCodeOnly,
        };

    public static bool TryGetColumnBinding(string? formNum, int columnIndex, out string columnBinding)
    {
        columnBinding = "";
        if (string.IsNullOrEmpty(formNum)
            || !ByFormNum.TryGetValue(formNum, out var map)
            || !map.TryGetValue(columnIndex, out var binding))
        {
            return false;
        }

        columnBinding = binding;
        return true;
    }
}
