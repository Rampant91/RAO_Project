using System;
using System.IO;
using Client_App.Services.Updates;
using Models.DTO;
using Xunit;

namespace Test.Updates;

public class NetworkUpdateCheckerTests : IDisposable
{
    private readonly string _root;
    private readonly NetworkUpdateChecker _checker = new();

    public NetworkUpdateCheckerTests()
    {
        _root = Path.Combine(Path.GetTempPath(), "mpzf-update-tests-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_root);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_root))
            {
                Directory.Delete(_root, recursive: true);
            }
        }
        catch
        {
            // ignore cleanup failures on locked temp files
        }
    }

    [Fact]
    public void EmptyLocalState_NeedsUpdate()
    {
        var remote = new NetworkReleaseInfo { MajorVersion = "1.3.0", ReleaseId = "12" };
        var local = new LocalUpdateState();

        Assert.True(_checker.IsUpdateAvailable(remote, local, _root));
    }

    [Fact]
    public void DifferentReleaseId_NeedsUpdate()
    {
        var remote = new NetworkReleaseInfo { MajorVersion = "1.3.0", ReleaseId = "12" };
        var local = new LocalUpdateState
        {
            InstalledMajorVersion = "1.3.0",
            InstalledReleaseId = "11"
        };

        Assert.True(_checker.IsUpdateAvailable(remote, local, _root));
    }

    [Fact]
    public void SameIds_AndMatchingFiles_NoUpdate()
    {
        var remote = new NetworkReleaseInfo { MajorVersion = "1.3.0", ReleaseId = "12" };
        var localAppDir = Path.Combine(_root, "local");
        var releaseDir = CreateReleaseFolder(remote);
        var stamp = DateTime.UtcNow;
        WriteBinary(Path.Combine(releaseDir, "Client_App.dll"), length: 100, stamp);
        WriteBinary(Path.Combine(localAppDir, "Client_App.dll"), length: 100, stamp);

        var local = new LocalUpdateState
        {
            InstalledMajorVersion = "1.3.0",
            InstalledReleaseId = "12"
        };

        Assert.False(_checker.IsUpdateAvailable(remote, local, _root, localAppDir));
    }

    [Fact]
    public void SameIds_ButFilesDiffer_NeedsRepair()
    {
        var remote = new NetworkReleaseInfo { MajorVersion = "1.3.0", ReleaseId = "12" };
        var localAppDir = Path.Combine(_root, "local");
        var releaseDir = CreateReleaseFolder(remote);
        WriteBinary(Path.Combine(releaseDir, "Client_App.dll"), length: 200, DateTime.UtcNow);
        WriteBinary(Path.Combine(localAppDir, "Client_App.dll"), length: 100, DateTime.UtcNow.AddHours(-1));

        var local = new LocalUpdateState
        {
            InstalledMajorVersion = "1.3.0",
            InstalledReleaseId = "12"
        };

        Assert.True(_checker.IsUpdateAvailable(remote, local, _root, localAppDir));
    }

    [Fact]
    public void LegacyPlaceholder_NeedsUpdate()
    {
        var remote = new NetworkReleaseInfo { MajorVersion = "1.3.0", ReleaseId = "12" };
        var local = new LocalUpdateState
        {
            InstalledReleaseId = NetworkUpdateLabels.LegacyPreUpdateReleaseId
        };

        Assert.True(_checker.IsUpdateAvailable(remote, local, _root));
    }

    private string CreateReleaseFolder(NetworkReleaseInfo remote)
    {
        var dir = Path.Combine(_root, remote.MajorVersion, remote.ReleaseId, NetworkUpdatePaths.WinPublishFolderName);
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static void WriteBinary(string path, int length, DateTime lastWriteUtc)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllBytes(path, new byte[length]);
        File.SetLastWriteTimeUtc(path, lastWriteUtc);
    }
}
