using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Каталог форм, поддерживаемых конвертером аналитической выгрузки → .RAODB.
/// Поддерживаются формы 1.1–1.9 и 2.1–2.12.
/// </summary>
public static class FormsExcelFormCatalog
{
    private static readonly Dictionary<string, FormsExcelFormSpec> ByFormNum;

    static FormsExcelFormCatalog()
    {
        var notes1 = FormsExcelExportHeadersForm11.NotesColumns;
        var notes2 = FormsExcelExportHeadersForm2x.NotesColumns;
        var specs = new[]
        {
            new FormsExcelFormSpec("1.1", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm11.ReportColumns, notes1),
            new FormsExcelFormSpec("1.2", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns12, notes1),
            new FormsExcelFormSpec("1.3", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns13, notes1),
            new FormsExcelFormSpec("1.4", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns14, notes1),
            new FormsExcelFormSpec("1.5", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns15, notes1),
            new FormsExcelFormSpec("1.6", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns16, notes1),
            new FormsExcelFormSpec("1.7", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns17, notes1),
            new FormsExcelFormSpec("1.8", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns18, notes1),
            new FormsExcelFormSpec("1.9", FormsExcelFormFamily.Form1, FormsExcelExportHeadersForm1x.ReportColumns19, notes1),
            new FormsExcelFormSpec("2.1", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns21, notes2),
            new FormsExcelFormSpec("2.2", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns22, notes2),
            new FormsExcelFormSpec("2.3", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns23, notes2),
            new FormsExcelFormSpec("2.4", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns24, notes2),
            new FormsExcelFormSpec("2.5", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns25, notes2),
            new FormsExcelFormSpec("2.6", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns26, notes2),
            new FormsExcelFormSpec("2.7", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns27, notes2),
            new FormsExcelFormSpec("2.8", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns28, notes2),
            new FormsExcelFormSpec("2.9", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns29, notes2),
            new FormsExcelFormSpec("2.10", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns210, notes2),
            new FormsExcelFormSpec("2.11", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns211, notes2),
            new FormsExcelFormSpec("2.12", FormsExcelFormFamily.Form2, FormsExcelExportHeadersForm2x.ReportColumns212, notes2)
        };

        ByFormNum = specs.ToDictionary(s => s.FormNum, StringComparer.Ordinal);
    }

    public static IReadOnlyCollection<FormsExcelFormSpec> All => ByFormNum.Values;

    public static bool TryGet(string formNum, out FormsExcelFormSpec? spec) =>
        ByFormNum.TryGetValue(formNum.Trim(), out spec);

    public static bool IsSupported(string formNum) => ByFormNum.ContainsKey(formNum.Trim());
}
