using System;
using System.IO;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Xunit;

namespace Test.ExcelExport;

/// <summary>
/// Индексы имён при совпадении: name.xlsx → name_1.xlsx → name_2.xlsx.
/// </summary>
public sealed class ResolveUniqueFilePathTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "rao_unique_xlsx_" + Guid.NewGuid().ToString("N"));

    public ResolveUniqueFilePathTests()
    {
        Directory.CreateDirectory(_dir);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_dir))
            {
                Directory.Delete(_dir, recursive: true);
            }
        }
        catch
        {
            // temp cleanup best-effort
        }
    }

    [Fact]
    public void ResolveUniqueFilePath_WhenFree_ReturnsSamePath()
    {
        var path = Path.Combine(_dir, "passport.xlsx");

        var resolved = ExcelBaseAsyncCommand.ResolveUniqueFilePath(path, _dir);

        Assert.Equal(path, resolved);
    }

    [Fact]
    public void ResolveUniqueFilePath_WhenExists_AddsIndex1()
    {
        var path = Path.Combine(_dir, "passport.xlsx");
        File.WriteAllText(path, "occupied");

        var resolved = ExcelBaseAsyncCommand.ResolveUniqueFilePath(path, _dir);

        Assert.Equal(Path.Combine(_dir, "passport_1.xlsx"), resolved);
    }

    [Fact]
    public void ResolveUniqueFilePath_WhenIndex1Taken_UsesIndex2()
    {
        File.WriteAllText(Path.Combine(_dir, "passport.xlsx"), "a");
        File.WriteAllText(Path.Combine(_dir, "passport_1.xlsx"), "b");

        var resolved = ExcelBaseAsyncCommand.ResolveUniqueFilePath(
            Path.Combine(_dir, "passport.xlsx"), _dir);

        Assert.Equal(Path.Combine(_dir, "passport_2.xlsx"), resolved);
    }
}
