using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Models.Comparers.FormContent;

/// <summary>
/// Стабильный набор полей для SHA-256 fingerprint содержимого.
/// Значения нормализуются так же, как в Form*Equality / IsContentEqual.
/// </summary>
public sealed class ContentFingerprintSink
{
    private readonly List<(string Name, string Value)> _parts = [];

    public void AddText(string name, string? value) =>
        _parts.Add((name, FormTextEquality.Normalize(value)));

    public void AddDate(string name, string? value)
    {
        if (FormDateEquality.TryParse(value, out var date))
        {
            _parts.Add((name, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            return;
        }

        _parts.Add((name, FormTextEquality.Normalize(value)));
    }

    public void AddExponential(string name, string? value) =>
        _parts.Add((name, FormExponentialEquality.NormalizeForFingerprint(value)));

    public void AddRadionuclids(string name, string? value) =>
        _parts.Add((name, FormRadionuclidsEquality.NormalizeForFingerprint(value)));

    public void AddDouble(string name, double value) =>
        _parts.Add((name, FormDoubleEquality.NormalizeForFingerprint(value)));

    public void AddDouble(string name, double? value) =>
        _parts.Add((name, value is null
            ? string.Empty
            : FormDoubleEquality.NormalizeForFingerprint(value.Value)));

    public void AddFloat(string name, float value) =>
        _parts.Add((name, FormDoubleEquality.NormalizeForFingerprint(value)));

    public void AddFloat(string name, float? value) =>
        _parts.Add((name, value is null
            ? string.Empty
            : FormDoubleEquality.NormalizeForFingerprint(value.Value)));

    public void AddRaw(string name, string? value) =>
        _parts.Add((name, value ?? string.Empty));

    public void AddRaw(string name, int value) =>
        _parts.Add((name, value.ToString(CultureInfo.InvariantCulture)));

    public void AddRaw(string name, int? value) =>
        _parts.Add((name, value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));

    public void AddRaw(string name, short value) =>
        _parts.Add((name, value.ToString(CultureInfo.InvariantCulture)));

    public void AddRaw(string name, short? value) =>
        _parts.Add((name, value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));

    public void AddRaw(string name, byte value) =>
        _parts.Add((name, value.ToString(CultureInfo.InvariantCulture)));

    public void AddRaw(string name, byte? value) =>
        _parts.Add((name, value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));

    public void AddRaw(string name, bool value) =>
        _parts.Add((name, value ? "1" : "0"));

    public void AddRaw(string name, long value) =>
        _parts.Add((name, value.ToString(CultureInfo.InvariantCulture)));

    public string ToCanonicalString()
    {
        return string.Join('\n',
            _parts
                .OrderBy(p => p.Name, StringComparer.Ordinal)
                .Select(p => p.Name + "=" + p.Value));
    }

    public static string Sha256Hex(string canonical)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(canonical));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
