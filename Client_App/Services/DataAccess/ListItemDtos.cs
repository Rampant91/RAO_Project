namespace Client_App.Services.DataAccess;

/// <summary>
/// Проекция организации для грида главного окна (без полной сущности Reports).
/// </summary>
public sealed class OrgListItem
{
    public required int ReportsId { get; init; }
    public required int? MasterReportId { get; init; }
    public required string FormNum { get; init; }
    public string? RegNo { get; init; }
    public string? Okpo { get; init; }
    public string? ShortJurLico { get; init; }
}

/// <summary>
/// Проекция отчёта для грида отчётов организации.
/// </summary>
public sealed class ReportListItem
{
    public required int ReportId { get; init; }
    public required int ReportsId { get; init; }
    public required string FormNum { get; init; }
    public string? StartPeriod { get; init; }
    public string? EndPeriod { get; init; }
    public string? Year { get; init; }
    public short CorrectionNumber { get; init; }
}
