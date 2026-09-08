using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Test.ReportExport;

/// <summary>
/// Наборы *_DB в IsContentEqual и ContributeToContentFingerprint должны совпадать.
/// </summary>
public partial class FormContentFingerprintContributeGuardTests
{
    [Fact]
    public void IsContentEqual_And_Contribute_UseSameDbFields()
    {
        var formsRoot = FindFormsRoot();
        Assert.True(Directory.Exists(formsRoot), $"Forms root not found: {formsRoot}");

        var mismatches = new List<string>();
        foreach (var path in Directory.EnumerateFiles(formsRoot, "Form*.cs", SearchOption.AllDirectories))
        {
            var fileName = Path.GetFileName(path);
            if (fileName is "Form.cs" or "Form1.cs" or "Form2.cs" or "FormCreator.cs" or "FormStaticData.cs")
            {
                continue;
            }

            if (path.Contains($"{Path.DirectorySeparatorChar}Form3{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                continue;
            }

            var text = File.ReadAllText(path);
            if (!text.Contains("override bool IsContentEqual", StringComparison.Ordinal))
            {
                continue;
            }

            var equalFields = ExtractDbFields(ExtractMethodBody(text, "IsContentEqual"));
            var contributeFields = ExtractDbFields(ExtractMethodBody(text, "ContributeToContentFingerprint"));

            if (equalFields.Count == 0)
            {
                mismatches.Add($"{fileName}: IsContentEqual has no *_DB fields");
                continue;
            }

            if (!equalFields.SetEquals(contributeFields))
            {
                var onlyEqual = equalFields.Except(contributeFields).OrderBy(x => x).ToList();
                var onlyContribute = contributeFields.Except(equalFields).OrderBy(x => x).ToList();
                mismatches.Add(
                    $"{fileName}: only in IsContentEqual=[{string.Join(", ", onlyEqual)}]; " +
                    $"only in Contribute=[{string.Join(", ", onlyContribute)}]");
            }
        }

        Assert.True(mismatches.Count == 0, string.Join(Environment.NewLine, mismatches));
    }

    private static HashSet<string> ExtractDbFields(string methodBody) =>
        DbFieldRegex().Matches(methodBody)
            .Select(m => m.Groups[1].Value)
            .ToHashSet(StringComparer.Ordinal);

    private static string ExtractMethodBody(string source, string methodName)
    {
        var marker = methodName == "IsContentEqual"
            ? "bool IsContentEqual"
            : "void ContributeToContentFingerprint";
        var start = source.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0)
        {
            return string.Empty;
        }

        var brace = source.IndexOf('{', start);
        if (brace < 0)
        {
            return string.Empty;
        }

        var depth = 0;
        for (var i = brace; i < source.Length; i++)
        {
            if (source[i] == '{')
            {
                depth++;
            }
            else if (source[i] == '}')
            {
                depth--;
                if (depth == 0)
                {
                    return source.Substring(brace, i - brace + 1);
                }
            }
        }

        return string.Empty;
    }

    private static string FindFormsRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "Models", "Forms");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Models", "Forms"));
    }

    [GeneratedRegex(@"\b([A-Za-z_][A-Za-z0-9_]*_DB)\b")]
    private static partial Regex DbFieldRegex();
}
