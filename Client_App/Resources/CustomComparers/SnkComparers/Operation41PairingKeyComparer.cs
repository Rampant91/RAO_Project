using System;
using System.Collections.Generic;

namespace Client_App.Resources.CustomComparers.SnkComparers;

/// <summary>
/// Сравнение ключевых полей операции 41 для парности при переводе РВ → РАО.
/// </summary>
public sealed class Operation41PairingKeyComparer : IEqualityComparer<Operation41PairingKey>
{
    private readonly SnkNumberEqualityComparer _numberComparer = new();
    private readonly SnkRadionuclidsEqualityComparer _radsComparer = new();
    private readonly Operation41PairingProfile _profile;

    public Operation41PairingKeyComparer(Operation41PairingProfile profile) => _profile = profile;

    public bool Equals(Operation41PairingKey x, Operation41PairingKey y) =>
        _profile switch
        {
            Operation41PairingProfile.Form11To15 => EqualsForm11To15(x, y),
            Operation41PairingProfile.Form12To16 => EqualsForm12To16(x, y),
            Operation41PairingProfile.Form13To16 => EqualsForm13To16(x, y),
            Operation41PairingProfile.Form14To16 => EqualsForm14To16(x, y),
            _ => false
        };

    public int GetHashCode(Operation41PairingKey obj) =>
        _profile switch
        {
            Operation41PairingProfile.Form11To15 => GetHashCodeForm11To15(obj),
            Operation41PairingProfile.Form12To16 => GetHashCodeForm12To16(obj),
            Operation41PairingProfile.Form13To16 => GetHashCodeForm13To16(obj),
            Operation41PairingProfile.Form14To16 => GetHashCodeForm14To16(obj),
            _ => 0
        };

    /// <summary>
    /// Ключ без паспорта, заводского номера и количества — для группировки при пустых серийных номерах (1.1 / 1.5).
    /// </summary>
    public bool AggregateGroupEquals(Operation41PairingKey x, Operation41PairingKey y) =>
        _profile == Operation41PairingProfile.Form11To15
        && _numberComparer.Equals(x.OpCode, y.OpCode)
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

    private bool EqualsForm11To15(Operation41PairingKey x, Operation41PairingKey y) =>
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

