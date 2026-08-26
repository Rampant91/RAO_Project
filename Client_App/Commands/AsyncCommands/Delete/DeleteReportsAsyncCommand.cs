using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using System;
using System.Collections;
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

        if (await ReportExportLock.TryBlockOrganizationAccessAsync(orgShell.Id))
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgShell.Id}: доступ заблокирован (экспорт/блокировка).",
                ErrorCodeLogger.Application);
            return;
        }

        #region MessageDeleteReports

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
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
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да")
            return;

        var orgId = orgShell.Id;
        try
        {
            await DeleteOrganizationByIdAsync(orgId).ConfigureAwait(true);
        }
        catch (Exception ex)
        {
            var msg = $"Удаление организации Id={orgId} не выполнено." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }
    }

    private async Task DeleteOrganizationByIdAsync(int orgId)
    {
        var db = StaticConfiguration.DBModel;

        var trackedOrg = await db.ReportsCollectionDbSet
            .FirstOrDefaultAsync(r => r.Id == orgId)
            .ConfigureAwait(true);

        if (trackedOrg is null)
        {
            ServiceExtension.LoggerManager.Warning(
                $"Удаление организации Id={orgId}: запись не найдена в БД.",
                ErrorCodeLogger.DataBase);
            InvalidateAndRefreshUi(orgId);
            return;
        }

        var masterId = trackedOrg.Master_DBId;
        var reportIds = await OrgReportsQuery.GetReportIdsAsync(db, orgId).ConfigureAwait(true);

        foreach (var batch in FirebirdInClause.Chunk(reportIds))
        {
            var toRemove = await db.ReportCollectionDbSet
                .Where(r => batch.Contains(r.Id))
                .ToListAsync()
                .ConfigureAwait(true);
            db.ReportCollectionDbSet.RemoveRange(toRemove);
        }

        // Титул (Master) может не входить в Report_Collection org — удаляем отдельно по Id.
        if (masterId is > 0)
        {
            var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == masterId.Value)
                         ?? await db.ReportCollectionDbSet
                             .FirstOrDefaultAsync(r => r.Id == masterId.Value)
                             .ConfigureAwait(true);
            if (master is not null)
                db.ReportCollectionDbSet.Remove(master);

            trackedOrg.Master_DBId = null;
            trackedOrg.Master_DB = null!;
        }

        db.ReportsCollectionDbSet.Remove(trackedOrg);
        await db.SaveChangesAsync().ConfigureAwait(true);

        RemoveOrgFromLocalStorage(orgId);
        await ProcessDataBaseFillEmpty(db).ConfigureAwait(true);
        InvalidateAndRefreshUi(orgId);
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

    private static async Task ProcessDataBaseFillEmpty(DataContext dbm)
    {
        if (!dbm.DBObservableDbSet.Any())
            dbm.DBObservableDbSet.Add(new DBObservable());

        foreach (var item in dbm.DBObservableDbSet)
        {
            foreach (var key in item.Reports_Collection)
            {
                var it = (Reports)key;
                if (it.Master_DB is null) continue;
                if (it.Master_DB.FormNum_DB == "") continue;
                if (it.Master_DB.Rows10.Count == 0)
                {
                    var ty1 = (Form10)FormCreator.Create("1.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create("1.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows10.Add(ty1);
                    it.Master_DB.Rows10.Add(ty2);
                }

                if (it.Master_DB.Rows20.Count == 0)
                {
                    var ty1 = (Form20)FormCreator.Create("2.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create("2.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows20.Add(ty1);
                    it.Master_DB.Rows20.Add(ty2);
                }

                it.Master_DB.Rows10.Sorted = false;
                it.Master_DB.Rows20.Sorted = false;
                await it.Master_DB.Rows10.QuickSortAsync();
                await it.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }
}
