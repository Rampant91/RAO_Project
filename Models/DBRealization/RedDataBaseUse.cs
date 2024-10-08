﻿using FirebirdSql.Data.FirebirdClient;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Data.Common;

namespace Models.DBRealization;

public static class RedDataBaseCreation
{
    public static DbConnection GetConnectionString(string _path)
    {
        var direct = Path.GetDirectoryName(_path);
        if (!Directory.Exists(direct))
        {
            Directory.CreateDirectory(direct);
        }
#if DEBUG
        var pth = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                pth = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "REDDB", "win-x64", "fbclient.dll");
            }
            if (RuntimeInformation.OSArchitecture == Architecture.X86)
            {
                pth = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "REDDB", "win-x32", "fbclient.dll");
            }
        }
        else
        {
            if (RuntimeInformation.OSArchitecture == Architecture.X64)
            {
                pth = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "REDDB", "linux-x64", "lib", "libfbclient.so");
            }
            if (RuntimeInformation.OSArchitecture == Architecture.X86)
            {
                pth = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\")), "data", "REDDB", "linux-x32", "lib", "libfbclient.so");
            }
        }
#else
            string pth = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (RuntimeInformation.OSArchitecture == Architecture.X64)
                {
                    pth = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "REDDB", "win-x64", "fbclient.dll");
                }
                if (RuntimeInformation.OSArchitecture == Architecture.X86)
                {
                    pth = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "REDDB", "win-x32", "fbclient.dll");
                }
            }
            else
            {
                if (RuntimeInformation.OSArchitecture == Architecture.X64)
                {
                    pth = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "REDDB", "linux-x64", "lib", "libfbclient.so");
                }
                if (RuntimeInformation.OSArchitecture == Architecture.X86)
                {
                    pth = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "REDDB", "linux-x32", "lib", "libfbclient.so");
                }
            }
#endif
        Console.WriteLine(_path);
        var connectionString = new FbConnectionStringBuilder
        {
            Database = _path,
            ServerType = FbServerType.Embedded,
            UserID = "SYSDBA",
            Password = "masterkey",
            Pooling = false,
            ConnectionLifeTime = 15,
            ClientLibrary = Path.GetFullPath(pth)
        }.ToString();

        return new FbConnection(connectionString);
    }
}