    private int GetHashCodeForm11To15(Operation41PairingKey obj)
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
        return HashCode.Combine(
            hash,
            _numberComparer.GetHashCode(obj.DocumentNumber),
            NormalizeOperationDate(obj.DocumentDate),
            _numberComparer.GetHashCode(obj.PackNumber),
            GetQuantity(obj));
    }

    private bool EqualsForm12To16(Operation41PairingKey x, Operation41PairingKey y) =>
        _numberComparer.Equals(x.OpCode, y.OpCode)
        && OperationDatesEqual(x.OpDate, y.OpDate)
        && _radsComparer.Equals(x.MainRadionuclids, y.MainRadionuclids)
        && OperationDatesEqual(x.ActivityMeasurementDate, y.ActivityMeasurementDate)
        && _numberComparer.Equals(x.DocumentVid, y.DocumentVid)
        && _numberComparer.Equals(x.DocumentNumber, y.DocumentNumber)
        && OperationDatesEqual(x.DocumentDate, y.DocumentDate)
        && _numberComparer.Equals(x.PackName, y.PackName)
        && _numberComparer.Equals(x.PackType, y.PackType)
        && _numberComparer.Equals(x.PackNumber, y.PackNumber);

    private int GetHashCodeForm12To16(Operation41PairingKey obj)
    {
        var hash = HashCode.Combine(
            _numberComparer.GetHashCode(obj.OpCode),
            NormalizeOperationDate(obj.OpDate),
            _radsComparer.GetHashCode(obj.MainRadionuclids),
            NormalizeOperationDate(obj.ActivityMeasurementDate),
            _numberComparer.GetHashCode(obj.DocumentVid),
            _numberComparer.GetHashCode(obj.DocumentNumber),
            NormalizeOperationDate(obj.DocumentDate));
        return HashCode.Combine(
            hash,
            _numberComparer.GetHashCode(obj.PackName),
            _numberComparer.GetHashCode(obj.PackType),
            _numberComparer.GetHashCode(obj.PackNumber));
    }

    private bool EqualsForm13To16(Operation41PairingKey x, Operation41PairingKey y) =>
        _numberComparer.Equals(x.OpCode, y.OpCode)
        && OperationDatesEqual(x.OpDate, y.OpDate)
        && _numberComparer.Equals(x.Type, y.Type)
        && _radsComparer.Equals(x.MainRadionuclids, y.MainRadionuclids)
        && OperationDatesEqual(x.ActivityMeasurementDate, y.ActivityMeasurementDate)
        && _numberComparer.Equals(x.DocumentVid, y.DocumentVid)
        && _numberComparer.Equals(x.DocumentNumber, y.DocumentNumber)
        && OperationDatesEqual(x.DocumentDate, y.DocumentDate)
        && _numberComparer.Equals(x.PackName, y.PackName)
        && _numberComparer.Equals(x.PackType, y.PackType)
        && _numberComparer.Equals(x.PackNumber, y.PackNumber);

    private int GetHashCodeForm13To16(Operation41PairingKey obj)
    {
        var hash = HashCode.Combine(
            _numberComparer.GetHashCode(obj.OpCode),
            NormalizeOperationDate(obj.OpDate),
            _numberComparer.GetHashCode(obj.Type),
            _radsComparer.GetHashCode(obj.MainRadionuclids),
            NormalizeOperationDate(obj.ActivityMeasurementDate),
            _numberComparer.GetHashCode(obj.DocumentVid),
            _numberComparer.GetHashCode(obj.DocumentNumber),
            NormalizeOperationDate(obj.DocumentDate));
        return HashCode.Combine(
            hash,
            _numberComparer.GetHashCode(obj.PackName),
            _numberComparer.GetHashCode(obj.PackType),
            _numberComparer.GetHashCode(obj.PackNumber));
    }

    private bool EqualsForm14To16(Operation41PairingKey x, Operation41PairingKey y) =>
        _numberComparer.Equals(x.OpCode, y.OpCode)
        && OperationDatesEqual(x.OpDate, y.OpDate)
        && _radsComparer.Equals(x.MainRadionuclids, y.MainRadionuclids)
        && OperationDatesEqual(x.ActivityMeasurementDate, y.ActivityMeasurementDate)
        && _numberComparer.Equals(x.DocumentVid, y.DocumentVid)
        && _numberComparer.Equals(x.DocumentNumber, y.DocumentNumber)
        && OperationDatesEqual(x.DocumentDate, y.DocumentDate)
        && _numberComparer.Equals(x.PackName, y.PackName)
        && _numberComparer.Equals(x.PackType, y.PackType)
        && _numberComparer.Equals(x.PackNumber, y.PackNumber);

    private int GetHashCodeForm14To16(Operation41PairingKey obj)
    {
        var hash = HashCode.Combine(
            _numberComparer.GetHashCode(obj.OpCode),
            NormalizeOperationDate(obj.OpDate),
            _radsComparer.GetHashCode(obj.MainRadionuclids),
            NormalizeOperationDate(obj.ActivityMeasurementDate),
            _numberComparer.GetHashCode(obj.DocumentVid),
            _numberComparer.GetHashCode(obj.DocumentNumber));
        return HashCode.Combine(
            hash,
            NormalizeOperationDate(obj.DocumentDate),
            _numberComparer.GetHashCode(obj.PackName),
            _numberComparer.GetHashCode(obj.PackType),
            _numberComparer.GetHashCode(obj.PackNumber));
    }

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
    string PackName,
    string PackType,
    string MainRadionuclids,
    string Mass,
    string Volume,
    string ActivityMeasurementDate,
    int? Quantity);
