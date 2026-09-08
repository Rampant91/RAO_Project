using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Models.Comparers.FormContent;
using Models.Forms;

namespace Models.Collections;

/// <summary>
/// Отпечаток содержимого отчёта для напоминания о номере корректировки при выгрузке.
/// </summary>
public static class ReportContentFingerprint
{
    public const string AlgorithmVersion = "v1";

    /// <summary>
    /// Mapped-поля Report, не входящие в хэш содержимого.
    /// </summary>
    public static readonly HashSet<string> HeaderExcludePropertyNames = new(StringComparer.Ordinal)
    {
        nameof(Report.Id),
        nameof(Report.CorrectionNumber_DB),
        nameof(Report.ExportDate_DB),
        nameof(Report.LastExportedCorrectionNumber_DB),
        nameof(Report.LastExportedFingerprint_DB),
        nameof(Report.NumberInOrder_DB),
        nameof(Report.ReportChangedDate),
        "ReportsId"
    };

    /// <summary>
    /// Считает SHA-256 hex (64 символа) содержимого отчёта. Строки и Notes должны быть загружены.
    /// </summary>
    public static string Compute(Report report)
    {
        ArgumentNullException.ThrowIfNull(report);

        var sink = new ContentFingerprintSink();
        sink.AddRaw("__algo", AlgorithmVersion);
        ContributeHeader(report, sink);

        foreach (var row in report.Rows.GetEnumerable().OfType<Form>().OrderBy(r => r.NumberInOrder_DB))
        {
            var rowSink = new ContentFingerprintSink();
            row.ContributeToContentFingerprint(rowSink);
            sink.AddRaw($"row:{row.NumberInOrder_DB}", rowSink.ToCanonicalString());
        }

        for (var i = 0; i < report.Notes.Count; i++)
        {
            var noteSink = new ContentFingerprintSink();
            report.Notes[i].ContributeToContentFingerprint(noteSink);
            sink.AddRaw($"note:{i}", noteSink.ToCanonicalString());
        }

        return ContentFingerprintSink.Sha256Hex(sink.ToCanonicalString());
    }

    public static IReadOnlyList<PropertyInfo> GetHeaderFingerprintProperties()
    {
        return typeof(Report)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead
                        && p.GetIndexParameters().Length == 0
                        && p.GetCustomAttribute<NotMappedAttribute>() is null
                        && p.Name.EndsWith("_DB", StringComparison.Ordinal)
                        && !HeaderExcludePropertyNames.Contains(p.Name)
                        && IsScalarFingerprintType(p.PropertyType))
            .OrderBy(p => p.Name, StringComparer.Ordinal)
            .ToList();
    }

    internal static void ContributeHeader(Report report, ContentFingerprintSink sink)
    {
        foreach (var prop in GetHeaderFingerprintProperties())
        {
            var value = prop.GetValue(report);
            switch (value)
            {
                case null:
                    sink.AddRaw(prop.Name, (string?)null);
                    break;
                case string s:
                    sink.AddText(prop.Name, s);
                    break;
                case bool b:
                    sink.AddRaw(prop.Name, b);
                    break;
                case byte by:
                    sink.AddRaw(prop.Name, by);
                    break;
                case short sh:
                    sink.AddRaw(prop.Name, sh);
                    break;
                case int i:
                    sink.AddRaw(prop.Name, i);
                    break;
                case long l:
                    sink.AddRaw(prop.Name, l);
                    break;
                case float f:
                    sink.AddFloat(prop.Name, f);
                    break;
                case double d:
                    sink.AddDouble(prop.Name, d);
                    break;
                default:
                    sink.AddRaw(prop.Name, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
                    break;
            }
        }
    }

    private static bool IsScalarFingerprintType(Type type)
    {
        var t = Nullable.GetUnderlyingType(type) ?? type;
        return t == typeof(string)
               || t == typeof(bool)
               || t == typeof(byte)
               || t == typeof(short)
               || t == typeof(int)
               || t == typeof(long)
               || t == typeof(float)
               || t == typeof(double)
               || t == typeof(DateTime);
    }
}
