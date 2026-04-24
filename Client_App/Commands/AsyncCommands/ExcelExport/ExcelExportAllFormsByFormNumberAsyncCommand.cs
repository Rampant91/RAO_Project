using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.OpenGL.Surfaces;
using Client_App.Interfaces.Logger;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выгрузка всех отчётов указанной формы (1.1-1.9, 2.1-2.12) выбранной организации в отдельные .xlsx файлы.
/// </summary>
public class ExcelExportAllFormsByFormNumberAsyncCommand(MainWindowVM mainWindowVM) : BaseAsyncCommand
{
    public override bool CanExecute(object? parameter) => 
        parameter is string formNum && 
        formNum is "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6" or "1.7" or "1.8" or "1.9" or
                   "2.1" or "2.2" or "2.3" or "2.4" or "2.5" or "2.6" or "2.7" or "2.8" or "2.9" or "2.10" or "2.11" or "2.12";

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not string formNum) return;

        // Получаем выбранную организацию
        var selectedReports = mainWindowVM.SelectedReports;
        if (selectedReports == null)
        {
            await ShowNoSelectedOrgMessage();
            return;
        }

        var cts = new CancellationTokenSource();
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        try
        {
            progressBarVM.SetProgressBar(5, "Запрос папки для сохранения");
            
            // Запрашиваем папку для сохранения один раз
            var destinationFolder = await RequestDestinationFolder();
            if (string.IsNullOrEmpty(destinationFolder))
            {
                await CancelCommandAndCloseProgressBar(cts, progressBar);
                return;
            }

            progressBarVM.SetProgressBar(10, "Получение списка отчётов");
            
            // Получаем отчёты выбранной организации для указанной формы
            var reports = await GetReportsByFormNumber(formNum, selectedReports.Id, cts);
            if (reports.Count == 0)
            {
                await ShowNoReportsMessage(formNum);
                await CancelCommandAndCloseProgressBar(cts, progressBar);
                return;
            }

            var totalReports = reports.Count;
            var exportedCount = 0;
            var printCommand = new ExcelExportFormPrintAsyncCommand();

            for (var i = 0; i < reports.Count; i++)
            {
                var report = reports[i];
                var progressPercent = 10 + (i * 80 / totalReports); // 10% - 90% прогресс
                
                progressBarVM.SetProgressBar(
                    progressPercent, 
                    $"Выгрузка отчёта {i + 1} из {totalReports}");

                try
                {
                    // Вызываем команду выгрузки для конкретного отчёта с подавлением диалогов
                    await printCommand.AsyncExecute(report, destinationFolder, suppressDialogs: true);
                    exportedCount++;
                }
                catch (Exception ex)
                {
                    // Логируем ошибку но продолжаем с другими отчётами
                    ServiceExtension.LoggerManager.Warning(
                        $"Ошибка выгрузки отчёта Id={report.Id}: {ex.Message}");
                }
            }

            progressBarVM.SetProgressBar(95, "Завершение выгрузки");
            await progressBar.CloseAsync();

            // Показываем финальный диалог с предложением открыть папку
            await ShowCompletionDialog(destinationFolder, exportedCount, totalReports);
        }
        catch (OperationCanceledException)
        {
            // Пользователь отменил операцию
        }
        catch (Exception ex)
        {
            ServiceExtension.LoggerManager.Error($"Ошибка пакетной выгрузки: {ex}");
            throw;
        }
        finally
        {
            await progressBar.CloseAsync();
        }
    }

    /// <summary>
    /// Запрашивает у пользователя папку для сохранения файлов.
    /// </summary>
    private static async Task<string?> RequestDestinationFolder()
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Выберите папку для сохранения выгрузок"
        };

        var result = await dialog.ShowAsync(Desktop.MainWindow);
        return result;
    }

    /// <summary>
    /// Получает отчёты указанной организации для указанной формы.
    /// </summary>
    private static async Task<List<Report>> GetReportsByFormNumber(string formNum, int reportsId, CancellationTokenSource cts)
    {
        var dbPath = Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB");
        await using var db = new DBModel(dbPath);

        var query = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable);

        var reps = formNum switch
        {
            "1.1" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows11)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.2" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows12)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.3" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows13)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.4" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows14)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.5" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows15)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.6" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows16)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.7" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows17)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.8" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows18)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "1.9" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows19)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.1" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows21)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.2" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows22)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.3" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows23)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.4" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows24)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.5" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows25)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.6" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows26)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.7" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows27)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.8" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows28)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.9" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows29)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.10" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows210)
                .FirstAsync(x => x.Id == reportsId),

            "2.11" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows211)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            "2.12" => await query
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection.Where(x => x.FormNum_DB == formNum)).ThenInclude(x => x.Rows212)
                .FirstAsync(x => x.Id == reportsId, cts.Token),

            _ => throw new ArgumentOutOfRangeException(nameof(formNum), formNum, null)
        };

        return reps.Report_Collection.ToList<Report>();
    }

    /// <summary>
    /// Показывает сообщение о не выбранной организации.
    /// </summary>
    private static async Task ShowNoSelectedOrgMessage()
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
            MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Информация",
                    ContentMessage = "Не выбрана организация для выгрузки.",
                    MinWidth = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));
    }

    /// <summary>
    /// Показывает сообщение об отсутствии отчётов.
    /// </summary>
    private static async Task ShowNoReportsMessage(string formNum)
    {
        await Dispatcher.UIThread.InvokeAsync(() =>
            MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Информация",
                    ContentMessage = $"Для формы {formNum} не найдено отчётов у выбранной организации.",
                    MinWidth = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));
    }

    /// <summary>
    /// Показывает финальный диалог с предложением открыть папку.
    /// </summary>
    private static async Task ShowCompletionDialog(string folderPath, int exported, int total)
    {
        var message = exported == total
            ? $"Успешно выгружено {exported} отчётов в папку:{Environment.NewLine}{folderPath}"
            : $"Выгружено {exported} из {total} отчётов.{Environment.NewLine}Некоторые отчёты не удалось выгрузить.{Environment.NewLine}{Environment.NewLine}Папка:{Environment.NewLine}{folderPath}";

        var answer = await Dispatcher.UIThread.InvokeAsync(async () =>
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок" },
                        new ButtonDefinition { Name = "Открыть папку" }
                    ],
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Готово",
                    ContentMessage = message,
                    MinWidth = 450,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

        if (answer is "Открыть папку")
        {
            Process.Start(new ProcessStartInfo 
            { 
                FileName = folderPath, 
                UseShellExecute = true 
            });
        }
    }

    /// <summary>
    /// Отмена операции и закрытие прогрессбара.
    /// </summary>
    private static async Task CancelCommandAndCloseProgressBar(CancellationTokenSource cts, AnyTaskProgressBar progressBar)
    {
        await cts.CancelAsync();
        await progressBar.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }
}
