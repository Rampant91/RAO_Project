using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Models.Collections;
using Xunit;

namespace Test.ReportExport;

/// <summary>
/// Каждый mapped *_DB у Report либо в хэше шапки, либо явно в exclude.
/// </summary>
public class ReportHeaderFingerprintExcludeGuardTests
{
    [Fact]
    public void EveryMappedReportDbProperty_IsInHeaderOrExclude()
    {
        var headerNames = ReportContentFingerprint.GetHeaderFingerprintProperties()
            .Select(p => p.Name)
            .ToHashSet(StringComparer.Ordinal);

        var mappedDbProps = typeof(Report)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead
                        && p.GetIndexParameters().Length == 0
                        && p.GetCustomAttribute<NotMappedAttribute>() is null
                        && p.Name.EndsWith("_DB", StringComparison.Ordinal))
            .Select(p => p.Name)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToList();

        var orphans = mappedDbProps
            .Where(name => !headerNames.Contains(name)
                           && !ReportContentFingerprint.HeaderExcludePropertyNames.Contains(name))
            .ToList();

        Assert.True(
            orphans.Count == 0,
            "Mapped *_DB на Report должны быть в Compute-шапке или в HeaderExcludePropertyNames:\n" +
            string.Join("\n", orphans));
    }
}
