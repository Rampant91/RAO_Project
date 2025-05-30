﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Базовый класс экспорта в файл .RAODB
/// </summary>
public abstract class ExportRaodbBaseAsyncCommand : BaseAsyncCommand
{
    private protected AnyTaskProgressBar ProgressBar;

    private CancellationTokenSource Cts = new();

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter), Cts.Token);
        }
        catch (OperationCanceledException)
        {
            //ignore
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        IsExecute = false;
    }

    /// <summary>
    /// Создание временной копии БД в папке temp
    /// </summary>
    /// <returns>Путь к временной БД</returns>
    private protected static string CreateTempDataBase()
    {
        var dbReadOnlyPath = Path.Combine(BaseVM.TmpDirectory, BaseVM.DbFileName + ".RAODB");
        try
        {
            if (!StaticConfiguration.IsFileLocked(dbReadOnlyPath))
            {
                File.Delete(dbReadOnlyPath);
                File.Copy(Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB"), dbReadOnlyPath);
            }
        }
        catch
        {
            return dbReadOnlyPath;
        }
        return dbReadOnlyPath;
    }
}