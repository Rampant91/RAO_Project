using Avalonia.Controls;
using Client_App.Resources;
using Client_App.Services;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views.Forms.Forms4;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранной формы.
/// </summary>
/// <param name="formParam">Содержит коллекцию отчётов Parameter (костыль, оттуда мы всегда берём только один отчёт)
/// и окно Window, которое нужно закрыть.</param>
public class ChangeFormAsyncCommand(FormParameter? formParam = null) : BaseAsyncCommand
{
    #region AsyncExecute
    
    /// <summary>
    /// Используется при вызове из других команд
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter != null)
        {
            // Открытие с главного: parameter = Report (через ICommand.Execute / async void).
            Execute(parameter);
            return;
        }

        if (formParam?.Window != null && formParam.Parameter is Report reportToOpen)
        {
            await FormReportWindowNavigator.ReplaceAsync(formParam.Window, reportToOpen).ConfigureAwait(true);
            return;
        }

        if (formParam?.Window != null)
            await Execute(formParam.Window);
    }

    #endregion

    #region Execute

    public override async void Execute(object? parameter)
    {
        await OpenReport(parameter);
    }

    private Task Execute(Window? window = null)
    {
        if (window is null) return Task.CompletedTask;

        window.Closed += WindowClosed;
        window.Close();

        return Task.CompletedTask;
    }

    #endregion

    #region OpenReport

    private static async Task OpenReport(object? parameter)
    {
        if (parameter is not Report report) return;

        if (await ReportExportLock.TryBlockReportAccessAsync(report.Id))
        {
            return;
        }

        if (report.Reports is null)
        {
            var orgId = await StaticConfiguration.DBModel.ReportCollectionDbSet
                .AsNoTracking()
                .Where(r => r.Id == report.Id)
                .Select(r => r.Reports != null ? r.Reports.Id : 0)
                .FirstOrDefaultAsync();
            if (orgId != 0)
            {
                report.Reports = ReportsStorage.LocalReports.Reports_Collection
                                    .FirstOrDefault(r => r.Id == orgId)
                                ?? await StaticConfiguration.DBModel.ReportsCollectionDbSet
                                    .AsNoTracking()
                                    .Include(x => x.Master_DB).ThenInclude(m => m.Rows10)
                                    .Include(x => x.Master_DB).ThenInclude(m => m.Rows20)
                                    .FirstOrDefaultAsync(x => x.Id == orgId);
            }
        }

        if (report.Reports != null)
            OrgMatchQuery.EnsureTitleRowsLoaded(report.Reports);

        var db = StaticConfiguration.DBModel;
        var modifiedEntities = db.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged);

        var mainWindow = Desktop.MainWindow as MainWindow;
        //var tre = ReportsStorage.LocalReports.Reports_Collection
        //    .FirstOrDefault(i => i.Report_Collection.Contains(report));
        var numForm = report.FormNum.Value;

        mainWindow?.SetReportOpeningOverlay(true);
        ChangeOrCreateVM changeOrCreateVM;
        try
        {
            changeOrCreateVM = new ChangeOrCreateVM(numForm, report);

            // Catch-up snapshot after full load (legacy FormChangeOrCreate / form 2).
            if (changeOrCreateVM.Storage is not null)
            {
                await ReportExportSnapshotService.EnsureSnapshotOnOpenAsync(changeOrCreateVM.Storage);
            }

            await Form2NewInterfaceOpener.PrepareSumRowsAsync(changeOrCreateVM, numForm);
        }
        catch
        {
            mainWindow?.SetReportOpeningOverlay(false);
            throw;
        }

        switch (numForm)
        {
            case "1.1":
                {
                    var form11VM = new Form_11VM(changeOrCreateVM.Storage);
                    var form11Window = new Form_11(form11VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await form11Window.ShowDialog(mainWindow);
                    break;
                }
            case "1.2":
                {
                    var form12VM = new Form_12VM(changeOrCreateVM.Storage);
                    var window = new Form_12(form12VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.3":
                {
                    var form13VM = new Form_13VM(changeOrCreateVM.Storage);
                    var window = new Form_13(form13VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.4":
                {
                    var form14VM = new Form_14VM(changeOrCreateVM.Storage);
                    var window = new Form_14(form14VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.5":
                {
                    var form15VM = new Form_15VM(changeOrCreateVM.Storage);
                    var window = new Form_15(form15VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.6":
                {
                    var form16VM = new Form_16VM(changeOrCreateVM.Storage);
                    var window = new Form_16(form16VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.7":
                {
                    var form17VM = new Form_17VM(changeOrCreateVM.Storage);
                    var window = new Form_17(form17VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.8":
                {
                    var form18VM = new Form_18VM(changeOrCreateVM.Storage);
                    var window = new Form_18(form18VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "1.9":
                {
                    var form19VM = new Form_19VM(changeOrCreateVM.Storage);
                    var window = new Form_19(form19VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "4.1":
                {
                    var form41VM = new Form_41VM(changeOrCreateVM.Storage);
                    var window = new Form_41(form41VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
            case "2.1" or "2.2" or "2.3" or "2.4" or "2.5" or "2.6" or "2.7" or "2.8" or "2.9" or "2.10" or "2.11" or "2.12"
                when Form2InterfaceFlags.UseNewInterface:
                {
                    await Form2NewInterfaceOpener.ShowDialogAsync(numForm, changeOrCreateVM.Storage, mainWindow);
                    break;
                }
            default:
                {
                    await MainWindowVM.ShowDialog.Handle(changeOrCreateVM);
                    break;
                }
        }

    }

    #endregion

    #region Events

    private async void WindowClosed(object? sender, System.EventArgs e)
    {
        if (formParam == null) return;
        await OpenReport(formParam.Parameter).ConfigureAwait(false);
    }

    #endregion
}