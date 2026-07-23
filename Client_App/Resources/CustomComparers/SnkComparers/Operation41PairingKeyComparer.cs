using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Вспомогательные методы нормализации полей при проверке парности операций 41.
/// </summary>
public static partial class Operation41PairingKeyComparer
{
    /// <summary>
    /// Маркеры «номера нет» после очистки спецсимволов (без lookalike — как CheckForEmptyString в СНК).
    /// </summary>
    private static readonly HashSet<string> EmptySerialMarkers = new(StringComparer.Ordinal)
    {
        "бн",
        "безномера",
        "нет",
        "отсутствует",
        "прим",
        "примечание"
    };

    /// <summary>
    /// Оба номера (паспорт и заводской) отсутствуют или являются служебными заглушками.
    /// </summary>
    public static bool SerialNumbersIsEmpty(string? pasNum, string? facNum) =>
        IsEmptySerial(pasNum) && IsEmptySerial(facNum);

    /// <summary>
    /// Пустая строка, «-», «б.н.», «без номера», «прим.» и т.п.
    /// </summary>
    public static bool IsEmptySerial(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
        {
            return true;
        }

        var normalized = value.ToLowerInvariant();
        normalized = DashesRegex().Replace(normalized, string.Empty);
        normalized = SerialCleanupRegex().Replace(normalized, string.Empty);
        return normalized.Length == 0 || EmptySerialMarkers.Contains(normalized);
    }

    public static string NormalizeDocumentVid(byte? documentVid) =>
        documentVid?.ToString() ?? string.Empty;

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex DashesRegex();

    [GeneratedRegex(@"[\\/:*?""<>|.,_\-;:\s+]")]
    private static partial Regex SerialCleanupRegex();
}
