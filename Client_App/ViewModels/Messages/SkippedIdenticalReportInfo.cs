namespace Client_App.ViewModels.Messages;

/// <summary>
/// Информация об отчёте, не импортированном как полная копия имеющегося в базе.
/// </summary>
public sealed class SkippedIdenticalReportInfo
{
    public string RegNum { get; init; } = "";
    public string Okpo { get; init; } = "";
    public string FormNum { get; init; } = "";
    public string StartPeriod { get; init; } = "";
    public string EndPeriod { get; init; } = "";
}
