using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Test.Architecture;

/// <summary>
/// Исходники с кириллицей должны оставаться валидным UTF-8 без символа замены U+FFFD.
/// </summary>
public class Utf8SourceGuardTests
{
    private static string RepoRoot
    {
        get
        {
            var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "RAO_Project.sln"))
                   && !File.Exists(Path.Combine(dir.FullName, "Client_App", "Client_App.csproj")))
            {
                dir = dir.Parent;
            }

            Assert.NotNull(dir);
            return dir!.FullName;
        }
    }

    public static IEnumerable<object[]> SourceRoots =>
    [
        [Path.Combine("Client_App", "Views", "Forms")],
        [Path.Combine("Client_App", "Views", "Passports")],
        [Path.Combine("Client_App", "Views", "StoragePoints")],
        [Path.Combine("Client_App", "Commands")],
        [Path.Combine("Client_App", "ViewModels", "Forms")],
    ];

    [Theory]
    [MemberData(nameof(SourceRoots))]
    public void Sources_DoNotContainUtf8ReplacementCharacter_InUserVisibleStrings(string relativeRoot)
    {
        var root = Path.Combine(RepoRoot, relativeRoot);
        Assert.True(Directory.Exists(root), $"Missing {root}");

        var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".cs", ".axaml" };
        var uiLineMarkers = new[]
        {
            "ButtonDefinition",
            "ContentTitle",
            "ContentHeader",
            "ContentMessage",
            "case \"",
            "Name = \"",
            "Text=\"",
            "Text=\"{",
        };

        var offenders = new List<string>();

        foreach (var path in Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories))
        {
            var ext = Path.GetExtension(path);
            if (!extensions.Contains(ext))
            {
                continue;
            }

            if (path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                || path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            {
                continue;
            }

            var lines = File.ReadAllLines(path, Encoding.UTF8);
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!line.Contains('\uFFFD', StringComparison.Ordinal))
                {
                    continue;
                }

                if (!uiLineMarkers.Any(marker => line.Contains(marker, StringComparison.Ordinal)))
                {
                    continue;
                }

                offenders.Add($"{Path.GetRelativePath(RepoRoot, path)}:{i + 1}: {line.Trim()}");
            }
        }

        Assert.True(
            offenders.Count == 0,
            "U+FFFD в UI-строках (битая кириллица после неверной правки):" + Environment.NewLine
            + string.Join(Environment.NewLine, offenders.OrderBy(x => x, StringComparer.OrdinalIgnoreCase)));
    }
}
