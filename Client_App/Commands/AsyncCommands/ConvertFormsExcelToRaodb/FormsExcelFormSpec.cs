using System.Collections.Generic;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Семейство аналитической выгрузки (Master 1.0 vs 2.0, ключ периода vs года).
/// </summary>
public enum FormsExcelFormFamily
{
    Form1,
    Form2
}

/// <summary>
/// Спецификация одной формы для автодетекта и разбора аналитической выгрузки.
/// </summary>
public sealed class FormsExcelFormSpec
{
    public FormsExcelFormSpec(
        string formNum,
        FormsExcelFormFamily family,
        IReadOnlyList<string> reportColumns,
        IReadOnlyList<string> notesColumns)
    {
        FormNum = formNum;
        Family = family;
        ReportColumns = reportColumns;
        NotesColumns = notesColumns;
    }

    public string FormNum { get; }
    public FormsExcelFormFamily Family { get; }
    public IReadOnlyList<string> ReportColumns { get; }
    public IReadOnlyList<string> NotesColumns { get; }

    public string ReportsSheetPrefix => $"Отчеты {FormNum}";
    public string NotesSheetPrefix => $"Примечания {FormNum}";
}
