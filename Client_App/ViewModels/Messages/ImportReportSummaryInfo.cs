namespace Client_App.ViewModels.Messages;

public enum ImportSummaryFormGroup
{
    Form1,
    Form2,
    Form3,
    Form4,
    Form5
}

/// <summary>
/// Строка сводки об импортированном или пропущенном отчёте.
/// </summary>
public sealed class ImportReportSummaryInfo
{
    public const string ReasonFullCopy = "Полная копия";
    public const string ReasonLowerCorrection = "Меньший номер корректировки";
    public const string ReasonUserCancelled = "Отменено пользователем";

    public string OrgColumn1 { get; init; } = "";
    public string OrgColumn2 { get; init; } = "";
    public string FormNum { get; init; } = "";
    public string Year { get; init; } = "";
    public string StartPeriod { get; init; } = "";
    public string EndPeriod { get; init; } = "";
    public string Reason { get; init; } = "";
}
