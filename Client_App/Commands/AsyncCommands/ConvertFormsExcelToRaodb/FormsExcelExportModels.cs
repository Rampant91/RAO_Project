using System.Collections.Generic;
using Models.Forms;

namespace Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;

/// <summary>
/// Ключ группировки строк аналитической выгрузки в один отчёт (формы 1.x).
/// </summary>
public readonly record struct FormsExcelReportKey(
    string RegNo,
    string Okpo,
    string StartPeriod,
    string EndPeriod,
    byte CorrectionNumber)
{
    public override string ToString() =>
        $"{RegNo}/{Okpo}, {FormatPeriod()}, № кор. {CorrectionNumber}";

    private string FormatPeriod()
    {
        var hasStart = StartPeriod is not "-" and not "";
        var hasEnd = EndPeriod is not "-" and not "";
        return (hasStart, hasEnd) switch
        {
            (true, true) => $"{StartPeriod}–{EndPeriod}",
            (true, false) => $"с {StartPeriod}",
            (false, true) => $"по {EndPeriod}",
            _ => "период не указан"
        };
    }
}

/// <summary>
/// Ключ группировки строк аналитической выгрузки в один отчёт (формы 2.x).
/// </summary>
public readonly record struct FormsExcelForm2ReportKey(
    string RegNo,
    string Okpo,
    int Year,
    byte CorrectionNumber)
{
    public override string ToString() =>
        $"{RegNo}/{Okpo}, {Year}, № кор. {CorrectionNumber}";
}

/// <summary>
/// Одна группа строк Excel = один будущий отчёт формы 1.x.
/// </summary>
public sealed class FormsExcelForm1ReportGroup
{
    public required string FormNum { get; init; }
    public required FormsExcelReportKey Key { get; init; }
    public List<Form> Rows { get; } = [];
    public List<Note> Notes { get; } = [];
}

/// <summary>
/// Одна группа строк Excel = один будущий отчёт формы 2.x.
/// </summary>
public sealed class FormsExcelForm2ReportGroup
{
    public required string FormNum { get; init; }
    public required FormsExcelForm2ReportKey Key { get; init; }
    public List<Form> Rows { get; } = [];
    public List<Note> Notes { get; } = [];
}

/// <summary>
/// Результат разбора листа/книги формы 1.x.
/// </summary>
public sealed class FormsExcelForm1ParseResult
{
    public List<FormsExcelForm1ReportGroup> Groups { get; } = [];
    public List<string> Warnings { get; } = [];
}

/// <summary>
/// Результат разбора листа/книги формы 2.x.
/// </summary>
public sealed class FormsExcelForm2ParseResult
{
    public List<FormsExcelForm2ReportGroup> Groups { get; } = [];
    public List<string> Warnings { get; } = [];
}

public enum FormsExcelOrgResolveStatus
{
    Found,
    NotFound,
    Ambiguous
}
