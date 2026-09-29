using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную организацию.
/// Org в гриде — AsNoTracking stub: нельзя делать DbSet.Remove(stub), только удаление по Id.
/// </summary>
public class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    private readonly MainWindowVM _mainWindowVM;

    public DeleteReportsAsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;

        mainWindowVM.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
                OnCanExecuteChanged();
        };
    }

    public override bool CanExecute(object? parameter) =>
        !IsExecute && (_mainWindowVM.SelectedReports is not null || parameter is Reports || parameter is IEnumerable);

    public override async Task AsyncExecute(object? parameter)
    {
        Reports? orgShell;
        if (parameter is IEnumerable enumerable)
            orgShell = enumerable.OfType<Reports>().FirstOrDefault();
        else if (parameter is Reports reports)
            orgShell = reports;
        else
            orgShell = _mainWindowVM.SelectedReports;

        if (orgShell is null)
        {
            ServiceExtension.LoggerManager.Warning(
                "Удаление организации: организация не выбрана.",
                ErrorCodeLogger.Application);
            return;
        }

        if (await ReportExportLock.TryBlockOrganizationAccessAsync(orgShell.Id))
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgShell.Id}: доступ заблокирован (экспорт/блокировка).",
                ErrorCodeLogger.Application);
            return;
        }

        #region MessageDeleteReports

        var answer = await MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да", IsDefault = true },
                    new ButtonDefinition { Name = "Нет", IsCancel = true }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowWindowDialogAsync(Desktop.MainWindow);

        #endregion

        if (answer is not "Да")
            return;

        var orgId = orgShell.Id;
        var cts = new CancellationTokenSource();
        AnyTaskProgressBar? progressBar = null;

        try
        {
            progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressVm = progressBar.AnyTaskProgressBarVM;
            progressVm.SetProgressBar(5, "Подготовка к удалению...", "Удаление организации", "Удаление");

            await DeleteOrganizationByIdAsync(orgId, progressVm, cts.Token).ConfigureAwait(false);

            progressVm.SetProgressBar(100, "Готово");
            await progressBar.CloseCompletedAsync().ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgId}: отменено пользователем.",
                ErrorCodeLogger.Application);
            if (progressBar is not null)
                await progressBar.CloseAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var msg = $"Удаление организации Id={orgId} не выполнено." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
            if (progressBar is not null)
                await progressBar.CloseAsync().ConfigureAwait(false);
        }
    }

    private async Task DeleteOrganizationByIdAsync(
        int orgId,
        AnyTaskProgressBarVM progressVm,
        CancellationToken cancellationToken)
    {
        var db = StaticConfiguration.DBModel;

        SetProgress(progressVm, 15, "Поиск организации в БД");
        var trackedOrg = await db.ReportsCollectionDbSet
            .FirstOrDefaultAsync(r => r.Id == orgId, cancellationToken)
            .ConfigureAwait(false);

        if (trackedOrg is null)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgId}: запись не найдена в БД.",
                ErrorCodeLogger.DataBase);
            await Dispatcher.UIThread.InvokeAsync(() => InvalidateAndRefreshUi(orgId));
            return;
        }

        var masterId = trackedOrg.Master_DBId;

        SetProgress(progressVm, 30, "Сбор списка отчётов");
        var reportIds = await OrgReportsQuery.GetReportIdsAsync(db, orgId, cancellationToken)
            .ConfigureAwait(false);

        SetProgress(progressVm, 45, $"Удаление отчётов ({reportIds.Length})");
        foreach (var batch in FirebirdInClause.Chunk(reportIds))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var toRemove = await db.ReportCollectionDbSet
                .Where(r => batch.Contains(r.Id))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            db.ReportCollectionDbSet.RemoveRange(toRemove);
        }

        // Титул (Master) может не входить в Report_Collection org — удаляем отдельно по Id.
        if (masterId is > 0)
        {
            SetProgress(progressVm, 70, "Удаление титульного листа");
            var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == masterId.Value)
                         ?? await db.ReportCollectionDbSet
                             .FirstOrDefaultAsync(r => r.Id == masterId.Value, cancellationToken)
                             .ConfigureAwait(false);
            if (master is not null)
                db.ReportCollectionDbSet.Remove(master);

            trackedOrg.Master_DBId = null;
            trackedOrg.Master_DB = null!;
        }

        SetProgress(progressVm, 85, "Сохранение изменений в БД");
        db.ReportsCollectionDbSet.Remove(trackedOrg);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        SetProgress(progressVm, 95, "Обновление списка");
        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            RemoveOrgFromLocalStorage(orgId);
            InvalidateAndRefreshUi(orgId);
        });
    }

    private static void SetProgress(AnyTaskProgressBarVM progressVm, int percent, string status)
    {
        Dispatcher.UIThread.Post(() => progressVm.SetProgressBar(percent, status));
    }

    private void InvalidateAndRefreshUi(int orgId)
    {
        Forms1WarmCache.Instance.InvalidateOrg(orgId);
        Forms1WarmCache.Instance.InvalidateOrgPages();
        MainWindowListQuery.InvalidateAllOrgKeysCaches();

        if (_mainWindowVM.SelectedReports?.Id == orgId)
            _mainWindowVM.SelectedReports = null;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var mainWindowVM = mainWindow?.DataContext as MainWindowVM ?? _mainWindowVM;
        mainWindowVM.UpdateReportsCollection();
        mainWindowVM.UpdateOrgsPageInfo();
        mainWindowVM.UpdateTotalReportCount();
        mainWindowVM.UpdateTotalReportsCount();
    }

    private static void RemoveOrgFromLocalStorage(int orgId)
    {
        var local = ReportsStorage.LocalReports;
        if (local?.Reports_Collection is null)
            return;

        foreach (var item in local.Reports_Collection.OfType<Reports>().Where(r => r.Id == orgId).ToList())
            local.Reports_Collection.Remove(item);
    }
}
