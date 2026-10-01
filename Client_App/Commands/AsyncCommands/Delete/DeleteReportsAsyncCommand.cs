using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.Views;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
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

        var orgId = orgShell.Id;
        var formNum = orgShell.Master_DB?.FormNum_DB
                      ?? orgShell.Master?.FormNum_DB
                      ?? "?";

        if (await ReportExportLock.TryBlockOrganizationAccessAsync(orgId))
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgId} FormNum={formNum}: доступ заблокирован (экспорт/блокировка).",
                ErrorCodeLogger.Application);
            return;
        }

        #region MessageDeleteReports

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(Desktop.MainWindow));

        #endregion

        if (answer is not "Да")
        {
            ServiceExtension.LoggerManager.Info(
                $"Удаление организации Id={orgId} FormNum={formNum}: отменено пользователем.",
                ErrorCodeLogger.Application);
            return;
        }

        ServiceExtension.LoggerManager.Info(
            $"Удаление организации Id={orgId} FormNum={formNum}: подтверждено, старт.",
            ErrorCodeLogger.Application);

        var sw = Stopwatch.StartNew();
        try
        {
            await DeleteOrganizationByIdAsync(orgId, formNum).ConfigureAwait(true);
            ServiceExtension.LoggerManager.Info(
                $"Удаление организации Id={orgId} FormNum={formNum}: успешно за {sw.ElapsedMilliseconds} мс.",
                ErrorCodeLogger.Application);
        }
        catch (Exception ex)
        {
            var msg = $"Удаление организации Id={orgId} FormNum={formNum}: сбой после {sw.ElapsedMilliseconds} мс." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }

    private async Task DeleteOrganizationByIdAsync(int orgId, string formNum)
    {
        LogStep(orgId, formNum, "отмена prefetch / background DB");
        MainWindowPrefetchService.Instance.CancelPending();
        Forms1WarmCache.Instance.CancelAllBackgroundWork();

        var db = StaticConfiguration.DBModel;

        LogStep(orgId, formNum, "загрузка tracked Reports");
        var trackedOrg = await db.ReportsCollectionDbSet
            .FirstOrDefaultAsync(r => r.Id == orgId)
            .ConfigureAwait(true);

        if (trackedOrg is null)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgId} FormNum={formNum}: запись не найдена в БД, только refresh UI.",
                ErrorCodeLogger.DataBase);
            InvalidateAndRefreshUi(orgId);
            return;
        }

        var masterId = trackedOrg.Master_DBId;
        LogStep(orgId, formNum, $"MasterId={masterId?.ToString() ?? "null"}, выборка Id отчётов");
        var reportIds = await OrgReportsQuery.GetReportIdsAsync(db, orgId).ConfigureAwait(true);
        LogStep(orgId, formNum, $"отчётов к удалению: {reportIds.Length}");

        foreach (var batch in FirebirdInClause.Chunk(reportIds))
        {
            var toRemove = await db.ReportCollectionDbSet
                .Where(r => batch.Contains(r.Id))
                .ToListAsync()
                .ConfigureAwait(true);
            db.ReportCollectionDbSet.RemoveRange(toRemove);
        }

        if (masterId is > 0)
        {
            LogStep(orgId, formNum, $"удаление Master Id={masterId.Value}");
            var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == masterId.Value)
                         ?? await db.ReportCollectionDbSet
                             .FirstOrDefaultAsync(r => r.Id == masterId.Value)
                             .ConfigureAwait(true);
            if (master is not null)
                db.ReportCollectionDbSet.Remove(master);

            trackedOrg.Master_DBId = null;
            trackedOrg.Master_DB = null!;
        }

        LogStep(orgId, formNum, "Remove Reports + SaveChanges");
        db.ReportsCollectionDbSet.Remove(trackedOrg);
        await db.SaveChangesAsync().ConfigureAwait(true);
        LogStep(orgId, formNum, "SaveChanges ок");

        LogStep(orgId, formNum, "удаление из LocalReports");
        RemoveOrgFromLocalStorage(orgId);

        LogStep(orgId, formNum, "refresh UI / OrgKeys");
        InvalidateAndRefreshUi(orgId);
    }

    private void InvalidateAndRefreshUi(int orgId)
    {
        Forms1WarmCache.Instance.InvalidateOrg(orgId);

        if (_mainWindowVM.SelectedReports?.Id == orgId)
            _mainWindowVM.SelectedReports = null;

        var mainWindow = Desktop.MainWindow as MainWindow;
        var mainWindowVM = mainWindow?.DataContext as MainWindowVM ?? _mainWindowVM;
        OrganizationListRefresh.AfterOrgStructureChanged(mainWindowVM);
        mainWindowVM.UpdateReportsCollection();
    }

    private static void RemoveOrgFromLocalStorage(int orgId)
    {
        var local = ReportsStorage.LocalReports;
        if (local?.Reports_Collection is null)
            return;

        foreach (var item in local.Reports_Collection.OfType<Reports>().Where(r => r.Id == orgId).ToList())
            local.Reports_Collection.Remove(item);
    }

    private static void LogStep(int orgId, string formNum, string step) =>
        ServiceExtension.LoggerManager.Info(
            $"Удаление организации Id={orgId} FormNum={formNum}: {step}.",
            ErrorCodeLogger.Application);
}
