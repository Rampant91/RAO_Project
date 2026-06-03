using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Models.DBRealization;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.ExcelExport.Snk;

/// <summary>
/// Подсчёт суммарного количества источников в наличии (СНК) по форме 1.1 по всей БД.
/// </summary>
public class CountForm11InStockTotalAsyncCommand : ExcelExportSnkBaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => true;

    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, "Запрос даты формирования СНК", "Подсчёт источников в наличии", "СНК_1.1");
        var (endSnkDate, snkParams) = await AskSnkEndDate(progressBar, cts);

        progressBarVM.SetProgressBar(10, "Создание временной БД");
        var tmpDbPath = await CreateTempDataBase(progressBar, cts);
        await using var db = new DBModel(tmpDbPath);

        progressBarVM.SetProgressBar(15, "Подсчёт источников в наличии (форма 1.1)");
        var total = await GetForm11InStockTotalQuantityAsync(db, endSnkDate, snkParams, progressBarVM, cts);

        progressBarVM.SetProgressBar(98, "Очистка временных данных");
        try
        {
            File.Delete(tmpDbPath);
        }
        catch
        {
            // ignored
        }

        progressBarVM.SetProgressBar(100, "Завершение");
        await progressBar.CloseAsync();

        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Подсчёт СНК",
                ContentHeader = "Форма 1.1",
                ContentMessage = $"На дату {endSnkDate:dd.MM.yyyy} в наличии по всей базе: {total} источников.",
                MinWidth = 400,
                MaxWidth = 600,
                MinHeight = 150,
                CanResize = true,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));
    }
}
