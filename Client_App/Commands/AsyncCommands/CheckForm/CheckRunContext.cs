using System;
using System.Threading;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Контекст одного прогона проверки: кэш запросов к БД, прогресс по строкам.
/// </summary>
public sealed class CheckRunContext : IDisposable
{
    private static readonly AsyncLocal<CheckRunContext?> ActiveScope = new();

    public static CheckRunContext? Active => ActiveScope.Value;

    private readonly bool _ownsQueryDb;
    private bool _disposed;

    public CheckRunContext(ReportCheckProgress? progress = null, DBModel? queryDb = null)
    {
        Progress = progress;
        QueryDb = queryDb;
        _ownsQueryDb = queryDb == null;
        if (_ownsQueryDb)
        {
            QueryDb = new DBModel(StaticConfiguration.DBPath);
        }
    }

    public ReportCheckProgress? Progress { get; }

    public DBModel? QueryDb { get; }

    /// <summary>Кэш результата <see cref="CheckBase"/> sibling-query для отчёта.</summary>
    public bool? EarlierSiblingReportCache { get; set; }

    public int RowProgressInterval { get; init; } = 16;

    public IDisposable EnterScope()
    {
        ActiveScope.Value = this;
        return this;
    }

    public void NotifyRowProgress(int currentRow, int totalRows)
    {
        if (totalRows <= 0)
        {
            return;
        }

        var interval = Math.Max(1, Math.Min(RowProgressInterval, totalRows / 20 + 1));
        if (currentRow != totalRows && currentRow % interval != 0)
        {
            return;
        }

        Progress?.OnCheckRow(currentRow, totalRows);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (ActiveScope.Value == this)
        {
            ActiveScope.Value = null;
        }

        if (_ownsQueryDb)
        {
            QueryDb?.Dispose();
        }
    }
}
