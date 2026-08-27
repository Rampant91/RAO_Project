using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_App.Services.DataAccess;

/// <summary>
/// Cached ordered DB Ids for one 1.x report. Unsaved add/delete does not change this list
/// (Deleted still in DB; Added are merged in memory). Invalidate after Save/Discard/reload.
/// </summary>
public static class FormRowOrderedIdsCache
{
    private static readonly object Gate = new();
    private static int _reportId;
    private static string? _formNum;
    private static List<int>? _ids;

    public static bool TryGet(int reportId, string formNum, out IReadOnlyList<int> ids)
    {
        lock (Gate)
        {
            if (_ids != null
                && _reportId == reportId
                && string.Equals(_formNum, formNum, StringComparison.Ordinal))
            {
                ids = _ids;
                return true;
            }
        }

        ids = Array.Empty<int>();
        return false;
    }

    public static void Store(int reportId, string formNum, IReadOnlyList<int> ids)
    {
        ArgumentNullException.ThrowIfNull(ids);
        var copy = ids.ToList();
        lock (Gate)
        {
            _reportId = reportId;
            _formNum = formNum;
            _ids = copy;
        }
    }

    public static void Invalidate()
    {
        lock (Gate)
        {
            _reportId = 0;
            _formNum = null;
            _ids = null;
        }
    }
}
