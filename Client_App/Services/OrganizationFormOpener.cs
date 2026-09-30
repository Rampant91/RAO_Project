using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Forms.Forms5;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms5;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Services;

/// <summary>
/// Opens organization edit windows (forms 1.0, 2.0, 4.0, 5.0) with Avalonia 11-safe error handling.
/// </summary>
public static class OrganizationFormOpener
{
    public static async Task TryOpenAsync(
        MainWindow mainWindow,
        MainWindowVM mainWindowVM,
        Reports selectedReports)
    {
        try
        {
            await OpenCoreAsync(mainWindow, mainWindowVM, selectedReports);
        }
        catch (Exception ex)
        {
            var msg = $"Failed to open organization form (Reports.Id={selectedReports?.Id})." +
                      $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.Application);

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Открытие организации",
                    ContentHeader = "Ошибка",
                    ContentMessage = ex.Message,
                    MinWidth = 420,
                    MinHeight = 140,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(mainWindow));
        }
    }

    private static async Task OpenCoreAsync(
        MainWindow mainWindow,
        MainWindowVM mainWindowVM,
        Reports selectedReports)
    {
        if (selectedReports is null)
            throw new InvalidOperationException("Organization is not selected.");

        var stubMaster = selectedReports.Master_DB ?? selectedReports.Master;
        if (stubMaster is null)
            throw new InvalidOperationException("Organization master report is missing.");

        var formNum = ResolveFormNum(stubMaster);
        var refreshOrgListAfterTitle = false;

        switch (formNum)
        {
            case "1.0":
            {
                var master = await EnsureTrackedMasterWithForm10Async(stubMaster).ConfigureAwait(true);
                var titleBefore = SnapshotForm10Title(master);
                var form10VM = new Form_10VM(formNum, master)
                {
                    IsSeparateDivision = IsSeparateDivisionForm10(master)
                };
                var window = new Form_10(form10VM);
                window.PrepareBeforeShow(mainWindow);
                await window.ShowDialog(mainWindow);

                var titleAfter = SnapshotForm10Title(master);
                if (titleBefore != titleAfter)
                {
                    MainWindowListQuery.UpsertOrgKeyForm10FromMaster(selectedReports.Id, master);
                    mainWindowVM.Forms1TabControlVM.RefreshOrgListAfterTitleChange();
                }

                refreshOrgListAfterTitle = true;
                break;
            }
            case "2.0":
            {
                var master = await EnsureTrackedMasterWithForm20Async(stubMaster).ConfigureAwait(true);
                var titleBefore = SnapshotForm20Title(master);
                var form20VM = new Form_20VM(formNum, master)
                {
                    IsSeparateDivision = IsSeparateDivisionForm20(master)
                };
                var window = new Form_20(form20VM);
                window.PrepareBeforeShow(mainWindow);
                await window.ShowDialog(mainWindow);

                var titleAfter = SnapshotForm20Title(master);
                if (titleBefore != titleAfter)
                {
                    MainWindowListQuery.UpsertOrgKeyForm20FromMaster(selectedReports.Id, master);
                    mainWindowVM.Forms2TabControlVM.RefreshOrgListAfterTitleChange();
                }

                refreshOrgListAfterTitle = true;
                break;
            }
            case "4.0":
            {
                var form40VM = new Form_40VM(formNum, stubMaster);
                var window = new Form_40(form40VM);
                window.PrepareBeforeShow(mainWindow);
                await window.ShowDialog(mainWindow);
                break;
            }
            case "5.0":
            {
                var form50VM = new Form_50VM(formNum, stubMaster);
                var window = new Form_50(form50VM);
                window.PrepareBeforeShow(mainWindow);
                await window.ShowDialog(mainWindow);
                break;
            }
            default:
                throw new InvalidOperationException($"Unsupported organization form: '{formNum}'.");
        }

        if (!refreshOrgListAfterTitle)
            mainWindowVM.UpdateReportsCollection();
    }

    private static string ResolveFormNum(Report master)
    {
        if (!string.IsNullOrWhiteSpace(master.FormNum_DB))
            return master.FormNum_DB.Trim();

        var fromRam = master.FormNum?.Value?.Trim();
        if (!string.IsNullOrWhiteSpace(fromRam))
            return fromRam!;

        throw new InvalidOperationException("Organization form number is empty.");
    }

    private static bool IsSeparateDivisionForm10(Report master)
    {
        var row = master.Rows10
            .OrderBy(r => r.NumberInOrder_DB)
            .ElementAtOrDefault(1);
        var okpo = row?.Okpo_DB ?? row?.Okpo?.Value;
        return !string.IsNullOrWhiteSpace(okpo);
    }

    private static bool IsSeparateDivisionForm20(Report master)
    {
        var row = master.Rows20
            .OrderBy(r => r.NumberInOrder_DB)
            .ElementAtOrDefault(1);
        var okpo = row?.Okpo_DB ?? row?.Okpo?.Value;
        return !string.IsNullOrWhiteSpace(okpo);
    }

    private static Form10TitleSelector.TitleFields SnapshotForm20Title(Report master)
    {
        var rows = master.Rows20.OrderBy(r => r.NumberInOrder_DB).ToList();
        var r0 = rows.ElementAtOrDefault(0);
        var r1 = rows.ElementAtOrDefault(1);
        return Form10TitleSelector.Pick(
            r0?.RegNo_DB, r0?.Okpo_DB, r0?.ShortJurLico_DB,
            r1?.RegNo_DB, r1?.Okpo_DB, r1?.ShortJurLico_DB);
    }

    private static Form10TitleSelector.TitleFields SnapshotForm10Title(Report master)
    {
        var rows = master.Rows10.OrderBy(r => r.NumberInOrder_DB).ToList();
        var r0 = rows.ElementAtOrDefault(0);
        var r1 = rows.ElementAtOrDefault(1);
        return Form10TitleSelector.Pick(
            r0?.RegNo_DB, r0?.Okpo_DB, r0?.ShortJurLico_DB,
            r1?.RegNo_DB, r1?.Okpo_DB, r1?.ShortJurLico_DB);
    }

    /// <summary>
    /// Master + Rows10 из основного DBModel (tracked), иначе Save/HasChanges не видят правки титула.
    /// </summary>
    private static async Task<Report> EnsureTrackedMasterWithForm10Async(Report stubOrTracked)
    {
        var db = StaticConfiguration.DBModel;
        if (stubOrTracked.Id <= 0)
            throw new InvalidOperationException("Organization master Id is missing.");

        var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == stubOrTracked.Id);
        if (master is null)
        {
            master = await db.ReportCollectionDbSet
                .Include(r => r.Rows10)
                .FirstOrDefaultAsync(r => r.Id == stubOrTracked.Id)
                .ConfigureAwait(true);
        }
        else if (master.Rows10.Count < 2)
        {
            await db.Entry(master).Collection(r => r.Rows10).LoadAsync().ConfigureAwait(true);
        }

        if (master is null)
            throw new InvalidOperationException($"Organization master Id={stubOrTracked.Id} not found in DB.");

        EnsureForm10RowsTracked(db, master);
        EnsureTwoForm10Rows(master);
        return master;
    }

    private static async Task<Report> EnsureTrackedMasterWithForm20Async(Report stubOrTracked)
    {
        var db = StaticConfiguration.DBModel;
        if (stubOrTracked.Id <= 0)
            throw new InvalidOperationException("Organization master Id is missing.");

        var master = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == stubOrTracked.Id);
        if (master is null)
        {
            master = await db.ReportCollectionDbSet
                .Include(r => r.Rows20)
                .FirstOrDefaultAsync(r => r.Id == stubOrTracked.Id)
                .ConfigureAwait(true);
        }
        else if (master.Rows20.Count < 2)
        {
            await db.Entry(master).Collection(r => r.Rows20).LoadAsync().ConfigureAwait(true);
        }

        if (master is null)
            throw new InvalidOperationException($"Organization master Id={stubOrTracked.Id} not found in DB.");

        EnsureForm20RowsTracked(db, master);
        EnsureTwoForm20Rows(master);
        return master;
    }

    private static void EnsureForm10RowsTracked(DBModel db, Report master)
    {
        foreach (var row in master.Rows10.OfType<Form10>())
        {
            if (row.Id == 0)
                continue;
            if (db.Entry(row).State == EntityState.Detached)
                db.form_10.Attach(row);
        }
    }

    private static void EnsureForm20RowsTracked(DBModel db, Report master)
    {
        foreach (var row in master.Rows20.OfType<Form20>())
        {
            if (row.Id == 0)
                continue;
            if (db.Entry(row).State == EntityState.Detached)
                db.form_20.Attach(row);
        }
    }

    private static void EnsureTwoForm10Rows(Report master)
    {
        while (master.Rows10.Count < 2)
        {
            var empty = (Form10)FormCreator.Create("1.0");
            empty.NumberInOrder_DB = (short)(master.Rows10.Count + 1);
            empty.ReportId = master.Id;
            master.Rows10.Add(empty);
        }
    }

    private static void EnsureTwoForm20Rows(Report master)
    {
        while (master.Rows20.Count < 2)
        {
            var empty = (Form20)FormCreator.Create("2.0");
            empty.NumberInOrder_DB = (short)(master.Rows20.Count + 1);
            empty.ReportId = master.Id;
            master.Rows20.Add(empty);
        }
    }
}
