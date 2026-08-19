using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.DBRealization;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Фоновый прогрев неактивных вкладок главного окна (org page 1 + stubs/reports первой org).
/// Один DBModel, одна очередь — Firebird embedded не любит параллельные соединения.
/// </summary>
public sealed class MainWindowPrefetchService
{
    public static MainWindowPrefetchService Instance { get; } = new();

    private readonly object _gate = new();
    private CancellationTokenSource? _cts;

    private static readonly byte[] TabFormNums = [1, 2, 4, 5];

    public void ScheduleWarmInactiveTabs(byte activeFormNum, string dbPath, int orgPageSize, int reportPageSize)
    {
        var inactiveMasters = TabFormNums
            .Where(f => f != activeFormNum)
            .Select(f => $"{f}.0")
            .ToArray();

        if (inactiveMasters.Length == 0)
            return;

        CancelPending();

        var cts = new CancellationTokenSource();
        lock (_gate)
            _cts = cts;

        _ = Task.Run(() => WarmTabsAsync(dbPath, orgPageSize, reportPageSize, inactiveMasters, cts.Token));
    }

    public void CancelPending()
    {
        lock (_gate)
        {
            try { _cts?.Cancel(); } catch { /* ignore */ }
            _cts = null;
        }
    }

    private static async Task WarmTabsAsync(
        string dbPath,
        int orgPageSize,
        int reportPageSize,
        string[] masterFormNums,
        CancellationToken ct)
    {
        try
        {
            await using var db = new DBModel(dbPath);
            var cache = Forms1WarmCache.Instance;

            foreach (var master in masterFormNums)
            {
                if (ct.IsCancellationRequested)
                    break;

                var orgs = cache.GetOrgPage(db, searchText: null, page: 1, orgPageSize, master);
                var first = orgs.Items.FirstOrDefault();
                if (first == null)
                    continue;

                cache.GetReportPage(db, first.Id, formNumWhiteList: null, page: 1, reportPageSize);
            }
        }
        catch (OperationCanceledException)
        {
            // expected
        }
        catch
        {
            // best-effort
        }
    }
}
