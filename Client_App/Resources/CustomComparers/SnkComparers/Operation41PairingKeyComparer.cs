using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сокращённое сравнение операции 41 для парности форм 1.1 и 1.5:
/// паспорт, заводской номер, тип, радионуклиды и дата операции (строка).
/// </summary>
public sealed class Operation41PairingKeyComparer
    : IEqualityComparer<(string PasNum, string FacNum, string Radionuclids, string Type, string OpDate)>
{
    private readonly SnkGroupKeyComparer _unitComparer = new();

    public bool Equals(
        (string PasNum, string FacNum, string Radionuclids, string Type, string OpDate) x,
        (string PasNum, string FacNum, string Radionuclids, string Type, string OpDate) y)
    {
        return _unitComparer.Equals(
                   (x.PasNum, x.FacNum, x.Radionuclids, x.Type),
                   (y.PasNum, y.FacNum, y.Radionuclids, y.Type))
               && OperationDatesEqual(x.OpDate, y.OpDate);
    }

    public int GetHashCode((string PasNum, string FacNum, string Radionuclids, string Type, string OpDate) obj)
    {
        return HashCode.Combine(
            _unitComparer.GetHashCode((obj.PasNum, obj.FacNum, obj.Radionuclids, obj.Type)),
            NormalizeOperationDate(obj.OpDate));
    }

    /// <summary>
    /// Сравнение дат операции, хранящихся в БД как строки.
    /// </summary>
    public static bool OperationDatesEqual(string? left, string? right)
    {
        var normalizedLeft = NormalizeOperationDate(left);
        var normalizedRight = NormalizeOperationDate(right);

        if (string.Equals(normalizedLeft, normalizedRight, StringComparison.Ordinal))
        {
            return true;
        }

        if (DateOnly.TryParse(left, out var leftDate) && DateOnly.TryParse(right, out var rightDate))
        {
            return leftDate == rightDate;
        }

        return false;
    }

    private static string NormalizeOperationDate(string? operationDate)
    {
        if (string.IsNullOrWhiteSpace(operationDate) || operationDate == "-")
        {
            return string.Empty;
        }

        var trimmed = operationDate.Trim();
        return DateOnly.TryParse(trimmed, out var dateOnly)
            ? dateOnly.ToString("yyyy-MM-dd")
            : trimmed;
    }
}
