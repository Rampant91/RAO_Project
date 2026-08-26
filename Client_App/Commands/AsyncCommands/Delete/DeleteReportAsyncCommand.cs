using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Logging;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Устаревшая команда удаления отчёта (не привязана к UI).
/// Оставлена Id-based, чтобы случайный вызов не ломался на stubs.
/// </summary>
public class DeleteReportAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = Desktop.MainWindow as MainWindow;

        #region MessageDeleteReport

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить отчет?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да")
            return;

        Report? reportShell = null;
        if (parameter is IEnumerable enumerable)
            reportShell = enumerable.OfType<Report>().FirstOrDefault();
        else if (parameter is Report report)
            reportShell = report;

        if (reportShell is null)
        {
            ServiceExtension.LoggerManager.Warning(
                "Удаление отчёта (legacy): отчёт не передан.",
                ErrorCodeLogger.Application);
            return;
        }

        try
        {
            var db = StaticConfiguration.DBModel;
            var tracked = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == reportShell.Id)
                          ?? await db.ReportCollectionDbSet
                              .Include(r => r.Reports)
                              .FirstOrDefaultAsync(r => r.Id == reportShell.Id);

            if (tracked is null)
            {
                ServiceExtension.LoggerManager.Warning(
                    $"Удаление отчёта (legacy) Id={reportShell.Id}: не найден в БД.",
                    ErrorCodeLogger.DataBase);
                return;
            }

            var orgId = tracked.Reports?.Id
                        ?? (mainWindow?.DataContext as MainWindowVM)?.SelectedReports?.Id
                        ?? 0;

            if (tracked.Reports is null && (mainWindow?.DataContext as MainWindowVM)?.SelectedReports is { } org)
                tracked.Reports = org;

            await ReportDeletionLogger.LogDeletionAsync(tracked);

            db.ReportCollectionDbSet.Remove(tracked);
            await db.SaveChangesAsync();

            if (orgId != 0)
                Forms1WarmCache.Instance.InvalidateOrg(orgId);

            if (mainWindow?.DataContext is MainWindowVM mainWindowVM)
            {
                mainWindowVM.UpdateReportCollection();
                mainWindowVM.UpdateFormsPageInfo();
                mainWindowVM.UpdateTotalReportCount();
            }
        }
        catch (Exception ex)
        {
            var msg = $"Удаление отчёта (legacy) Id={reportShell.Id}." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }
}
