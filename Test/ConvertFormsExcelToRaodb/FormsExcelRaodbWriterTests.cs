using System;
using System.IO;
using Client_App.Commands.AsyncCommands.ConvertFormsExcelToRaodb;
using Xunit;

namespace Test.ConvertFormsExcelToRaodb;

public sealed class FormsExcelRaodbWriterTests
{
    [Fact]
    public void InsertIndexInFilePath_AppendsIndex_When_FileExists()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            var path = Path.Combine(dir, "report.RAODB");
            File.WriteAllText(path, "x");

            var result = FormsExcelRaodbWriter.InsertIndexInFilePath(path);

            Assert.Equal(Path.Combine(dir, "report#1.RAODB"), result);
            Assert.False(File.Exists(result));
        }
        finally
        {
            Directory.Delete(dir, recursive: true);
        }
    }

    [Fact]
    public void InsertIndexInFilePath_Returns_Original_When_Free()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.RAODB");
        Assert.Equal(path, FormsExcelRaodbWriter.InsertIndexInFilePath(path));
    }
}
