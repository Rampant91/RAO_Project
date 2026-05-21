using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сравнение ключевых полей операции 41 для парности форм 1.1 и 1.5.
/// </summary>
public sealed class Operation41PairingKeyComparer : IEqualityComparer<Operation41PairingKey>
{
    private readonly SnkNumberEqualityComparer _numberComparer = new();
    private readonly SnkRadionuclidsEqualityComparer _radsComparer = new();

    public bool Equals(Operation41PairingKey x, Operation41PairingKey y) =>
        _numberComparer.Equals(x.OpCode, y.OpCode)
               && OperationDatesEqual(x.OpDate, y.OpDate)
               && _numberComparer.Equals(x.PasNum, y.PasNum)
               && _numberComparer.Equals(x.Type, y.Type)
               && _radsComparer.Equals(x.Radionuclids, y.Radionuclids)
               && _numberComparer.Equals(x.FacNum, y.FacNum)
               && OperationDatesEqual(x.CreationDate, y.CreationDate)
               && _numberComparer.Equals(x.DocumentVid, y.DocumentVid)
               && _numberComparer.Equals(x.DocumentNumber, y.DocumentNumber)
               && OperationDatesEqual(x.DocumentDate, y.DocumentDate)
               && _numberComparer.Equals(x.PackNumber, y.PackNumber)
               && GetQuantity(x) == GetQuantity(y);

    public int GetHashCode(Operation41PairingKey obj)
    {
        var hash = HashCode.Combine(
            _numberComparer.GetHashCode(obj.OpCode),
            NormalizeOperationDate(obj.OpDate),
            _numberComparer.GetHashCode(obj.PasNum),
            _numberComparer.GetHashCode(obj.Type),
            _radsComparer.GetHashCode(obj.Radionuclids),
            _numberComparer.GetHashCode(obj.FacNum),
            NormalizeOperationDate(obj.CreationDate),
            _numberComparer.GetHashCode(obj.DocumentVid));
        hash = HashCode.Combine(
            hash,
            _numberComparer.GetHashCode(obj.DocumentNumber),
            NormalizeOperationDate(obj.DocumentDate),
            _numberComparer.GetHashCode(obj.PackNumber),
            GetQuantity(obj));
        return hash;
    }

    /// <summary>
    /// Ключ без паспорта, заводского номера и количества — для группировки при пустых серийных номерах.
    /// </summary>
    public bool AggregateGroupEquals(Operation41PairingKey x, Operation41PairingKey y) =>
        _numberComparer.Equals(x.OpCode, y.OpCode)
        && OperationDatesEqual(x.OpDate, y.OpDate)
        && _numberComparer.Equals(x.Type, y.Type)
        && _radsComparer.Equals(x.Radionuclids, y.Radionuclids)
        && OperationDatesEqual(x.CreationDate, y.CreationDate)
        && _numberComparer.Equals(x.DocumentVid, y.DocumentVid)
        && _numberComparer.Equals(x.DocumentNumber, y.DocumentNumber)
        && OperationDatesEqual(x.DocumentDate, y.DocumentDate)
        && _numberComparer.Equals(x.PackNumber, y.PackNumber);

    public int GetAggregateGroupHashCode(Operation41PairingKey obj)
    {
        var hash = HashCode.Combine(
            _numberComparer.GetHashCode(obj.OpCode),
            NormalizeOperationDate(obj.OpDate),
            _numberComparer.GetHashCode(obj.Type),
            _radsComparer.GetHashCode(obj.Radionuclids),
            NormalizeOperationDate(obj.CreationDate),
            _numberComparer.GetHashCode(obj.DocumentVid),
            _numberComparer.GetHashCode(obj.DocumentNumber),
            NormalizeOperationDate(obj.DocumentDate));
        return HashCode.Combine(hash, _numberComparer.GetHashCode(obj.PackNumber));
    }

    public static bool SerialNumbersIsEmpty(string? pasNum, string? facNum)
    {
        var numberComparer = new SnkNumberEqualityComparer();
        return numberComparer.Equals(pasNum, string.Empty) && numberComparer.Equals(facNum, string.Empty);
    }

    public static int GetQuantity(Operation41PairingKey key) =>
        key.Quantity is > 0 ? key.Quantity.Value : 1;

    /// <summary>
    /// Сравнение дат, хранящихся в БД как строки.
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

    public static string NormalizeDocumentVid(byte? documentVid) =>
        documentVid?.ToString() ?? string.Empty;

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

/// <summary>
/// Ключевые поля операции 41 для сравнения.
/// </summary>
public readonly record struct Operation41PairingKey(
    string OpCode,
    string OpDate,
    string PasNum,
    string FacNum,
    string Type,
    string Radionuclids,
    string CreationDate,
    string DocumentVid,
    string DocumentNumber,
    string DocumentDate,
    string PackNumber,
    int? Quantity);
