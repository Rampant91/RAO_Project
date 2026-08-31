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
using Client_App.ViewModels.MainWindowTabs;
using Client_App.ViewModels.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.ExcelExport;

/// <summary>
/// Выгрузка всех отчётов указанной формы (1.1-1.9, 2.1-2.12) или всех форм группы (all-1, all-2)
/// выбранной организации в отдельные .xlsx файлы.
/// </summary>
public class ExcelExportAllFormsByFormNumberAsyncCommand : BaseAsyncCommand
{
    private static readonly string[] Forms1Numbers = ["1.1", "1.2", "1.3", "1.4", "1.5", "1.6", "1.7", "1.8", "1.9"];
    private static readonly string[] Forms2Numbers = ["2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"];

    public sealed class ExportByFormCommandParameter
    {
        public ExportByFormCommandParameter(string formNum, Reports selectedReports)
        {
            FormNum = formNum;
            SelectedReports = selectedReports;
        }

        public string FormNum { get; }
        public Reports SelectedReports { get; }
    }

    private readonly MainWindowVM _mainWindowVM;
    
    public ExcelExportAllFormsByFormNumberAsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;
        
        // Подписываемся на изменение SelectedReports для обновления CanExecute
        mainWindowVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }
    
    public override bool CanExecute(object? parameter) => 
        parameter switch
        {
            ExportByFormCommandParameter p => p.SelectedReports is not null,
            string => _mainWindowVM.SelectedReports is not null,
            _ => false
        };

    public override async Task AsyncExecute(object? parameter)
    {
        string formNum;
        Reports? selectedReports;

        switch (parameter)
        {
            case ExportByFormCommandParameter p:
                formNum = p.FormNum;
                selectedReports = p.SelectedReports;
                break;
            case string s:
                formNum = s;
                selectedReports = _mainWindowVM.SelectedReports;
                break;
            default:
                return;
        }

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

            var formNumbers = ResolveFormNumbers(formNum);
            var workItems = await LoadReportsForExport(
                formNumbers,
                selectedReports.Id,
                progressBarVM,
                cts);

            if (workItems.Count == 0)
            {
                await ShowNoReportsMessage(formNum);
                await CancelCommandAndCloseProgressBar(cts, progressBar);
                return;
            }

            var totalReports = workItems.Count;
            var exportedCount = 0;
            var printCommands = new Dictionary<char, ExcelExportFormPrintAsyncCommand>();

            for (var i = 0; i < workItems.Count; i++)
            {
                var (currentFormNum, report) = workItems[i];
                var progressPercent = 10 + (i * 80 / totalReports);

                progressBarVM.SetProgressBar(
                    progressPercent,
                    formNumbers.Length > 1
                        ? $"Выгрузка отчёта {i + 1} из {totalReports} (форма {currentFormNum})"
                        : $"Выгрузка отчёта {i + 1} из {totalReports}");

                try
                {
                    var formGroup = currentFormNum[0];
                    if (!printCommands.TryGetValue(formGroup, out var printCommand))
                    {
                        FormsTabControlBaseVM formsVM = formGroup switch
                        {
                            '1' => _mainWindowVM.Forms1TabControlVM,
                            '2' => _mainWindowVM.Forms2TabControlVM,
                            _ => throw new ArgumentOutOfRangeException(nameof(currentFormNum), currentFormNum, null)
                        };
                        printCommand = new ExcelExportFormPrintAsyncCommand(formsVM);
                        printCommands[formGroup] = printCommand;
                    }

                    await printCommand.AsyncExecute(report, destinationFolder, suppressDialogs: true);
                    exportedCount++;
                }
                catch (Exception ex)
                {
                    ServiceExtension.LoggerManager.Warning(
                        $"Ошибка выгрузки отчёта Id={report.Id}, форма {currentFormNum}: {ex.Message}");
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

    private static string[] ResolveFormNumbers(string formNum) => formNum switch
    {
        "all-1" => Forms1Numbers,
        "all-2" => Forms2Numbers,
        _ => [formNum]
    };

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

    private readonly record struct ExportReportListInfo(
        int ReportId,
        string FormNum_DB,
        string? StartPeriod_DB,
        string? EndPeriod_DB,
        int? Year_DB,
        byte CorrectionNumber_DB);

    /// <summary>
    /// Загружает список отчётов для выгрузки: облегчённая проекция полей отчётов
    /// и титульные данные организации (как в «Список организаций»).
    /// </summary>
    private static async Task<List<(string FormNum, Report Report)>> LoadReportsForExport(
        string[] formNumbers,
        int reportsId,
        AnyTaskProgressBarVM progressBarVM,
        CancellationTokenSource cts)
    {
        var dbPath = Path.Combine(BaseVM.RaoDirectory, BaseVM.DbFileName + ".RAODB");
        await using var db = new DBModel(dbPath);
        var token = cts.Token;

        progressBarVM.SetProgressBar(11, "Загрузка данных организации");

        var formGroup = formNumbers[0][0];
        var orgQuery = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .Where(reps => reps.Id == reportsId);

        var org = formGroup switch
        {
            '1' => await orgQuery
                .Include(reps => reps.Master_DB).ThenInclude(m => m.Rows10)
                .FirstAsync(token),
            '2' => await orgQuery
                .Include(reps => reps.Master_DB).ThenInclude(m => m.Rows20)
                .FirstAsync(token),
            _ => throw new ArgumentOutOfRangeException(nameof(formNumbers), formNumbers[0], null)
        };

        progressBarVM.SetProgressBar(12, "Загрузка списка отчётов");

        var reportRows = await db.ReportCollectionDbSet
            .AsNoTracking()
            .Where(r => r.Reports != null && r.Reports.Id == reportsId)
            .Where(r => formNumbers.Contains(r.FormNum_DB))
            .Select(r => new ExportReportListInfo(
                r.Id,
                r.FormNum_DB,
                r.StartPeriod_DB,
                r.EndPeriod_DB,
                r.Year_DB,
                r.CorrectionNumber_DB))
            .ToListAsync(token);

        var formOrder = formNumbers
            .Select((formNum, index) => (formNum, index))
            .ToDictionary(x => x.formNum, x => x.index);

        return reportRows
            .OrderBy(r => formOrder.GetValueOrDefault(r.FormNum_DB, int.MaxValue))
            .ThenBy(r => r.ReportId)
            .Select(r => (r.FormNum_DB, ToExportReport(r, org)))
            .ToList();
    }

    private static Report ToExportReport(ExportReportListInfo info, Reports org) => new()
    {
        Id = info.ReportId,
        FormNum_DB = info.FormNum_DB,
        StartPeriod_DB = info.StartPeriod_DB,
        EndPeriod_DB = info.EndPeriod_DB,
        Year_DB = info.Year_DB,
        CorrectionNumber_DB = info.CorrectionNumber_DB,
        Reports = org
    };

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
        var message = formNum switch
        {
            "all-1" => "Для форм 1.1–1.9 не найдено отчётов у выбранной организации.",
            "all-2" => "Для форм 2.1–2.12 не найдено отчётов у выбранной организации.",
            _ => $"Для формы {formNum} не найдено отчётов у выбранной организации."
        };

        await Dispatcher.UIThread.InvokeAsync(() =>
            MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Информация",
                    ContentMessage = message,
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
