using System;
using System.Threading;

namespace Models.DBRealization;

/// <summary>
/// Allows intentional bulk <see cref="Collections.Reports"/> deletes (startup orphan cleanup).
/// UI deletes one org at a time and does not need this scope.
/// </summary>
public static class EfReportsDeleteGuard
{
    private static readonly AsyncLocal<bool> AllowBulk = new();

    public static bool AllowBulkReportsDelete => AllowBulk.Value;

    /// <summary>
    /// While disposed, <see cref="DataContext"/> may SaveChanges with more than one Deleted Reports.
    /// </summary>
    public static IDisposable AllowBulkDeletes() => new Scope();

    private sealed class Scope : IDisposable
    {
        private readonly bool _previous;
        private bool _disposed;

        public Scope()
        {
            _previous = AllowBulk.Value;
            AllowBulk.Value = true;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            AllowBulk.Value = _previous;
        }
    }
}
