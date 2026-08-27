using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Logging;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранный отчёт у выбранной организации (контекстное меню таблицы отчётов).
/// </summary>
public class NewDeleteFormAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewDeleteFormAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReport))
                OnCanExecuteChanged();
        };
    }

    public override bool CanExecute(object? parameter) =>
        !IsExecute && (parameter is Report || _formsTabControlVM.SelectedReport is not null);

    public override async Task AsyncExecute(object? parameter)
    {
        var reportShell = parameter as Report ?? _formsTabControlVM.SelectedReport;
        if (reportShell is null)
        {
            ServiceExtension.LoggerManager.Warning(
                "Удаление отчёта: команда вызвана без выбранного отчёта.",
                ErrorCodeLogger.Application);
            return;
        }

        if (await ReportExportLock.TryBlockReportAccessAsync(reportShell.Id))
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление отчёта Id={reportShell.Id}: доступ заблокирован (идёт экспорт или блокировка).",
                ErrorCodeLogger.Application);
            return;
        }

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

        try
        {
            await DeleteReportFromDbAndRefreshUiAsync(reportShell).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            var msg = $"Удаление отчёта Id={reportShell.Id} не выполнено." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }

    private async Task DeleteReportFromDbAndRefreshUiAsync(Report reportShell)
    {
        var db = StaticConfiguration.DBModel;
        var org = _formsTabControlVM.SelectedReports;
        var orgId = org?.Id
                    ?? reportShell.Reports?.Id
                    ?? 0;

        // Список на главном — stubs; Remove(stub) из Report_Collection не находит объект и EF ничего не пишет.
        var tracked = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == reportShell.Id);
        if (tracked is null)
        {
            tracked = await db.ReportCollectionDbSet
                .Include(r => r.Reports!)
                .ThenInclude(reps => reps.Master_DB)
                .FirstOrDefaultAsync(r => r.Id == reportShell.Id)
                .ConfigureAwait(true);
        }

        if (tracked is null)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление отчёта Id={reportShell.Id}: запись не найдена в БД.",
                ErrorCodeLogger.DataBase);
            return;
        }

        if (tracked.Reports is null && org is not null)
            tracked.Reports = org;

        if (orgId == 0)
            orgId = tracked.Reports?.Id ?? 0;

        try
        {
            await ReportDeletionLogger.LogDeletionAsync(tracked).ConfigureAwait(true);
        }
        catch (Exception logEx)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление отчёта Id={tracked.Id}: не удалось записать Deleted_Reports.log. {logEx.Message}",
                ErrorCodeLogger.Application);
        }

        db.ReportCollectionDbSet.Remove(tracked);
        await db.SaveChangesAsync().ConfigureAwait(true);

        RemoveFromInMemoryCollections(org, tracked.Id);

        if (orgId != 0)
            Forms1WarmCache.Instance.InvalidateOrg(orgId);

        _formsTabControlVM.SelectedReport = null;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var mainWindowVM = mainWindow?.DataContext as MainWindowVM;
        if (mainWindowVM is not null)
        {
            mainWindowVM.UpdateReportCollection();
            mainWindowVM.UpdateFormsPageInfo();
            mainWindowVM.UpdateTotalReportCount();
        }
        else
        {
            _formsTabControlVM.UpdateReportCollection();
            _formsTabControlVM.UpdateFormsPageInfo();
            _formsTabControlVM.UpdateTotalReportCount();
        }
    }

    private static void RemoveFromInMemoryCollections(Reports? org, int reportId)
    {
        if (org?.Report_Collection is null)
            return;

        var inOrg = org.Report_Collection
            .OfType<Report>()
            .Where(r => r.Id == reportId)
            .ToList();
        foreach (var r in inOrg)
            org.Report_Collection.Remove(r);
    }
}
